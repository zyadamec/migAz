using MigAz.Azure.Arm;
using MigAz.Azure.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MigAz.Azure.Asm
{
    public class VirtualMachine
    {
        #region Variables

        private CloudService _AsmCloudService;
        private AzureContext _AzureContext;
        private XmlNode _XmlNode;
        private Hashtable _VmDetails;
        Disk _OSVirtualHardDisk;
        List<Disk> _DataDisks;
        List<LoadBalancerRule> _LoadBalancerRules;
        List<NetworkInterface> _NetworkInterfaces;
        private VirtualNetwork _SourceVirtualNetwork;
        private Subnet _SourceSubnet;
        private IVirtualNetwork _TargetVirtualNetwork;
        private ISubnet _TargetSubnet;
        private ArmAvailabilitySet _TargetAvailabilitySet = null;
        private NetworkSecurityGroup _AsmNetworkSecurityGroup = null;
        private NetworkInterface _PrimaryNetworkInterface = null;
        private string _TargetName = String.Empty;

        #endregion

        #region Constructors

        private VirtualMachine() { }

        public VirtualMachine(AzureContext azureContext, CloudService asmCloudService, ISettingsProvider settingsProvider, XmlNode virtualMachineXml, Hashtable vmDetails)
        {
            this._AsmCloudService = asmCloudService;
            this._AzureContext = azureContext;
            this._XmlNode = virtualMachineXml;
            this._VmDetails = vmDetails;
            this.TargetName = this.RoleName;
            this._PrimaryNetworkInterface = new NetworkInterface(azureContext, this, settingsProvider, null);

            _OSVirtualHardDisk = new Disk(azureContext, _XmlNode.SelectSingleNode("//OSVirtualHardDisk"));

            _DataDisks = new List<Disk>();
            foreach (XmlNode dataDiskNode in _XmlNode.SelectNodes("//DataVirtualHardDisks/DataVirtualHardDisk"))
            {
                Disk asmDisk = new Disk(azureContext, dataDiskNode);
                _DataDisks.Add(asmDisk);
            }

            _LoadBalancerRules = new List<LoadBalancerRule>();
            foreach (XmlNode loadBalancerRuleNode in _XmlNode.SelectNodes("//ConfigurationSets/ConfigurationSet/InputEndpoints/InputEndpoint"))
            {
                _LoadBalancerRules.Add(new LoadBalancerRule(_AzureContext, loadBalancerRuleNode));
            }

            _NetworkInterfaces = new List<NetworkInterface>();
            foreach (XmlNode networkInterfaceNode in _XmlNode.SelectNodes("//ConfigurationSets/ConfigurationSet/NetworkInterfaces/NetworkInterface"))
            {
                _NetworkInterfaces.Add(new NetworkInterface(_AzureContext, this, settingsProvider, networkInterfaceNode));
            }
        }

        #endregion

        public async Task InitializeChildren()
        {
            this._TargetAvailabilitySet = _AzureContext.AzureRetriever.GetAzureARMAvailabilitySet(this);


            if (this.VirtualNetworkName != String.Empty)
            {
                _SourceVirtualNetwork = await _AzureContext.AzureRetriever.GetAzureAsmVirtualNetwork(this.VirtualNetworkName);

                if (_SourceVirtualNetwork != null)
                {
                    foreach (Subnet asmSubnet in _SourceVirtualNetwork.Subnets)
                    {
                        if (asmSubnet.Name == this.SubnetName)
                        {
                            _SourceSubnet = asmSubnet;
                            break;
                        }
                    }
                }
            }

            await _OSVirtualHardDisk.InitializeChildren();
            foreach (Disk asmDisk in this.DataDisks)
            {
                await asmDisk.InitializeChildren();
            }

            if (this.NetworkSecurityGroupName != String.Empty)
                _AsmNetworkSecurityGroup = await _AzureContext.AzureRetriever.GetAzureAsmNetworkSecurityGroup(this.NetworkSecurityGroupName);
        }

        internal string GetDefaultAvailabilitySetName()
        {
            if (this.AvailabilitySetName != String.Empty)
                return this.AvailabilitySetName;
            else
                return this.CloudServiceName;

        }

        #region Properties

        public CloudService Parent
        {
            get { return _AsmCloudService; }
        }

        public NetworkInterface PrimaryNetworkInterface
        {
            get { return _PrimaryNetworkInterface; }
        }

        public List<Disk> DataDisks
        {
            get { return _DataDisks; }
        }

        public List<LoadBalancerRule> LoadBalancerRules
        {
            get { return _LoadBalancerRules; }
        }

        public List<NetworkInterface> NetworkInterfaces
        {
            get { return _NetworkInterfaces; }
        }

        public string AvailabilitySetName
        {
            get
            {
                if (_XmlNode.SelectSingleNode("//AvailabilitySetName") == null)
                    return String.Empty;

                return _XmlNode.SelectSingleNode("//AvailabilitySetName").InnerText;
            }
        }

        public NetworkSecurityGroup NetworkSecurityGroup
        {
            get { return _AsmNetworkSecurityGroup; }
        }

        public ArmAvailabilitySet TargetAvailabilitySet
        {
            get { return _TargetAvailabilitySet; }
            set { _TargetAvailabilitySet = value; }
        }

        public string RoleName
        {
            get { return _XmlNode.SelectSingleNode("//RoleName").InnerText; }
        }

        public string RoleSize
        {
            get { return _XmlNode.SelectSingleNode("//RoleSize").InnerText; }
        }

        public bool EnabledIpForwarding
        {
            get { return _XmlNode.SelectNodes("//ConfigurationSets/ConfigurationSet/IPForwarding").Count > 0; }
        }

        private string NetworkSecurityGroupName
        {
            get
            {
                if (_XmlNode.SelectSingleNode("//ConfigurationSets/ConfigurationSet/NetworkSecurityGroup") == null)
                    return String.Empty;

                return _XmlNode.SelectSingleNode("//ConfigurationSets/ConfigurationSet/NetworkSecurityGroup").InnerText;
            }
        }

        public string StaticVirtualNetworkIPAddress
        {
            get
            {
                if (_XmlNode.SelectSingleNode("//ConfigurationSets/ConfigurationSet[1]/StaticVirtualNetworkIPAddress") == null)
                    return String.Empty;

                return _XmlNode.SelectSingleNode("//ConfigurationSets/ConfigurationSet[1]/StaticVirtualNetworkIPAddress").InnerText;
            }
        }

        public string SubnetName
        {
            get
            {
                if (_XmlNode.SelectSingleNode("//ConfigurationSets/ConfigurationSet/SubnetNames/SubnetName") == null)
                    return String.Empty;

                return _XmlNode.SelectSingleNode("//ConfigurationSets/ConfigurationSet/SubnetNames[1]/SubnetName").InnerText;
            }
        }

        public string OSVirtualHardDiskOS
        {
            get { return _XmlNode.SelectSingleNode("//OSVirtualHardDisk/OS").InnerText; }
        }

        public Disk OSVirtualHardDisk
        {
            get { return _OSVirtualHardDisk; }
        }

        public string VirtualNetworkName
        {
            get { return _VmDetails["virtualnetworkname"].ToString(); }
        }

        public string LoadBalancerName
        {
            get { return _VmDetails["loadbalancername"].ToString(); }
        }

        public string CloudServiceName
        {
            get { return _VmDetails["cloudservicename"].ToString(); }
        }

        public bool HasPublicIPs
        {
            get
            {
                return _XmlNode.SelectNodes("//ConfigurationSets/ConfigurationSet/PublicIPs").Count > 0;
            }
        }

        public IVirtualNetwork TargetVirtualNetwork
        {
            get { return _TargetVirtualNetwork; }
            set { _TargetVirtualNetwork = value; }
        }

        public Subnet SourceSubnet
        {
            get { return _SourceSubnet; }
        }
        public VirtualNetwork SourceVirtualNetwork
        {
            get { return _SourceVirtualNetwork; }
        }

        public ISubnet TargetSubnet
        {
            get { return _TargetSubnet; }
            set { _TargetSubnet = value; }
        }

        public string TargetName
        {
            get { return _TargetName; }
            set { _TargetName = value; }
        }

        public string GetFinalTargetName()
        {
            return this.TargetName + _AzureContext.SettingsProvider.VirtualMachineSuffix;
        }

        #endregion
    }
}
