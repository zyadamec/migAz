using System.Xml;

namespace MigAz.Azure.Asm
{
    public class VirtualNetworkGateway
    {
        private AzureContext _AzureContext;
        private XmlNode _GatewayXml;
        private VirtualNetwork _AsmVirtualNetwork;

        private VirtualNetworkGateway() { }

        public VirtualNetworkGateway(AzureContext azureContext, VirtualNetwork parentNetwork, XmlNode gatewayXml)
        {
            this._AzureContext = azureContext;
            this._AsmVirtualNetwork = parentNetwork;
            this._GatewayXml = gatewayXml;
        }

        public string GatewayType
        {
            get { return _GatewayXml.SelectSingleNode("//GatewayType").InnerText; }
        }

        public string State
        {
            get { return _GatewayXml.SelectSingleNode("//State").InnerText; }
        }

        public string GatewaySize
        {
            get { return _GatewayXml.SelectSingleNode("//GatewaySize").InnerText; }
        }

        public bool IsProvisioned
        {
            get { return this.State != "NotProvisioned"; }
        }
    }
}
