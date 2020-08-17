﻿using System;
using System.Collections.Generic;
using System.IO;
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

namespace NaNoE.V2.Windows
{
    /// <summary>
    /// Interaction logic for ViewSettingsWindow.xaml
    /// </summary>
    public partial class ViewSettingsWindow : Window
    {
        public ViewSettingsWindow()
        {
            InitializeComponent();
        }

        private void cmbStyle_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < cmbStyle.Items.Count; ++i)
            {
                var item = (ListBoxItem)cmbStyle.Items[i];
                if (item.Content.ToString() == MainWindow.Instance.SStyle)
                {
                    cmbStyle.SelectedIndex = i;
                    cmbStyle.Items.Refresh();
                    break;
                }
            }
        }

        private void butSave_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists("style.opt")) File.Delete("style.opt");

            string line = ((ListBoxItem)cmbStyle.SelectedItem).Content.ToString();
            using (var f = File.OpenWrite("style.opt"))
            {
                using (var w = new StreamWriter(f))
                {
                    w.WriteLine(line);
                }
            }

            MainWindow.Instance.SStyle = line;
            this.Close();
        }
    }
}
