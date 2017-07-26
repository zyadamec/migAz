using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.MigrationTarget 
{
    public class ManagedDisk : IStorageTarget
    {
        private AzureContext _AzureContext;
        private StorageAccountType _StorageAccountType = StorageAccountType.Premium;

        private ManagedDisk() { }

        public ManagedDisk(AzureContext azureContext)
        {
            _AzureContext = azureContext;
        }

        public StorageAccountType StorageAccountType
        {
            get { return _StorageAccountType; }
            set { _StorageAccountType = value; }
        }

        public string BlobStorageNamespace
        {
            get
            {
                return _AzureContext.AzureServiceUrls.GetBlobEndpointUrl();
            }
        }
    }
}
