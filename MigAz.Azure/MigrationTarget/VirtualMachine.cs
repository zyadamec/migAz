using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.MigrationTarget
{
    public class VirtualMachine : IMigrationTarget
    {
        private AvailabilitySet _TargetAvailabilitySet = null;
        private string _TargetName = String.Empty;
        private AzureContext _AzureContext;
        private String _TargetSize = String.Empty;
        private List<NetworkInterface> _NetworkInterfaces = new List<NetworkInterface>();

        private VirtualMachine() { }

        public VirtualMachine(AzureContext azureContext, Asm.VirtualMachine virtualMachine)
        {
            this._AzureContext = azureContext;
            this.Source = virtualMachine;
            this.TargetName = virtualMachine.RoleName;
            this._TargetSize = virtualMachine.RoleSize;
        }

        public VirtualMachine(AzureContext azureContext, Arm.VirtualMachine virtualMachine)
        {
            this._AzureContext = azureContext;
            this.Source = virtualMachine;
            this.TargetName = virtualMachine.Name;
            // this._TargetStaticIpAddress = virtualMachine.; // TODO now russell, Needed
            this._TargetSize = virtualMachine.VmSize;
        }

        public AvailabilitySet ParentAvailabilitySet // todo now russell
        {
            get;set;
        }

        public IVirtualMachine Source
        { get; set; }

        public List<NetworkInterface> NetworkInterfaces
        {
            get { return _NetworkInterfaces; }
        }


        public String TargetSize
        {
            get { return _TargetSize; }
            set { _TargetSize = value.Trim(); }
        }

    
        public string TargetName
        {
            get { return _TargetName; }
            set { _TargetName = value; }
        }

        public string GetFinalTargetName()
        {
            return this.TargetName + _AzureContext.SettingsProvider.VirtualMachineSuffix;
        }



        public AvailabilitySet TargetAvailabilitySet
        {
            get { return _TargetAvailabilitySet; }
            set { _TargetAvailabilitySet = value; }
        }
    }
}
