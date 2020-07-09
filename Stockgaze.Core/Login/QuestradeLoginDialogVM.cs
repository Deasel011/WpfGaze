//  ==========================================================================
//   Code created by Philippe Deslongchamps.
//   For the Stockgaze project.
//  ==========================================================================

using Prism.Mvvm;

namespace Stockgaze.Core.Login
{

    public class QuestradeLoginDialogVM : BindableBase
    {

        public const string PlaceholderValue = "paste refresh token in here";

        private bool m_isDemo;

        private string m_refreshToken = PlaceholderValue;

        public bool IsDemo
        {
            get => m_isDemo;
            set => SetProperty(ref m_isDemo, value);
        }

        public string RefreshToken
        {
            get => m_refreshToken;
            set => SetProperty(ref m_refreshToken, value);
        }

    }

}