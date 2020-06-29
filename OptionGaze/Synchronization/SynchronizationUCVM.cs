using OptionGaze.Manager;
using Prism.Mvvm;

namespace OptionGaze.Synchronization
{

    public class SynchronizationUCVM : BindableBase
    {

        private bool m_isRefreshing;

        private QuestradeSymbolDataManager m_symbolDataManager;

        private QuestradeSymbolIdManager m_symbolIdManager;

        public bool IsRefreshing
        {
            get => m_isRefreshing;
            set => SetProperty(ref m_isRefreshing, value);
        }

        public QuestradeSymbolIdManager SymbolIdManager
        {
            get => m_symbolIdManager;
            set => SetProperty(ref m_symbolIdManager, value);
        }

        public QuestradeSymbolDataManager SymbolDataManager
        {
            get => m_symbolDataManager;
            set => SetProperty(ref m_symbolDataManager, value);
        }

        public SynchronizationUCVM(QuestradeSymbolDataManager symbolDataManager, QuestradeSymbolIdManager symbolIdManager)
        {
            m_symbolDataManager = symbolDataManager;
            m_symbolIdManager = symbolIdManager;
        }

    }

}