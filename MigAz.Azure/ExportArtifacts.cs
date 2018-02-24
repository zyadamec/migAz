using MigAz.Azure.MigrationTarget;
using MigAz.Core.Interface;
using System.Collections.Generic;
using System;
using MigAz.Core.Generator;
using System.Threading.Tasks;
using MigAz.Core;
using System.Linq;

namespace MigAz.Azure
{
    public class ExportArtifacts : IExportArtifacts
    {
        private List<MigAzGeneratorAlert> _Alerts = new List<MigAzGeneratorAlert>();

        public delegate Task AfterResourceValidationHandler();
        public event AfterResourceValidationHandler AfterResourceValidation;

        public ExportArtifacts()
        {
            NetworkSecurityGroups = new List<NetworkSecurityGroup>();
            RouteTables = new List<RouteTable>();
            StorageAccounts = new List<StorageAccount>();
            VirtualNetworks = new List<VirtualNetwork>();
            AvailablitySets = new List<AvailabilitySet>();
            VirtualMachines = new List<VirtualMachine>();
            LoadBalancers = new List<LoadBalancer>();
            PublicIPs = new List<PublicIp>();
            Disks = new List<Disk>();
            NetworkInterfaces = new List<NetworkInterface>();
            RouteTables = new List<RouteTable>();
        }


        public ResourceGroup ResourceGroup { get; set; }
        public List<NetworkSecurityGroup> NetworkSecurityGroups { get; }
        public List<StorageAccount> StorageAccounts { get; }
        public List<VirtualNetwork> VirtualNetworks { get; }
        public List<AvailabilitySet> AvailablitySets { get; }
        public List<VirtualMachine> VirtualMachines { get; }
        public List<LoadBalancer> LoadBalancers { get; }
        public List<PublicIp> PublicIPs { get; }
        public List<Disk> Disks { get; }
        public List<NetworkInterface> NetworkInterfaces { get; }
        public List<RouteTable> RouteTables { get; }

        public List<MigAzGeneratorAlert> Alerts
        {
            get { return _Alerts; }
        }

        public NetworkSecurityGroup SeekNetworkSecurityGroup(string sourceName)
        {
            foreach (NetworkSecurityGroup networkSecurityGroup in NetworkSecurityGroups)
            {
                if (networkSecurityGroup.ToString() == sourceName)
                    return networkSecurityGroup;
            }

            return null;
        }

        internal PublicIp SeekPublicIp(string sourceName)
        {
            foreach (PublicIp publicIp in PublicIPs)
            {
                if (publicIp.ToString() == sourceName)
                    return publicIp;
            }

            return null;
        }

        internal bool ContainsLoadBalancer(LoadBalancer loadBalancer)
        {
            foreach (LoadBalancer exportArtifactLoadBalancer in LoadBalancers)
            {
                if (exportArtifactLoadBalancer.ToString() == loadBalancer.ToString())
                    return true;
            }

            return false;
        }


        public bool HasErrors
        {
            get
            {
                foreach (MigAzGeneratorAlert alert in this.Alerts)
                {
                    if (alert.AlertType == AlertType.Error)
                        return true;
                }

                return false;
            }
        }

        public MigAzGeneratorAlert SeekAlert(string alertMessage)
        {
            foreach (MigAzGeneratorAlert alert in this.Alerts)
            {
                if (alert.Message.Contains(alertMessage))
                    return alert;
            }

            return null;
        }

        public async Task ValidateAzureResources()
        {
            Alerts.Clear();

            if (this.ResourceGroup == null)
            {
                this.AddAlert(AlertType.Error, "Target Resource Group must be provided for template generation.", this.ResourceGroup);
            }
            else
            {
                if (this.ResourceGroup.TargetLocation == null)
                {
                    this.AddAlert(AlertType.Error, "Target Resource Group Location must be provided for template generation.", this.ResourceGroup);
                }
            }

            foreach (MigrationTarget.StorageAccount targetStorageAccount in this.StorageAccounts)
            {
                if (targetStorageAccount.ToString() == targetStorageAccount.SourceName)
                    this.AddAlert(AlertType.Error, "Target Name for Storage Account '" + targetStorageAccount.ToString() + "' must be different than its Source Name.", targetStorageAccount);

                if (targetStorageAccount.BlobStorageNamespace == null || targetStorageAccount.BlobStorageNamespace.Trim().Length <= 0)
                    this.AddAlert(AlertType.Error, "Blob Storage Namespace for Target Storage Account '" + targetStorageAccount.ToString() + "' must be specified.", targetStorageAccount);
            }

            foreach (MigrationTarget.VirtualNetwork targetVirtualNetwork in this.VirtualNetworks)
            {
                foreach (MigrationTarget.Subnet targetSubnet in targetVirtualNetwork.TargetSubnets)
                {
                    if (targetSubnet.NetworkSecurityGroup != null)
                    {
                        MigrationTarget.NetworkSecurityGroup networkSecurityGroupInMigration = this.SeekNetworkSecurityGroup(targetSubnet.NetworkSecurityGroup.ToString());

                        if (networkSecurityGroupInMigration == null)
                        {
                            this.AddAlert(AlertType.Error, "Virtual Network '" + targetVirtualNetwork.ToString() + "' Subnet '" + targetSubnet.ToString() + "' utilizes Network Security Group (NSG) '" + targetSubnet.NetworkSecurityGroup.ToString() + "', but the NSG resource is not added into the migration template.", targetSubnet);
                        }
                    }
                }
            }

            foreach (MigrationTarget.NetworkSecurityGroup targetNetworkSecurityGroup in this.NetworkSecurityGroups)
            {
                if (targetNetworkSecurityGroup.TargetName == string.Empty)
                    this.AddAlert(AlertType.Error, "Target Name for Network Security Group must be specified.", targetNetworkSecurityGroup);
            }

            foreach (MigrationTarget.LoadBalancer targetLoadBalancer in this.LoadBalancers)
            {
                if (targetLoadBalancer.TargetName == string.Empty)
                    this.AddAlert(AlertType.Error, "Target Name for Load Balancer must be specified.", targetLoadBalancer);

                if (targetLoadBalancer.FrontEndIpConfigurations.Count == 0)
                {
                    this.AddAlert(AlertType.Error, "Load Balancer must have a FrontEndIpConfiguration.", targetLoadBalancer);
                }
                else
                {
                    if (targetLoadBalancer.LoadBalancerType == MigrationTarget.LoadBalancerType.Internal)
                    {
                        if (targetLoadBalancer.FrontEndIpConfigurations[0].TargetSubnet == null)
                        {
                            this.AddAlert(AlertType.Error, "Internal Load Balancer must have an internal Subnet association.", targetLoadBalancer);
                        }
                        else
                        {
                            if (targetLoadBalancer.FrontEndIpConfigurations[0].TargetPrivateIPAllocationMethod == IPAllocationMethodEnum.Static)
                            {
                                if (!IPv4CIDR.IsIpAddressInAddressPrefix(targetLoadBalancer.FrontEndIpConfigurations[0].TargetSubnet.AddressPrefix, targetLoadBalancer.FrontEndIpConfigurations[0].TargetPrivateIpAddress))
                                {
                                    this.AddAlert(AlertType.Error, "Load Balancer '" + targetLoadBalancer.ToString() + "' IP Address '" + targetLoadBalancer.FrontEndIpConfigurations[0].TargetPrivateIpAddress + "' is not valid in Subnet '" + targetLoadBalancer.FrontEndIpConfigurations[0].TargetSubnet.ToString() + "' Address Prefix '" + targetLoadBalancer.FrontEndIpConfigurations[0].TargetSubnet.AddressPrefix + "'.", targetLoadBalancer);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (targetLoadBalancer.FrontEndIpConfigurations[0].PublicIp == null)
                        {
                            this.AddAlert(AlertType.Error, "Public Load Balancer must have either a Public IP association.", targetLoadBalancer);
                        }
                        else
                        {
                            // Ensure the selected Public IP Address is "in the migration" as a target new Public IP Object
                            bool publicIpExistsInMigration = false;
                            foreach (Azure.MigrationTarget.PublicIp publicIp in this.PublicIPs)
                            {
                                if (publicIp.TargetName == targetLoadBalancer.FrontEndIpConfigurations[0].PublicIp.TargetName)
                                {
                                    publicIpExistsInMigration = true;
                                    break;
                                }
                            }

                            if (!publicIpExistsInMigration)
                                this.AddAlert(AlertType.Error, "Public IP '" + targetLoadBalancer.FrontEndIpConfigurations[0].PublicIp.TargetName + "' specified for Load Balancer '" + targetLoadBalancer.ToString() + "' is not included in the migration template.", targetLoadBalancer);
                        }
                    }
                }
            }

            foreach (Azure.MigrationTarget.AvailabilitySet availablitySet in this.AvailablitySets)
            {
                if (availablitySet.TargetVirtualMachines.Count == 0)
                {
                    this.AddAlert(AlertType.Warning, "Availability Set '" + availablitySet.ToString() + "' does not contain any Virtual Machines and will be exported as an empty Availability Set.", availablitySet);
                }
                else if (availablitySet.TargetVirtualMachines.Count == 1)
                {
                    this.AddAlert(AlertType.Warning, "Availability Set '" + availablitySet.ToString() + "' only contains a single VM.  Only utilize an Availability Set if additional VMs will be added; otherwise, a single VM instance should not reside within an Availability Set.", availablitySet);
                }

                if (!availablitySet.IsManagedDisks && !availablitySet.IsUnmanagedDisks)
                {
                    this.AddAlert(AlertType.Error, "All OS and Data Disks for Virtual Machines contained within Availablity Set '" + availablitySet.ToString() + "' should be either Unmanaged Disks or Managed Disks for consistent deployment.", availablitySet);
                }
            }

            foreach (Azure.MigrationTarget.VirtualMachine virtualMachine in this.VirtualMachines)
            {
                if (virtualMachine.TargetName == string.Empty)
                    this.AddAlert(AlertType.Error, "Target Name for Virtual Machine '" + virtualMachine.ToString() + "' must be specified.", virtualMachine);

                if (virtualMachine.TargetAvailabilitySet == null)
                {
                    if (virtualMachine.OSVirtualHardDisk.TargetStorage != null && virtualMachine.OSVirtualHardDisk.TargetStorage.StorageAccountType != StorageAccountType.Premium_LRS)
                        this.AddAlert(AlertType.Warning, "Virtual Machine '" + virtualMachine.ToString() + "' is not part of an Availability Set.  OS Disk should be migrated to Azure Premium Storage to receive an Azure SLA for single server deployments.  Existing configuration will receive no (0%) Service Level Agreement (SLA).", virtualMachine);

                    foreach (Azure.MigrationTarget.Disk dataDisk in virtualMachine.DataDisks)
                    {
                        if (dataDisk.TargetStorage != null && dataDisk.TargetStorage.StorageAccountType != StorageAccountType.Premium_LRS)
                            this.AddAlert(AlertType.Warning, "Virtual Machine '" + virtualMachine.ToString() + "' is not part of an Availability Set.  Data Disk '" + dataDisk.ToString() + "' should be migrated to Azure Premium Storage to receive an Azure SLA for single server deployments.  Existing configuration will receive no (0%) Service Level Agreement (SLA).", virtualMachine);
                    }
                }
                else
                {
                    bool virtualMachineAvailabitySetExists = false;
                    foreach (MigrationTarget.AvailabilitySet targetAvailabilitySet in this.AvailablitySets)
                    {
                        if (targetAvailabilitySet.ToString() == virtualMachine.TargetAvailabilitySet.ToString())
                            virtualMachineAvailabitySetExists = true;
                    }

                    if (!virtualMachineAvailabitySetExists)
                        this.AddAlert(AlertType.Error, "Virtual Machine '" + virtualMachine.ToString() + "' utilizes Availability Set '" + virtualMachine.TargetAvailabilitySet.ToString() + "'; however, the Availability Set is not included in the Export.", virtualMachine);
                }

                if (virtualMachine.TargetSize == null)
                {
                    this.AddAlert(AlertType.Error, "Target Size for Virtual Machine '" + virtualMachine.ToString() + "' must be specified.", virtualMachine);
                }
                else
                {
                    // Ensure that the selected target size is available in the target Azure Location
                    if (this.ResourceGroup != null && this.ResourceGroup.TargetLocation != null)
                    {
                        List<Arm.VMSize> vmSizes = await this.ResourceGroup.TargetLocation.GetAzureARMVMSizes();
                        if (vmSizes == null || vmSizes.Count == 0)
                        {
                            this.AddAlert(AlertType.Error, "No ARM VM Sizes are available for Azure Location '" + this.ResourceGroup.TargetLocation.DisplayName + "'.", virtualMachine);
                        }
                        else
                        {
                            // Ensure selected target VM Size is available in the Target Azure Location
                            Arm.VMSize matchedVmSize = vmSizes.Where(a => a.Name == virtualMachine.TargetSize.Name).FirstOrDefault();
                            if (matchedVmSize == null)
                                this.AddAlert(AlertType.Error, "Specified VM Size '" + virtualMachine.TargetSize.Name + "' for Virtual Machine '" + virtualMachine.ToString() + "' is invalid as it is not available in Azure Location '" + this.ResourceGroup.TargetLocation.DisplayName + "'.", virtualMachine);
                        }
                    }

                    if (virtualMachine.OSVirtualHardDisk.TargetStorage.StorageAccountType == StorageAccountType.Premium_LRS && !virtualMachine.TargetSize.IsStorageTypeSupported(virtualMachine.OSVirtualHardDisk.StorageAccountType))
                    {
                        this.AddAlert(AlertType.Error, "Premium Disk based Virtual Machines must be of VM Series 'B', 'DS', 'DS v2', 'DS v3', 'GS', 'GS v2', 'Ls' or 'Fs'.", virtualMachine);
                    }
                }

                foreach (Azure.MigrationTarget.NetworkInterface networkInterface in virtualMachine.NetworkInterfaces)
                {
                    // Seek the inclusion of the Network Interface in the export object
                    bool networkInterfaceExistsInExport = false;
                    foreach (Azure.MigrationTarget.NetworkInterface targetNetworkInterface in this.NetworkInterfaces)
                    {
                        if (String.Compare(networkInterface.SourceName, targetNetworkInterface.SourceName, true) == 0)
                        {
                            networkInterfaceExistsInExport = true;
                            break;
                        }
                    }
                    if (!networkInterfaceExistsInExport)
                    {
                        this.AddAlert(AlertType.Error, "Network Interface Card (NIC) '" + networkInterface.ToString() + "' is used by Virtual Machine '" + virtualMachine.ToString() + "', but is not included in the exported resources.", virtualMachine);
                    }

                    if (networkInterface.NetworkSecurityGroup != null)
                    {
                        MigrationTarget.NetworkSecurityGroup networkSecurityGroupInMigration = this.SeekNetworkSecurityGroup(networkInterface.NetworkSecurityGroup.ToString());

                        if (networkSecurityGroupInMigration == null)
                        {
                            this.AddAlert(AlertType.Error, "Network Interface Card (NIC) '" + networkInterface.ToString() + "' utilizes Network Security Group (NSG) '" + networkInterface.NetworkSecurityGroup.ToString() + "', but the NSG resource is not added into the migration template.", networkInterface);
                        }
                    }

                    foreach (Azure.MigrationTarget.NetworkInterfaceIpConfiguration ipConfiguration in networkInterface.TargetNetworkInterfaceIpConfigurations)
                    {
                        if (ipConfiguration.TargetVirtualNetwork == null)
                            this.AddAlert(AlertType.Error, "Target Virtual Network for Virtual Machine '" + virtualMachine.ToString() + "' Network Interface '" + networkInterface.ToString() + "' must be specified.", networkInterface);
                        else
                        {
                            if (ipConfiguration.TargetVirtualNetwork.GetType() == typeof(MigrationTarget.VirtualNetwork))
                            {
                                MigrationTarget.VirtualNetwork virtualMachineTargetVirtualNetwork = (MigrationTarget.VirtualNetwork)ipConfiguration.TargetVirtualNetwork;
                                bool targetVNetExists = false;

                                foreach (MigrationTarget.VirtualNetwork targetVirtualNetwork in this.VirtualNetworks)
                                {
                                    if (targetVirtualNetwork.TargetName == virtualMachineTargetVirtualNetwork.TargetName)
                                    {
                                        targetVNetExists = true;
                                        break;
                                    }
                                }

                                if (!targetVNetExists)
                                    this.AddAlert(AlertType.Error, "Target Virtual Network '" + virtualMachineTargetVirtualNetwork.ToString() + "' for Virtual Machine '" + virtualMachine.ToString() + "' Network Interface '" + networkInterface.ToString() + "' is invalid, as it is not included in the migration / template.", networkInterface);
                            }
                        }

                        if (ipConfiguration.TargetSubnet == null)
                            this.AddAlert(AlertType.Error, "Target Subnet for Virtual Machine '" + virtualMachine.ToString() + "' Network Interface '" + networkInterface.ToString() + "' must be specified.", networkInterface);
                        else
                        {
                            if (!IPv4CIDR.IsValidCIDR(ipConfiguration.TargetSubnet.AddressPrefix))
                            {
                                this.AddAlert(AlertType.Error, "Target Subnet '" + ipConfiguration.TargetSubnet.ToString() + "' used by Virtual Machine '" + virtualMachine.ToString() + "' has an invalid IPv4 Address Prefix: " + ipConfiguration.TargetSubnet.AddressPrefix, ipConfiguration.TargetSubnet);
                            }
                            else
                            {
                                if (ipConfiguration.TargetPrivateIPAllocationMethod == IPAllocationMethodEnum.Static)
                                {
                                    if (!IPv4CIDR.IsIpAddressInAddressPrefix(ipConfiguration.TargetSubnet.AddressPrefix, ipConfiguration.TargetPrivateIpAddress))
                                    {
                                        this.AddAlert(AlertType.Error, "Target IP Address '" + ipConfiguration.TargetPrivateIpAddress + "' is not valid in Subnet '" + ipConfiguration.TargetSubnet.ToString() + "' Address Prefix '" + ipConfiguration.TargetSubnet.AddressPrefix + "'.", networkInterface);
                                    }
                                }
                            }

                        }

                        if (ipConfiguration.TargetPublicIp != null)
                        {
                            MigrationTarget.PublicIp publicIpInMigration = this.SeekPublicIp(ipConfiguration.TargetPublicIp.ToString());

                            if (publicIpInMigration == null)
                            {
                                this.AddAlert(AlertType.Error, "Network Interface Card (NIC) '" + networkInterface.ToString() + "' IP Configuration '" + ipConfiguration.ToString() + "' utilizes Public IP '" + ipConfiguration.TargetPublicIp.ToString() + "', but the Public IP resource is not added into the migration template.", networkInterface);
                            }
                        }
                    }
                }

                ValidateVMDisk(virtualMachine.OSVirtualHardDisk);
                foreach (MigrationTarget.Disk dataDisk in virtualMachine.DataDisks)
                {
                    ValidateVMDisk(dataDisk);
                }

                if (!virtualMachine.IsManagedDisks && !virtualMachine.IsUnmanagedDisks)
                {
                    this.AddAlert(AlertType.Error, "All OS and Data Disks for Virtual Machine '" + virtualMachine.ToString() + "' should be either Unmanaged Disks or Managed Disks for consistent deployment.", virtualMachine);
                }
            }

            foreach (Azure.MigrationTarget.Disk targetDisk in this.Disks)
            {
                ValidateDiskStandards(targetDisk);
            }

            // todo now asap - Add test for NSGs being present in Migration
            //MigrationTarget.NetworkSecurityGroup targetNetworkSecurityGroup = (MigrationTarget.NetworkSecurityGroup)this.SeekNetworkSecurityGroup(targetSubnet.NetworkSecurityGroup.ToString());
            //if (targetNetworkSecurityGroup == null)
            //{
            //    this.AddAlert(AlertType.Error, "Subnet '" + subnet.name + "' utilized ASM Network Security Group (NSG) '" + targetSubnet.NetworkSecurityGroup.ToString() + "', which has not been added to the ARM Subnet as the NSG was not included in the ARM Template (was not selected as an included resources for export).", targetNetworkSecurityGroup);
            //}

            // todo add error if existing target disk storage is not in the same data center / region as vm.

            if (AfterResourceValidation != null)
                await AfterResourceValidation.Invoke();
        }

        private void ValidateVMDisk(MigrationTarget.Disk targetDisk)
        {
            if (targetDisk.IsManagedDisk)
            {
                // All Managed Disks are included in the Export Artifacts, so we aren't including a call here to ValidateDiskStandards for Managed Disks.  
                // Only non Managed Disks are validated against Disk Standards below.

                // VM References a managed disk, ensure it is included in the Export Artifacts
                bool targetDiskInExport = false;
                foreach (Azure.MigrationTarget.Disk exportDisk in this.Disks)
                {
                    if (targetDisk.SourceName == exportDisk.SourceName)
                        targetDiskInExport = true;
                }

                if (!targetDiskInExport && targetDisk.ParentVirtualMachine != null)
                {
                    this.AddAlert(AlertType.Error, "Virtual Machine '" + targetDisk.ParentVirtualMachine.SourceName + "' references Managed Disk '" + targetDisk.SourceName + "' which has not been added as an export resource.", targetDisk.ParentVirtualMachine);
                }
            }
            else
            {
                // We are calling Validate Disk Standards here (only for non-managed disks, as noted above) as all Managed Disks are validated for Disk Standards through the ExportArtifacts.Disks Collection
                ValidateDiskStandards(targetDisk);
            }

        }

        private void ValidateDiskStandards(MigrationTarget.Disk targetDisk)
        {
            if (targetDisk.DiskSizeInGB == 0)
                this.AddAlert(AlertType.Error, "Disk '" + targetDisk.ToString() + "' does not have a Disk Size defined.  Disk Size (not to exceed 4095 GB) is required.", targetDisk);

            if (targetDisk.IsSmallerThanSourceDisk)
                this.AddAlert(AlertType.Error, "Disk '" + targetDisk.ToString() + "' Size of " + targetDisk.DiskSizeInGB.ToString() + " GB cannot be smaller than the source Disk Size of " + targetDisk.SourceDisk.DiskSizeGb.ToString() + " GB.", targetDisk);

            if (targetDisk.SourceDisk != null && targetDisk.SourceDisk.GetType() == typeof(Azure.Arm.ClassicDisk))
            {
                if (targetDisk.SourceDisk.IsEncrypted)
                {
                    this.AddAlert(AlertType.Error, "Disk '" + targetDisk.ToString() + "' is encrypted.  MigAz does not contain support for moving encrypted Azure Compute VMs.", targetDisk);
                }
            }

            if (targetDisk.TargetStorage == null)
                this.AddAlert(AlertType.Error, "Disk '" + targetDisk.ToString() + "' Target Storage must be specified.", targetDisk);
            else
            {
                if (targetDisk.TargetStorage.GetType() == typeof(Azure.Arm.StorageAccount))
                {
                    Arm.StorageAccount armStorageAccount = (Arm.StorageAccount)targetDisk.TargetStorage;
                    if (armStorageAccount.Location.Name != this.ResourceGroup.TargetLocation.Name)
                    {
                        this.AddAlert(AlertType.Error, "Target Storage Account '" + armStorageAccount.Name + "' is not in the same region (" + armStorageAccount.Location.Name + ") as the Target Resource Group '" + this.ResourceGroup.ToString() + "' (" + this.ResourceGroup.TargetLocation.Name + ").", targetDisk);
                    }
                }
                //else if (targetDisk.TargetStorage.GetType() == typeof(Azure.MigrationTarget.StorageAccount))
                //{
                //    Azure.MigrationTarget.StorageAccount targetStorageAccount = (Azure.MigrationTarget.StorageAccount)targetDisk.TargetStorage;
                //    bool targetStorageExists = false;

                //    foreach (Azure.MigrationTarget.StorageAccount storageAccount in this.StorageAccounts)
                //    {
                //        if (storageAccount.ToString() == targetStorageAccount.ToString())
                //        {
                //            targetStorageExists = true;
                //            break;
                //        }
                //    }

                //    if (!targetStorageExists)
                //        this.AddAlert(AlertType.Error, "Target Storage Account '" + targetStorageAccount.ToString() + "' for Disk '" + targetDisk.ToString() + "' is invalid, as it is not included in the migration / template.", targetDisk);
                //}

                if (targetDisk.TargetStorage.GetType() == typeof(Azure.MigrationTarget.StorageAccount) ||
                    targetDisk.TargetStorage.GetType() == typeof(Azure.Arm.StorageAccount))
                {
                    if (targetDisk.TargetStorageAccountBlob == null || targetDisk.TargetStorageAccountBlob.Trim().Length == 0)
                        this.AddAlert(AlertType.Error, "Target Storage Blob Name is required.", targetDisk);
                    else if (!targetDisk.TargetStorageAccountBlob.ToLower().EndsWith(".vhd"))
                        this.AddAlert(AlertType.Error, "Target Storage Blob Name '" + targetDisk.TargetStorageAccountBlob + "' for Disk is invalid, as it must end with '.vhd'.", targetDisk);
                }

                if (targetDisk.TargetStorage.StorageAccountType == StorageAccountType.Premium_LRS)
                {
                    if (targetDisk.DiskSizeInGB > 0 && targetDisk.DiskSizeInGB < 32)
                        this.AddAlert(AlertType.Recommendation, "Consider using disk size 32GB (P4), as this disk will be billed at that capacity per Azure Premium Storage billing sizes.", targetDisk);
                    else if (targetDisk.DiskSizeInGB > 32 && targetDisk.DiskSizeInGB < 64)
                        this.AddAlert(AlertType.Recommendation, "Consider using disk size 64GB (P6), as this disk will be billed at that capacity per Azure Premium Storage billing sizes.", targetDisk);
                    else if (targetDisk.DiskSizeInGB > 64 && targetDisk.DiskSizeInGB < 128)
                        this.AddAlert(AlertType.Recommendation, "Consider using disk size 128GB (P10), as this disk will be billed at that capacity per Azure Premium Storage billing sizes.", targetDisk);
                    else if (targetDisk.DiskSizeInGB > 128 && targetDisk.DiskSizeInGB < 512)
                        this.AddAlert(AlertType.Recommendation, "Consider using disk size 512GB (P20), as this disk will be billed at that capacity per Azure Premium Storage billing sizes.", targetDisk);
                    else if (targetDisk.DiskSizeInGB > 512 && targetDisk.DiskSizeInGB < 1023)
                        this.AddAlert(AlertType.Recommendation, "Consider using disk size 1023GB (P30), as this disk will be billed at that capacity per Azure Premium Storage billing sizes.", targetDisk);
                    else if (targetDisk.DiskSizeInGB > 1023 && targetDisk.DiskSizeInGB < 2047)
                        this.AddAlert(AlertType.Recommendation, "Consider using disk size 2047GB (P40), as this disk will be billed at that capacity per Azure Premium Storage billing sizes.", targetDisk);
                    else if (targetDisk.DiskSizeInGB > 2047 && targetDisk.DiskSizeInGB < 4095)
                        this.AddAlert(AlertType.Recommendation, "Consider using disk size 4095GB (P50), as this disk will be billed at that capacity per Azure Premium Storage billing sizes.", targetDisk);
                }
            }
        }

        private void AddAlert(AlertType alertType, string message, object sourceObject)
        {
            this.Alerts.Add(new MigAzGeneratorAlert(alertType, message, sourceObject));
        }
    }
}