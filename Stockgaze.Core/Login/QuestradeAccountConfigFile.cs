//  ==========================================================================
//   Code created by Philippe Deslongchamps.
//   For the Stockgaze project.
//  ==========================================================================

using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace Stockgaze.Core.Login
{

    public class QuestradeAccountConfigFile : ConfigFile
    {

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

        [JsonProperty(nameof(AccessToken))]
        private byte[] ProtectedAccessToken { get; set; }

        [JsonProperty(nameof(RefreshToken))]
        private byte[] ProtectedRefreshToken { get; set; }

        public QuestradeAccountConfigFile()
        {
            Filename = $"{nameof(QuestradeAccountConfigFile)}.json";
        }

    } 
    
    public class EmailConfigFile : ConfigFile
    {

        [JsonIgnore]
        public string Password
        {
            get => Encoding.Default.GetString(ProtectedData.Unprotect(ProtectedPassword, null, DataProtectionScope.CurrentUser));
            set => ProtectedPassword = ProtectedData.Protect(Encoding.Default.GetBytes(value), null, DataProtectionScope.CurrentUser);
        }

        [JsonProperty(nameof(IsDemo))]
        public bool IsDemo { get; set; }

        [JsonProperty(nameof(Password))]
        private byte[] ProtectedPassword { get; set; }

        [JsonProperty(nameof(From))]
        public string From { get; set; }        
        
        [JsonProperty(nameof(Username))]
        public string Username { get; set; }
        
        [JsonProperty(nameof(To))]
        public string To { get; set; }
        
        [JsonProperty(nameof(SmtpServer))]
        public string SmtpServer { get; set; }

        [JsonProperty(nameof(Port))]
        public string Port { get; set; }

        [JsonProperty(nameof(Protocol))]
        public string Protocol { get; set; }

        public EmailConfigFile()
        {
            Filename = $"{nameof(EmailConfigFile)}.json";
        }

    }

}