using System.Collections.Generic;

namespace MigAz.Models
{
    public class AsmArtefacts
    {
        public AsmArtefacts()
        {
            StorageAccounts = new List<Storage>();
            AllStorageAccounts = new List<Storage>();
            VirtualNetworks = new List<VirtualNW>();
            VirtualMachines = new List<VirtualMC>();
           
        }

        public List<Storage> StorageAccounts { get; private set; }
        public List<Storage> AllStorageAccounts { get; private set; }
        public List<VirtualNW> VirtualNetworks { get; private set; }
        public List<VirtualMC> VirtualMachines { get; private set; }
    

    }
}