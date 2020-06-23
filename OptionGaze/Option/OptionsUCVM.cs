//  ==========================================================================
//   Code created by Philippe Deslongchamps.
//   For the Stockgaze project.
//  ==========================================================================
using OptionGaze.Services;
using OptionGaze.WPFTools;
using Prism.Mvvm;

namespace OptionGaze.Option
{

    public class OptionsUCVM : BindableBase
    {

        private string m_search;

        public SearchService SearchService;

        public string Search
        {
            get => m_search;
            set => SetProperty(ref m_search, value);
        }

        public BindableCollection<SymbolModel> SearchResults { get; set; }

        public void Initialize()
        {
            SearchService = new SearchService(GazerVM.GetQuestradeAccountManager());
        }

    }

}