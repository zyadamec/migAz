using Microsoft.IdentityModel.Clients.ActiveDirectory;
using MigAz.Azure.Interface;
using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MigAz.Azure
{
    public class AzureTokenProvider
    {
        private const string strReturnUrl = "urn:ietf:wg:oauth:2.0:oob";
        private const string strClientId = "1950a258-227b-4e31-a9cf-717495945fc2";
        private AuthenticationResult _AuthenticationResult = null;
        private ILogProvider _LogProvider;

        private AzureTokenProvider() { }

        internal AzureTokenProvider(ILogProvider logProvider)
        {
            _LogProvider = logProvider;
        }

        public AuthenticationResult AuthenticationResult
        {
            get { return _AuthenticationResult; }
            set { _AuthenticationResult = value; }
        }

        public string AccessToken
        {
            get
            {
                if (this.AuthenticationResult == null)
                    return String.Empty;

                return this.AuthenticationResult.AccessToken;
            }
        }

        public async Task<AuthenticationResult> GetToken(string loginUrl, string resourceUrl, Guid azureAdTenantGuid, PromptBehavior promptBehavior = PromptBehavior.Auto)
        {
            _LogProvider.WriteLog("GetToken", "Start token request : Azure AD Tenant ID " + azureAdTenantGuid.ToString());
            _LogProvider.WriteLog("GetToken", " - Login Url: " + loginUrl);
            _LogProvider.WriteLog("GetToken", " - Resource Url: " + resourceUrl);
            _LogProvider.WriteLog("GetToken", " - Azure AD Tenant Guid: " + azureAdTenantGuid.ToString());

            string authenticationUrl = loginUrl + azureAdTenantGuid;
            _LogProvider.WriteLog("GetToken", "Authentication Url: " + authenticationUrl);

            _AuthenticationResult = null;
            AuthenticationContext context = new AuthenticationContext(authenticationUrl);

            PlatformParameters platformParams = new PlatformParameters(promptBehavior, null);
            _AuthenticationResult = await context.AcquireTokenAsync(resourceUrl, strClientId, new Uri(strReturnUrl), platformParams);

            _LogProvider.WriteLog("GetToken", "End token request");

            return _AuthenticationResult;
        }

        public async Task<AuthenticationResult> LoginAzureProvider(string loginUrl, string resourceUrl, PromptBehavior promptBehavior = PromptBehavior.Auto)
        {
            _LogProvider.WriteLog("LoginAzureProvider", "Start token request");
            _LogProvider.WriteLog("GetToken", " - Login Url: " + loginUrl);
            _LogProvider.WriteLog("GetToken", " - Resource Url: " + resourceUrl);

            string authenticationUrl = loginUrl + "common";
            _LogProvider.WriteLog("LoginAzureProvider", "Authentication Url: " + authenticationUrl);

            AuthenticationContext context = new AuthenticationContext(authenticationUrl);

            PlatformParameters platformParams = new PlatformParameters(promptBehavior, null);
            AuthenticationResult authenticationResult = await context.AcquireTokenAsync(resourceUrl, strClientId, new Uri(strReturnUrl), platformParams);
            if (authenticationResult == null)
            {
                _LogProvider.WriteLog("LoginAzureProvider", "Failed to obtain the token (null AuthenticationResult returned).");
            }

            _AuthenticationResult = authenticationResult;

            _LogProvider.WriteLog("LoginAzureProvider", "End token request");

            return _AuthenticationResult;
        }

        public static bool operator ==(AzureTokenProvider lhs, AzureTokenProvider rhs)
        {
            bool status = false;
            if (((object)lhs == null && (object)rhs == null) ||
                ((object)lhs != null && (object)rhs != null && lhs.AuthenticationResult == null && rhs.AuthenticationResult == null) ||
                    ((object)lhs != null && (object)rhs != null && lhs.AuthenticationResult == rhs.AuthenticationResult))
            {
                status = true;
            }
            return status;
        }

        public static bool operator !=(AzureTokenProvider lhs, AzureTokenProvider rhs)
        {
            return !(lhs == rhs);
        }
    }
}
