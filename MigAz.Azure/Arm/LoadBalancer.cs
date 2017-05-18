using MigAz.Core.Interface;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Arm
{
    public class LoadBalancer : ILoadBalancer
    {
        private JToken _LoadBalancerToken;

        public LoadBalancer(JToken jToken)
        {
            this._LoadBalancerToken = jToken;
        }

        public string Id => (string)_LoadBalancerToken["id"];

        public string Name => (string)_LoadBalancerToken["name"];

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
