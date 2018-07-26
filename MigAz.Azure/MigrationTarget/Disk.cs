// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using MigAz.Azure.Interface;
using MigAz.Core;
using MigAz.Core.ArmTemplate;
using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.MigrationTarget
{
    public class Disk : Core.MigrationTarget
    {
        private Int32 _DiskSizeInGB = 0;
        private VirtualMachine _ParentVirtualMachine;

        #region Constructors

        private Disk() : base(ArmConst.MicrosoftCompute, ArmConst.Disks) { }

        public Disk(Asm.Disk sourceDisk, VirtualMachine parentVirtualMachine, TargetSettings targetSettings) : base(ArmConst.MicrosoftCompute, ArmConst.Disks)
        {
            this.SourceDisk = sourceDisk;
            this._ParentVirtualMachine = parentVirtualMachine;
            this.TargetStorage = new ManagedDiskStorage(sourceDisk);

            this.SetTargetName(sourceDisk.DiskName, targetSettings);
            this.Lun = sourceDisk.Lun;
            this.HostCaching = sourceDisk.HostCaching;
            this.DiskSizeInGB = sourceDisk.DiskSizeGb;
            this.TargetStorageAccountBlob = sourceDisk.StorageAccountBlob;
            this.SourceStorageAccount = sourceDisk.SourceStorageAccount;
        }

        public Disk(IArmDisk sourceDisk, VirtualMachine parentVirtualMachine, TargetSettings targetSettings) : base(ArmConst.MicrosoftCompute, ArmConst.Disks)
        {
            this.SourceDisk = (IDisk)sourceDisk;
            this._ParentVirtualMachine = parentVirtualMachine;
            this.TargetStorage = new ManagedDiskStorage(sourceDisk);

            if (sourceDisk != null)
            {
                if (sourceDisk.GetType() == typeof(Azure.Arm.ClassicDisk))
                {
                    Azure.Arm.ClassicDisk armDisk = (Azure.Arm.ClassicDisk)sourceDisk;

                    this.SetTargetName(armDisk.Name, targetSettings);
                    this.Lun = armDisk.Lun;
                    this.HostCaching = armDisk.Caching;
                    this.DiskSizeInGB = armDisk.DiskSizeGb;
                    this.TargetStorageAccountBlob = armDisk.StorageAccountBlob;
                    this.SourceStorageAccount = armDisk.SourceStorageAccount;
                }
                else if (sourceDisk.GetType() == typeof(Azure.Arm.ManagedDisk))
                {
                    Azure.Arm.ManagedDisk armManagedDisk = (Azure.Arm.ManagedDisk)sourceDisk;

                    this.SetTargetName(armManagedDisk.Name, targetSettings);
                    this.DiskSizeInGB = armManagedDisk.DiskSizeGb;
                }
            }
        }

        #endregion

        public VirtualMachine ParentVirtualMachine
        {
            get { return _ParentVirtualMachine; }
            set { _ParentVirtualMachine = value; }
        }

        public string HostCaching
        {
            get;set;
        }

        public IDisk SourceDisk { get; set; }

        public String SourceName
        {
            get
            {
                if (this.SourceDisk == null)
                    return String.Empty;
                else
                    return this.SourceDisk.ToString();
            }
        }

        public Int64? Lun
        {
            get;
            set;
        }

        public Int32 DiskSizeInGB
        {
            get { return _DiskSizeInGB; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("DiskSizeInGB cannot be negative.");

                // https://azure.microsoft.com/en-us/blog/azure-introduces-new-disks-sizes-up-to-4tb/
                if (value > 4095)
                    throw new ArgumentException("DiskSizeInGB cannot be greater than 4095 GB.");
                
                _DiskSizeInGB = value;
            }
        }

        public IStorageAccount SourceStorageAccount
        {
            get; private set;
        }

        public IStorageTarget TargetStorage
        {
            get;
            set;
        }
        
        public string TargetStorageAccountContainer
        {
            get { return "vhds"; }
        }

        public string TargetStorageAccountBlob
        {
            get; set;
        }

        public StorageAccountType StorageAccountType
        {
            get {
                if (this.TargetStorage != null)
                    return this.TargetStorage.StorageAccountType;

                return StorageAccountType.Premium_LRS;
            }
        }

        public string TargetMediaLink
        {
            get
            {
                return "https://" + TargetStorage.ToString() + "." + TargetStorage.BlobStorageNamespace + "/vhds/" + this.TargetStorageAccountBlob;
            }
        }

        public bool IsManagedDisk
        {
            get
            {
                return this.TargetStorage != null && this.TargetStorage.GetType() == typeof(Azure.MigrationTarget.ManagedDiskStorage);
            }
        }

        public bool IsUnmanagedDisk
        {
            get
            {
                return this.TargetStorage != null && (this.TargetStorage.GetType() == typeof(Azure.MigrationTarget.StorageAccount) || this.TargetStorage.GetType() == typeof(Azure.Arm.StorageAccount));
            }
        }
        public string ReferenceId
        {
            get
            {
                if (this.TargetStorage != null && this.TargetStorage.GetType() == typeof(Azure.MigrationTarget.ManagedDiskStorage))
                    return "[concat(subscription().id, '/resourcegroups/', resourceGroup().name, '/providers/Microsoft.Compute/disks/', '" + this.ToString() + "')]";
                else return 
                        String.Empty;
            }
        }

        public bool IsSmallerThanSourceDisk
        {
            get
            {
                if (this.SourceDisk == null)
                    return false;

                if (this.DiskSizeInGB < this.SourceDisk.DiskSizeGb)
                    return true;

                return false;
            }
        }

        public bool IsTargetLunDifferentThanSourceLun
        {
            get
            {
                if (this.SourceDisk == null)
                    return false; // No source disk to compare to, thus false
                else
                    return this.Lun != this.SourceDisk.Lun;
            }
        }

        public override string ImageKey { get { return "Disk"; } }

        public override string FriendlyObjectName { get { return "Disk"; } }


        public override void SetTargetName(string targetName, TargetSettings targetSettings)
        {
            this.TargetName = targetName.Trim().Replace(" ", String.Empty);
            this.TargetNameResult = this.TargetName;
        }

    }
}

