using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Arm
{
    public class NetworkInterfaceIpConfiguration
    {
        private JToken _NetworkInterfaceIpConfiguration;
        private VirtualNetwork _VirtualNetwork;
        private Subnet _Subnet;

        public NetworkInterfaceIpConfiguration(JToken networkInterfaceIpConfiguration)
        {
            _NetworkInterfaceIpConfiguration = networkInterfaceIpConfiguration;
        }

        public string Id => (string)_NetworkInterfaceIpConfiguration["id"];
        public string Name => (string)_NetworkInterfaceIpConfiguration["name"];
        public string ProvisioningState => (string)_NetworkInterfaceIpConfiguration["properties"]["provisioningState"];
        public string PrivateIpAddress => (string)_NetworkInterfaceIpConfiguration["properties"]["privateIPAddress"];
        public string PrivateIpAddressVersion => (string)_NetworkInterfaceIpConfiguration["properties"]["privateIPAddressVersion"];
        public string PrivateIpAllocationMethod => (string)_NetworkInterfaceIpConfiguration["properties"]["privateIPAllocationMethod"];
        public bool IsPrimary => Convert.ToBoolean((string)_NetworkInterfaceIpConfiguration["properties"]["primary"]);
        public string SubnetId => (string)_NetworkInterfaceIpConfiguration["properties"]["subnet"]["id"];
        public string PublicIpAddressId => (string)_NetworkInterfaceIpConfiguration["properties"]["publicIPAddress"]["id"];
        public VirtualNetwork VirtualNetwork => _VirtualNetwork;
        public Subnet Subnet => _Subnet;

    }
}
