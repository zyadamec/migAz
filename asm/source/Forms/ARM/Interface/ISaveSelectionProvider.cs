using System;
using System.Windows.Forms;

namespace MigAz.Interface
{
    public interface ISaveSelectionProvider
    {
        void Save(Guid subscriptionId, ListView lvwVirtualNetworks, ListView lvwStorageAccounts, ListView lvwVirtualMachines);
        void Read(Guid subscriptionId, ref ListView lvwVirtualNetworks, ref ListView lvwStorageAccounts, ref ListView lvwVirtualMachines);
    }
}
