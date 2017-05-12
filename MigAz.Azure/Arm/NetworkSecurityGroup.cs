using MigAz.Core.Interface;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Arm
{
    public class NetworkSecurityGroup  : INetworkSecurityGroup
    {
        private JToken _NetworkSecurityGroup;
        private List<NetworkSecurityGroupRule> _Rules = new List<NetworkSecurityGroupRule>();

        private NetworkSecurityGroup() { }

        public NetworkSecurityGroup(JToken networkSecurityGroupToken)
        {
            _NetworkSecurityGroup = networkSecurityGroupToken;

            // todo now asap, need to get Network Security Group Rules here
            //foreach (JToken networkInterfaceIpConfigurationToken in _NetworkInterface["properties"]["ipConfigurations"])
            //{
            //    _NetworkInterfaceIpConfigurations.Add(new NetworkInterfaceIpConfiguration(networkInterfaceIpConfigurationToken));
            //}
        }

        public string Id => (string)_NetworkSecurityGroup["id"];

        public string Name => (string)_NetworkSecurityGroup["name"];

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
