//  ==========================================================================
//  Copyright (C) 2020 by Genetec, Inc.
//  All rights reserved.
//  May be used only in accordance with a valid Source Code License Agreement.
//  ==========================================================================

using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace OptionGaze.Login
{

    public class QuestradeAccountConfigFile : ConfigFile
    {
        [JsonProperty(nameof(AccessToken))]
        private byte[] ProtectedAccessToken { get; set; }

        [JsonProperty(nameof(RefreshToken))]
        private byte[] ProtectedRefreshToken { get; set; }

        [JsonIgnore]
        public string AccessToken
        {
            get => Encoding.Default.GetString(ProtectedData.Unprotect(ProtectedAccessToken, null, DataProtectionScope.CurrentUser));
            set => ProtectedAccessToken = ProtectedData.Protect(Encoding.Default.GetBytes(value), null, DataProtectionScope.CurrentUser);
        }

        [JsonIgnore]
        public string RefreshToken
        {
            get => Encoding.Default.GetString(ProtectedData.Unprotect(ProtectedRefreshToken, null, DataProtectionScope.CurrentUser));
            set => ProtectedRefreshToken = ProtectedData.Protect(Encoding.Default.GetBytes(value), null, DataProtectionScope.CurrentUser);
        }

        [JsonProperty(nameof(IsDemo))]
        public bool IsDemo { get; set; }

    }

}