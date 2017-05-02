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

        public string TargetMediaLink
        {
            get;set;
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
    }
}
