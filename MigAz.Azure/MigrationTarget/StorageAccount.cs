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

        private StorageAccount() { }

        public StorageAccount(AzureContext azureContext, IStorageAccount source)
        {
            _AzureContext = azureContext;
            _Source = source;
            this.TargetName = source.Name;
            this.AccountType = source.AccountType;
        }

        public string TargetName { get; set; }
        public string AccountType { get; set; }
        public string BlobStorageNamespace { get; } // todo has no value assigned
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

        public override string ToString()
        {
            if (this.TargetName.Length + this._AzureContext.SettingsProvider.StorageAccountSuffix.Length > 24)
                return this.TargetName.Substring(0, 24 - this._AzureContext.SettingsProvider.StorageAccountSuffix.Length) + this._AzureContext.SettingsProvider.StorageAccountSuffix;
            else
                return this.TargetName + this._AzureContext.SettingsProvider.StorageAccountSuffix;
        }
    }
}
