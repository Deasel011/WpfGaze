using OptionGaze.Manager;
using Prism.Mvvm;

namespace OptionGaze.Synchronization
{

    public class SynchronizationUCVM : BindableBase
    {

        private bool m_isRefreshing;

        private QuestradeOptionManager m_optionManager;

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

        public QuestradeOptionManager OptionManager
        {
            get => m_optionManager;
            set => SetProperty(ref m_optionManager, value);
        }

        public SynchronizationUCVM(QuestradeSymbolDataManager symbolDataManager, QuestradeSymbolIdManager symbolIdManager, QuestradeOptionManager optionManager)
        {
            m_symbolDataManager = symbolDataManager;
            m_symbolIdManager = symbolIdManager;
            m_optionManager = optionManager;
        }

    }

}