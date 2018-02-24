using MigAz.Azure.Arm;
using MigAz.Azure.Asm;
using MigAz.Azure.Interface;
using MigAz.Core.Interface;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using System.Linq;
using MigAz.Core.ArmTemplate;
using System.Xml.Linq;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using MigAz.Core;

namespace MigAz.Azure
{
    public class AzureSubscription : ISubscription
    {

        #region Variables

        private XmlNode _XmlNode;
        private JObject _SubscriptionJson;
        private AzureEnvironment _AzureEnvironment;
        private AzureTenant _ParentTenant;
        private AzureContext _AzureContext;

        private bool _IsAsmLoaded = false;
        private bool _IsArmLoaded = false;
        private List<Arm.Location> _ArmLocations;
        private List<Arm.Provider> _ArmProviders;
        private List<Arm.ResourceGroup> _ArmResourceGroups = new List<Arm.ResourceGroup>();
        private Dictionary<Arm.ResourceGroup, List<Arm.AvailabilitySet>> _ArmAvailabilitySets = new Dictionary<Arm.ResourceGroup, List<Arm.AvailabilitySet>>();
        private Dictionary<Arm.ResourceGroup, List<Arm.VirtualMachine>> _ArmVirtualMachines = new Dictionary<Arm.ResourceGroup, List<Arm.VirtualMachine>>();
        private Dictionary<Arm.ResourceGroup, List<Arm.VirtualMachineImage>> _ArmVirtualMachineImages = new Dictionary<Arm.ResourceGroup, List<Arm.VirtualMachineImage>>();
        private Dictionary<Arm.ResourceGroup, List<Arm.VirtualNetwork>> _ArmVirtualNetworks = new Dictionary<Arm.ResourceGroup, List<Arm.VirtualNetwork>>();
        private Dictionary<Arm.ResourceGroup, List<Arm.NetworkSecurityGroup>> _ArmNetworkSecurityGroups = new Dictionary<Arm.ResourceGroup, List<Arm.NetworkSecurityGroup>>();
        private Dictionary<Arm.ResourceGroup, List<Arm.StorageAccount>> _ArmStorageAccounts = new Dictionary<Arm.ResourceGroup, List<Arm.StorageAccount>>();
        private Dictionary<Arm.ResourceGroup, List<Arm.ManagedDisk>> _ArmManagedDisks = new Dictionary<Arm.ResourceGroup, List<Arm.ManagedDisk>>();
        private Dictionary<Arm.ResourceGroup, List<Arm.LoadBalancer>> _ArmLoadBalancers = new Dictionary<Arm.ResourceGroup, List<Arm.LoadBalancer>>();
        private Dictionary<Arm.ResourceGroup, List<Arm.NetworkInterface>> _ArmNetworkInterfaces = new Dictionary<Arm.ResourceGroup, List<Arm.NetworkInterface>>();
        private Dictionary<Arm.ResourceGroup, List<Arm.VirtualNetworkGateway>> _ArmVirtualNetworkGateways = new Dictionary<Arm.ResourceGroup, List<Arm.VirtualNetworkGateway>>();
        private Dictionary<Arm.ResourceGroup, List<Arm.PublicIP>> _ArmPublicIPs = new Dictionary<Arm.ResourceGroup, List<Arm.PublicIP>>();
        private Dictionary<Arm.ResourceGroup, List<Arm.RouteTable>> _ArmRouteTables = new Dictionary<Arm.ResourceGroup, List<Arm.RouteTable>>();

        private List<Azure.MigrationTarget.NetworkSecurityGroup> _AsmTargetNetworkSecurityGroups = new List<MigrationTarget.NetworkSecurityGroup>();
        private List<Azure.MigrationTarget.StorageAccount> _AsmTargetStorageAccounts = new List<MigrationTarget.StorageAccount>();
        private List<Azure.MigrationTarget.VirtualNetwork> _AsmTargetVirtualNetworks = new List<MigrationTarget.VirtualNetwork>();
        private List<Azure.MigrationTarget.VirtualMachine> _AsmTargetVirtualMachines = new List<MigrationTarget.VirtualMachine>();
        private List<Azure.MigrationTarget.StorageAccount> _ArmTargetStorageAccounts = new List<MigrationTarget.StorageAccount>();
        private List<Azure.MigrationTarget.VirtualNetwork> _ArmTargetVirtualNetworks = new List<MigrationTarget.VirtualNetwork>();
        private List<Azure.MigrationTarget.VirtualMachine> _ArmTargetVirtualMachines = new List<MigrationTarget.VirtualMachine>();
        private List<Azure.MigrationTarget.AvailabilitySet> _ArmTargetAvailabilitySets = new List<MigrationTarget.AvailabilitySet>();
        //private List<Azure.MigrationTarget.VirtualMachineImage> _ArmTargetVirtualMachineImages = new List<MigrationTarget.VirtualMachineImage>();
        private List<Azure.MigrationTarget.Disk> _ArmTargetManagedDisks = new List<MigrationTarget.Disk>();
        private List<Azure.MigrationTarget.RouteTable> _ArmTargetRouteTables = new List<MigrationTarget.RouteTable>();
        private List<Azure.MigrationTarget.LoadBalancer> _ArmTargetLoadBalancers = new List<MigrationTarget.LoadBalancer>();
        private List<Azure.MigrationTarget.NetworkSecurityGroup> _ArmTargetNetworkSecurityGroups = new List<MigrationTarget.NetworkSecurityGroup>();
        private List<Azure.MigrationTarget.PublicIp> _ArmTargetPublicIPs = new List<MigrationTarget.PublicIp>();
        private List<Azure.MigrationTarget.NetworkInterface> _ArmTargetNetworkInterfaces = new List<MigrationTarget.NetworkInterface>();

        // ASM Object Cache (Subscription Context Specific)
        private List<Asm.VirtualNetwork> _VirtualNetworks;
        private List<Asm.RoleSize> _AsmRoleSizes;
        private List<Asm.StorageAccount> _StorageAccounts;
        private List<CloudService> _CloudServices;
        private List<ReservedIP> _AsmReservedIPs;

        private string _ApiUrl = String.Empty;
        private string _TokenResourceUrl = String.Empty;

        #endregion

        #region Constructors

        private AzureSubscription() { }

        //internal AzureSubscription(AzureContext azureContext, XmlNode xmlNode, AzureEnvironment azureEnvironment)
        //{
        //    _AzureContext = azureContext;
        //    _XmlNode = xmlNode;
        //    _AzureEnvironment = azureEnvironment;
        //    _ApiUrl = azureContext.GetARMServiceManagementUrl();
        //    _TokenResourceUrl = azureContext.GetARMTokenResourceUrl();
        //}

        public AzureSubscription(AzureContext azureContext, JObject subscriptionJson, AzureTenant parentAzureTenant, AzureEnvironment azureEnvironment)
        {
            _AzureContext = azureContext;
            _SubscriptionJson = subscriptionJson;
            _ParentTenant = parentAzureTenant;
            _AzureEnvironment = azureEnvironment;
            _ApiUrl = azureContext.GetARMServiceManagementUrl();
            _TokenResourceUrl = azureContext.GetARMTokenResourceUrl();
        }


        public async Task InitializeChildrenAsync(bool useCache = false)
        {
            // russell here now
            //await this.GetAzureARMLocations();
            //await this.GetResourceManagerProviders();

            _ArmProviders = await this.GetResourceManagerProviders(useCache);
        }

        #endregion

        #region Properties

        public string Name
        {
            get
            {
                if (_XmlNode != null)
                    return _XmlNode.SelectSingleNode("SubscriptionName").InnerText;
                else if (_SubscriptionJson != null)
                    return (string)_SubscriptionJson["displayName"];
                else
                    return String.Empty;
            }
        }

        public string TokenResourceUrl
        {
            get { return _TokenResourceUrl; }
        }
        public string ApiUrl
        {
            get { return _ApiUrl; }
        }

        public AzureTenant Parent
        {
            get { return _ParentTenant; }
        }

        public AzureEnvironment AzureEnvironment
        {
            get { return _AzureEnvironment; }
        }

        public Guid AzureAdTenantId
        {
            get
            {
                if (this.Parent != null)
                    return this.Parent.TenantId;
                else if (_XmlNode != null)
                    return new Guid(_XmlNode.SelectSingleNode("AADTenantID").InnerText);
                else
                    return Guid.Empty;
            }
        }

        public Guid SubscriptionId
        {
            get
            {
                if (_XmlNode != null)
                    return new Guid(_XmlNode.SelectSingleNode("SubscriptionID").InnerText);
                else if (_SubscriptionJson != null)
                    return new Guid((string)_SubscriptionJson["subscriptionId"]);
                else
                    return Guid.Empty;

            }
        }

        public string offercategories
        {
            get
            {
                if (_XmlNode != null && _XmlNode.SelectSingleNode("OfferCategories") != null)
                    return _XmlNode.SelectSingleNode("OfferCategories").InnerText;
                else return String.Empty;
            }
        }

        public string SubscriptionStatus
        {
            get
            {
                if (_XmlNode != null && _XmlNode.SelectSingleNode("SubscriptionStatus") != null)
                    return _XmlNode.SelectSingleNode("SubscriptionStatus").InnerText;
                else return String.Empty;
            }
        }

        public string AccountAdminLiveEmailId
        {
            get { return _XmlNode.SelectSingleNode("AccountAdminLiveEmailId").InnerText; }
        }

        public string ServiceAdminLiveEmailId
        {
            get { return _XmlNode.SelectSingleNode("ServiceAdminLiveEmailId").InnerText; }
        }

        public Int32 MaxCoreCount
        {
            get { return Convert.ToInt32(_XmlNode.SelectSingleNode("MaxCoreCount").InnerText); }
        }

        public Int32 MaxStorageAccounts
        {
            get { return Convert.ToInt32(_XmlNode.SelectSingleNode("MaxStorageAccounts").InnerText); }
        }

        public Int32 MaxHostedServices
        {
            get { return Convert.ToInt32(_XmlNode.SelectSingleNode("MaxHostedServices").InnerText); }
        }

        public Int32 CurrentCoreCount
        {
            get { return Convert.ToInt32(_XmlNode.SelectSingleNode("CurrentCoreCount").InnerText); }
        }

        public Int32 CurrentHostedServices
        {
            get { return Convert.ToInt32(_XmlNode.SelectSingleNode("CurrentHostedServices").InnerText); }
        }

        public Int32 CurrentStorageAccounts
        {
            get { return Convert.ToInt32(_XmlNode.SelectSingleNode("CurrentStorageAccounts").InnerText); }
        }

        public Int32 MaxVirtualNetworkSites
        {
            get { return Convert.ToInt32(_XmlNode.SelectSingleNode("MaxVirtualNetworkSites").InnerText); }
        }

        public Int32 CurrentVirtualNetworkSites
        {
            get { return Convert.ToInt32(_XmlNode.SelectSingleNode("CurrentVirtualNetworkSites").InnerText); }
        }

        public Int32 MaxLocalNetworkSites
        {
            get { return Convert.ToInt32(_XmlNode.SelectSingleNode("MaxLocalNetworkSites").InnerText); }
        }

        public Int32 MaxDnsServers
        {
            get { return Convert.ToInt32(_XmlNode.SelectSingleNode("MaxDnsServers").InnerText); }
        }

        public Int32 CurrentDnsServers
        {
            get { return Convert.ToInt32(_XmlNode.SelectSingleNode("CurrentDnsServers").InnerText); }
        }

        public Int32 MaxExtraVIPCount
        {
            get { return Convert.ToInt32(_XmlNode.SelectSingleNode("MaxExtraVIPCount").InnerText); }
        }

        public Int32 MaxReservedIPs
        {
            get { return Convert.ToInt32(_XmlNode.SelectSingleNode("MaxReservedIPs").InnerText); }
        }


        public Int32 CurrentReservedIPs
        {
            get { return Convert.ToInt32(_XmlNode.SelectSingleNode("CurrentReservedIPs").InnerText); }
        }

        public Int32 MaxPublicIPCount
        {
            get { return Convert.ToInt32(_XmlNode.SelectSingleNode("MaxPublicIPCount").InnerText); }
        }

        public Int32 CurrentNetworkSecurityGroups
        {
            get { return Convert.ToInt32(_XmlNode.SelectSingleNode("CurrentNetworkSecurityGroups").InnerText); }
        }

        public Int32 MaxNetworkSecurityGroups
        {
            get { return Convert.ToInt32(_XmlNode.SelectSingleNode("MaxNetworkSecurityGroups").InnerText); }
        }

        public Int32 MaxNetworkSecurityRulesPerGroup
        {
            get { return Convert.ToInt32(_XmlNode.SelectSingleNode("MaxNetworkSecurityRulesPerGroup").InnerText); }
        }

        public Int32 MaxRouteTables
        {
            get { return Convert.ToInt32(_XmlNode.SelectSingleNode("MaxRouteTables").InnerText); }
        }

        public Int32 MaxRoutesPerRouteTable
        {
            get { return Convert.ToInt32(_XmlNode.SelectSingleNode("MaxRoutesPerRouteTable").InnerText); }
        }

        public Int32 MaxRoutesBackendPerRouteTable
        {
            get { return Convert.ToInt32(_XmlNode.SelectSingleNode("MaxRoutesBackendPerRouteTable").InnerText); }
        }

        public DateTime CreatedTime
        {
            get { return Convert.ToDateTime(_XmlNode.SelectSingleNode("CreatedTime").InnerText); }
        }

        #endregion

        public void ResetArm()
        {
            _IsArmLoaded = false;
        }

        public void ResetAsm()
        {
            _IsAsmLoaded = false;
        }


        public static bool operator ==(AzureSubscription lhs, AzureSubscription rhs)
        {
            bool status = false;
            if (((object)lhs == null && (object)rhs == null) ||
                    ((object)lhs != null && (object)rhs != null && ((AzureSubscription)lhs).SubscriptionId == ((AzureSubscription)rhs).SubscriptionId))
            {
                status = true;
            }
            return status;
        }

        public static bool operator !=(AzureSubscription lhs, AzureSubscription rhs)
        {
            return !(lhs == rhs);
        }
        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(AzureSubscription))
                return false;

            return ((AzureSubscription)obj).SubscriptionId == this.SubscriptionId;
        }

        public override string ToString()
        {
            return Name + " (" + SubscriptionId + ")";
        }

        public void ClearCache()
        {
            _ArmVirtualMachines = new Dictionary<Arm.ResourceGroup, List<Arm.VirtualMachine>>();
            _ArmVirtualMachineImages = new Dictionary<Arm.ResourceGroup, List<Arm.VirtualMachineImage>>();
            _ArmAvailabilitySets = new Dictionary<Arm.ResourceGroup, List<Arm.AvailabilitySet>>();
            _ArmVirtualNetworks = new Dictionary<Arm.ResourceGroup, List<Arm.VirtualNetwork>>();
            _ArmNetworkSecurityGroups = new Dictionary<Arm.ResourceGroup, List<Arm.NetworkSecurityGroup>>();
            _ArmStorageAccounts = new Dictionary<Arm.ResourceGroup, List<Arm.StorageAccount>>();
            _ArmManagedDisks = new Dictionary<Arm.ResourceGroup, List<Arm.ManagedDisk>>();
            _ArmLoadBalancers = new Dictionary<Arm.ResourceGroup, List<Arm.LoadBalancer>>();
            _ArmNetworkInterfaces = new Dictionary<Arm.ResourceGroup, List<Arm.NetworkInterface>>();
            _ArmVirtualNetworkGateways = new Dictionary<Arm.ResourceGroup, List<Arm.VirtualNetworkGateway>>();
            _ArmPublicIPs = new Dictionary<Arm.ResourceGroup, List<Arm.PublicIP>>();
            _ArmResourceGroups = new List<Arm.ResourceGroup>();
            _ArmRouteTables = new Dictionary<Arm.ResourceGroup, List<Arm.RouteTable>>();
        }

        internal Dictionary<Arm.ResourceGroup, List<Arm.VirtualMachine>> ArmVirtualMachines
        {
            get { return _ArmVirtualMachines; }
        }
        internal Dictionary<Arm.ResourceGroup, List<Arm.VirtualMachineImage>> ArmVirtualMachineImages
        {
            get { return _ArmVirtualMachineImages; }
        }
        internal Dictionary<Arm.ResourceGroup, List<Arm.AvailabilitySet>> ArmAvailabilitySets
        {
            get { return _ArmAvailabilitySets; }
        }
        internal Dictionary<Arm.ResourceGroup, List<Arm.VirtualNetwork>> ArmVirtualNetworks
        {
            get { return _ArmVirtualNetworks; }
        }
        internal Dictionary<Arm.ResourceGroup, List<Arm.StorageAccount>> ArmStorageAccounts
        {
            get { return _ArmStorageAccounts; }
        }
        internal Dictionary<Arm.ResourceGroup, List<Arm.NetworkSecurityGroup>> ArmNetworkSecurityGroups
        {
            get { return _ArmNetworkSecurityGroups; }
        }
        internal Dictionary<Arm.ResourceGroup, List<Arm.ManagedDisk>> ArmManagedDisks
        {
            get { return _ArmManagedDisks; }
        }
        internal Dictionary<Arm.ResourceGroup, List<Arm.LoadBalancer>> ArmLoadBalancers
        {
            get { return _ArmLoadBalancers; }
        }
        internal Dictionary<Arm.ResourceGroup, List<Arm.NetworkInterface>> ArmNetworkInterfaces
        {
            get { return _ArmNetworkInterfaces; }
        }
        internal Dictionary<Arm.ResourceGroup, List<Arm.VirtualNetworkGateway>> ArmVirtualNetworkGateways
        {
            get { return _ArmVirtualNetworkGateways; }
        }
        internal Dictionary<Arm.ResourceGroup, List<Arm.PublicIP>> ArmPublicIPs
        {
            get { return _ArmPublicIPs; }
        }
        internal List<Arm.ResourceGroup> ArmResourceGroups
        {
            get { return _ArmResourceGroups; }
        }
        internal Dictionary<Arm.ResourceGroup, List<Arm.RouteTable>> ArmRouteTables
        {
            get { return _ArmRouteTables; }
        }

        #region Target Properties

        public List<Azure.MigrationTarget.NetworkSecurityGroup> AsmTargetNetworkSecurityGroups { get { return _AsmTargetNetworkSecurityGroups; } }

        public List<Azure.MigrationTarget.StorageAccount> AsmTargetStorageAccounts { get { return _AsmTargetStorageAccounts; } }
        public List<Azure.MigrationTarget.VirtualNetwork> AsmTargetVirtualNetworks { get { return _AsmTargetVirtualNetworks; } }
        public List<Azure.MigrationTarget.VirtualMachine> AsmTargetVirtualMachines { get { return _AsmTargetVirtualMachines; } }


        public List<Azure.MigrationTarget.StorageAccount> ArmTargetStorageAccounts { get { return _ArmTargetStorageAccounts; } }
        public List<Azure.MigrationTarget.VirtualNetwork> ArmTargetVirtualNetworks { get { return _ArmTargetVirtualNetworks; } }
        public List<Azure.MigrationTarget.VirtualMachine> ArmTargetVirtualMachines { get { return _ArmTargetVirtualMachines; } }
        //public List<Azure.MigrationTarget.VirtualMachineImage> ArmTargetVirtualMachineImages { get { return _ArmTargetVirtualMachineImages; } }
        public List<Azure.MigrationTarget.AvailabilitySet> ArmTargetAvailabilitySets { get { return _ArmTargetAvailabilitySets; } }
        public List<Azure.MigrationTarget.Disk> ArmTargetManagedDisks { get { return _ArmTargetManagedDisks; } }
        public List<Azure.MigrationTarget.RouteTable> ArmTargetRouteTables { get { return _ArmTargetRouteTables; } }
        public List<Azure.MigrationTarget.LoadBalancer> ArmTargetLoadBalancers { get { return _ArmTargetLoadBalancers; } }
        public List<Azure.MigrationTarget.NetworkSecurityGroup> ArmTargetNetworkSecurityGroups { get { return _ArmTargetNetworkSecurityGroups; } }
        public List<Azure.MigrationTarget.PublicIp> ArmTargetPublicIPs { get { return _ArmTargetPublicIPs; } }
        public List<Azure.MigrationTarget.NetworkInterface> ArmTargetNetworkInterfaces { get { return _ArmTargetNetworkInterfaces; } }

        #endregion

        public async Task BindAsmResources(TargetSettings targetSettings)
        {
            if (!_IsAsmLoaded)
            {
                _IsAsmLoaded = true;

                await this.GetAzureARMLocations();
                await this.GetAzureASMRoleSizes();

                foreach (Azure.Asm.NetworkSecurityGroup asmNetworkSecurityGroup in await this.GetAzureAsmNetworkSecurityGroups())
                {
                    // Ensure we load the Full Details to get NSG Rules
                    Azure.Asm.NetworkSecurityGroup asmNetworkSecurityGroupFullDetail = await this.GetAzureAsmNetworkSecurityGroup(asmNetworkSecurityGroup.Name);

                    MigrationTarget.NetworkSecurityGroup targetNetworkSecurityGroup = new MigrationTarget.NetworkSecurityGroup(asmNetworkSecurityGroupFullDetail, targetSettings);
                    this.AsmTargetNetworkSecurityGroups.Add(targetNetworkSecurityGroup);
                }

                List<Azure.Asm.VirtualNetwork> asmVirtualNetworks = await this.GetAzureAsmVirtualNetworks();
                foreach (Azure.Asm.VirtualNetwork asmVirtualNetwork in asmVirtualNetworks)
                {
                    MigrationTarget.VirtualNetwork targetVirtualNetwork = new MigrationTarget.VirtualNetwork(asmVirtualNetwork, this.AsmTargetNetworkSecurityGroups, this.ArmTargetRouteTables, targetSettings);
                    this.AsmTargetVirtualNetworks.Add(targetVirtualNetwork);
                }

                foreach (Azure.Asm.StorageAccount asmStorageAccount in await this.GetAzureAsmStorageAccounts())
                {
                    MigrationTarget.StorageAccount targetStorageAccount = new MigrationTarget.StorageAccount(asmStorageAccount, targetSettings);
                    this.AsmTargetStorageAccounts.Add(targetStorageAccount);
                }

                List<Azure.Asm.CloudService> asmCloudServices = await this.GetAzureAsmCloudServices();
                foreach (Azure.Asm.CloudService asmCloudService in asmCloudServices)
                {
                    List<Azure.MigrationTarget.VirtualMachine> cloudServiceTargetVirtualMachines = new List<Azure.MigrationTarget.VirtualMachine>();
                    MigrationTarget.AvailabilitySet targetAvailabilitySet = new MigrationTarget.AvailabilitySet(asmCloudService, targetSettings);

                    foreach (Azure.Asm.VirtualMachine asmVirtualMachine in asmCloudService.VirtualMachines)
                    {
                        MigrationTarget.VirtualMachine targetVirtualMachine = new MigrationTarget.VirtualMachine(asmVirtualMachine, this, targetSettings);
                        targetVirtualMachine.TargetAvailabilitySet = targetAvailabilitySet;
                        cloudServiceTargetVirtualMachines.Add(targetVirtualMachine);
                        this.AsmTargetVirtualMachines.Add(targetVirtualMachine);
                    }

                    MigrationTarget.LoadBalancer targetLoadBalancer = new MigrationTarget.LoadBalancer();
                    targetLoadBalancer.SetTargetName(asmCloudService.Name, targetSettings);
                    targetLoadBalancer.SourceName = asmCloudService.Name + "-LB";

                    Azure.MigrationTarget.FrontEndIpConfiguration frontEndIpConfiguration = new Azure.MigrationTarget.FrontEndIpConfiguration(targetLoadBalancer);

                    Azure.MigrationTarget.BackEndAddressPool backEndAddressPool = new Azure.MigrationTarget.BackEndAddressPool(targetLoadBalancer);

                    // if internal load balancer
                    if (asmCloudService.ResourceXml.SelectNodes("//Deployments/Deployment/LoadBalancers/LoadBalancer/FrontendIpConfiguration/Type").Count > 0)
                    {
                        string virtualnetworkname = asmCloudService.ResourceXml.SelectSingleNode("//Deployments/Deployment/VirtualNetworkName").InnerText;
                        string subnetname = asmCloudService.ResourceXml.SelectSingleNode("//Deployments/Deployment/LoadBalancers/LoadBalancer/FrontendIpConfiguration/SubnetName").InnerText.Replace(" ", "");

                        if (asmCloudService.ResourceXml.SelectNodes("//Deployments/Deployment/LoadBalancers/LoadBalancer/FrontendIpConfiguration/StaticVirtualNetworkIPAddress").Count > 0)
                        {
                            frontEndIpConfiguration.TargetPrivateIPAllocationMethod = MigrationTarget.IPAllocationMethodEnum.Static;
                            frontEndIpConfiguration.TargetPrivateIpAddress = asmCloudService.ResourceXml.SelectSingleNode("//Deployments/Deployment/LoadBalancers/LoadBalancer/FrontendIpConfiguration/StaticVirtualNetworkIPAddress").InnerText;
                        }

                        if (cloudServiceTargetVirtualMachines.Count > 0)
                        {
                            if (cloudServiceTargetVirtualMachines[0].PrimaryNetworkInterface != null)
                            {
                                if (cloudServiceTargetVirtualMachines[0].PrimaryNetworkInterface.TargetNetworkInterfaceIpConfigurations.Count > 0)
                                {
                                    frontEndIpConfiguration.TargetVirtualNetwork = cloudServiceTargetVirtualMachines[0].PrimaryNetworkInterface.TargetNetworkInterfaceIpConfigurations[0].TargetVirtualNetwork;
                                    frontEndIpConfiguration.TargetSubnet = cloudServiceTargetVirtualMachines[0].PrimaryNetworkInterface.TargetNetworkInterfaceIpConfigurations[0].TargetSubnet;
                                }
                            }
                        }
                    }
                    else // if external load balancer
                    {
                        Azure.MigrationTarget.PublicIp loadBalancerPublicIp = new Azure.MigrationTarget.PublicIp();
                        loadBalancerPublicIp.SourceName = asmCloudService.Name + "-PIP";
                        loadBalancerPublicIp.SetTargetName(asmCloudService.Name, targetSettings);
                        loadBalancerPublicIp.DomainNameLabel = asmCloudService.Name;

                        frontEndIpConfiguration.PublicIp = loadBalancerPublicIp;

                        targetLoadBalancer.LoadBalancerType = MigrationTarget.LoadBalancerType.Public;
                    }

                    foreach (Azure.MigrationTarget.VirtualMachine targetVirtualMachine in cloudServiceTargetVirtualMachines)
                    {
                        if (targetVirtualMachine.PrimaryNetworkInterface != null)
                            targetVirtualMachine.PrimaryNetworkInterface.BackEndAddressPool = backEndAddressPool;

                        Azure.Asm.VirtualMachine asmVirtualMachine = (Azure.Asm.VirtualMachine)targetVirtualMachine.Source;
                        foreach (XmlNode inputendpoint in asmVirtualMachine.ResourceXml.SelectNodes("//ConfigurationSets/ConfigurationSet/InputEndpoints/InputEndpoint"))
                        {
                            if (inputendpoint.SelectSingleNode("LoadBalancedEndpointSetName") == null) // if it's a inbound nat rule
                            {
                                Azure.MigrationTarget.InboundNatRule targetInboundNatRule = new Azure.MigrationTarget.InboundNatRule(targetLoadBalancer);
                                targetInboundNatRule.Name = asmVirtualMachine.RoleName + "-" + inputendpoint.SelectSingleNode("Name").InnerText;
                                targetInboundNatRule.FrontEndPort = Int32.Parse(inputendpoint.SelectSingleNode("Port").InnerText);
                                targetInboundNatRule.BackEndPort = Int32.Parse(inputendpoint.SelectSingleNode("LocalPort").InnerText);
                                targetInboundNatRule.Protocol = inputendpoint.SelectSingleNode("Protocol").InnerText;
                                targetInboundNatRule.FrontEndIpConfiguration = frontEndIpConfiguration;

                                if (targetVirtualMachine.PrimaryNetworkInterface != null)
                                    targetVirtualMachine.PrimaryNetworkInterface.InboundNatRules.Add(targetInboundNatRule);
                            }
                            else // if it's a load balancing rule
                            {
                                XmlNode probenode = inputendpoint.SelectSingleNode("LoadBalancerProbe");

                                Azure.MigrationTarget.Probe targetProbe = null;
                                foreach (Azure.MigrationTarget.Probe existingProbe in targetLoadBalancer.Probes)
                                {
                                    if (existingProbe.Name == inputendpoint.SelectSingleNode("LoadBalancedEndpointSetName").InnerText)
                                    {
                                        targetProbe = existingProbe;
                                        break;
                                    }
                                }

                                if (targetProbe == null)
                                {
                                    targetProbe = new Azure.MigrationTarget.Probe();

                                    if (inputendpoint.SelectSingleNode("LoadBalancedEndpointSetName") != null)
                                        targetProbe.Name = inputendpoint.SelectSingleNode("LoadBalancedEndpointSetName").InnerText;

                                    if (inputendpoint.SelectSingleNode("Port") != null)
                                        targetProbe.Port = Int32.Parse(inputendpoint.SelectSingleNode("Port").InnerText);

                                    if (inputendpoint.SelectSingleNode("Protocol") != null)
                                        targetProbe.Protocol = inputendpoint.SelectSingleNode("Protocol").InnerText;

                                    targetLoadBalancer.Probes.Add(targetProbe);
                                }

                                Azure.MigrationTarget.LoadBalancingRule targetLoadBalancingRule = null;
                                foreach (Azure.MigrationTarget.LoadBalancingRule existingLoadBalancingRule in targetLoadBalancer.LoadBalancingRules)
                                {
                                    if (existingLoadBalancingRule.Name == inputendpoint.SelectSingleNode("LoadBalancedEndpointSetName").InnerText)
                                    {
                                        targetLoadBalancingRule = existingLoadBalancingRule;
                                        break;
                                    }
                                }

                                if (targetLoadBalancingRule == null)
                                {
                                    targetLoadBalancingRule = new Azure.MigrationTarget.LoadBalancingRule(targetLoadBalancer);
                                    targetLoadBalancingRule.Name = inputendpoint.SelectSingleNode("LoadBalancedEndpointSetName").InnerText;
                                    targetLoadBalancingRule.FrontEndIpConfiguration = frontEndIpConfiguration;
                                    targetLoadBalancingRule.BackEndAddressPool = targetLoadBalancer.BackEndAddressPools[0];
                                    targetLoadBalancingRule.Probe = targetProbe;
                                    targetLoadBalancingRule.FrontEndPort = Int32.Parse(inputendpoint.SelectSingleNode("Port").InnerText);
                                    targetLoadBalancingRule.BackEndPort = Int32.Parse(inputendpoint.SelectSingleNode("LocalPort").InnerText);
                                    targetLoadBalancingRule.Protocol = inputendpoint.SelectSingleNode("Protocol").InnerText;
                                }
                            }
                        }
                    }

                }
            }
        }

        public async Task BindArmResources(TargetSettings targetSettings)
        {
            if (!_IsArmLoaded)
            {
                _IsArmLoaded = true;

                await this.GetAzureARMLocations();

                List<Task> armNetworkSecurityGroupTasks = new List<Task>();
                foreach (ResourceGroup armResourceGroup in await this.GetAzureARMResourceGroups())
                {
                    Task armNetworkSecurityGroupTask = LoadARMNetworkSecurityGroups(armResourceGroup, targetSettings);
                    armNetworkSecurityGroupTasks.Add(armNetworkSecurityGroupTask);
                }
                await Task.WhenAll(armNetworkSecurityGroupTasks.ToArray());

                List<Task> armRouteTableTasks = new List<Task>();
                foreach (ResourceGroup armResourceGroup in await this.GetAzureARMResourceGroups())
                {
                    Task armRouteTableTask = LoadARMRouteTables(armResourceGroup, targetSettings);
                    armRouteTableTasks.Add(armRouteTableTask);
                }
                await Task.WhenAll(armRouteTableTasks.ToArray());

                List<Task> armPublicIPTasks = new List<Task>();
                foreach (ResourceGroup armResourceGroup in await this.GetAzureARMResourceGroups())
                {
                    Task armPublicIPTask = LoadARMPublicIPs(armResourceGroup, targetSettings);
                    armPublicIPTasks.Add(armPublicIPTask);
                }
                await Task.WhenAll(armPublicIPTasks.ToArray());


                List<Task> armVirtualNetworkTasks = new List<Task>();
                foreach (ResourceGroup armResourceGroup in await this.GetAzureARMResourceGroups())
                {
                    Task armVirtualNetworkTask = LoadARMVirtualNetworks(armResourceGroup, targetSettings);
                    armVirtualNetworkTasks.Add(armVirtualNetworkTask);
                }
                await Task.WhenAll(armVirtualNetworkTasks.ToArray());

                List<Task> armStorageAccountTasks = new List<Task>();
                foreach (ResourceGroup armResourceGroup in await this.GetAzureARMResourceGroups())
                {
                    Task armStorageAccountTask = LoadARMStorageAccounts(armResourceGroup, targetSettings);
                    armStorageAccountTasks.Add(armStorageAccountTask);
                }
                await Task.WhenAll(armStorageAccountTasks.ToArray());

                List<Task> armManagedDiskTasks = new List<Task>();
                foreach (ResourceGroup armResourceGroup in await this.GetAzureARMResourceGroups())
                {
                    Task armManagedDiskTask = LoadARMManagedDisks(armResourceGroup, targetSettings);
                    armManagedDiskTasks.Add(armManagedDiskTask);
                }
                await Task.WhenAll(armManagedDiskTasks.ToArray());

                List<Task> armAvailabilitySetTasks = new List<Task>();
                foreach (ResourceGroup armResourceGroup in await this.GetAzureARMResourceGroups())
                {
                    Task armAvailabilitySetTask = LoadARMAvailabilitySets(armResourceGroup, targetSettings);
                    armAvailabilitySetTasks.Add(armAvailabilitySetTask);
                }
                await Task.WhenAll(armAvailabilitySetTasks.ToArray());

                List<Task> armNetworkInterfaceTasks = new List<Task>();
                foreach (ResourceGroup armResourceGroup in await this.GetAzureARMResourceGroups())
                {
                    Task armNetworkInterfaceTask = LoadARMNetworkInterfaces(armResourceGroup, this.ArmTargetVirtualNetworks, this.ArmTargetNetworkSecurityGroups, targetSettings);
                    armNetworkInterfaceTasks.Add(armNetworkInterfaceTask);
                }
                await Task.WhenAll(armNetworkInterfaceTasks.ToArray());

                List<Task> armVirtualMachineTasks = new List<Task>();
                foreach (ResourceGroup armResourceGroup in await this.GetAzureARMResourceGroups())
                {
                    Task armVirtualMachineTask = LoadARMVirtualMachines(armResourceGroup, targetSettings);
                    armVirtualMachineTasks.Add(armVirtualMachineTask);
                }
                await Task.WhenAll(armVirtualMachineTasks.ToArray());


                List<Task> armLoadBalancerTasks = new List<Task>();
                foreach (ResourceGroup armResourceGroup in await this.GetAzureARMResourceGroups())
                {
                    Task armLoadBalancerTask = LoadARMLoadBalancers(armResourceGroup, targetSettings);
                    armLoadBalancerTasks.Add(armLoadBalancerTask);
                }
                await Task.WhenAll(armLoadBalancerTasks.ToArray());

                //List<Task> armVirtualMachineImageTasks = new List<Task>();
                //foreach (ResourceGroup armResourceGroup in await this.GetAzureARMResourceGroups())
                //{
                //    Task armVirtualMachineImageTask = LoadARMVirtualMachineImages(armResourceGroup);
                //    armVirtualMachineImageTasks.Add(armVirtualMachineImageTask);
                //}
                //await Task.WhenAll(armVirtualMachineImageTasks.ToArray());
            }
        }

        public async virtual Task<List<Arm.Location>> GetAzureARMLocations()
        {
            _AzureContext.LogProvider.WriteLog("GetAzureARMLocations", "Start");

            if (_ArmLocations != null)
                return _ArmLocations;

            JObject locationsJson = await this.GetAzureARMResources("Locations", null, null);

            _ArmLocations = new List<Arm.Location>();

            if (locationsJson != null)
            {
                var locations = from location in locationsJson["value"]
                                select location;

                if (locations != null)
                {
                    foreach (var location in locations)
                    {
                        Arm.Location armLocation = new Arm.Location(_AzureContext, this, location);
                        _ArmLocations.Add(armLocation);

                        _AzureContext.LogProvider.WriteLog("GetAzureARMLocations", "Created Arm Location " + armLocation.ToString());
                    }
                }
            }

            if (_AzureContext.IncludePreviewRegions)
            {
                _AzureContext.LogProvider.WriteLog("GetAzureARMLocations", "France preview regions added by code");

                // Add Preview Location for France Central (centered on Eiffel Tower)
                var franceCentralJson = "{ \"id\": \"/subscriptions/" + _AzureContext.AzureSubscription.SubscriptionId.ToString("D") + "/locations/francecentral\", \"name\": \"francecentral\",  \"displayName\": \"France Central\",  \"longitude\": \"2.294\",  \"latitude\": \"48.858\"}";
                var lfc = JToken.Parse(franceCentralJson);
                _ArmLocations.Add(new Arm.Location(_AzureContext, this, lfc));

                // Add Preview Location for France South (centered on If Castle)
                var franceSouthJson = "{ \"id\": \"/subscriptions/" + _AzureContext.AzureSubscription.SubscriptionId.ToString("D") + "/locations/francesouth\", \"name\": \"francesouth\",  \"displayName\": \"France South\",  \"longitude\": \"5.325126\",  \"latitude\": \"43.279841\"}";
                var lfs = JToken.Parse(franceSouthJson);
                _ArmLocations.Add(new Arm.Location(_AzureContext, this, lfs));

            }
             
            List<Task> armLocationChildTasks = new List<Task>();
            foreach (Arm.Location armLocation in _ArmLocations)
            {
                Task armLocationChildTask = armLocation.InitializeChildrenAsync();
                armLocationChildTasks.Add(armLocationChildTask);
            }
            await Task.WhenAll(armLocationChildTasks.ToArray());

            return _ArmLocations;
        }

        #region ASM Methods

        internal async Task<RoleSize> GetAzureASMRoleSize(string roleSize)
        {
            List<Asm.RoleSize> asmRoleSizes = await this.GetAzureASMRoleSizes();
            return asmRoleSizes.Where(a => a.Name == roleSize).FirstOrDefault();
        }

        public async virtual Task<List<Asm.Location>> GetAzureASMLocations()
        {
            _AzureContext.LogProvider.WriteLog("GetAzureASMLocations", "Start");

            XmlNode locationsXml = await this.GetAzureAsmResources("Locations", null);
            List<Asm.Location> azureLocations = new List<Asm.Location>();
            foreach (XmlNode locationXml in locationsXml.SelectNodes("/Locations/Location"))
            {
                azureLocations.Add(new Asm.Location(_AzureContext, locationXml));
            }

            return azureLocations.OrderBy(a => a.DisplayName).ToList();
        }

        public async virtual Task<List<ReservedIP>> GetAzureAsmReservedIPs()
        {
            _AzureContext.LogProvider.WriteLog("GetAzureAsmReservedIPs", "Start");

            if (_AsmReservedIPs != null)
                return _AsmReservedIPs;

            _AsmReservedIPs = new List<ReservedIP>();
            XmlDocument reservedIPsXml = await this.GetAzureAsmResources("ReservedIPs", null);
            foreach (XmlNode reservedIPXml in reservedIPsXml.SelectNodes("/ReservedIPs/ReservedIP"))
            {
                _AsmReservedIPs.Add(new ReservedIP(_AzureContext, reservedIPXml));
            }

            return _AsmReservedIPs;
        }

        public async virtual Task<List<Asm.StorageAccount>> GetAzureAsmStorageAccounts()
        {
            _AzureContext.LogProvider.WriteLog("GetAzureAsmStorageAccounts", "Start");

            if (_StorageAccounts != null)
                return _StorageAccounts;

            _StorageAccounts = new List<Asm.StorageAccount>();
            XmlDocument storageAccountsXml = await this.GetAzureAsmResources("StorageAccounts", null);
            foreach (XmlNode storageAccountXml in storageAccountsXml.SelectNodes("//StorageService"))
            {
                Asm.StorageAccount asmStorageAccount = new Asm.StorageAccount(_AzureContext, storageAccountXml);
                _StorageAccounts.Add(asmStorageAccount);
            }

            _AzureContext.StatusProvider.UpdateStatus("BUSY: Loading Storage Account Keys");
            List<Task> storageAccountKeyTasks = new List<Task>();
            foreach (Asm.StorageAccount asmStorageAccount in _StorageAccounts)
            {
                storageAccountKeyTasks.Add(asmStorageAccount.LoadStorageAccountKeysAsynch());
            }
            await Task.WhenAll(storageAccountKeyTasks);

            return _StorageAccounts;
        }

        public async virtual Task<Asm.StorageAccount> GetAzureAsmStorageAccount(string storageAccountName)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureAsmStorageAccount", "Start");

            if (storageAccountName == null)
                return null;

            foreach (Asm.StorageAccount asmStorageAccount in await this.GetAzureAsmStorageAccounts())
            {
                if (asmStorageAccount.Name == storageAccountName)
                    return asmStorageAccount;
            }

            return null;
        }

        public async virtual Task<List<Asm.VirtualNetwork>> GetAzureAsmVirtualNetworks()
        {
            _AzureContext.LogProvider.WriteLog("GetAzureAsmVirtualNetworks", "Start");

            if (_VirtualNetworks != null)
                return _VirtualNetworks;

            _VirtualNetworks = new List<Asm.VirtualNetwork>();
            foreach (XmlNode virtualnetworksite in (await this.GetAzureAsmResources("VirtualNetworks", null)).SelectNodes("//VirtualNetworkSite"))
            {
                Asm.VirtualNetwork asmVirtualNetwork = new Asm.VirtualNetwork(_AzureContext, virtualnetworksite);
                await asmVirtualNetwork.InitializeChildrenAsync();
                _VirtualNetworks.Add(asmVirtualNetwork);
            }
            return _VirtualNetworks;
        }

        public async virtual Task<List<Asm.RoleSize>> GetAzureASMRoleSizes()
        {
            _AzureContext.LogProvider.WriteLog("GetAzureASMRoleSizes", "Start");

            if (_AsmRoleSizes != null)
                return _AsmRoleSizes;

            _AsmRoleSizes = new List<Asm.RoleSize>();
            foreach (XmlNode roleSizeNode in (await this.GetAzureAsmResources("RoleSize", null)).SelectNodes("//RoleSize"))
            {
                Asm.RoleSize asmRoleSize = new Asm.RoleSize(_AzureContext, roleSizeNode);
                _AsmRoleSizes.Add(asmRoleSize);
            }
            return _AsmRoleSizes;
        }

        public async Task<Asm.VirtualNetwork> GetAzureAsmVirtualNetwork(string virtualNetworkName)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureAsmVirtualNetwork", "Start");

            foreach (Asm.VirtualNetwork asmVirtualNetwork in await this.GetAzureAsmVirtualNetworks())
            {
                if (asmVirtualNetwork.Name == virtualNetworkName)
                {
                    return asmVirtualNetwork;
                }
            }

            return null;
        }

        internal async Task<AffinityGroup> GetAzureAsmAffinityGroup(string affinityGroupName)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureAsmAffinityGroup", "Start");

            Hashtable affinitygroupinfo = new Hashtable();
            affinitygroupinfo.Add("affinitygroupname", affinityGroupName);

            XmlNode affinityGroupXml = await this.GetAzureAsmResources("AffinityGroup", affinitygroupinfo);
            AffinityGroup asmAffinityGroup = new AffinityGroup(_AzureContext, affinityGroupXml.SelectSingleNode("AffinityGroup"));
            return asmAffinityGroup;
        }

        public async virtual Task<Asm.NetworkSecurityGroup> GetAzureAsmNetworkSecurityGroup(string networkSecurityGroupName)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureAsmNetworkSecurityGroup", "Start");

            Hashtable networkSecurityGroupInfo = new Hashtable();
            networkSecurityGroupInfo.Add("name", networkSecurityGroupName);

            XmlNode networkSecurityGroupXml = await this.GetAzureAsmResources("NetworkSecurityGroup", networkSecurityGroupInfo);
            Asm.NetworkSecurityGroup asmNetworkSecurityGroup = new Asm.NetworkSecurityGroup(_AzureContext, networkSecurityGroupXml.SelectSingleNode("NetworkSecurityGroup"));
            return asmNetworkSecurityGroup;
        }

        public async virtual Task<List<Asm.NetworkSecurityGroup>> GetAzureAsmNetworkSecurityGroups()
        {
            _AzureContext.LogProvider.WriteLog("GetAzureAsmNetworkSecurityGroups", "Start");

            List<Asm.NetworkSecurityGroup> networkSecurityGroups = new List<Asm.NetworkSecurityGroup>();

            XmlDocument x = await this.GetAzureAsmResources("NetworkSecurityGroups", null);
            foreach (XmlNode networkSecurityGroupNode in (await this.GetAzureAsmResources("NetworkSecurityGroups", null)).SelectNodes("//NetworkSecurityGroup"))
            {
                Asm.NetworkSecurityGroup asmNetworkSecurityGroup = new Asm.NetworkSecurityGroup(_AzureContext, networkSecurityGroupNode);
                networkSecurityGroups.Add(asmNetworkSecurityGroup);
            }

            return networkSecurityGroups;
        }

        public async virtual Task<Asm.RouteTable> GetAzureAsmRouteTable(string routeTableName)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureAsmRouteTable", "Start");

            Hashtable info = new Hashtable();
            info.Add("name", routeTableName);
            XmlDocument routeTableXml = await this.GetAzureAsmResources("RouteTable", info);
            return new Asm.RouteTable(_AzureContext, routeTableXml);
        }

        internal async Task<Asm.VirtualMachine> GetAzureAsmVirtualMachine(CloudService asmCloudService, string virtualMachineName)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureAsmVirtualMachine", "Start");

            Hashtable vmDetails = await this.GetVMDetails(asmCloudService.Name, virtualMachineName);
            XmlDocument virtualMachineXml = await this.GetAzureAsmResources("VirtualMachine", vmDetails);
            Asm.VirtualMachine asmVirtualMachine = new Asm.VirtualMachine(this._AzureContext, asmCloudService, virtualMachineXml, vmDetails);
            await asmVirtualMachine.InitializeChildren();

            return asmVirtualMachine;
        }

        private async Task<Hashtable> GetVMDetails(string cloudServiceName, string virtualMachineName)
        {
            _AzureContext.LogProvider.WriteLog("GetVMDetails", "Start");

            Hashtable cloudserviceinfo = new Hashtable();
            cloudserviceinfo.Add("name", cloudServiceName);

            Hashtable virtualmachineinfo = new Hashtable();
            virtualmachineinfo.Add("cloudservicename", cloudServiceName);
            virtualmachineinfo.Add("virtualmachinename", virtualMachineName);

            XmlDocument hostedservice = await GetAzureAsmResources("CloudService", cloudserviceinfo);
            if (hostedservice.SelectNodes("//Deployments/Deployment").Count > 0)
            {
                if (hostedservice.SelectNodes("//Deployments/Deployment")[0].SelectNodes("RoleList/Role")[0].SelectNodes("RoleType").Count > 0)
                {
                    if (hostedservice.SelectNodes("//Deployments/Deployment")[0].SelectNodes("RoleList/Role")[0].SelectSingleNode("RoleType").InnerText == "PersistentVMRole")
                    {
                        if (hostedservice.SelectNodes("//Deployments/Deployment")[0].SelectSingleNode("VirtualNetworkName") != null)
                        {
                            virtualmachineinfo.Add("virtualnetworkname", hostedservice.SelectNodes("//Deployments/Deployment")[0].SelectSingleNode("VirtualNetworkName").InnerText);
                        }
                        else
                        {
                            virtualmachineinfo.Add("virtualnetworkname", String.Empty);
                        }

                        virtualmachineinfo.Add("deploymentname", hostedservice.SelectNodes("//Deployments/Deployment")[0].SelectSingleNode("Name").InnerText);

                        XmlNodeList roles = hostedservice.SelectNodes("//Deployments/Deployment")[0].SelectNodes("RoleList/Role");
                        // GetVMLBMapping is necessary because a Cloud Service can have multiple availability sets
                        // On ARM, a load balancer can only be attached to 1 availability set
                        // Because of this, if multiple availability sets exist, we are breaking the cloud service in multiple load balancers
                        //     to respect all availability sets
                        Dictionary<string, string> vmlbmapping = GetVMLBMapping(cloudServiceName, roles);
                        foreach (XmlNode role in roles)
                        {
                            string currentVM = role.SelectSingleNode("RoleName").InnerText;
                            if (currentVM == virtualMachineName)
                            {
                                virtualmachineinfo.Add("loadbalancername", vmlbmapping[virtualMachineName]);
                                return virtualmachineinfo;
                            }
                        }
                    }
                }
            }
            throw new InvalidOperationException("Requested VM could not be found");
        }

        public Dictionary<string, string> GetVMLBMapping(string cloudservicename, XmlNodeList roles)
        {
            Dictionary<string, string> aslbnamemapping = new Dictionary<string, string>();
            Dictionary<string, string> aslbnamemapping2 = new Dictionary<string, string>();

            foreach (XmlNode role in roles)
            {
                string availabilitysetname = String.Empty;
                if (role.SelectNodes("AvailabilitySetName").Count > 0)
                {
                    availabilitysetname = role.SelectSingleNode("AvailabilitySetName").InnerText;
                }

                if (!aslbnamemapping.ContainsKey(availabilitysetname))
                {
                    aslbnamemapping.Add(availabilitysetname, String.Empty);
                }
            }

            if (aslbnamemapping.Count == 1)
            {
                foreach (KeyValuePair<string, string> keyvaluepair in aslbnamemapping)
                {
                    aslbnamemapping2.Add(keyvaluepair.Key, cloudservicename);
                }
            }
            else
            {
                int increment = 1;
                foreach (KeyValuePair<string, string> keyvaluepair in aslbnamemapping)
                {
                    aslbnamemapping2.Add(keyvaluepair.Key, cloudservicename + "-LB" + increment.ToString());
                    increment += 1;
                }
            }

            Dictionary<string, string> vmlbmapping = new Dictionary<string, string>();

            foreach (XmlNode role in roles)
            {
                string virtualmachinename = role.SelectSingleNode("RoleName").InnerText;
                string availabilitysetname = String.Empty;
                if (role.SelectNodes("AvailabilitySetName").Count > 0)
                {
                    availabilitysetname = role.SelectSingleNode("AvailabilitySetName").InnerText;
                }
                string loadbalancername = aslbnamemapping2[availabilitysetname];

                vmlbmapping.Add(virtualmachinename, loadbalancername);
            }

            return vmlbmapping;
        }




        public async virtual Task<List<CloudService>> GetAzureAsmCloudServices()
        {
            _AzureContext.LogProvider.WriteLog("GetAzureAsmCloudServices", "Start");

            if (_CloudServices != null)
                return _CloudServices;

            XmlDocument cloudServicesXml = await this.GetAzureAsmResources("CloudServices", null);
            _CloudServices = new List<CloudService>();
            foreach (XmlNode cloudServiceXml in cloudServicesXml.SelectNodes("//HostedService"))
            {
                CloudService tempCloudService = new CloudService(_AzureContext, cloudServiceXml);

                Hashtable cloudServiceInfo = new Hashtable();
                cloudServiceInfo.Add("name", tempCloudService.Name);
                XmlDocument cloudServiceDetailXml = await this.GetAzureAsmResources("CloudService", cloudServiceInfo);
                CloudService asmCloudService = new CloudService(_AzureContext, cloudServiceDetailXml);

                _CloudServices.Add(asmCloudService);
            }

            List<Task> cloudServiceVMTasks = new List<Task>();
            foreach (CloudService asmCloudService in _CloudServices)
            {
                cloudServiceVMTasks.Add(asmCloudService.LoadChildrenAsync());
            }

            await Task.WhenAll(cloudServiceVMTasks);

            return _CloudServices;
        }

        public async virtual Task<CloudService> GetAzureAsmCloudService(string cloudServiceName)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureAsmCloudService", "Start");

            foreach (CloudService asmCloudService in await this.GetAzureAsmCloudServices())
            {
                if (asmCloudService.Name == cloudServiceName)
                    return asmCloudService;
            }

            return null;
        }

        public async Task<StorageAccountKeys> GetAzureAsmStorageAccountKeys(string storageAccountName)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureAsmStorageAccountKeys", "Start");

            Hashtable storageAccountInfo = new Hashtable();
            storageAccountInfo.Add("name", storageAccountName);

            XmlDocument storageAccountKeysXml = await this.GetAzureAsmResources("StorageAccountKeys", storageAccountInfo);
            return new StorageAccountKeys(_AzureContext, storageAccountKeysXml);
        }

        public async virtual Task<List<ClientRootCertificate>> GetAzureAsmClientRootCertificates(Asm.VirtualNetwork asmVirtualNetwork)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureAsmClientRootCertificates", "Start");

            Hashtable virtualNetworkInfo = new Hashtable();
            virtualNetworkInfo.Add("virtualnetworkname", asmVirtualNetwork.Name);
            XmlDocument clientRootCertificatesXml = await this.GetAzureAsmResources("ClientRootCertificates", virtualNetworkInfo);

            List<ClientRootCertificate> asmClientRootCertificates = new List<ClientRootCertificate>();
            foreach (XmlNode clientRootCertificateXml in clientRootCertificatesXml.SelectNodes("//ClientRootCertificate"))
            {
                ClientRootCertificate asmClientRootCertificate = new ClientRootCertificate(_AzureContext, asmVirtualNetwork, clientRootCertificateXml);
                await asmClientRootCertificate.InitializeChildrenAsync();
                asmClientRootCertificates.Add(asmClientRootCertificate);
            }

            return asmClientRootCertificates;
        }

        public async Task<XmlDocument> GetAzureAsmClientRootCertificateData(Asm.VirtualNetwork asmVirtualNetwork, string certificateThumbprint)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureAsmClientRootCertificateData", "Start");

            Hashtable certificateInfo = new Hashtable();
            certificateInfo.Add("virtualnetworkname", asmVirtualNetwork.Name);
            certificateInfo.Add("thumbprint", certificateThumbprint);
            XmlDocument clientRootCertificateXml = await this.GetAzureAsmResources("ClientRootCertificate", certificateInfo);
            return clientRootCertificateXml;
        }

        public async virtual Task<Asm.VirtualNetworkGateway> GetAzureAsmVirtualNetworkGateway(Asm.VirtualNetwork asmVirtualNetwork)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureAsmVirtualNetworkGateway", "Start");

            Hashtable virtualNetworkGatewayInfo = new Hashtable();
            virtualNetworkGatewayInfo.Add("virtualnetworkname", asmVirtualNetwork.Name);
            virtualNetworkGatewayInfo.Add("localnetworksitename", String.Empty);

            XmlDocument gatewayXml = await this.GetAzureAsmResources("VirtualNetworkGateway", virtualNetworkGatewayInfo);
            return new Asm.VirtualNetworkGateway(_AzureContext, asmVirtualNetwork, gatewayXml);
        }

        public async virtual Task<string> GetAzureAsmVirtualNetworkSharedKey(string virtualNetworkName, string localNetworkSiteName)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureAsmVirtualNetworkSharedKey", "Start");

            try
            {
                Hashtable virtualNetworkGatewayInfo = new Hashtable();
                virtualNetworkGatewayInfo.Add("virtualnetworkname", virtualNetworkName);
                virtualNetworkGatewayInfo.Add("localnetworksitename", localNetworkSiteName);

                XmlDocument connectionShareKeyXml = await this.GetAzureAsmResources("VirtualNetworkGatewaySharedKey", virtualNetworkGatewayInfo);
                if (connectionShareKeyXml.SelectSingleNode("//Value") == null)
                    return String.Empty;

                return connectionShareKeyXml.SelectSingleNode("//Value").InnerText;
            }
            catch (Exception exc)
            {
                _AzureContext.LogProvider.WriteLog("GetAzureAsmVirtualNetworkSharedKey", "Exception: " + exc.Message + exc.StackTrace);
                return String.Empty;
            }
        }

        #endregion

        #region ARM Methods

        public async Task<List<ResourceGroup>> GetAzureARMResourceGroups()
        {
            _AzureContext.LogProvider.WriteLog("GetAzureARMResourceGroups", "Start");

            if (this.ArmResourceGroups.Count > 0)
                return this.ArmResourceGroups;

            JObject resourceGroupsJson = await this.GetAzureARMResources("ResourceGroups", null, null);

            var resourceGroups = from resourceGroup in resourceGroupsJson["value"]
                                 select resourceGroup;

            foreach (JObject resourceGroupJson in resourceGroups)
            {
                ResourceGroup resourceGroup = new ResourceGroup(resourceGroupJson, _AzureContext.AzureEnvironment, _AzureContext.AzureSubscription);
                await resourceGroup.InitializeChildrenAsync(_AzureContext);
                this.ArmResourceGroups.Add(resourceGroup);
                _AzureContext.LogProvider.WriteLog("GetAzureARMResourceGroups", "Loaded ARM Resource Group '" + resourceGroup.Name + "'.");

            }

            return this.ArmResourceGroups;
        }

        public virtual Arm.VirtualNetwork GetAzureARMVirtualNetwork(AzureSubscription azureSubscription, string virtualNetworkId)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureARMVirtualNetwork", "Start");

            if (azureSubscription == null || azureSubscription.ArmVirtualNetworks == null)
                return null;

            if (virtualNetworkId.ToLower().Contains("/subnets/"))
                virtualNetworkId = virtualNetworkId.Substring(0, virtualNetworkId.ToLower().IndexOf("/subnets/"));

            foreach (List<Arm.VirtualNetwork> listVirtualNetworks in azureSubscription.ArmVirtualNetworks.Values)
            {
                foreach (Arm.VirtualNetwork armVirtualNetwork in listVirtualNetworks)
                {
                    if (String.Compare(armVirtualNetwork.Id, virtualNetworkId, true) == 0)
                        return armVirtualNetwork;
                }
            }

            return null;
        }

        public async Task<List<Arm.VirtualNetwork>> GetAzureARMVirtualNetworks()
        {
            List<Arm.VirtualNetwork> virtualNetworks = new List<Arm.VirtualNetwork>();

            foreach (ResourceGroup resourceGroup in await this.GetAzureARMResourceGroups())
            {
                foreach (Arm.VirtualNetwork virtualNetwork in await this.GetAzureARMVirtualNetworks(resourceGroup))
                {
                    virtualNetworks.Add(virtualNetwork);
                }
            }

            return virtualNetworks;
        }

        public async Task<List<Arm.VirtualNetwork>> GetAzureARMVirtualNetworks(Arm.Location azureLocation)
        {
            List<Arm.VirtualNetwork> virtualNetworks = new List<Arm.VirtualNetwork>();

            foreach (ResourceGroup resourceGroup in await this.GetAzureARMResourceGroups())
            {
                foreach (Arm.VirtualNetwork virtualNetwork in await this.GetAzureARMVirtualNetworks(resourceGroup))
                {
                    if (virtualNetwork.Location.Name == azureLocation.Name)
                        virtualNetworks.Add(virtualNetwork);
                }
            }

            return virtualNetworks;
        }

        public async virtual Task<List<Arm.VirtualNetwork>> GetAzureARMVirtualNetworks(ResourceGroup resourceGroup)
        {
            if (resourceGroup == null)
                throw new ArgumentException("ResourceGroup parameter must be provided");

            _AzureContext.LogProvider.WriteLog("GetAzureARMVirtualNetworks", "Start - '" + resourceGroup.ToString() + "' Resource Group");

            if (resourceGroup.AzureSubscription.ArmVirtualNetworks.ContainsKey(resourceGroup))
                return resourceGroup.AzureSubscription.ArmVirtualNetworks[resourceGroup];

            JObject virtualNetworksJson = await this.GetAzureARMResources("VirtualNetworks", resourceGroup, null);

            var virtualNetworks = from vnet in virtualNetworksJson["value"]
                                  select vnet;

            List<Arm.VirtualNetwork> resourceGroupVirtualNetworks = new List<Arm.VirtualNetwork>();

            foreach (var virtualNetwork in virtualNetworks)
            {
                Arm.VirtualNetwork armVirtualNetwork = new Arm.VirtualNetwork(virtualNetwork);

                await armVirtualNetwork.InitializeChildrenAsync(_AzureContext);
                foreach (Arm.VirtualNetworkGateway v in await _AzureContext.AzureSubscription.GetAzureARMVirtualNetworkGateways(resourceGroup))
                {
                    // todo now asap, why is this here
                }

                resourceGroupVirtualNetworks.Add(armVirtualNetwork);
                _AzureContext.LogProvider.WriteLog("GetAzureARMVirtualNetworks", "Loaded ARM Virtual Network '" + armVirtualNetwork.Name + "'.");
                _AzureContext.StatusProvider.UpdateStatus("Loaded ARM Virtual Network '" + armVirtualNetwork.Name + "'.");

            }

            resourceGroup.AzureSubscription.ArmVirtualNetworks.Add(resourceGroup, resourceGroupVirtualNetworks);
            return resourceGroupVirtualNetworks;
        }

        internal async Task<Arm.ManagedDisk> GetAzureARMManagedDisk(Arm.VirtualMachine virtualMachine, string name)
        {
            foreach (Arm.ManagedDisk managedDisk in await this.GetAzureARMManagedDisks(virtualMachine.ResourceGroup))
            {
                if (String.Compare(managedDisk.OwnerId, virtualMachine.Id, true) == 0 && String.Compare(managedDisk.Name, name, true) == 0)
                    return managedDisk;
            }

            return null;
        }

        public async virtual Task<List<Arm.ManagedDisk>> GetAzureARMManagedDisks(ResourceGroup resourceGroup)
        {
            if (resourceGroup == null)
                throw new ArgumentException("ResourceGroup parameter must be provided");

            _AzureContext.LogProvider.WriteLog("GetAzureARMManagedDisks", "Start - '" + resourceGroup.ToString() + "' Resource Group");

            if (resourceGroup.AzureSubscription.ArmManagedDisks.ContainsKey(resourceGroup))
                return resourceGroup.AzureSubscription.ArmManagedDisks[resourceGroup];

            JObject managedDisksJson = await this.GetAzureARMResources("ManagedDisks", resourceGroup, null);

            var managedDisks = from managedDisk in managedDisksJson["value"]
                               select managedDisk;

            List<Arm.ManagedDisk> resourceGroupManagedDisks = new List<Arm.ManagedDisk>();

            foreach (var managedDisk in managedDisks)
            {
                Arm.ManagedDisk armManagedDisk = new Arm.ManagedDisk(this._AzureContext, managedDisk);
                await armManagedDisk.InitializeChildrenAsync(_AzureContext);
                resourceGroupManagedDisks.Add(armManagedDisk);
            }

            resourceGroup.AzureSubscription.ArmManagedDisks.Add(resourceGroup, resourceGroupManagedDisks);
            return resourceGroupManagedDisks;
        }

        public async Task<List<Arm.StorageAccount>> GetAzureARMStorageAccounts()
        {
            List<Arm.StorageAccount> storageAccounts = new List<Arm.StorageAccount>();

            foreach (ResourceGroup resourceGroup in await this.GetAzureARMResourceGroups())
            {
                foreach (Arm.StorageAccount storageAccount in await this.GetAzureARMStorageAccounts(resourceGroup))
                {
                    storageAccounts.Add(storageAccount);
                }
            }

            return storageAccounts;
        }

        public async virtual Task<List<Arm.StorageAccount>> GetAzureARMStorageAccounts(Arm.Location azureLocation)
        {
            List<Arm.StorageAccount> storageAccounts = new List<Arm.StorageAccount>();

            foreach (ResourceGroup resourceGroup in await this.GetAzureARMResourceGroups())
            {
                foreach (Arm.StorageAccount storageAccount in await this.GetAzureARMStorageAccounts(resourceGroup))
                {
                    if (storageAccount.Location.Name == azureLocation.Name)
                        storageAccounts.Add(storageAccount);
                }
            }

            return storageAccounts;
        }

        public async virtual Task<List<Arm.StorageAccount>> GetAzureARMStorageAccounts(ResourceGroup resourceGroup)
        {
            if (resourceGroup == null)
                throw new ArgumentException("ResourceGroup parameter must be provided");

            _AzureContext.LogProvider.WriteLog("GetAzureARMStorageAccounts", "Start - '" + resourceGroup.ToString() + "' Resource Group");

            if (resourceGroup.AzureSubscription.ArmStorageAccounts.ContainsKey(resourceGroup))
                return resourceGroup.AzureSubscription.ArmStorageAccounts[resourceGroup];

            JObject storageAccountsJson = await this.GetAzureARMResources("StorageAccounts", resourceGroup, null);

            var storageAccounts = from storage in storageAccountsJson["value"]
                                  select storage;

            List<Arm.StorageAccount> resouceGroupStorageAccounts = new List<Arm.StorageAccount>();

            foreach (var storageAccount in storageAccounts)
            {
                Arm.StorageAccount armStorageAccount = new Arm.StorageAccount(storageAccount, _AzureContext);
                await armStorageAccount.InitializeChildrenAsync(_AzureContext);
                armStorageAccount.ResourceGroup = await this.GetAzureARMResourceGroup(armStorageAccount.Id);
                _AzureContext.LogProvider.WriteLog("GetAzureARMVirtualNetworks", "Loaded ARM Storage Account '" + armStorageAccount.Name + "'.");
                _AzureContext.StatusProvider.UpdateStatus("Loaded ARM Storage Account '" + armStorageAccount.Name + "'.");


                await this.GetAzureARMStorageAccountKeys(armStorageAccount);

                resouceGroupStorageAccounts.Add(armStorageAccount);
            }

            resourceGroup.AzureSubscription.ArmStorageAccounts.Add(resourceGroup, resouceGroupStorageAccounts);
            return resouceGroupStorageAccounts;
        }

        public virtual Arm.StorageAccount GetAzureARMStorageAccount(AzureSubscription azureSubscription, string name)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureARMStorageAccount", "Start");

            if (azureSubscription == null || azureSubscription.ArmStorageAccounts == null)
                return null;

            foreach (List<Arm.StorageAccount> listStorageAccounts in azureSubscription.ArmStorageAccounts.Values)
            {
                foreach (Arm.StorageAccount armStorageAccount in listStorageAccounts)
                {
                    if (String.Compare(armStorageAccount.Name, name, true) == 0)
                        return armStorageAccount;
                }
            }

            return null;
        }

        public async Task<Arm.Location> GetAzureARMLocation(string location)
        {
            if (location == null || location.Length == 0)
                throw new ArgumentException("Location parameter must be provided.");

            List<Arm.Location> armLocations = await this.GetAzureARMLocations();
            Arm.Location matchedLocation = armLocations.Where(a => a.DisplayName == location).FirstOrDefault();

            if (matchedLocation == null)
                matchedLocation = armLocations.Where(a => a.Name == location).FirstOrDefault();

            return matchedLocation;
        }

        internal async Task<List<Provider>> GetResourceManagerProviders(bool allowCache = false)
        {
            _AzureContext.LogProvider.WriteLog("GetResourceManagerProviders", "Start - Subscription : " + this.ToString());

            if (_ArmProviders != null)
                return _ArmProviders;

            if (_AzureContext == null)
                throw new ArgumentNullException("AzureContext is null.  Unable to call Azure API without Azure Context.");
            if (_AzureContext.TokenProvider == null)
                throw new ArgumentNullException("TokenProvider Context is null.  Unable to call Azure API without TokenProvider.");

            AuthenticationResult armToken = await _AzureContext.TokenProvider.GetToken(this.TokenResourceUrl, this.AzureAdTenantId);

            // https://docs.microsoft.com/en-us/rest/api/resources/providers/list
            string url = this.ApiUrl + "subscriptions/" + this.SubscriptionId + "/providers?&api-version=2017-05-10";
            _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting ARM Providers for Subscription: " + this.ToString());

            AzureRestRequest azureRestRequest = new AzureRestRequest(url, armToken, allowCache);
            AzureRestResponse azureRestResponse = await _AzureContext.AzureRetriever.GetAzureRestResponse(azureRestRequest);
            JObject providersJson = JObject.Parse(azureRestResponse.Response);

            _AzureContext.StatusProvider.UpdateStatus("BUSY: Instantiating ARM Providers for Subscription: " + this.ToString());

            var providers = from provider in providersJson["value"]
                          select provider;

            _ArmProviders = new List<Provider>();
            foreach (var provider in providers)
            {
                Provider armProvider = new Provider(provider, this);
                _ArmProviders.Add(armProvider);
            }

            return _ArmProviders;
        }
        
        public String GetProviderMaxApiVersion(string providerNamespace, string resourceType)
        {
            if (_ArmProviders == null)
                throw new Exception("You must first call InitializeChildrenAsync on the Azure Subscription before querying providers.");

            Provider provider = _ArmProviders.Where(a => String.Compare(a.Namespace, providerNamespace, true) == 0).FirstOrDefault();
            if (provider == null)
                throw new ArgumentException("Unable to locate Provider with namespace '" + providerNamespace + "'.");

            ProviderResourceType prt = provider.ResourceTypes.Where(a => String.Compare(a.ResourceType, resourceType, true) == 0).FirstOrDefault();
            if (prt == null)
                throw new ArgumentException("Unable to locate Resource Type '" + resourceType + "' in Provider Namespace '" + providerNamespace + "'.");

            return prt.MaxApiVersion;
        }

        internal async Task<JToken> GetAzureArmVirtualMachineDetail(Arm.VirtualMachine virtualMachine)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureArmVirtualMachine", "Start - '" + virtualMachine.ResourceGroup.ToString() + "' Resource Group / '" + virtualMachine.ToString() + "' Virtual Machine");

            // https://docs.microsoft.com/en-us/rest/api/compute/virtualmachines/virtualmachines-get
            string url = _AzureContext.AzureServiceUrls.GetARMServiceManagementUrl() + "subscriptions/" + this.SubscriptionId + "/resourceGroups/" + virtualMachine.ResourceGroup.ToString() + ArmConst.ProviderVirtualMachines + virtualMachine.ToString() + "?$expand=instanceView&api-version=2016-04-30-preview";
            _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting ARM Azure Virtual Machine Details : '" + virtualMachine.ResourceGroup.ToString() + "' / '" + virtualMachine.ToString() + "' " + this.SubscriptionId);

            AuthenticationResult armToken = await _AzureContext.TokenProvider.GetToken(_AzureContext.AzureServiceUrls.GetARMServiceManagementUrl(), this.AzureAdTenantId);

            AzureRestRequest azureRestRequest = new AzureRestRequest(url, armToken, "GET", false);
            AzureRestResponse azureRestResponse = await _AzureContext.AzureRetriever.GetAzureRestResponse(azureRestRequest);
            JObject virtualMachineResult = JObject.Parse(azureRestResponse.Response);

            return virtualMachineResult;
        }

        public async Task<List<Arm.VirtualMachine>> GetAzureArmVirtualMachines(ResourceGroup resourceGroup)
        {
            if (resourceGroup == null)
                throw new ArgumentException("ResourceGroup parameter must be provided");

            _AzureContext.LogProvider.WriteLog("GetAzureArmVirtualMachines", "Start - '" + resourceGroup.ToString() + "' Resource Group");

            if (resourceGroup.AzureSubscription.ArmVirtualMachines.ContainsKey(resourceGroup))
                return resourceGroup.AzureSubscription.ArmVirtualMachines[resourceGroup];

            JObject virtualMachineJson = await this.GetAzureARMResources("VirtualMachines", resourceGroup, null);

            var virtualMachines = from virtualMachine in virtualMachineJson["value"]
                                  select virtualMachine;

            List<Arm.VirtualMachine> resourceGroupVirtualMachines = new List<Arm.VirtualMachine>();

            foreach (var virtualMachine in virtualMachines)
            {
                Arm.VirtualMachine armVirtualMachine = new Arm.VirtualMachine(this._AzureContext, virtualMachine);
                await armVirtualMachine.InitializeChildrenAsync(_AzureContext);
                resourceGroupVirtualMachines.Add(armVirtualMachine);
                _AzureContext.LogProvider.WriteLog("GetAzureArmVirtualMachines", "Loaded ARM Virtual Machine '" + armVirtualMachine.Name + "'.");
                _AzureContext.StatusProvider.UpdateStatus("Loaded ARM Virtual Machine '" + armVirtualMachine.Name + "'.");
            }

            resourceGroup.AzureSubscription.ArmVirtualMachines.Add(resourceGroup, resourceGroupVirtualMachines);
            return resourceGroupVirtualMachines;
        }
        public async Task<List<Arm.VirtualMachineImage>> GetAzureArmVirtualMachineImages(ResourceGroup resourceGroup)
        {
            if (resourceGroup == null)
                throw new ArgumentException("ResourceGroup parameter must be provided");

            _AzureContext.LogProvider.WriteLog("GetAzureArmVirtualMachineImages", "Start - '" + resourceGroup.ToString() + "' Resource Group");

            if (resourceGroup.AzureSubscription.ArmVirtualMachineImages.ContainsKey(resourceGroup))
                return resourceGroup.AzureSubscription.ArmVirtualMachineImages[resourceGroup];

            JObject virtualMachineImagesJson = await this.GetAzureARMResources("VirtualMachineImages", resourceGroup, null);

            var virtualMachineImages = from virtualMachineImage in virtualMachineImagesJson["value"]
                                       select virtualMachineImage;

            List<Arm.VirtualMachineImage> resourceGroupVirtualMachineImages = new List<Arm.VirtualMachineImage>();

            foreach (var virtualMachineImage in virtualMachineImages)
            {
                Arm.VirtualMachineImage armVirtualMachineImage = new Arm.VirtualMachineImage(this._AzureContext, virtualMachineImage);
                await armVirtualMachineImage.InitializeChildrenAsync(_AzureContext);
                resourceGroupVirtualMachineImages.Add(armVirtualMachineImage);
                _AzureContext.LogProvider.WriteLog("GetAzureArmVirtualMachineImages", "Loaded ARM Virtual Machine Image '" + armVirtualMachineImage.Name + "'.");
                _AzureContext.StatusProvider.UpdateStatus("Loaded ARM Virtual Machine '" + armVirtualMachineImage.Name + "'.");
            }

            resourceGroup.AzureSubscription.ArmVirtualMachineImages.Add(resourceGroup, resourceGroupVirtualMachineImages);
            return resourceGroupVirtualMachineImages;
        }

        internal async Task GetAzureARMStorageAccountKeys(Arm.StorageAccount armStorageAccount)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureARMStorageAccountKeys", "Start - ARM Storage Account '" + armStorageAccount.Name + "'.");

            Hashtable storageAccountKeyInfo = new Hashtable();
            storageAccountKeyInfo.Add("ResourceGroupName", armStorageAccount.ResourceGroup.Name);
            storageAccountKeyInfo.Add("StorageAccountName", armStorageAccount.Name);

            JObject storageAccountKeysJson = await this.GetAzureARMResources("StorageAccountKeys", armStorageAccount.ResourceGroup, storageAccountKeyInfo);

            var storageAccountKeys = from keys in storageAccountKeysJson["keys"]
                                     select keys;

            armStorageAccount.Keys.Clear();
            foreach (var storageAccountKey in storageAccountKeys)
            {
                StorageAccountKey armStorageAccountKey = new StorageAccountKey(storageAccountKey);
                armStorageAccount.Keys.Add(armStorageAccountKey);
            }

            return;
        }

        public async Task<List<Arm.AvailabilitySet>> GetAzureARMAvailabilitySets(ResourceGroup resourceGroup)
        {
            if (resourceGroup == null)
                throw new ArgumentException("ResourceGroup parameter must be provided");

            _AzureContext.LogProvider.WriteLog("GetAzureARMAvailabilitySets", "Start - '" + resourceGroup.ToString() + "' Resource Group");

            if (resourceGroup.AzureSubscription.ArmAvailabilitySets.ContainsKey(resourceGroup))
                return resourceGroup.AzureSubscription.ArmAvailabilitySets[resourceGroup];

            JObject availabilitySetJson = await this.GetAzureARMResources("AvailabilitySets", resourceGroup, null);

            var availabilitySets = from availabilitySet in availabilitySetJson["value"]
                                   select availabilitySet;

            List<Arm.AvailabilitySet> resourceGroupAvailabilitySets = new List<Arm.AvailabilitySet>();

            foreach (var availabilitySet in availabilitySets)
            {
                Arm.AvailabilitySet armAvailabilitySet = new Arm.AvailabilitySet(availabilitySet);
                await armAvailabilitySet.InitializeChildrenAsync(this._AzureContext);
                resourceGroupAvailabilitySets.Add(armAvailabilitySet);
            }

            resourceGroup.AzureSubscription.ArmAvailabilitySets.Add(resourceGroup, resourceGroupAvailabilitySets);
            return resourceGroupAvailabilitySets;
        }

        public Arm.AvailabilitySet GetAzureARMAvailabilitySet(AzureSubscription azureSubscription, string availabilitySetId)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureARMAvailabilitySet", "Start");

            if (azureSubscription == null || azureSubscription.ArmAvailabilitySets == null)
                return null;

            foreach (List<Arm.AvailabilitySet> listAvailabilitySet in azureSubscription.ArmAvailabilitySets.Values)
            {
                foreach (Arm.AvailabilitySet armAvailabilitySet in listAvailabilitySet)
                {
                    if (String.Compare(armAvailabilitySet.Id, availabilitySetId, true) == 0)
                        return armAvailabilitySet;
                }
            }

            return null;
        }

        public async Task<ResourceGroup> GetAzureARMResourceGroup(string id)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureARMResourceGroup", "Start");

            if (id != null && id != String.Empty)
            {
                string[] idSplit = id.Split('/');

                if (idSplit.Length >= 4)
                {
                    string seekResourceGroupId = "/" + idSplit[1] + "/" + idSplit[2] + "/" + idSplit[3] + "/" + idSplit[4];

                    foreach (ResourceGroup resourceGroup in await this.GetAzureARMResourceGroups())
                    {
                        if (String.Equals(resourceGroup.Id, seekResourceGroupId, StringComparison.OrdinalIgnoreCase))
                            return resourceGroup;
                    }
                }
            }

            return null;
        }

        public async Task<List<Arm.NetworkInterface>> GetAzureARMNetworkInterfaces(ResourceGroup resourceGroup)
        {
            if (resourceGroup == null)
                throw new ArgumentException("ResourceGroup parameter must be provided");

            _AzureContext.LogProvider.WriteLog("GetAzureARMNetworkInterfaces", "Start - '" + resourceGroup.ToString() + "' Resource Group");

            if (resourceGroup.AzureSubscription.ArmNetworkInterfaces.ContainsKey(resourceGroup))
                return resourceGroup.AzureSubscription.ArmNetworkInterfaces[resourceGroup];

            JObject networkInterfacesJson = await this.GetAzureARMResources("NetworkInterfaces", resourceGroup, null);

            var networkInterfaces = from networkInterface in networkInterfacesJson["value"]
                                    select networkInterface;

            List<Arm.NetworkInterface> resourceGroupNetworkInterfaces = new List<Arm.NetworkInterface>();

            foreach (var networkInterface in networkInterfaces)
            {
                Arm.NetworkInterface armNetworkInterface = new Arm.NetworkInterface(networkInterface);
                await armNetworkInterface.InitializeChildrenAsync(_AzureContext);
                resourceGroupNetworkInterfaces.Add(armNetworkInterface);
                _AzureContext.LogProvider.WriteLog("GetAzureARMNetworkInterfaces", "Loaded ARM Network Interface '" + armNetworkInterface.Name + "'.");
            }

            resourceGroup.AzureSubscription.ArmNetworkInterfaces.Add(resourceGroup, resourceGroupNetworkInterfaces);
            return resourceGroupNetworkInterfaces;
        }

        public async Task<Arm.NetworkInterface> GetAzureARMNetworkInterface(string id)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureARMNetworkInterface", "Start");

            int providerIndexOf = id.ToLower().IndexOf(ArmConst.ProviderNetworkInterfaces.ToLower());
            int postNicSeperatorIndexOf = id.Substring(providerIndexOf + ArmConst.ProviderNetworkInterfaces.Length + 1).IndexOf("/");
            if (postNicSeperatorIndexOf > -1)
                id = id.Substring(0, providerIndexOf + ArmConst.ProviderNetworkInterfaces.Length + postNicSeperatorIndexOf + 1);

            ResourceGroup resourceGroup = await GetAzureARMResourceGroup(id);
            if (resourceGroup != null)
            {
                foreach (Arm.NetworkInterface networkInterface in await this.GetAzureARMNetworkInterfaces(resourceGroup))
                {
                    if (String.Compare(networkInterface.Id, id, StringComparison.InvariantCultureIgnoreCase) == 0)
                        return networkInterface;
                }
            }

            return null;
        }

        public async Task<List<Arm.VirtualNetworkGateway>> GetAzureARMVirtualNetworkGateways(ResourceGroup resourceGroup)
        {
            if (resourceGroup == null)
                throw new ArgumentException("ResourceGroup parameter must be provided");

            _AzureContext.LogProvider.WriteLog("GetAzureARMVirtualNetworkGateways", "Start - '" + resourceGroup.ToString() + "' Resource Group");

            if (resourceGroup.AzureSubscription.ArmVirtualNetworkGateways.ContainsKey(resourceGroup))
                return resourceGroup.AzureSubscription.ArmVirtualNetworkGateways[resourceGroup];

            JObject virtualNetworkGatewaysJson = await this.GetAzureARMResources("VirtualNetworkGateways", resourceGroup, null);

            var virtualNetworkGateways = from virtualNetworkGateway in virtualNetworkGatewaysJson["value"]
                                         select virtualNetworkGateway;

            List<Arm.VirtualNetworkGateway> resourceGroupVirtualNetworkGateways = new List<Arm.VirtualNetworkGateway>();

            foreach (var virtualNetworkGateway in virtualNetworkGateways)
            {
                Arm.VirtualNetworkGateway armVirtualNetworkGateway = new Arm.VirtualNetworkGateway(virtualNetworkGateway);
                resourceGroupVirtualNetworkGateways.Add(armVirtualNetworkGateway);
            }

            resourceGroup.AzureSubscription.ArmVirtualNetworkGateways.Add(resourceGroup, resourceGroupVirtualNetworkGateways);
            return resourceGroupVirtualNetworkGateways;
        }


        public async Task<Arm.NetworkSecurityGroup> GetAzureARMNetworkSecurityGroup(string id)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureARMNetworkSecurityGroup", "Start");

            ResourceGroup resourceGroup = await GetAzureARMResourceGroup(id);
            if (resourceGroup != null)
            {
                foreach (Arm.NetworkSecurityGroup networkSecurityGroup in await this.GetAzureARMNetworkSecurityGroups(resourceGroup))
                {
                    if (String.Compare(networkSecurityGroup.Id, id, StringComparison.InvariantCultureIgnoreCase) == 0)
                        return networkSecurityGroup;
                }
            }

            return null;
        }
        public async Task<Arm.RouteTable> GetAzureARMRouteTable(string id)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureARMRouteTable", "Start");

            ResourceGroup resourceGroup = await GetAzureARMResourceGroup(id);
            if (resourceGroup != null)
            {
                foreach (Arm.RouteTable routeTable in await this.GetAzureARMRouteTables(resourceGroup))
                {
                    if (String.Compare(routeTable.Id, id, StringComparison.InvariantCultureIgnoreCase) == 0)
                        return routeTable;
                }
            }

            return null;
        }

        public async Task<List<Arm.NetworkSecurityGroup>> GetAzureARMNetworkSecurityGroups(ResourceGroup resourceGroup)
        {
            if (resourceGroup == null)
                throw new ArgumentException("ResourceGroup parameter must be provided");

            _AzureContext.LogProvider.WriteLog("GetAzureARMNetworkSecurityGroups", "Start - '" + resourceGroup.ToString() + "' Resource Group");

            if (resourceGroup.AzureSubscription.ArmNetworkSecurityGroups.ContainsKey(resourceGroup))
                return resourceGroup.AzureSubscription.ArmNetworkSecurityGroups[resourceGroup];

            JObject networkSecurityGroupsJson = await this.GetAzureARMResources("NetworkSecurityGroups", resourceGroup, null);

            var networkSecurityGroups = from networkSecurityGroup in networkSecurityGroupsJson["value"]
                                        select networkSecurityGroup;

            List<Arm.NetworkSecurityGroup> resourceGroupNetworkSecurityGroups = new List<Arm.NetworkSecurityGroup>();

            foreach (var networkSecurityGroup in networkSecurityGroups)
            {
                Arm.NetworkSecurityGroup armNetworkSecurityGroup = new Arm.NetworkSecurityGroup(networkSecurityGroup);
                await armNetworkSecurityGroup.InitializeChildrenAsync(this._AzureContext);
                resourceGroupNetworkSecurityGroups.Add(armNetworkSecurityGroup);
                _AzureContext.LogProvider.WriteLog("GetAzureARMNetworkSecurityGroups", "Loaded ARM Network Security Group '" + armNetworkSecurityGroup.Name + "'.");
                _AzureContext.StatusProvider.UpdateStatus("Loaded ARM Network Security Group '" + armNetworkSecurityGroup.Name + "'.");
            }

            resourceGroup.AzureSubscription.ArmNetworkSecurityGroups.Add(resourceGroup, resourceGroupNetworkSecurityGroups);
            return resourceGroupNetworkSecurityGroups;
        }

        public async Task<List<Arm.RouteTable>> GetAzureARMRouteTables(ResourceGroup resourceGroup)
        {
            if (resourceGroup == null)
                throw new ArgumentException("ResourceGroup parameter must be provided");

            _AzureContext.LogProvider.WriteLog("GetAzureARMRouteTables", "Start - '" + resourceGroup.ToString() + "' Resource Group");

            if (resourceGroup.AzureSubscription.ArmRouteTables.ContainsKey(resourceGroup))
                return resourceGroup.AzureSubscription.ArmRouteTables[resourceGroup];

            JObject routeTableJson = await this.GetAzureARMResources("RouteTables", resourceGroup, null);

            var routeTables = from routeTable in routeTableJson["value"]
                              select routeTable;

            List<Arm.RouteTable> resourceGroupRouteTables = new List<Arm.RouteTable>();

            foreach (var routeTable in routeTables)
            {
                Arm.RouteTable armRouteTable = new Arm.RouteTable(routeTable);
                await armRouteTable.InitializeChildrenAsync(this._AzureContext);
                resourceGroupRouteTables.Add(armRouteTable);
                _AzureContext.LogProvider.WriteLog("GetAzureARMRouteTables", "Loaded ARM Route Table'" + armRouteTable.Name + "'.");
                _AzureContext.StatusProvider.UpdateStatus("Loaded ARM Route Table '" + armRouteTable.Name + "'.");
            }

            resourceGroup.AzureSubscription.ArmRouteTables.Add(resourceGroup, resourceGroupRouteTables);
            return resourceGroupRouteTables;
        }

        public async Task<PublicIP> GetAzureARMPublicIP(string id)
        {
            ResourceGroup resourceGroup = await this.GetAzureARMResourceGroup(id);

            if (resourceGroup != null)
            {
                foreach (PublicIP publicIp in await this.GetAzureARMPublicIPs(resourceGroup))
                {
                    if (String.Compare(publicIp.Id, id) == 0)
                        return publicIp;
                }
            }

            return null;
        }

        public async Task<List<Arm.PublicIP>> GetAzureARMPublicIPs(ResourceGroup resourceGroup)
        {
            if (resourceGroup == null)
                throw new ArgumentException("ResourceGroup parameter must be provided");

            _AzureContext.LogProvider.WriteLog("GetAzureARMPublicIPs", "Start - '" + resourceGroup.ToString() + "' Resource Group");

            if (resourceGroup.AzureSubscription.ArmPublicIPs.ContainsKey(resourceGroup))
                return resourceGroup.AzureSubscription.ArmPublicIPs[resourceGroup];

            JObject publicIPJson = await this.GetAzureARMResources("PublicIPs", resourceGroup, null);

            var publicIPs = from publicIP in publicIPJson["value"]
                            select publicIP;

            List<Arm.PublicIP> resourceGroupPublicIPs = new List<Arm.PublicIP>();

            foreach (var publicIP in publicIPs)
            {
                Arm.PublicIP armPublicIP = new Arm.PublicIP(publicIP);
                await armPublicIP.InitializeChildrenAsync(_AzureContext);
                resourceGroupPublicIPs.Add(armPublicIP);
            }

            resourceGroup.AzureSubscription.ArmPublicIPs.Add(resourceGroup, resourceGroupPublicIPs);
            return resourceGroupPublicIPs;
        }

        public async Task<List<Arm.LoadBalancer>> GetAzureARMLoadBalancers(ResourceGroup resourceGroup)
        {
            if (resourceGroup == null)
                throw new ArgumentException("ResourceGroup parameter must be provided");

            _AzureContext.LogProvider.WriteLog("GetAzureARMLoadBalancers", "Start - '" + resourceGroup.ToString() + "' Resource Group");

            if (resourceGroup.AzureSubscription.ArmLoadBalancers.ContainsKey(resourceGroup))
                return resourceGroup.AzureSubscription.ArmLoadBalancers[resourceGroup];

            JObject loadBalancersJson = await this.GetAzureARMResources("LoadBalancers", resourceGroup, null);

            var loadBalancers = from loadBalancer in loadBalancersJson["value"]
                                select loadBalancer;

            List<Arm.LoadBalancer> resourceGroupLoadBalancers = new List<Arm.LoadBalancer>();

            foreach (var loadBalancer in loadBalancers)
            {
                Arm.LoadBalancer armLoadBalancer = new Arm.LoadBalancer(loadBalancer);
                await armLoadBalancer.InitializeChildrenAsync(_AzureContext);
                resourceGroupLoadBalancers.Add(armLoadBalancer);
            }

            resourceGroup.AzureSubscription.ArmLoadBalancers.Add(resourceGroup, resourceGroupLoadBalancers);
            return resourceGroupLoadBalancers;
        }

        #endregion

        private async Task<XmlDocument> GetAzureAsmResources(string resourceType, Hashtable info)
        {
            _AzureContext.LogProvider.WriteLog("GetAzuereAsmResources", "Start");

            _AzureContext.LogProvider.WriteLog("GetAzureASMResources", "Start REST Request");

            string url = null;
            switch (resourceType)
            {
                case "VirtualNetworks":
                    url = _AzureContext.AzureServiceUrls.GetASMServiceManagementUrl() + this.SubscriptionId + "/services/networking/virtualnetwork";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting Virtual Networks for Subscription ID : " + this.SubscriptionId + "...");
                    break;
                // https://msdn.microsoft.com/en-us/library/azure/dn469422.aspx
                case "RoleSize":
                    url = _AzureContext.AzureServiceUrls.GetASMServiceManagementUrl() + this.SubscriptionId + "/rolesizes";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting Role Sizes for Subscription ID : " + this.SubscriptionId + "...");
                    break;
                case "ClientRootCertificates":
                    url = _AzureContext.AzureServiceUrls.GetASMServiceManagementUrl() + this.SubscriptionId + "/services/networking/" + info["virtualnetworkname"] + "/gateway/clientrootcertificates";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting Client Root Certificates for Virtual Network : " + info["virtualnetworkname"] + "...");
                    break;
                case "ClientRootCertificate":
                    url = _AzureContext.AzureServiceUrls.GetASMServiceManagementUrl() + this.SubscriptionId + "/services/networking/" + info["virtualnetworkname"] + "/gateway/clientrootcertificates/" + info["thumbprint"];
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting certificate data for certificate : " + info["thumbprint"] + "...");
                    break;
                case "NetworkSecurityGroup":
                    url = _AzureContext.AzureServiceUrls.GetASMServiceManagementUrl() + this.SubscriptionId + "/services/networking/networksecuritygroups/" + info["name"] + "?detaillevel=Full";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting Network Security Group : " + info["name"] + "...");
                    break;
                case "NetworkSecurityGroups":
                    url = _AzureContext.AzureServiceUrls.GetASMServiceManagementUrl() + this.SubscriptionId + "/services/networking/networksecuritygroups";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting Network Security Groups");
                    break;
                case "RouteTable":
                    url = _AzureContext.AzureServiceUrls.GetASMServiceManagementUrl() + this.SubscriptionId + "/services/networking/routetables/" + info["name"] + "?detailLevel=full";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting Route Table : " + info["routetablename"] + "...");
                    break;
                case "NSGSubnet":
                    url = _AzureContext.AzureServiceUrls.GetASMServiceManagementUrl() + this.SubscriptionId + "/services/networking/virtualnetwork/" + info["virtualnetworkname"] + "/subnets/" + info["subnetname"] + "/networksecuritygroups";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting NSG for subnet " + info["subnetname"] + "...");
                    break;
                case "VirtualNetworkGateway":
                    url = _AzureContext.AzureServiceUrls.GetASMServiceManagementUrl() + this.SubscriptionId + "/services/networking/" + info["virtualnetworkname"] + "/gateway";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting Virtual Network Gateway : " + info["virtualnetworkname"] + "...");
                    break;
                case "VirtualNetworkGatewaySharedKey":
                    url = _AzureContext.AzureServiceUrls.GetASMServiceManagementUrl() + this.SubscriptionId + "/services/networking/" + info["virtualnetworkname"] + "/gateway/connection/" + info["localnetworksitename"] + "/sharedkey";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting Virtual Network Gateway Shared Key: " + info["localnetworksitename"] + "...");
                    break;
                case "StorageAccounts":
                    url = _AzureContext.AzureServiceUrls.GetASMServiceManagementUrl() + this.SubscriptionId + "/services/storageservices";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting Storage Accounts for Subscription ID : " + this.SubscriptionId + "...");
                    break;
                case "StorageAccount":
                    url = _AzureContext.AzureServiceUrls.GetASMServiceManagementUrl() + this.SubscriptionId + "/services/storageservices/" + info["name"];
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting Storage Account '" + info["name"] + " ' for Subscription ID : " + this.SubscriptionId + "...");
                    break;
                case "StorageAccountKeys":
                    url = _AzureContext.AzureServiceUrls.GetASMServiceManagementUrl() + this.SubscriptionId + "/services/storageservices/" + info["name"] + "/keys";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting Storage Account '" + info["name"] + "' Keys.");
                    break;
                case "CloudServices":
                    url = _AzureContext.AzureServiceUrls.GetASMServiceManagementUrl() + this.SubscriptionId + "/services/hostedservices";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting Cloud Services for Subscription ID : " + this.SubscriptionId + "...");
                    break;
                case "CloudService":
                    url = _AzureContext.AzureServiceUrls.GetASMServiceManagementUrl() + this.SubscriptionId + "/services/hostedservices/" + info["name"] + "?embed-detail=true";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting Virtual Machines for Cloud Service : " + info["name"] + "...");
                    break;
                case "VirtualMachine":
                    url = _AzureContext.AzureServiceUrls.GetASMServiceManagementUrl() + this.SubscriptionId + "/services/hostedservices/" + info["cloudservicename"] + "/deployments/" + info["deploymentname"] + "/roles/" + info["virtualmachinename"];
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting Virtual Machine '" + info["virtualmachinename"] + "' for Cloud Service '" + info["virtualmachinename"] + "'");
                    break;
                case "VMImages":
                    url = _AzureContext.AzureServiceUrls.GetASMServiceManagementUrl() + this.SubscriptionId + "/services/images";
                    break;
                case "ReservedIPs":
                    url = _AzureContext.AzureServiceUrls.GetASMServiceManagementUrl() + this.SubscriptionId + "/services/networking/reservedips";
                    break;
                case "AffinityGroup":
                    url = _AzureContext.AzureServiceUrls.GetASMServiceManagementUrl() + this.SubscriptionId + "/affinitygroups/" + info["affinitygroupname"];
                    break;
                case "Locations":
                    url = _AzureContext.AzureServiceUrls.GetASMServiceManagementUrl() + this.SubscriptionId + "/locations";
                    break;
                default:
                    throw new ArgumentException("Unknown ResourceType: " + resourceType);
            }

            AuthenticationResult asmToken = await _AzureContext.TokenProvider.GetToken(_AzureContext.AzureServiceUrls.GetASMServiceManagementUrl(), this.AzureAdTenantId);

            AzureRestRequest azureRestRequest = new AzureRestRequest(url, asmToken);
            azureRestRequest.Headers.Add("x-ms-version", "2015-04-01");
            AzureRestResponse azureRestResponse = await _AzureContext.AzureRetriever.GetAzureRestResponse(azureRestRequest);

            return RemoveXmlns(azureRestResponse.Response);
        }

        public static XmlDocument RemoveXmlns(String xml)
        {
            if (!xml.StartsWith("<"))
            {
                xml = $"<data>{xml}</data>";
            }
            XDocument d = XDocument.Parse(xml);
            d.Root.DescendantsAndSelf().Attributes().Where(x => x.IsNamespaceDeclaration).Remove();

            foreach (var elem in d.Descendants())
                elem.Name = elem.Name.LocalName;

            var xmlDocument = new XmlDocument();
            xmlDocument.Load(d.CreateReader());

            return xmlDocument;
        }

        private async Task<JObject> GetAzureARMResources(string resourceType, ResourceGroup resourceGroup, Hashtable info)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureARMResources", "Start");

            string methodType = "GET";
            string url = null;
            bool useCached = true;

            if (_AzureContext == null)
                throw new ArgumentNullException("AzureContext is null.  Unable to call Azure API without Azure Context.");
            if (_AzureContext.TokenProvider == null)
                throw new ArgumentNullException("TokenProvider Context is null.  Unable to call Azure API without TokenProvider.");

            switch (resourceType)
            {
                case "ResourceGroups":
                    // https://docs.microsoft.com/en-us/rest/api/resources/resourcegroups#ResourceGroups_List
                    url = this.ApiUrl + "subscriptions/" + this.SubscriptionId + "/resourcegroups?api-version=2016-09-01";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting ARM Resource Groups...");
                    break;
                case "Locations":
                    // https://docs.microsoft.com/en-us/rest/api/resources/subscriptions#Subscriptions_ListLocations
                    url = this.ApiUrl + "subscriptions/" + this.SubscriptionId + ArmConst.Locations + "?api-version=2016-06-01";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting ARM Azure Locations for Subscription ID : " + this.SubscriptionId + "...");
                    break;
                case "AvailabilitySets":
                    // https://docs.microsoft.com/en-us/rest/api/compute/availabilitysets/availabilitysets-list-subscription
                    url = this.ApiUrl + "subscriptions/" + this.SubscriptionId + "/resourceGroups/" + resourceGroup.Name + ArmConst.ProviderAvailabilitySets + "?api-version=" + this.GetProviderMaxApiVersion("Microsoft.Compute", "availabilitySets");
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting ARM Availability Sets for Resource Group '" + resourceGroup.Name + "'.");
                    break;
                case "VirtualNetworks":
                    // https://msdn.microsoft.com/en-us/library/azure/mt163557.aspx
                    // https://docs.microsoft.com/en-us/rest/api/network/list-virtual-networks-within-a-subscription
                    url = this.ApiUrl + "subscriptions/" + this.SubscriptionId + "/resourceGroups/" + resourceGroup.Name + ArmConst.ProviderVirtualNetwork + "?api-version=" + this.GetProviderMaxApiVersion("Microsoft.Network", "virtualNetworks");
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting ARM Virtual Networks for Resource Group '" + resourceGroup.Name + "'.");
                    break;
                case "VirtualNetworkGateways":
                    // https://docs.microsoft.com/en-us/rest/api/network/virtualnetworkgateways#VirtualNetworkGateways_List
                    url = this.ApiUrl + "subscriptions/" + this.SubscriptionId + "/resourceGroups/" + resourceGroup.Name + ArmConst.ProviderVirtualNetworkGateways + "?api-version=" +this.GetProviderMaxApiVersion("Microsoft.Network", "virtualNetworkGateways");
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting ARM Virtual Network Gateways for Resource Group '" + resourceGroup.Name + "'.");
                    break;
                case "NetworkSecurityGroups":
                    // https://docs.microsoft.com/en-us/rest/api/network/networksecuritygroups#NetworkSecurityGroups_ListAll
                    url = this.ApiUrl + "subscriptions/" + this.SubscriptionId + "/resourceGroups/" + resourceGroup.Name + ArmConst.ProviderNetworkSecurityGroups + "?api-version=" + this.GetProviderMaxApiVersion("Microsoft.Network", "networkSecurityGroups");
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting ARM Network Security Groups for Resource Group '" + resourceGroup.Name + "'.");
                    break;
                case "NetworkInterfaces":
                    // https://docs.microsoft.com/en-us/rest/api/network/networkinterfaces#NetworkInterfaces_ListAll
                    url = this.ApiUrl + "subscriptions/" + this.SubscriptionId + "/resourceGroups/" + resourceGroup.Name + ArmConst.ProviderNetworkInterfaces + "?api-version=2017-03-01";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting ARM Network Interfaces for Resource Group '" + resourceGroup.Name + "'.");
                    break;
                case "StorageAccounts":
                    // https://docs.microsoft.com/en-us/rest/api/storagerp/storageaccounts#StorageAccounts_List
                    url = this.ApiUrl + "subscriptions/" + this.SubscriptionId + "/resourceGroups/" + resourceGroup.Name + ArmConst.ProviderStorageAccounts + "?api-version=2016-01-01";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting ARM Storage Accounts for Resource Group '" + resourceGroup.Name + "'.");
                    break;
                case "StorageAccountKeys":
                    // https://docs.microsoft.com/en-us/rest/api/storagerp/storageaccounts#StorageAccounts_ListKeys
                    methodType = "POST";
                    url = this.ApiUrl + "subscriptions/" + this.SubscriptionId + "/resourceGroups/" + resourceGroup.Name + ArmConst.ProviderStorageAccounts + info["StorageAccountName"] + "/listKeys?api-version=2016-01-01";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting ARM Storage Account Key for Subscription ID : " + this.SubscriptionId + " / Storage Account: " + info["StorageAccountName"] + " ...");
                    break;
                case "VirtualMachines":
                    // https://docs.microsoft.com/en-us/rest/api/compute/virtualmachines/virtualmachines-list-subscription
                    url = this.ApiUrl + "subscriptions/" + this.SubscriptionId + "/resourceGroups/" + resourceGroup.Name + ArmConst.ProviderVirtualMachines + "?api-version=" + this.GetProviderMaxApiVersion("Microsoft.Compute", "virtualMachines");
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting ARM Virtual Machines for Resource Group '" + resourceGroup.Name + "'.");
                    break;
                case "VirtualMachineImages":
                    // https://docs.microsoft.com/en-us/rest/api/compute/manageddisks/images/images-list-by-resource-group
                    url = this.ApiUrl + "subscriptions/" + this.SubscriptionId + "/resourceGroups/" + resourceGroup.Name + ArmConst.ProviderVirtualMachineImages + "?api-version=2016-04-30-preview";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting ARM Virtual Machine Images for Resource Group '" + resourceGroup.Name + "'.");
                    break;
                case "ManagedDisks":
                    // https://docs.microsoft.com/en-us/rest/api/manageddisks/disks/disks-list-by-subscription
                    url = this.ApiUrl + "subscriptions/" + this.SubscriptionId + "/resourceGroups/" + resourceGroup.Name + ArmConst.ProviderManagedDisks + "?api-version=" + this.GetProviderMaxApiVersion("Microsoft.Compute", "disks");
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting ARM Managed Disks for Resource Group '" + resourceGroup.Name + "'.");
                    break;
                case "LoadBalancers":
                    // https://docs.microsoft.com/en-us/rest/api/network/loadbalancer/list-load-balancers-within-a-subscription
                    url = this.ApiUrl + "subscriptions/" + this.SubscriptionId + "/resourceGroups/" + resourceGroup.Name + ArmConst.ProviderLoadBalancers + "?api-version=2016-09-01";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting ARM Load Balancers for Resource Group '" + resourceGroup.Name + "'.");
                    break;
                case "PublicIPs":
                    // https://docs.microsoft.com/en-us/rest/api/network/virtualnetwork/list-public-ip-addresses-within-a-resource-group
                    url = this.ApiUrl + "subscriptions/" + this.SubscriptionId + "/resourceGroups/" + resourceGroup.Name + ArmConst.ProviderPublicIpAddress + "?api-version=" + this.GetProviderMaxApiVersion("Microsoft.Network", "publicIPAddresses");
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting ARM Public IPs for Resource Group '" + resourceGroup.Name + "'.");
                    break;
                case "RouteTables":
                    // https://docs.microsoft.com/en-us/rest/api/virtualnetwork/routetables/list
                    url = this.ApiUrl + "subscriptions/" + this.SubscriptionId + "/resourceGroups/" + resourceGroup.Name + ArmConst.ProviderRouteTables + "?api-version=" + this.GetProviderMaxApiVersion("Microsoft.Network", "routeTables"); 
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting ARM Route Tables for Resource Group '" + resourceGroup.Name + "'.");
                    break;
                default:
                    throw new ArgumentException("Unknown ResourceType: " + resourceType);
            }

            AuthenticationResult armToken = await _AzureContext.TokenProvider.GetToken(this.TokenResourceUrl, this.AzureAdTenantId);

            AzureRestRequest azureRestRequest = new AzureRestRequest(url, armToken, methodType, useCached);
            AzureRestResponse azureRestResponse = await _AzureContext.AzureRetriever.GetAzureRestResponse(azureRestRequest);
            return JObject.Parse(azureRestResponse.Response);
        }

        private async Task LoadARMManagedDisks(ResourceGroup resourceGroup, TargetSettings targetSettings)
        {
            if (resourceGroup == null)
                throw new ArgumentException("ResourceGroup parameter must be provided");

            foreach (Arm.ManagedDisk armManagedDisk in await this.GetAzureARMManagedDisks(resourceGroup))
            {
                MigrationTarget.Disk targetManagedDisk = new MigrationTarget.Disk(armManagedDisk, null, targetSettings);
                this.ArmTargetManagedDisks.Add(targetManagedDisk);
            }
        }

        private async Task LoadARMPublicIPs(ResourceGroup resourceGroup, TargetSettings targetSettings)
        {
            if (resourceGroup == null)
                throw new ArgumentException("ResourceGroup parameter must be provided");

            foreach (Arm.PublicIP armPublicIp in await this.GetAzureARMPublicIPs(resourceGroup))
            {
                MigrationTarget.PublicIp targetPublicIP = new MigrationTarget.PublicIp(armPublicIp, targetSettings);
                this.ArmTargetPublicIPs.Add(targetPublicIP);
            }
        }

        private async Task LoadARMNetworkInterfaces(ResourceGroup armResourceGroup, List<MigrationTarget.VirtualNetwork> armVirtualNetworks, List<MigrationTarget.NetworkSecurityGroup> armNetworkSecurityGroups, TargetSettings targetSettings)
        {
            foreach (Arm.NetworkInterface armNetworkInterface in await this.GetAzureARMNetworkInterfaces(armResourceGroup))
            {
                MigrationTarget.NetworkInterface targetNetworkInterface = new MigrationTarget.NetworkInterface(armNetworkInterface, armVirtualNetworks, armNetworkSecurityGroups, targetSettings);
                this.ArmTargetNetworkInterfaces.Add(targetNetworkInterface);
            }
        }

        private async Task LoadARMLoadBalancers(ResourceGroup resourceGroup, TargetSettings targetSettings)
        {
            if (resourceGroup == null)
                throw new ArgumentException("ResourceGroup parameter must be provided");

            foreach (Arm.LoadBalancer armLoadBalancer in await this.GetAzureARMLoadBalancers(resourceGroup))
            {
                MigrationTarget.LoadBalancer targetLoadBalancer = new MigrationTarget.LoadBalancer(armLoadBalancer, targetSettings);
                foreach (Azure.MigrationTarget.FrontEndIpConfiguration targetFrontEndIpConfiguration in targetLoadBalancer.FrontEndIpConfigurations)
                {
                    if (targetFrontEndIpConfiguration.Source != null && targetFrontEndIpConfiguration.Source.PublicIP != null)
                    {
                        foreach (Azure.MigrationTarget.PublicIp targetPublicIp in this.ArmTargetPublicIPs)
                        {
                            if (targetPublicIp.SourceName == targetFrontEndIpConfiguration.Source.PublicIP.Name)
                            {
                                targetFrontEndIpConfiguration.PublicIp = targetPublicIp;
                            }
                        }
                    }
                }
                this.ArmTargetLoadBalancers.Add(targetLoadBalancer);
            }
        }
        private async Task LoadARMAvailabilitySets(ResourceGroup resourceGroup, TargetSettings targetSettings)
        {
            if (resourceGroup == null)
                throw new ArgumentException("ResourceGroup parameter must be provided");

            foreach (Arm.AvailabilitySet armAvailabilitySet in await this.GetAzureARMAvailabilitySets(resourceGroup))
            {
                MigrationTarget.AvailabilitySet targetAvailabilitySet = new MigrationTarget.AvailabilitySet(armAvailabilitySet, targetSettings);
                this.ArmTargetAvailabilitySets.Add(targetAvailabilitySet);
            }
        }
      
        private async Task LoadARMVirtualMachines(ResourceGroup resourceGroup, TargetSettings targetSettings)
        {
            if (resourceGroup == null)
                throw new ArgumentException("ResourceGroup parameter must be provided");

            foreach (Arm.VirtualMachine armVirtualMachine in await this.GetAzureArmVirtualMachines(resourceGroup))
            {
                MigrationTarget.VirtualMachine targetVirtualMachine = new MigrationTarget.VirtualMachine(armVirtualMachine, this, targetSettings);
                this.ArmTargetVirtualMachines.Add(targetVirtualMachine);

                if (armVirtualMachine.AvailabilitySet != null)
                {
                    foreach (MigrationTarget.AvailabilitySet targetAvailabilitySet in this.ArmTargetAvailabilitySets)
                    {
                        if (targetAvailabilitySet.SourceAvailabilitySet != null)
                        {
                            Arm.AvailabilitySet sourceAvailabilitySet = (Arm.AvailabilitySet)targetAvailabilitySet.SourceAvailabilitySet;
                            if (sourceAvailabilitySet.Id == armVirtualMachine.AvailabilitySet.Id)
                            {
                                targetVirtualMachine.TargetAvailabilitySet = targetAvailabilitySet;
                            }
                        }
                    }
                }

                foreach (MigrationTarget.NetworkInterface networkInterface in targetVirtualMachine.NetworkInterfaces)
                {
                    if (networkInterface.SourceNetworkInterface != null)
                    {
                        Arm.NetworkInterface sourceNetworkInterface = (Arm.NetworkInterface)networkInterface.SourceNetworkInterface;
                        if (sourceNetworkInterface.NetworkSecurityGroup != null)
                        {
                            foreach (Azure.MigrationTarget.NetworkSecurityGroup targetNetworkSecurityGroup in this.ArmTargetNetworkSecurityGroups)
                            {
                                if (targetNetworkSecurityGroup.TargetName == sourceNetworkInterface.NetworkSecurityGroup.Name)
                                    networkInterface.TargetNetworkSecurityGroup = targetNetworkSecurityGroup;
                            }
                        }
                    }

                    foreach (MigrationTarget.NetworkInterfaceIpConfiguration networkInterfaceIpConfiguration in networkInterface.TargetNetworkInterfaceIpConfigurations)
                    {
                        if (networkInterfaceIpConfiguration.SourceIpConfiguration != null)
                        {
                            Arm.NetworkInterfaceIpConfiguration sourceIpConfiguration = (Arm.NetworkInterfaceIpConfiguration)networkInterfaceIpConfiguration.SourceIpConfiguration;

                            if (sourceIpConfiguration.PublicIP != null)
                            {
                                foreach (Azure.MigrationTarget.PublicIp targetPublicIp in this.ArmTargetPublicIPs)
                                {
                                    if (targetPublicIp.TargetName == sourceIpConfiguration.PublicIP.Name)
                                        networkInterfaceIpConfiguration.TargetPublicIp = targetPublicIp;
                                }
                            }


                        }
                    }
                }
            }
        }

        private async Task LoadARMStorageAccounts(ResourceGroup resourceGroup, TargetSettings targetSettings)
        {
            if (resourceGroup == null)
                throw new ArgumentException("ResourceGroup parameter must be provided");

            foreach (Arm.StorageAccount armStorageAccount in await this.GetAzureARMStorageAccounts(resourceGroup))
            {
                MigrationTarget.StorageAccount targetStorageAccount = new MigrationTarget.StorageAccount(armStorageAccount, targetSettings);
                this.ArmTargetStorageAccounts.Add(targetStorageAccount);
            }
        }

        private async Task LoadARMVirtualNetworks(ResourceGroup resourceGroup, TargetSettings targetSettings)
        {
            if (resourceGroup == null)
                throw new ArgumentException("ResourceGroup parameter must be provided");

            foreach (Arm.VirtualNetwork armVirtualNetwork in await this.GetAzureARMVirtualNetworks(resourceGroup))
            {
                MigrationTarget.VirtualNetwork targetVirtualNetwork = new MigrationTarget.VirtualNetwork(armVirtualNetwork, this.ArmTargetNetworkSecurityGroups, this.ArmTargetRouteTables, targetSettings);
                this.ArmTargetVirtualNetworks.Add(targetVirtualNetwork);
            }
        }

        private async Task LoadARMNetworkSecurityGroups(ResourceGroup resourceGroup, TargetSettings targetSettings)
        {
            if (resourceGroup == null)
                throw new ArgumentException("ResourceGroup parameter must be provided");

            foreach (Arm.NetworkSecurityGroup armNetworkSecurityGroup in await this.GetAzureARMNetworkSecurityGroups(resourceGroup))
            {
                MigrationTarget.NetworkSecurityGroup targetNetworkSecurityGroup = new MigrationTarget.NetworkSecurityGroup(armNetworkSecurityGroup, targetSettings);
                this.ArmTargetNetworkSecurityGroups.Add(targetNetworkSecurityGroup);
            }
        }
        private async Task LoadARMRouteTables(ResourceGroup resourceGroup, TargetSettings targetSettings)
        {
            if (resourceGroup == null)
                throw new ArgumentException("ResourceGroup parameter must be provided");

            foreach (Arm.RouteTable armRouteTable in await this.GetAzureARMRouteTables(resourceGroup))
            {
                MigrationTarget.RouteTable targetRouteTable = new MigrationTarget.RouteTable(armRouteTable, targetSettings);
                this.ArmTargetRouteTables.Add(targetRouteTable);
            }
        }

    }
}
