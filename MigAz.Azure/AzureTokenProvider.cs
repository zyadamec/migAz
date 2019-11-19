// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.IdentityModel.Clients.ActiveDirectory;
using MigAz.Azure.Interface;
using MigAz.Azure.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using MigAz.Azure.ADAL;
using Microsoft.Identity.Client;

namespace MigAz.Azure
{
    public class AzureTokenProvider : ITokenProvider
    {
        private const string strReturnUrl = "urn:ietf:wg:oauth:2.0:oob";
        private const string strClientId = "1950a258-227b-4e31-a9cf-717495945fc2";
        private ILogProvider _LogProvider;
        private AzureEnvironment _AzureEnvironment;
        private AuthenticationContext _AuthenticationContext;
        private Dictionary<Guid, AuthenticationContext> _TenantAuthenticationContext = new Dictionary<Guid, AuthenticationContext>();
        private UserInfo _LastUserInfo;

        private AzureTokenProvider() { }

        public AzureTokenProvider(AzureEnvironment azureEnvironment, ILogProvider logProvider)
        {
            _AzureEnvironment = azureEnvironment;
            _AuthenticationContext = new AuthenticationContext(_AzureEnvironment.ActiveDirectoryEndpoint + _AzureEnvironment.AdTenant);
            _LogProvider = logProvider;
        }

        public AzureEnvironment AzureEnvironment
        {
            get { return _AzureEnvironment; }
        }

        public UserInfo LastUserInfo
        {
            get { return _LastUserInfo; }
            set { _LastUserInfo = value; }
        }

        private AuthenticationContext GetTenantAuthenticationContext(Guid tenantGuid)
        {
            if (tenantGuid == Guid.Empty)
                return _AuthenticationContext;

            if (_TenantAuthenticationContext.Keys.Contains(tenantGuid))
                return _TenantAuthenticationContext[tenantGuid];
            else
            {
                AuthenticationContext tenantAuthenticationContext = new AuthenticationContext(_AzureEnvironment.ActiveDirectoryEndpoint + tenantGuid.ToString() + "/");
                _TenantAuthenticationContext.Add(tenantGuid, tenantAuthenticationContext);
                return tenantAuthenticationContext;
            }
        }

        public async Task<Microsoft.IdentityModel.Clients.ActiveDirectory.AuthenticationResult> GetToken(string resourceUrl, Guid azureAdTenantGuid, PromptBehavior promptBehavior = PromptBehavior.Auto)
        {
            _LogProvider.WriteLog("GetToken", "Start token request : Azure AD Tenant ID " + azureAdTenantGuid.ToString());
            _LogProvider.WriteLog("GetToken", " - Resource Url: " + resourceUrl);
            _LogProvider.WriteLog("GetToken", " - Azure AD Tenant Guid: " + azureAdTenantGuid.ToString());
            _LogProvider.WriteLog("GetToken", " - Prompt Behavior: " + promptBehavior.ToString());
            if (_LastUserInfo == null)
            {
                _LogProvider.WriteLog("GetToken", " - Required User: N/A");
            }
            else
            {
                _LogProvider.WriteLog("GetToken", " - Required User: " + _LastUserInfo.DisplayableId);
            }

            AuthenticationContext tenantAuthenticationContext = GetTenantAuthenticationContext(azureAdTenantGuid);

            PlatformParameters platformParams = new PlatformParameters(promptBehavior, null);
            Microsoft.IdentityModel.Clients.ActiveDirectory.AuthenticationResult authenticationResult;

            if (_LastUserInfo == null)
            {
                authenticationResult = await tenantAuthenticationContext.AcquireTokenAsync(resourceUrl, strClientId, new Uri(strReturnUrl), platformParams);
            }
            else
            {
                UserIdentifier userIdentifier = new UserIdentifier(_LastUserInfo.DisplayableId, UserIdentifierType.RequiredDisplayableId);
                authenticationResult = await tenantAuthenticationContext.AcquireTokenAsync(resourceUrl, strClientId, new Uri(strReturnUrl), platformParams, userIdentifier);
            }

            if (authenticationResult == null)
                _LastUserInfo = null;
            else
                _LastUserInfo = authenticationResult.UserInfo;

            _LogProvider.WriteLog("GetToken", "End token request");

            return authenticationResult;
        }

        public async Task<Microsoft.IdentityModel.Clients.ActiveDirectory.AuthenticationResult> Login(string resourceUrl, PromptBehavior promptBehavior = PromptBehavior.Always)
        {
            _LogProvider.WriteLog("LoginAzureProvider", "Start token request");
            _LogProvider.WriteLog("GetToken", " - Resource Url: " + resourceUrl);
            _LogProvider.WriteLog("GetToken", " - Prompt Behavior: " + promptBehavior.ToString());

            // todonowasap working here, null not valid, requires custom web ui
            // https://github.com/AzureAD/microsoft-authentication-library-for-dotnet/wiki/MSAL.NET-3-released
            // https://github.com/AzureAD/microsoft-authentication-library-for-dotnet/wiki/MSAL.NET-3-released#more-extensibility
            // https://github.com/AzureAD/microsoft-authentication-library-for-dotnet/wiki/CustomWebUi
            // http://thewindowsupdate.com/2019/04/14/how-to-use-active-directory-authentication-library-adal-for-net-on-net-core-3-0-wpf-apps/
            // https://aka.ms/msal-net-3x
            // https://github.com/AzureAD/microsoft-authentication-library-for-dotnet/wiki/Client-Applications


            IPublicClientApplication app = PublicClientApplicationBuilder.Create(strClientId)
                .WithRedirectUri("http://localhost")
                .Build();
            string[] scopes = new string[] { "user.read" };
            Microsoft.Identity.Client.AuthenticationResult result = await app
                .AcquireTokenInteractive(scopes)
                .WithUseEmbeddedWebView(false)
                .ExecuteAsync();
            //DefaultOsBrowserWebUi defaultOsBrowserWebUi = new DefaultOsBrowserWebUi();

            //PlatformParameters platformParams = new PlatformParameters(promptBehavior, defaultOsBrowserWebUi);
            //AuthenticationResult authenticationResult = await _AuthenticationContext.AcquireTokenAsync(resourceUrl, strClientId, new Uri(strReturnUrl), platformParams);

            //if (authenticationResult == null)
            //    _LastUserInfo = null;
            //else
            //    _LastUserInfo = authenticationResult.UserInfo;

            _LogProvider.WriteLog("LoginAzureProvider", "End token request");

            //return authenticationResult;
            return null;
        }

        public static bool operator ==(AzureTokenProvider lhs, AzureTokenProvider rhs)
        {
            bool status = false;
            if (((object)lhs == null && (object)rhs == null) ||
                ((object)lhs != null && (object)rhs != null && lhs.AzureEnvironment == rhs.AzureEnvironment))
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

