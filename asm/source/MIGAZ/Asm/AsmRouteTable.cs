using MIGAZ.Azure;
using System.Collections.Generic;
using System.Xml;

namespace MIGAZ.Asm
{
    public class AsmRouteTable
    {
        private List<AsmRoute> _Routes;
        private AzureContext _AzureContext;
        private XmlNode _XmlNode;

        public AsmRouteTable(AzureContext azureContext, XmlNode routeTableNode)
        {
            this._AzureContext = azureContext;
            this._XmlNode = routeTableNode;

            _Routes = new List<AsmRoute>();
            foreach (XmlNode routeNode in _XmlNode.SelectNodes("//RouteList/Route"))
            {
                _Routes.Add(new AsmRoute(this._AzureContext, routeNode));
            }
        }

        #region Properties

        public string Name
        {
            get { return _XmlNode.SelectSingleNode("//Name").InnerText; }
        }

        public string Location
        {
            get
            {
                return _XmlNode.SelectSingleNode("//Location").InnerText;
            }
        }

        public List<AsmRoute> Routes
        {
            get { return _Routes; }
        }

        #endregion

    }
}