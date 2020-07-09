using System;
using Prism.Mvvm;
using Questrade.BusinessObjects.Entities;

namespace Stockgaze.Core.Option
{

    public class OptionIdFilterBindableWrapper : BindableBase
    {

        private readonly OptionIdFilter m_optionIdFilter;

        public DateTime ExpiryDate
        {
            get => m_optionIdFilter.m_expiryDate;
            set => SetProperty(ref m_optionIdFilter.m_expiryDate, value);
        }

        public double MaxStrikePrice
        {
            get => m_optionIdFilter.m_maxstrikePrice;
            set => SetProperty(ref m_optionIdFilter.m_maxstrikePrice, value);
        }

        public double MinStrikePrice
        {
            get => m_optionIdFilter.m_minstrikePrice;
            set => SetProperty(ref m_optionIdFilter.m_minstrikePrice, value);
        }

        public OptionType OptionType
        {
            get => m_optionIdFilter.m_optionType;
            set => SetProperty(ref m_optionIdFilter.m_optionType, value);
        }

        public OptionIdFilterBindableWrapper()
        {
            m_optionIdFilter = new OptionIdFilter
                {m_expiryDate = DateTime.Now.Add(TimeSpan.FromDays(14)), m_maxstrikePrice = 999.99, m_minstrikePrice = 0.01};
        }

        public OptionIdFilter GetValue()
        {
            return m_optionIdFilter;
        }

    }

}