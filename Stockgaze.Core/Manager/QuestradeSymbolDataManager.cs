using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prism.Mvvm;
using Questrade.BusinessObjects.Entities;
using Stockgaze.Core.Repositories;

namespace Stockgaze.Core.Manager
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

        public bool HasData => Data.Any();

        public QuestradeSymbolDataManager(QuestradeSymbolDataConfig questradeSymbolDataConfig)
        {
            m_questradeSymbolDataConfig = questradeSymbolDataConfig;
            if (m_questradeSymbolDataConfig.FileExist)
            {
                m_questradeSymbolDataConfig.Load().Wait();
            }
        }

        public DateTime LastUpdated => m_questradeSymbolDataConfig.LastUpdated;

        public List<SymbolData> Data => m_questradeSymbolDataConfig.Data;

        public async Task Refresh()
        {
            var lastUpdated = m_questradeSymbolDataConfig.LastUpdated;
            IsRefreshing = true;
            await m_questradeSymbolDataConfig.Refresh();
            if (!m_questradeSymbolDataConfig.LastUpdated.Equals(lastUpdated))
            {
                RaisePropertyChanged(nameof(LastUpdated));
                RaisePropertyChanged(nameof(HasData));
            }

            IsRefreshing = false;
        }

    }

}