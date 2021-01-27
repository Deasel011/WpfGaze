//  ==========================================================================
//   Code created by Philippe Deslongchamps.
//   For the Stockgaze project.
//  ==========================================================================

using System;
using System.ComponentModel;
using System.Windows;
using OptionGaze.Login;
using Stockgaze.Core;

namespace OptionGaze
{

    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {

        private readonly GazerController m_gazerController;

        public MainWindow()
        {
            try
            {
                InitializeComponent();
                m_gazerController = new GazerController();
                DataContext = m_gazerController;
                m_gazerController.Initialize();
            }
            catch (Exception e)
            {
                MessageBox.Show($"message:{e.Message} stacktrace:{e.StackTrace}");
            }
        }

        private void QuestradeLoginButtonClick(object sender, RoutedEventArgs e)
        {
            var dialog = new QuestradeLogin();
            if (dialog.ShowDialog() == true)
            {
                MessageBox.Show($"You entered the refresh token: {dialog.ResponseText} and Demo: {dialog.IsDemo}");
                var refreshToken = dialog.ResponseText;
                var isDemo = dialog.IsDemo;
                ((GazerController)DataContext).QuestradeAccountManager.Login(refreshToken, isDemo);
            }
        }

        private async void QuestradeSymbolsRefreshButtonClick(object sender, RoutedEventArgs e)
        {
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            m_gazerController?.Dispose();
            base.OnClosing(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            m_gazerController?.Dispose();
            base.OnClosed(e);
        }

    }

}