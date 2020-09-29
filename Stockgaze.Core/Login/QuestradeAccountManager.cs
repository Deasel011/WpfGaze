//  ==========================================================================
//   Code created by Philippe Deslongchamps.
//   For the Stockgaze project.
//  ==========================================================================

using System;
using System.Threading;
using System.Threading.Tasks;
using Prism.Mvvm;
using QuestradeAPI;

namespace Stockgaze.Core.Login
{

    public class QuestradeAccountManager : BindableBase
    {

        public QuestradeAccountManager()
        {
            m_semaphoreSlim = new SemaphoreSlim(1);
        }

        private SemaphoreSlim m_semaphoreSlim;
        
        private AuthenticationInfoImplementation m_authInfo;

        public string AccessToken => AuthInfo.AccessToken;

        public bool IsConnected => AuthInfo?.IsAuthenticated ?? false;

        public AuthenticationInfoImplementation GetAuthInfo => AuthInfo;

        private AuthenticationInfoImplementation AuthInfo
        {
            get => m_authInfo;
            set

            {
                SetProperty(ref m_authInfo, value);
                RaisePropertyChanged(nameof(IsConnected));
            }
        }

        public async Task<bool> TryRefreshAuth()
        {
            try
            {
                await m_semaphoreSlim.WaitAsync();

                var config = new QuestradeAccountConfigFile();

                if (!config.FileExist)
                {
                    return false;
                }

                await config.Load();
                if (!string.IsNullOrEmpty(config.AccessToken) && !string.IsNullOrEmpty(config.RefreshToken) && AuthAgent.GetInstance().Authenticate(config.RefreshToken, config.IsDemo) is AuthenticationInfoImplementation success && success.IsAuthenticated)
                {
                    await RefreshAndSaveConfigFile(config, success);
                    AuthInfo = success;
                    return true;
                }

                return false;
            }
            finally
            {
                m_semaphoreSlim.Release();
            }
        }

        public async Task Login(string token, bool isDemo = true)
        {
            var questradeAccountConfigFile = new QuestradeAccountConfigFile();
            await questradeAccountConfigFile.Load();
            questradeAccountConfigFile.RefreshToken = token;

            var success = AuthAgent.GetInstance().Authenticate(token, isDemo);
            if (!success.IsValid)
            {
                throw new Exception($"{success.AuthError.ErrorCode} - {success.AuthError.ErrorMessage}");
            }

            await RefreshAndSaveConfigFile(questradeAccountConfigFile, success);
            AuthInfo = success;
        }

        private Task RefreshAndSaveConfigFile(QuestradeAccountConfigFile questradeAccountConfigFile, AuthenticationInfoImplementation success)
        {
            questradeAccountConfigFile.AccessToken = success.AccessToken;
            questradeAccountConfigFile.RefreshToken = success.RefreshToken;
            questradeAccountConfigFile.IsDemo = success.IsDemo;
            return questradeAccountConfigFile.Save();
        }

    }

}