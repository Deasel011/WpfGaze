using System.Windows;
using System.Windows.Controls;

namespace OptionGaze.Dividend
{
    public partial class DividendUserControl : UserControl
    {
        public DividendViewModel DividendViewModel { get; }
        
        public DividendUserControl()
        {
            InitializeComponent();
            DividendViewModel = new DividendViewModel();
            DataContext = DividendViewModel;
        }

        private async void LoadButtonOnClick(object sender, RoutedEventArgs e)
        {
            await DividendViewModel.LoadSymbolsWithDividends();
        }
    }
}