using System;
using System.Threading.Tasks;
using Prism.Mvvm;
using Stockgaze.Core.Login;
using Stockgaze.Core.Manager;
using Stockgaze.Core.Repositories;

namespace Stockgaze.Core
{

    public class GazerVM : BindableBase
    {

        private static QuestradeAccountManager s_questradeAccountManager;

        private static QuestradeSymbolIdManager s_questradeSymbolIdManager;

        private static QuestradeSymbolDataManager s_questradeSymbolDataManager;

        private static QuestradeOptionManager s_questradeOptionManager;

        private static SchedulingManager s_schedulingManager;

        private bool m_questradeSymbolsAreUpdated;

        public bool QuestradeSymbolsAreUpdated
        {
            get => m_questradeSymbolsAreUpdated;
            set => SetProperty(ref m_questradeSymbolsAreUpdated, value);
        }

        public QuestradeSymbolIdManager QuestradeSymbolIdManager
        {
            get => s_questradeSymbolIdManager ?? (s_questradeSymbolIdManager = new QuestradeSymbolIdManager(new QuestradeSymbolsConfig()));
            set => SetProperty(ref s_questradeSymbolIdManager, value);
        }

        public QuestradeSymbolDataManager QuestradeSymbolDataManager
        {
            get => s_questradeSymbolDataManager ?? (s_questradeSymbolDataManager = new QuestradeSymbolDataManager(new QuestradeSymbolDataConfig()));
            set => SetProperty(ref s_questradeSymbolDataManager, value);
        }

        public QuestradeAccountManager QuestradeAccountManager
        {
            get => s_questradeAccountManager ?? (s_questradeAccountManager = new QuestradeAccountManager());
            set => SetProperty(ref s_questradeAccountManager, value);
        }

        public static QuestradeAccountManager GetQuestradeAccountManager()
        {
            return s_questradeAccountManager ?? (s_questradeAccountManager = new QuestradeAccountManager());
        }

        public static QuestradeSymbolDataManager GetQuestradeSymbolDataManager()
        {
            return s_questradeSymbolDataManager ?? (s_questradeSymbolDataManager = new QuestradeSymbolDataManager(new QuestradeSymbolDataConfig()));
        }

        public static QuestradeSymbolIdManager GetQuestradeSymbolIdManager()
        {
            return s_questradeSymbolIdManager ?? (s_questradeSymbolIdManager = new QuestradeSymbolIdManager(new QuestradeSymbolsConfig()));
        }

        public async Task Initialize()
        {
            if (await GetQuestradeAccountManager().TryRefreshAuth())
            {
                QuestradeSymbolsAreUpdated = QuestradeSymbolIdManager.LastUpdated.AddDays(28) > DateTime.Now;
            }
        }

        public static QuestradeOptionManager GetQuestradeOptionManager()
        {
            return s_questradeOptionManager ?? (s_questradeOptionManager = new QuestradeOptionManager(new QuestradeOptionsConfig()));
        }

        public static SchedulingManager GetSchedulingManager()
        {
            return s_schedulingManager ?? (s_schedulingManager = new SchedulingManager());
        }

    }

}