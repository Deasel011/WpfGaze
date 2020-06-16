using Prism.Mvvm;

namespace OptionGaze.Login
{

    public class QuestradeLoginDialogVM: BindableBase
    {

        private string m_refreshToken = PlaceholderValue;

        private bool m_isDemo;

        public const string PlaceholderValue = "paste refresh token in here";

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