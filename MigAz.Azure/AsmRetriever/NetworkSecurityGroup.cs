using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MigAz.Azure.Asm
{
    public class NetworkSecurityGroup
    {
        #region Variables

        private AzureContext _AzureContext = null;
        private XmlNode _XmlNode = null;
        private List<NetworkSecurityGroupRule> _Rules;

        private string _TargetName = String.Empty;

        #endregion

        #region Constructors

        private NetworkSecurityGroup() { }

        public NetworkSecurityGroup(AzureContext azureContext, XmlNode xmlNode)
        {
            _AzureContext = azureContext;
            _XmlNode = xmlNode;

            this.TargetName = this.Name;

            _Rules = new List<NetworkSecurityGroupRule>();
            foreach (XmlNode rule in _XmlNode.SelectNodes("//Rules/Rule"))
            {
                _Rules.Add(new NetworkSecurityGroupRule(_AzureContext, rule));
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

        public List<NetworkSecurityGroupRule> Rules
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
