using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using MigAz.Azure.Arm;
using MigAz.Azure.Asm;

namespace MigAz.Azure
{
    public class AzureRetriever
    {
        private AzureContext _AzureContext;
        private object _lockObject = new object();
        private AzureSubscription _AzureSubscription = null;
        private List<AzureSubscription> _AzureSubscriptions;

        // ASM Object Cache (Subscription Context Specific)
        private List<AsmVirtualNetwork> _VirtualNetworks;
        private List<AsmStorageAccount> _StorageAccounts;
        private List<AsmCloudService> _CloudServices;
        private List<AsmReservedIP> _AsmReservedIPs;

        // ARM Object Cache (Subscription Context Specific)
        private List<ArmLocation> _ArmLocations;
        private List<AzureTenant> _ArmTenants;
        private List<AzureSubscription> _ArmSubscriptions;
        private List<ArmResourceGroup> _ArmResourceGroups;
        private List<ArmVirtualNetwork> _ArmVirtualNetworks;
        private List<ArmStorageAccount> _ArmStorageAccounts;
        private List<ArmAvailabilitySet> _ArmAvailabilitySets;

        private Dictionary<string, XmlDocument> _asmXmlDocumentCache;
        private Dictionary<string, JObject> _armJsonDocumentCache;

        private AzureRetriever() { }

        public AzureRetriever(AzureContext azureContext)
        {
            _AzureContext = azureContext;
        }

        public void ClearCache()
        {
            _asmXmlDocumentCache = null;
            _armJsonDocumentCache = null;
            _ArmLocations = null;
            _ArmTenants = null;
            _ArmSubscriptions = null;
            _ArmResourceGroups = null;
            _ArmVirtualNetworks = null;
            _ArmStorageAccounts = null;
            _ArmAvailabilitySets = null;
            _VirtualNetworks = null;
            _StorageAccounts = null;
            _CloudServices = null;
        }

        protected XmlDocument RemoveXmlns(String xml)
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

        private void writeRetreiverResultToLog(string url, string xml)
        {
            lock (_lockObject)
            {
                string logfilepath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\MigAz\\MigAz-XML-" + string.Format("{0:yyyyMMdd}", DateTime.Now) + ".log";
                string text = DateTime.Now.ToString() + "   " + url + Environment.NewLine;
                File.AppendAllText(logfilepath, text);
                text = xml + Environment.NewLine;
                File.AppendAllText(logfilepath, text);
                text = Environment.NewLine;
                File.AppendAllText(logfilepath, text);
            }
        }

        internal ArmAvailabilitySet GetAzureARMAvailabilitySet(AsmVirtualMachine asmVirtualMachine)
        {
            if (_ArmAvailabilitySets == null)
                _ArmAvailabilitySets = new List<ArmAvailabilitySet>();

            foreach (ArmAvailabilitySet armAvailabilitySet in _ArmAvailabilitySets)
            {
                if (armAvailabilitySet.name == asmVirtualMachine.GetDefaultAvailabilitySetName())
                    return armAvailabilitySet;
            }

            ArmAvailabilitySet newArmAvailabilitySet = new ArmAvailabilitySet(this._AzureContext, asmVirtualMachine);
            _ArmAvailabilitySets.Add(newArmAvailabilitySet);

            return newArmAvailabilitySet;
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

        internal async Task SetSubscriptionContext(AzureSubscription azureSubscription)
        {
            ClearCache();
            _AzureSubscription = azureSubscription;
            await this._AzureContext.TokenProvider.GetToken(azureSubscription);
        }

        public async Task<List<AzureSubscription>> GetSubscriptions()
        {
            if (_AzureSubscriptions != null)
                return _AzureSubscriptions;

            _AzureSubscriptions = new List<AzureSubscription>();
            XmlDocument subscriptionsXml = await this.GetAzureAsmResources("Subscriptions", null);
            foreach (XmlNode subscriptionXml in subscriptionsXml.SelectNodes("//Subscription"))
            {
                AzureSubscription azureSubscription = new AzureSubscription(subscriptionXml, this._AzureContext.AzureEnvironment);
                _AzureSubscriptions.Add(azureSubscription);
            }

            return _AzureSubscriptions;
        }

        #region ASM Methods

        private async Task<XmlDocument> GetAzureAsmResources(string resourceType, Hashtable info)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureASMResources", "Start");

            string url = null;
            switch (resourceType)
            {
                case "Subscriptions":
                    url = AzureServiceUrls.GetASMServiceManagementUrl(this._AzureContext.AzureEnvironment) + "subscriptions";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting Subscriptions...");
                    break;
                case "VirtualNetworks":
                    url = AzureServiceUrls.GetASMServiceManagementUrl(this._AzureContext.AzureEnvironment) + _AzureSubscription.SubscriptionId + "/services/networking/virtualnetwork";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting Virtual Networks for Subscription ID : " + _AzureSubscription.SubscriptionId + "...");
                    break;
                case "ClientRootCertificates":
                    url = AzureServiceUrls.GetASMServiceManagementUrl(this._AzureContext.AzureEnvironment) + _AzureSubscription.SubscriptionId + "/services/networking/" + info["virtualnetworkname"] + "/gateway/clientrootcertificates";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting Client Root Certificates for Virtual Network : " + info["virtualnetworkname"] + "...");
                    break;
                case "ClientRootCertificate":
                    url = AzureServiceUrls.GetASMServiceManagementUrl(this._AzureContext.AzureEnvironment) + _AzureSubscription.SubscriptionId + "/services/networking/" + info["virtualnetworkname"] + "/gateway/clientrootcertificates/" + info["thumbprint"];
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting certificate data for certificate : " + info["thumbprint"] + "...");
                    break;
                case "NetworkSecurityGroup":
                    url = AzureServiceUrls.GetASMServiceManagementUrl(this._AzureContext.AzureEnvironment) + _AzureSubscription.SubscriptionId + "/services/networking/networksecuritygroups/" + info["name"] + "?detaillevel=Full";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting Network Security Group : " + info["name"] + "...");
                    break;
                case "NetworkSecurityGroups":
                    url = AzureServiceUrls.GetASMServiceManagementUrl(this._AzureContext.AzureEnvironment) + _AzureSubscription.SubscriptionId + "/services/networking/networksecuritygroups";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting Network Security Groups");
                    break;
                case "RouteTable":
                    url = AzureServiceUrls.GetASMServiceManagementUrl(this._AzureContext.AzureEnvironment) + _AzureSubscription.SubscriptionId + "/services/networking/routetables/" + info["name"] + "?detailLevel=full";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting Route Table : " + info["routetablename"] + "...");
                    break;
                case "NSGSubnet":
                    url = AzureServiceUrls.GetASMServiceManagementUrl(this._AzureContext.AzureEnvironment) + _AzureSubscription.SubscriptionId + "/services/networking/virtualnetwork/" + info["virtualnetworkname"] + "/subnets/" + info["subnetname"] + "/networksecuritygroups";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting NSG for subnet " + info["subnetname"] + "...");
                    break;
                case "VirtualNetworkGateway":
                    url = AzureServiceUrls.GetASMServiceManagementUrl(this._AzureContext.AzureEnvironment) + _AzureSubscription.SubscriptionId + "/services/networking/" + info["virtualnetworkname"] + "/gateway";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting Virtual Network Gateway : " + info["virtualnetworkname"] + "...");
                    break;
                case "VirtualNetworkGatewaySharedKey":
                    url = AzureServiceUrls.GetASMServiceManagementUrl(this._AzureContext.AzureEnvironment) + _AzureSubscription.SubscriptionId + "/services/networking/" + info["virtualnetworkname"] + "/gateway/connection/" + info["localnetworksitename"] + "/sharedkey";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting Virtual Network Gateway Shared Key: " + info["localnetworksitename"] + "...");
                    break;
                case "StorageAccounts":
                    url = AzureServiceUrls.GetASMServiceManagementUrl(this._AzureContext.AzureEnvironment) + _AzureSubscription.SubscriptionId + "/services/storageservices";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting Storage Accounts for Subscription ID : " + _AzureSubscription.SubscriptionId + "...");
                    break;
                case "StorageAccount":
                    url = AzureServiceUrls.GetASMServiceManagementUrl(this._AzureContext.AzureEnvironment) + _AzureSubscription.SubscriptionId + "/services/storageservices/" + info["name"];
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting Storage Account '" + info["name"] + " ' for Subscription ID : " + _AzureSubscription.SubscriptionId + "...");
                    break;
                case "StorageAccountKeys":
                    url = AzureServiceUrls.GetASMServiceManagementUrl(this._AzureContext.AzureEnvironment) + _AzureSubscription.SubscriptionId + "/services/storageservices/" + info["name"] + "/keys";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting Storage Account '" + info["name"] + "' Keys.");
                    break;
                case "CloudServices":
                    url = AzureServiceUrls.GetASMServiceManagementUrl(this._AzureContext.AzureEnvironment) + _AzureSubscription.SubscriptionId + "/services/hostedservices";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting Cloud Services for Subscription ID : " + _AzureSubscription.SubscriptionId + "...");
                    break;
                case "CloudService":
                    url = AzureServiceUrls.GetASMServiceManagementUrl(this._AzureContext.AzureEnvironment) + _AzureSubscription.SubscriptionId + "/services/hostedservices/" + info["name"] + "?embed-detail=true";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting Virtual Machines for Cloud Service : " + info["name"] + "...");
                    break;
                case "VirtualMachine":
                    url = AzureServiceUrls.GetASMServiceManagementUrl(this._AzureContext.AzureEnvironment) + _AzureSubscription.SubscriptionId + "/services/hostedservices/" + info["cloudservicename"] + "/deployments/" + info["deploymentname"] + "/roles/" + info["virtualmachinename"];
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting Virtual Machine '" + info["virtualmachinename"] + "' for Cloud Service '" + info["virtualmachinename"] + "'");
                    break;
                case "VMImages":
                    url = AzureServiceUrls.GetASMServiceManagementUrl(this._AzureContext.AzureEnvironment) + _AzureSubscription.SubscriptionId + "/services/images";
                    break;
                case "ReservedIPs":
                    url = AzureServiceUrls.GetASMServiceManagementUrl(this._AzureContext.AzureEnvironment) + _AzureSubscription.SubscriptionId + "/services/networking/reservedips";
                    break;
                case "AffinityGroup":
                    url = AzureServiceUrls.GetASMServiceManagementUrl(this._AzureContext.AzureEnvironment) + _AzureSubscription.SubscriptionId + "/affinitygroups/" + info["affinitygroupname"];
                    break;
                case "Locations":
                    url = AzureServiceUrls.GetASMServiceManagementUrl(this._AzureContext.AzureEnvironment) + _AzureSubscription.SubscriptionId + "/locations";
                    break;
            }

            _AzureContext.LogProvider.WriteLog("GetAzureASMResources", "GET " + url);

            if (_asmXmlDocumentCache == null)
                _asmXmlDocumentCache = new Dictionary<string, XmlDocument>();

            if (_asmXmlDocumentCache.ContainsKey(url))
            {
                _AzureContext.LogProvider.WriteLog("GetAzureASMResources", "FROM XML CACHE");
                _AzureContext.LogProvider.WriteLog("GetAzureASMResources", "End");
                writeRetreiverResultToLog(url, "Cached");
                return _asmXmlDocumentCache[url];
            }

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + _AzureContext.TokenProvider.AuthenticationResult.AccessToken);
            request.Headers.Add("x-ms-version", "2015-04-01");
            request.Method = "GET";

            string xml = String.Empty;
            try
            {
                HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
                xml = new StreamReader(response.GetResponseStream()).ReadToEnd();
                _AzureContext.LogProvider.WriteLog("GetAzureASMResources", "RESPONSE " + response.StatusCode);
            }
            catch (Exception exception)
            {
                _AzureContext.LogProvider.WriteLog("GetAzureASMResources", "EXCEPTION: " + exception.Message + "  URL: " + url);
                throw exception;
            }

            if (xml != String.Empty)
            {
                XmlDocument xmlDoc = RemoveXmlns(xml);

                _AzureContext.LogProvider.WriteLog("GetAzureASMResources", "End");
                writeRetreiverResultToLog(url, xml);

                if (!_asmXmlDocumentCache.ContainsKey(url))
                    _asmXmlDocumentCache.Add(url, xmlDoc);

                return xmlDoc;
            }
            else
            {
                _AzureContext.LogProvider.WriteLog("GetAzureASMResources", "End");
                writeRetreiverResultToLog(url, String.Empty);
                return null;
            }
        }

        public async virtual Task<List<AsmLocation>> GetAzureASMLocations()
        {
            XmlNode locationsXml = await this.GetAzureAsmResources("Locations", null);
            List<AsmLocation> azureLocations = new List<AsmLocation>();
            foreach (XmlNode locationXml in locationsXml.SelectNodes("/Locations/Location"))
            {
                azureLocations.Add(new AsmLocation(_AzureContext, locationXml));
            }

            return azureLocations;
        }

        public async virtual Task<List<AsmReservedIP>> GetAzureAsmReservedIPs()
        {
            if (_AsmReservedIPs != null)
                return _AsmReservedIPs;

            _AsmReservedIPs = new List<AsmReservedIP>();
            XmlDocument reservedIPsXml = await this.GetAzureAsmResources("ReservedIPs", null);
            foreach (XmlNode reservedIPXml in reservedIPsXml.SelectNodes("/ReservedIPs/ReservedIP"))
            {
                _AsmReservedIPs.Add(new AsmReservedIP(_AzureContext, reservedIPXml));
            }

            return _AsmReservedIPs;
        }

        public async virtual Task<List<AsmStorageAccount>> GetAzureAsmStorageAccounts()
        {
            if (_StorageAccounts != null)
                return _StorageAccounts;

            _StorageAccounts = new List<AsmStorageAccount>();
            XmlDocument storageAccountsXml = await this.GetAzureAsmResources("StorageAccounts", null);
            foreach (XmlNode storageAccountXml in storageAccountsXml.SelectNodes("//StorageService"))
            {
                AsmStorageAccount asmStorageAccount = new AsmStorageAccount(_AzureContext, storageAccountXml);
                _StorageAccounts.Add(asmStorageAccount);
            }

            _AzureContext.StatusProvider.UpdateStatus("BUSY: Loading Storage Account Keys");
            List<Task> storageAccountKeyTasks = new List<Task>();
            foreach (AsmStorageAccount asmStorageAccount in _StorageAccounts)
            {
                storageAccountKeyTasks.Add(asmStorageAccount.LoadStorageAccountKeysAsynch());
            }
            await Task.WhenAll(storageAccountKeyTasks);

            return _StorageAccounts;
        }

        public async virtual Task<AsmStorageAccount> GetAzureAsmStorageAccount(string storageAccountName)
        {
            foreach (AsmStorageAccount asmStorageAccount in await this.GetAzureAsmStorageAccounts())
            {
                if (asmStorageAccount.Name == storageAccountName)
                    return asmStorageAccount;
            }

            return null;
        }

        public async virtual Task<List<AsmVirtualNetwork>> GetAzureAsmVirtualNetworks()
        {
            if (_VirtualNetworks != null)
                return _VirtualNetworks;

            _VirtualNetworks = new List<AsmVirtualNetwork>();
            foreach (XmlNode virtualnetworksite in (await this.GetAzureAsmResources("VirtualNetworks", null)).SelectNodes("//VirtualNetworkSite"))
            {
                AsmVirtualNetwork asmVirtualNetwork = new AsmVirtualNetwork(_AzureContext, virtualnetworksite);
                await asmVirtualNetwork.InitializeChildrenAsync();
                _VirtualNetworks.Add(asmVirtualNetwork);
            }
            return _VirtualNetworks;
        }

        public async Task<AsmVirtualNetwork> GetAzureAsmVirtualNetwork(string virtualNetworkName)
        {
            foreach (AsmVirtualNetwork asmVirtualNetwork in await this.GetAzureAsmVirtualNetworks())
            {
                if (asmVirtualNetwork.Name == virtualNetworkName)
                {
                    return asmVirtualNetwork;
                }
            }

            return null;
        }

        internal async Task<AsmAffinityGroup> GetAzureAsmAffinityGroup(string affinityGroupName)
        {
            Hashtable affinitygroupinfo = new Hashtable();
            affinitygroupinfo.Add("affinitygroupname", affinityGroupName);

            XmlNode affinityGroupXml = await this.GetAzureAsmResources("AffinityGroup", affinitygroupinfo);
            AsmAffinityGroup asmAffinityGroup = new AsmAffinityGroup(_AzureContext, affinityGroupXml.SelectSingleNode("AffinityGroup"));
            return asmAffinityGroup;
        }

        public async virtual Task<AsmNetworkSecurityGroup> GetAzureAsmNetworkSecurityGroup(string networkSecurityGroupName)
        {
            Hashtable networkSecurityGroupInfo = new Hashtable();
            networkSecurityGroupInfo.Add("name", networkSecurityGroupName);

            XmlNode networkSecurityGroupXml = await this.GetAzureAsmResources("NetworkSecurityGroup", networkSecurityGroupInfo);
            AsmNetworkSecurityGroup asmNetworkSecurityGroup = new AsmNetworkSecurityGroup(_AzureContext, networkSecurityGroupXml.SelectSingleNode("NetworkSecurityGroup"));
            return asmNetworkSecurityGroup;
        }

        public async virtual Task<List<AsmNetworkSecurityGroup>> GetAzureAsmNetworkSecurityGroups()
        {
            List<AsmNetworkSecurityGroup> networkSecurityGroups = new List<AsmNetworkSecurityGroup>();

            XmlDocument x = await this.GetAzureAsmResources("NetworkSecurityGroups", null);
            foreach (XmlNode networkSecurityGroupNode in (await this.GetAzureAsmResources("NetworkSecurityGroups", null)).SelectNodes("//NetworkSecurityGroup"))
            {
                AsmNetworkSecurityGroup asmNetworkSecurityGroup = new AsmNetworkSecurityGroup(_AzureContext, networkSecurityGroupNode);
                networkSecurityGroups.Add(asmNetworkSecurityGroup);
            }

            return networkSecurityGroups;
        }

        public async virtual Task<AsmRouteTable> GetAzureAsmRouteTable(string routeTableName)
        {
            Hashtable info = new Hashtable();
            info.Add("name", routeTableName);
            XmlDocument routeTableXml = await this.GetAzureAsmResources("RouteTable", info);
            return new AsmRouteTable(_AzureContext, routeTableXml);
        }

        internal async Task<AsmVirtualMachine> GetAzureAsmVirtualMachine(AsmCloudService asmCloudService, string virtualMachineName)
        {
            Hashtable vmDetails = await this.GetVMDetails(asmCloudService.ServiceName, virtualMachineName);
            XmlDocument virtualMachineXml = await this.GetAzureAsmResources("VirtualMachine", vmDetails);
            AsmVirtualMachine asmVirtualMachine = new AsmVirtualMachine(this._AzureContext, asmCloudService, this._AzureContext.SettingsProvider, virtualMachineXml, vmDetails);
            await asmVirtualMachine.InitializeChildren();

            return asmVirtualMachine;
        }

        private async Task<Hashtable> GetVMDetails(string cloudServiceName, string virtualMachineName)
        {
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

        public async virtual Task<List<AsmCloudService>> GetAzureAsmCloudServices()
        {
            if (_CloudServices != null)
                return _CloudServices;

            XmlDocument cloudServicesXml = await this.GetAzureAsmResources("CloudServices", null);
            _CloudServices = new List<AsmCloudService>();
            foreach (XmlNode cloudServiceXml in cloudServicesXml.SelectNodes("//HostedService"))
            {
                AsmCloudService tempCloudService = new AsmCloudService(_AzureContext, cloudServiceXml);

                Hashtable cloudServiceInfo = new Hashtable();
                cloudServiceInfo.Add("name", tempCloudService.ServiceName);
                XmlDocument cloudServiceDetailXml = await this.GetAzureAsmResources("CloudService", cloudServiceInfo);
                AsmCloudService asmCloudService = new AsmCloudService(_AzureContext, cloudServiceDetailXml);

                _CloudServices.Add(asmCloudService);
            }

            List<Task> cloudServiceVMTasks = new List<Task>();
            foreach (AsmCloudService asmCloudService in _CloudServices)
            {
                cloudServiceVMTasks.Add(asmCloudService.LoadChildrenAsync());
            }

            await Task.WhenAll(cloudServiceVMTasks);

            return _CloudServices;
        }

        public async virtual Task<AsmCloudService> GetAzureAsmCloudService(string cloudServiceName)
        {
            foreach (AsmCloudService asmCloudService in await this.GetAzureAsmCloudServices())
            {
                if (asmCloudService.ServiceName == cloudServiceName)
                    return asmCloudService;
            }

            return null;
        }

        public async Task<AsmStorageAccountKeys> GetAzureAsmStorageAccountKeys(string storageAccountName)
        {
            Hashtable storageAccountInfo = new Hashtable();
            storageAccountInfo.Add("name", storageAccountName);

            XmlDocument storageAccountKeysXml = await this.GetAzureAsmResources("StorageAccountKeys", storageAccountInfo);
            return new AsmStorageAccountKeys(_AzureContext, storageAccountKeysXml);
        }

        public async virtual Task<List<AsmClientRootCertificate>> GetAzureAsmClientRootCertificates(AsmVirtualNetwork asmVirtualNetwork)
        {
            Hashtable virtualNetworkInfo = new Hashtable();
            virtualNetworkInfo.Add("virtualnetworkname", asmVirtualNetwork.Name);
            XmlDocument clientRootCertificatesXml = await this.GetAzureAsmResources("ClientRootCertificates", virtualNetworkInfo);

            List<AsmClientRootCertificate> asmClientRootCertificates = new List<AsmClientRootCertificate>();
            foreach (XmlNode clientRootCertificateXml in clientRootCertificatesXml.SelectNodes("//ClientRootCertificate"))
            {
                AsmClientRootCertificate asmClientRootCertificate = new AsmClientRootCertificate(_AzureContext, asmVirtualNetwork, clientRootCertificateXml);
                await asmClientRootCertificate.InitializeChildrenAsync();
                asmClientRootCertificates.Add(asmClientRootCertificate);
            }

            return asmClientRootCertificates;
        }

        public async Task<XmlDocument> GetAzureAsmClientRootCertificateData(AsmVirtualNetwork asmVirtualNetwork, string certificateThumbprint)
        {
            Hashtable certificateInfo = new Hashtable();
            certificateInfo.Add("virtualnetworkname", asmVirtualNetwork.Name);
            certificateInfo.Add("thumbprint", certificateThumbprint);
            XmlDocument clientRootCertificateXml = await this.GetAzureAsmResources("ClientRootCertificate", certificateInfo);
            return clientRootCertificateXml;
        }

        public async virtual Task<AsmVirtualNetworkGateway> GetAzureAsmVirtualNetworkGateway(AsmVirtualNetwork asmVirtualNetwork)
        {
            Hashtable virtualNetworkGatewayInfo = new Hashtable();
            virtualNetworkGatewayInfo.Add("virtualnetworkname", asmVirtualNetwork.Name);
            virtualNetworkGatewayInfo.Add("localnetworksitename", String.Empty);

            XmlDocument gatewayXml = await this.GetAzureAsmResources("VirtualNetworkGateway", virtualNetworkGatewayInfo);
            return new AsmVirtualNetworkGateway(_AzureContext, asmVirtualNetwork, gatewayXml);
        }

        public async virtual Task<string> GetAzureAsmVirtualNetworkSharedKey(string virtualNetworkName, string localNetworkSiteName)
        {
            Hashtable virtualNetworkGatewayInfo = new Hashtable();
            virtualNetworkGatewayInfo.Add("virtualnetworkname", virtualNetworkName);
            virtualNetworkGatewayInfo.Add("localnetworksitename", localNetworkSiteName);

            XmlDocument connectionShareKeyXml = await this.GetAzureAsmResources("VirtualNetworkGatewaySharedKey", virtualNetworkGatewayInfo);
            if (connectionShareKeyXml.SelectSingleNode("//Value") == null)
                return String.Empty;

            return connectionShareKeyXml.SelectSingleNode("//Value").InnerText;
        }

        #endregion

        #region ARM Methods

        private async Task<JObject> GetAzureARMResources(string resourceType, Hashtable info)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureARMResources", "Start");
            string methodType = "GET";

            string url = null;
            switch (resourceType)
            {
                case "Tenants":
                    url = AzureServiceUrls.GetARMServiceManagementUrl(this._AzureContext.AzureEnvironment) + "tenants?api-version=2015-01-01";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting Tenants...");
                    break;
                case "Subscriptions":
                    url = AzureServiceUrls.GetARMServiceManagementUrl(this._AzureContext.AzureEnvironment) + "subscriptions?api-version=2015-01-01";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting Subscriptions...");
                    break;
                case "ResourceGroups":
                    url = AzureServiceUrls.GetARMServiceManagementUrl(this._AzureContext.AzureEnvironment) + "subscriptions/" + _AzureSubscription.SubscriptionId + "/resourcegroups?api-version=2015-01-01";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting ARM Resource Groups...");
                    break;
                case "Locations":
                    // https://docs.microsoft.com/en-us/rest/api/resources/subscriptions#Subscriptions_ListLocations
                    url = AzureServiceUrls.GetARMServiceManagementUrl(this._AzureContext.AzureEnvironment) + "subscriptions/" + _AzureSubscription.SubscriptionId + ArmConst.Locations + "?api-version=2016-06-01";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting ARM Azure Locations for Subscription ID : " + _AzureSubscription.SubscriptionId + "...");
                    break;
                case "VirtualNetworks":
                    // https://msdn.microsoft.com/en-us/library/azure/mt163557.aspx
                    // https://docs.microsoft.com/en-us/rest/api/network/list-virtual-networks-within-a-subscription
                    url = AzureServiceUrls.GetARMServiceManagementUrl(this._AzureContext.AzureEnvironment) + "subscriptions/" + _AzureSubscription.SubscriptionId + ArmConst.ProviderVirtualNetwork + "?api-version=2016-12-01";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting ARM Virtual Networks for Subscription ID : " + _AzureSubscription.SubscriptionId + "...");
                    break;
                case "StorageAccounts":
                    // https://docs.microsoft.com/en-us/rest/api/storagerp/storageaccounts#StorageAccounts_List
                    url = AzureServiceUrls.GetARMServiceManagementUrl(this._AzureContext.AzureEnvironment) + "subscriptions/" + _AzureSubscription.SubscriptionId + ArmConst.ProviderStorageAccounts + "?api-version=2016-01-01";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting ARM Storage Accounts for Subscription ID : " + _AzureSubscription.SubscriptionId + "...");
                    break;
                case "StorageAccountKeys":
                    // https://docs.microsoft.com/en-us/rest/api/storagerp/storageaccounts#StorageAccounts_ListKeys
                    methodType = "POST";
                    url = AzureServiceUrls.GetARMServiceManagementUrl(this._AzureContext.AzureEnvironment) + "subscriptions/" + _AzureSubscription.SubscriptionId + "/resourceGroups/" + info["ResourceGroupName"] + ArmConst.ProviderStorageAccounts + info["StorageAccountName"] + "/listKeys?api-version=2016-01-01";
                    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting ARM Storage Accounts for Subscription ID : " + _AzureSubscription.SubscriptionId + "...");
                    break;
            }

            _AzureContext.LogProvider.WriteLog("GetAzureARMResources", "GET " + url);

            if (_armJsonDocumentCache == null)
                _armJsonDocumentCache = new Dictionary<string, JObject>();

            if (_armJsonDocumentCache.ContainsKey(url))
            {
                _AzureContext.LogProvider.WriteLog("GetAzureARMResources", "FROM JSON CACHE");
                _AzureContext.LogProvider.WriteLog("GetAzureARMResources", "End");
                writeRetreiverResultToLog(url, "Cached");
                return _armJsonDocumentCache[url];
            }

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + _AzureContext.TokenProvider.AuthenticationResult.AccessToken);
            request.ContentType = "application/json";
            request.Method = methodType;

            if (request.Method == "POST")
                request.ContentLength = 0;

            string webRequesetResult = String.Empty;
            try
            {
                HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
                webRequesetResult = new StreamReader(response.GetResponseStream()).ReadToEnd();
                _AzureContext.LogProvider.WriteLog("GetAzureARMResources", "RESPONSE " + response.StatusCode);
            }
            catch (Exception exception)
            {
                _AzureContext.LogProvider.WriteLog("GetAzureARMResources", "EXCEPTION: " + exception.Message + "  URL: " + url);
                throw exception;
            }

            if (webRequesetResult != String.Empty)
            {
                JObject webRequestResultJson = JObject.Parse(webRequesetResult);

                _AzureContext.LogProvider.WriteLog("GetAzureARMResources", "End");
                writeRetreiverResultToLog(url, webRequesetResult);

                if (!_armJsonDocumentCache.ContainsKey(url))
                    _armJsonDocumentCache.Add(url, webRequestResultJson);

                return webRequestResultJson;
            }
            else
            {
                _AzureContext.LogProvider.WriteLog("GetAzureARMResources", "End");
                writeRetreiverResultToLog(url, String.Empty);
                return null;
            }
        }

        public async Task<List<AzureTenant>> GetAzureARMTenants()
        {
            if (_ArmTenants != null)
                return _ArmTenants;

            JObject tenantsJson = await this.GetAzureARMResources("Tenants", null);

            var tenants = from tenant in tenantsJson["value"]
                                select tenant;

            _ArmTenants = new List<AzureTenant>();

            foreach (JObject tenantJson in tenants)
            {
                AzureTenant azureSubscription = new AzureTenant(tenantJson, _AzureContext.AzureEnvironment);
                _ArmTenants.Add(azureSubscription);
            }

            return _ArmTenants;
        }

        public async Task<List<AzureSubscription>> GetAzureARMSubscriptions()
        {
            if (_ArmSubscriptions != null)
                return _ArmSubscriptions;

            JObject subscriptionsJson = await this.GetAzureARMResources("Subscriptions", null);

            var subscriptions = from subscription in subscriptionsJson["value"]
                                select subscription;

            _ArmSubscriptions = new List<AzureSubscription>();

            foreach (JObject azureSubscriptionJson in subscriptions)
            {
                AzureSubscription azureSubscription = new AzureSubscription(azureSubscriptionJson, _AzureContext.AzureEnvironment);
                _ArmSubscriptions.Add(azureSubscription);
            }

            return _ArmSubscriptions;
        }

        public async Task<List<ArmResourceGroup>> GetAzureARMResourceGroups()
        {
            if (_ArmResourceGroups != null)
                return _ArmResourceGroups;

            JObject resourceGroupsJson = await this.GetAzureARMResources("ResourceGroups", null);

            var resourceGroups = from resourceGroup in resourceGroupsJson["value"]
                                select resourceGroup;

            _ArmResourceGroups = new List<ArmResourceGroup>();

            foreach (JObject resourceGroupJson in resourceGroups)
            {
                ArmResourceGroup azureSubscription = new ArmResourceGroup(resourceGroupJson, _AzureContext.AzureEnvironment);
                _ArmResourceGroups.Add(azureSubscription);
            }

            return _ArmResourceGroups;
        }

        public async virtual Task<ArmVirtualNetwork> GetAzureARMVirtualNetwork(string virtualNetworkName)
        {
            foreach (ArmVirtualNetwork armVirtualNetwork in await GetAzureARMVirtualNetworks())
            {
                if (armVirtualNetwork.Name == virtualNetworkName)
                    return armVirtualNetwork;
            }

            return null;
        }

        public async virtual Task<List<ArmVirtualNetwork>> GetAzureARMVirtualNetworks()
        {
            if (_ArmVirtualNetworks != null)
                return _ArmVirtualNetworks;

            JObject virtualNetworksJson = await this.GetAzureARMResources("VirtualNetworks", null);

            var virtualNetworks = from vnet in virtualNetworksJson["value"]
                                  select vnet;

            _ArmVirtualNetworks = new List<ArmVirtualNetwork>();

            foreach (var virtualNetwork in virtualNetworks)
            {
                ArmVirtualNetwork armVirtualNetwork = new ArmVirtualNetwork(virtualNetwork);
                _ArmVirtualNetworks.Add(armVirtualNetwork);
            }

            return _ArmVirtualNetworks;
        }

        public async virtual Task<List<ArmStorageAccount>> GetAzureARMStorageAccounts()
        {
            if (_ArmStorageAccounts != null)
                return _ArmStorageAccounts;

            JObject storageAccountsJson = await this.GetAzureARMResources("StorageAccounts", null);

            var storageAccounts = from storage in storageAccountsJson["value"]
                                  select storage;

            _ArmStorageAccounts = new List<ArmStorageAccount>();

            foreach (var storageAccount in storageAccounts)
            {
                ArmStorageAccount armStorageAccount = new ArmStorageAccount(_AzureContext, storageAccount);
                await this.GetAzureARMStorageAccountKeys(armStorageAccount);

                _ArmStorageAccounts.Add(armStorageAccount);
            }

            return _ArmStorageAccounts;
        }


        public async virtual Task<List<ArmLocation>> GetAzureARMLocations()
        {
            if (_ArmLocations != null)
                return _ArmLocations;

            JObject locationsJson = await this.GetAzureARMResources("Locations", null);

            var locations = from location in locationsJson["value"]
                                  select location;

            _ArmLocations = new List<ArmLocation>();

            foreach (var location in locations)
            {
                ArmLocation armLocation = new ArmLocation(_AzureContext, location);
                _ArmLocations.Add(armLocation);
            }

            _ArmLocations = _ArmLocations.OrderBy(x => x.DisplayName).ToList();

            return _ArmLocations;
        }

        internal async Task GetAzureARMStorageAccountKeys(ArmStorageAccount armStorageAccount)
        {
            Hashtable storageAccountKeyInfo = new Hashtable();
            storageAccountKeyInfo.Add("ResourceGroupName", armStorageAccount.ResourceGroup);
            storageAccountKeyInfo.Add("StorageAccountName", armStorageAccount.Name);

            JObject storageAccountKeysJson = await this.GetAzureARMResources("StorageAccountKeys", storageAccountKeyInfo);

            var storageAccountKeys = from keys in storageAccountKeysJson["keys"]
                                  select keys;

            armStorageAccount.Keys.Clear();
            foreach (var storageAccountKey in storageAccountKeys)
            {
                ArmStorageAccountKey armStorageAccountKey = new ArmStorageAccountKey(storageAccountKey);
                armStorageAccount.Keys.Add(armStorageAccountKey);
            }

            return;
        }

        #endregion

    }
}
