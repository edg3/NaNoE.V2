using NaNoE.V2.Data;
using System.Windows;

namespace NaNoE.V2.Windows.Popups
{
    /// <summary>
    /// Interaction logic for AddNoteWindow.xaml
    /// </summary>
    public partial class AddNoteWindow : Window
    {
        private NHelper _helper;
        public AddNoteWindow(NHelper helper)
        {
            InitializeComponent();

            _helper = helper;
        }

        private void butAdd_Click(object sender, RoutedEventArgs e)
        {
            if (txtNote.Text.Length == 0)
            {
                MessageBox.Show("Oops, it wouldn't make sense to add a blank note, would it?");
            }
            else
            {
                DataConnection.Instance.InsertHelperNote(_helper, txtNote.Text);
                this.Close();
            }
        }
    }
}
