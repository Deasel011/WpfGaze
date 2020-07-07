//  ==========================================================================
//   Code created by Philippe Deslongchamps.
//   For the Stockgaze project.
//  ==========================================================================

using System;
using System.IO;
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
            await m_optionsUCVM.LoadOptions();
        }

    }

}