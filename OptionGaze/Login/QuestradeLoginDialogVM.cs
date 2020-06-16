//  ==========================================================================
//   Code created by Philippe Deslongchamps.
//   For the Stockgaze project.
//  ==========================================================================

using Prism.Mvvm;

namespace OptionGaze.Login
{

    public class QuestradeLoginDialogVM : BindableBase
    {

        public const string PlaceholderValue = "paste refresh token in here";

        private bool m_isDemo;

        private string m_refreshToken = PlaceholderValue;

        public bool IsDemo
        {
            get => m_isDemo;
            set
            {
                if (m_isDemo == value)
                {
                    return;
                }

                RaisePropertyChanged(nameof(IsDemo));
                m_isDemo = value;
            }
        }

        public string RefreshToken
        {
            get => m_refreshToken;
            set
            {
                if (m_refreshToken == value)
                {
                    return;
                }

                RaisePropertyChanged(nameof(RefreshToken));
                m_refreshToken = value;
            }
        }

    }

}