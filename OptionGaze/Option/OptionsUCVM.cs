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

        private string m_search;

        public DataSearchService DataSearchService;

        public string Search
        {
            get => m_search;
            set => SetProperty(ref m_search, value);
        }
        
        public BindableCollection<SymbolModel> Options { get; set; }

        public void Initialize()
        {
            DataSearchService = new DataSearchService(GazerVM.GetQuestradeAccountManager());
            Options = new BindableCollection<SymbolModel>();
        }

        public async Task LoadOptions(List<ulong> ids)
        {
            //Get list of symbols with options
            DataSearchService.SetIdsList(ids);

            var symbolsData = await DataSearchService.Search(null);
            Options.Clear();
            Options.AddRange(symbolsData.Where(sd => sd.m_hasOptions).Select(sd => new SymbolModel
            {
                QuestradeSymbolId = sd.m_symbolId, Description = sd.m_description, Symbol = sd.m_symbol
            }));

        }

    }

}