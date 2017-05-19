using MigAz.Core.Interface;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Arm
{
    public class NetworkSecurityGroup : ArmResource, INetworkSecurityGroup
    {
        private List<NetworkSecurityGroupRule> _Rules = new List<NetworkSecurityGroupRule>();

        private NetworkSecurityGroup() : base(null) { }

        public NetworkSecurityGroup(JToken resourceToken) : base(resourceToken)
        {
            foreach (JToken securityRulesToken in ResourceToken["properties"]["securityRules"])
            {
                _Rules.Add(new NetworkSecurityGroupRule(securityRulesToken));
            }
        }

        public List<NetworkSecurityGroupRule> Rules => _Rules;

        public ResourceGroup ResourceGroup { get; set; }

        internal async Task InitializeChildrenAsync(AzureContext azureContext)
        {
            this.ResourceGroup = await azureContext.AzureRetriever.GetAzureARMResourceGroup(this.Id);

            return;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
