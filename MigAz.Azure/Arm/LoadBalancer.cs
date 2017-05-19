using MigAz.Core.Interface;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Arm
{
    public class LoadBalancer : ArmResource, ILoadBalancer
    {
        public LoadBalancer(JToken resourceToken) : base(resourceToken)
        {
        }

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
