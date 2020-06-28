//  ==========================================================================
//   Code created by Philippe Deslongchamps.
//   For the Stockgaze project.
//  ==========================================================================
using System;
using System.Threading.Tasks;
using OptionGaze.Login;
using OptionGaze.Repositories;
using OptionGaze.Services;
using OptionGaze.Symbols;
using Prism.Mvvm;

namespace OptionGaze
{

    public class GazerVM : BindableBase //, IDisposable
    {

        private static QuestradeAccountManager s_questradeAccountManager;

        private static QuestradeSymbolsManager s_questradeSymbolsManager;

        private bool m_questradeSymbolsAreUpdated;

        public bool QuestradeSymbolsAreUpdated
        {
            get => m_questradeSymbolsAreUpdated;
            set => SetProperty(ref m_questradeSymbolsAreUpdated, value);
        }

        public QuestradeSymbolsManager QuestradeSymbolsManager
        {
            get => s_questradeSymbolsManager ?? (s_questradeSymbolsManager = new QuestradeSymbolsManager(new QuestradeSymbolsConfig()));
            set => SetProperty(ref s_questradeSymbolsManager, value);
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

        public async Task Initialize()
        {
            if (GetQuestradeAccountManager().TryRefreshAuth())
            {
                QuestradeSymbolsManager.AffectSearchService(new SymbolSearchService(QuestradeAccountManager));
                QuestradeSymbolsAreUpdated = QuestradeSymbolsManager.LastUpdated.AddDays(14) > DateTime.Now;
            }
        }

    }

}