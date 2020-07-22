using NaNoE.V2.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace NaNoE.V2.ViewModels
{
    /// <summary>
    /// View Model for Bookmarks view
    /// </summary>
    class BookmarksViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Initiate BookmarksViewModel
        /// </summary>
        public BookmarksViewModel()
        {
            _goBack = new CommandBase(new Action(_run_goBack));
            _goElementJump = new CommandBase(new Action(_run_goElementJump));
        }

        /// <summary>
        /// Refresh lists of 'bookmark' points
        ///  - Bookmarks
        ///  - Chapters
        /// </summary>
        internal void RefreshLists()
        {
            var chapters = DataConnection.Instance.GetChapterIDs();
            var chaptersAdjusted = new List<string>();
            for (int i = 0; i < chapters.Count; ++i)
            {
                chaptersAdjusted.Add("Chapter " + (i + 1) + "\t" + chapters[i]);
            }
            Chapters = chaptersAdjusted;

            var bookmarks = DataConnection.Instance.GetBookmarks();
            // TODO: consider sort? adds delays though?
            Bookmarks = bookmarks;
        }

        /// <summary>
        /// Execute Go Back
        /// </summary>
        private void _run_goBack()
        {
            Navigator.Instance.GoBack();
        }

        /// <summary>
        /// Function to Go Back
        /// </summary>
        private ICommand _goBack;
        public ICommand GoBack
        {
            get { return _goBack; }
        }

        /// <summary>
        /// Element Number
        /// </summary>
        private string _elementNum = "";
        public string ElementNum
        {
            get { return _elementNum; }
            set
            {
                _elementNum = value;
                if (null != PropertyChanged) PropertyChanged(this, new PropertyChangedEventArgs("ElementNum"));
            }
        }

        /// <summary>
        /// Go to Element
        /// </summary>
        private void _run_goElementJump()
        {
            try
            {
                var val = int.Parse(ElementNum);
                if (val < 1)
                {
                    MessageBox.Show("Cant jump to a number that low.");
                }
                else if (val > MapCount)
                {
                    MessageBox.Show("Cant jump to a number that high.");
                }
                else
                {
                    DataConnection.Instance.Position = val;
                    DataConnection.Instance.UpdateNItems();
                    Navigator.Instance.GoBack();
                }
            }
            catch
            {
                MessageBox.Show("Error: There was a problem with the number you put for the element.");
            }
        }

        /// <summary>
        /// Go to Element
        ///  - It checks if the element is in the map
        /// </summary>
        private ICommand _goElementJump;
        public ICommand GoElementJump
        {
            get { return _goElementJump; }
        }

        /// <summary>
        /// List of Chapter's
        /// </summary>
        private List<string> _chapters = new List<string>();
        public List<string> Chapters
        {
            get { return _chapters; }
            set
            {
                _chapters = value;
                if (null != PropertyChanged) PropertyChanged(this, new PropertyChangedEventArgs("Chapters"));
            }
        }

        /// <summary>
        /// List of Bookmarks
        /// </summary>
        private List<string> _bookmarks = new List<string>();
        public List<string> Bookmarks
        {
            get { return _bookmarks; }
            set
            {
                _bookmarks = value;
                if (null != PropertyChanged) PropertyChanged(this, new PropertyChangedEventArgs("Bookmarks"));
            }
        }

        /// <summary>
        /// Size of the novel's map of elements
        /// </summary>
        public int MapCount
        {
            get { return DataConnection.Instance.MapSize; }
        }
    }
}
