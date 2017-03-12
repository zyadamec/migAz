using MigAz.Core.Interface;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MigAz.Azure.Arm
{
    public class ArmVirtualNetwork : Core.ArmTemplate.VirtualNetwork, IVirtualNetwork
    {
        private JToken _VirtualNetwork;
        private List<ISubnet> _Subnets = new List<ISubnet>();

        private ArmVirtualNetwork() : base(Guid.Empty) { }

        public ArmVirtualNetwork(JToken virtualNetwork) : base(Guid.Empty)
        {
            _VirtualNetwork = virtualNetwork;

            var subnets = from vnet in _VirtualNetwork["properties"]["subnets"]
                          select vnet;

            foreach (var subnet in subnets)
            {
                ArmSubnet armSubnet = new ArmSubnet(this, subnet);
                _Subnets.Add(armSubnet);
            }
        }

        public string Name => (string)_VirtualNetwork["name"];
        public string Id => (string)_VirtualNetwork["id"];
        public string Location => (string)_VirtualNetwork["location"];
        public string TargetId => this.Id;
        public override string ToString()
        {
            return this.Name;
        }

        public List<ISubnet> Subnets
        {
            get { return _Subnets; }
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
