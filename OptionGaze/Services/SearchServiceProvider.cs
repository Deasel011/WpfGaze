using System.Linq;
using OptionGaze.Repositories;

namespace OptionGaze.Services
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
            OptionsSearchService = new OptionsSearchService(GazerVM.GetQuestradeAccountManager());
            OptionsSearchService.SetIdsList(symbolData.Data.Where(sd => sd.m_hasOptions).Select(sd => sd.m_symbolId).ToList());

            return OptionsSearchService;
        }

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
            DataSearchService.SetIdsList(symbolsConfig.Data.Select(es => es.m_symbolId).ToList());
            return DataSearchService;
        }

    }

}