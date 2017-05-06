using MigAz.Core.Interface;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Arm
{

    public class NetworkInterface : INetworkInterface
    {
        private JToken _NetworkInterface;
        private VirtualMachine _VirtualMachine;
        private List<NetworkInterfaceIpConfiguration> _NetworkInterfaceIpConfigurations = new List<NetworkInterfaceIpConfiguration>();

        private NetworkInterface() { }

        public NetworkInterface(JToken networkInterfaceToken)
        {
            _NetworkInterface = networkInterfaceToken;

            foreach (JToken networkInterfaceIpConfigurationToken in _NetworkInterface["properties"]["ipConfigurations"])
            {
                _NetworkInterfaceIpConfigurations.Add(new NetworkInterfaceIpConfiguration(networkInterfaceIpConfigurationToken));
            }
        }

        public string Id => (string)_NetworkInterface["id"];
        public string Name => (string)_NetworkInterface["name"];
        public string Location => (string)_NetworkInterface["location"];
        public string ProvisioningState => (string)_NetworkInterface["properties"]["provisioningState"];
        public string EnableIPForwarding => (string)_NetworkInterface["properties"]["enableIPForwarding"];
        public string EnableAcceleratedNetworking => (string)_NetworkInterface["properties"]["enableAcceleratedNetworking"];
        public Guid ResourceGuid => new Guid((string)_NetworkInterface["properties"]["resourceGuid"]);
        public bool IsPrimary => Convert.ToBoolean((string)_NetworkInterface["properties"]["primary"]);
        public VirtualMachine VirtualMachine
        {
            get { return _VirtualMachine; }
            set { _VirtualMachine = value; }
        }

        public List<NetworkInterfaceIpConfiguration> NetworkInterfaceIpConfigurations
        {
            get { return _NetworkInterfaceIpConfigurations; }
        }

        public NetworkInterfaceIpConfiguration PrimaryIpConfiguration
        {
            get
            {
                foreach (NetworkInterfaceIpConfiguration ipConfiguration in this._NetworkInterfaceIpConfigurations)
                {
                    if (ipConfiguration.IsPrimary)
                        return ipConfiguration;
                }

                return null;
            }
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
