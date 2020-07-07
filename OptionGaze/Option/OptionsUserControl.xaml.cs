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
            try
            {
                InitializeComponent();
                m_optionsUCVM = new OptionsUCVM();
                DataContext = m_optionsUCVM;
                m_optionsUCVM.Initialize();
            }
            catch (Exception e)
            {
                MessageBox.Show($"message:{e.Message} stacktrace:{e.StackTrace}");
            }
        }

        private async void LoadButtonOnClick(object sender, RoutedEventArgs e)
        {
            await m_optionsUCVM.LoadOptions();
        }

    }

}