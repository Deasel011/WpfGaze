//  ==========================================================================
//   Code created by Philippe Deslongchamps.
//   For the Stockgaze project.
//  ==========================================================================

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using Prism.Mvvm;
using Questrade.BusinessObjects.Entities;
using Stockgaze.Core.Models;
using Stockgaze.Core.Services;
using Stockgaze.Core.WPFTools;

namespace Stockgaze.Core.Option
{

    public class OptionsController : BindableBase, IDisposable
    {

        private bool _hideStockPricesGreaterThanStrikePrices;

        public bool HideStockPricesGreaterThanStrikePrices
        {
            get => _hideStockPricesGreaterThanStrikePrices;
            set
            {
                SetProperty(ref _hideStockPricesGreaterThanStrikePrices, value);
                SetOptionCollectionView();
            }
        }

        private bool _filterInfiniteReturn;

        private bool _searchNasdaq;

        private bool _searchNyse;

        private bool _searchTsx;

        public DataSearchService DataSearchService;

        private bool m_isLoading;

        private OptionIdFilterBindableWrapper m_op;

        public ListCollectionView OptionCollectionView { get; private set; }

        public OptionDataSearchService OptionDataSearchService;

        public QuoteDataSearchService QuoteDataSearchService;

        private ProgressReporter m_progress;

        private int m_filterMinVolume;
        
        private ProgressReporter m_progress2;

        private TaskScheduler _uiScheduler;

        public int FilterMinVolume
        {
            get => m_filterMinVolume;
            set
            {
                SetProperty(ref m_filterMinVolume, value);
                SetOptionCollectionView();
            }
        }

        public bool FilterInfiniteReturn
        {
            get => _filterInfiniteReturn;
            set
            {
                SetProperty(ref _filterInfiniteReturn, value);
                SetOptionCollectionView();
            }
        }

        public bool SearchNasdaq
        {
            get => _searchNasdaq;
            set => SetProperty(ref _searchNasdaq, value);
        }

        public bool SearchTsx
        {
            get => _searchTsx;
            set => SetProperty(ref _searchTsx, value);
        }

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

        public List<SymbolOptionModel> OptionSymbolsSynchronous { get; set; } = new List<SymbolOptionModel>();

        public OptionIdFilterBindableWrapper OptionIdFilter
        {
            get => m_op;
            set => SetProperty(ref m_op, value);
        }

        public ProgressReporter Progress
        {
            get => m_progress;
            set => SetProperty(ref m_progress, value);
        }
        
        public ProgressReporter Progress2
        {
            get => m_progress2;
            set => SetProperty(ref m_progress2, value);
        }

        public void Dispose()
        {
            OptionSymbols.CollectionChanged -= RefreshCollectionView;
            m_op.PropertyChanged -= OptionFilterOnPropertyChanged;
        }

        public void Initialize()
        {
            DataSearchService = new DataSearchService(GazerController.GetQuestradeAccountManager());
            OptionDataSearchService = new OptionDataSearchService(GazerController.GetQuestradeAccountManager());
            OptionSymbols = new BindableCollection<SymbolOptionModel>();
            OptionSymbolsSynchronous = new List<SymbolOptionModel>();
            QuoteDataSearchService = new QuoteDataSearchService(GazerController.GetQuestradeAccountManager());
            m_op = new OptionIdFilterBindableWrapper();
            m_op.PropertyChanged += OptionFilterOnPropertyChanged;
        }

        private void SetOptionCollectionView()
        {
            OptionCollectionView = new ListCollectionView(runSync?OptionSymbolsSynchronous.ToList():OptionSymbols.ToList()) {
                Filter = filterObject => {
                    var option = filterObject as SymbolOptionModel;

                    bool show = option.Volume >= Convert.ToUInt64(FilterMinVolume);

                    if (FilterInfiniteReturn)
                    {
                        show = show && !double.IsInfinity(option.Return);
                    }

                    if (HideStockPricesGreaterThanStrikePrices)
                    {
                        show = show && option.StockPrice < option.StrikePrice;
                    }

                    return show;
                }
            };
            RaisePropertyChanged(nameof(OptionCollectionView));
        }

        private void RefreshCollectionView(object sender, NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(OptionCollectionView));
        }

        private void OptionFilterOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(OptionIdFilter));
        }

        public async Task LoadOptions()
        {
            IsLoading = true;
            //Get list of symbols with options
            var symbolDataManager = GazerController.GetQuestradeSymbolDataManager();
            var symbolDataWithOptions = symbolDataManager.Data.Where(sd => sd.m_hasOptions && FilterExchange(sd.m_listingExchange)).ToList();
            if (runSync) OptionSymbolsSynchronous.Clear(); else OptionSymbols.Clear();
            Progress2 = new ProgressReporter(symbolDataWithOptions.Count);
            PopulateGrid(symbolDataWithOptions);
            await HydrateGrid();
            QuoteDataSearchService.SetIdsList(symbolDataWithOptions.Select(s => s.m_symbolId).ToList());
            var quotes = await QuoteDataSearchService.Search(null, Progress2);
            quotes.ForEach(quote =>
            {
                if (runSync)
                {
                    OptionSymbolsSynchronous.Where(o => o.QuestradeSymbolId == quote.m_symbolId).ToList().ForEach(o =>
                    {
                        o.StockPrice = quote.m_lastTradePrice;
                        o.Symbol = quote.m_symbol;
                    });
                }else
                    OptionSymbols.Where(o => o.QuestradeSymbolId == quote.m_symbolId).ToList().ForEach(o =>
                {
                    o.StockPrice = quote.m_lastTradePrice;
                    o.Symbol = quote.m_symbol;
                });
            });
            SetOptionCollectionView();
            IsLoading = false;
        }

        public bool runSync { get; set; } = false;

        private bool FilterExchange(string listingExchange)
        {
            switch (listingExchange)
            {
                case "NYSE":
                    return _searchNyse;
                case "TSX":
                    return _searchTsx;
                case "NASDAQ":
                    return _searchNasdaq;
                default:
                    return false;
            }
        }

        private async Task HydrateGrid()
        {
            //get calls
            if (OptionIdFilter.OptionType == OptionType.Call || OptionIdFilter.OptionType == OptionType.Undefined)
            {
                var oldValue = OptionIdFilter.OptionType;
                OptionIdFilter.OptionType = OptionType.Call;
                OptionDataSearchService.SetFilter(new List<OptionIdFilter> {OptionIdFilter.GetValue()});
                var ids = runSync?OptionSymbolsSynchronous.Select(os => os.m_callId).ToList():OptionSymbols.Select(os => os.m_callId).ToList();
                Progress = new ProgressReporter(ids.Count());
                OptionDataSearchService.SetIdsList(ids);
                var result = await OptionDataSearchService.Search(null, Progress);
                
                foreach (var optionData in result)
                {
                    var symbolOptionModel = runSync?OptionSymbolsSynchronous.First(os => os.m_callId == optionData.m_symbolId):OptionSymbols.First(os => os.m_callId == optionData.m_symbolId);

                    var newSymbol = symbolOptionModel.Clone();
                    newSymbol.Symbol = optionData.m_symbol;
                    newSymbol.Volatility = optionData.m_volatility;
                    newSymbol.Vwap = optionData.m_VWAP;
                    newSymbol.OptionPrice = optionData.m_lastTradePrice;
                    newSymbol.OptionType = OptionType.Call;
                    newSymbol.Volume = optionData.m_volume;
                    if(runSync)
                        OptionSymbolsSynchronous.Add(newSymbol);
                    else
                        OptionSymbols.Add(newSymbol);
                }
                OptionIdFilter.OptionType = oldValue;
            }

            // TODO get puts

            // TODO remove undefined
            if (runSync)
                OptionSymbolsSynchronous.Where(os => os.OptionType == OptionType.Undefined)
                             .ToList()
                             .ForEach(os => OptionSymbolsSynchronous.Remove(os));
            else
                OptionSymbols.Where(os => os.OptionType == OptionType.Undefined).ToList()
                .ForEach(os => OptionSymbols.Remove(os));
        }

        private void PopulateGrid(List<SymbolData> symbolsData)
        {
            //Get list of options
            var optionsDataManager = GazerController.GetQuestradeOptionManager();
            optionsDataManager.Data
                .Where(o => symbolsData.Any(s => s.m_symbolId == o.SymbolId)).ToList()
                .ForEach(optionBySymbol =>
                {
                    if (runSync)
                    {
                        OptionSymbolsSynchronous.AddRange(optionBySymbol.Item2.Where(cped => cped.m_expiryDate <= OptionIdFilter.ExpiryDate).SelectMany(chain =>
                        {
                            return chain.m_chainPerRoot.SelectMany(chainPerRoot =>
                            {
                                return chainPerRoot.m_chainPerStrikePrice.Where((cpsp, index) =>
                                    cpsp.m_strikePrice >= OptionIdFilter.MinStrikePrice && cpsp.m_strikePrice <= OptionIdFilter.MaxStrikePrice).Select(
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
                    }else
                        OptionSymbols.AddRange(optionBySymbol.Item2.Where(cped => cped.m_expiryDate <= OptionIdFilter.ExpiryDate).SelectMany(chain =>
                    {
                        return chain.m_chainPerRoot.SelectMany(chainPerRoot =>
                        {
                            return chainPerRoot.m_chainPerStrikePrice.Where((cpsp, index) =>
                                cpsp.m_strikePrice >= OptionIdFilter.MinStrikePrice && cpsp.m_strikePrice <= OptionIdFilter.MaxStrikePrice).Select(
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

        public void ApplyConfig(OptionControllerConfig optionControllerConfig)
        {
            FilterInfiniteReturn = optionControllerConfig.FilterInfiniteReturn;
            FilterMinVolume = optionControllerConfig.FilterMinVolume;
            SearchNasdaq = optionControllerConfig.SearchNasdaq;
            SearchNyse = optionControllerConfig.SearchNyse;
            SearchTsx = optionControllerConfig.SearchTsx;
            OptionIdFilter = new OptionIdFilterBindableWrapper {
                ExpiryDate = optionControllerConfig.ExpiryDate,
                MaxStrikePrice = optionControllerConfig.MaxStrikePrice,
                MinStrikePrice = optionControllerConfig.MinStrikePrice,
                OptionType = OptionType.Call
            };
        }

    }

    public class ProgressReporter: BindableBase
    {

        public ProgressReporter(int totalElements)
        {
            m_totalElements = totalElements;
        }

        private ulong m_progress;

        private readonly int m_totalElements;

        public ulong Progress
        {
            get => m_progress;
            set => SetProperty(ref m_progress, value);
        }

        public void SetProgress(int index)
        {
            try
            {
                Progress = Convert.ToUInt64(Math.Round((double)index / (double)m_totalElements * 100));
            }
            catch (OverflowException e)
            {
                //SUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUP
            }
        }
    }
}