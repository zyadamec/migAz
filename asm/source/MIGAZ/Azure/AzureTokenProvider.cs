using Microsoft.IdentityModel.Clients.ActiveDirectory;
using MIGAZ.Asm;
using MIGAZ.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MIGAZ.Azure
{
    public class AzureTokenProvider // : ITokenProvider
    {
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

            AuthenticationContext context = new AuthenticationContext(AzureServiceUrls.GetLoginUrl(azureSubscription.AzureEnvironment) + azureSubscription.AzureAdTenantId.ToString());

            PlatformParameters platformParams = new PlatformParameters(PromptBehavior.Auto, null);
            _AuthenticationResult = await context.AcquireTokenAsync(AzureServiceUrls.GetASMServiceManagementUrl(azureSubscription.AzureEnvironment), app.Default.ClientId, new Uri(app.Default.ReturnURL), platformParams);
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
            AuthenticationContext context = new AuthenticationContext(AzureServiceUrls.GetLoginUrl(azureEnvironment) + "common");

            PlatformParameters platformParams = new PlatformParameters(PromptBehavior.Always, null);
            AuthenticationResult authenticationResult = await context.AcquireTokenAsync(AzureServiceUrls.GetASMServiceManagementUrl(azureEnvironment), app.Default.ClientId, new Uri(app.Default.ReturnURL), platformParams);
            if (authenticationResult == null)
            {
                throw new InvalidOperationException("Failed to obtain the token");
            }

            return new AzureTokenProvider(authenticationResult);
        }
    }
}
