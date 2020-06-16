//  ==========================================================================
//  Copyright (C) 2020 by Genetec, Inc.
//  All rights reserved.
//  May be used only in accordance with a valid Source Code License Agreement.
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

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new GazerVM();
            ((GazerVM)DataContext).Initialize();
        }

        private void QuestradeLoginButtonClick(object sender, RoutedEventArgs e)
        {
            var dialog = new QuestradeLogin();
            if (dialog.ShowDialog() == true)
            {
                MessageBox.Show($"You entered the refresh token: {dialog.ResponseText} and Demo: {dialog.IsDemo}");
                var refreshToken = dialog.ResponseText;
                var isDemo = dialog.IsDemo;
                ((GazerVM)DataContext).QuestradeAccountManager.Login(refreshToken,isDemo);
            }
        }
        
    }

}