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
        public string Name
        {
            get;set;
        }
        public string HostCaching
        {
            get;set;
        }

        public IStorageTarget TargetStorageAccount
        {
            get;set;
        }

        public Int64? Lun
        {
            get;set;
        }

        public Int64 DiskSizeInGB
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
                return string.Empty;
                //todo now russell
                //string targetMediaLink = this.TargetMediaLink;

                //if (this.TargetStorageAccount.GetType() == typeof(Azure.Arm.StorageAccount))
                //{
                //    Azure.Arm.StorageAccount targetStorageAccount = (Azure.Arm.StorageAccount)this.TargetStorageAccount;
                //    targetMediaLink = targetMediaLink.Replace(this.SourceStorageAccount.Name + "." + this.SourceStorageAccount.BlobStorageNamespace, targetStorageAccount.Name + "." + targetStorageAccount.BlobStorageNamespace);
                //}
                //else if (this.TargetStorageAccount.GetType() == typeof(Azure.MigrationTarget.StorageAccount))
                //{
                //    Azure.MigrationTarget.StorageAccount targetStorageAccount = (Azure.MigrationTarget.StorageAccount)this.TargetStorageAccount;
                //    targetMediaLink = targetMediaLink.Replace(this.SourceStorageAccount.Name + "." + this.SourceStorageAccount.BlobStorageNamespace, targetStorageAccount.ToString() + "." + targetStorageAccount.BlobStorageNamespace);
                //}

                //targetMediaLink = targetMediaLink.Replace(this.DiskName, this.TargetName);

                //return targetMediaLink;
            }
        }
    }
}
