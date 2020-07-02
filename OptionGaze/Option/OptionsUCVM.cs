//  ==========================================================================
//   Code created by Philippe Deslongchamps.
//   For the Stockgaze project.
//  ==========================================================================

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
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

        public void Dispose()
        {
            m_op.PropertyChanged -= OptionFilterOnPropertyChanged;
        }

        public DataSearchService DataSearchService;

        public OptionDataSearchService OptionDataSearchService;
        public BindableCollection<SymbolOptionModel> OptionSymbols { get; set; }

        private OptionIdFilterBindableWrapper m_op;

        public OptionIdFilterBindableWrapper OptionIdFilter
        {
            get => m_op;
            set => SetProperty(ref m_op, value);
        }

        public void Initialize()
        {
            DataSearchService = new DataSearchService(GazerVM.GetQuestradeAccountManager());
            OptionDataSearchService = new OptionDataSearchService(GazerVM.GetQuestradeAccountManager());
            OptionSymbols = new BindableCollection<SymbolOptionModel>();
            m_op = new OptionIdFilterBindableWrapper();
            m_op.PropertyChanged += OptionFilterOnPropertyChanged;
        }

        private void OptionFilterOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(OptionIdFilter));
        }

        public async Task LoadOptions(List<ulong> ids)
        {
            //Get list of symbols with options
            var symbolDataManager = GazerVM.GetQuestradeSymbolDataManager();
            var symbolDataWithOptions = symbolDataManager.Data.Where(sd => sd.m_hasOptions).ToList();
            PopulateGrid(symbolDataWithOptions);
            await HydrateGrid();
        }

        private async Task HydrateGrid()
        {
            //get calls
            if (OptionIdFilter.OptionType == OptionType.Call || OptionIdFilter.OptionType == OptionType.Undefined)
            {
                var oldValue = OptionIdFilter.OptionType;
                OptionIdFilter.OptionType = OptionType.Call;
                OptionDataSearchService.SetFilter(new List<OptionIdFilter>{OptionIdFilter.GetValue()});
                OptionDataSearchService.SetIdsList(OptionSymbols.Select(os => os.m_callId).ToList());
                var result = await OptionDataSearchService.Search(null);
                
                foreach (var optionData in result)
                {
                    var symbolOptionModel = OptionSymbols.First(os => os.m_callId == optionData.m_symbolId);

                    var newSymbol = symbolOptionModel.Clone();
                    newSymbol.Volatility = optionData.m_volatility;
                    newSymbol.Vwap = optionData.m_VWAP;
                    newSymbol.OptionPrice = optionData.m_askPrice;
                    newSymbol.OptionType = OptionType.Call;
                    OptionSymbols.Add(newSymbol);
                }

                OptionIdFilter.OptionType = oldValue;
            }
            
            // TODO get puts
            
            // TODO remove undefined
            OptionSymbols.Where(os=>os.OptionType == OptionType.Undefined).ToList().ForEach(os=>OptionSymbols.Remove(os));
        }

        private void PopulateGrid(List<SymbolData> symbolsData)
        {
            
            //Get list of options
            var optionsDataManager = GazerVM.GetQuestradeOptionManager();
            optionsDataManager.Data.ForEach(optionBySymbol =>
            {
                OptionSymbols.AddRange(optionBySymbol.Item2.SelectMany(chain =>
                {
                    return chain.m_chainPerRoot.Where((cpr,index)=>index < 2).SelectMany(chainPerRoot=>
                    {
                        return chainPerRoot.m_chainPerStrikePrice.Where((cpsp,index)=>index < 2).Select(chainPerStrikePrice => new SymbolOptionModel
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
            
            // var symbolDataWithHydratedOptions = new List<SymbolOptionModel>();
            // symbolDataWithHydratedOptions.AddRange(symbolsData.Where(sd => sd.m_hasOptions).Select(sd => new SymbolOptionModel
            // {
            //     QuestradeSymbolId = sd.m_symbolId,
            //     Description = sd.m_description,
            //     Symbol = sd.m_symbol,
            //     Exchange = sd.m_listingExchange,
            //     ExpiryDate = sd.m_optionExpiryDate,
            //     StrikePrice = sd.m_optionStrikePrice,
            //     OptionPrice = 0,
            //     OptionType = sd.m_optionType,
            //     StockPrice = 0
            // }));
        }

    }

}