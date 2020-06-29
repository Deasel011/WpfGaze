//  ==========================================================================
//   Code created by Philippe Deslongchamps.
//   For the Stockgaze project.
//  ==========================================================================

using System.Windows;
using OptionGaze.Login;

namespace OptionGaze
{

    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {

        private readonly GazerVM m_gazerVm;

        public MainWindow()
        {
            InitializeComponent();
            m_gazerVm = new GazerVM();
            DataContext = m_gazerVm;
            m_gazerVm.Initialize();
        }

        private void QuestradeLoginButtonClick(object sender, RoutedEventArgs e)
        {
            var dialog = new QuestradeLogin();
            if (dialog.ShowDialog() == true)
            {
                MessageBox.Show($"You entered the refresh token: {dialog.ResponseText} and Demo: {dialog.IsDemo}");
                var refreshToken = dialog.ResponseText;
                var isDemo = dialog.IsDemo;
                ((GazerVM)DataContext).QuestradeAccountManager.Login(refreshToken, isDemo);
            }
        }

        private async void QuestradeSymbolsRefreshButtonClick(object sender, RoutedEventArgs e)
        {
        }

    }

}