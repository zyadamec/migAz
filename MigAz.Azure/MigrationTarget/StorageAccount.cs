using MigAz.Core;
using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.MigrationTarget
{
    public class StorageAccount : Core.MigrationTarget, IStorageTarget
    {
        private IStorageAccount _Source;
        private string _TargetName = String.Empty;
        private string _TargetNameResult = String.Empty;
        private StorageAccountType _StorageAccountType = StorageAccountType.Premium_LRS;

        public StorageAccount()
        {
            _TargetName = "migaz" + Guid.NewGuid().ToString().ToLower().Replace("-", "").Substring(0, 19);
            _TargetNameResult = _TargetName;
        }
        public StorageAccount(StorageAccountType storageAccountType)
        {
            _TargetName = "migaz" + Guid.NewGuid().ToString().ToLower().Replace("-", "").Substring(0, 19);
            _TargetNameResult = _TargetName;
            this.StorageAccountType = storageAccountType;
        }

        public StorageAccount(IStorageAccount source, TargetSettings targetSettings)
        {
            _Source = source;
            this.SetTargetName(source.Name, targetSettings);
            this.StorageAccountType = MigrationTarget.StorageAccount.GetStorageAccountType(source.AccountType);
        }

        public string BlobStorageNamespace
        {
            get
            {
                return "RussellTODONOW";
                //if (_AzureContext == null)
                //    return String.Empty;
                //else
                //    return _AzureContext.AzureServiceUrls.GetBlobEndpointUrl();
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


        private static StorageAccountType GetStorageAccountType(string storageAccountType)
        {
            // https://msdn.microsoft.com/en-us/library/azure/hh264518.aspx

            if (String.Compare("Premium_LRS", storageAccountType, true) == 0)
                return StorageAccountType.Premium_LRS;

            return StorageAccountType.Standard_LRS;
        }

        public string TargetName
        {
            get { return _TargetName; }
        }

        public string TargetNameResult
        {
            get { return _TargetNameResult; }
        }

        public static int MaximumTargetNameLength(TargetSettings targetSettings)
        {
            return 24 - targetSettings.StorageAccountSuffix.Length;
        }

        public override void SetTargetName(string targetName, TargetSettings targetSettings)
        {
            string value = targetName.Trim().Replace(" ", String.Empty).ToLower();

            if (value.Length + targetSettings.StorageAccountSuffix.Length > 24)
                value = value.Substring(0, 24 - targetSettings.StorageAccountSuffix.Length);

            _TargetName = value;
            _TargetNameResult = _TargetName + targetSettings.StorageAccountSuffix;
        }

        public override string ToString()
        {
            return this.TargetNameResult;
        }
    }
}
