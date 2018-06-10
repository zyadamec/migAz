using MigAz.Core.Interface;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Arm
{
    public class VirtualNetworkGatewayConnection : ArmResource, IConnection
    {
        private VirtualNetworkGatewayConnection() : base(null, null) { }

        public VirtualNetworkGatewayConnection(AzureSubscription azureSubscription, JToken resourceToken) : base(azureSubscription, resourceToken)
        {
        }

        public string connectionType => (string)ResourceToken["properties"]["connectionType"];
        public string routingWeight => (string)ResourceToken["properties"]["routingWeight"];
        public string enableBgp => (string)ResourceToken["properties"]["enableBgp"];
        public string usePolicyBasedTrafficSelectors => (string)ResourceToken["properties"]["usePolicyBasedTrafficSelectors"];
        //public string ipsecPolicies => (string)ResourceToken["properties"]["ipsecPolicies"];
        public string ingressBytesTransferred => (string)ResourceToken["properties"]["ingressBytesTransferred"];
        public string egressBytesTransferred => (string)ResourceToken["properties"]["egressBytesTransferred"];

        public override string ToString()
        {
            return this.Name;
        }
    }
}
