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

        private Disk() { }

        public Disk(Asm.Disk sourceDisk)
        {
            this.SourceDisk = sourceDisk;
            this.Name = sourceDisk.DiskName;
            this.Lun = sourceDisk.Lun;
            this.HostCaching = sourceDisk.HostCaching;
            this.DiskSizeInGB = sourceDisk.DiskSizeInGB;
            this.StorageAccountBlob = sourceDisk.StorageAccountBlob;
            this.SourceStorageAccount = sourceDisk.SourceStorageAccount;
        }

        public Disk(Arm.Disk sourceDisk)
        {
            this.SourceDisk = sourceDisk;
            this.Name = sourceDisk.Name;
            this.Lun = sourceDisk.Lun;
            this.HostCaching = sourceDisk.Caching;
            this.DiskSizeInGB = sourceDisk.DiskSizeGb;
            this.StorageAccountBlob = sourceDisk.StorageAccountBlob;
            this.SourceStorageAccount = sourceDisk.SourceStorageAccount;
        }

        public string Name
        {
            get;set;
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

        public IStorageTarget TargetStorageAccount
        {
            get;set;
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

        public IStorageTarget TargetStorageAccountName
        {
            get;set;
        }

        public string TargetMediaLink
        {
            get
            {
                string targetMediaLink = String.Empty;// this.SourceMediaLink;

                if (this.TargetStorageAccount.GetType() == typeof(Azure.Arm.StorageAccount))
                {
                    Azure.Arm.StorageAccount targetStorageAccount = (Azure.Arm.StorageAccount)this.TargetStorageAccount;
                    targetMediaLink = targetMediaLink.Replace(this.SourceStorageAccount.Name + "." + this.SourceStorageAccount.BlobStorageNamespace, targetStorageAccount.Name + "." + targetStorageAccount.BlobStorageNamespace);
                }
                else if (this.TargetStorageAccount.GetType() == typeof(Azure.MigrationTarget.StorageAccount))
                {
                    Azure.MigrationTarget.StorageAccount targetStorageAccount = (Azure.MigrationTarget.StorageAccount)this.TargetStorageAccount;
                    targetMediaLink = targetMediaLink.Replace(this.SourceStorageAccount.Name + "." + this.SourceStorageAccount.BlobStorageNamespace, targetStorageAccount.ToString() + "." + targetStorageAccount.BlobStorageNamespace);
                }

                // todo now russell targetMediaLink = targetMediaLink.Replace(this.DiskName, this.d);

                return targetMediaLink;
            }
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
