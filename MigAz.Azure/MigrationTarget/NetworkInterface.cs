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
        private IVirtualNetwork _TargetVirtualNetwork;
        private ISubnet _TargetSubnet;
        private String _TargetStaticIpAddress = String.Empty;
        private bool _EnableIPForwarding = false;

        private NetworkInterface() { }

        public NetworkInterface(AzureContext azureContext)
        {
            _AzureContext = azureContext;
        }

        public ISubnet TargetSubnet
        {
            get { return _TargetSubnet; }
            set { _TargetSubnet = value; }
        }
        public IVirtualNetwork TargetVirtualNetwork
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

        public String TargetName
        {
            get;set;
        }

        public string ToString()
        {
            return this.TargetName + _AzureContext.SettingsProvider.VirtualMachineSuffix;
        }

        public String StaticVirtualNetworkIPAddress
        {
            get;set;
        }

    }
}
