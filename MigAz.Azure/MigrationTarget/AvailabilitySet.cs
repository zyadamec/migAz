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
        private Arm.AvailabilitySet _SourceAvailabilitySet;

        private AvailabilitySet() { }

        public AvailabilitySet(AzureContext azureContext)
        {
            _AzureContext = azureContext;
        }
        public AvailabilitySet(AzureContext azureContext, String targetName)
        {
            _AzureContext = azureContext;
            this.TargetName = targetName;
        }
        public AvailabilitySet(AzureContext azureContext, Asm.VirtualMachine asmVirtualMachine)
        {
            _AzureContext = azureContext;
            this.TargetName = asmVirtualMachine.GetDefaultAvailabilitySetName();
        }
        public AvailabilitySet(AzureContext azureContext, Arm.AvailabilitySet availabilitySet)
        {
            _AzureContext = azureContext;
            _SourceAvailabilitySet = availabilitySet;

            this.TargetName = _SourceAvailabilitySet.Name;
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
