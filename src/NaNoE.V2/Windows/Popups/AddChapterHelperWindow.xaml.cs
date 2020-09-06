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
    /// Interaction logic for AddHelperWindow.xaml
    /// </summary>
    public partial class AddChapterHelperWindow : Window
    {
        public AddChapterHelperWindow()
        {
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            var items = (from i in DataConnection.Instance.Helpers
                         where i.Name.StartsWith("[A:")
                         select i).ToList();
            foreach (var item in items)
            {
                lstPositions.Items.Add(item);
            }
            var padder = (lstPositions.Items.Count + 1).ToString();
            if (padder.Length < 3)
            {
                while (padder.Length < 3) padder = "0" + padder;
            }
            var returned = lstPositions.Items.Add("[A:" + padder + "]");
            lstPositions.SelectedIndex = returned;
        }

        private void butAdd_Click(object sender, RoutedEventArgs e)
        {
            if (txtName.Text.Length == 0)
            {
                MessageBox.Show("Please add any name for yourself, this can help you with what you'd want in chapters.");
            }
            else
            {
                var whereTo = lstPositions.SelectedItem.ToString().Split(' ')[0];
                DataConnection.Instance.InsertHelper(txtName.Text, whereTo);
                this.Close();
            }
        }
    }
}
