//  ==========================================================================
//   Code created by Philippe Deslongchamps.
//   For the Stockgaze project.
//  ==========================================================================

using System.Windows;

namespace OptionGaze.Login
{

    public partial class QuestradeLogin : Window
    {

        public string ResponseText
        {
            get => RefreshToken.Text;
            set => RefreshToken.Text = value;
        }

        public bool IsDemo
        {
            get => IsDemoChecked.IsChecked ?? false;
            set => IsDemoChecked.IsChecked = value;
        }

        public QuestradeLogin()
        {
            InitializeComponent();
            DataContext = new QuestradeLoginDialogVM();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (ResponseText == QuestradeLoginDialogVM.PlaceholderValue)
            {
                DialogResult = false;
            }
            else
            {
                DialogResult = true;
            }
        }

    }

}