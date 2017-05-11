using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.MigrationTarget
{
    public class Disk : IMigrationTarget
    {
        private string _TargetName = String.Empty;


        private Disk() { }

        public Disk(Asm.Disk sourceDisk)
        {
            this.SourceDisk = sourceDisk;
            this.TargetName = sourceDisk.DiskName;
            this.Lun = sourceDisk.Lun;
            this.HostCaching = sourceDisk.HostCaching;
            this.DiskSizeInGB = sourceDisk.DiskSizeInGB;
            this.StorageAccountBlob = sourceDisk.StorageAccountBlob;
            this.SourceStorageAccount = sourceDisk.SourceStorageAccount;
        }

        public Disk(Arm.Disk sourceDisk)
        {
            this.SourceDisk = sourceDisk;
            this.TargetName = sourceDisk.Name;
            this.Lun = sourceDisk.Lun;
            this.HostCaching = sourceDisk.Caching;
            this.DiskSizeInGB = sourceDisk.DiskSizeGb;
            this.StorageAccountBlob = sourceDisk.StorageAccountBlob;
            this.SourceStorageAccount = sourceDisk.SourceStorageAccount;
        }


        public string TargetName
        {
            get { return _TargetName; }
            set { _TargetName = value.Trim().Replace(" ", String.Empty); }
        }

        public string HostCaching
        {
            get;set;
        }

        public IDisk SourceDisk { get; set; }

        public String SourceMediaLink
        {
            get
            {
                if (this.SourceDisk == null)
                    return String.Empty;

                return this.SourceDisk.MediaLink;
            }
        }

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
            get;set;
        }

        public Int64? DiskSizeInGB
        {
            get; set;
        }

        public string StorageAccountContainer
        {
            get; set;
        }
        public string StorageAccountName
        {
            get; set;
        }

        public string StorageAccountBlob
        {
            get;set;
        }

        public IStorageAccount SourceStorageAccount
        {
            get; private set;
        }

        public string SourceStorageKey
        {
            get;set;
        }

        public IStorageTarget TargetStorageAccount
        {
            get;set;
        }

        public StorageAccountType StorageAccountType // todo, this needs to reflect the storage account type from the actual storage account
        {
            get { return StorageAccountType.Standard; }
        }

        public string TargetMediaLink
        {
            get
            {
                return "https://" + TargetStorageAccount.ToString() + "." + TargetStorageAccount.BlobStorageNamespace + "/vhds/" + this.StorageAccountBlob;
            }
        }

        public override string ToString()
        {
            return this.TargetName;
        }
    }
}
