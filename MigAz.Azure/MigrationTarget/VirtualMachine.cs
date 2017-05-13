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
        private List<Disk> _DataDisks = new List<Disk>();

        private VirtualMachine() { }

        public VirtualMachine(AzureContext azureContext, Asm.VirtualMachine virtualMachine)
        {
            this._AzureContext = azureContext;
            this.Source = virtualMachine;
            this.TargetName = virtualMachine.RoleName;
            this._TargetSize = virtualMachine.RoleSize;
            this.OSVirtualHardDisk = new Disk(virtualMachine.OSVirtualHardDisk);
            this.OSVirtualHardDiskOS = virtualMachine.OSVirtualHardDiskOS;

            foreach (Asm.Disk disk in virtualMachine.DataDisks)
            {
                this.DataDisks.Add(new Disk(disk));
            }

            foreach (Asm.NetworkInterface asmNetworkInterface in virtualMachine.NetworkInterfaces)
            {
                Azure.MigrationTarget.NetworkInterface migrationNetworkInterface = new Azure.MigrationTarget.NetworkInterface(_AzureContext, asmNetworkInterface);
                this.NetworkInterfaces.Add(migrationNetworkInterface);

                foreach (Asm.NetworkInterfaceIpConfiguration asmNetworkInterfaceIpConfiguration in asmNetworkInterface.NetworkInterfaceIpConfigurations)
                {
                    Azure.MigrationTarget.NetworkInterfaceIpConfiguration migrationNetworkInterfaceIpConfiguration = new Azure.MigrationTarget.NetworkInterfaceIpConfiguration(_AzureContext, asmNetworkInterfaceIpConfiguration);
                    migrationNetworkInterface.TargetNetworkInterfaceIpConfigurations.Add(migrationNetworkInterfaceIpConfiguration);
                }
            }
        }

        public VirtualMachine(AzureContext azureContext, Arm.VirtualMachine virtualMachine)
        {
            this._AzureContext = azureContext;
            this.Source = virtualMachine;
            this.TargetName = virtualMachine.Name;
            this._TargetSize = virtualMachine.VmSize;
            this.OSVirtualHardDisk = new Disk(virtualMachine.OSVirtualHardDisk);
            this.OSVirtualHardDiskOS = virtualMachine.OSVirtualHardDiskOS;

            foreach (Arm.Disk disk in virtualMachine.DataDisks)
            {
                this.DataDisks.Add(new Disk(disk));
            }

            foreach (Arm.NetworkInterface armNetworkInterface in virtualMachine.NetworkInterfaces)
            {
                Azure.MigrationTarget.NetworkInterface migrationNetworkInterface = new Azure.MigrationTarget.NetworkInterface(_AzureContext, armNetworkInterface);
                this.NetworkInterfaces.Add(migrationNetworkInterface);
            }
        }

        public Disk OSVirtualHardDisk
        {
            get; set;
        }

        public List<Disk> DataDisks
        {
            get { return _DataDisks; }
        }

        public IVirtualMachine Source
        { get; set; }

        public List<NetworkInterface> NetworkInterfaces
        {
            get { return _NetworkInterfaces; }
        }

        public NetworkInterface PrimaryNetworkInterface
        {
            get
            {
                foreach (NetworkInterface networkInterface in this.NetworkInterfaces)
                {
                    if (networkInterface.IsPrimary)
                        return networkInterface;
                }

                return null;
            }
        }

        public String TargetSize
        {
            get { return _TargetSize; }
            set { _TargetSize = value.Trim(); }
        }


        public string TargetName
        {
            get { return _TargetName; }
            set { _TargetName = value.Trim().Replace(" ", String.Empty); }
        }

        public string SourceName
        {
            get
            {
                if (this.Source == null)
                    return String.Empty;
                else
                    return this.Source.ToString();
            }
        }

        public string OSVirtualHardDiskOS
        {
            get; set;
        }

        public override string ToString()
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
