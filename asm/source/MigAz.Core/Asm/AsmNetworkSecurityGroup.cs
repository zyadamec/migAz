using MIGAZ.Core.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MIGAZ.Core.Asm
{
    public class AsmNetworkSecurityGroup
    {
        #region Variables

        private AzureContext _AzureContext = null;
        private XmlNode _XmlNode = null;
        private List<AsmNetworkSecurityGroupRule> _Rules;

        private string _TargetName = String.Empty;

        #endregion

        #region Constructors

        private AsmNetworkSecurityGroup() { }

        public AsmNetworkSecurityGroup(AzureContext azureContext, XmlNode xmlNode)
        {
            _AzureContext = azureContext;
            _XmlNode = xmlNode;

            this.TargetName = this.Name;

            _Rules = new List<AsmNetworkSecurityGroupRule>();
            foreach (XmlNode rule in _XmlNode.SelectNodes("//Rules/Rule"))
            {
                _Rules.Add(new AsmNetworkSecurityGroupRule(_AzureContext, rule));
            }

        }

        #endregion

        #region Properties

        public string Name
        {
            get { return _XmlNode.SelectSingleNode("Name").InnerText; }
        }

        public string Location
        {
            get { return _XmlNode.SelectSingleNode("Location").InnerText; }
        }

        public List<AsmNetworkSecurityGroupRule> Rules
        {
            get { return _Rules; }
        }

        public string TargetName
        {
            get { return _TargetName; }
            set { _TargetName = value.Trim(); }
        }

        public string GetFinalTargetName()
        {
            return this.TargetName + _AzureContext.SettingsProvider.NetworkSecurityGroupSuffix;
        }
        #endregion
    }
}
