using MigAz.Core.ArmTemplate;
using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.MigrationTarget
{
    public class VirtualNetwork : IMigrationTarget
    {
        private AzureContext _AzureContext;
        private string _TargetName = String.Empty;
        private List<Subnet> _TargetSubnets = new List<Subnet>();
        List<string> _AddressPrefixes = new List<string>();
        List<string> _DnsServers = new List<string>();

        private VirtualNetwork() { }

        public VirtualNetwork(AzureContext azureContext, Asm.VirtualNetwork virtualNetwork)
        {
            this._AzureContext = azureContext;
            this.Source = virtualNetwork;
            this.TargetName = virtualNetwork.Name;

            foreach (String addressPrefix in virtualNetwork.AddressPrefixes)
            {
                this.AddressPrefixes.Add(addressPrefix);
            }
            foreach (String dnsServer in virtualNetwork.DnsServers)
            {
                this.DnsServers.Add(dnsServer);
            }
        }

        public VirtualNetwork(AzureContext azureContext, Arm.VirtualNetwork virtualNetwork)
        {
            this._AzureContext = azureContext;
            this.Source = virtualNetwork;
            this.TargetName = virtualNetwork.Name;

            foreach (String addressPrefix in virtualNetwork.AddressPrefixes)
            {
                this.AddressPrefixes.Add(addressPrefix);
            }
            foreach (String dnsServer in virtualNetwork.DnsServers)
            {
                this.DnsServers.Add(dnsServer);
            }
        }

        public IVirtualNetwork Source { get; }

        public string TargetName
        {
            get { return _TargetName; }
            set { _TargetName = value.Replace(" ", String.Empty).Trim(); }
        }
        public List<Subnet> TargetSubnets
        {
            get { return _TargetSubnets; }
        }

        public string GetFinalTargetName()
        {
            return this.TargetName + _AzureContext.SettingsProvider.VirtualNetworkSuffix;
        }

        public string TargetId
        {
            get { return "[concat(" + ArmConst.ResourceGroupId + ", '" + ArmConst.ProviderVirtualNetwork + this.GetFinalTargetName() + "')]"; }
        }



        public List<string> AddressPrefixes
        {
            get
            {

                return _AddressPrefixes;
            }
        }

        public List<string> DnsServers
        {
            get
            {
                return _DnsServers;
            }
        }


    }
}
