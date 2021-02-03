using NaNoE.V2.Data;
using System.Windows;

namespace NaNoE.V2.Windows.Popups
{
    /// <summary>
    /// Interaction logic for AddCharacterWindow.xaml
    /// </summary>
    public partial class AddCharacterWindow : Window
    {
        public AddCharacterWindow()
        {
            InitializeComponent();
        }

        private void butAdd_Click(object sender, RoutedEventArgs e)
        {
            if (txtCharacter.Text.Length == 0)
            {
                MessageBox.Show("Characters need names, perhaps, what would this one be called?");
            }
            // TODO: verify unique name in this & item window
            else
            {
                DataConnection.Instance.InsertHelper(txtCharacter.Text, "[C]");
                this.Close();
            }
        }
    }
}
