using MigAz.Core;
using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.MigrationTarget
{
    public class NetworkInterface : Core.MigrationTarget
    {
        private INetworkInterface _SourceNetworkInterface;
        private bool _EnableIPForwarding = false;
        private List<MigrationTarget.NetworkInterfaceIpConfiguration> _TargetNetworkInterfaceIpConfigurations = new List<MigrationTarget.NetworkInterfaceIpConfiguration>();
        private BackEndAddressPool _BackEndAddressPool = null;
        private List<InboundNatRule> _InboundNatRules = new List<InboundNatRule>();
        private VirtualMachine _ParentVirtualMachine;

        private NetworkInterface() { }

        public NetworkInterface(Asm.VirtualMachine virtualMachine, Asm.NetworkInterface networkInterface, List<VirtualNetwork> virtualNetworks, List<NetworkSecurityGroup> networkSecurityGroups, TargetSettings targetSettings)
        {
            _SourceNetworkInterface = networkInterface;
            this.SetTargetName(networkInterface.Name, targetSettings);
            this.IsPrimary = networkInterface.IsPrimary;
            this.EnableIPForwarding = networkInterface.EnableIpForwarding;

            foreach (Asm.NetworkInterfaceIpConfiguration asmNetworkInterfaceIpConfiguration in networkInterface.NetworkInterfaceIpConfigurations)
            {
                NetworkInterfaceIpConfiguration migrationNetworkInterfaceIpConfiguration = new NetworkInterfaceIpConfiguration(asmNetworkInterfaceIpConfiguration, virtualNetworks, targetSettings);
                this.TargetNetworkInterfaceIpConfigurations.Add(migrationNetworkInterfaceIpConfiguration);
            }

            if (virtualMachine.NetworkSecurityGroup != null)
            {
                this.NetworkSecurityGroup = NetworkSecurityGroup.SeekNetworkSecurityGroup(networkSecurityGroups, virtualMachine.NetworkSecurityGroup.ToString());
            }
        }

        public NetworkInterface(Arm.NetworkInterface networkInterface, List<MigrationTarget.VirtualNetwork> armVirtualNetworks, List<MigrationTarget.NetworkSecurityGroup> armNetworkSecurityGroups, TargetSettings targetSettings)
        {
            _SourceNetworkInterface = networkInterface;
            this.SetTargetName(networkInterface.Name, targetSettings);
            this.IsPrimary = networkInterface.IsPrimary;
            this.EnableIPForwarding = networkInterface.EnableIPForwarding;

            foreach (Arm.NetworkInterfaceIpConfiguration armNetworkInterfaceIpConfiguration in networkInterface.NetworkInterfaceIpConfigurations)
            {
                NetworkInterfaceIpConfiguration targetNetworkInterfaceIpConfiguration = new NetworkInterfaceIpConfiguration(armNetworkInterfaceIpConfiguration, armVirtualNetworks, targetSettings);
                this.TargetNetworkInterfaceIpConfigurations.Add(targetNetworkInterfaceIpConfiguration);
            }

            if (networkInterface.NetworkSecurityGroup != null)
            {
                this.NetworkSecurityGroup = NetworkSecurityGroup.SeekNetworkSecurityGroup(armNetworkSecurityGroups, networkInterface.NetworkSecurityGroup.ToString());
            }
        }

        public List<MigrationTarget.NetworkInterfaceIpConfiguration> TargetNetworkInterfaceIpConfigurations
        {
            get { return _TargetNetworkInterfaceIpConfigurations; }
        }

        public bool EnableIPForwarding
        {
            get { return _EnableIPForwarding; }
            set { _EnableIPForwarding = value; }
        }

        public VirtualMachine ParentVirtualMachine
        {
            get { return _ParentVirtualMachine; }
            set { _ParentVirtualMachine = value; }
        }

        public bool IsPrimary { get; set; }

        public NetworkSecurityGroup NetworkSecurityGroup
        {
            get; set;
        }

        public List<InboundNatRule> InboundNatRules
        {
            get { return _InboundNatRules; }
        }

        public BackEndAddressPool BackEndAddressPool
        {
            get { return _BackEndAddressPool; }
            set { _BackEndAddressPool = value; }
        }

        public INetworkInterface SourceNetworkInterface
        {
            get { return _SourceNetworkInterface; }
        }

        public String SourceName
        {
            get
            {
                if (this.SourceNetworkInterface == null)
                    return String.Empty;
                else
                    return this.SourceNetworkInterface.ToString();
            }
        }

        public override string ImageKey { get { return "NetworkInterface"; } }

        public override string FriendlyObjectName { get { return "Network Interface"; } }


        public NetworkSecurityGroup TargetNetworkSecurityGroup { get; set; }

        public override void SetTargetName(string targetName, TargetSettings targetSettings)
        {
            this.TargetName = targetName.Trim().Replace(" ", String.Empty);
            this.TargetNameResult = this.TargetName + targetSettings.NetworkInterfaceCardSuffix;
        }

    }
}
