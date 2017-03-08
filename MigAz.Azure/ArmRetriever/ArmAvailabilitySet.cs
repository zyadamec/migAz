using System;

namespace MigAz.Azure.Arm
{
    public class ArmAvailabilitySet : Core.ArmTemplate.AvailabilitySet
    {
        private String _TargetName = String.Empty;
        private AzureContext _AzureContext;

        private ArmAvailabilitySet() : base(Guid.Empty) { }

        public ArmAvailabilitySet(AzureContext azureContext, Asm.VirtualMachine asmVirtualMachine) : base(Guid.Empty)
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
