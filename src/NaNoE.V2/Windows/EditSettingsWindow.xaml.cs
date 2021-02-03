using NaNoE.V2.Data;
using System;
using System.Windows;

namespace NaNoE.V2.Windows
{
    /// <summary>
    /// Interaction logic for EditSettingsWindow.xaml
    /// </summary>
    public partial class EditSettingsWindow : Window
    {
        public EditSettingsWindow()
        {
            InitializeComponent();
        }

        private void lstOptions_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModelLocator.Instance.EditOptionsVM.VisibleList = lstOptions;
        }

        private void lstIgnored_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModelLocator.Instance.EditOptionsVM.VisibleIgnoredList = lstIgnored;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            EditProcessor.Instance.SaveEditsOptions();
        }

        private void lstPhrases_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModelLocator.Instance.EditOptionsVM.VisiblePhraseList = lstPhrases;
        }
    }
}
