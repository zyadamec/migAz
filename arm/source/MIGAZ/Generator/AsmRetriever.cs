using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace MIGAZ.Generator
{
    public class AsmRetriever
    {
        private ILogProvider _logProvider;
        private IStatusProvider _statusProvider;

        public Dictionary<string, string> _documentCache = new Dictionary<string, string>();

        public AsmRetriever(ILogProvider logProvider, IStatusProvider statusProvider)
        {
            _logProvider = logProvider;
            _statusProvider = statusProvider;
        }
        
        public virtual XmlDocument GetAzureASMResources(string resourceType, string subscriptionId, Hashtable info, string token)
        {


            /*
            _logProvider.WriteLog("GetAzureASMResources", "Start");

            string url = null;
            switch (resourceType)
            {
                case "Subscriptions":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + "subscriptions?api-version=2015-01-01";
                    _statusProvider.UpdateStatus("BUSY: Getting Subscriptions...");
                    break;
               
                case "VirtualNetworks":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + subscriptionId + "/services/networking/virtualnetwork";
                    _statusProvider.UpdateStatus("BUSY: Getting Virtual Networks for Subscription ID : " + subscriptionId + "...");
                    break;
                case "ClientRootCertificates":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + subscriptionId + "/services/networking/" + info["virtualnetworkname"] + "/gateway/clientrootcertificates";
                    _statusProvider.UpdateStatus("BUSY: Getting Client Root Certificates for Virtual Network : " + info["virtualnetworkname"] + "...");
                    break;
                case "ClientRootCertificate":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + subscriptionId + "/services/networking/" + info["virtualnetworkname"] + "/gateway/clientrootcertificates/" + info["thumbprint"];
                    _statusProvider.UpdateStatus("BUSY: Getting certificate data for certificate : " + info["thumbprint"] + "...");
                    break;
                case "NetworkSecurityGroup":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + subscriptionId + "/services/networking/networksecuritygroups/" + info["name"] + "?detaillevel=Full";
                    _statusProvider.UpdateStatus("BUSY: Getting Network Security Group : " + info["name"] + "...");
                    break;
                case "RouteTable":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + subscriptionId + "/services/networking/routetables/" + info["name"] + "?detailLevel=full";
                    _statusProvider.UpdateStatus("BUSY: Getting Route Table : " + info["routetablename"] + "...");
                    break;
                case "NSGSubnet":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + subscriptionId + "/services/networking/virtualnetwork/" + info["virtualnetworkname"] + "/subnets/" + info["subnetname"] + "/networksecuritygroups";
                    _statusProvider.UpdateStatus("BUSY: Getting NSG for subnet " + info["subnetname"] + "...");
                    break;
                case "VirtualNetworkGateway":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + subscriptionId + "/services/networking/" + info["virtualnetworkname"] + "/gateway";
                    _statusProvider.UpdateStatus("BUSY: Getting Virtual Network Gateway : " + info["virtualnetworkname"] + "...");
                    break;
                case "VirtualNetworkGatewaySharedKey":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + subscriptionId + "/services/networking/" + info["virtualnetworkname"] + "/gateway/connection/" + info["localnetworksitename"] + "/sharedkey";
                    _statusProvider.UpdateStatus("BUSY: Getting Virtual Network Gateway Shared Key: " + info["localnetworksitename"] + "...");
                    break;
                case "StorageAccounts":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + subscriptionId + "/services/storageservices";
                    _statusProvider.UpdateStatus("BUSY: Getting Storage Accounts for Subscription ID : " + subscriptionId + "...");
                    break;
                case "StorageAccount":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + subscriptionId + "/services/storageservices/" + info["name"];
                    _statusProvider.UpdateStatus("BUSY: Getting Storage Accounts for Subscription ID : " + subscriptionId + "...");
                    break;
                case "StorageAccountKeys":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + subscriptionId + "/services/storageservices/" + info["name"] + "/keys";
                    _statusProvider.UpdateStatus("BUSY: Getting Storage Accounts for Subscription ID : " + subscriptionId + "...");
                    break;
                case "CloudServices":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + subscriptionId + "/services/hostedservices";
                    _statusProvider.UpdateStatus("BUSY: Getting Cloud Services for Subscription ID : " + subscriptionId + "...");
                    break;
                case "CloudService":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + subscriptionId + "/services/hostedservices/" + info["name"] + "?embed-detail=true";
                    _statusProvider.UpdateStatus("BUSY: Getting Virtual Machines for Cloud Service : " + info["name"] + "...");
                    break;
                case "VirtualMachine":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + subscriptionId + "/services/hostedservices/" + info["cloudservicename"] + "/deployments/" + info["deploymentname"] + "/roles/" + info["virtualmachinename"];
                    _statusProvider.UpdateStatus("BUSY: Getting Virtual Machines for Cloud Service : " + info["virtualmachinename"] + "...");
                    break;
                case "VMImages":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + subscriptionId + "/services/images";
                    break;
                case "ReservedIPs":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + subscriptionId + "/services/networking/reservedips";
                    break;
                case "AffinityGroup":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + subscriptionId + "/affinitygroups/" + info["affinitygroupname"];
                    break;
            }

            Application.DoEvents();

            _logProvider.WriteLog("GetAzureASMResources", "GET " + url);

            if (_documentCache.ContainsKey(url))
            {
                _logProvider.WriteLog("GetAzureASMResources", "FROM XML CACHE");
                _logProvider.WriteLog("GetAzureASMResources", "End");
                writeXMLtoFile(url, "Cached");
                return _documentCache[url];
            }

            Uri uri = new Uri(String.Format(url));

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
            request.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + token);
            request.ContentType = "application/json";
            //request.Headers.Add("x-ms-version", "2015-04-01");
            request.Method = "GET";


            string xml = "";
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                //   xml = new StreamReader(response.GetResponseStream()).ReadToEnd();
                //  _logProvider.WriteLog("GetAzureASMResources", "RESPONSE " + response.StatusCode);

                string result = null;
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }

            }
            catch (Exception exception)
            {
                _logProvider.WriteLog("GetAzureASMResources", "EXCEPTION " + exception.Message);
                DialogResult dialogresult = MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                xml = "";
                //Application.ExitThread();
            }

            if (xml != "")
            {
                XmlDocument xmlDoc = RemoveXmlns(xml);

                _logProvider.WriteLog("GetAzureASMResources", "End");
                writeXMLtoFile(url, xml);

                _documentCache.Add(url, xmlDoc);
                return xmlDoc;
            }
            else
            {
                //XmlNodeList xmlnode = null;
                //return xmlnode;
                XmlDocument xmlDoc = new XmlDocument();

                _logProvider.WriteLog("GetAzureASMResources", "End");
                writeXMLtoFile(url, "");
                return null;
            }
            */

            return null;
        }
        
        public virtual string GetAzureARMResources(string resourceType, string subscriptionId, Hashtable info, string token,string RGName)
        {
            _logProvider.WriteLog("GetAzureARMResources", "Start");

            string url = null;
            switch (resourceType)
            {
                case "Subscriptions":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + "subscriptions?api-version=2015-01-01";
                    _statusProvider.UpdateStatus("BUSY: Getting Subscriptions...");
                    break;
                case "Tenants":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + "tenants?api-version=2015-01-01";
                    _statusProvider.UpdateStatus("BUSY: Getting Tenants...");
                    break;
                case "RG":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + "subscriptions/" + subscriptionId + "/resourcegroups?api-version=2015-01-01";
                    _statusProvider.UpdateStatus("BUSY: Getting Resource Groups...");
                    break;
                case "VirtualNetworks":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + "subscriptions/" + subscriptionId  + "/resourceGroups/"+ RGName + "/providers/Microsoft.Network/virtualnetworks?api-version=2016-03-30";
                    _statusProvider.UpdateStatus("BUSY: Getting Virtual Networks for Subscription ID : " + subscriptionId + " in the Resource Group " + RGName + "...");
                    break;
                case "VirtualNetwork":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + info["VirtualNWId"] + "?api-version=2016-03-30";
                    _statusProvider.UpdateStatus("BUSY: Getting Virtual Network details...");
                    break;
                case "VirtualMachines":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + "subscriptions/" + subscriptionId + "/resourceGroups/" + RGName  + "/providers/Microsoft.Compute/virtualMachines?api-version=2016-03-30";
                    _statusProvider.UpdateStatus("BUSY: Getting Virtual Machines for Subscription ID : " + subscriptionId  + " in the Resource Group " + RGName + "...");
                    break;
                case "VirtualMachine":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + "subscriptions/" + subscriptionId + "/resourceGroups/" + RGName + "/providers/Microsoft.Compute/virtualMachines/" + info["virtualmachineName"] + "?api-version=2016-03-30";
                    _statusProvider.UpdateStatus("BUSY: Getting Virtual Machine for Subscription ID : " + subscriptionId + " in the Resource Group " + RGName + "...");
                    break;
                case "NetworkInterface":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + info["networkinterfaceId"] + "?api-version=2016-03-30";
                    _statusProvider.UpdateStatus("BUSY: Getting Network Interface details...");
                    break;
                case "Loadbalancer":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + info["LBId"] + "?api-version=2016-03-30";
                    _statusProvider.UpdateStatus("BUSY: Getting LoadBalancer details...");
                    break;
                case "NetworkSecurityGroup":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + info["NetworkSecurityGroup"] + "?api-version=2016-03-30";
                    _statusProvider.UpdateStatus("BUSY: Getting Network Security Group : " + info["name"] + "...");
                    break;
                case "RouteTable":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + info["RouteTable"] + "?api-version=2016-03-30";
                    _statusProvider.UpdateStatus("BUSY: Getting Route Table : " + info["routetablename"] + "...");
                    break;
                case "Subnet":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + info["SubnetId"] + "?api-version=2016-03-30";
                    _statusProvider.UpdateStatus("BUSY: Getting Subnet details : " + info["SubnetId"] + "...");
                    break;
                case "AvailabilitySet":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + info["AvailId"] + "?api-version=2016-03-30";
                    _statusProvider.UpdateStatus("BUSY: Getting Availability Set details : " + info["AvailId"] + "...");
                    break;
                case "VirtualNetworkGateway":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + "subscriptions/" + subscriptionId + "/resourceGroups/"+ info["RGName"]+ "/providers/Microsoft.Network/virtualNetworkGateways/"+ info["vnetGWName"] +"?api-version=2016-03-30";
                    _statusProvider.UpdateStatus("BUSY: Getting Virtual Network Gateway : " + info["vnetGWName"] + "...");
                    break;
                case "PublicIP":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + info["publicipId"] + "?api-version=2016-03-30";
                    _statusProvider.UpdateStatus("BUSY: Getting PublicIP details...");
                    break;
                case "Connections":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + "subscriptions/" + subscriptionId + "/resourceGroups/" + RGName + "/providers/Microsoft.Network/connections?api-version=2016-03-30";
                    _statusProvider.UpdateStatus("BUSY: Getting Connections from the Resource Group : " + RGName + "...");
                    break;
                case "Domains":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.GraphAuth) + "myorganization/domains?api-version=1.6";
                    _statusProvider.UpdateStatus("BUSY: Getting Tenant Domain details from Graph...");
                    break;
                case "sharedkey":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + info["connectionid"] + "/sharedkey?api-version=2016-03-30";
                    _statusProvider.UpdateStatus("BUSY: Getting SharedKey details...");
                    break;
                case "localNetworkGateway":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + info["localnwgwid"] + "?api-version=2016-03-30";
                    _statusProvider.UpdateStatus("BUSY: Getting LocalNWGateway details...");
                    break;
                case "ClientRootCertificates":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + subscriptionId + "/services/networking/" + info["virtualnetworkname"] + "/gateway/clientrootcertificates";
                    _statusProvider.UpdateStatus("BUSY: Getting Client Root Certificates for Virtual Network : " + info["virtualnetworkname"] + "...");
                    break;
                case "ClientRootCertificate":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + subscriptionId + "/services/networking/" + info["virtualnetworkname"] + "/gateway/clientrootcertificates/" + info["thumbprint"];
                    _statusProvider.UpdateStatus("BUSY: Getting certificate data for certificate : " + info["thumbprint"] + "...");
                    break;
                case "NSGSubnet":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + subscriptionId + "/services/networking/virtualnetwork/" + info["virtualnetworkname"] + "/subnets/" + info["subnetname"] + "/networksecuritygroups";
                    _statusProvider.UpdateStatus("BUSY: Getting NSG for subnet " + info["subnetname"] + "...");
                    break;
                case "VirtualNetworkGatewaySharedKey":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + subscriptionId + "/services/networking/" + info["virtualnetworkname"] + "/gateway/connection/" + info["localnetworksitename"] + "/sharedkey";
                    _statusProvider.UpdateStatus("BUSY: Getting Virtual Network Gateway Shared Key: " + info["localnetworksitename"] + "...");
                    break;
                case "StorageAccounts":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + "subscriptions/" + subscriptionId + "/resourceGroups/" + RGName + "/providers/Microsoft.Storage/storageAccounts?api-version=2015-05-01-preview";
                    _statusProvider.UpdateStatus("BUSY: Getting Storage Accounts for Subscription ID : " + subscriptionId + " in the Resource Group " + RGName + "...");
                    break;
                case "StorageAccount":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + "subscriptions/" + subscriptionId + "/resourceGroups/" + RGName + "/providers/Microsoft.Storage/storageAccounts/" + info["name"] + "?api-version=2015-05-01-preview";
                    _statusProvider.UpdateStatus("BUSY: Getting Storage Accounts for Subscription ID : " + subscriptionId + "...");
                    break;
                case "StorageAccountKeys":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + "subscriptions/" + subscriptionId + "/resourceGroups/" + RGName + "/providers/Microsoft.Storage/storageAccounts/" + info["name"] + "/listKeys?api-version=2016-01-01";
                    _statusProvider.UpdateStatus("BUSY: Getting Storage Accounts for Subscription ID : " + subscriptionId + "...");
                    break;
                case "CloudServices":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + subscriptionId + "/services/hostedservices";
                    _statusProvider.UpdateStatus("BUSY: Getting Cloud Services for Subscription ID : " + subscriptionId + "...");
                    break;
                case "CloudService":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + subscriptionId + "/services/hostedservices/" + info["name"] + "?embed-detail=true";
                    _statusProvider.UpdateStatus("BUSY: Getting Virtual Machines for Cloud Service : " + info["name"] + "...");
                    break;
                case "VMImages":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + subscriptionId + "/services/images";
                    break;
                case "ReservedIPs":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + subscriptionId + "/services/networking/reservedips";
                    break;
                case "AffinityGroup":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + subscriptionId + "/affinitygroups/" + info["affinitygroupname"];
                    break;
            }

            Application.DoEvents();

            _logProvider.WriteLog("GetAzureARMResources", "GET " + url);


            if (_documentCache.ContainsKey(url))
            {
                _logProvider.WriteLog("GetAzureARMResources", "FROM XML CACHE");
                _logProvider.WriteLog("GetAzureARMResources", "End");
                writeXMLtoFile(url, "Cached");
                return _documentCache[url];
            }
            

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + token);
            request.ContentType = "application/json";
            // request.Headers.Add("x-ms-version", "2015-04-01");
            if (resourceType == "StorageAccountKeys")
            {
                request.Method = "POST";
                request.ContentLength = 0;
            }
            else
            {
                request.Method = "GET";
            }
            string json = "";
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                json = new StreamReader(response.GetResponseStream()).ReadToEnd();
                 _logProvider.WriteLog("GetAzureARMResources", "RESPONSE " + response.StatusCode);

               /* 
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                */
            }
            catch (Exception exception)
            {
                _logProvider.WriteLog("GetAzureARMResources", "EXCEPTION " + exception.Message);
                DialogResult dialogresult = MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                json = "";
                //Application.ExitThread();
            }

            if (json != "")
            {
                _logProvider.WriteLog("GetAzureARMResources", "End");
                writeXMLtoFile(url, json);

                if ((resourceType != "Subscriptions") && (resourceType != "Domains") )
                {
                    _documentCache.Add(url, json);
                }

                return json;
            }
            else
            {
                
                _logProvider.WriteLog("GetAzureARMResources", "End");
                writeXMLtoFile(url, "");
                return null;
            }
            
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

        private void writeXMLtoFile(string url, string xml)
        {
            string logfilepath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\MIGAZ\\MIGAZ-JSON-" + string.Format("{0:yyyyMMdd}", DateTime.Now) + ".log";
            string text = DateTime.Now.ToString() + "   " + url + Environment.NewLine;
            File.AppendAllText(logfilepath, text);
            text = xml + Environment.NewLine;
            File.AppendAllText(logfilepath, text);
            text = Environment.NewLine;
            File.AppendAllText(logfilepath, text);
        }

        public virtual void GetVMDetails(string subscriptionId, string RGName, string virtualMachineName, string token, out string deploymentName, out string virtualNetworkName, out string loadBalancerName)
        {
            deploymentName = "";
            loadBalancerName = "";

            Hashtable VMInfo = new Hashtable();
            VMInfo.Add("virtualmachineName", virtualMachineName);


            //Listing Virtual Network
            var VMDetails = GetAzureARMResources("VirtualMachine", subscriptionId, VMInfo, token, RGName);
            var VMResults = JsonConvert.DeserializeObject<dynamic>(VMDetails);

            foreach (var VM in VMResults.properties.networkProfile.networkInterfaces)
            {
                string VMnwId = VM.id;
                VMnwId = VMnwId.Replace("/subscriptions/", "subscriptions/");

                Hashtable NWInfo = new Hashtable();
                NWInfo.Add("networkinterfaceId", VMnwId);

                var NWDetails = GetAzureARMResources("NetworkInterface", subscriptionId, NWInfo, token, RGName);
                var NWResults = JsonConvert.DeserializeObject<dynamic>(NWDetails);


                foreach (var VN in NWResults.properties.ipConfigurations)
                {
                    virtualNetworkName = VN.properties.subnet.id;
                    return;
                        //"/subscriptions/0a742bb2-7e55-4a79-ba30-dfc41d61a3d2/resourceGroups/DemoRG2/providers/Microsoft.Network/virtualNetworks/DemoRG2-vnet/subnets/default"
                }


                /*
                var listItem = new ListViewItem(RGName);
                listItem.SubItems.AddRange(new[] { vnetName });
                lvwVirtualNetworks.Items.Add(listItem);
                Application.DoEvents();

                */
            }


            /*
            Hashtable cloudserviceinfo = new Hashtable();
            cloudserviceinfo.Add("name", RGName);

            XmlDocument hostedservice = GetAzureASMResources("CloudService", subscriptionId, cloudserviceinfo, token);
            if (hostedservice.SelectNodes("//Deployments/Deployment").Count > 0)
            {
                if (hostedservice.SelectNodes("//Deployments/Deployment")[0].SelectNodes("RoleList/Role")[0].SelectNodes("RoleType").Count > 0)
                {
                    if (hostedservice.SelectNodes("//Deployments/Deployment")[0].SelectNodes("RoleList/Role")[0].SelectSingleNode("RoleType").InnerText == "PersistentVMRole")
                    {
                        virtualNetworkName = "empty";
                        if (hostedservice.SelectNodes("//Deployments/Deployment")[0].SelectSingleNode("VirtualNetworkName") != null)
                        {
                            virtualNetworkName = hostedservice.SelectNodes("//Deployments/Deployment")[0].SelectSingleNode("VirtualNetworkName").InnerText;
                        }
                        deploymentName = hostedservice.SelectNodes("//Deployments/Deployment")[0].SelectSingleNode("Name").InnerText;
                        XmlNodeList roles = hostedservice.SelectNodes("//Deployments/Deployment")[0].SelectNodes("RoleList/Role");
                        // GetVMLBMapping is necessary because a Cloud Service can have multiple availability sets
                        // On ARM, a load balancer can only be attached to 1 availability set
                        // Because of this, if multiple availability sets exist, we are breaking the cloud service in multiple load balancers
                        //     to respect all availability sets
                        Dictionary<string, string> vmlbmapping = GetVMLBMapping(RGName, roles);
                        foreach (XmlNode role in roles)
                        {
                            string currentVM = role.SelectSingleNode("RoleName").InnerText;
                            if (currentVM == virtualMachineName)
                            {
                                loadBalancerName = vmlbmapping[virtualMachineName];
                                return;
                            }
                        }
                    }
                }
            }
            */
            throw new InvalidOperationException("Requested VM could not be found");
        }

        public virtual void GetARMVMDetails(string subscriptionId, string RGName, string virtualMachineName, string token, out string virtualNetworkName)
        {
            
            Hashtable VMInfo = new Hashtable();
            VMInfo.Add("virtualmachineName", virtualMachineName);


            //Listing Virtual Network
            var VMDetails = GetAzureARMResources("VirtualMachine", subscriptionId, VMInfo, token, RGName);
            var VMResults = JsonConvert.DeserializeObject<dynamic>(VMDetails);

            foreach (var VM in VMResults.properties.networkProfile.networkInterfaces)
            {
                string VMnwId = VM.id;
                VMnwId = VMnwId.Replace("/subscriptions/", "subscriptions/");

                Hashtable NWInfo = new Hashtable();
                NWInfo.Add("networkinterfaceId", VMnwId);

                var NWDetails = GetAzureARMResources("NetworkInterface", subscriptionId, NWInfo, token, RGName);
                var NWResults = JsonConvert.DeserializeObject<dynamic>(NWDetails);


                foreach (var VN in NWResults.properties.ipConfigurations)
                {
                    virtualNetworkName = VN.properties.subnet.id;
                    return;
            
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
                string availabilitysetname = "empty";
                if (role.SelectNodes("AvailabilitySetName").Count > 0)
                {
                    availabilitysetname = role.SelectSingleNode("AvailabilitySetName").InnerText;
                }

                if (!aslbnamemapping.ContainsKey(availabilitysetname))
                {
                    aslbnamemapping.Add(availabilitysetname, "");
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
                string availabilitysetname = "empty";
                if (role.SelectNodes("AvailabilitySetName").Count > 0)
                {
                    availabilitysetname = role.SelectSingleNode("AvailabilitySetName").InnerText;
                }
                string loadbalancername = aslbnamemapping2[availabilitysetname];

                vmlbmapping.Add(virtualmachinename, loadbalancername);
            }

            return vmlbmapping;
        }
    }
}
