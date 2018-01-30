using MigAz.Azure.Interface;
using MigAz.Core;
using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.MigrationTarget
{
    public class NetworkInterfaceIpConfiguration : IVirtualNetworkTarget, IMigrationTarget
    {
        private INetworkInterfaceIpConfiguration _SourceIpConfiguration;
        private string _TargetName = String.Empty;
        private string _TargetNameResult = String.Empty;

        private NetworkInterfaceIpConfiguration() { }

        public NetworkInterfaceIpConfiguration(Azure.Asm.NetworkInterfaceIpConfiguration ipConfiguration, List<VirtualNetwork> virtualNetworks, TargetSettings targetSettings)
        {
            _SourceIpConfiguration = ipConfiguration;

            this.SetTargetName(ipConfiguration.Name, targetSettings);
            this.TargetPrivateIPAllocationMethod = ipConfiguration.PrivateIpAllocationMethod;
            this.TargetPrivateIpAddress = ipConfiguration.PrivateIpAddress;

            #region Attempt to default Target Virtual Network and Target Subnet objects from source names

            this.TargetVirtualNetwork = SeekVirtualNetwork(virtualNetworks, ipConfiguration.VirtualNetworkName);
            if (this.TargetVirtualNetwork != null && this.TargetVirtualNetwork.GetType() == typeof(Azure.MigrationTarget.VirtualNetwork)) // Should only be of this type, as we don't default to another existing ARM VNet (which would be of the base interface type also)
            {
                Azure.MigrationTarget.VirtualNetwork targetVirtualNetwork = (Azure.MigrationTarget.VirtualNetwork)this.TargetVirtualNetwork;
                foreach (Subnet targetSubnet in targetVirtualNetwork.TargetSubnets)
                {
                    if (targetSubnet.SourceName == ipConfiguration.SubnetName)
                    {
                        this.TargetSubnet = targetSubnet;
                        break;
                    }
                }
            }

            #endregion

        }

        public NetworkInterfaceIpConfiguration(Azure.Arm.NetworkInterfaceIpConfiguration ipConfiguration, List<VirtualNetwork> virtualNetworks, TargetSettings targetSettings)
        {
            _SourceIpConfiguration = ipConfiguration;

            #region Attempt to default Target Virtual Network and Target Subnet objects from source names

            this.SetTargetName(ipConfiguration.Name, targetSettings);
            this.TargetPrivateIPAllocationMethod = ipConfiguration.PrivateIpAllocationMethod;
            this.TargetPrivateIpAddress = ipConfiguration.PrivateIpAddress;
            this.TargetVirtualNetwork = SeekVirtualNetwork(virtualNetworks, ipConfiguration.VirtualNetworkName);
            if (this.TargetVirtualNetwork != null && this.TargetVirtualNetwork.GetType() == typeof(Azure.MigrationTarget.VirtualNetwork)) // Should only be of this type, as we don't default to another existing ARM VNet (which would be of the base interface type also)
            {
                Azure.MigrationTarget.VirtualNetwork targetVirtualNetwork = (Azure.MigrationTarget.VirtualNetwork)this.TargetVirtualNetwork;
                foreach (Subnet targetSubnet in targetVirtualNetwork.TargetSubnets)
                {
                    if (targetSubnet.SourceName == ipConfiguration.SubnetName)
                    {
                        this.TargetSubnet = targetSubnet;
                        break;
                    }
                }
            }

            #endregion
            
        }

        private VirtualNetwork SeekVirtualNetwork(List<VirtualNetwork> virtualNetworks, string virtualNetworkName)
        {
            foreach (VirtualNetwork targetVirtualNetwork in virtualNetworks)
            {
                if (targetVirtualNetwork.SourceName == virtualNetworkName)
                    return targetVirtualNetwork;
            }

            return null;
        }

        public INetworkInterfaceIpConfiguration SourceIpConfiguration
        {
            get { return _SourceIpConfiguration; }
        }


        public string SourceName
        {
            get
            {
                if (this.SourceIpConfiguration == null)
                    return String.Empty;

                return this.SourceIpConfiguration.Name;
            }
        }

        public PublicIp TargetPublicIp { get; set; }
        public NetworkSecurityGroup TargetNetworkSecurityGroup { get; set; }

        public string TargetName
        {
            get { return _TargetName; }
        }

        public string TargetNameResult
        {
            get { return _TargetNameResult; }
        }

        public void SetTargetName(string targetName, TargetSettings targetSettings)
        {
            _TargetName = targetName.Trim().Replace(" ", String.Empty);
            _TargetNameResult = _TargetName;
        }

        public override string ToString()
        {
            return this.TargetNameResult;
        }
    }
}
