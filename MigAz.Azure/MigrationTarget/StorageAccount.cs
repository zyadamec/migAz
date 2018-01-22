using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.MigrationTarget
{
    public class StorageAccount : IStorageTarget, IMigrationTarget
    {
        private AzureContext _AzureContext = null;
        private IStorageAccount _Source;
        private string _TargetName = String.Empty;
        private StorageAccountType _StorageAccountType = StorageAccountType.Premium_LRS;

        public StorageAccount()
        {
            this.TargetName = "migaz" + Guid.NewGuid().ToString().ToLower().Replace("-", "").Substring(0, 19);
        }
        public StorageAccount(StorageAccountType storageAccountType)
        {
            this.TargetName = "migaz" + Guid.NewGuid().ToString().ToLower().Replace("-", "").Substring(0, 19);
            this.StorageAccountType = storageAccountType;
        }

        public StorageAccount(AzureContext azureContext, IStorageAccount source)
        {
            _AzureContext = azureContext;
            _Source = source;
            this.TargetName = source.Name;
            this.StorageAccountType = MigrationTarget.StorageAccount.GetStorageAccountType(source.AccountType);
        }

        public string TargetName
        {
            get { return _TargetName; }
            set { _TargetName = value.Trim().Replace(" ", String.Empty); }
        }

        public string BlobStorageNamespace
        {
            get
            {
                if (_AzureContext == null)
                    return String.Empty;
                else
                    return _AzureContext.AzureServiceUrls.GetBlobEndpointUrl();
            }
        }

        public IStorageAccount SourceAccount
        {
            get { return _Source; }
        }

        public String SourceName
        {
            get
            {
                if (this.SourceAccount == null)
                    return String.Empty;
                else
                    return this.SourceAccount.ToString();
            }
        }

        public StorageAccountType StorageAccountType
        {
            get { return _StorageAccountType; }
            set { _StorageAccountType = value; }
        }

        public override string ToString()
        {
            if (_AzureContext == null)
                return this.TargetName;
            else if (this.TargetName.Length + this._AzureContext.TargetSettings.StorageAccountSuffix.Length > 24)
                return this.TargetName.Substring(0, 24 - this._AzureContext.TargetSettings.StorageAccountSuffix.Length) + this._AzureContext.TargetSettings.StorageAccountSuffix;
            else
                return this.TargetName + this._AzureContext.TargetSettings.StorageAccountSuffix;
        }

        private static StorageAccountType GetStorageAccountType(string storageAccountType)
        {
            // https://msdn.microsoft.com/en-us/library/azure/hh264518.aspx

            if (String.Compare("Premium_LRS", storageAccountType, true) == 0)
                return StorageAccountType.Premium_LRS;

            return StorageAccountType.Standard_LRS;
        }
    }
}
