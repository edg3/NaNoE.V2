using NaNoE.V2.ViewModels;

namespace NaNoE.V2
{
    /// <summary>
    /// Reference to data bind all views we make to their view models
    /// </summary>
    class ViewModelLocator
    {
        /// <summary>
        /// Static instance reference
        /// </summary>
        private static ViewModelLocator _instance;
        public static ViewModelLocator Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Create an instance
        /// </summary>
        public ViewModelLocator()
        {
            _instance = this;
        }

        /// <summary>
        /// Novel VM
        /// </summary>
        private NovelViewModel _novelVM = new NovelViewModel();
        public NovelViewModel NovelVM
        {
            get { return _novelVM; }
        }

        /// <summary>
        /// Bookmarks VM
        /// </summary>
        private BookmarksViewModel _bookmarksVM = new BookmarksViewModel();
        public BookmarksViewModel BookmarksVM
        {
            get { return _bookmarksVM; }
        }

        /// <summary>
        /// Edit VM
        /// </summary>
        private EditViewModel _editVM = new EditViewModel();
        public EditViewModel EditVM
        {
            get { return _editVM; }
        }

        /// <summary>
        /// Edit Options VM
        /// </summary>
        private EditOptionsViewModel _editOptionsVM = new EditOptionsViewModel();
        public EditOptionsViewModel EditOptionsVM
        {
            get { return _editOptionsVM; }
        }

        /// <summary>
        /// View Options VM
        /// </summary>
        private ViewOptionsViewModel _viewOptionsVM = new ViewOptionsViewModel();
        public ViewOptionsViewModel  ViewOptionsVM
        {
            get { return _viewOptionsVM; }
        }
    }
}
