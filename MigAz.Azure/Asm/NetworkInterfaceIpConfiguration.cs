using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Asm
{
    public class NetworkInterfaceIpConfiguration : INetworkInterfaceIpConfiguration
    {
        private AzureContext _AzureContext;
        private String _Name = "ipconfig1";
        private String _PrivateIpAllocationMethod = "Dynamic";

        public NetworkInterfaceIpConfiguration(AzureContext azureContext)
        {
            _AzureContext = azureContext;
        }

        public async Task InitializeChildrenAsync()
        {
            //_VirtualNetwork = await _AzureContext.AzureRetriever.GetAzureARMVirtualNetwork(this.VirtualNetworkName);

            //if (_VirtualNetwork != null)
            //{
            //    foreach (Subnet subnet in _VirtualNetwork.Subnets)
            //    {
            //        if (subnet.Name == this.SubnetName)
            //        {
            //            _Subnet = subnet;
            //            break;
            //        }
            //    }
            //}
        }

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public string PrivateIpAddress { get; set; }
        public string PrivateIpAddressVersion { get; set; }

        public String PrivateIpAllocationMethod
        {
            get { return _PrivateIpAllocationMethod; }
            set
            {
                if (value == "Static" || value == "Dynamic")
                    _PrivateIpAllocationMethod = value;
                else
                    throw new ArgumentException("Must be 'Static' or 'Dynamic'.");
            }
        }

        public bool IsPrimary { get; set; }
        public string SubnetId { get; set; }
        public string PublicIpAddressId { get; set; }

        public string VirtualNetworkName { get; set; }

        public string SubnetName { get; set; }

        public VirtualNetwork VirtualNetwork { get; set; }
        public Subnet Subnet { get; set; }

    }
}
