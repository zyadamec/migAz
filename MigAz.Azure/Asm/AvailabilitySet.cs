using System;

namespace MigAz.Azure.Asm
{
    public class AvailabilitySet : Core.ArmTemplate.AvailabilitySet
    {
        private String _TargetName = String.Empty;
        private AzureContext _AzureContext;

        private AvailabilitySet() : base(Guid.Empty) { }

        public AvailabilitySet(AzureContext azureContext, Asm.VirtualMachine asmVirtualMachine) : base(Guid.Empty)
        {
            _AzureContext = azureContext;
            if (asmVirtualMachine.AvailabilitySetName != String.Empty)
                TargetName = asmVirtualMachine.AvailabilitySetName;
            else
                TargetName = asmVirtualMachine.CloudServiceName;
        }

        public string TargetName
        {
            get { return _TargetName; }
            set { _TargetName = value; }
        }

        public string GetFinalTargetName()
        {
            return this.TargetName + this._AzureContext.SettingsProvider.AvailabilitySetSuffix;
        }
    }
}
