using System;

namespace MigAz.Azure.Asm
{
    public class AvailabilitySet
    {
        private AzureContext _AzureContext;

        private AvailabilitySet() { }

        public AvailabilitySet(AzureContext azureContext, Asm.VirtualMachine asmVirtualMachine)
        {
            _AzureContext = azureContext;
        }

        public string Name // todo now russell, from constructor
        {
            get;set;
        }
    }
}
