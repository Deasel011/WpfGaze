using System.Linq;
using OptionGaze.Repositories;

namespace OptionGaze.Services
{

    public static class SearchServiceProvider
    {

        private static DataSearchService DataSearchService { get; set; }

        private static SymbolSearchService SymbolSearchService { get; set; }

        public static SymbolSearchService GetSymbolSearchService()
        {
            return SymbolSearchService ?? (SymbolSearchService = new SymbolSearchService(GazerVM.GetQuestradeAccountManager()));
        }

        public static DataSearchService GetDataSearchService()
        {
            var symbolsConfig = new QuestradeSymbolsConfig();
            if (symbolsConfig.FileExist)
            {
                symbolsConfig.Load();
            }

            if (DataSearchService != null) return DataSearchService;

            DataSearchService = new DataSearchService(GazerVM.GetQuestradeAccountManager());
            DataSearchService.SetIdsList(symbolsConfig.QuestradeEquitySymbols.Select(es => es.m_symbolId).ToList());
            return DataSearchService;
        }

    }

}