using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Arm
{
    public class BackEndAddressPool : ArmResource
    {
        private LoadBalancer _ParentLoadBalancer = null;
        private List<LoadBalancingRule> _LoadBalancingRules = new List<LoadBalancingRule>();

        public BackEndAddressPool(LoadBalancer loadBalancer, JToken backEndAddressPoolToken) : base(backEndAddressPoolToken)
        {
            _ParentLoadBalancer = loadBalancer;
        }

        internal override async Task InitializeChildrenAsync(AzureContext azureContext)
        {
            // todo now russell asap load "backendIPConfigurations": 
        }

        public LoadBalancer LoadBalancer
        {
            get { return _ParentLoadBalancer; }
        }

        public List<LoadBalancingRule> LoadBalancingRules
        {
            get { return _LoadBalancingRules; }
        }

        internal List<String> LoadBalancingRuleIds
        {
            get
            {
                List<String> loadBalancingRuleIds = new List<string>();

                if (this.ResourceToken["properties"]["loadBalancingRules"] != null)
                {
                    foreach (JToken loadBalancingRuleId in this.ResourceToken["properties"]["loadBalancingRules"])
                    {
                        loadBalancingRuleIds.Add((string)loadBalancingRuleId["id"]);
                    }
                }

                return loadBalancingRuleIds;
            }
        }
    }
}
