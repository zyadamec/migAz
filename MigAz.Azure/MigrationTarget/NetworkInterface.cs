using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.MigrationTarget
{
    public class NetworkInterface : IMigrationTarget
    {
        private AzureContext _AzureContext;
        private INetworkInterface _SourceNetworkInterface;
        private bool _EnableIPForwarding = false;
        private List<MigrationTarget.NetworkInterfaceIpConfiguration> _TargetNetworkInterfaceIpConfigurations = new List<MigrationTarget.NetworkInterfaceIpConfiguration>();
        private BackEndAddressPool _BackEndAddressPool = null;
        private List<InboundNatRule> _InboundNatRules = new List<InboundNatRule>();
        private string _TargetName = String.Empty;
        private VirtualMachine _ParentVirtualMachine;

        private NetworkInterface() { }

        public NetworkInterface(AzureContext azureContext, Asm.VirtualMachine virtualMachine, Asm.NetworkInterface networkInterface, List<VirtualNetwork> virtualNetworks, List<NetworkSecurityGroup> networkSecurityGroups)
        {
            _AzureContext = azureContext;
            _SourceNetworkInterface = networkInterface;
            this.TargetName = networkInterface.Name;
            this.IsPrimary = networkInterface.IsPrimary;
            this.EnableIPForwarding = networkInterface.EnableIpForwarding;

            foreach (Asm.NetworkInterfaceIpConfiguration asmNetworkInterfaceIpConfiguration in networkInterface.NetworkInterfaceIpConfigurations)
            {
                Azure.MigrationTarget.NetworkInterfaceIpConfiguration migrationNetworkInterfaceIpConfiguration = new Azure.MigrationTarget.NetworkInterfaceIpConfiguration(_AzureContext, asmNetworkInterfaceIpConfiguration, virtualNetworks);
                this.TargetNetworkInterfaceIpConfigurations.Add(migrationNetworkInterfaceIpConfiguration);
            }

            if (virtualMachine.NetworkSecurityGroup != null)
            {
                this.NetworkSecurityGroup = NetworkSecurityGroup.SeekNetworkSecurityGroup(networkSecurityGroups, virtualMachine.NetworkSecurityGroup.ToString());
            }
        }

        public NetworkInterface(AzureContext azureContext, Arm.NetworkInterface networkInterface)
        {
            _AzureContext = azureContext;
            _SourceNetworkInterface = networkInterface;

            this.TargetName = networkInterface.Name;
            this.IsPrimary = networkInterface.IsPrimary;
            this.EnableIPForwarding = networkInterface.EnableIPForwarding;

            foreach (Arm.NetworkInterfaceIpConfiguration armNetworkInterfaceIpConfiguration in networkInterface.NetworkInterfaceIpConfigurations)
            {
                MigrationTarget.NetworkInterfaceIpConfiguration targetNetworkInterfaceIpConfiguration = new NetworkInterfaceIpConfiguration(azureContext, armNetworkInterfaceIpConfiguration, azureContext.AzureSubscription.ArmTargetVirtualNetworks);
                this.TargetNetworkInterfaceIpConfigurations.Add(targetNetworkInterfaceIpConfiguration);
            }

            if (networkInterface.NetworkSecurityGroup != null)
            {
                this.NetworkSecurityGroup = NetworkSecurityGroup.SeekNetworkSecurityGroup(azureContext.AzureSubscription.ArmTargetNetworkSecurityGroups, networkInterface.NetworkSecurityGroup.ToString());
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

        public string TargetName
        {
            get { return _TargetName; }
            set { _TargetName = value.Trim().Replace(" ", String.Empty); }
        }

        public override string ToString()
        {
            return this.TargetName + _AzureContext.TargetSettings.NetworkInterfaceCardSuffix;
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

        public NetworkSecurityGroup TargetNetworkSecurityGroup { get; set; }
    }
}
