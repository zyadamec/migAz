using MigAz.Azure.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MigAz.Azure.Asm
{
    public class AsmLocation : ILocation
    {
        #region Properties

        private XmlNode _XmlNode;
        private AzureContext _AzureContext;

        #endregion

        #region Constructors

        private AsmLocation() { }

        public AsmLocation(AzureContext azureContext, XmlNode xmlNode)
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

