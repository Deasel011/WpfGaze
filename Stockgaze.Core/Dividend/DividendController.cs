using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Questrade.BusinessObjects.Entities;
using Stockgaze.Core.Helpers;
using Stockgaze.Core.Services;

namespace Stockgaze.Core.Dividend
{
    public class DividendController
    {
        private IEnumerable<SymbolData> m_symbolsWithDividends;
        private QuoteDataSearchService m_quoteDataSearchService;
        
        private QuoteDataSearchService QuoteDataSearchService =>
            m_quoteDataSearchService ?? (m_quoteDataSearchService = new QuoteDataSearchService(GazerController.GetQuestradeAccountManager()));

        public async Task LoadDividends(ProgressReporter progress = null)
        {
            //Get list of symbols that have dividends
            var symbolDataManager = GazerController.GetQuestradeSymbolDataManager();
            m_symbolsWithDividends = symbolDataManager.Data.Where(sd => sd.m_dividend > 0.0 && sd.m_yield < 50.0 && sd.m_exDate > DateTime.Now.Subtract(TimeSpan.FromDays(60))).OrderByDescending(sd => sd.m_yield).Take(200).ToList();
            progress?.SetTotalElements(m_symbolsWithDividends.Count());
            QuoteDataSearchService.SetIdsList(m_symbolsWithDividends.Select(s => s.m_symbolId).ToList());
            var quotes = await QuoteDataSearchService.Search(null, progress);
            quotes.ForEach(quote =>
            {
                m_symbolsWithDividends.Where(o => o.m_symbolId == quote.m_symbolId).ToList().ForEach(o =>
                {
                    o.m_prevDayClosePrice = quote.m_lastTradePrice;
                });
            });
        }

        public List<SymbolData> OrderedDividends => m_symbolsWithDividends.OrderByDescending(d => d.m_yield).ToList();
    }
}