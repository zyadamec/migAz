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
        private IMigrationVirtualNetwork _TargetVirtualNetwork;
        private IMigrationSubnet _TargetSubnet;
        private String _TargetStaticIpAddress = String.Empty;
        private bool _EnableIPForwarding = false;
        private List<LoadBalancerRule> _LoadBalancerRules = new List<LoadBalancerRule>();
        private string _TargetName = String.Empty;

        private NetworkInterface() { }

        public NetworkInterface(AzureContext azureContext, Asm.NetworkInterface networkInterface)
        {
            _AzureContext = azureContext;
            _SourceNetworkInterface = networkInterface;
            this.TargetName = networkInterface.Name;
            this.IsPrimary = networkInterface.IsPrimary;
        }

        public NetworkInterface(AzureContext azureContext, Arm.NetworkInterface networkInterface)
        {
            _AzureContext = azureContext;
            _SourceNetworkInterface = networkInterface;
            this.TargetName = networkInterface.Name;
            this.IsPrimary = networkInterface.IsPrimary;
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

        public String TargetStaticIpAddress
        {
            get { return _TargetStaticIpAddress; }
            set { _TargetStaticIpAddress = value.Trim(); }
        }

        public bool EnableIPForwarding
        {
            get { return _EnableIPForwarding; }
            set { _EnableIPForwarding = value; }
        }

        public string TargetName
        {
            get { return _TargetName; }
            set { _TargetName = value.Trim().Replace(" ", String.Empty); }
        }

        public override string ToString()
        {
            return this.TargetName + _AzureContext.SettingsProvider.NetworkInterfaceCardSuffix;
        }

        public String StaticVirtualNetworkIPAddress
        {
            get;set;
        }
        public bool HasPublicIPs
        {
            get { return false; } // todo now russell
        }
        public bool IsPrimary { get; set; }

        public INetworkSecurityGroup NetworkSecurityGroup
        {
            get; set; // todo now russell
        }

        public string LoadBalancerName
        {
            get;set;
        }

        public List<LoadBalancerRule> LoadBalancerRules
        {
            get { return _LoadBalancerRules; } // todo now russell
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
    }
}
