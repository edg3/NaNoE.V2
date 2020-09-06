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
