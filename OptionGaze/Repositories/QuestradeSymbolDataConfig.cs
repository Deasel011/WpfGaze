using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OptionGaze.Services;
using Questrade.BusinessObjects.Entities;

namespace OptionGaze.Repositories
{

    public class QuestradeSymbolDataConfig : ConfigFile, IQuestradeStockDataRepository
    {

        private DataSearchService m_dataSearchService;

        public QuestradeSymbolDataConfig()
        {
            Filename = $"{nameof(QuestradeSymbolDataConfig)}.json";
        }

        public QuestradeSymbolDataConfig(DataSearchService dataSearchService) : this()
        {
            m_dataSearchService = dataSearchService;
        }

        public DateTime LastUpdated { get; set; } = DateTime.MinValue;

        public List<SymbolData> Data { get; set; }

        public async Task Refresh()
        {
            if (m_dataSearchService == null)
            {
                m_dataSearchService = SearchServiceProvider.GetDataSearchService();
            }

            var symbols = await m_dataSearchService.Search(string.Empty);
            Data = new List<SymbolData>(symbols.Where(s => s.m_isQuotable && s.m_isTradable));
            LastUpdated = DateTime.Now;
            await Save();
        }

    }

}