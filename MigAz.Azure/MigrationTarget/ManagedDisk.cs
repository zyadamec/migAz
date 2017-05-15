using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.MigrationTarget
{
    public class ManagedDisk
    {
        private AzureContext _AzureContext;
        private Arm.ManagedDisk _SourceManagedDisk;

        private ManagedDisk() { }

        public ManagedDisk(AzureContext azureContext, Arm.ManagedDisk managedDisk)
        {
            _AzureContext = azureContext;
            _SourceManagedDisk = managedDisk;

        }
    }
}
