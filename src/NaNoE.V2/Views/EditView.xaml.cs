using System.Windows;

namespace NaNoE.V2.Views
{
    /// <summary>
    /// Interaction logic for EditView.xaml
    /// </summary>
    public partial class EditView : Window
    {
        /// <summary>
        /// Initiate EditView
        /// </summary>
        public EditView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Get the latest Data from the Edit VM on Load
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">The event args</param>
        private void txtContent_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModelLocator.Instance.EditVM.txtContent = txtContent;
            txtContent.Text = ViewModelLocator.Instance.EditVM.Element.Data;
            txtContent.Focus();
        }

        /// <summary>
        /// When the text line is moved it updates the word number
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">The event args</param>
        private void txtContent_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var tmp = txtContent.Text.Substring(0, txtContent.SelectionStart);
            var splt = tmp.Split(' ').Length;
            if (null != lblWord) lblWord.Content = "Word: " + splt;
        }

        private void grdEdit_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshStyles();
        }

        private void RefreshStyles()
        {
            switch (MainWindow.Instance.SStyle)
            {
                case "Light":
                    grdEdit.Style = MainWindow.SLightBackground;
                    break;
                default:
                    grdEdit.Style = MainWindow.SDarkBackground;
                    break;
            }
        }
    }
}