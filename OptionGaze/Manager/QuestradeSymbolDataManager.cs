using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OptionGaze.Repositories;
using Prism.Mvvm;
using Questrade.BusinessObjects.Entities;

namespace OptionGaze.Manager
{

    public class QuestradeSymbolDataManager : BindableBase, IQuestradeStockDataRepository
    {

        private readonly QuestradeSymbolDataConfig m_questradeSymbolDataConfig;

        private bool m_isRefreshing;

        public bool IsRefreshing
        {
            get => m_isRefreshing;
            set => SetProperty(ref m_isRefreshing, value);
        }

        public QuestradeSymbolDataManager(QuestradeSymbolDataConfig questradeSymbolDataConfig)
        {
            m_questradeSymbolDataConfig = questradeSymbolDataConfig;
        }

        public DateTime LastUpdated => m_questradeSymbolDataConfig.LastUpdated;

        public List<SymbolData> QuestradeSymbolData => m_questradeSymbolDataConfig.QuestradeSymbolData;

        public async Task Refresh()
        {
            var lastUpdated = m_questradeSymbolDataConfig.LastUpdated;
            IsRefreshing = true;
            await m_questradeSymbolDataConfig.Refresh();
            if (!m_questradeSymbolDataConfig.LastUpdated.Equals(lastUpdated))
            {
                RaisePropertyChanged(nameof(LastUpdated));
            }

            IsRefreshing = false;
        }

    }

}