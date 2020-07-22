using NaNoE.V2.Data;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NaNoE.V2.Views
{
    /// <summary>
    /// Interaction logic for BookmarksView.xaml
    /// </summary>
    public partial class BookmarksView : Window
    {
        /// <summary>
        /// Initiate the view
        /// </summary>
        public BookmarksView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Textbox Input filter
        ///  - allow only digits
        /// </summary>
        /// <param name="sender">Object sending request</param>
        /// <param name="e">The event args</param>
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
        }

        /// <summary>
        /// Jump to selected chapter
        /// </summary>
        /// <param name="sender">Object sending request</param>
        /// <param name="e">The event args</param>
        private void butChapterJump_Click(object sender, RoutedEventArgs e)
        {
            if (lstChapters.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a chapter above.");
            }
            else
            {
                DataConnection.Instance.Position = int.Parse(lstChapters.SelectedItem.ToString().Split('\t')[1]) + 1;
                DataConnection.Instance.UpdateNItems();
                Navigator.Instance.GoTo("novel");
            }
        }

        /// <summary>
        /// Jump to selected bookmark
        /// </summary>
        /// <param name="sender">Object sending requests</param>
        /// <param name="e">The event args</param>
        private void butBookmarkJump_Click(object sender, RoutedEventArgs e)
        {
            if (lstBookmarks.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a bookmark above.");
            }
            else
            {
                var splt = int.Parse(((string)lstBookmarks.SelectedItem).Split(' ')[0]);
                DataConnection.Instance.Position = DataConnection.Instance.MapPosition(splt);
                DataConnection.Instance.UpdateNItems();
                Navigator.Instance.GoTo("novel");
            }
        }

        /// <summary>
        /// Double clicked on Chapters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstChapters_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            butChapterJump_Click(null, null);
        }

        /// <summary>
        /// Double clicked on Bookmarks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstBookmarks_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            butBookmarkJump_Click(null, null);
        }
    }
}
