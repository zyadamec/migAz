// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.IdentityModel.Clients.ActiveDirectory;
using MigAz.Azure.Core.ArmTemplate;
using MigAz.Azure.Core.Interface;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MigAz.Azure.Arm
{
    public class VirtualNetwork : ArmResource, IVirtualNetwork, IMigrationVirtualNetwork
    {
        private List<VirtualNetworkGateway> ResourceTokenGateways = new List<VirtualNetworkGateway>();
        private List<ISubnet> _Subnets = new List<ISubnet>();
        private List<string> _AddressPrefixes = new List<string>();
        private List<string> _DnsServers = new List<string>();

        private VirtualNetwork() : base(null, null) { }

        public VirtualNetwork(AzureSubscription azureSubscription, JToken resourceToken) : base(azureSubscription, resourceToken)
        {
            var subnets = from vnet in ResourceToken["properties"]["subnets"]
                          select vnet;

            foreach (var subnet in subnets)
            {
                Subnet armSubnet = new Subnet(this, subnet);
                _Subnets.Add(armSubnet);
            }

            var addressPrefixes = from vnet in ResourceToken["properties"]["addressSpace"]["addressPrefixes"]
                                  select vnet;

            foreach (var addressPrefix in addressPrefixes)
            {
                _AddressPrefixes.Add(addressPrefix.ToString());
            }

            if (ResourceToken["properties"] != null)
            {
                if (ResourceToken["properties"]["dhcpOptions"] != null)
                {
                    if (ResourceToken["properties"]["dhcpOptions"]["dnsServers"] != null)
                    {
                        var dnsPrefixes = from vnet in ResourceToken["properties"]["dhcpOptions"]["dnsServers"]
                                          select vnet;

                        foreach (var dnsPrefix in dnsPrefixes)
                        {
                            _DnsServers.Add(dnsPrefix.ToString());
                        }
                    }
                }
            }
        }

        public new async Task InitializeChildrenAsync()
        {
            await base.InitializeChildrenAsync();

            foreach (Subnet subnet in this.Subnets)
            {
                await subnet.InitializeChildrenAsync();
            }
        }

        #region Properties

        public string Type => (string)ResourceToken["type"];
        public ISubnet GatewaySubnet
        {
            get
            {
                foreach (Subnet subnet in this.Subnets)
                {
                    if (subnet.Name == ArmConst.GatewaySubnetName)
                        return subnet;
                }

                return null;
            }
        }
        public List<ISubnet> Subnets => _Subnets;
        public List<string> AddressPrefixes => _AddressPrefixes;
        public List<string> DnsServers => _DnsServers;

        public List<VirtualNetworkGateway> VirtualNetworkGateways
        {
            get { return ResourceTokenGateways; }
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return this.Name;
        }
        
        public async Task<(bool isAvailable, List<String> availableIps)> IsIpAddressAvailable(string ipAddress)
        {
            //https://docs.microsoft.com/en-us/rest/api/virtualnetwork/virtualnetworks/checkipaddressavailability
            const String CheckIPAddressAvailability = "CheckIPAddressAvailability";
            bool isAvailable = false;
            List<String> ipsAvailable = null;

            this.AzureSubscription.LogProvider.WriteLog("IsIpAddressAvailable", "Start");
            this.AzureSubscription.StatusProvider.UpdateStatus("BUSY: Checking if IP Address '" + ipAddress + "' is available in Azure Virtual Network '" + this.Name + "'.");

            if (this.AzureSubscription.SubscriptionId == Guid.Empty)
            {
                return (true, null);
            }

            AzureContext azureContext = this.AzureSubscription.AzureTenant.AzureContext;

            if (this.AzureSubscription.ExistsProviderResourceType(ArmConst.MicrosoftNetwork, "virtualNetworks"))
            {

                if (azureContext != null && azureContext.LogProvider != null)
                    azureContext.LogProvider.WriteLog("CheckNameAvailability", "Storage Account '" + this.ToString());

                if (azureContext != null && azureContext.StatusProvider != null)
                    azureContext.StatusProvider.UpdateStatus("Checking Name Availabilty for target Storage Account '" + this.ToString() + "'");

                // https://docs.microsoft.com/en-us/rest/api/storagerp/storageaccounts/checknameavailability
                string url = this.AzureSubscription.ApiUrl + "subscriptions/" + this.AzureSubscription.SubscriptionId + "/resourceGroups/" + this.ResourceGroup.Name + ArmConst.ProviderVirtualNetwork + this.Name + "/" + CheckIPAddressAvailability + "?api-version=" + this.AzureSubscription.GetProviderMaxApiVersion(ArmConst.MicrosoftNetwork, "virtualNetworks") + "&ipAddress=" + ipAddress;

                AuthenticationResult authenticationResult = await azureContext.TokenProvider.GetToken(azureContext.AzureEnvironment.ResourceManagerEndpoint, azureContext.AzureSubscription.AzureAdTenantId);

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticationResult.AccessToken);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.BaseAddress = new Uri(azureContext.AzureEnvironment.ResourceManagerEndpoint);

                    if (azureContext != null && azureContext.LogProvider != null)
                        azureContext.LogProvider.WriteLog("CheckNameAvailability", "Storage Account '" + this.ToString() + "' PostAsync " + url + "");

                    using (var response = await client.GetAsync(url))
                    {
                        String strResponse = response.Content.ReadAsStringAsync().Result.ToString();
                        JObject responseJson = JObject.Parse(strResponse);
                        isAvailable = (response.StatusCode == System.Net.HttpStatusCode.OK &&
                            responseJson != null &&
                            responseJson["available"] != null &&
                            String.Compare(responseJson["available"].ToString(), "True", true) == 0);

                        if (responseJson["availableIPAddresses"] != null)
                        {
                            ipsAvailable = new List<String>();

                            var ipAddressesJson = from ipAddressJson in responseJson["availableIPAddresses"]
                                                  select ipAddressJson;

                            foreach (String ipAddressJson in ipAddressesJson)
                            {
                                ipsAvailable.Add(ipAddressJson);
                            }
                        }
                    }
                }


            }
            else
            {
                azureContext.LogProvider.WriteLog("CheckNameAvailability", "Provider Resource Type does not exist.  Unable to check if storage account name is available.");
                isAvailable = true;
            }

            if (azureContext != null && azureContext.StatusProvider != null)
                azureContext.StatusProvider.UpdateStatus("Ready");

            return (isAvailable, ipsAvailable);
        }


        #endregion

        public bool HasNonGatewaySubnet
        {
            get
            {
                if ((this.Subnets.Count() == 0) ||
                    ((this.Subnets.Count() == 1) && (this.Subnets[0].Name == ArmConst.GatewaySubnetName)))
                    return false;

                return true;
            }
        }

        public bool HasGatewaySubnet
        {
            get
            {
                return this.GatewaySubnet != null;
            }
        }

        public static bool operator ==(VirtualNetwork lhs, VirtualNetwork rhs)
        {
            bool status = false;
            if (((object)lhs == null && (object)rhs == null) ||
                    ((object)lhs != null && (object)rhs != null && lhs.Id == rhs.Id))
            {
                status = true;
            }
            return status;
        }

        public static bool operator !=(VirtualNetwork lhs, VirtualNetwork rhs)
        {
            return !(lhs == rhs);
        }
    }
}

