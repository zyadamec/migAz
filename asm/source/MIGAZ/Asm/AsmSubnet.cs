using MIGAZ.Azure;
using MIGAZ.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MIGAZ.Asm
{
    public class AsmSubnet : ISubnet
    {
        #region Variables

        private AzureContext _AzureContext = null;
        private AsmVirtualNetwork _Parent;
        private AsmNetworkSecurityGroup _AsmNetworkSecurityGroup = null;
        private AsmRouteTable _AsmRouteTable = null;
        private XmlNode _XmlNode = null;
        private String _TargetName = null;

        #endregion

        #region Constructors

        private AsmSubnet() { }

        internal AsmSubnet(AzureContext azureContext, AsmVirtualNetwork parent, XmlNode xmlNode)
        {
            _AzureContext = azureContext;
            _Parent = parent;
            _XmlNode = xmlNode;

            this.TargetName = this.Name;
        }

        #endregion

        #region Properties

        public string Name
        {
            get { return _XmlNode.SelectSingleNode("Name").InnerText; }
        }
        public string TargetName
        {
            get { return _TargetName; }
            set { _TargetName = value.Replace(" ", String.Empty); }
        }

        public string Id
        {
            get { return this.Name; }
        }

        public string TargetId
        {
            get {  return "[concat(" + ArmConst.ResourceGroupId + ", '" + ArmConst.ProviderVirtualNetwork + this.Parent.TargetName + "/subnets/" + this.TargetName + "')]"; }
        }

        public string AddressPrefix
        {
            get { return _XmlNode.SelectSingleNode("AddressPrefix").InnerText; }
        }

        private string NetworkSecurityGroupName
        {
            get
            {
                if (_XmlNode.SelectSingleNode("NetworkSecurityGroup") == null)
                    return String.Empty;

                return _XmlNode.SelectSingleNode("NetworkSecurityGroup").InnerText;
            }
        }

        public AsmNetworkSecurityGroup NetworkSecurityGroup
        {
            get
            {
                return _AsmNetworkSecurityGroup;
            }
        }

        private String RouteTableName
        {
            get
            {
                if (_XmlNode.SelectSingleNode("RouteTableName") == null)
                    return String.Empty;

                return _XmlNode.SelectSingleNode("RouteTableName").InnerText;
            }
        }

        public AsmRouteTable RouteTable { get { return _AsmRouteTable; } }

        public AsmVirtualNetwork Parent
        {
            get { return _Parent; }
        }

        public bool IsGatewaySubnet
        {
            get { return this.Name == ArmConst.GatewaySubnetName; }
        }

        


        #endregion

        #region Methods

        public override string ToString()
        {
            return this.Name;
        }

        public async Task InitializeChildrenAsync()
        {
            if (this.NetworkSecurityGroupName != String.Empty)
            {
                _AsmNetworkSecurityGroup = await _AzureContext.AzureRetriever.GetAzureAsmNetworkSecurityGroup(this.NetworkSecurityGroupName);
            }

            if (this.RouteTableName != String.Empty)
            {
                _AsmRouteTable = await _AzureContext.AzureRetriever.GetAzureAsmRouteTable(this.RouteTableName);
            }
        }

        #endregion

    }
}
