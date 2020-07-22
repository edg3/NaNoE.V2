using Microsoft.Win32;
using NaNoE.V2.Data;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using Xceed.Words.NET;

namespace NaNoE.V2.Windows
{
    /// <summary>
    /// Interaction logic for ImportWordDocumentWindow.xaml
    /// </summary>
    public partial class ImportWordDocumentWindow : Window
    {
        /// <summary>
        /// Initiate Import Word Document Window
        /// </summary>
        public ImportWordDocumentWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Open DocX command
        ///  - Converts docx to novel DB for NaNoE.V2 use
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">Event args</param>
        private void butOpenDocX_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "Word 2007 Documents (*.docx)|*.docx";
            var ofdResult = ofd.ShowDialog();

            if (ofdResult == true)
            {
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

                        MessageBox.Show("Importing data. Warning: This might take a while.");

                        using (var stream = File.Open(ofd.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        {
                            using (var doc = DocX.Load(stream))
                            {
                                foreach (var para in doc.Paragraphs)
                                {
                                    if (para.Text.Length > 3)
                                    {
                                        if (para.Text.Substring(0, 4) == "[ch]")
                                        {
                                            DataConnection.Instance.InsertElement(1, "Chapter", true);
                                        }
                                        else
                                        {
                                            if (para.Text.Contains('\t'))
                                            {
                                                para.ReplaceText("\t", "");
                                            }
                                            DataConnection.Instance.InsertElement(0, para.Text, false);
                                        }
                                    }
                                }
                            }
                        }

                        MessageBox.Show("Complete! good luck with your novel!");
                        Navigator.Instance.GoTo("novel");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("File already exists...");
                    }
                }
            }
        }
    }
}