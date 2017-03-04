using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure
{
    public class AzureTenant
    {
        private JObject _TenantJson;
        private AzureContext _AzureContext;
        private List<AzureDomain> _Domains;

        internal AzureTenant(JObject tenantsJson, AzureContext azureContext)
        {
            _TenantJson = tenantsJson;
            _AzureContext = azureContext;
        }

        public string Id
        {
            get { return (string)_TenantJson["id"]; }
        }
        public string TenantId
        {
            get { return (string)_TenantJson["tenantId"]; }
        }
        public AzureDomain DefaultDomain
        {
            get
            {
                if (Domains == null)
                    return null;

                foreach (AzureDomain azureDomain in Domains)
                {
                    if (azureDomain.IsDefault)
                        return azureDomain;
                }

                return null;
            }
        }

        public override string ToString()
        {
            if (DefaultDomain == null)
                return TenantId;
            else
                return DefaultDomain.Name + " (" + TenantId + ")";
        }

        public List<AzureDomain> Domains
        {
            get { return _Domains; }

        }

        public async Task InitializeChildren()
        {
            _Domains = await _AzureContext.AzureRetriever.GetAzureARMDomains(TenantId);
        }
    }
}
