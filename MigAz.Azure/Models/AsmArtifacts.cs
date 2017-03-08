using MigAz.Azure.Asm;
using System.Collections.Generic;
using System;

namespace MigAz.Azure.Models
{
    public class AsmArtifacts
    {
        public AsmArtifacts()
        {
            NetworkSecurityGroups = new List<NetworkSecurityGroup>();
            StorageAccounts = new List<StorageAccount>();
            VirtualNetworks = new List<VirtualNetwork>();
            VirtualMachines = new List<VirtualMachine>();
        }

        public List<NetworkSecurityGroup> NetworkSecurityGroups { get; private set; }
        public List<StorageAccount> StorageAccounts { get; private set; }
        public List<VirtualNetwork> VirtualNetworks { get; private set; }
        public List<VirtualMachine> VirtualMachines { get; private set; }

        internal NetworkSecurityGroup SeekNetworkSecurityGroup(string sourceName)
        {
            foreach (NetworkSecurityGroup asmNetworkSecurityGroup in NetworkSecurityGroups)
            {
                if (asmNetworkSecurityGroup.Name == sourceName)
                    return asmNetworkSecurityGroup;
            }

            return null;
        }
    }
}