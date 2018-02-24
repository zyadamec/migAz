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
    public class AzureStackContext : AzureContext
    {
        AzureStackEndpoints _AzureStackEndpoints;

        private AzureStackContext() : base(null, null) { }

        public AzureStackContext(ILogProvider logProvider, IStatusProvider statusProvider, PromptBehavior defaultPromptBehavior = PromptBehavior.Always) : base(logProvider, statusProvider, defaultPromptBehavior)
        {
        }

        public AzureStackEndpoints AzureStackEndpoints
        {
            get { return _AzureStackEndpoints; }
        }

        public override string GetARMTokenResourceUrl()
        {
            if (this.AzureStackEndpoints == null)
                return String.Empty;
            else
                return this.AzureStackEndpoints.Audiences;
        }

        public override string GetARMServiceManagementUrl()
        {
            return _AzureStackEndpoints.ManagementEndpoint;
            //return _AzureStackEndpoints.PortalEndpoint;  // is this for user subscriptions?
        }

        public async Task Login()
        {
            // AzureStack Login via PowerShell:  https://docs.microsoft.com/en-us/azure/azure-stack/azure-stack-powershell-configure-admin
            await base.Login(_AzureStackEndpoints.LoginEndpoint, _AzureStackEndpoints.Audiences);

            List<AzureTenant> tenants = await this.AzureRetriever.GetAzureARMTenants();
            List<AzureDomain> domains = await this.AzureRetriever.GetAzureARMDomains(tenants[0]);
            List<AzureSubscription> subscriptions = await GetAzureStackARMSubscriptions(tenants[0]);
            //UserAuthenticated?.Invoke(this);
        }


        public async Task<List<AzureSubscription>> GetAzureStackARMSubscriptions(AzureTenant azureTenant)
        {
            //_AzureContext.LogProvider.WriteLog("GetAzureARMSubscriptions", "Start - azureTenant: " + azureTenant.ToString());

            String subscriptionsUrl = this.GetARMServiceManagementUrl() + "subscriptions?api-version=2015-01-01";
            AuthenticationResult authenticationResult = await this.TokenProvider.GetToken(this.GetARMTokenResourceUrl(), azureTenant.TenantId, Microsoft.IdentityModel.Clients.ActiveDirectory.PromptBehavior.Auto);// _AzureContext.AzureServiceUrls.GetARMServiceManagementUrl(), azureTenant.TenantId);

            //_AzureContext.StatusProvider.UpdateStatus("BUSY: Getting Subscriptions...");

            AzureRestRequest azureRestRequest = new AzureRestRequest(subscriptionsUrl, authenticationResult, "GET", false);
            AzureRestResponse azureRestResponse = await this.AzureRetriever.GetAzureRestResponse(azureRestRequest);
            JObject subscriptionsJson = JObject.Parse(azureRestResponse.Response);

            var subscriptions = from subscription in subscriptionsJson["value"]
                                select subscription;

            List<AzureSubscription> tenantSubscriptions = new List<AzureSubscription>();

            foreach (JObject azureSubscriptionJson in subscriptions)
            {
                AzureSubscription azureSubscription = new AzureSubscription(this, azureSubscriptionJson, azureTenant, this.AzureEnvironment);
                tenantSubscriptions.Add(azureSubscription);
            }

            return tenantSubscriptions;
        }

        internal async Task LoadMetadataEndpoints(string azureStackEnvironment)
        {
            string metadataEndpointsUrlBase = azureStackEnvironment;

            if (!metadataEndpointsUrlBase.EndsWith("/"))
                metadataEndpointsUrlBase += "/";

            String metadataEndpointsUrl = metadataEndpointsUrlBase + "metadata/endpoints?api-version=2015-01-01";

            AzureRestRequest azureRestRequest = new AzureRestRequest(metadataEndpointsUrl);
            AzureRestResponse azureRestResponse = await AzureRetriever.GetAzureRestResponse(azureRestRequest);
            _AzureStackEndpoints = new AzureStackEndpoints(metadataEndpointsUrlBase, azureRestResponse);
        }
    }
}
