using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.MigrationTarget
{
    public class AvailabilitySet : IMigrationTarget
    {
        private AzureContext _AzureContext;

        private AvailabilitySet() { }

        public AvailabilitySet(AzureContext azureContext)
        {
            _AzureContext = azureContext;
        }
        public AvailabilitySet(AzureContext azureContext, Asm.VirtualMachine asmVirtualMachine)
        {
            _AzureContext = azureContext;
            this.TargetName = asmVirtualMachine.GetDefaultAvailabilitySetName();
        }

        public string TargetName
        {
            get; set;
        }

        public override string ToString()
        {
            return this.TargetName + this._AzureContext.SettingsProvider.AvailabilitySetSuffix;
        }
    }
}
