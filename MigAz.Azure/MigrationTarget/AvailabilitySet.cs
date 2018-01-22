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

            if (availabilitySet.PlatformFaultDomainCount < Constants.AvailabilitySetMinPlatformFaultDomain)
            {
                // todo future, track object translation alerts
                this.PlatformFaultDomainCount = Constants.AvailabilitySetMinPlatformFaultDomain;
            }
            else if (availabilitySet.PlatformFaultDomainCount > Constants.AvailabilitySetMaxPlatformFaultDomain)
            {
                // todo future, track object translation alerts
                this.PlatformFaultDomainCount = Constants.AvailabilitySetMaxPlatformFaultDomain;
            }
            else
            {
                this.PlatformFaultDomainCount = availabilitySet.PlatformFaultDomainCount;
            }

            if (availabilitySet.PlatformUpdateDomainCount < Constants.AvailabilitySetMinPlatformUpdateDomain)
            {
                // todo future, track object translation alerts
                this.PlatformUpdateDomainCount = Constants.AvailabilitySetMinPlatformUpdateDomain;
            }
            else if (availabilitySet.PlatformUpdateDomainCount > Constants.AvailabilitySetMaxPlatformUpdateDomain)
            {
                // todo future, track object translation alerts
                this.PlatformUpdateDomainCount = Constants.AvailabilitySetMaxPlatformUpdateDomain;
            }
            else
            {
                this.PlatformUpdateDomainCount = availabilitySet.PlatformUpdateDomainCount;
            }
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
                if (value >= Constants.AvailabilitySetMinPlatformUpdateDomain && value <= Constants.AvailabilitySetMaxPlatformUpdateDomain)
                    _PlatformUpdateDomainCount = value;
                else
                    throw new ArgumentException("Platform Update Domain Count must be between " + Constants.AvailabilitySetMinPlatformUpdateDomain.ToString() + " and " + Constants.AvailabilitySetMaxPlatformUpdateDomain.ToString() + ".");
            }
        }

        public Int32 PlatformFaultDomainCount
        {
            get { return _PlatformFaultDomainCount; }
            set
            {
                if (value >= Constants.AvailabilitySetMinPlatformFaultDomain && value <= Constants.AvailabilitySetMaxPlatformFaultDomain)
                    _PlatformFaultDomainCount = value;
                else
                    throw new ArgumentException("Platform Fault Domain Count must be between " + Constants.AvailabilitySetMinPlatformFaultDomain.ToString() + " and " + Constants.AvailabilitySetMaxPlatformFaultDomain.ToString() + ".");
            }
        }

        public override string ToString()
        {
            return this.TargetName + this._AzureContext.TargetSettings.AvailabilitySetSuffix;
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
