//  ==========================================================================
//   Code created by Philippe Deslongchamps.
//   For the Stockgaze project.
//  ==========================================================================

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using OptionGaze.Services;
using OptionGaze.WPFTools;
using Prism.Mvvm;

namespace OptionGaze.Option
{

    public class OptionsUCVM : BindableBase
    {

        public DataSearchService DataSearchService;


        public BindableCollection<SymbolOptionModel> OptionSymbols { get; set; }

        public void Initialize()
        {
            DataSearchService = new DataSearchService(GazerVM.GetQuestradeAccountManager());
            OptionSymbols = new BindableCollection<SymbolOptionModel>();
        }

        public async Task LoadOptions(List<ulong> ids)
        {
            //Get list of symbols with options
            DataSearchService.SetIdsList(ids);

            var symbolsData = await DataSearchService.Search(null);
            OptionSymbols.Clear();
            OptionSymbols.AddRange(symbolsData.Where(sd => sd.m_hasOptions).Select(sd => new SymbolOptionModel
            {
                QuestradeSymbolId = sd.m_symbolId,
                Description = sd.m_description,
                Symbol = sd.m_symbol,
                Exchange = sd.m_listingExchange,
                ExpiryDate = sd.m_optionExpiryDate,
                StrikePrice = sd.m_optionStrikePrice,
                OptionPrice = 0,
                OptionType = sd.m_optionType,
                StockPrice = 0
            }));
        }

    }

}