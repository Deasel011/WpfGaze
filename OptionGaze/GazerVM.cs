//  ==========================================================================
//   Code created by Philippe Deslongchamps.
//   For the Stockgaze project.
//  ==========================================================================

using OptionGaze.Login;
using Prism.Mvvm;

namespace OptionGaze
{

    public class GazerVM : BindableBase //, IDisposable
    {

        private QuestradeAccountManager m_questradeAccountManager;

        public GazerVM()
        {
            QuestradeAccountManager = new QuestradeAccountManager();
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

        public void Initialize()
        {
            QuestradeAccountManager.TryRefreshAuth();
        }

    }

}