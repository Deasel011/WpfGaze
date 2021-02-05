using System;
using Prism.Mvvm;

namespace Stockgaze.Core.Models
{
    public class DividendModel : BindableBase
    {
        private double m_dividend;
        private string m_symbol;
        private string m_description;
        private string m_exchange;
        private ulong m_questradeSymbolId;
        private double m_stockPrice;
        private DateTime m_dividendDate;
        private DateTime m_exDividendDate;

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
                RaisePropertyChanged(nameof(Yield));
            }
        }        
        
        public double Dividend
        {
            get => m_dividend;
            set
            {
                SetProperty(ref m_dividend, value);
                RaisePropertyChanged(nameof(Yield));
            }
        }

        public double Yield => Math.Round(Dividend / StockPrice * 100, 2);

        public DateTime DividendDate
        {
            get => m_dividendDate;
            set => SetProperty(ref m_dividendDate, value);
        }

        public DateTime ExDividendDate
        {
            get => m_exDividendDate;
            set => SetProperty(ref m_exDividendDate, value);
        }
    }
}