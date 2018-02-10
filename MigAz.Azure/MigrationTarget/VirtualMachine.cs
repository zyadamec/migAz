using MigAz.Azure.Interface;
using MigAz.Core.Interface;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigAz.Azure.Arm;
using MigAz.Core;

namespace MigAz.Azure.MigrationTarget
{
    public class VirtualMachine : Core.MigrationTarget
    {
        private AvailabilitySet _TargetAvailabilitySet = null;
        private string _TargetName = String.Empty;
        private string _TargetNameResult = String.Empty;
        private Arm.VMSize _TargetSize;
        private List<NetworkInterface> _NetworkInterfaces = new List<NetworkInterface>();
        private List<Disk> _DataDisks = new List<Disk>();
        private Dictionary<string, string> _PlanAttributes;

        private VirtualMachine() { }

        public VirtualMachine(Asm.VirtualMachine virtualMachine, AzureSubscription azureSubscription, TargetSettings targetSettings)
        {
            this.Source = virtualMachine;
            this.SetTargetName(virtualMachine.RoleName, targetSettings);
            this.OSVirtualHardDisk = new Disk(virtualMachine.OSVirtualHardDisk, this, targetSettings);
            this.OSVirtualHardDiskOS = virtualMachine.OSVirtualHardDiskOS;

            if (targetSettings.DefaultTargetDiskType == ArmDiskType.ClassicDisk)
                this.OSVirtualHardDisk.TargetStorage = SeekTargetStorageAccount(azureSubscription.AsmTargetStorageAccounts, virtualMachine.OSVirtualHardDisk.StorageAccountName);

            foreach (Asm.Disk asmDataDisk in virtualMachine.DataDisks)
            {
                Disk targetDataDisk = new Disk(asmDataDisk, this, targetSettings);
                this.DataDisks.Add(targetDataDisk);

                if (targetSettings.DefaultTargetDiskType == ArmDiskType.ClassicDisk)
                    targetDataDisk.TargetStorage = SeekTargetStorageAccount(azureSubscription.AsmTargetStorageAccounts, asmDataDisk.StorageAccountName);
            }

            foreach (Asm.NetworkInterface asmNetworkInterface in virtualMachine.NetworkInterfaces)
            {
                NetworkInterface migrationNetworkInterface = new NetworkInterface(virtualMachine, asmNetworkInterface, azureSubscription.AsmTargetVirtualNetworks, azureSubscription.AsmTargetNetworkSecurityGroups, targetSettings);
                migrationNetworkInterface.ParentVirtualMachine = this;
                this.NetworkInterfaces.Add(migrationNetworkInterface);
            }

            #region Seek ARM Target Size

            // Get ARM Based Location (that matches location of Source ASM VM
            Arm.Location armLocation = azureSubscription.GetAzureARMLocation(virtualMachine.Location).Result;
            if (armLocation != null)
            {
                // First, try to seek matching ARM VM Size by name
                if (armLocation.VMSizes != null)
                {
                    this.TargetSize = armLocation.VMSizes.Where(a => a.Name == virtualMachine.RoleSize.Name).FirstOrDefault();

                    if (this.TargetSize == null)
                    {
                        // if not found, defer to alternate matching options

                        Dictionary<string, string> VMSizeTable = new Dictionary<string, string>();
                        VMSizeTable.Add("ExtraSmall", "Standard_A0");
                        VMSizeTable.Add("Small", "Standard_A1");
                        VMSizeTable.Add("Medium", "Standard_A2");
                        VMSizeTable.Add("Large", "Standard_A3");
                        VMSizeTable.Add("ExtraLarge", "Standard_A4");
                        VMSizeTable.Add("A5", "Standard_A5");
                        VMSizeTable.Add("A6", "Standard_A6");
                        VMSizeTable.Add("A7", "Standard_A7");
                        VMSizeTable.Add("A8", "Standard_A8");
                        VMSizeTable.Add("A9", "Standard_A9");
                        VMSizeTable.Add("A10", "Standard_A10");
                        VMSizeTable.Add("A11", "Standard_A11");

                        if (VMSizeTable.ContainsKey(virtualMachine.RoleSize.Name))
                        {
                            this.TargetSize = armLocation.VMSizes.Where(a => a.Name == VMSizeTable[virtualMachine.RoleSize.Name]).FirstOrDefault();
                        }
                    }
                }
            }

            #endregion
        }

        public VirtualMachine(Arm.VirtualMachine virtualMachine, AzureSubscription azureSubscription, TargetSettings targetSettings)
        {
            this.Source = virtualMachine;
            this.SetTargetName(virtualMachine.Name, targetSettings);
            this.TargetSize = virtualMachine.VmSize;
            this.OSVirtualHardDiskOS = virtualMachine.OSVirtualHardDiskOS;

            if (virtualMachine.OSVirtualHardDisk.GetType() == typeof(Azure.Arm.ManagedDisk))
            {
                Azure.Arm.ManagedDisk sourceManagedDisk = (Azure.Arm.ManagedDisk)virtualMachine.OSVirtualHardDisk;

                foreach (Disk targetDisk in azureSubscription.ArmTargetManagedDisks)
                {
                    if ((targetDisk.SourceDisk != null) && (targetDisk.SourceDisk.GetType() == typeof(Azure.Arm.ManagedDisk)))
                    {
                        Azure.Arm.ManagedDisk targetDiskSourceDisk = (Azure.Arm.ManagedDisk)targetDisk.SourceDisk;
                        if (String.Compare(targetDiskSourceDisk.Name, sourceManagedDisk.Name, true) == 0)
                        {
                            this.OSVirtualHardDisk = targetDisk;
                            targetDisk.ParentVirtualMachine = this;
                            targetDisk.HostCaching = sourceManagedDisk.HostCaching;
                            break;
                        }
                    }
                }
            }
            else
            {
                this.OSVirtualHardDisk = new Disk(virtualMachine.OSVirtualHardDisk, this, targetSettings);
            }

            if (virtualMachine.OSVirtualHardDisk.GetType() == typeof(Arm.ClassicDisk))
            {
                Arm.ClassicDisk armDisk = (Arm.ClassicDisk)virtualMachine.OSVirtualHardDisk;
                if (targetSettings.DefaultTargetDiskType == ArmDiskType.ClassicDisk)
                    this.OSVirtualHardDisk.TargetStorage = SeekTargetStorageAccount(azureSubscription.ArmTargetStorageAccounts, armDisk.StorageAccountName);
            }

            foreach (IArmDisk dataDisk in virtualMachine.DataDisks)
            {
                if (dataDisk.GetType() == typeof(Azure.Arm.ManagedDisk))
                {
                    Azure.Arm.ManagedDisk sourceManagedDisk = (Azure.Arm.ManagedDisk)dataDisk;

                    foreach (Disk targetDisk in azureSubscription.ArmTargetManagedDisks)
                    {
                        if ((targetDisk.SourceDisk != null) && (targetDisk.SourceDisk.GetType() == typeof(Azure.Arm.ManagedDisk)))
                        {
                            Azure.Arm.ManagedDisk targetDiskSourceDisk = (Azure.Arm.ManagedDisk)targetDisk.SourceDisk;
                            if (String.Compare(targetDiskSourceDisk.Name, sourceManagedDisk.Name, true) == 0)
                            {
                                this.DataDisks.Add(targetDisk);
                                targetDisk.ParentVirtualMachine = this;
                                targetDisk.Lun = sourceManagedDisk.Lun;
                                targetDisk.HostCaching = sourceManagedDisk.HostCaching;
                                break;
                            }
                        }
                    }
                }
                else if(dataDisk.GetType() == typeof(Arm.ClassicDisk))
                {
                    Disk targetDataDisk = new Disk(dataDisk, this, targetSettings);
                    this.DataDisks.Add(targetDataDisk);

                    Arm.ClassicDisk armDisk = (Arm.ClassicDisk)dataDisk;
                    if (targetSettings.DefaultTargetDiskType == ArmDiskType.ClassicDisk)
                        targetDataDisk.TargetStorage = SeekTargetStorageAccount(azureSubscription.ArmTargetStorageAccounts, armDisk.StorageAccountName);
                }
            }

            foreach (Arm.NetworkInterface armNetworkInterface in virtualMachine.NetworkInterfaces)
            {
                foreach (NetworkInterface targetNetworkInterface in azureSubscription.ArmTargetNetworkInterfaces)
                {
                    if ((targetNetworkInterface.SourceNetworkInterface != null) && (targetNetworkInterface.SourceNetworkInterface.GetType() == typeof(Azure.Arm.NetworkInterface)))
                    {
                        Azure.Arm.NetworkInterface targetNetworkInterfaceSourceInterface = (Azure.Arm.NetworkInterface)targetNetworkInterface.SourceNetworkInterface;
                        if (String.Compare(targetNetworkInterfaceSourceInterface.Name, armNetworkInterface.Name, true) == 0)
                        {
                            this.NetworkInterfaces.Add(targetNetworkInterface);
                            targetNetworkInterface.ParentVirtualMachine = this;
                            break;
                        }
                    }
                }

                
            }

            if (virtualMachine.HasPlan)
            {
                _PlanAttributes = new Dictionary<string, string>();

                foreach (JProperty planAttribute in virtualMachine.ResourceToken["plan"])
                {
                    _PlanAttributes.Add(planAttribute.Name, planAttribute.Value.ToString());
                }
            }
        }

        private StorageAccount SeekTargetStorageAccount(List<StorageAccount> storageAccounts, string sourceAccountName)
        {
            foreach (StorageAccount targetStorageAccount in storageAccounts)
            {
                if (targetStorageAccount.SourceName == sourceAccountName)
                    return targetStorageAccount;
            }

            return null;
        }

        public Disk OSVirtualHardDisk
        {
            get; set;
        }

        public List<Disk> DataDisks
        {
            get { return _DataDisks; }
        }

        public IVirtualMachine Source { get; set; }

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

        public Arm.VMSize TargetSize
        {
            get { return _TargetSize; }
            set { _TargetSize = value; }
        }


        public Dictionary<string, string> PlanAttributes
        {
            get { return _PlanAttributes; }
            set { _PlanAttributes = value; }
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

        public AvailabilitySet TargetAvailabilitySet
        {
            get { return _TargetAvailabilitySet; }
            set
            {
                if (_TargetAvailabilitySet != null)
                {
                    _TargetAvailabilitySet.TargetVirtualMachines.Remove(this);
                }

                _TargetAvailabilitySet = value;

                if (_TargetAvailabilitySet != null)
                {
                    _TargetAvailabilitySet.TargetVirtualMachines.Add(this);
                }
            }
        }

        public bool IsManagedDisks
        {
            get
            {
                if (!this.OSVirtualHardDisk.IsManagedDisk)
                    return false;

                foreach (Azure.MigrationTarget.Disk dataDisk in this.DataDisks)
                {
                    if (!dataDisk.IsManagedDisk)
                        return false;
                }

                return true;
            }
        }

        public bool IsUnmanagedDisks
        {
            get
            {
                if (!this.OSVirtualHardDisk.IsUnmanagedDisk)
                    return false;

                foreach (Azure.MigrationTarget.Disk dataDisk in this.DataDisks)
                {
                    if (!dataDisk.IsUnmanagedDisk)
                        return false;
                }

                return true;
            }
        }

        public string TargetName
        {
            get { return _TargetName; }
        }

        public string TargetNameResult
        {
            get { return _TargetNameResult; }
        }

        public override void SetTargetName(string targetName, TargetSettings targetSettings)
        {
            _TargetName = targetName.Trim().Replace(" ", String.Empty);
            _TargetNameResult = _TargetName + targetSettings.VirtualMachineSuffix;
        }

        public override string ToString()
        {
            return this.TargetNameResult;
        }
    }
}
