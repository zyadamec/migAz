using MIGAZ.Generator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Xml;
using System.IO;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using MIGAZ.Asm;
using MIGAZ.Interface;
using MIGAZ.Azure;

namespace MIGAZ.Tests.Fakes
{
    class FakeAzureRetriever : AzureRetriever
    {
        private AzureContext _AzureContext;
        private List<AzureLocation> _AzureLocations = new List<AzureLocation>();
        private List<AsmCloudService> _CloudServices = new List<AsmCloudService>();
        private List<AsmStorageAccount> _StorageAccounts = new List<AsmStorageAccount>();
        private List<AsmVirtualNetwork> _VirtualNetworks = new List<AsmVirtualNetwork>();
        private List<AsmClientRootCertificate> _ClientRootCertificates = new List<AsmClientRootCertificate>();
        private AsmVirtualNetworkGateway _VirtualNetworkGateway;
        private AsmNetworkSecurityGroup _NetworkSecurityGroup;
        private AsmRouteTable _AsmRouteTable;

        private Dictionary<string, string[]> _keyProperties = new Dictionary<string, string[]>
        {
            { "Subscriptions", new string[] { } },
            { "VirtualNetworks", new string[] { } },
            { "ClientRootCertificates", new string[] { "virtualnetworkname"} },
            { "ClientRootCertificate", new string[] { "virtualnetworkname", "thumbprint" } },
            { "NetworkSecurityGroup", new string[] { "name"} },
            { "RouteTable", new string[] { "name"} },
            { "NSGSubnet", new string[] { "virtualnetworkname", "subnetname" } },
            { "VirtualNetworkGateway", new string[] { "virtualnetworkname" } },
            { "VirtualNetworkGatewaySharedKey", new string[] { "virtualnetworkname", "localnetworksitename" } },
            { "StorageAccounts", new string[] { } },
            { "StorageAccount", new string[] { "name" } },
            { "StorageAccountKeys", new string[] { "name" } },
            { "CloudServices", new string[] { } },
            { "CloudService", new string[] { "name" } },
            { "VirtualMachine", new string[] { "cloudservicename", "virtualmachinename", "deploymentname"} },
            { "VMImages", new string[] { } },
        };
        private Dictionary<string, XmlDocument> _responses = new Dictionary<string, XmlDocument>();

        public FakeAzureRetriever(AzureContext azureContext) : base(azureContext)
        {
            _AzureContext = azureContext;
        }

        public async void LoadDocuments(string path)
        {
            string[] files = Directory.GetFiles(path, "*.xml");
            Array.Sort(files);
            foreach (var filename in files)
            {
                var doc = new XmlDocument();
                doc.Load(filename);

                var title = Path.GetFileNameWithoutExtension(filename);
                var parts = title.Split('-');
                string resourceType;
                var info = new Hashtable();

                switch (parts[1].ToLower())
                {
                    case "azurelocations":
                        resourceType = "azurelocations";

                        foreach (XmlNode azureLocationXml in doc.SelectNodes("/Locations/Location"))
                        {
                            _AzureLocations.Add(new AzureLocation(_AzureContext, azureLocationXml));
                        }

                        break;
                    case "cloudservice":
                        resourceType = "CloudService";
                        info.Add("name", parts[2]);

                        _CloudServices.Add(new AsmCloudService(_AzureContext, doc));

                        break;
                    case "virtualmachine":
                        resourceType = "VirtualMachine";
                        info.Add("cloudservicename", parts[2]);
                        info.Add("virtualmachinename", parts[3]);
                        info.Add("deploymentname", parts[4]);
                        info.Add("loadbalancername", String.Empty);
                        info.Add("virtualnetworkname", String.Empty);
                        AsmCloudService parentCloudService = this.GetAzureAsmCloudService(parts[2]).Result;
                        AsmVirtualMachine asmVirtualMachine = new AsmVirtualMachine(_AzureContext, parentCloudService, this._AzureContext.SettingsProvider, doc, info);
                        await asmVirtualMachine.InitializeChildren();
                        asmVirtualMachine.OSVirtualHardDisk.TargetStorageAccount = asmVirtualMachine.OSVirtualHardDisk.SourceStorageAccount;
                        asmVirtualMachine.TargetVirtualNetwork = asmVirtualMachine.SourceVirtualNetwork;
                        asmVirtualMachine.TargetSubnet = asmVirtualMachine.SourceSubnet;

                        foreach (AsmDisk dataDisk in asmVirtualMachine.DataDisks)
                        {
                            dataDisk.TargetStorageAccount = dataDisk.SourceStorageAccount;
                        }

                        parentCloudService.VirtualMachines.Add(asmVirtualMachine);

                        break;
                    case "storageaccountkeys":
                        resourceType = "StorageAccountKeys";
                        info.Add("name", parts[2]);

                        this.GetAzureAsmStorageAccount(parts[2]).Result.Keys = new AsmStorageAccountKeys(_AzureContext, doc);
                        break;
                    case "storageaccount":
                        resourceType = "StorageAccount";
                        info.Add("name", parts[2]);

                        _StorageAccounts.Add(new AsmStorageAccount(_AzureContext, doc));
                        break;
                    case "virtualnetworks":
                        resourceType = "VirtualNetworks";
                        foreach (XmlNode virtualnetworksite in doc.SelectNodes("//VirtualNetworkSite"))
                        {
                            AsmVirtualNetwork asmVirtualNetwork = new AsmVirtualNetwork(_AzureContext, virtualnetworksite);
                            await asmVirtualNetwork.InitializeChildrenAsync();
                            _VirtualNetworks.Add(asmVirtualNetwork);
                        }
                        break;
                    case "clientrootcertificates":
                        resourceType = "ClientRootCertificates";
                        info.Add("virtualnetworkname", parts[2]);

                        foreach (XmlNode clientRootCertificateXml in doc.SelectNodes("//ClientRootCertificate"))
                        {
                            _ClientRootCertificates.Add(new AsmClientRootCertificate(_AzureContext, _VirtualNetworks[0], clientRootCertificateXml));
                        }

                        break;
                    case "clientrootcertificate":
                        resourceType = "ClientRootCertificate";
                        info.Add("virtualnetworkname", parts[2]);
                        info.Add("thumbprint", parts[3]);
                        break;
                    case "virtualnetworkgateway":
                        resourceType = "VirtualNetworkGateway";
                        info.Add("virtualnetworkname", parts[2]);

                        _VirtualNetworkGateway = new AsmVirtualNetworkGateway(_AzureContext, _VirtualNetworks[0], doc);
                        _VirtualNetworks[0].Gateway = _VirtualNetworkGateway;

                        break;
                    case "virtualnetworkgatewaysharedkey":
                        resourceType = "VirtualNetworkGatewaySharedKey";
                        info.Add("virtualnetworkname", parts[2]);
                        info.Add("localnetworksitename", parts[3]);
                        break;
                    case "networksecuritygroup":
                        resourceType = "NetworkSecurityGroup";
                        info.Add("name", parts[2]);

                        _NetworkSecurityGroup = new AsmNetworkSecurityGroup(_AzureContext, doc.SelectSingleNode("NetworkSecurityGroup"));

                        break;
                    case "routetable":
                        resourceType = "RouteTable";
                        info.Add("name", parts[2]);

                        _AsmRouteTable = new AsmRouteTable(_AzureContext, doc);

                        break;
                    case "reservedips":
                        resourceType = "ReservedIPs";
                        break;
                    default:
                        throw new Exception();
                }

                SetResponse(resourceType, info, doc);
            }
        }

        public void SetResponse(string resourceType, Hashtable info, XmlDocument doc)
        {
            string key = resourceType + ":" + SerialiseHashTable(resourceType, info);
            _responses[key] = doc;
        }

        public async Task<XmlDocument> GetAzureASMResources(string resourceType, Hashtable info)
        {
            string key = resourceType + ":" + SerialiseHashTable(resourceType, info);
            var xmlDoc = _responses[key];
            return RemoveXmlns(xmlDoc.OuterXml);
        }



        private string SerialiseHashTable(string resourceType, Hashtable ht)
        {
            if (ht == null)
            {
                return String.Empty;
            }
            var sb = new StringBuilder();

            // Sort keys
            var keyList = new List<string>();
            foreach(var key in ht.Keys)
            {
                keyList.Add((string)key);
            }
            keyList.Sort();

            foreach (var key in keyList)
            {
                if (_keyProperties[resourceType].Contains(key)) // Don't include properties from the hashtable that aren't needed
                {
                    sb.Append(key);
                    sb.Append("=");
                    sb.Append(ht[key]);
                    sb.Append(";");
                }
            }
            return sb.ToString();
        }

        public async override Task<List<AsmCloudService>> GetAzureAsmCloudServices()
        {
            return _CloudServices;
        }

        public async override Task<AsmCloudService> GetAzureAsmCloudService(string cloudServiceName)
        {
            if (_CloudServices != null)
            {
                foreach (AsmCloudService asmCloudService in _CloudServices)
                {
                    if (asmCloudService.ServiceName == cloudServiceName)
                        return asmCloudService;
                }
            }

            return null;
        }

        public async override Task<List<AsmReservedIP>> GetAzureAsmReservedIPs()
        {
            return new List<AsmReservedIP>();
        }

        public async override Task<List<AsmStorageAccount>> GetAzureAsmStorageAccounts()
        {
            return new List<AsmStorageAccount>();
        }

        public async override Task<AsmStorageAccount> GetAzureAsmStorageAccount(string storageAccountName)
        {
            foreach (AsmStorageAccount asmStorageAccount in _StorageAccounts)
            {
                if (asmStorageAccount.Name == storageAccountName)
                    return asmStorageAccount;
            }

            return null;
        }

        public async override Task<List<AsmVirtualNetwork>> GetAzureAsmVirtualNetworks()
        {
            return _VirtualNetworks;
        }

        public async override Task<List<AsmClientRootCertificate>> GetAzureAsmClientRootCertificates(AsmVirtualNetwork asmVirtualNetwork)
        {
            return _ClientRootCertificates;
        }

        public async override Task<AsmVirtualNetworkGateway> GetAzureAsmVirtualNetworkGateway(AsmVirtualNetwork asmVirtualNetwork)
        {
            return _VirtualNetworkGateway;
        }

        public async override Task<string> GetAzureAsmVirtualNetworkSharedKey(string virtualNetworkName, string localNetworkSiteName)
        {
            return String.Empty;
        }

        public async override Task<AsmNetworkSecurityGroup> GetAzureAsmNetworkSecurityGroup(string networkSecurityGroupName)
        {
            return _NetworkSecurityGroup;
        }

        public async override Task<AsmRouteTable> GetAzureAsmRouteTable(string routeTableName)
        {
            return _AsmRouteTable;
        }

        public async override Task<List<AzureLocation>> GetAzureLocations()
        {
            return _AzureLocations;
        }

    }
}
