using NaNoE.V2.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
