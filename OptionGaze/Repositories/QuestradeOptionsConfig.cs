using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OptionGaze.Services;
using Questrade.BusinessObjects.Entities;

namespace OptionGaze.Repositories
{

    public class QuestradeOptionsConfig : ConfigFile, IQuestradeOptionRepository
    {

        private OptionsSearchService m_optionsSearchService;

        public QuestradeOptionsConfig()
        {
            Filename = $"{nameof(QuestradeOptionsConfig)}.json";
        }

        public QuestradeOptionsConfig(OptionsSearchService optionsSearchService)
        {
            m_optionsSearchService = optionsSearchService;
        }

        public DateTime LastUpdated { get; set; } = DateTime.MinValue;

        public List<(ulong SymbolId, List<ChainPerExpiryDate>)> Data { get; set; }

        public async Task Refresh()
        {
            if (m_optionsSearchService == null)
            {
                m_optionsSearchService = SearchServiceProvider.GetOptionsSearchService();
            }

            var symbols = await m_optionsSearchService.Search(string.Empty);
            Data = new List<(ulong SymbolId, List<ChainPerExpiryDate>)>(symbols);
            LastUpdated = DateTime.Now;
            await Save();
        }

    }

}