using System;
using System.Windows.Forms;

namespace MigAzAWS.Interface
{
    public interface ISaveSelectionProvider
    {
        void Save(string AWSRegion, ListView lvwVirtualNetworks,  ListView lvwVirtualMachines);
        void Read(string AWSRegion, ref ListView lvwVirtualNetworks, ref ListView lvwVirtualMachines);
    }
}
