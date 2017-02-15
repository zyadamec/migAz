using MIGAZ.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MIGAZ.Asm
{
    public class AsmLoadBalancer
    {
        private AzureContext _AzureContext;
        private AsmVirtualNetwork _AsmVirtualNetwork;
        private AsmSubnet _AsmSubnet;
        private XmlNode _XmlNode;
        private string _TargetName = String.Empty;
        
        public AsmLoadBalancer(AzureContext azureContext, AsmVirtualNetwork asmVirtualNetwork, XmlNode loadBalancerXml)
        {
            this._AzureContext = azureContext;
            this._AsmVirtualNetwork = asmVirtualNetwork;
            this._XmlNode = loadBalancerXml;

            this.TargetName = this.SubnetName;

            foreach (AsmSubnet subnet in _AsmVirtualNetwork.Subnets)
            {
                if (subnet.Name == this.SubnetName)
                {
                    _AsmSubnet = subnet;
                    break;
                }
            }
        }

        public AsmSubnet Subnet
        {
            get { return _AsmSubnet; }
        }

        public string TargetName
        {
            get { return _TargetName; }
            set { _TargetName = value; }
        }

        public string GetFinalTargetName()
        {
            return this.TargetName + this._AzureContext.SettingsProvider.LoadBalancerSuffix;
        }

        private string SubnetName
        {
            get { return _XmlNode.SelectSingleNode("//Deployments/Deployment/LoadBalancers/LoadBalancer/FrontendIpConfiguration/SubnetName").InnerText; }
        }

        public string Type
        {
            get { return _XmlNode.SelectSingleNode("//Deployments/Deployment/LoadBalancers/LoadBalancer/FrontendIpConfiguration/Type").InnerText; }
        }
    }
}
