using MIGAZ.Core.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MIGAZ.Core.Asm
{
    public class AsmVirtualNetworkGateway
    {
        private AzureContext _AzureContext;
        private XmlNode _GatewayXml;
        private AsmVirtualNetwork _AsmVirtualNetwork;

        private AsmVirtualNetworkGateway() { }

        public AsmVirtualNetworkGateway(AzureContext azureContext, AsmVirtualNetwork parentNetwork, XmlNode gatewayXml)
        {
            this._AzureContext = azureContext;
            this._AsmVirtualNetwork = parentNetwork;
            this._GatewayXml = gatewayXml;
        }

        public string GatewayType
        {
            get { return _GatewayXml.SelectSingleNode("//GatewayType").InnerText; }
        }

        public string GetFinalTargetname()
        {
            return this._AsmVirtualNetwork.TargetName + this._AzureContext.SettingsProvider.VirtualNetworkGatewaySuffix;
        }
    }
}
