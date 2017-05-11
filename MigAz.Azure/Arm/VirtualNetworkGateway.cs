using MigAz.Core.Interface;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Arm
{
    public class VirtualNetworkGateway : IVirtualNetworkGateway
    {
        private AzureContext _AzureContext;
        private JToken _VirtualNetworkGateway;

        private VirtualNetworkGateway() { }

        public VirtualNetworkGateway(AzureContext azureContext, JToken virtualNetworkGateway)
        {
            _AzureContext = azureContext;
            _VirtualNetworkGateway = virtualNetworkGateway;
        }

        public string Name => (string)_VirtualNetworkGateway["name"];
        public string Id => (string)_VirtualNetworkGateway["id"];
        public string Location => (string)_VirtualNetworkGateway["location"];

        public override string ToString()
        {
            return this.Name;
        }
    }
}
