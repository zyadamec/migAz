using Microsoft.IdentityModel.Clients.ActiveDirectory;
using MigAz.Azure;
using MigAz.Core.Interface;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.AzureStack
{
    public class AzureStackContext
    {
        AzureContext _AzureContext;

        private AzureStackContext() { }

        public AzureStackContext(AzureContext azureContext)
        {
            _AzureContext = azureContext;
        }

        public AzureContext AzureContext
        {
            get { return _AzureContext; }
        }

        public async Task Login()
        {
            // AzureStack:  https://docs.microsoft.com/en-us/azure/azure-stack/azure-stack-powershell-configure-admin
            await _AzureContext.Login();
            //_AzureContext.AzureEnvironment = AzureEnvironment.AzureCloud;
            //await _AzureContext.TokenProvider.Login("https://management.core.windows.net/", Microsoft.IdentityModel.Clients.ActiveDirectory.PromptBehavior.Always);
            List<AzureTenant> tenants = await _AzureContext.AzureRetriever.GetAzureARMTenants();
            List<AzureDomain> domains = await _AzureContext.AzureRetriever.GetAzureARMDomains(tenants[0]);
            List<AzureSubscription> subscriptions = await GetAzureStackARMSubscriptions(tenants[0]);
            //UserAuthenticated?.Invoke(this);
        }


        public async Task<List<AzureSubscription>> GetAzureStackARMSubscriptions(AzureTenant azureTenant)
        {
            //_AzureContext.LogProvider.WriteLog("GetAzureARMSubscriptions", "Start - azureTenant: " + azureTenant.ToString());

            String subscriptionsUrl = "https://adminmanagement.local.azurestack.external/" + "subscriptions?api-version=2015-01-01";
            AuthenticationResult authenticationResult = await this._AzureContext.TokenProvider.GetToken("https://adminmanagement.azstackcspmulti1.onmicrosoft.com/3f59c3a8-6c85-479f-a7a7-51ec04e1d05a", azureTenant.TenantId, Microsoft.IdentityModel.Clients.ActiveDirectory.PromptBehavior.Auto);// _AzureContext.AzureServiceUrls.GetARMServiceManagementUrl(), azureTenant.TenantId);

            //_AzureContext.StatusProvider.UpdateStatus("BUSY: Getting Subscriptions...");

            AzureRestRequest azureRestRequest = new AzureRestRequest(subscriptionsUrl, authenticationResult.AccessToken, "GET", false);
            AzureRestResponse azureRestResponse = await _AzureContext.AzureRetriever.GetAzureRestResponse(azureRestRequest);
            JObject subscriptionsJson = JObject.Parse(azureRestResponse.Response);

            var subscriptions = from subscription in subscriptionsJson["value"]
                                select subscription;

            List<AzureSubscription> tenantSubscriptions = new List<AzureSubscription>();

            foreach (JObject azureSubscriptionJson in subscriptions)
            {
                AzureSubscription azureSubscription = new AzureSubscription(this._AzureContext, azureSubscriptionJson, azureTenant, _AzureContext.AzureEnvironment);
                tenantSubscriptions.Add(azureSubscription);
            }

            return tenantSubscriptions;
        }

    }
}
