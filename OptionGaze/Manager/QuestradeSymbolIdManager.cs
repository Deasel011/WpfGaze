//  ==========================================================================
//   Code created by Philippe Deslongchamps.
//   For the Stockgaze project.
//  ==========================================================================

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OptionGaze.Repositories;
using Prism.Mvvm;
using Questrade.BusinessObjects.Entities;

namespace OptionGaze.Manager
{

    public class QuestradeSymbolIdManager : BindableBase, IQuestradeStockIdRepository
    {

        private readonly QuestradeSymbolsConfig m_questradeSymbolsConfig;

        private bool m_isRefreshing;

        public bool IsRefreshing
        {
            get => m_isRefreshing;
            set => SetProperty(ref m_isRefreshing, value);
        }

        public QuestradeSymbolIdManager(QuestradeSymbolsConfig questradeSymbolsConfig)
        {
            m_questradeSymbolsConfig = questradeSymbolsConfig;
            if (m_questradeSymbolsConfig.FileExist)
            {
                m_questradeSymbolsConfig.Load();
            }
        }

        public DateTime LastUpdated => m_questradeSymbolsConfig.LastUpdated;

        public List<EquitySymbol> QuestradeEquitySymbols => m_questradeSymbolsConfig.QuestradeEquitySymbols;

        public async Task Refresh()
        {
            var lastUpdated = m_questradeSymbolsConfig.LastUpdated;
            IsRefreshing = true;
            await m_questradeSymbolsConfig.Refresh();
            if (!m_questradeSymbolsConfig.LastUpdated.Equals(lastUpdated))
            {
                RaisePropertyChanged(nameof(LastUpdated));
            }

            IsRefreshing = false;
        }

    }

}