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
    public class AzureTokenProvider : ITokenProvider
    {
        private const string strReturnUrl = "urn:ietf:wg:oauth:2.0:oob";
        private const string strClientId = "1950a258-227b-4e31-a9cf-717495945fc2";
        private ILogProvider _LogProvider;
        private string _LogonUrl = String.Empty;
        private AuthenticationContext _AuthenticationContext;
        private Dictionary<Guid, AuthenticationContext> _TenantAuthenticationContext = new Dictionary<Guid, AuthenticationContext>();
        private UserInfo _LastUserInfo;

        private AzureTokenProvider() { }

        public AzureTokenProvider(string logonUrl, ILogProvider logProvider)
        {
            _LogonUrl = logonUrl;
            _AuthenticationContext = new AuthenticationContext(_LogonUrl + "common");
            _LogProvider = logProvider;
        }

        public string LogonUrl
        {
            get { return _LogonUrl; }
        }

        public UserInfo LastUserInfo
        {
            get { return _LastUserInfo; }
        }

        private AuthenticationContext GetTenantAuthenticationContext(Guid tenantGuid)
        {
            if (tenantGuid == Guid.Empty)
                return _AuthenticationContext;

            if (_TenantAuthenticationContext.Keys.Contains(tenantGuid))
                return _TenantAuthenticationContext[tenantGuid];
            else
            {
                AuthenticationContext tenantAuthenticationContext = new AuthenticationContext(_LogonUrl + tenantGuid.ToString() + "/");
                _TenantAuthenticationContext.Add(tenantGuid, tenantAuthenticationContext);
                return tenantAuthenticationContext;
            }
        }

        public async Task<AuthenticationResult> GetToken(string resourceUrl, Guid azureAdTenantGuid, PromptBehavior promptBehavior = PromptBehavior.Auto)
        {
            _LogProvider.WriteLog("GetToken", "Start token request : Azure AD Tenant ID " + azureAdTenantGuid.ToString());
            _LogProvider.WriteLog("GetToken", " - Resource Url: " + resourceUrl);
            _LogProvider.WriteLog("GetToken", " - Azure AD Tenant Guid: " + azureAdTenantGuid.ToString());
            _LogProvider.WriteLog("GetToken", " - Prompt Behavior: " + promptBehavior.ToString());

            AuthenticationContext tenantAuthenticationContext = GetTenantAuthenticationContext(azureAdTenantGuid);
            
            PlatformParameters platformParams = new PlatformParameters(promptBehavior, null);
            AuthenticationResult authenticationResult = await tenantAuthenticationContext.AcquireTokenAsync(resourceUrl, strClientId, new Uri(strReturnUrl), platformParams);

            if (authenticationResult == null)
                _LastUserInfo = null;
            else
                _LastUserInfo = authenticationResult.UserInfo;

            _LogProvider.WriteLog("GetToken", "End token request");

            return authenticationResult;
        }

        public async Task<AuthenticationResult> Login(string resourceUrl, PromptBehavior promptBehavior = PromptBehavior.Always)
        {
            _LogProvider.WriteLog("LoginAzureProvider", "Start token request");
            _LogProvider.WriteLog("GetToken", " - Resource Url: " + resourceUrl);
            _LogProvider.WriteLog("GetToken", " - Prompt Behavior: " + promptBehavior.ToString());

            PlatformParameters platformParams = new PlatformParameters(promptBehavior, null);
            AuthenticationResult authenticationResult = await _AuthenticationContext.AcquireTokenAsync(resourceUrl, strClientId, new Uri(strReturnUrl), platformParams);

            if (authenticationResult == null)
                _LastUserInfo = null;
            else
                _LastUserInfo = authenticationResult.UserInfo;

            _LogProvider.WriteLog("LoginAzureProvider", "End token request");

            return authenticationResult;
        }

        public static bool operator ==(AzureTokenProvider lhs, AzureTokenProvider rhs)
        {
            bool status = false;
            if (((object)lhs == null && (object)rhs == null) ||
                ((object)lhs != null && (object)rhs != null && lhs._LogonUrl == rhs._LogonUrl))
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
