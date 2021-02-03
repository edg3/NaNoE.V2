using NaNoE.V2.Data;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NaNoE.V2.ViewModels
{
    /// <summary>
    /// Edit View Model
    /// </summary>
    class EditViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Initiate Edit View Model
        /// </summary>
        public EditViewModel()
        {
            _goToStart = new CommandBase(new Action(_run_goToStart));
            _goUpOne = new CommandBase(new Action(_run_goUpOne));
            _goToBookmarks = new CommandBase(new Action(_run_goToBookmarks));
            _goDownOne = new CommandBase(new Action(_run_goDownOne));
            _goToEnd = new CommandBase(new Action(_run_goToEnd));

            _goToNovelView = new CommandBase(new Action(_run_goToNovelView));

            _saveChanges = new CommandBase(new Action(_run_saveChanges));
            _refreshEdits = new CommandBase(new Action(_run_refreshEdits));
            _deleteElement = new CommandBase(new Action(_run_deleteElement));
            _jumpNext = new CommandBase(new Action(_run_jumpNext));

            _resetIgnored = new CommandBase(new Action(_run_resetIgnored));
        }

        /// <summary>
        /// Run Go to Novel View
        /// </summary>
        private void _run_goToNovelView()
        {
            Navigator.Instance.GoTo("novel");
        }

        /// <summary>
        /// Go To Novel View
        /// </summary>
        private ICommand _goToNovelView;
        public ICommand GoToNovelView
        {
            get { return _goToNovelView; }
        }

        /// <summary>
        /// Run Go Up One
        /// </summary>
        private void _run_goUpOne()
        {
            if (DataConnection.Instance.Position > 1)
            {
                --DataConnection.Instance.Position;
                DataConnection.Instance.UpdateNItems();
                Navigator.Instance.GoTo("edit");
            }
        }

        /// <summary>
        /// Go Up One
        /// </summary>
        private ICommand _goUpOne;
        public ICommand GoUpOne
        {
            get { return _goUpOne; }
        }

        /// <summary>
        /// Run Go To Start
        /// </summary>
        private void _run_goToStart()
        {
            if (DataConnection.Instance.Position > 1)
            {
                DataConnection.Instance.Position = 1;
                DataConnection.Instance.UpdateNItems();
                Navigator.Instance.GoTo("edit");
            }
        }

        /// <summary>
        /// Go To Start
        /// </summary>
        private ICommand _goToStart;
        public ICommand GoToStart
        {
            get { return _goToStart; }
        }

        /// <summary>
        /// Update the view styles and data
        /// </summary>
        internal void Refresh()
        {
            DataConnection.Instance.UpdateNItems();

            // If no element exists (e.g. deleted the last one) - go to Novel View
            if (null == Element)
            {
                Navigator.Instance.GoTo("novel");
                return;
            }

            if (null != txtContent) txtContent.Text = Element.Data;
            UpdateStyles();
        }

        /// <summary>
        /// Run Go To Bookmarks
        /// </summary>
        private void _run_goToBookmarks()
        {
            Navigator.Instance.GoTo("bookmarks");
        }

        /// <summary>
        /// Go To Bookmarks
        /// </summary>
        private ICommand _goToBookmarks;
        public ICommand GoToBookmarks
        {
            get { return _goToBookmarks; }
        }

        /// <summary>
        /// Run Go Down One
        /// </summary>
        private void _run_goDownOne()
        {
            if (DataConnection.Instance.Position < DataConnection.Instance.MapSize)
            {
                ++DataConnection.Instance.Position;
                DataConnection.Instance.UpdateNItems();
                Navigator.Instance.GoTo("edit");
            }
        }

        /// <summary>
        /// Go Down One
        /// </summary>
        private ICommand _goDownOne;
        public ICommand GoDownOne
        {
            get { return _goDownOne; }
        }

        /// <summary>
        /// Run Go To End
        /// </summary>
        private void _run_goToEnd()
        {
            if (DataConnection.Instance.Position < DataConnection.Instance.MapSize)
            {
                DataConnection.Instance.Position = DataConnection.Instance.MapSize;
                DataConnection.Instance.UpdateNItems();
                Navigator.Instance.GoTo("edit");
            }
        }

        /// <summary>
        /// Go to End
        /// </summary>
        private ICommand _goToEnd;
        public ICommand GoToEnd
        {
            get { return _goToEnd; }
        }

        /// <summary>
        /// Reference to DataConnection.WordCount
        /// </summary>
        public int Count
        {
            get { return DataConnection.Instance.WordCount; }
        }

        private ControlType ItemType = ControlType.Paragraph;

        /// <summary>
        /// Element we are looking at
        /// </summary>
        private NItem _element = null;
        public NItem Element
        {
            get
            {
                if (DataConnection.Instance.NPosition == null) return null;
                if ((null != _element) && (null != DataConnection.Instance.NPosition))
                {
                    if (DataConnection.Instance.NPosition.ID == _element.ID) return _element;
                }

                var item = DataConnection.Instance.NPosition;
                ID = item.ID;
                switch (item.CType)
                {
                    case ControlType.Chapter:
                        ElementType = "Chapter";
                        AllowEdit = false;
                        break;

                    case ControlType.Bookmark:
                        ElementType = "Bookmark";
                        AllowEdit = false;
                        break;

                    case ControlType.Note:
                        ElementType = "Note";
                        AllowEdit = true;
                        break;

                    case ControlType.Paragraph:
                        ElementType = "Paragraph";
                        AllowEdit = true;
                        break;
                }

                if (null != txtContent) txtContent.Text = item.Data;
                ItemType = item.CType;
                _run_refreshEdits();

                _element = item;

                return item;
            }
        }

        /// <summary>
        /// If the element we are at allows editing
        /// </summary>
        private bool _allowEdit = true;
        public bool AllowEdit
        {
            get { return _allowEdit; }
            set
            {
                _allowEdit = value;
                if (null != PropertyChanged) PropertyChanged(this, new PropertyChangedEventArgs("AllowEdit"));
            }
        }

        /// <summary>
        /// The type of element we are at
        /// </summary>
        private string _elementType = "";
        public string ElementType
        {
            get { return _elementType; }
            set
            {
                _elementType = value;
                if (null != PropertyChanged) PropertyChanged(this, new PropertyChangedEventArgs("ElementType"));
            }
        }

        /// <summary>
        /// The ID of the element we are at
        /// </summary>
        private int _id = 0;
        public int ID
        {
            get { return _id; }
            set
            {
                _id = value;
                if (null != PropertyChanged) PropertyChanged(this, new PropertyChangedEventArgs("ID"));
            }
        }

        /// <summary>
        /// Previous NItem
        /// </summary>
        public NItem Previous
        {
            get { return DataConnection.Instance.NPrevious; }
        }

        /// <summary>
        /// Next NItem
        /// </summary>
        public NItem Next
        {
            get { return DataConnection.Instance.NNext; }
        }

        /// <summary>
        /// Previous item style
        /// </summary>
        private Style _previousStyle;
        public Style PreviousStyle
        {
            get { return _previousStyle; }
            set
            {
                _previousStyle = value;
                if (null != PropertyChanged) PropertyChanged(this, new PropertyChangedEventArgs("PreviousStyle"));
            }
        }

        /// <summary>
        /// Next item style
        /// </summary>
        private Style _nextStyle;
        public Style NextStyle
        {
            get { return _nextStyle; }
            set
            {
                _nextStyle = value;
                if (null != PropertyChanged) PropertyChanged(this, new PropertyChangedEventArgs("NextStyle"));
            }
        }

        /// <summary>
        /// Update previous and next style settings
        /// </summary>
        public void UpdateStyles()
        {
            if (null != Previous)
            {
                switch (Previous.CType)
                {
                    case ControlType.Paragraph: PreviousStyle = MainWindow.SPara; break;
                    case ControlType.Chapter: PreviousStyle = MainWindow.SChap; break;
                    case ControlType.Bookmark: PreviousStyle = MainWindow.SBook; break;
                    case ControlType.Note: PreviousStyle = MainWindow.SNote; break;
                }
            }

            if (null != Next)
            {
                switch (Next.CType)
                {
                    case ControlType.Paragraph: NextStyle = MainWindow.SPara; break;
                    case ControlType.Chapter: NextStyle = MainWindow.SChap; break;
                    case ControlType.Bookmark: NextStyle = MainWindow.SBook; break;
                    case ControlType.Note: NextStyle = MainWindow.SNote; break;
                }
            }
        }

        /// <summary>
        /// Get the difference between two numbers
        ///  - i.e. for word count, e.g. 3 words more, or -2 words less
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private int Diff(int a, int b)
        {
            if ((a < b) || (a > b))
            {
                return b - a;
            }

            return 0;
        }

        /// <summary>
        /// Run Save Changes
        /// </summary>
        private void _run_saveChanges()
        {
            var data = txtContent.Text; // Check a lot, must be better way on Element binding which refreshes when needed
            var element = Element;

            var incremental = Diff(element.Data.Split(' ').Length, data.Split(' ').Length);
            DataConnection.Instance.AdjustWordCount(incremental);

            DataConnection.Instance.UpdateData(element.ID, data);
            _element = DataConnection.Instance.GetPosition();
            Navigator.Instance.GoTo("edit");
        }

        /// <summary>
        /// Save Changes
        /// </summary>
        private ICommand _saveChanges;
        public ICommand SaveChanges
        {
            get { return _saveChanges; }
        }

        /// <summary>
        /// Refresh Edits Suggestions
        /// </summary>
        private void _run_refreshEdits()
        {
            MainWindow.Instance.lstSuggestions.Items.Clear();
            if (ItemType == ControlType.Paragraph)
            {
                if (null == txtContent)
                {
                    EditProcessor.Instance.Position = "edit";
                    var editMap = EditProcessor.Instance.Process(DataConnection.Instance.NPosition.Data);
                    for (int i = 0; i < editMap.Count; ++i)
                    {
                        MainWindow.Instance.lstSuggestions.Items.Add(editMap[i]);
                    }
                }
                else
                {
                    var editMap = EditProcessor.Instance.Process(txtContent.Text);
                    for (int i = 0; i < editMap.Count; ++i)
                    {
                        MainWindow.Instance.lstSuggestions.Items.Add(editMap[i]);
                    }
                }
            }
            MainWindow.Instance.lstSuggestions.Items.Refresh();
        }

        /// <summary>
        /// Refresh Edits
        /// </summary>
        private ICommand _refreshEdits;
        public ICommand RefreshEdits
        {
            get { return _refreshEdits; }
        }

        /// <summary>
        /// Run Delete Element
        /// </summary>
        private void _run_deleteElement()
        {
            var result = MessageBox.Show("Are you sure you would like to delete this element? You wont get it back.", "Warning: About to delete element.", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                DataConnection.Instance.DeletePosition();
                if (DataConnection.Instance.MapSize > 0)
                {
                    Navigator.Instance.GoTo("edit");
                }
                else
                {
                    MessageBox.Show("Nothing left to look at in edit view.");
                    Navigator.Instance.GoTo("novel");
                }
            }
        }

        /// <summary>
        /// Delete Element
        /// </summary>
        private ICommand _deleteElement;
        public ICommand DeleteElement
        {
            get { return _deleteElement; }
        }

        /// <summary>
        /// Size of the novel's map of elements
        /// </summary>
        public int MapCount
        {
            get { return DataConnection.Instance.MapSize; }
        }

        /// <summary>
        /// Position we are in
        /// </summary>
        public int Position
        {
            get { return DataConnection.Instance.Position; }
        }

        /// <summary>
        /// Run Jump Next
        /// </summary>
        private async void _run_jumpNext()
        {
            if (txtContent.Text != _element.Data)
            {
                MessageBox.Show("You have not saved changes...");
            }
            else
            {
                MainWindow.Instance.grdSearch.Visibility = Visibility.Visible;
                await System.Threading.Tasks.Task.Delay(1);

                DataConnection.Instance.EditJump();
                Navigator.Instance.GoTo("edit");

                MainWindow.Instance.grdSearch.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// Jump to Next item that has edit suggestions.
        /// </summary>
        private ICommand _jumpNext;
        public ICommand JumpNext
        {
            get { return _jumpNext; }
        }

        /// <summary>
        /// Textbox to move cursor in
        /// </summary>
        internal TextBox txtContent;

        /// <summary>
        /// Run Reset Ignored
        /// </summary>
        private void _run_resetIgnored()
        {
            var answer = MessageBox.Show("Are you sure you want to reset all paragraphs to not be flag ignored?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (answer == MessageBoxResult.Yes)
            {
                DataConnection.Instance.ResetFlags();
                MessageBox.Show("All paragraphs will be checked in 'Next...' jumps again.");

                // Refresh
                Navigator.Instance.GoTo("edit");
            }
        }

        /// <summary>
        /// Edited element flagged ignored
        /// </summary>
        public bool Flagged
        {
            get { return Element.Flagged; }
            set
            {
                var clone = txtContent.Text.Clone().ToString();
                if (Element.Data != clone)
                {
                    MessageBox.Show("You should save your changes first...");
                }
                else
                {
                    Element.Flagged = value;
                }
            }
        }

        /// <summary>
        /// Rest Ignored
        /// </summary>
        private ICommand _resetIgnored;
        public ICommand ResetIgnored
        {
            get { return _resetIgnored; }
        }
    }
}
