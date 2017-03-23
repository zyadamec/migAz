using MigAz.Core.Interface;
using System.Collections.Generic;
using System.Windows.Forms;

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

        public List<INetworkSecurityGroup> NetworkSecurityGroups { get; }
        public List<IStorageAccount> StorageAccounts { get; }
        public List<IVirtualNetwork> VirtualNetworks { get; }
        public List<IVirtualMachine> VirtualMachines { get; }

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