using MigAz.Azure.MigrationTarget;
using MigAz.Core.Interface;
using System.Collections.Generic;
using System;

namespace MigAz.Azure
{
    public class ExportArtifacts : IExportArtifacts
    {
        public ExportArtifacts()
        {
            NetworkSecurityGroups = new List<NetworkSecurityGroup>();
            StorageAccounts = new List<StorageAccount>();
            VirtualNetworks = new List<VirtualNetwork>();
            AvailablitySets = new List<AvailabilitySet>();
            VirtualMachines = new List<VirtualMachine>();
            LoadBalancers = new List<LoadBalancer>();
            PublicIPs = new List<PublicIp>();
            Disks = new List<Disk>();
            NetworkInterfaces = new List<NetworkInterface>();
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
    }
}