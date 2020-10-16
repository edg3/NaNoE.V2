using NaNoE.V2.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace NaNoE.V2.ViewModels
{
    /// <summary>
    /// Novel View Model
    /// </summary>
    class NovelViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Private members
        /// </summary>
        private int _wordCount = 0;
        private char[] splt = new char[1] { ' ' };

        /// <summary>
        /// The writing content
        /// </summary>
        private string _content = "";
        public string Content
        {
            get { return _content; }
            set
            {
                _content = value;
                var tmp = value.Split(splt);
                if ((tmp.Length > _wordCount) || (tmp.Length < _wordCount))
                {
                    if (_wordCount != 0)
                    {
                        MainWindow.Instance.lstSuggestions.Items.Clear();
                        _run_Check();
                    }
                    _wordCount = tmp.Length;
                }
                if (null != PropertyChanged) PropertyChanged(this, new PropertyChangedEventArgs("Content"));
                MainWindow.Instance.ListSuggestions.Items.Refresh();
            }
        }

        /// <summary>
        /// Run Spell Check
        /// </summary>
        private void _run_Check()
        {
            try
            {
                EditMap = EditProcessor.Instance.Process(_content);
                for (int i = 0; i < EditMap.Count; ++i)
                {
                    MainWindow.Instance.lstSuggestions.Items.Add(EditMap[i]);
                }
            } catch { }
        }

        /// <summary>
        /// The list of edits to show the user
        /// </summary>
        private List<string> _editMap = new List<string>();
        public List<string> EditMap
        {
            get { return _editMap; }
            set
            {
                _editMap = value;
                if (null != PropertyChanged) PropertyChanged(this, new PropertyChangedEventArgs("EditMap"));
            }
        }

        /// <summary>
        /// Initiate Novel View Model
        /// </summary>
        public NovelViewModel()
        {
            _goToEditView = new CommandBase(new Action(_run_goToEditView));
            _goToStart = new CommandBase(new Action(_run_goToStart));
            _goUpOne = new CommandBase(new Action(_run_goUpOne));
            _goToBookmarks = new CommandBase(new Action(_run_goToBookmarks));
            _goDownOne = new CommandBase(new Action(_run_goDownOne));
            _goToEnd = new CommandBase(new Action(_run_goToEnd));

            _addChapter = new CommandBase(new Action(_run_addChapter));
            _addNote = new CommandBase(new Action(_run_addNote));
            _addBookmark = new CommandBase(new Action(_run_addBookmark));

            _previousStyle = MainWindow.SPara;
            _nextStyle = MainWindow.SPara;
        }

        /// <summary>
        /// Run Go To Edit View
        /// </summary>
        private void _run_goToEditView()
        {
            if (DataConnection.Instance.MapSize == 0)
            {
                MessageBox.Show("Can't go to edit mode with nothing in the novel.");
                return;
            }
            if (DataConnection.Instance.Position == 0) DataConnection.Instance.Position = 1;
            Navigator.Instance.GoTo("edit");
        }

        /// <summary>
        /// Go To Edit View
        /// </summary>
        private ICommand _goToEditView;
        public ICommand GoToEditView
        {
            get { return _goToEditView; }
        }

        /// <summary>
        /// Run Add Chapter
        /// </summary>
        private void _run_addChapter()
        {
            if (Content.Length > 0)
            {
                MessageBox.Show("Can't create Chapter with text in the text box.");
            }
            else
            {
                DataConnection.Instance.InsertElement(1, "Chapter", true);
                Content = "";
                Navigator.Instance.GoTo("novel");
            }
        }

        /// <summary>
        /// Add Chapter
        /// </summary>
        private ICommand _addChapter;
        public ICommand AddChapter
        {
            get { return _addChapter; }
        }

        /// <summary>
        /// Run Add Note
        /// </summary>
        private void _run_addNote()
        {
            if (Content.Length == 0)
            {
                MessageBox.Show("Can't create Note with no text in the text box.");
            }
            else
            {
                DataConnection.Instance.InsertElement(2, Content, true);
                Content = "";
                Navigator.Instance.GoTo("novel");
            }
        }

        /// <summary>
        /// Add Note
        /// </summary>
        private ICommand _addNote;
        public ICommand AddNote
        {
            get { return _addNote; }
        }

        /// <summary>
        /// Run Add Bookmark
        /// </summary>
        private void _run_addBookmark()
        {
            if (Content.Length < 1)
            {
                MessageBox.Show("Please specify a title for the bookmark of up to 25 characters.");
            }
            else if (Content.Length > 25)
            {
                MessageBox.Show("Please specify a title for the bookmark of no more than 25 characters.");
            }
            else
            {
                DataConnection.Instance.InsertElement(3, Content, true);
                Content = "";
                Navigator.Instance.GoTo("novel");
            }
        }

        /// <summary>
        /// Add Bookmark
        /// </summary>
        private ICommand _addBookmark;
        public ICommand AddBookmark
        {
            get { return _addBookmark; }
        }

        /// <summary>
        /// Previous NItem (i.e. position this will be after)
        /// </summary>
        public NItem Previous
        {
            get { return DataConnection.Instance.NPosition; }
        }

        /// <summary>
        /// Next NItem
        /// </summary>
        public NItem Next
        {
            get { return DataConnection.Instance.NNext; }
        }
        
        /// <summary>
        /// Run Go Up One
        /// </summary>
        private void _run_goUpOne()
        {
            if (DataConnection.Instance.Position > 0)
            {
                --DataConnection.Instance.Position;
                DataConnection.Instance.UpdateNItems();
                UpdateStyles();
                Navigator.Instance.GoTo("novel");
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
            if (DataConnection.Instance.Position > 0)
            {
                DataConnection.Instance.Position = 0;
                DataConnection.Instance.UpdateNItems();
                UpdateStyles();
                Navigator.Instance.GoTo("novel");
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
        /// Run Go Bookmarks
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
                UpdateStyles();
                Navigator.Instance.GoTo("novel");
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
                UpdateStyles();
                Navigator.Instance.GoTo("novel");
            }
        }

        /// <summary>
        /// Go To End
        /// </summary>
        private ICommand _goToEnd;
        public ICommand GoToEnd
        {
            get { return _goToEnd; }
        }

        /// <summary>
        /// Reference to word count
        /// </summary>
        public int Count
        {
            get { return DataConnection.Instance.WordCount; }
        }

        /// <summary>
        /// Get previous NItem style
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
        /// Get next NItem style
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
        /// Update the styles for shown NItems
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
    }
}
