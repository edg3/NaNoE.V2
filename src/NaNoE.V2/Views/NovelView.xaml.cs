using NaNoE.V2.Data;
using System.Windows;
using System.Windows.Input;

namespace NaNoE.V2.Views
{
    /// <summary>
    /// Interaction logic for NovelView.xaml
    /// </summary>
    public partial class NovelView : Window
    {
        /// <summary>
        /// Initiate novel view
        /// </summary>
        public NovelView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Check for when Enter is pressed to create paragraph
        /// </summary>
        /// <param name="sender">Object that sent it</param>
        /// <param name="e">Event args</param>
        private void rtbContent_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                DataConnection.Instance.InsertElement(0, ViewModelLocator.Instance.NovelVM.Content.Trim(), false);
                ViewModelLocator.Instance.NovelVM.Content = "";
                Navigator.Instance.GoTo("novel");
            }
            else
            {
                ViewModelLocator.Instance.NovelVM.Content = txtContent.Text;
            }
        }

        /// <summary>
        /// Put the window focus in the text box for writing automatically
        /// </summary>
        /// <param name="sender">Object that sent it</param>
        /// <param name="e">Event args</param>
        private void txtContent_Loaded(object sender, RoutedEventArgs e)
        {
            txtContent.Focus();
        }

        private void grdNovel_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshStyles();
        }

        private void RefreshStyles()
        {
            switch (MainWindow.Instance.SStyle)
            {
                case "Light":
                    grdNovel.Style = MainWindow.SLightBackground;
                    break;
                default:
                    grdNovel.Style = MainWindow.SDarkBackground;
                    break;
            }
        }
    }
}