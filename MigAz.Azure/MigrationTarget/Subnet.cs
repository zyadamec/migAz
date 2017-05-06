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
        private ISubnet _SourceSubnet;
        private String _TargetName = String.Empty;
        private MigrationTarget.VirtualNetwork _ParentVirtualNetwork;

        private Subnet() { }

        public Subnet(AzureContext azureContext, MigrationTarget.VirtualNetwork parentVirtualNetwork, ISubnet source)
        {
            _AzureContext = azureContext;
            _ParentVirtualNetwork = parentVirtualNetwork;
            _SourceSubnet = source;

            if (source.GetType() == typeof(Asm.Subnet))
            {
                Asm.Subnet asmSubnet = (Asm.Subnet)source;

                if (asmSubnet.NetworkSecurityGroup != null)
                {
                    this.NetworkSecurityGroup = new NetworkSecurityGroup(azureContext, asmSubnet.NetworkSecurityGroup);
                }

                if (asmSubnet.RouteTable != null)
                {
                    this.RouteTable = new RouteTable(azureContext, asmSubnet.RouteTable);
                }

                this.AddressPrefix = asmSubnet.AddressPrefix;
            }
            else if (source.GetType() == typeof(Arm.Subnet))
            {
                Arm.Subnet armSubnet = (Arm.Subnet)source;

                if (armSubnet.NetworkSecurityGroup != null)
                {
                    this.NetworkSecurityGroup = new NetworkSecurityGroup(azureContext, armSubnet.NetworkSecurityGroup);
                }

                if (armSubnet.RouteTable != null)
                {
                    this.RouteTable = new RouteTable(azureContext, armSubnet.RouteTable);
                }

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

        public ISubnet SourceSubnet
        {
            get { return _SourceSubnet; }
        }

        public String SourceName
        {
            get
            {
                if (this.SourceSubnet == null)
                    return String.Empty;
                else
                    return this.SourceSubnet.ToString();
            }
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
