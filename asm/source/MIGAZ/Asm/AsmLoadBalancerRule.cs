using MIGAZ.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MIGAZ.Asm
{
    public class AsmLoadBalancerRule
    {
        #region Variables

        private AzureContext _AzureContext;
        private XmlNode _XmlNode;

        #endregion

        #region Constructors

        private AsmLoadBalancerRule() { }

        public AsmLoadBalancerRule(AzureContext azureContext, XmlNode xmlNode)
        {
            _AzureContext = azureContext;
            _XmlNode = xmlNode;
        }

        #endregion

        #region Properties

        public Int64 ProbePort
        {
            get
            {
                XmlNode probenode = _XmlNode.SelectSingleNode("LoadBalancerProbe");
                return Int64.Parse(probenode.SelectSingleNode("Port").InnerText);
            }
        }

        public string ProbeProtocol
        {
            get
            {
                XmlNode probenode = _XmlNode.SelectSingleNode("LoadBalancerProbe");
                return probenode.SelectSingleNode("Protocol").InnerText;
            }
        }

        public Int64 Port
        {
            get { return Int64.Parse(_XmlNode.SelectSingleNode("Port").InnerText); }
        }

        public Int64 LocalPort
        {
            get { return Int64.Parse(_XmlNode.SelectSingleNode("LocalPort").InnerText); }
        }
        
        public string Protocol
        {
            get { return _XmlNode.SelectSingleNode("Protocol").InnerText; }
        }

        public string Name
        {
            get { return _XmlNode.SelectSingleNode("Name").InnerText; }
        }

        public string LoadBalancedEndpointSetName
        {
            get
            {
                if (_XmlNode.SelectSingleNode("LoadBalancedEndpointSetName") == null)
                    return String.Empty;

                return _XmlNode.SelectSingleNode("LoadBalancedEndpointSetName").InnerText;
            }
        }

        #endregion
    }
}
