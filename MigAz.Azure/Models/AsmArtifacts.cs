using MigAz.Azure.Asm;
using System.Collections.Generic;
using System;

namespace MigAz.Azure.Models
{
    public class AsmArtifacts
    {
        public AsmArtifacts()
        {
            NetworkSecurityGroups = new List<AsmNetworkSecurityGroup>();
            StorageAccounts = new List<AsmStorageAccount>();
            VirtualNetworks = new List<AsmVirtualNetwork>();
            VirtualMachines = new List<AsmVirtualMachine>();
        }

        public List<AsmNetworkSecurityGroup> NetworkSecurityGroups { get; private set; }
        public List<AsmStorageAccount> StorageAccounts { get; private set; }
        public List<AsmVirtualNetwork> VirtualNetworks { get; private set; }
        public List<AsmVirtualMachine> VirtualMachines { get; private set; }

        internal AsmNetworkSecurityGroup SeekNetworkSecurityGroup(string sourceName)
        {
            foreach (AsmNetworkSecurityGroup asmNetworkSecurityGroup in NetworkSecurityGroups)
            {
                if (asmNetworkSecurityGroup.Name == sourceName)
                    return asmNetworkSecurityGroup;
            }

            return null;
        }
    }
}