using NaNoE.V2.Data;
using NaNoE.V2.Views;
using System.Windows;

namespace NaNoE.V2
{
    /// <summary>
    /// The view navigator
    /// </summary>
    class Navigator
    {
        /// <summary>
        /// MainWindow instance
        /// </summary>
        private MainWindow _main;

        /// <summary>
        /// Instance reference
        /// </summary>
        private static Navigator _instance;
        public static Navigator Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Initialise the Navigator
        /// </summary>
        /// <param name="main">MainWindow instance</param>
        public Navigator(MainWindow main)
        {
            _main = main;
            _instance = this;
        }

        /// <summary>
        /// Navigation
        /// </summary>
        private string _previousView = null;
        private string _currentView = null;
        public string CurrentView
        {
            get { return _currentView; }
        }

        /// <summary>
        /// Changes the view shown
        /// </summary>
        /// <param name="name">The view to go to</param>
        public void GoTo(string name)
        {
            Window w = null;
            switch (name)
            {
                case "start": w = new StartView(); break;
                case "novel":
                    if (null != ViewModelLocator.Instance) ViewModelLocator.Instance.NovelVM.UpdateStyles();
                    w = new NovelView(); break;
                case "bookmarks":
                    if (null != ViewModelLocator.Instance) ViewModelLocator.Instance.BookmarksVM.RefreshLists();
                    w = new BookmarksView(); break;
                case "edit":
                    if (null != ViewModelLocator.Instance) ViewModelLocator.Instance.EditVM.Refresh();
                    w = new EditView(); break;
                case "readme":
                    w = new ReadmeView(); break;
            }

            if (null != w)
            {
                if (null != MainWindow.Instance)
                {
                    if (MainWindow.Instance.ListSuggestions.Items.Count > 0)
                    {
                        MainWindow.Instance.ListSuggestions.Items.Clear();
                    }

                    if ("edit" == name)
                    {
                        if (null != ViewModelLocator.Instance.EditVM.Element)
                        {
                            if (ViewModelLocator.Instance.EditVM.Element.CType == Data.ControlType.Paragraph)
                            {
                                var answer = EditProcessor.Instance.Process(ViewModelLocator.Instance.EditVM.Element.Data);
                                for (int i = 0; i < answer.Count; ++i)
                                {
                                    MainWindow.Instance.ListSuggestions.Items.Add(answer[i]);
                                }
                            }
                        }
                    }

                    MainWindow.Instance.ListSuggestions.Items.Refresh();
                }

                _main.frmContent.Content = w.Content;
                w.Close();

                _previousView = _currentView;
                _currentView = name;
                EditProcessor.Instance.Position = name;
            }
        }

        /// <summary>
        /// Go back to previous view
        /// </summary>
        public void GoBack()
        {
            if (null != _previousView)
            {
                if ("readme" != _previousView)
                {
                    GoTo(_previousView);
                }
                else
                {
                    GoTo("novel");
                }
            }
        }

    }
}
