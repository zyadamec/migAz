using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MIGAZ.Azure
{
    public class AzureLocation
    {
        #region Properties

        private XmlNode _XmlNode;
        private AzureContext _AzureContext;

        #endregion

        #region Constructors

        private AzureLocation() { }

        public AzureLocation(AzureContext azureContext, XmlNode xmlNode)
        {
            _XmlNode = xmlNode;
            _AzureContext = azureContext;
        }

        #endregion

        #region Properties

        public string Name
        {
            get { return _XmlNode.SelectSingleNode("Name").InnerText; }
        }

        public string DisplayName
        {
            get { return _XmlNode.SelectSingleNode("DisplayName").InnerText; }
        }

        #endregion

        public override string ToString()
        {
            return this.Name;
        }
    }
}
