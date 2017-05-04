using MigAz.Core.ArmTemplate;
using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.MigrationTarget
{
    public class Subnet : IMigrationTarget, IMigrationSubnet
    {
        private AzureContext _AzureContext = null;
        private ISubnet _Source;
        private String _TargetName = String.Empty;
        private MigrationTarget.VirtualNetwork _ParentVirtualNetwork;

        private Subnet() { }

        public Subnet(AzureContext azureContext, MigrationTarget.VirtualNetwork parentVirtualNetwork, ISubnet source)
        {
            _AzureContext = azureContext;
            _ParentVirtualNetwork = parentVirtualNetwork;
            _Source = source;

            //todo now russell, construct NSG and RouteTable
            if (source.GetType() == typeof(Asm.Subnet))
            {
                Asm.Subnet asmSubnet = (Asm.Subnet)source;
                this.AddressPrefix = asmSubnet.AddressPrefix;
            }
            else if (source.GetType() == typeof(Arm.Subnet))
            {
                Arm.Subnet armSubnet = (Arm.Subnet)source;
                this.AddressPrefix = armSubnet.AddressPrefix;
            }

            this.TargetName = source.Name;
        }

        public String AddressPrefix { get; set; }

        public String TargetName
        {
            get { return _TargetName; }
            set { _TargetName = value.Replace(" ", String.Empty).Trim(); }
        }

        public ISubnet Source
        {
            get { return _Source; }
        }

        public MigrationTarget.VirtualNetwork ParentVirtualNetwork
        {
            get { return _ParentVirtualNetwork; }
        }

        public override string ToString()
        {
            return this.TargetName;
        }

        public string TargetId
        {
            get { return "[concat(" + ArmConst.ResourceGroupId + ", '" + ArmConst.ProviderVirtualNetwork + this.ParentVirtualNetwork.ToString() + "/subnets/" + this.TargetName + "')]"; }
        }

        public RouteTable RouteTable { get; set;  }
        public NetworkSecurityGroup NetworkSecurityGroup { get; set; }

        public bool IsGatewaySubnet
        {
            get { return this.TargetName == ArmConst.GatewaySubnetName; }
        }
    }
}
