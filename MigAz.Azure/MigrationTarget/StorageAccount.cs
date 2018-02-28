// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

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
        private StorageAccountType _StorageAccountType = StorageAccountType.Premium_LRS;

        public StorageAccount()
        {
            this.TargetName = "migaz" + Guid.NewGuid().ToString().ToLower().Replace("-", "").Substring(0, 19);
            this.TargetNameResult = this.TargetName;
        }
        public StorageAccount(StorageAccountType storageAccountType)
        {
            this.TargetName = "migaz" + Guid.NewGuid().ToString().ToLower().Replace("-", "").Substring(0, 19);
            this.TargetNameResult = this.TargetName;
            this.StorageAccountType = storageAccountType;
        }
        public StorageAccount(string name, TargetSettings targetSettings)
        {
            this.SetTargetName(name, targetSettings);
        }

        public StorageAccount(IStorageAccount source, TargetSettings targetSettings)
        {
            _Source = source;
            this.SetTargetName(source.Name, targetSettings);
            this.StorageAccountType = MigrationTarget.StorageAccount.GetStorageAccountType(source.AccountType);
        }

        public string BlobStorageNamespace
        {
            get;set;
        }

        public IStorageAccount SourceAccount
        {
            get { return _Source; }
        }

        public override string ImageKey { get { return "StorageAccount"; } }

        public override string FriendlyObjectName { get { return "Storage Account"; } }

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

        public static int MaximumTargetNameLength(TargetSettings targetSettings)
        {
            return 24 - targetSettings.StorageAccountSuffix.Length;
        }

        public override void SetTargetName(string targetName, TargetSettings targetSettings)
        {
            string value = targetName.Trim().Replace(" ", String.Empty).ToLower();

            if (value.Length + targetSettings.StorageAccountSuffix.Length > 24)
                value = value.Substring(0, 24 - targetSettings.StorageAccountSuffix.Length);

            this.TargetName = value;
            this.TargetNameResult = this.TargetName + targetSettings.StorageAccountSuffix;
        }
    }
}

