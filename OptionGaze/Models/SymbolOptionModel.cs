//  ==========================================================================
//   Code created by Philippe Deslongchamps.
//   For the Stockgaze project.
//  ==========================================================================

using System;
using System.Collections.Generic;
using Prism.Mvvm;
using Questrade.BusinessObjects.Entities;

namespace OptionGaze.Option
{

    public class SymbolOptionModel : BindableBase
    {

        private string m_description;

        private string m_exchange;

        private DateTime m_expiryDate;

        private double m_optionPrice;

        private OptionType m_optionType;

        private ulong m_questradeSymbolId;

        private double m_stockPrice;

        private double m_strikePrice;

        private string m_symbol;

        private double m_volatility;

        private double m_vwap;

        public ulong m_callId;

        public ulong m_putId;

        public static IEqualityComparer<SymbolOptionModel> QuestradeSymbolIdComparer { get; } = new QuestradeSymbolIdEqualityComparer();

        public string Symbol
        {
            get => m_symbol;
            set => SetProperty(ref m_symbol, value);
        }

        public string Description
        {
            get => m_description;
            set => SetProperty(ref m_description, value);
        }

        public string Exchange
        {
            get => m_exchange;
            set => SetProperty(ref m_exchange, value);
        }

        public ulong QuestradeSymbolId
        {
            get => m_questradeSymbolId;
            set => SetProperty(ref m_questradeSymbolId, value);
        }

        public double StockPrice
        {
            get => m_stockPrice;
            set
            {
                SetProperty(ref m_stockPrice, value);
                RaisePropertyChanged(nameof(Return));
            }
        }

        public double StrikePrice
        {
            get => m_strikePrice;
            set => SetProperty(ref m_strikePrice, value);
        }

        public DateTime ExpiryDate
        {
            get => m_expiryDate;
            set => SetProperty(ref m_expiryDate, value);
        }

        public double OptionPrice
        {
            get => m_optionPrice;
            set
            {
                SetProperty(ref m_optionPrice, value);
                RaisePropertyChanged(nameof(Return));
            }
        }

        public OptionType OptionType
        {
            get => m_optionType;
            set => SetProperty(ref m_optionType, value);
        }

        public double Volatility
        {
            get => m_volatility;
            set => SetProperty(ref m_volatility, value);
        }

        public double Vwap
        {
            get => m_vwap;
            set => SetProperty(ref m_vwap, value);
        }

        public double Return => OptionPrice / StockPrice;


        private sealed class QuestradeSymbolIdEqualityComparer : IEqualityComparer<SymbolOptionModel>
        {

            public bool Equals(SymbolOptionModel x, SymbolOptionModel y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;

                return x.m_questradeSymbolId == y.m_questradeSymbolId;
            }

            public int GetHashCode(SymbolOptionModel obj)
            {
                return obj.m_questradeSymbolId.GetHashCode();
            }

        }

        public SymbolOptionModel Clone()
        {
            return new SymbolOptionModel
            {
                m_description = m_description,

                m_exchange = m_exchange,

                m_expiryDate = m_expiryDate,

                m_optionPrice = m_optionPrice,

                m_optionType = m_optionType,

                m_questradeSymbolId = m_questradeSymbolId,

                m_stockPrice = m_stockPrice,

                m_strikePrice = m_strikePrice,

                m_symbol = m_symbol,

                m_volatility = m_volatility,

                m_vwap = m_vwap
            };
        }

    }

}