using MigAz.Azure.Arm;
using MigAz.Azure.Asm;
using MigAz.Azure.Interface;
using MigAz.Azure.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MigAz.Azure.Generator.AsmToArm
{
    public class TemplateGenerator
    {
        private ILogProvider _logProvider;
        private IStatusProvider _statusProvider;
        private ITelemetryProvider _telemetryProvider;
        private ISettingsProvider _settingsProvider;
        private AsmArtifacts _ASMArtifacts;

        private TemplateGenerator() { }

        public TemplateGenerator(ILogProvider logProvider, IStatusProvider statusProvider, ITelemetryProvider telemetryProvider, 
            ISettingsProvider settingsProvider)
        {
            _logProvider = logProvider;
            _statusProvider = statusProvider;
            _telemetryProvider = telemetryProvider;
            _settingsProvider = settingsProvider;
        }

        public async Task<TemplateResult> GenerateTemplate(ISubscription sourceASMSubscription, ISubscription targetARMSubscription, AsmArtifacts artifacts, ArmResourceGroup targetResourceGroup, string outputPath)
        {
            _ASMArtifacts = artifacts;

            if (targetResourceGroup == null)
            {
                throw new ArgumentException("Target Resource Group must be provided for template generation.");
            }

            if (targetResourceGroup.Location == null)
            {
                throw new ArgumentException("Target Resource Group Location must be provided for template generation.");
            }

            foreach (AsmNetworkSecurityGroup asmNetworkSecurityGroup in artifacts.NetworkSecurityGroups)
            {
                if (asmNetworkSecurityGroup.TargetName == String.Empty)
                    throw new ArgumentException("Target Name for ASM Network Security Group '" + asmNetworkSecurityGroup.Name + "' must be specified.");
            }

            foreach (AsmVirtualMachine asmVirtualMachine in artifacts.VirtualMachines)
            {
                if (asmVirtualMachine.TargetName == String.Empty)
                    throw new ArgumentException("Target Name for ASM Virtual Machine '" + asmVirtualMachine.RoleName + "' must be specified.");

                if (asmVirtualMachine.TargetAvailabilitySet == null)
                    throw new ArgumentException("Target Availability Set for ASM Virtual Machine '" + asmVirtualMachine.RoleName + "' must be specified.");

                if (asmVirtualMachine.TargetVirtualNetwork == null)
                    throw new ArgumentException("Target Virtual Network for ASM Virtual Machine '" + asmVirtualMachine.RoleName + "' must be specified.");

                if (asmVirtualMachine.TargetSubnet == null)
                    throw new ArgumentException("Target Subnet for ASM Virtual Machine '" + asmVirtualMachine.RoleName + "' must be specified.");

                if (asmVirtualMachine.OSVirtualHardDisk.TargetStorageAccount == null)
                    throw new ArgumentException("Target Storage Account for ASM Virtual Machine '" + asmVirtualMachine.RoleName + "' OS Disk must be specified.");

                foreach (AsmDisk dataDisk in asmVirtualMachine.DataDisks)
                {
                    if (dataDisk.TargetStorageAccount == null)
                    {
                        throw new ArgumentException("Target Storage Account for ASM Virtual Machine '" + asmVirtualMachine.RoleName + "' Data Disk '" + dataDisk.DiskName + "' must be specified.");
                    }
                }
            }

            TemplateResult templateResult = new TemplateResult(sourceASMSubscription, targetARMSubscription, targetResourceGroup, _logProvider, outputPath);

            _logProvider.WriteLog("GenerateTemplate", "Start - Execution " + templateResult.ExecutionGuid.ToString());

            _logProvider.WriteLog("GenerateTemplate", "Start processing selected Network Security Groups");
            // process selected virtual networks
            foreach (AsmNetworkSecurityGroup asmNetworkSecurityGroup in artifacts.NetworkSecurityGroups)
            {
                _statusProvider.UpdateStatus("BUSY: Exporting Virtual Network : " + asmNetworkSecurityGroup.GetFinalTargetName());
                await BuildNetworkSecurityGroup(templateResult, asmNetworkSecurityGroup);
            }
            _logProvider.WriteLog("GenerateTemplate", "End processing selected Network Security Groups");

            _logProvider.WriteLog("GenerateTemplate", "Start processing selected virtual networks");
            // process selected virtual networks
            foreach (AsmVirtualNetwork asmVirtualNetwork in artifacts.VirtualNetworks)
            {
                _statusProvider.UpdateStatus("BUSY: Exporting Virtual Network : " + asmVirtualNetwork.GetFinalTargetName());
                await BuildVirtualNetworkObject(templateResult, asmVirtualNetwork);
            }
            _logProvider.WriteLog("GenerateTemplate", "End processing selected virtual networks");

            _logProvider.WriteLog("GenerateTemplate", "Start processing selected storage accounts");

            // process selected storage accounts
            foreach (AsmStorageAccount asmStorageAccount in artifacts.StorageAccounts)
            {
                _statusProvider.UpdateStatus("BUSY: Exporting Storage Account : " + asmStorageAccount.GetFinalTargetName());
                BuildStorageAccountObject(templateResult, asmStorageAccount);
            }
            _logProvider.WriteLog("GenerateTemplate", "End processing selected storage accounts");

            _logProvider.WriteLog("GenerateTemplate", "Start processing selected cloud services and virtual machines");

            // process selected cloud services and virtual machines
            foreach (AsmVirtualMachine asmVirtualMachine in artifacts.VirtualMachines)
            {
                _statusProvider.UpdateStatus("BUSY: Exporting Cloud Service : " + asmVirtualMachine.CloudServiceName);

                BuildPublicIPAddressObject(templateResult, asmVirtualMachine);
                BuildLoadBalancerObject(templateResult, asmVirtualMachine.Parent, asmVirtualMachine, artifacts);
        
                // process availability set
                BuildAvailabilitySetObject(templateResult, asmVirtualMachine);

                // process network interface
                List<NetworkProfile_NetworkInterface> networkinterfaces = new List<NetworkProfile_NetworkInterface>();
                await BuildNetworkInterfaceObject(templateResult, asmVirtualMachine, networkinterfaces);

                // process virtual machine
                await BuildVirtualMachineObject(templateResult, asmVirtualMachine, networkinterfaces);
            }
            _logProvider.WriteLog("GenerateTemplate", "End processing selected cloud services and virtual machines");


            _statusProvider.UpdateStatus("BUSY: saving template files");
            _logProvider.WriteLog("GenerateTemplate", "Writing Template Output Files");
            templateResult.Write();

            // post Telemetry Record to ASMtoARMToolAPI
            if (_settingsProvider.AllowTelemetry)
            {
                _statusProvider.UpdateStatus("BUSY: saving telemetry information");
                //_telemetryProvider.PostTelemetryRecord(templateResult);// TODO
            }

            _statusProvider.UpdateStatus("Ready");

            _logProvider.WriteLog("GenerateTemplate", "End - Execution " + templateResult.ExecutionGuid.ToString());

            _ASMArtifacts = null;

            return templateResult;
        }

        private void BuildPublicIPAddressObject(TemplateResult templateResult, ref NetworkInterface networkinterface)
        {
            _logProvider.WriteLog("BuildPublicIPAddressObject", "Start");

            PublicIPAddress publicipaddress = new PublicIPAddress(templateResult.ExecutionGuid);
            publicipaddress.name = networkinterface.name;
            publicipaddress.location = templateResult.TargetResourceGroup.Location.Name;
            publicipaddress.properties = new PublicIPAddress_Properties();

            templateResult.AddResource(publicipaddress);

            NetworkInterface_Properties networkinterface_properties = (NetworkInterface_Properties)networkinterface.properties;
            networkinterface_properties.ipConfigurations[0].properties.publicIPAddress = new Reference();
            networkinterface_properties.ipConfigurations[0].properties.publicIPAddress.id = "[concat(" + ArmConst.ResourceGroupId + ", '" + ArmConst.ProviderPublicIpAddress + publicipaddress.name + "')]";
            networkinterface.properties = networkinterface_properties;

            networkinterface.dependsOn.Add(networkinterface_properties.ipConfigurations[0].properties.publicIPAddress.id);
            _logProvider.WriteLog("BuildPublicIPAddressObject", "End");
        }

        private void BuildPublicIPAddressObject(TemplateResult templateResult, AsmVirtualMachine asmVirtualMachine)
        {
            _logProvider.WriteLog("BuildPublicIPAddressObject", "Start");
            
            string publicipaddress_name = asmVirtualMachine.LoadBalancerName;

            string publicipallocationmethod = "Dynamic";
            if (asmVirtualMachine.Parent.AsmReservedIP != null)
                publicipallocationmethod = "Static";

            Hashtable dnssettings = new Hashtable();
            dnssettings.Add("domainNameLabel", (publicipaddress_name + _settingsProvider.StorageAccountSuffix).ToLower());

            PublicIPAddress_Properties publicipaddress_properties = new PublicIPAddress_Properties();
            publicipaddress_properties.dnsSettings = dnssettings;
            publicipaddress_properties.publicIPAllocationMethod = publicipallocationmethod;

            PublicIPAddress publicipaddress = new PublicIPAddress(templateResult.ExecutionGuid);
            publicipaddress.name = publicipaddress_name + _settingsProvider.PublicIPSuffix;
            publicipaddress.location = templateResult.TargetResourceGroup.Location.Name;
            publicipaddress.properties = publicipaddress_properties;

            templateResult.AddResource(publicipaddress);

            _logProvider.WriteLog("BuildPublicIPAddressObject", "End");
        }

        private void BuildAvailabilitySetObject(TemplateResult templateResult, AsmVirtualMachine asmVirtualMachine)
        {
            _logProvider.WriteLog("BuildAvailabilitySetObject", "Start");

            string virtualmachinename = asmVirtualMachine.RoleName;
            string cloudservicename = asmVirtualMachine.CloudServiceName;

            AvailabilitySet availabilityset = new AvailabilitySet(templateResult.ExecutionGuid);

            availabilityset.name = asmVirtualMachine.TargetAvailabilitySet.GetFinalTargetName();
            availabilityset.location = templateResult.TargetResourceGroup.Location.Name;

            templateResult.AddResource(availabilityset);

            _logProvider.WriteLog("BuildAvailabilitySetObject", "End");
        }

        private void BuildLoadBalancerObject(TemplateResult templateResult, AsmCloudService asmCloudService, AsmVirtualMachine asmVirtualMachine, AsmArtifacts artifacts)
        {
            _logProvider.WriteLog("BuildLoadBalancerObject", "Start");

            LoadBalancer loadbalancer = (LoadBalancer) templateResult.GetResource(typeof(LoadBalancer), asmVirtualMachine.LoadBalancerName);

            if (loadbalancer == null)
            {
                loadbalancer = new LoadBalancer(templateResult.ExecutionGuid);
                loadbalancer.name = asmVirtualMachine.LoadBalancerName;
                loadbalancer.location = templateResult.TargetResourceGroup.Location.Name;

                FrontendIPConfiguration_Properties frontendipconfiguration_properties = new FrontendIPConfiguration_Properties();

                // if internal load balancer
                // shouldn't this change to a foreach loop?
                if (asmCloudService.LoadBalancers.Count > 0)
                {
                    string virtualnetworkname = asmCloudService.VirtualNetwork.GetFinalTargetName();
                    string subnetname = asmCloudService.LoadBalancers[0].Subnet.TargetName;

                    frontendipconfiguration_properties.privateIPAllocationMethod = "Dynamic";
                    if (asmCloudService.StaticVirtualNetworkIPAddress != String.Empty)
                    {
                        frontendipconfiguration_properties.privateIPAllocationMethod = "Static";
                        frontendipconfiguration_properties.privateIPAddress = asmCloudService.StaticVirtualNetworkIPAddress;
                    }

                    List<string> dependson = new List<string>();
                    dependson.Add("[concat(" + ArmConst.ResourceGroupId + ", '" + ArmConst.ProviderVirtualNetwork + virtualnetworkname + "')]");
                    loadbalancer.dependsOn = dependson;

                    Reference subnet_ref = new Reference();
                    subnet_ref.id = "[concat(" + ArmConst.ResourceGroupId + ", '" + ArmConst.ProviderVirtualNetwork + virtualnetworkname + "/subnets/" + subnetname + "')]";
                    frontendipconfiguration_properties.subnet = subnet_ref;
                }
                // if external load balancer
                else
                {
                    List<string> dependson = new List<string>();
                    dependson.Add("[concat(" + ArmConst.ResourceGroupId + ", '" + ArmConst.ProviderPublicIpAddress + loadbalancer.name + _settingsProvider.PublicIPSuffix + "')]");
                    loadbalancer.dependsOn = dependson;

                    Reference publicipaddress_ref = new Reference();
                    publicipaddress_ref.id = "[concat(" + ArmConst.ResourceGroupId + ", '" + ArmConst.ProviderPublicIpAddress + loadbalancer.name + _settingsProvider.PublicIPSuffix + "')]";
                    frontendipconfiguration_properties.publicIPAddress = publicipaddress_ref;
                }

                LoadBalancer_Properties loadbalancer_properties = new LoadBalancer_Properties();

                FrontendIPConfiguration frontendipconfiguration = new FrontendIPConfiguration();
                frontendipconfiguration.properties = frontendipconfiguration_properties;

                List<FrontendIPConfiguration> frontendipconfigurations = new List<FrontendIPConfiguration>();
                frontendipconfigurations.Add(frontendipconfiguration);
                loadbalancer_properties.frontendIPConfigurations = frontendipconfigurations;

                Hashtable backendaddresspool = new Hashtable();
                backendaddresspool.Add("name", "default");
                List<Hashtable> backendaddresspools = new List<Hashtable>();
                backendaddresspools.Add(backendaddresspool);
                loadbalancer_properties.backendAddressPools = backendaddresspools;

                List<InboundNatRule> inboundnatrules = new List<InboundNatRule>();
                List<LoadBalancingRule> loadbalancingrules = new List<LoadBalancingRule>();
                List<Probe> probes = new List<Probe>();

                loadbalancer_properties.inboundNatRules = inboundnatrules;
                loadbalancer_properties.loadBalancingRules = loadbalancingrules;
                loadbalancer_properties.probes = probes;
                loadbalancer.properties = loadbalancer_properties;
            }

            LoadBalancer_Properties properties = (LoadBalancer_Properties)loadbalancer.properties;

            // Add Load Balancer Rules
            foreach (AsmLoadBalancerRule asmLoadBalancerRule in asmVirtualMachine.LoadBalancerRules)
            {
                if (asmLoadBalancerRule.LoadBalancedEndpointSetName == String.Empty) // if it's a inbound nat rule
                {
                    InboundNatRule_Properties inboundnatrule_properties = new InboundNatRule_Properties();
                    inboundnatrule_properties.frontendPort = asmLoadBalancerRule.Port;
                    inboundnatrule_properties.backendPort = asmLoadBalancerRule.LocalPort;
                    inboundnatrule_properties.protocol = asmLoadBalancerRule.Protocol;

                    Reference frontendIPConfiguration = new Reference();
                    frontendIPConfiguration.id = "[concat(" + ArmConst.ResourceGroupId + ", '" + ArmConst.ProviderLoadBalancers + loadbalancer.name + "/frontendIPConfigurations/default')]";
                    inboundnatrule_properties.frontendIPConfiguration = frontendIPConfiguration;

                    InboundNatRule inboundnatrule = new InboundNatRule();
                    inboundnatrule.name = asmVirtualMachine.RoleName + "-" + asmLoadBalancerRule.Name;
                    inboundnatrule.name = inboundnatrule.name.Replace(" ", String.Empty);  // future enhancement, move to target name
                    inboundnatrule.properties = inboundnatrule_properties;

                    if (!properties.inboundNatRules.Contains(inboundnatrule))
                        properties.inboundNatRules.Add(inboundnatrule);
                }
                else // if it's a load balancing rule
                {
                    string name = asmLoadBalancerRule.LoadBalancedEndpointSetName.Replace(" ", String.Empty);

                    // build probe
                    Probe_Properties probe_properties = new Probe_Properties();
                    probe_properties.port = asmLoadBalancerRule.ProbePort;
                    probe_properties.protocol = asmLoadBalancerRule.ProbeProtocol;

                    Probe probe = new Probe();
                    probe.name = name;
                    probe.properties = probe_properties;

                    if (!properties.probes.Contains(probe))
                        properties.probes.Add(probe);

                    // build load balancing rule
                    Reference frontendipconfiguration_ref = new Reference();
                    frontendipconfiguration_ref.id = "[concat(" + ArmConst.ResourceGroupId + ", '" + ArmConst.ProviderLoadBalancers + loadbalancer.name + "/frontendIPConfigurations/default')]";

                    Reference backendaddresspool_ref = new Reference();
                    backendaddresspool_ref.id = "[concat(" + ArmConst.ResourceGroupId + ", '" + ArmConst.ProviderLoadBalancers + loadbalancer.name + "/backendAddressPools/default')]";

                    Reference probe_ref = new Reference();
                    probe_ref.id = "[concat(" + ArmConst.ResourceGroupId + ",'" + ArmConst.ProviderLoadBalancers + loadbalancer.name + "/probes/" + probe.name + "')]";

                    LoadBalancingRule_Properties loadbalancingrule_properties = new LoadBalancingRule_Properties();
                    loadbalancingrule_properties.frontendIPConfiguration = frontendipconfiguration_ref;
                    loadbalancingrule_properties.backendAddressPool = backendaddresspool_ref;
                    loadbalancingrule_properties.probe = probe_ref;
                    loadbalancingrule_properties.frontendPort = asmLoadBalancerRule.Port;
                    loadbalancingrule_properties.backendPort = asmLoadBalancerRule.LocalPort;
                    loadbalancingrule_properties.protocol = asmLoadBalancerRule.Protocol;

                    LoadBalancingRule loadbalancingrule = new LoadBalancingRule();
                    loadbalancingrule.name = name;
                    loadbalancingrule.properties = loadbalancingrule_properties;

                    if (!properties.loadBalancingRules.Contains(loadbalancingrule))
                        properties.loadBalancingRules.Add(loadbalancingrule);

                    _logProvider.WriteLog("BuildLoadBalancerRules", ArmConst.ProviderLoadBalancers + loadbalancer.name + "/loadBalancingRules/" + loadbalancingrule.name);
                }
            }

            // Add the load balancer only if there is any Load Balancing rule or Inbound NAT rule
            if (properties.inboundNatRules.Count > 0 || properties.loadBalancingRules.Count > 0)
            {
                templateResult.AddResource(loadbalancer);
            }
            else
            {
                _logProvider.WriteLog("BuildLoadBalancerObject", "EMPTY Microsoft.Network/loadBalancers/" + loadbalancer.name);
            }

            _logProvider.WriteLog("BuildLoadBalancerObject", "End");
        }

        private async Task BuildVirtualNetworkObject(TemplateResult templateResult, AsmVirtualNetwork asmVirtualNetwork)
        {
            _logProvider.WriteLog("BuildVirtualNetworkObject", "Start");

            List<string> dependson = new List<string>();

            AddressSpace addressspace = new AddressSpace();
            addressspace.addressPrefixes = asmVirtualNetwork.AddressPrefixes;

            VirtualNetwork_dhcpOptions dhcpoptions = new VirtualNetwork_dhcpOptions();
            dhcpoptions.dnsServers = asmVirtualNetwork.DnsServers;

            VirtualNetwork virtualnetwork = new VirtualNetwork(templateResult.ExecutionGuid);
            virtualnetwork.name = asmVirtualNetwork.GetFinalTargetName();
            virtualnetwork.location = templateResult.TargetResourceGroup.Location.Name;
            virtualnetwork.dependsOn = dependson;

            List<Subnet> subnets = new List<Subnet>();
            if (asmVirtualNetwork.Subnets.Count == 0)
            {
                Subnet_Properties properties = new Subnet_Properties();
                properties.addressPrefix = asmVirtualNetwork.AddressPrefixes[0];

                Subnet subnet = new Subnet();
                subnet.name = "Subnet1";
                subnet.properties = properties;

                subnets.Add(subnet);
                templateResult.Messages.Add($"VNET '{virtualnetwork.name}' has no subnets defined. We've created a default subnet 'Subnet1' covering the entire address space.");
            }
            else
            {
                foreach (AsmSubnet asmSubnet in asmVirtualNetwork.Subnets)
                {
                    Subnet_Properties properties = new Subnet_Properties();
                    properties.addressPrefix = asmSubnet.AddressPrefix;

                    Subnet subnet = new Subnet();
                    subnet.name = asmSubnet.TargetName;
                    subnet.properties = properties;

                    subnets.Add(subnet);

                    // add Network Security Group if exists
                    if (asmSubnet.NetworkSecurityGroup != null)
                    {
                        AsmNetworkSecurityGroup asmNetworkSecurityGroup = _ASMArtifacts.SeekNetworkSecurityGroup(asmSubnet.NetworkSecurityGroup.Name);

                        if (asmNetworkSecurityGroup == null)
                        {
                            templateResult.Messages.Add("Subnet '" + subnet.name + "' utilized ASM Network Security Group (NSG) '" + asmSubnet.NetworkSecurityGroup.Name + "', which has not been added to the ARM Subnet as the NSG was not included in the ARM Template (was not selected as an included resources for export).");
                        }
                        else
                        {
                            // Add NSG reference to the subnet
                            Reference networksecuritygroup_ref = new Reference();
                            networksecuritygroup_ref.id = "[concat(" + ArmConst.ResourceGroupId + ", '" + ArmConst.ProviderNetworkSecurityGroups + asmNetworkSecurityGroup.GetFinalTargetName() + "')]";

                            properties.networkSecurityGroup = networksecuritygroup_ref;

                            // Add NSG dependsOn to the Virtual Network object
                            if (!virtualnetwork.dependsOn.Contains(networksecuritygroup_ref.id))
                            {
                                virtualnetwork.dependsOn.Add(networksecuritygroup_ref.id);
                            }
                        }
                    }

                    // add Route Table if exists
//                    if (subnetnode.SelectNodes("RouteTableName").Count > 0)
                    if (asmSubnet.RouteTable != null)
                    {
                        RouteTable routetable = await BuildRouteTable(templateResult, asmSubnet.RouteTable);

                        // Add Route Table reference to the subnet
                        Reference routetable_ref = new Reference();
                        routetable_ref.id = "[concat(" + ArmConst.ResourceGroupId + ", '" + ArmConst.ProviderRouteTables + routetable.name + "')]";

                        properties.routeTable = routetable_ref;

                        // Add Route Table dependsOn to the Virtual Network object
                        if (!virtualnetwork.dependsOn.Contains(routetable_ref.id))
                        {
                            virtualnetwork.dependsOn.Add(routetable_ref.id);
                        }
                    }
                }
            }

            VirtualNetwork_Properties virtualnetwork_properties = new VirtualNetwork_Properties();
            virtualnetwork_properties.addressSpace = addressspace;
            virtualnetwork_properties.subnets = subnets;
            virtualnetwork_properties.dhcpOptions = dhcpoptions;

            virtualnetwork.properties = virtualnetwork_properties;

            templateResult.AddResource(virtualnetwork);

            await AddGatewaysToVirtualNetwork(templateResult, asmVirtualNetwork, virtualnetwork);

            _logProvider.WriteLog("BuildVirtualNetworkObject", "End");
        }

        private async Task AddGatewaysToVirtualNetwork(TemplateResult templateResult, AsmVirtualNetwork asmVirtualNetwork, VirtualNetwork virtualnetwork)
        {
            // Process Virtual Network Gateway, if exists
            if ((asmVirtualNetwork.Gateway != null) && (asmVirtualNetwork.Gateway.IsProvisioned))
            {
                // Gateway Public IP Address
                PublicIPAddress_Properties publicipaddress_properties = new PublicIPAddress_Properties();
                publicipaddress_properties.publicIPAllocationMethod = "Dynamic";

                PublicIPAddress publicipaddress = new PublicIPAddress(templateResult.ExecutionGuid);
                publicipaddress.name = asmVirtualNetwork.TargetName + _settingsProvider.VirtualNetworkGatewaySuffix + _settingsProvider.PublicIPSuffix;
                publicipaddress.location = virtualnetwork.location;
                publicipaddress.properties = publicipaddress_properties;

                templateResult.AddResource(publicipaddress);

                // Virtual Network Gateway
                Reference subnet_ref = new Reference();
                subnet_ref.id = "[concat(" + ArmConst.ResourceGroupId + ", '" + ArmConst.ProviderVirtualNetwork + virtualnetwork.name + "/subnets/" + ArmConst.GatewaySubnetName + "')]";

                Reference publicipaddress_ref = new Reference();
                publicipaddress_ref.id = "[concat(" + ArmConst.ResourceGroupId + ", '" + ArmConst.ProviderPublicIpAddress + publicipaddress.name + "')]";

                var dependson = new List<string>();
                dependson.Add("[concat(" + ArmConst.ResourceGroupId + ", '" + ArmConst.ProviderVirtualNetwork + virtualnetwork.name + "')]");
                dependson.Add("[concat(" + ArmConst.ResourceGroupId + ", '" + ArmConst.ProviderPublicIpAddress + publicipaddress.name + "')]");

                IpConfiguration_Properties ipconfiguration_properties = new IpConfiguration_Properties();
                ipconfiguration_properties.privateIPAllocationMethod = "Dynamic";
                ipconfiguration_properties.subnet = subnet_ref;
                ipconfiguration_properties.publicIPAddress = publicipaddress_ref;

                IpConfiguration virtualnetworkgateway_ipconfiguration = new IpConfiguration();
                virtualnetworkgateway_ipconfiguration.name = "GatewayIPConfig";
                virtualnetworkgateway_ipconfiguration.properties = ipconfiguration_properties;

                VirtualNetworkGateway_Sku virtualnetworkgateway_sku = new VirtualNetworkGateway_Sku();
                virtualnetworkgateway_sku.name = "Basic";
                virtualnetworkgateway_sku.tier = "Basic";

                List<IpConfiguration> virtualnetworkgateway_ipconfigurations = new List<IpConfiguration>();
                virtualnetworkgateway_ipconfigurations.Add(virtualnetworkgateway_ipconfiguration);

                VirtualNetworkGateway_Properties virtualnetworkgateway_properties = new VirtualNetworkGateway_Properties();
                virtualnetworkgateway_properties.ipConfigurations = virtualnetworkgateway_ipconfigurations;
                virtualnetworkgateway_properties.sku = virtualnetworkgateway_sku;

                // If there is VPN Client configuration
                if (asmVirtualNetwork.VPNClientAddressPrefixes.Count > 0)
                {
                    AddressSpace vpnclientaddresspool = new AddressSpace();
                    vpnclientaddresspool.addressPrefixes = asmVirtualNetwork.VPNClientAddressPrefixes;

                    VPNClientConfiguration vpnclientconfiguration = new VPNClientConfiguration();
                    vpnclientconfiguration.vpnClientAddressPool = vpnclientaddresspool;

                    //Process vpnClientRootCertificates
                    List<VPNClientCertificate> vpnclientrootcertificates = new List<VPNClientCertificate>();
                    foreach (AsmClientRootCertificate certificate in asmVirtualNetwork.ClientRootCertificates)
                    {
                        VPNClientCertificate_Properties vpnclientcertificate_properties = new VPNClientCertificate_Properties();
                        vpnclientcertificate_properties.PublicCertData = certificate.PublicCertData;

                        VPNClientCertificate vpnclientcertificate = new VPNClientCertificate();
                        vpnclientcertificate.name = certificate.TargetSubject;
                        vpnclientcertificate.properties = vpnclientcertificate_properties;

                        vpnclientrootcertificates.Add(vpnclientcertificate);
                    }

                    vpnclientconfiguration.vpnClientRootCertificates = vpnclientrootcertificates;

                    virtualnetworkgateway_properties.vpnClientConfiguration = vpnclientconfiguration;
                }

                if (asmVirtualNetwork.LocalNetworkSites.Count > 0 && asmVirtualNetwork.LocalNetworkSites[0].ConnectionType == "Dedicated")
                {
                    virtualnetworkgateway_properties.gatewayType = "ExpressRoute";
                    virtualnetworkgateway_properties.enableBgp = null;
                    virtualnetworkgateway_properties.vpnType = null;
                }
                else
                {
                    virtualnetworkgateway_properties.gatewayType = "Vpn";
                    string vpnType = asmVirtualNetwork.Gateway.GatewayType;
                    if (vpnType == "StaticRouting")
                    {
                        vpnType = "PolicyBased";
                    }
                    else if (vpnType == "DynamicRouting")
                    {
                        vpnType = "RouteBased";
                    }
                    virtualnetworkgateway_properties.vpnType = vpnType;
                }

                VirtualNetworkGateway virtualnetworkgateway = new VirtualNetworkGateway(templateResult.ExecutionGuid);
                virtualnetworkgateway.location = virtualnetwork.location;
                virtualnetworkgateway.name = asmVirtualNetwork.TargetName + _settingsProvider.VirtualNetworkGatewaySuffix;
                virtualnetworkgateway.properties = virtualnetworkgateway_properties;
                virtualnetworkgateway.dependsOn = dependson;

                templateResult.AddResource(virtualnetworkgateway);

                if (!asmVirtualNetwork.HasGatewaySubnet)
                    templateResult.Messages.Add("The Virtual Network '" + asmVirtualNetwork.TargetName + "' does not contain the necessary '" + ArmConst.GatewaySubnetName + "' subnet for deployment of the '" + virtualnetworkgateway.name + "' Gateway.");

                await AddLocalSiteToGateway(templateResult, asmVirtualNetwork, virtualnetwork, virtualnetworkgateway);
            }
        }

        private async Task AddLocalSiteToGateway(TemplateResult templateResult, AsmVirtualNetwork asmVirtualNetwork, VirtualNetwork virtualnetwork, VirtualNetworkGateway virtualnetworkgateway)
        {
            // Local Network Gateways & Connections
            foreach (AsmLocalNetworkSite asmLocalNetworkSite in asmVirtualNetwork.LocalNetworkSites)
            {
                GatewayConnection_Properties gatewayconnection_properties = new GatewayConnection_Properties();
                var dependson = new List<string>();

                if (asmLocalNetworkSite.ConnectionType == "IPsec")
                {
                    // Local Network Gateway
                    List<String> addressprefixes = asmLocalNetworkSite.AddressPrefixes;

                    AddressSpace localnetworkaddressspace = new AddressSpace();
                    localnetworkaddressspace.addressPrefixes = addressprefixes;

                    LocalNetworkGateway_Properties localnetworkgateway_properties = new LocalNetworkGateway_Properties();
                    localnetworkgateway_properties.localNetworkAddressSpace = localnetworkaddressspace;
                    localnetworkgateway_properties.gatewayIpAddress = asmLocalNetworkSite.VpnGatewayAddress;

                    LocalNetworkGateway localnetworkgateway = new LocalNetworkGateway(templateResult.ExecutionGuid);
                    localnetworkgateway.name = asmLocalNetworkSite.Name + "-LocalGateway";
                    localnetworkgateway.name = localnetworkgateway.name.Replace(" ", String.Empty);

                    localnetworkgateway.location = virtualnetwork.location;
                    localnetworkgateway.properties = localnetworkgateway_properties;

                    templateResult.AddResource(localnetworkgateway);

                    Reference localnetworkgateway_ref = new Reference();
                    localnetworkgateway_ref.id = "[concat(" + ArmConst.ResourceGroupId + ", '" + ArmConst.ProviderLocalNetworkGateways + localnetworkgateway.name + "')]";
                    dependson.Add(localnetworkgateway_ref.id);

                    gatewayconnection_properties.connectionType = asmLocalNetworkSite.ConnectionType;
                    gatewayconnection_properties.localNetworkGateway2 = localnetworkgateway_ref;

                    string connectionShareKey = asmLocalNetworkSite.SharedKey;
                    if (connectionShareKey == String.Empty)
                    {
                        gatewayconnection_properties.sharedKey = "***SHARED KEY GOES HERE***";
                        templateResult.Messages.Add($"Unable to retrieve shared key for VPN connection '{virtualnetworkgateway.name}'. Please edit the template to provide this value.");
                    }
                    else
                    {
                        gatewayconnection_properties.sharedKey = connectionShareKey;
                    }
                }
                else if (asmLocalNetworkSite.ConnectionType == "Dedicated")
                {
                    gatewayconnection_properties.connectionType = "ExpressRoute";
                    gatewayconnection_properties.peer = new Reference() { id = "/subscriptions/***/resourceGroups/***" + ArmConst.ProviderExpressRouteCircuits + "***" }; // todo, this is incomplete
                    templateResult.Messages.Add($"Gateway '{virtualnetworkgateway.name}' connects to ExpressRoute. MigAz is unable to migrate ExpressRoute circuits. Please create or convert the circuit yourself and update the circuit resource ID in the generated template.");
                }

                // Connections
                Reference virtualnetworkgateway_ref = new Reference();
                virtualnetworkgateway_ref.id = "[concat(" + ArmConst.ResourceGroupId + ", '" + ArmConst.ProviderVirtualNetworkGateways + virtualnetworkgateway.name + "')]";
                     
                dependson.Add(virtualnetworkgateway_ref.id);

                gatewayconnection_properties.virtualNetworkGateway1 = virtualnetworkgateway_ref;

                GatewayConnection gatewayconnection = new GatewayConnection(templateResult.ExecutionGuid);
                gatewayconnection.name = virtualnetworkgateway.name + "-" + asmLocalNetworkSite.TargetName + "-connection"; // TODO, HardCoded
                gatewayconnection.location = virtualnetwork.location;
                gatewayconnection.properties = gatewayconnection_properties;
                gatewayconnection.dependsOn = dependson;

                templateResult.AddResource(gatewayconnection);

            }
        }

        private async Task<NetworkSecurityGroup> BuildNetworkSecurityGroup(TemplateResult templateResult, AsmNetworkSecurityGroup asmNetworkSecurityGroup)
        {
            _logProvider.WriteLog("BuildNetworkSecurityGroup", "Start");

            NetworkSecurityGroup networksecuritygroup = new NetworkSecurityGroup(templateResult.ExecutionGuid);
            networksecuritygroup.name = asmNetworkSecurityGroup.GetFinalTargetName();
            networksecuritygroup.location = asmNetworkSecurityGroup.Location;

            NetworkSecurityGroup_Properties networksecuritygroup_properties = new NetworkSecurityGroup_Properties();
            networksecuritygroup_properties.securityRules = new List<SecurityRule>();

            // for each rule
            foreach (AsmNetworkSecurityGroupRule asmNetworkSecurityGroupRule in asmNetworkSecurityGroup.Rules)
            {
                // if not system rule
                if (!asmNetworkSecurityGroupRule.IsSystemRule)
                {
                    SecurityRule_Properties securityrule_properties = new SecurityRule_Properties();
                    securityrule_properties.description = asmNetworkSecurityGroupRule.Name;
                    securityrule_properties.direction = asmNetworkSecurityGroupRule.Type;
                    securityrule_properties.priority = asmNetworkSecurityGroupRule.Priority;
                    securityrule_properties.access = asmNetworkSecurityGroupRule.Action;
                    securityrule_properties.sourceAddressPrefix = asmNetworkSecurityGroupRule.SourceAddressPrefix;
                    securityrule_properties.destinationAddressPrefix = asmNetworkSecurityGroupRule.DestinationAddressPrefix;
                    securityrule_properties.sourcePortRange = asmNetworkSecurityGroupRule.SourcePortRange;
                    securityrule_properties.destinationPortRange = asmNetworkSecurityGroupRule.DestinationPortRange;
                    securityrule_properties.protocol = asmNetworkSecurityGroupRule.Protocol;

                    SecurityRule securityrule = new SecurityRule();
                    securityrule.name = asmNetworkSecurityGroupRule.Name;
                    securityrule.properties = securityrule_properties;

                    networksecuritygroup_properties.securityRules.Add(securityrule);
                }
            }

            networksecuritygroup.properties = networksecuritygroup_properties;

            templateResult.AddResource(networksecuritygroup);

            _logProvider.WriteLog("BuildNetworkSecurityGroup", "End");

            return networksecuritygroup;
        }

        private async Task<RouteTable> BuildRouteTable(TemplateResult templateResult, AsmRouteTable asmRouteTable)
        {
            _logProvider.WriteLog("BuildRouteTable", "Start");

            RouteTable routetable = new RouteTable(templateResult.ExecutionGuid);
            routetable.name = asmRouteTable.Name;
            routetable.location = templateResult.TargetResourceGroup.Location.Name;

            RouteTable_Properties routetable_properties = new RouteTable_Properties();
            routetable_properties.routes = new List<Route>();

            // for each route
            foreach (AsmRoute asmRoute in asmRouteTable.Routes)
            {
                //securityrule_properties.protocol = rule.SelectSingleNode("Protocol").InnerText;
                Route_Properties route_properties = new Route_Properties();
                route_properties.addressPrefix = asmRoute.AddressPrefix;

                // convert next hop type string
                switch (asmRoute.NextHopType)
                {
                    case "VirtualAppliance":
                        route_properties.nextHopType = "VirtualAppliance";
                        break;
                    case "VPNGateway":
                        route_properties.nextHopType = "VirtualNetworkGateway";
                        break;
                    case "Internet":
                        route_properties.nextHopType = "Internet";
                        break;
                    case "VNETLocal":
                        route_properties.nextHopType = "VnetLocal";
                        break;
                    case "Null":
                        route_properties.nextHopType = "None";
                        break;
                }
                if (route_properties.nextHopType == "VirtualAppliance")
                    route_properties.nextHopIpAddress = asmRoute.NextHopIpAddress;

                Route route = new Route();
                route.name = asmRoute.TargetName;
                route.properties = route_properties;

                routetable_properties.routes.Add(route);
            }

            routetable.properties = routetable_properties;

            templateResult.AddResource(routetable);

            _logProvider.WriteLog("BuildRouteTable", "End");

            return routetable;
        }

        private async Task BuildNetworkInterfaceObject(TemplateResult templateResult, AsmVirtualMachine asmVirtualMachine, List<NetworkProfile_NetworkInterface> networkinterfaces)
        {
            _logProvider.WriteLog("BuildNetworkInterfaceObject", "Start");

            Reference subnet_ref = new Reference();

            if (asmVirtualMachine.TargetSubnet != null)
                subnet_ref.id = asmVirtualMachine.TargetSubnet.TargetId;

            string privateIPAllocationMethod = "Dynamic";
            string privateIPAddress = null;
            if (asmVirtualMachine.StaticVirtualNetworkIPAddress != String.Empty)
            {
                privateIPAllocationMethod = "Static";
                privateIPAddress = asmVirtualMachine.StaticVirtualNetworkIPAddress;
            }

            List<string> dependson = new List<string>();
            if (asmVirtualMachine.TargetVirtualNetwork != null && asmVirtualMachine.TargetVirtualNetwork.GetType() == typeof(AsmVirtualNetwork))
                dependson.Add(asmVirtualMachine.TargetVirtualNetwork.TargetId);

            // If there is at least one endpoint add the reference to the LB backend pool
            List<Reference> loadBalancerBackendAddressPools = new List<Reference>();
            if (asmVirtualMachine.LoadBalancerRules.Count > 0)
            {
                Reference loadBalancerBackendAddressPool = new Reference();
                loadBalancerBackendAddressPool.id = "[concat(" + ArmConst.ResourceGroupId + ", '" + ArmConst.ProviderLoadBalancers + asmVirtualMachine.LoadBalancerName + "/backendAddressPools/default')]";

                loadBalancerBackendAddressPools.Add(loadBalancerBackendAddressPool);

                dependson.Add("[concat(" + ArmConst.ResourceGroupId + ", '" + ArmConst.ProviderLoadBalancers + asmVirtualMachine.LoadBalancerName + "')]");
            }

            // Adds the references to the inboud nat rules
            List<Reference> loadBalancerInboundNatRules = new List<Reference>();
            foreach (AsmLoadBalancerRule asmLoadBalancerRule in asmVirtualMachine.LoadBalancerRules)
            {
                if (asmLoadBalancerRule.LoadBalancedEndpointSetName == String.Empty) // don't want to add a load balance endpoint as an inbound nat rule
                {
                    string inboundnatrulename = asmVirtualMachine.RoleName + "-" + asmLoadBalancerRule.Name;
                    inboundnatrulename = inboundnatrulename.Replace(" ", String.Empty);

                    Reference loadBalancerInboundNatRule = new Reference();
                    loadBalancerInboundNatRule.id = "[concat(" + ArmConst.ResourceGroupId + ", '" + ArmConst.ProviderLoadBalancers + asmVirtualMachine.LoadBalancerName + "/inboundNatRules/" + inboundnatrulename + "')]";

                    loadBalancerInboundNatRules.Add(loadBalancerInboundNatRule);
                }
            }

            IpConfiguration_Properties ipconfiguration_properties = new IpConfiguration_Properties();
            ipconfiguration_properties.privateIPAllocationMethod = privateIPAllocationMethod;
            ipconfiguration_properties.privateIPAddress = privateIPAddress;
            ipconfiguration_properties.subnet = subnet_ref;
            ipconfiguration_properties.loadBalancerInboundNatRules = loadBalancerInboundNatRules;

            // basic size VMs cannot have load balancer rules
            if (!asmVirtualMachine.RoleSize.Contains("Basic"))
            {
                ipconfiguration_properties.loadBalancerBackendAddressPools = loadBalancerBackendAddressPools;
                // TODO, shouldn't this have an upgrade warning of being skipped if not Basic??
            }

            string ipconfiguration_name = "ipconfig1";
            IpConfiguration ipconfiguration = new IpConfiguration();
            ipconfiguration.name = ipconfiguration_name;
            ipconfiguration.properties = ipconfiguration_properties;

            List<IpConfiguration> ipConfigurations = new List<IpConfiguration>();
            ipConfigurations.Add(ipconfiguration);

            NetworkInterface_Properties networkinterface_properties = new NetworkInterface_Properties();
            networkinterface_properties.ipConfigurations = ipConfigurations;
            if (asmVirtualMachine.EnabledIpForwarding)
            {
                networkinterface_properties.enableIPForwarding = true;
            }

            NetworkInterface primaryNetworkInterface = new NetworkInterface(templateResult.ExecutionGuid);
            primaryNetworkInterface.name = asmVirtualMachine.PrimaryNetworkInterface.GetFinalTargetName();
            primaryNetworkInterface.location = templateResult.TargetResourceGroup.Location.Name;
            primaryNetworkInterface.properties = networkinterface_properties;
            primaryNetworkInterface.dependsOn = dependson;

            NetworkProfile_NetworkInterface_Properties networkinterface_ref_properties = new NetworkProfile_NetworkInterface_Properties();
            networkinterface_ref_properties.primary = true;

            NetworkProfile_NetworkInterface networkinterface_ref = new NetworkProfile_NetworkInterface();
            networkinterface_ref.id = "[concat(" + ArmConst.ResourceGroupId + ", '" + ArmConst.ProviderNetworkInterfaces + primaryNetworkInterface.name + "')]";
            networkinterface_ref.properties = networkinterface_ref_properties;

            if (asmVirtualMachine.NetworkSecurityGroup != null)
            {
                AsmNetworkSecurityGroup asmNetworkSecurityGroup = _ASMArtifacts.SeekNetworkSecurityGroup(asmVirtualMachine.NetworkSecurityGroup.Name);

                if (asmNetworkSecurityGroup == null)
                {
                    templateResult.Messages.Add("Network Interface Card (NIC) '" + primaryNetworkInterface.name + "' utilized ASM Network Security Group (NSG) '" + asmVirtualMachine.NetworkSecurityGroup.Name + "', which has not been added to the NIC as the NSG was not included in the ARM Template (was not selected as an included resources for export).");
                }
                else
                {
                    // Add NSG reference to the network interface
                    Reference networksecuritygroup_ref = new Reference();
                    networksecuritygroup_ref.id = "[concat(" + ArmConst.ResourceGroupId + ", '" + ArmConst.ProviderNetworkSecurityGroups + asmNetworkSecurityGroup.GetFinalTargetName() + "')]";

                    networkinterface_properties.NetworkSecurityGroup = networksecuritygroup_ref;
                    primaryNetworkInterface.properties = networkinterface_properties;

                    // Add NSG dependsOn to the Network Interface object
                    if (!primaryNetworkInterface.dependsOn.Contains(networksecuritygroup_ref.id))
                    {
                        primaryNetworkInterface.dependsOn.Add(networksecuritygroup_ref.id);
                    }
                }

            }

            if (asmVirtualMachine.HasPublicIPs)
            {
                BuildPublicIPAddressObject(templateResult, ref primaryNetworkInterface);
            }

            networkinterfaces.Add(networkinterface_ref);

            templateResult.AddResource(primaryNetworkInterface);

            foreach (AsmNetworkInterface asmNetworkInterface in asmVirtualMachine.NetworkInterfaces)
            {
                subnet_ref = new Reference();
                subnet_ref.id = asmNetworkInterface.Parent.TargetSubnet.TargetId;

                privateIPAllocationMethod = "Dynamic";
                privateIPAddress = null;
                if (asmNetworkInterface.StaticVirtualNetworkIPAddress != String.Empty)
                {
                    privateIPAllocationMethod = "Static";
                    privateIPAddress = asmNetworkInterface.StaticVirtualNetworkIPAddress;
                }

                ipconfiguration_properties = new IpConfiguration_Properties();
                ipconfiguration_properties.privateIPAllocationMethod = privateIPAllocationMethod;
                ipconfiguration_properties.privateIPAddress = privateIPAddress;
                ipconfiguration_properties.subnet = subnet_ref;

                ipconfiguration_name = "ipconfig1";
                ipconfiguration = new IpConfiguration();
                ipconfiguration.name = ipconfiguration_name;
                ipconfiguration.properties = ipconfiguration_properties;

                ipConfigurations = new List<IpConfiguration>();
                ipConfigurations.Add(ipconfiguration);

                networkinterface_properties = new NetworkInterface_Properties();
                networkinterface_properties.ipConfigurations = ipConfigurations;
                if (asmNetworkInterface.EnableIPForwarding)
                {
                    networkinterface_properties.enableIPForwarding = true;
                }

                dependson = new List<string>();
                dependson.Add(asmNetworkInterface.Parent.TargetVirtualNetwork.TargetId);

                NetworkInterface additionalNetworkInterface = new NetworkInterface(templateResult.ExecutionGuid);
                additionalNetworkInterface.name = asmNetworkInterface.GetFinalTargetName();
                additionalNetworkInterface.location = templateResult.TargetResourceGroup.Location.Name;
                additionalNetworkInterface.properties = networkinterface_properties;
                additionalNetworkInterface.dependsOn = dependson;

                networkinterface_ref_properties = new NetworkProfile_NetworkInterface_Properties();
                networkinterface_ref_properties.primary = false;

                networkinterface_ref = new NetworkProfile_NetworkInterface();
                networkinterface_ref.id = "[concat(" + ArmConst.ResourceGroupId + ", '" + ArmConst.ProviderNetworkInterfaces + additionalNetworkInterface.name + "')]";
                networkinterface_ref.properties = networkinterface_ref_properties;

                networkinterfaces.Add(networkinterface_ref);

                templateResult.Resources.Add(additionalNetworkInterface);
            }

            _logProvider.WriteLog("BuildNetworkInterfaceObject", "End");
        }

        private async Task BuildVirtualMachineObject(TemplateResult templateResult, AsmVirtualMachine asmVirtualMachine, List<NetworkProfile_NetworkInterface> networkinterfaces)
        {
            _logProvider.WriteLog("BuildVirtualMachineObject", "Start");

            string virtualmachinename = asmVirtualMachine.GetFinalTargetName();
            string ostype = asmVirtualMachine.OSVirtualHardDiskOS;

            string osDiskTargetStorageAccountName = String.Empty;
            if (asmVirtualMachine.OSVirtualHardDisk.TargetStorageAccount.GetType() == typeof(AsmStorageAccount))
            {
                AsmStorageAccount asmStorageAccount = (AsmStorageAccount)asmVirtualMachine.OSVirtualHardDisk.TargetStorageAccount;
                osDiskTargetStorageAccountName = asmStorageAccount.GetFinalTargetName();
            }
            else if (asmVirtualMachine.OSVirtualHardDisk.TargetStorageAccount.GetType() == typeof(ArmStorageAccount))
            {
                ArmStorageAccount armStorageAccount = (ArmStorageAccount)asmVirtualMachine.OSVirtualHardDisk.TargetStorageAccount;
                osDiskTargetStorageAccountName = armStorageAccount.Name;
            }

            string newdiskurl = asmVirtualMachine.OSVirtualHardDisk.TargetMediaLink;

            List<IStorageAccount> storageaccountdependencies = new List<IStorageAccount>();
            storageaccountdependencies.Add(asmVirtualMachine.OSVirtualHardDisk.TargetStorageAccount);

            HardwareProfile hardwareprofile = new HardwareProfile();
            hardwareprofile.vmSize = GetVMSize(asmVirtualMachine.RoleSize);

            NetworkProfile networkprofile = new NetworkProfile();
            networkprofile.networkInterfaces = networkinterfaces;

            Vhd vhd = new Vhd();
            vhd.uri = newdiskurl;

            OsDisk osdisk = new OsDisk();
            osdisk.name = asmVirtualMachine.OSVirtualHardDisk.DiskName;
            osdisk.vhd = vhd;
            osdisk.caching = asmVirtualMachine.OSVirtualHardDisk.HostCaching;

            ImageReference imagereference = new ImageReference();
            OsProfile osprofile = new OsProfile();

            // if the tool is configured to create new VMs with empty data disks
            if (_settingsProvider.BuildEmpty)
            {
                osdisk.createOption = "FromImage";

                osprofile.computerName = virtualmachinename;
                osprofile.adminUsername = "[parameters('adminUsername')]";
                osprofile.adminPassword = "[parameters('adminPassword')]";

                if (!templateResult.Parameters.ContainsKey("adminUsername"))
                {
                    Parameter parameter = new Parameter();
                    parameter.type = "string";
                    templateResult.Parameters.Add("adminUsername", parameter);
                }

                if (!templateResult.Parameters.ContainsKey("adminPassword"))
                {
                    Parameter parameter = new Parameter();
                    parameter.type = "securestring";
                    templateResult.Parameters.Add("adminPassword", parameter);
                }

                if (ostype == "Windows")
                {
                    imagereference.publisher = "MicrosoftWindowsServer";
                    imagereference.offer = "WindowsServer";
                    imagereference.sku = "2016-Datacenter";
                    imagereference.version = "latest";
                }
                else if (ostype == "Linux")
                {
                    imagereference.publisher = "Canonical";
                    imagereference.offer = "UbuntuServer";
                    imagereference.sku = "16.04.0-LTS";
                    imagereference.version = "latest";
                }
                else
                {
                    imagereference.publisher = "<publisher>";
                    imagereference.offer = "<offer>";
                    imagereference.sku = "<sku>";
                    imagereference.version = "<version>";
                }
            }
            // if the tool is configured to attach copied disks
            else
            {
                osdisk.createOption = "Attach";
                osdisk.osType = ostype;

                // Block of code to help copying the blobs to the new storage accounts
                CopyBlobDetail copyblobdetail = new CopyBlobDetail();
                copyblobdetail.SourceSA = asmVirtualMachine.OSVirtualHardDisk.StorageAccountName;
                copyblobdetail.SourceContainer = asmVirtualMachine.OSVirtualHardDisk.StorageAccountContainer;
                copyblobdetail.SourceBlob = asmVirtualMachine.OSVirtualHardDisk.StorageAccountBlob;
                copyblobdetail.SourceKey = asmVirtualMachine.OSVirtualHardDisk.SourceStorageAccount.Keys.Primary;
                copyblobdetail.DestinationSA = osDiskTargetStorageAccountName;
                copyblobdetail.DestinationContainer = asmVirtualMachine.OSVirtualHardDisk.StorageAccountContainer;
                copyblobdetail.DestinationBlob = asmVirtualMachine.OSVirtualHardDisk.StorageAccountBlob;
                templateResult.CopyBlobDetails.Add(copyblobdetail);
                // end of block of code to help copying the blobs to the new storage accounts
            }

            // process data disks
            List<DataDisk> datadisks = new List<DataDisk>();
            foreach (AsmDisk dataDisk in asmVirtualMachine.DataDisks)
            {
                string dataDiskTargetStorageAccountName = String.Empty;
                if (dataDisk.TargetStorageAccount.GetType() == typeof(AsmStorageAccount))
                {
                    AsmStorageAccount asmStorageAccount = (AsmStorageAccount)dataDisk.TargetStorageAccount;
                    dataDiskTargetStorageAccountName = asmStorageAccount.GetFinalTargetName();
                }
                else if (dataDisk.TargetStorageAccount.GetType() == typeof(ArmStorageAccount))
                {
                    ArmStorageAccount armStorageAccount = (ArmStorageAccount)dataDisk.TargetStorageAccount;
                    dataDiskTargetStorageAccountName = armStorageAccount.Name;
                }

                DataDisk datadisk = new DataDisk();
                datadisk.name = dataDisk.DiskName;
                datadisk.caching = dataDisk.HostCaching;
                datadisk.diskSizeGB = dataDisk.DiskSizeInGB;
                if (dataDisk.Lun.HasValue)
                    datadisk.lun = dataDisk.Lun.Value;

                newdiskurl = dataDisk.TargetMediaLink;

                // if the tool is configured to create new VMs with empty data disks
                if (_settingsProvider.BuildEmpty)
                {
                    datadisk.createOption = "Empty";
                }
                // if the tool is configured to attach copied disks
                else
                {
                    datadisk.createOption = "Attach";

                    // Block of code to help copying the blobs to the new storage accounts
                    CopyBlobDetail copyblobdetail = new CopyBlobDetail();
                    copyblobdetail.SourceSA = dataDisk.StorageAccountName;
                    copyblobdetail.SourceContainer = dataDisk.StorageAccountContainer;
                    copyblobdetail.SourceBlob = dataDisk.StorageAccountBlob;
                    copyblobdetail.SourceKey = dataDisk.SourceStorageAccount.Keys.Primary;
                    copyblobdetail.DestinationSA = dataDiskTargetStorageAccountName;
                    copyblobdetail.DestinationContainer = dataDisk.StorageAccountContainer;
                    copyblobdetail.DestinationBlob = dataDisk.StorageAccountBlob;
                    templateResult.CopyBlobDetails.Add(copyblobdetail);
                    // end of block of code to help copying the blobs to the new storage accounts
                }

                vhd = new Vhd();
                vhd.uri = newdiskurl;
                datadisk.vhd = vhd;

                if (!storageaccountdependencies.Contains(dataDisk.TargetStorageAccount))
                    storageaccountdependencies.Add(dataDisk.TargetStorageAccount);

                datadisks.Add(datadisk);
            }

            StorageProfile storageprofile = new StorageProfile();
            if (_settingsProvider.BuildEmpty) { storageprofile.imageReference = imagereference; }
            storageprofile.osDisk = osdisk;
            storageprofile.dataDisks = datadisks;

            VirtualMachine_Properties virtualmachine_properties = new VirtualMachine_Properties();
            virtualmachine_properties.hardwareProfile = hardwareprofile;
            if (_settingsProvider.BuildEmpty) { virtualmachine_properties.osProfile = osprofile; }
            virtualmachine_properties.networkProfile = networkprofile;
            virtualmachine_properties.storageProfile = storageprofile;

            List<string> dependson = new List<string>();
            dependson.Add("[concat(" + ArmConst.ResourceGroupId + ", '" + ArmConst.ProviderNetworkInterfaces + asmVirtualMachine.PrimaryNetworkInterface.GetFinalTargetName() + "')]");

            // Diagnostics Extension
            Extension extension_iaasdiagnostics = null;

            //XmlNodeList resourceextensionreferences = resource.SelectNodes("//ResourceExtensionReferences/ResourceExtensionReference");
            //foreach (XmlNode resourceextensionreference in resourceextensionreferences)
            //{
            //    if (resourceextensionreference.SelectSingleNode("Name").InnerText == "IaaSDiagnostics")
            //    {
            //        string json = Base64Decode(resourceextensionreference.SelectSingleNode("ResourceExtensionParameterValues/ResourceExtensionParameterValue/Value").InnerText);
            //        var resourceextensionparametervalue = JsonConvert.DeserializeObject<dynamic>(json);
            //        string diagnosticsstorageaccount = resourceextensionparametervalue.storageAccount.Value + _settingsProvider.UniquenessSuffix;
            //        string xmlcfgvalue = Base64Decode(resourceextensionparametervalue.xmlCfg.Value);
            //        xmlcfgvalue = xmlcfgvalue.Replace("\n", String.Empty);
            //        xmlcfgvalue = xmlcfgvalue.Replace("\r", String.Empty);

            //        XmlDocument xmlcfg = new XmlDocument();
            //        xmlcfg.LoadXml(xmlcfgvalue);

            //        XmlNodeList mynodelist = xmlcfg.SelectNodes("/wadCfg/DiagnosticMonitorConfiguration/Metrics");

            //        extension_iaasdiagnostics = new Extension();
            //        extension_iaasdiagnostics.name = "Microsoft.Insights.VMDiagnosticsSettings";
            //        extension_iaasdiagnostics.type = "extensions";
            //        extension_iaasdiagnostics.location = virtualmachineinfo["location"].ToString();
            //        extension_iaasdiagnostics.dependsOn = new List<string>();
            //        extension_iaasdiagnostics.dependsOn.Add("[concat(" + ArmConst.ResourceGroupId + ", '" + ArmConst.ProviderVirtualMachines + virtualmachinename + "')]");
            //        extension_iaasdiagnostics.dependsOn.Add("[concat(" + ArmConst.ResourceGroupId + ", '" + ArmConst.ProviderStorageAccounts + diagnosticsstorageaccount + "')]");

            //        Extension_Properties extension_iaasdiagnostics_properties = new Extension_Properties();
            //        extension_iaasdiagnostics_properties.publisher = "Microsoft.Azure.Diagnostics";
            //        extension_iaasdiagnostics_properties.type = "IaaSDiagnostics";
            //        extension_iaasdiagnostics_properties.typeHandlerVersion = "1.5";
            //        extension_iaasdiagnostics_properties.autoUpgradeMinorVersion = true;
            //        extension_iaasdiagnostics_properties.settings = new Dictionary<string, string>();
            //        extension_iaasdiagnostics_properties.settings.Add("xmlCfg", "[base64('" + xmlcfgvalue + "')]");
            //        extension_iaasdiagnostics_properties.settings.Add("storageAccount", diagnosticsstorageaccount);
            //        extension_iaasdiagnostics.properties = new Extension_Properties();
            //        extension_iaasdiagnostics.properties = extension_iaasdiagnostics_properties;
            //    }
            //}

            // Availability Set
            Reference availabilityset = new Reference();
            availabilityset.id = "[concat(" + ArmConst.ResourceGroupId + ", '" + ArmConst.ProviderAvailabilitySets + asmVirtualMachine.TargetAvailabilitySet.GetFinalTargetName() + "')]";
            virtualmachine_properties.availabilitySet = availabilityset;

            dependson.Add("[concat(" + ArmConst.ResourceGroupId + ", '" + ArmConst.ProviderAvailabilitySets + asmVirtualMachine.TargetAvailabilitySet.GetFinalTargetName() + "')]");

            foreach (IStorageAccount storageaccountdependency in storageaccountdependencies)
            {
                if (storageaccountdependency.GetType() == typeof(AsmStorageAccount))
                    dependson.Add("[concat(" + ArmConst.ResourceGroupId + ", '" + ArmConst.ProviderStorageAccounts + storageaccountdependency + "')]");
            }

            VirtualMachine virtualmachine = new VirtualMachine(templateResult.ExecutionGuid);
            virtualmachine.name = virtualmachinename;
            virtualmachine.location = templateResult.TargetResourceGroup.Location.Name;
            virtualmachine.properties = virtualmachine_properties;
            virtualmachine.dependsOn = dependson;
            virtualmachine.resources = new List<ArmResource>();
            if (extension_iaasdiagnostics != null) { virtualmachine.resources.Add(extension_iaasdiagnostics); }

            templateResult.AddResource(virtualmachine);

            _logProvider.WriteLog("BuildVirtualMachineObject", "End");
        }

        private void BuildStorageAccountObject(TemplateResult templateResult, AsmStorageAccount asmStorageAccount)
        {
            _logProvider.WriteLog("BuildStorageAccountObject", "Start");

            StorageAccount_Properties storageaccount_properties = new StorageAccount_Properties();
            storageaccount_properties.accountType = asmStorageAccount.AccountType;

            StorageAccount storageaccount = new StorageAccount(templateResult.ExecutionGuid);
            storageaccount.name = asmStorageAccount.GetFinalTargetName();
            storageaccount.location = templateResult.TargetResourceGroup.Location.Name;
            storageaccount.properties = storageaccount_properties;

            templateResult.AddResource(storageaccount);

            _logProvider.WriteLog("BuildStorageAccountObject", "End");
        }

        private string GetVMSize(string vmsize)
        {
            Dictionary<string, string> VMSizeTable = new Dictionary<string, string>();
            VMSizeTable.Add("ExtraSmall", "Standard_A0");
            VMSizeTable.Add("Small", "Standard_A1");
            VMSizeTable.Add("Medium", "Standard_A2");
            VMSizeTable.Add("Large", "Standard_A3");
            VMSizeTable.Add("ExtraLarge", "Standard_A4");
            VMSizeTable.Add("A5", "Standard_A5");
            VMSizeTable.Add("A6", "Standard_A6");
            VMSizeTable.Add("A7", "Standard_A7");
            VMSizeTable.Add("A8", "Standard_A8");
            VMSizeTable.Add("A9", "Standard_A9");
            VMSizeTable.Add("A10", "Standard_A10");
            VMSizeTable.Add("A11", "Standard_A11");

            if (VMSizeTable.ContainsKey(vmsize))
            {
                return VMSizeTable[vmsize];
            }
            else
            {
                return vmsize;
            }
        }
    }
}
