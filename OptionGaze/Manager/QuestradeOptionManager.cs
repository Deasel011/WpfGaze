using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OptionGaze.Repositories;
using Prism.Mvvm;
using Questrade.BusinessObjects.Entities;

namespace OptionGaze.Manager
{

    public class QuestradeOptionManager : BindableBase, IQuestradeOptionRepository
    {

        private readonly QuestradeOptionsConfig m_questradeOptionsConfig;

        private bool m_isRefreshing;

        public bool IsRefreshing
        {
            get => m_isRefreshing;
            set => SetProperty(ref m_isRefreshing, value);
        }

        public QuestradeOptionManager(QuestradeOptionsConfig questradeOptionsConfig)
        {
            m_questradeOptionsConfig = questradeOptionsConfig;
            if (m_questradeOptionsConfig.FileExist)
            {
                m_questradeOptionsConfig.Load();
            }
        }

        public DateTime LastUpdated => m_questradeOptionsConfig.LastUpdated;

        public List<(ulong SymbolId, List<ChainPerExpiryDate>)> Data => m_questradeOptionsConfig.Data;

        public async Task Refresh()
        {
            var lastUpdated = m_questradeOptionsConfig.LastUpdated;
            IsRefreshing = true;
            await m_questradeOptionsConfig.Refresh();
            if (!m_questradeOptionsConfig.LastUpdated.Equals(lastUpdated))
            {
                RaisePropertyChanged(nameof(LastUpdated));
            }

            IsRefreshing = false;
        }

    }

}