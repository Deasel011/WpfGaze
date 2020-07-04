//  ==========================================================================
//   Code created by Philippe Deslongchamps.
//   For the Stockgaze project.
//  ==========================================================================

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using OptionGaze.Services;
using OptionGaze.WPFTools;
using Prism.Mvvm;
using Questrade.BusinessObjects.Entities;

namespace OptionGaze.Option
{
    public class OptionsUCVM : BindableBase, IDisposable
    {
        public DataSearchService DataSearchService;
        private bool m_isLoading;

        private OptionIdFilterBindableWrapper m_op;

        public OptionDataSearchService OptionDataSearchService;

        public QuoteDataSearchService QuoteDataSearchService;
        
        private bool _searchNasdaq;
        
        public bool SearchNasdaq
        {
            get => _searchNasdaq;
            set => SetProperty(ref _searchNasdaq, value);
        }
        
        private bool _searchTsx;
        
        public bool SearchTsx
        {
            get => _searchTsx;
            set => SetProperty(ref _searchTsx, value);
        }
        
        private bool _searchNyse;
        
        public bool SearchNyse
        {
            get => _searchNyse;
            set => SetProperty(ref _searchNyse, value);
        }

        public bool IsLoading
        {
            get => m_isLoading;
            set => SetProperty(ref m_isLoading, value);
        }

        public BindableCollection<SymbolOptionModel> OptionSymbols { get; set; }

        public OptionIdFilterBindableWrapper OptionIdFilter
        {
            get => m_op;
            set => SetProperty(ref m_op, value);
        }

        public void Dispose()
        {
            m_op.PropertyChanged -= OptionFilterOnPropertyChanged;
        }

        public void Initialize()
        {
            DataSearchService = new DataSearchService(GazerVM.GetQuestradeAccountManager());
            OptionDataSearchService = new OptionDataSearchService(GazerVM.GetQuestradeAccountManager());
            OptionSymbols = new BindableCollection<SymbolOptionModel>();
            QuoteDataSearchService = new QuoteDataSearchService(GazerVM.GetQuestradeAccountManager());
            m_op = new OptionIdFilterBindableWrapper();
            m_op.PropertyChanged += OptionFilterOnPropertyChanged;
        }

        private void OptionFilterOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(OptionIdFilter));
        }

        public async Task LoadOptions(List<ulong> ids)
        {
            IsLoading = true;
            //Get list of symbols with options
            var symbolDataManager = GazerVM.GetQuestradeSymbolDataManager();
            var symbolDataWithOptions = symbolDataManager.Data.Where(sd => sd.m_hasOptions).ToList();

            PopulateGrid(symbolDataWithOptions);
            await HydrateGrid();
            QuoteDataSearchService.SetIdsList(symbolDataWithOptions.Select(s=>s.m_symbolId).ToList());
            var quotes = await QuoteDataSearchService.Search(null);
            quotes.ForEach(quote =>
            {
                OptionSymbols.Where(o=>o.QuestradeSymbolId == quote.m_symbolId).ToList().ForEach(o=>
                {
                    o.StockPrice = quote.m_lastTradePrice;
                    o.Symbol = quote.m_symbol;
                });
            });
            IsLoading = false;
        }

        private async Task HydrateGrid()
        {
            //get calls
            if (OptionIdFilter.OptionType == OptionType.Call || OptionIdFilter.OptionType == OptionType.Undefined)
            {
                var oldValue = OptionIdFilter.OptionType;
                OptionIdFilter.OptionType = OptionType.Call;
                OptionDataSearchService.SetFilter(new List<OptionIdFilter> {OptionIdFilter.GetValue()});
                OptionDataSearchService.SetIdsList(OptionSymbols.Select(os => os.m_callId).ToList());
                var result = await OptionDataSearchService.Search(null);

                foreach (var optionData in result)
                {
                    var symbolOptionModel = OptionSymbols.First(os => os.m_callId == optionData.m_symbolId);

                    var newSymbol = symbolOptionModel.Clone();
                    newSymbol.Symbol = optionData.m_symbol;
                    newSymbol.Volatility = optionData.m_volatility;
                    newSymbol.Vwap = optionData.m_VWAP;
                    newSymbol.OptionPrice = optionData.m_lastTradePrice;
                    newSymbol.OptionType = OptionType.Call;
                    OptionSymbols.Add(newSymbol);
                }

                OptionIdFilter.OptionType = oldValue;
            }

            // TODO get puts

            // TODO remove undefined
            OptionSymbols.Where(os => os.OptionType == OptionType.Undefined).ToList()
                .ForEach(os => OptionSymbols.Remove(os));
        }

        private void PopulateGrid(List<SymbolData> symbolsData)
        {
            //Get list of options
            var optionsDataManager = GazerVM.GetQuestradeOptionManager();
            optionsDataManager.Data.ForEach(optionBySymbol =>
            {
                OptionSymbols.AddRange(optionBySymbol.Item2.SelectMany(chain =>
                {
                    return chain.m_chainPerRoot.Where((cpr, index) => index < 2).SelectMany(chainPerRoot =>
                    {
                        return chainPerRoot.m_chainPerStrikePrice.Where((cpsp, index) => index < 2).Select(
                            chainPerStrikePrice => new SymbolOptionModel
                            {
                                QuestradeSymbolId = optionBySymbol.SymbolId,
                                ExpiryDate = chain.m_expiryDate,
                                Description = chain.m_description,
                                Exchange = chain.m_listingExchange,
                                StrikePrice = chainPerStrikePrice.m_strikePrice,
                                m_callId = chainPerStrikePrice.m_callSymbolId,
                                m_putId = chainPerStrikePrice.m_putSymbolId
                            });
                    });
                }));
            });
        }
    }
}