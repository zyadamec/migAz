using MigAz.Core.ArmTemplate;
using MigAz.Core.Interface;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MigAz.Azure.Arm
{
    public class VirtualNetwork : IVirtualNetwork
    {
        private JToken _VirtualNetwork;
        private List<ISubnet> _Subnets = new List<ISubnet>();
        private List<string> _DnsServers = new List<string>();
        private List<string> _AddressPrefixes = new List<string>();
        private List<string> _DnsPrefixes = new List<string>();

        private VirtualNetwork() { }

        public VirtualNetwork(JToken virtualNetwork) 
        {
            _VirtualNetwork = virtualNetwork;

            var subnets = from vnet in _VirtualNetwork["properties"]["subnets"]
                          select vnet;

            foreach (var subnet in subnets)
            {
                Subnet armSubnet = new Subnet(this, subnet);
                _Subnets.Add(armSubnet);
            }

            var addressPrefixes = from vnet in _VirtualNetwork["properties"]["addressSpace"]["addressPrefixes"]
                                  select vnet;

            foreach (var addressPrefix in addressPrefixes)
            {
                _AddressPrefixes.Add(addressPrefix.ToString());
            }

            //var dnsPrefixes = from vnet in _VirtualNetwork["properties"]["dhcpOptions"]["dnsServers"]
            //                      select vnet;

            //foreach (var dnsPrefix in dnsPrefixes)
            //{
            //    _DnsPrefixes.Add(dnsPrefix.ToString());
            //}
        }

        public string Name => (string)_VirtualNetwork["name"];
        public string Id => (string)_VirtualNetwork["id"];
        public string Location => (string)_VirtualNetwork["location"];
        public string Type => (string)_VirtualNetwork["type"];
        public List<ISubnet> Subnets => _Subnets;
        public List<string> DnsServers => _DnsServers;
        public List<string> AddressPrefixes => _AddressPrefixes;
        public List<string> DnsPrefixes => _DnsPrefixes;

        public ResourceGroup ResourceGroup { get; set; }

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