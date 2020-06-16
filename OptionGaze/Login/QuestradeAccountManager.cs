//  ==========================================================================
//  Copyright (C) 2020 by Genetec, Inc.
//  All rights reserved.
//  May be used only in accordance with a valid Source Code License Agreement.
//  ==========================================================================

using System;
using Prism.Mvvm;
using QuestradeAPI;

namespace OptionGaze.Login
{

    public class QuestradeAccountManager: BindableBase
    {

        private AuthenticationInfoImplementation m_authInfo;

        public bool TryRefreshAuth()
        {
            var config = new QuestradeAccountConfigFile();
            
            if (!config.FileExist)
            {
                return false;
            }
            
            config.Load();
            if (!string.IsNullOrEmpty(config.AccessToken) && !string.IsNullOrEmpty(config.RefreshToken) &&
                AuthAgent.GetInstance().Authenticate(config.RefreshToken, config.IsDemo) is AuthenticationInfoImplementation success && success.IsAuthenticated)
            {
                RefreshAndSaveConfigFile(config, success);
                AuthInfo = success;
                return true;
            }

            return false;
        }

        private AuthenticationInfoImplementation AuthInfo
        {
            get => m_authInfo;
            set
            {
                if (m_authInfo == value)
                {
                    return;
                }
                m_authInfo = value;
                RaisePropertyChanged(nameof(IsConnected));
            }
        }

        public string AccessToken => AuthInfo.AccessToken;

        public bool IsConnected => AuthInfo?.IsAuthenticated ?? false;
        
        public void Login(string token, bool isDemo = true)
        {
            QuestradeAccountConfigFile questradeAccountConfigFile = new QuestradeAccountConfigFile();
            questradeAccountConfigFile.Load();
            questradeAccountConfigFile.RefreshToken = token;
            
            var success = AuthAgent.GetInstance().Authenticate(token, isDemo);
            if (!success.IsValid)
            {
                throw new Exception($"{success.AuthError.ErrorCode} - {success.AuthError.ErrorMessage}");
            }

            RefreshAndSaveConfigFile(questradeAccountConfigFile, success);
            AuthInfo = success;
        }

        private void RefreshAndSaveConfigFile(QuestradeAccountConfigFile questradeAccountConfigFile, AuthenticationInfoImplementation success)
        {
            questradeAccountConfigFile.AccessToken = success.AccessToken;
            questradeAccountConfigFile.RefreshToken = success.RefreshToken;
            questradeAccountConfigFile.IsDemo = success.IsDemo;
            questradeAccountConfigFile.Save();
        }

    }

}