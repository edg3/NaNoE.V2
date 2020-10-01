using System;
using System.Windows;

namespace NaNoE.V2.Views
{
    /// <summary>
    /// Interaction logic for ReadmeView.xaml
    /// </summary>
    public partial class ReadmeView : Window
    {
        /// <summary>
        /// Initiate Readme View
        /// </summary>
        public ReadmeView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Action to go Back to previous view
        /// </summary>
        /// <param name="sender">Object that send it</param>
        /// <param name="e">Event args</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Navigator.Instance.GoBack();
        }

        /// <summary>
        /// Web link for more information
        /// TODO: Need to take this to NaNoE.V2 Page
        /// </summary>
        /// <param name="sender">Object that sent it</param>
        /// <param name="e">Event Args</param>
        private void butWeb_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.edg3.co.za/nanoe.v2/");
        }

        private void grdReadme_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshStyles();
        }

        private void RefreshStyles()
        {
            switch (MainWindow.Instance.SStyle)
            {
                case "Light":
                    grdReadme.Style = MainWindow.SLightBackground;
                    break;
                default:
                    grdReadme.Style = MainWindow.SDarkBackground;
                    break;
            }
        }
    }
}