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
        private IAvailabilitySetSource _SourceAvailabilitySet;
        private string _TargetName = String.Empty;
        private List<VirtualMachine> _TargetVirtualMachines = new List<VirtualMachine>();
        private Int32 _PlatformUpdateDomainCount = 5;
        private Int32 _PlatformFaultDomainCount = 3;

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

        public AvailabilitySet(AzureContext azureContext, Asm.CloudService asmCloudService)
        {
            _AzureContext = azureContext;
            _SourceAvailabilitySet = asmCloudService;
            this.TargetName = _SourceAvailabilitySet.Name;
        }

        public AvailabilitySet(AzureContext azureContext, Arm.AvailabilitySet availabilitySet)
        {
            _AzureContext = azureContext;
            _SourceAvailabilitySet = availabilitySet;

            this.TargetName = _SourceAvailabilitySet.Name;
            this.PlatformFaultDomainCount = availabilitySet.PlatformFaultDomainCount;
            this.PlatformUpdateDomainCount = availabilitySet.PlatformUpdateDomainCount;
        }

        public IAvailabilitySetSource SourceAvailabilitySet
        {
            get { return _SourceAvailabilitySet; }
        }

        public String SourceName
        {
            get
            {
                if (this.SourceAvailabilitySet == null)
                    return String.Empty;
                else
                    return this.SourceAvailabilitySet.ToString();
            }
        }

        public List<VirtualMachine> TargetVirtualMachines
        {
            get { return _TargetVirtualMachines; }
        }

        public string TargetName
        {
            get { return _TargetName; }
            set { _TargetName = value.Trim().Replace(" ", String.Empty); }
        }

        // https://docs.microsoft.com/en-us/azure/virtual-machines/windows/manage-availability?toc=%2Fazure%2Fvirtual-machines%2Fwindows%2Ftoc.json
        public Int32 PlatformUpdateDomainCount
        {
            get { return _PlatformUpdateDomainCount; }
            set
            {
                if (value >= 5 && value <= 20)
                    _PlatformUpdateDomainCount = value;
                else
                    throw new ArgumentException("Platform Update Domain Count must be between 5 and 20.");
            }
        }

        public Int32 PlatformFaultDomainCount
        {
            get { return _PlatformFaultDomainCount; }
            set
            {
                if (value >= 2 && value <= 3)
                    _PlatformFaultDomainCount = value;
                else
                    throw new ArgumentException("Platform Fault Domain Count must be between 2 and 3.");
            }
        }

        public override string ToString()
        {
            return this.TargetName + this._AzureContext.SettingsProvider.AvailabilitySetSuffix;
        }

        internal bool IsManagedDisks
        {
            get
            {
                foreach (VirtualMachine targetVirtualMachine in _TargetVirtualMachines)
                {
                    if (!targetVirtualMachine.IsManagedDisks)
                        return false;
                }

                return true;
            }
        }

        internal bool IsUnmanagedDisks
        {
            get
            {
                foreach (VirtualMachine targetVirtualMachine in _TargetVirtualMachines)
                {
                    if (!targetVirtualMachine.IsUnmanagedDisks)
                        return false;
                }

                return true;
            }
        }
    }
}
