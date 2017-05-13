using MigAz.Core.ArmTemplate;
using MigAz.Core.Interface;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MigAz.Azure.Arm
{
    public class VirtualNetwork : IVirtualNetwork, IMigrationVirtualNetwork
    {
        private AzureContext _AzureContext;
        private JToken _VirtualNetwork;
        private List<VirtualNetworkGateway> _VirtualNetworkGateways = new List<VirtualNetworkGateway>();
        private List<ISubnet> _Subnets = new List<ISubnet>();
        private List<string> _AddressPrefixes = new List<string>();
        private List<string> _DnsServers = new List<string>();

        private VirtualNetwork() { }

        public VirtualNetwork(AzureContext azureContext, JToken virtualNetwork) 
        {
            _AzureContext = azureContext;
            _VirtualNetwork = virtualNetwork;

            var subnets = from vnet in _VirtualNetwork["properties"]["subnets"]
                          select vnet;

            foreach (var subnet in subnets)
            {
                Subnet armSubnet = new Subnet(_AzureContext, this, subnet);
                _Subnets.Add(armSubnet);
            }

            var addressPrefixes = from vnet in _VirtualNetwork["properties"]["addressSpace"]["addressPrefixes"]
                                  select vnet;

            foreach (var addressPrefix in addressPrefixes)
            {
                _AddressPrefixes.Add(addressPrefix.ToString());
            }

            if (_VirtualNetwork["properties"] != null)
            {
                if (_VirtualNetwork["properties"]["dhcpOptions"] != null)
                {
                    if (_VirtualNetwork["properties"]["dhcpOptions"]["dnsServers"] != null)
                    {
                        var dnsPrefixes = from vnet in _VirtualNetwork["properties"]["dhcpOptions"]["dnsServers"]
                                          select vnet;

                        foreach (var dnsPrefix in dnsPrefixes)
                        {
                            _DnsServers.Add(dnsPrefix.ToString());
                        }
                    }
                }
            }
        }

        public async Task InitializeChildrenAsync()
        {
            this.ResourceGroup = await _AzureContext.AzureRetriever.GetAzureARMResourceGroup(this.Id);

            foreach (Subnet subnet in this.Subnets)
            {
                await subnet.InitializeChildrenAsync();
            }
        }

        public string Name => (string)_VirtualNetwork["name"];
        public string Id => (string)_VirtualNetwork["id"];
        public string Location => (string)_VirtualNetwork["location"];
        public string Type => (string)_VirtualNetwork["type"];
        public ISubnet GatewaySubnet
        {
            get
            {
                foreach (Subnet subnet in this.Subnets)
                {
                    if (subnet.Name == ArmConst.GatewaySubnetName)
                        return subnet;
                }

                return null;
            }
        }
        public List<ISubnet> Subnets => _Subnets;
        public List<string> AddressPrefixes => _AddressPrefixes;
        public List<string> DnsServers => _DnsServers;

        public ResourceGroup ResourceGroup { get; set; }

        public List<VirtualNetworkGateway> VirtualNetworkGateways
        {
            get { return _VirtualNetworkGateways; }
        }

        public override string ToString()
        {
            return this.Name;
        }

        public bool HasNonGatewaySubnet
        {
            get
            {
                if ((this.Subnets.Count() == 0) ||
                    ((this.Subnets.Count() == 1) && (this.Subnets[0].Name == ArmConst.GatewaySubnetName)))
                    return false;

                return true;
            }
        }

        public bool HasGatewaySubnet
        {
            get
            {
                return this.GatewaySubnet != null;
            }
        }

        public static bool operator ==(VirtualNetwork lhs, VirtualNetwork rhs)
        {
            bool status = false;
            if (((object)lhs == null && (object)rhs == null) ||
                    ((object)lhs != null && (object)rhs != null && lhs.Id == rhs.Id))
            {
                status = true;
            }
            return status;
        }

        public static bool operator !=(VirtualNetwork lhs, VirtualNetwork rhs)
        {
            return !(lhs == rhs);
        }
    }
}
