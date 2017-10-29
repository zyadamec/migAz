using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.MigrationTarget
{
    public class ManagedDiskStorage : IStorageTarget
    {
        private StorageAccountType _StorageAccountType = StorageAccountType.Premium;

        public string BlobStorageNamespace
        {
            get
            {
                return String.Empty;
            }
        }

        public StorageAccountType StorageAccountType
        {
            get { return _StorageAccountType; }
            set { _StorageAccountType = value; }
        }
    }
}
