using System.Windows;

namespace NaNoE.V2.Views
{
    /// <summary>
    /// Interaction logic for StartView.xaml
    /// </summary>
    public partial class StartView : Window
    {
        public StartView()
        {
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshStyles();
        }

        private void RefreshStyles()
        {
            switch (MainWindow.Instance.SStyle)
            {
                case "Light":
                    grdView.Style = MainWindow.SLightBackground;
                    break;
                default:
                    grdView.Style = MainWindow.SDarkBackground;
                    break;
            }
        }
    }
}
