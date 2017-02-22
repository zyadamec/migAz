using MIGAZ.Core.Asm;
using System.Collections.Generic;

namespace MIGAZ.Core.Models
{
    public class AsmArtifacts
    {
        public AsmArtifacts()
        {
            StorageAccounts = new List<AsmStorageAccount>();
            VirtualNetworks = new List<AsmVirtualNetwork>();
            VirtualMachines = new List<AsmVirtualMachine>();
        }

        public List<AsmStorageAccount> StorageAccounts { get; private set; }
        public List<AsmVirtualNetwork> VirtualNetworks { get; private set; }
        public List<AsmVirtualMachine> VirtualMachines { get; private set; }
    }
}