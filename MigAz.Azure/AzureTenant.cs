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
        private AzureEnvironment _AzureEnvironment;

        internal AzureTenant(JObject tenantsJson, AzureEnvironment azureEnvironment)
        {
            _TenantJson = tenantsJson;
            _AzureEnvironment = azureEnvironment;
        }

        public string Id
        {
            get { return String.Empty; }
        }

        public List<AzureDomain> Domains
        {
            get { return new List<AzureDomain>(); }

        //                        //Gathering the Token for Graph to list the Tenant Information
        //            var token_grpah = GetToken(azureTenant.Id, PromptBehavior.Auto, false, "GraphAuth");

        //var DomDetails = _AzureRetriever.GetAzureARMResources("Domains", null, null, token_grpah, null);
        //var Domresults = JsonConvert.DeserializeObject<dynamic>(DomDetails);


    }
}
}
