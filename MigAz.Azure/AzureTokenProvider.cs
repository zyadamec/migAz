using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MigAz.Azure
{
    public class AzureTokenProvider // : ITokenProvider
    {
        private const string strReturnUrl = "urn:ietf:wg:oauth:2.0:oob";
        private const string strClientId = "1950a258-227b-4e31-a9cf-717495945fc2";
        private AuthenticationResult _AuthenticationResult = null;

        private AzureTokenProvider() { }

        internal AzureTokenProvider(AuthenticationResult commonAuthenticationResult)
        {
            _AuthenticationResult = commonAuthenticationResult;
        }

        public AuthenticationResult AuthenticationResult
        {
            get { return _AuthenticationResult; }
        }

        public async Task GetToken(AzureSubscription azureSubscription)
        {
            _AuthenticationResult = null;

            if (azureSubscription == null)
            {
                return;
            }

            AuthenticationContext context = new AuthenticationContext(AzureServiceUrls.GetAzureLoginUrl(azureSubscription.AzureEnvironment) + azureSubscription.AzureAdTenantId.ToString());

            PlatformParameters platformParams = new PlatformParameters(PromptBehavior.Auto, null);
            _AuthenticationResult = await context.AcquireTokenAsync(AzureServiceUrls.GetASMServiceManagementUrl(azureSubscription.AzureEnvironment), strClientId, new Uri(strReturnUrl), platformParams);
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

        internal async static Task<AzureTokenProvider> LoginAzureProvider(AzureEnvironment azureEnvironment)
        {
            AuthenticationContext context = new AuthenticationContext(AzureServiceUrls.GetAzureLoginUrl(azureEnvironment) + "common");

            PlatformParameters platformParams = new PlatformParameters(PromptBehavior.Always, null);
            AuthenticationResult authenticationResult = await context.AcquireTokenAsync(AzureServiceUrls.GetASMServiceManagementUrl(azureEnvironment), strClientId, new Uri(strReturnUrl), platformParams);
            if (authenticationResult == null)
            {
                throw new InvalidOperationException("Failed to obtain the token");
            }

            return new AzureTokenProvider(authenticationResult);
        }

        internal async Task<AuthenticationResult> GetGraphToken(AzureEnvironment azureEnvironment, string tenantId)
        {
            AuthenticationContext context = new AuthenticationContext(AzureServiceUrls.GetAzureLoginUrl(azureEnvironment) + tenantId);

            PlatformParameters platformParams = new PlatformParameters(PromptBehavior.Auto, null);
            AuthenticationResult authenticationResult = await context.AcquireTokenAsync(AzureServiceUrls.GetGraphApiUrl(azureEnvironment), strClientId, new Uri(strReturnUrl), platformParams);
            if (authenticationResult == null)
            {
                throw new InvalidOperationException("Failed to obtain the token");
            }

            return authenticationResult;
        }
        internal async Task<AuthenticationResult> GetAzureToken(AzureEnvironment azureEnvironment, string tenantId)
        {
            AuthenticationContext context = new AuthenticationContext(AzureServiceUrls.GetAzureLoginUrl(azureEnvironment) + tenantId);

            PlatformParameters platformParams = new PlatformParameters(PromptBehavior.Auto, null);
            AuthenticationResult authenticationResult = await context.AcquireTokenAsync(AzureServiceUrls.GetARMServiceManagementUrl(azureEnvironment), strClientId, new Uri(strReturnUrl), platformParams);
            if (authenticationResult == null)
            {
                throw new InvalidOperationException("Failed to obtain the token");
            }

            return authenticationResult;
        }
    }
}
