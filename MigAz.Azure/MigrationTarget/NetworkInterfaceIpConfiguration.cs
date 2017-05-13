using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.MigrationTarget
{
    public class NetworkInterfaceIpConfiguration : IMigrationTarget
    {
        private AzureContext _AzureContext;
        private INetworkInterfaceIpConfiguration _SourceIpConfiguration;
        private string _TargetName = String.Empty;
        private IMigrationVirtualNetwork _TargetVirtualNetwork;
        private IMigrationSubnet _TargetSubnet;
        private String _TargetPrivateIPAllocationMethod = "Dynamic";
        private String _TargetStaticIpAddress = String.Empty;
        private bool _TargetEnableIPForwarding = false;

        public NetworkInterfaceIpConfiguration()
        {
            this.TargetName = "ipconfig1";
        }

        public NetworkInterfaceIpConfiguration(AzureContext azureContext, Azure.Asm.NetworkInterfaceIpConfiguration ipConfiguration)
        {
            _AzureContext = azureContext;
            _SourceIpConfiguration = ipConfiguration;

            this.TargetName = ipConfiguration.Name;
            this.TargetPrivateIPAllocationMethod = ipConfiguration.PrivateIpAllocationMethod;
            this.TargetPrivateIpAddress = ipConfiguration.PrivateIpAddress;
            // todo now asap, this needs to populate other default property values
        }

        public NetworkInterfaceIpConfiguration(AzureContext azureContext, Azure.Arm.NetworkInterfaceIpConfiguration ipConfiguration)
        {
            _AzureContext = azureContext;
            _SourceIpConfiguration = ipConfiguration;

            this.TargetName = ipConfiguration.Name;
            this.TargetPrivateIPAllocationMethod = ipConfiguration.PrivateIpAllocationMethod;
            this.TargetPrivateIpAddress = ipConfiguration.PrivateIpAddress;
            // todo now asap, this needs to populate other default property values
        }

        public INetworkInterfaceIpConfiguration SourceIpConfiguration
        {
            get { return _SourceIpConfiguration; }
        }

        public IMigrationSubnet TargetSubnet
        {
            get { return _TargetSubnet; }
            set { _TargetSubnet = value; }
        }
        public IMigrationVirtualNetwork TargetVirtualNetwork
        {
            get { return _TargetVirtualNetwork; }
            set { _TargetVirtualNetwork = value; }
        }

        public String TargetPrivateIPAllocationMethod
        {
            get { return _TargetPrivateIPAllocationMethod; }
            set
            {
                if (value == "Static" || value == "Dynamic")
                    _TargetPrivateIPAllocationMethod = value;
                else
                    throw new ArgumentException("Must be 'Static' or 'Dynamic'.");
            }
        }

        public String TargetPrivateIpAddress
        {
            get { return _TargetStaticIpAddress; }
            set { _TargetStaticIpAddress = value.Trim(); }
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

        public string TargetName
        {
            get { return _TargetName; }
            set { _TargetName = value.Trim().Replace(" ", String.Empty); }
        }

        public override string ToString()
        {
            return this.TargetName;
        }
    }
}
