using NaNoE.V2.Data;
using System.Windows;

namespace NaNoE.V2.Windows.Popups
{
    /// <summary>
    /// Interaction logic for AddItemWindow.xaml
    /// </summary>
    public partial class AddItemWindow : Window
    {
        public AddItemWindow()
        {
            InitializeComponent();
        }

        private void butAdd_Click(object sender, RoutedEventArgs e)
        {
            if (txtItem.Text.Length == 0)
            {
                MessageBox.Show("You can't make an item without any name at all, oops...");
            }
            else
            {
                DataConnection.Instance.InsertHelper(txtItem.Text, "[I]");
                this.Close();
            }
        }
    }
}
