using MIGAZ.Core.Azure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MIGAZ.Core.Asm
{
    public class AsmCloudService
    {
        private AzureContext _AzureContext;
        private XmlNode _XmlNode;
        private AsmAffinityGroup _AsmAffinityGroup;
        private List<AsmVirtualMachine> _VirtualMachines = new List<AsmVirtualMachine>();
        private List<AsmLoadBalancer> _LoadBalancers = new List<AsmLoadBalancer>();
        private AsmReservedIP _AsmReservedIP;
        private AsmVirtualNetwork _AsmVirtualNetwork;

        private AsmCloudService() { }

        public AsmCloudService(AzureContext azureContext, XmlNode cloudServiceXml)
        {
            this._AzureContext = azureContext;
            this._XmlNode = cloudServiceXml;
        }

        public string ServiceName
        {
            get
            {
                if (_XmlNode.SelectSingleNode("ServiceName") != null)
                    return _XmlNode.SelectSingleNode("ServiceName").InnerText;

                if (_XmlNode.SelectSingleNode("HostedService/ServiceName") != null)
                    return _XmlNode.SelectSingleNode("HostedService/ServiceName").InnerText;

                return "Unknown";
            }
        }

        public AsmAffinityGroup AffinityGroup
        {
            get { return _AsmAffinityGroup; }
        }

        private string AffinityGroupName
        {
            get
            {
                if (_XmlNode.SelectSingleNode("/HostedService/HostedServiceProperties/AffinityGroup") == null)
                    return String.Empty;

                return _XmlNode.SelectSingleNode("/HostedService/HostedServiceProperties/AffinityGroup").InnerText;
            }
        }

        public string Location
        {
            get
            {
                if (this.AffinityGroup != null)
                    return this.AffinityGroup.Location;

                if (_XmlNode.SelectSingleNode("HostedService/HostedServiceProperties/Location") != null)
                    return _XmlNode.SelectSingleNode("HostedService/HostedServiceProperties/Location").InnerText;

                return "Unknown";
            }
        }

        public AsmVirtualNetwork VirtualNetwork
        {
            get { return _AsmVirtualNetwork; }
        }

        private string VirtualNetworkName
        {
            get
            {
                if (_XmlNode.SelectSingleNode("//Deployments/Deployment/VirtualNetworkName") == null)
                    return String.Empty;

                return _XmlNode.SelectSingleNode("//Deployments/Deployment/VirtualNetworkName").InnerText;
            }
        }


        public AsmReservedIP AsmReservedIP
        {
            get { return _AsmReservedIP; }
        }
        
        public List<AsmLoadBalancer> LoadBalancers
        {
            get { return _LoadBalancers; }
        }

        public List<AsmVirtualMachine> VirtualMachines
        {
            get { return _VirtualMachines; }
        }

        public string StaticVirtualNetworkIPAddress
        {
            get
            {
                if (_XmlNode.SelectSingleNode("//Deployments/Deployment/LoadBalancers/LoadBalancer/FrontendIpConfiguration/StaticVirtualNetworkIPAddress") == null)
                    return String.Empty;
                else
                    return _XmlNode.SelectSingleNode("//Deployments/Deployment/LoadBalancers/LoadBalancer/FrontendIpConfiguration/StaticVirtualNetworkIPAddress").InnerText;
            }
        }

        public virtual AsmVirtualMachine GetVirtualMachine(string virtualMachineName)
        {
            foreach (AsmVirtualMachine asmVirtualMachine in this.VirtualMachines)
            {
                if (asmVirtualMachine.RoleName == virtualMachineName)
                    return asmVirtualMachine;
            }

            return null;
        }

        public async Task LoadChildrenAsync()
        {
            if (this.VirtualNetworkName != String.Empty)
                _AsmVirtualNetwork = await this._AzureContext.AzureRetriever.GetAzureAsmVirtualNetwork(this.VirtualNetworkName);

            if (this.AffinityGroupName == String.Empty)
                _AsmAffinityGroup = null;
            else
                _AsmAffinityGroup = await this._AzureContext.AzureRetriever.GetAzureAsmAffinityGroup(this.AffinityGroupName);

            _VirtualMachines = new List<AsmVirtualMachine>();
            if (_XmlNode.SelectNodes("//Deployments/Deployment").Count > 0)
            {
                if (_XmlNode.SelectNodes("//Deployments/Deployment")[0].SelectNodes("RoleList/Role")[0].SelectNodes("RoleType").Count > 0)
                {
                    if (_XmlNode.SelectNodes("//Deployments/Deployment")[0].SelectNodes("RoleList/Role")[0].SelectSingleNode("RoleType").InnerText == "PersistentVMRole")
                    {

                        XmlNodeList roles = _XmlNode.SelectNodes("//Deployments/Deployment")[0].SelectNodes("RoleList/Role");

                        foreach (XmlNode role in roles)
                        {
                            string virtualmachinename = role.SelectSingleNode("RoleName").InnerText;

                            AsmVirtualMachine asmVirtualMachine = await _AzureContext.AzureRetriever.GetAzureAsmVirtualMachine(this, virtualmachinename);
                            _VirtualMachines.Add(asmVirtualMachine);
                        }
                    }
                }
            }

            List<AsmReservedIP> asmReservedIPs = await _AzureContext.AzureRetriever.GetAzureAsmReservedIPs();
            foreach (AsmReservedIP asmReservedIP in asmReservedIPs)
            {
                if (asmReservedIP.ServiceName == this.ServiceName)
                {
                    _AsmReservedIP = asmReservedIP;
                }
            }

            XmlNodeList loadBalancersXml = _XmlNode.SelectNodes("//Deployments/Deployment/LoadBalancers/LoadBalancer");

            foreach (XmlNode loadBalancerXml in loadBalancersXml)
            {
                AsmLoadBalancer asmLoadBalancer = new AsmLoadBalancer(_AzureContext, _AsmVirtualNetwork, loadBalancerXml);
                _LoadBalancers.Add(asmLoadBalancer);
            }
        }

    }
}
