using MigAz.Core.Interface;
using System.Collections.Generic;

namespace MigAz.Core.Generator
{
    public class ExportArtifacts : IExportArtifacts
    {
        public ExportArtifacts()
        {
            NetworkSecurityGroups = new List<INetworkSecurityGroup>();
            StorageAccounts = new List<IStorageAccount>();
            VirtualNetworks = new List<IVirtualNetwork>();
            VirtualMachines = new List<IVirtualMachine>();
        }

        public List<INetworkSecurityGroup> NetworkSecurityGroups { get; private set; }
        public List<IStorageAccount> StorageAccounts { get; private set; }
        public List<IVirtualNetwork> VirtualNetworks { get; private set; }
        public List<IVirtualMachine> VirtualMachines { get; private set; }

        public INetworkSecurityGroup SeekNetworkSecurityGroup(string sourceName)
        {
            foreach (INetworkSecurityGroup asmNetworkSecurityGroup in NetworkSecurityGroups)
            {
                if (asmNetworkSecurityGroup.Name == sourceName)
                    return asmNetworkSecurityGroup;
            }

            return null;
        }
    }
}