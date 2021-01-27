using System.Linq;
using Stockgaze.Core.Repositories;

namespace Stockgaze.Core.Services
{

    public static class SearchServiceProvider
    {

        private static OptionsSearchService OptionsSearchService { get; set; }

        private static DataSearchService DataSearchService { get; set; }

        private static SymbolSearchService SymbolSearchService { get; set; }

        public static OptionsSearchService GetOptionsSearchService()
        {
            var symbolData = new QuestradeSymbolDataConfig();
            if (symbolData.FileExist)
            {
                symbolData.Load();
            }

            if (OptionsSearchService != null) return OptionsSearchService;
            OptionsSearchService = new OptionsSearchService(GazerController.GetQuestradeAccountManager());
            OptionsSearchService.SetIdsList(symbolData.Data.Where(sd => sd.m_hasOptions).Select(sd => sd.m_symbolId).ToList());

            return OptionsSearchService;
        }

        public static SymbolSearchService GetSymbolSearchService()
        {
            return SymbolSearchService ?? (SymbolSearchService = new SymbolSearchService(GazerController.GetQuestradeAccountManager()));
        }

        public static DataSearchService GetDataSearchService()
        {
            var symbolsConfig = new QuestradeSymbolsConfig();
            if (symbolsConfig.FileExist)
            {
                symbolsConfig.Load();
            }

            if (DataSearchService != null) return DataSearchService;

            DataSearchService = new DataSearchService(GazerController.GetQuestradeAccountManager());
            DataSearchService.SetIdsList(symbolsConfig.Data.Select(es => es.m_symbolId).ToList());
            return DataSearchService;
        }

    }

}