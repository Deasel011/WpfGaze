//  ==========================================================================
//   Code created by Philippe Deslongchamps.
//   For the Stockgaze project.
//  ==========================================================================
using System.Collections.Generic;
using Prism.Mvvm;

namespace OptionGaze.Option
{

    public class SymbolModel : BindableBase
    {

        private string m_description;

        private string m_exchange;

        private ulong m_questradeSymbolId;

        private string m_symbol;

        private double m_stockPrice;

        private double m_strikePrice;

        public static IEqualityComparer<SymbolModel> QuestradeSymbolIdComparer { get; } = new QuestradeSymbolIdEqualityComparer();

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
            set => SetProperty(ref m_stockPrice, value);
        }

        public double StrikePrice
        {
            get => m_strikePrice;
            set => SetProperty(ref m_strikePrice, value);
        }
        
        

        private sealed class QuestradeSymbolIdEqualityComparer : IEqualityComparer<SymbolModel>
        {

            public bool Equals(SymbolModel x, SymbolModel y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;

                return x.m_questradeSymbolId == y.m_questradeSymbolId;
            }

            public int GetHashCode(SymbolModel obj)
            {
                return obj.m_questradeSymbolId.GetHashCode();
            }

        }

    }

}