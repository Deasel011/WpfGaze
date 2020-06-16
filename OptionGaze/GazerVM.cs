using System;
using System.ComponentModel;
using OptionGaze.Login;
using Prism.Mvvm;

namespace OptionGaze
{

    public class GazerVM : BindableBase//, IDisposable
    {

        private QuestradeAccountManager m_questradeAccountManager;

        public GazerVM()
        {
            QuestradeAccountManager = new QuestradeAccountManager();
            // QuestradeAccountManager.PropertyChanged += QuestradeAccountManagerOnPropertyChanged;
        }

        // private void QuestradeAccountManagerOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        // {
        //     RaisePropertyChanged(nameof(QuestradeAccountManager));
        // }
        //
        // public void Dispose()
        // {
        //     QuestradeAccountManager.PropertyChanged -= QuestradeAccountManagerOnPropertyChanged;
        // }

        public void Initialize()
        {
            QuestradeAccountManager.TryRefreshAuth();
        }

        public QuestradeAccountManager QuestradeAccountManager
        {
            get => m_questradeAccountManager;
            set
            {
                if (m_questradeAccountManager == value)
                {
                    return;
                }
                RaisePropertyChanged(nameof(QuestradeAccountManager));
                m_questradeAccountManager = value;
            }
        }

    }

}