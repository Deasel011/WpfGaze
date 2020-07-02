//  ==========================================================================
//   Code created by Philippe Deslongchamps.
//   For the Stockgaze project.
//  ==========================================================================

using System.Linq;
using System.Windows;
using System.Windows.Controls;
using OptionGaze.Repositories;

namespace OptionGaze.Option
{

    public partial class OptionsUserControl : UserControl
    {

        public OptionsUCVM m_optionsUCVM { get; set; }

        public OptionsUserControl()
        {
            InitializeComponent();
            m_optionsUCVM = new OptionsUCVM();
            DataContext = m_optionsUCVM;
            m_optionsUCVM.Initialize();
        }

        private async void LoadButtonOnClick(object sender, RoutedEventArgs e)
        {
            var symbolsConfig = new QuestradeSymbolsConfig();
            if (symbolsConfig.FileExist)
            {
                await symbolsConfig.Load();
            }

            await m_optionsUCVM.LoadOptions(symbolsConfig.Data.Select(es => es.m_symbolId).ToList());
        }

    }

}