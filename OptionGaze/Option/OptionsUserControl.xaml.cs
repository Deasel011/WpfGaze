//  ==========================================================================
//   Code created by Philippe Deslongchamps.
//   For the Stockgaze project.
//  ==========================================================================

using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Stockgaze.Core.Option;

namespace OptionGaze.Option
{

    public partial class OptionsUserControl : UserControl
    {

        public OptionsController OptionsController { get; }

        public OptionsUserControl()
        {
            try
            {
                InitializeComponent();
                OptionsController = new OptionsController();
                DataContext = OptionsController;
                OptionsController.Initialize();
            }
            catch (Exception e)
            {
                MessageBox.Show($"message:{e.Message} stacktrace:{e.StackTrace}");
            }
        }

        private async void LoadButtonOnClick(object sender, RoutedEventArgs e)
        {
            await OptionsController.LoadOptions();
        }

    }

}