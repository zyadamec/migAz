using MigAz.Core.Interface;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MigAz.Azure.Arm
{
    public class VirtualNetwork : Core.ArmTemplate.VirtualNetwork, IVirtualNetwork
    {
        private JToken _VirtualNetwork;
        private List<ISubnet> _Subnets = new List<ISubnet>();
        private List<string> _DnsServers = new List<string>();
        private List<string> _AddressPrefixes = new List<string>();

        private VirtualNetwork() : base(Guid.Empty) { }

        public VirtualNetwork(JToken virtualNetwork) : base(Guid.Empty)
        {
            _VirtualNetwork = virtualNetwork;

            var subnets = from vnet in _VirtualNetwork["properties"]["subnets"]
                          select vnet;

            foreach (var subnet in subnets)
            {
                Subnet armSubnet = new Subnet(this, subnet);
                _Subnets.Add(armSubnet);
            }
        }

        public string Name => (string)_VirtualNetwork["name"];
        public string Id => (string)_VirtualNetwork["id"];
        public string Location => (string)_VirtualNetwork["location"];
        public string TargetId => this.Id;
        public List<ISubnet> Subnets => _Subnets;
        public List<string> DnsServers => _DnsServers;
        public List<string> AddressPrefixes => _AddressPrefixes;

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

    }
}


            //foreach (var addressprefix in resource.properties.addressSpace.addressPrefixes)
            //{
            //    addressprefixes.Add(addressprefix.Value);
            //}

     //if (resource.properties.dhcpOptions != null)
     //       {
     //           foreach (var dnsserver in resource.properties.dhcpOptions.dnsServers)
     //           {
     //               dnsservers.Add(dnsserver.Value);
     //           }
     //       }