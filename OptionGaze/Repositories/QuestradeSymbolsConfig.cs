//  ==========================================================================
//   Code created by Philippe Deslongchamps.
//   For the Stockgaze project.
//  ==========================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OptionGaze.Services;
using Questrade.BusinessObjects.Entities;

namespace OptionGaze.Repositories
{

    public interface IQuestradeStockIdRepository
    {

        DateTime LastUpdated { get; }

        List<EquitySymbol> QuestradeEquitySymbols { get; }

        Task Refresh();

    }

    public class QuestradeSymbolsConfig : ConfigFile, IQuestradeStockIdRepository
    {

        private SymbolSearchService m_symbolSearchService;

        public QuestradeSymbolsConfig()
        {
            Filename = $"{nameof(QuestradeSymbolsConfig)}.json";
        }

        public QuestradeSymbolsConfig(SymbolSearchService symbolSearchService) : this()
        {
            m_symbolSearchService = symbolSearchService;
        }

        public DateTime LastUpdated { get; set; } = DateTime.MinValue;

        public List<EquitySymbol> QuestradeEquitySymbols { get; set; }

        public async Task Refresh()
        {
            if (m_symbolSearchService == null)
            {
                throw new MissingMemberException("Search Service has not been affected or is null. Please affect a search service to the repository.");
            }

            var symbols = await m_symbolSearchService.Search(string.Empty);
            QuestradeEquitySymbols = new List<EquitySymbol>(symbols.Where(s => s.m_isQuotable && s.m_isTradable));
            LastUpdated = DateTime.Now;
            await Save();
        }

        public void AffectSearchService(SymbolSearchService symbolSearchService)
        {
            m_symbolSearchService = symbolSearchService;
        }

    }

}