using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using Prism.Mvvm;
using Questrade.BusinessObjects.Entities;
using Stockgaze.Core.Dividend;
using Stockgaze.Core.Helpers;
using Stockgaze.Core.Models;

namespace OptionGaze.Dividend
{
    public class DividendViewModel: BindableBase
    {
        private bool m_isLoading;
        private ProgressReporter m_progress;
        public ListCollectionView DividendCollectionView { get; private set; }
        public bool IsLoading
        {
            get => m_isLoading;
            set => SetProperty(ref m_isLoading, value);
        }

        public DividendViewModel()
        {
            DividendController = new DividendController();
        }

        public async Task LoadSymbolsWithDividends()
        {
            Progress = new ProgressReporter(0);
            IsLoading = true;
            await DividendController.LoadDividends(Progress);
            DividendCollectionView = new ListCollectionView(OrderedDividends);
            RaisePropertyChanged(nameof(DividendCollectionView));
            IsLoading = false;
        }

        private List<DividendModel> OrderedDividends => DividendController.OrderedDividends.Select(d => new DividendModel
        {
            Symbol = d.m_symbol,
            Exchange = d.m_listingExchange,
            Description = d.m_description,
            Dividend = d.m_dividend,
            QuestradeSymbolId = d.m_symbolId,
            StockPrice = d.m_prevDayClosePrice,
            DividendDate = d.m_dividendDate,
            ExDividendDate = d.m_exDate
        }).ToList();
        
        private DividendController DividendController { get; }

        public ProgressReporter Progress
        {
            get => m_progress;
            set => SetProperty(ref m_progress, value);
        }
    }
}