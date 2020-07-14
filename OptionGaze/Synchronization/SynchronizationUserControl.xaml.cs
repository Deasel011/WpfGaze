using System;
using System.Windows;
using System.Windows.Controls;
using Stockgaze.Core;
using Stockgaze.Core.Synchronization;

namespace OptionGaze.Synchronization
{

    public partial class SynchronizationUserControl : UserControl
    {

        private readonly SynchronizationUCVM m_synchronizationucvm;

        public SynchronizationUserControl()
        {
            InitializeComponent();
            m_synchronizationucvm = new SynchronizationUCVM(GazerVM.GetQuestradeSymbolDataManager(), GazerVM.GetQuestradeSymbolIdManager(),
                GazerVM.GetQuestradeOptionManager(), GazerVM.GetSchedulingManager());
            DataContext = m_synchronizationucvm;
        }

        private async void RefreshSymbolId_OnClick(object sender, RoutedEventArgs e)
        {
            var res = MessageBox.Show("Are you sure you want to refresh the symbols, this operation can take up to half an hour.");
            if (res == MessageBoxResult.OK)
            {
                m_synchronizationucvm.IsRefreshing = true;
                await m_synchronizationucvm.SymbolIdManager.Refresh();
                m_synchronizationucvm.IsRefreshing = false;
            }
        }

        private async void RefreshSymbolData_OnClick(object sender, RoutedEventArgs e)
        {
            var res = MessageBox.Show("Are you sure you want to refresh the symbol data, this operation can take a few minutes.");
            if (res == MessageBoxResult.OK)
            {
                m_synchronizationucvm.IsRefreshing = true;
                await m_synchronizationucvm.SymbolDataManager.Refresh();
                m_synchronizationucvm.IsRefreshing = false;
            }
        }

        private async void RefreshOptions_OnClick(object sender, RoutedEventArgs e)
        {
            var res = MessageBox.Show("Are you sure you want to refresh the options, this operation can take a few minutes.");
            if (res == MessageBoxResult.OK)
            {
                m_synchronizationucvm.IsRefreshing = true;
                await m_synchronizationucvm.OptionManager.Refresh();
                m_synchronizationucvm.IsRefreshing = false;
            }
        }

    }

}