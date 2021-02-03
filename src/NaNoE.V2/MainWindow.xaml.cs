using Microsoft.Win32;
using NaNoE.V2.Data;
using NaNoE.V2.Windows;
using NaNoE.V2.Windows.Popups;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace NaNoE.V2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        /// <summary>
        /// Static style references
        /// </summary>
        public static Style SPara { get; private set; }
        public static Style SChap { get; private set; }
        public static Style SBook { get; private set; }
        public static Style SNote { get; private set; }

        public static Style SDarkBackground { get; private set; }
        public static Style SLightBackground { get; private set; }

        /// <summary>
        /// Static Main Window reference
        /// </summary>
        private static MainWindow _instance;
        internal static MainWindow Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Initiate the Main Window
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            this.FindResource("vmLocator");

            // Initiate the Edit Processor for its static reference
            new EditProcessor();

            // Initiate the navigator and go to the first view
            new Navigator(this);
            Navigator.Instance.GoTo("start");

            SPara = (Style)this.FindResource("styParagraph");
            SChap = (Style)this.FindResource("styChapter");
            SBook = (Style)this.FindResource("styBookmark");
            SNote = (Style)this.FindResource("styNote");
            SDarkBackground = (Style)this.FindResource("styDark");
            SLightBackground = (Style)this.FindResource("styLight");

            if (File.Exists("style.opt"))
            {
                using (var f = File.OpenRead("style.opt"))
                {
                    using (var r = new StreamReader(f))
                    {
                        SStyle = r.ReadLine();
                    }
                }
            }

            RefreshStyle();

            lstNotes.ItemsSource = DataConnection.Instance.Helpers;
            lstNotes.Items.Refresh();

            _instance = this;
        }

        /// <summary>
        /// Style option set
        /// </summary>
        private string _sStyle = "Light";
        public string SStyle
        {
            get => _sStyle;
            set
            {
                _sStyle = value;
                var args = new PropertyChangedEventArgs("SStyle");
                PropertyChanged?.Invoke(this, args);

                try
                {
                    this.RefreshStyle();
                    Navigator.Instance.GoTo(Navigator.Instance.CurrentView);
                }
                catch { }
            }
        }

        /// <summary>
        /// Refresh the style of the view
        /// </summary>
        private void RefreshStyle()
        {
            switch (_sStyle)
            {
                case "Light":
                    grdMainWindowGrid.Style = SLightBackground;
                    break;
                default:
                    grdMainWindowGrid.Style = SDarkBackground;
                    break;
            }
        }

        /// <summary>
        /// Create new novel menu button function
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">Event args</param>
        private void NewNovelClick(object sender, RoutedEventArgs e)
        {
            if (DataConnection.Instance.Connected) DataConnection.Instance.Close();

            var sfd = new SaveFileDialog();
            sfd.FileName = "novel";
            sfd.DefaultExt = ".ndb";
            sfd.Filter = "NDB (.ndb)|*.ndb";
            Nullable<bool> answer = sfd.ShowDialog();

            if (answer == true)
            {
                if (!File.Exists(sfd.FileName))
                {
                    DataConnection.Instance.Create(sfd.FileName.ToString());
                    Navigator.Instance.GoTo("novel");

                    RefreshHelperAction?.Invoke();
                }
                else
                {
                    MessageBox.Show("File already exists...", "You can't choose that name");
                }
            }
        }

        /// <summary>
        /// Open an existing novel menu item function
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">Event args</param>
        private void OpenNovelClick(object sender, RoutedEventArgs e)
        {
            if (DataConnection.Instance.Connected) DataConnection.Instance.Close();

            var ofd = new OpenFileDialog();
            ofd.DefaultExt = ".ndb";
            ofd.Filter = "NDB (.ndb)|*.ndb";
            Nullable<bool> answer = ofd.ShowDialog();

            if (answer == true)
            {
                if (File.Exists(ofd.FileName))
                {
                    if (File.Exists("last")) File.Delete("last");
                    var last = File.OpenWrite("last");
                    using (var ow = new StreamWriter(last))
                    {
                        ow.Write(ofd.FileName);
                    }
                    last.Close();

                    DataConnection.Instance.Open(ofd.FileName.ToString());
                    Navigator.Instance.GoTo("novel");

                    RefreshHelperAction?.Invoke();
                }
                else
                {
                    MessageBox.Show("File doesn't exist...", "That file doesn't exist");
                }
            }
        }

        /// <summary>
        /// Show the Readme view
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">Event args</param>
        private void ShowReadmeClick(object sender, RoutedEventArgs e)
        {
            Navigator.Instance.GoTo("readme");
        }

        private void GetCountsClick(object sender, RoutedEventArgs e)
        {
            if (!DataConnection.Instance.Connected)
            {
                MessageBox.Show("We can't count words if we've not opened a novel, sorry.");
            }
            else
            {
                RepetitionWindow rep = new RepetitionWindow();
                rep.ShowDialog();
            }
        }

        /// <summary>
        /// Minimise or maximise the edit view helper
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">Event args</param>
        private void ShowSide(object sender, RoutedEventArgs e)
        {
            if (grdView.ColumnDefinitions[2].Width.Value == 0)
            {
                colView.Width = new GridLength(200, GridUnitType.Pixel);
                butOpen.Content = "<";
            }
            else
            {
                colView.Width = new GridLength(0, GridUnitType.Pixel);
                butOpen.Content = ">";
            }

        }

        /// <summary>
        /// When the window is loaded, i.e. has all the controls
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">Event args</param>
        private void MainWindowLoaded(object sender, RoutedEventArgs e)
        {
            Navigator.Instance.GoTo("start");
        }

        /// <summary>
        /// View main Config menu item function
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">Event args</param>
        private void ViewConfigClick(object sender, RoutedEventArgs e)
        {
            ViewSettingsWindow window = new ViewSettingsWindow();
            window.Show();
        }

        /// <summary>
        /// View edit config menu item function
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">Event args</param>
        private void ViewEditConfigClick(object sender, RoutedEventArgs e)
        {
            EditSettingsWindow window = new EditSettingsWindow();
            window.Show();
        }

        /// <summary>
        /// Open Import DocX novel menu item function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImportWordDocumentClick(object sender, RoutedEventArgs e)
        {
            Window w = new ImportWordDocumentWindow();
            w.ShowDialog();
        }

        /// <summary>
        /// Export novel menu item function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExportNovelClick(object sender, RoutedEventArgs e)
        {
            if (!DataConnection.Instance.Connected)
            {
                MessageBox.Show("Oh dear, you haven't opened a novel, so you can export it as a Word DocX.", "Error");
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Word Document (.docx)|.docx";
            sfd.FilterIndex = 2;
            sfd.RestoreDirectory = true;

            var ans = sfd.ShowDialog();

            if (null == ans) return;

            if (ans == true)
            {
                MessageBox.Show("Please note, this could be slow depending on your computer. Please, be patient till the next message.", "Note");

                using (var doc = DocX.Create(sfd.FileName))
                {
                    Formatting _format_head = new Formatting();
                    _format_head.Bold = true;
                    _format_head.Size = 72;

                    Formatting _format_tail = new Formatting();
                    _format_tail.Bold = false;
                    _format_tail.Size = 11;

                    int chCount = 0;

                    var paragraphs = DataConnection.Instance.ExportGetNovel();
                    for (int i = 0; i < paragraphs.Count; ++i)
                    {
                        switch (paragraphs[i].CType)
                        {
                            case ControlType.Paragraph:
                                doc.InsertParagraph(paragraphs[i].Data, false, _format_tail);
                                break;
                            case ControlType.Chapter:
                                ++chCount;
                                doc.InsertParagraph(paragraphs[i].Data + " " + chCount, false, _format_head);
                                break;
                        }
                    }

                    doc.Save();
                }

                MessageBox.Show("The DocX export of the novel is complete!", "Done");
            }
        }

        /// <summary>
        /// Export raw novel menu function
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">Event args</param>
        private void ExportRawClick(object sender, RoutedEventArgs e)
        {
            if (!DataConnection.Instance.Connected)
            {
                MessageBox.Show("Oh dear, you haven't opened a novel, so you can export it as a Word DocX.", "Error");
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Word Document (.docx)|.docx";
            sfd.FilterIndex = 2;
            sfd.RestoreDirectory = true;

            var ans = sfd.ShowDialog();

            if (null == ans) return;

            if (ans == true)
            {
                MessageBox.Show("Please note, this could be slow depending on your computer. Please, be patient till the next message.", "Note");

                using (var doc = DocX.Create(sfd.FileName))
                {
                    Formatting _format_head = new Formatting();
                    _format_head.Bold = true;
                    _format_head.Size = 72;

                    Formatting _format_tail = new Formatting();
                    _format_tail.Bold = false;
                    _format_tail.Size = 11;

                    Formatting _format_note = new Formatting();
                    _format_note.Bold = false;
                    _format_note.Size = 9;

                    Formatting _format_bookmark = new Formatting();
                    _format_bookmark.Bold = true;
                    _format_bookmark.Size = 35;

                    int chCount = 0;

                    var paragraphs = DataConnection.Instance.ExportGetRaw();
                    for (int i = 0; i < paragraphs.Count; ++i)
                    {
                        switch (paragraphs[i].CType)
                        {
                            case ControlType.Paragraph:
                                doc.InsertParagraph(paragraphs[i].Data, false, _format_tail);
                                break;
                            case ControlType.Chapter:
                                ++chCount;
                                doc.InsertParagraph(paragraphs[i].Data + " " + chCount, false, _format_head);
                                break;
                            case ControlType.Note:
                                doc.InsertParagraph("[note] " + paragraphs[i].Data, false, _format_note);
                                break;
                            case ControlType.Bookmark:
                                doc.InsertParagraph("[bookmark] " + paragraphs[i].Data, false, _format_bookmark);
                                break;
                        }
                    }

                    doc.Save();
                }

                MessageBox.Show("The Raw DocX export of the novel is complete!", "Done");
            }
        }

        /// <summary>
        /// The list used to show suggested edits
        /// </summary>
        private ListBox _listSuggestions;
        private ListBox _lstHelpersAdditional;
        private ListBox _lstHelpersItemsAdditional;

        public event PropertyChangedEventHandler PropertyChanged;

        public ListBox ListSuggestions
        {
            get { return _listSuggestions; }
        }

        /// <summary>
        /// Make the reference to the list when it is loaded
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">Event args</param>
        private void lstSuggestions_Loaded(object sender, RoutedEventArgs e)
        {
            _listSuggestions = lstSuggestions;
        }

        /// <summary>
        /// Show additional information for edit suggestions on double click
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">Event args</param>
        private void lstSuggestions_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (null != lstSuggestions.SelectedItem)
            {
                var line = (string)lstSuggestions.SelectedItem;
                if (line.Contains('}') || line.Contains("Possible Repetition: ")) return;

                var splt = line.Split(':');
                if (splt[0].Contains("Spell Check")) return;

                line = splt[1].TrimStart(' ');
                var opt = (from item in EditProcessor.Instance.EditOptions
                           where item.Detail == line
                           select item).First();
                var msg = opt.Message.Replace("|", "\r\n\r\n");
                MessageBox.Show(msg, opt.Detail);
            }
        }

        /// <summary>
        /// Open last novel menu item function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenLastClick(object sender, RoutedEventArgs e)
        {
            if (!File.Exists("last"))
            {
                MessageBox.Show("Oh dear, we don't seem to have record of the last file opened, sorry...", "Oops...");
            }
            else
            {
                string file = "";
                var last = File.OpenRead("last");
                using (var ore = new StreamReader(last))
                {
                    file = ore.ReadLine();
                }
                last.Close();

                if (File.Exists(file))
                {
                    try
                    {
                        DataConnection.Instance.Open(file);
                        Navigator.Instance.GoTo("novel");

                        RefreshHelperAction?.Invoke();
                    }
                    catch
                    {
                        MessageBox.Show("Oh, uh, you tried to do something that shouldn't ever even be possible.", "Oops...");
                    }
                }
                else
                {
                    MessageBox.Show("Oh dear, it would seem the last file we opened isn't there anymore, sorry...", "Oops...");
                }
            }
        }

        /// <summary>
        /// Export all data as raw data menu item function
        ///  - primarily for bug testing and to watch all the data
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">Event args</param>
        private void DebugAllDataToTXT(object sender, RoutedEventArgs e)
        {
            if (!DataConnection.Instance.Connected)
            {
                MessageBox.Show("Oh dear, you haven't opened a novel, so you can't export it as debug data.", "Error");
                return;
            }

            MessageBox.Show("Please note, this if for debug and testing purposes.\n\nIt is also here for in case any data, or use of data, is accidentally incorrect. The design shouldn't have problems, despite it being strange, just note that you can retrieve all stored data with this in case you lose something for no reason.\n\nAlso note, this doesn't bring back 'deleted' elements in the NaNoE.V2 data type '.nne'.", "Note");

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Word Document (.txt)|.txt";
            sfd.FilterIndex = 2;
            sfd.RestoreDirectory = true;

            var ans = sfd.ShowDialog();

            if (null == ans) return;

            if (ans == true)
            {
                var raw = DataConnection.Instance.GetRaw();
                var file = File.OpenWrite(sfd.FileName);
                using (var writer = new StreamWriter(file))
                {
                    foreach (var line in raw)
                    {
                        writer.WriteLine(line);
                    }
                }
                file.Close();
            }
        }

        /// <summary>
        /// In edit view we move the cursor to the position through right click
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event args</param>
        private void lstSuggestions_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Navigator.Instance.CurrentView == "edit")
            {
                if (null != ViewModelLocator.Instance.EditVM.txtContent)
                {
                    if (null != lstSuggestions.SelectedItem)
                    {
                        var line = ViewModelLocator.Instance.EditVM.txtContent.Text;

                        if (lstSuggestions.SelectedItem.ToString().Contains("Possible Repetition: "))
                        {
                            var selected = lstSuggestions.SelectedItem.ToString().Split(']')[1].Trim(' ');
                            var pos = line.IndexOf(selected);
                            ViewModelLocator.Instance.EditVM.txtContent.CaretIndex = pos;
                            ViewModelLocator.Instance.EditVM.txtContent.Focus();
                            return;
                        }

                        var splt = line.Split(' ');

                        if (lstSuggestions.SelectedItem.ToString().Contains(']'))
                        {
                            var selected = lstSuggestions.SelectedItem.ToString().Split(']');
                            var num = int.Parse(selected[0]);

                            int i = 0;
                            for (int j = 0; j < num - 1; ++j)
                            {
                                i += 1 + splt[j].Length;
                            }

                            ViewModelLocator.Instance.EditVM.txtContent.CaretIndex = i;
                            ViewModelLocator.Instance.EditVM.txtContent.Focus();
                        }
                        else if (lstSuggestions.SelectedItem.ToString().Contains('}'))
                        {
                            var selected = lstSuggestions.SelectedItem.ToString().Split('}');
                            var num = int.Parse(selected[0]);

                            ViewModelLocator.Instance.EditVM.txtContent.CaretIndex = num;
                            ViewModelLocator.Instance.EditVM.txtContent.Focus();
                        }
                    }
                }
            }
        }

        private void ImportHelpers_Click(object sender, RoutedEventArgs e)
        {
            if (!DataConnection.Instance.Connected)
            {
                MessageBox.Show("You need to have an open 'ndb' novel to import Helpers.");
            }
            else if (DataConnection.Instance.Helpers.Count > 0)
            {
                MessageBox.Show("Not implemented yet: helpers needs blank currently.");
            }
            else
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "NDB File|*.ndb";
                var answer = ofd.ShowDialog();
                if (answer == true)
                {
                    using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + ofd.FileName + "; Version=3;"))
                    {
                        conn.Open();
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = "SELECT id, name FROM helpers";
                            var inAns = cmd.ExecuteReader();
                            if (inAns.HasRows)
                            {
                                while (inAns.Read())
                                {
                                    int in_id = inAns.GetInt32(0);
                                    string in_name = inAns.GetString(1);

                                    string helper = "";
                                    char q = '[';
                                    while (q != ' ')
                                    {
                                        q = in_name[0];
                                        in_name = in_name.Substring(1);

                                        if (q != ' ') helper += q;
                                    }

                                    DataConnection.Instance.ImportHelper(in_name, helper);
                                    var lastHelper = DataConnection.Instance.GetMaxHelperID();

                                    var cmd2 = conn.CreateCommand();
                                    cmd2.CommandText = "SELECT data FROM helperitems WHERE helperid = " + in_id + ";";
                                    var ans2 = cmd2.ExecuteReader();
                                    if (ans2.HasRows)
                                    {
                                        while (ans2.Read())
                                        {
                                            DataConnection.Instance.InsertHelperNote(lastHelper, ans2.GetString(0));
                                        }
                                    }
                                }
                            }
                        }
                        conn.Close();
                    }
                }
            }
        }

        private void butAddChapter_Click(object sender, RoutedEventArgs e)
        {
            if (!DataConnection.Instance.Connected)
            {
                MessageBox.Show("Can't add a chapter helper without a 'ndb' open.");
            }
            else
            {
                AddChapterHelperWindow window = new AddChapterHelperWindow();
                window.ShowDialog();

                lstNotes.SelectedItem = null;
                lstNotes.Items.Refresh();
            }
        }

        private void butAddItem_Click(object sender, RoutedEventArgs e)
        {
            if (!DataConnection.Instance.Connected)
            {
                MessageBox.Show("Can't add an item helper without a 'ndb' open.");
            }
            else
            {
                AddItemWindow window = new AddItemWindow();
                window.ShowDialog();

                lstNotes.SelectedItem = null;
                lstNotes.Items.Refresh();
            }
        }

        public List<NHelper> Helpers
        {
            get => DataConnection.Instance.Helpers;
        }
        public Action RefreshHelperAction { get; private set; }

        private void lstNotes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (null != lstNotes.SelectedItem)
            {
                var item = (NHelper)lstNotes.SelectedItem;
                lstInnerNotes.ItemsSource = item.Items;
                lstInnerNotes.Items.Refresh();

                item.RefreshNotes();
                lstInnerNotes.ItemsSource = item.Items;
                lstInnerNotes.Items.Refresh();
            }
            else
            {
                lstInnerNotes.ItemsSource = new List<string>();
            }
            lstNotes.Items.Refresh();
        }

        private void butAddItemNote_Click(object sender, RoutedEventArgs e)
        {
            if (null == lstNotes.SelectedItem)
            {
                MessageBox.Show("Please select a Helper to add a note to.");
            }
            else
            {
                AddNoteWindow window = new AddNoteWindow((NHelper)(lstNotes.SelectedItem));
                window.ShowDialog();
                lstInnerNotes.Items.Refresh();
            }
        }

        private void lstInnerNotes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (null != lstInnerNotes.SelectedItem)
            {
                MessageBox.Show(lstInnerNotes.SelectedItem.ToString());
            }
        }

        private void lstNotes_Loaded(object sender, RoutedEventArgs e)
        {
            _lstHelpersAdditional = lstNotes;
            this.RefreshHelperAction += () =>
            {
                _lstHelpersAdditional.Items.Refresh();
                _lstHelpersItemsAdditional.Items.Refresh();
            };
        }

        private void lstInnerNotes_Loaded(object sender, RoutedEventArgs e)
        {
            _lstHelpersItemsAdditional = lstInnerNotes;
        }

        private void butAddChar_Click(object sender, RoutedEventArgs e)
        {
            if (!DataConnection.Instance.Connected)
            {
                MessageBox.Show("Can't add a character helper without a 'ndb' open.");
            }
            else
            {
                AddCharacterWindow window = new AddCharacterWindow();
                window.ShowDialog();

                lstNotes.SelectedItem = null;
                lstNotes.Items.Refresh();
            }
        }

        private void butDelSet_Click(object sender, RoutedEventArgs e)
        {
            if (!DataConnection.Instance.Connected)
            {
                MessageBox.Show("Can't delete helpers from the edge of the universe... Sorry, please, load a 'ndb'.");
            }
            else if (null == lstNotes.SelectedItem)
            {
                MessageBox.Show("Please select one you would like to delete.");
            }
            else
            {
                var answer = MessageBox.Show("Are you sure you would like to delete this category?", "Warning!", MessageBoxButton.YesNo);
                if (answer == MessageBoxResult.Yes)
                {
                    DataConnection.Instance.DeleteHelper(((NHelper)lstNotes.SelectedItem).ID);
                    lstNotes.SelectedItem = null;
                    lstNotes.Items.Refresh();
                }
            }
        }

        private void butDeleteItemNote_Click(object sender, RoutedEventArgs e)
        {
            if (!DataConnection.Instance.Connected)
            {
                MessageBox.Show("Please load an ndb before you can delete note items.");
            }
            else if (null == lstInnerNotes.SelectedItem)
            {
                MessageBox.Show("Please select a category, then note in that category, you would like to delete.");
            }
            else
            {
                var answer = MessageBox.Show("Are you sure you would like to delete this note inside the category?", "Warning!", MessageBoxButton.YesNo);
                if (answer == MessageBoxResult.Yes)
                {
                    var item = (NHelperItem)lstInnerNotes.SelectedItem;
                    DataConnection.Instance.DeleteHelperItem(item.ID);
                    var helper = ((NHelper)lstNotes.SelectedItem);
                    helper.RefreshNotes();
                    lstInnerNotes.SelectedItem = null;
                    lstInnerNotes.ItemsSource = helper.Items;
                    lstInnerNotes.Items.Refresh();
                }
            }
        }
    }
}