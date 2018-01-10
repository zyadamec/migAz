using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using MigAz.Azure.Interface;
using MigAz.Core.Interface;

namespace MigAz.Azure.Arm
{
    public class ClassicDisk : ArmResource, IArmDisk
    {
        private StorageAccount _SourceStorageAccount = null;
        private Arm.VirtualMachine _ParentVirtualMachine = null;

        public ClassicDisk(VirtualMachine virtualMachine, JToken resourceToken) : base(resourceToken)
        {
            _ParentVirtualMachine = virtualMachine;
        }

        private ClassicDisk() : base(null) { }

        public async new Task InitializeChildrenAsync(AzureContext azureContext)
        {
            _SourceStorageAccount = azureContext.AzureRetriever.GetAzureARMStorageAccount(azureContext.AzureSubscription, StorageAccountName);
            this.SourceManagedDisk = await azureContext.AzureRetriever.GetAzureARMManagedDisk(this.ParentVirtualMachine, this.Name);
        }

        public Arm.VirtualMachine ParentVirtualMachine
        {
            get { return _ParentVirtualMachine; }
        }

        public string CreateOption => (string)this.ResourceToken["createOption"];
        public string Caching => (string)this.ResourceToken["caching"];
        public int DiskSizeGb
        {
            get
            {
                try
                {
                    Int32 diskSizeGb = 0;
                    Int32.TryParse((string)this.ResourceToken["diskSizeGB"], out diskSizeGb);

                    return diskSizeGb;
                }
                catch (System.NullReferenceException)
                {
                    return 0;
                }
            }
        }

        public int Lun => Convert.ToInt32((string)ResourceToken["lun"]);

        public string MediaLink
        {
            get
            {
                if (this.ResourceToken["vhd"] == null)
                    return String.Empty;
                if (this.ResourceToken["vhd"]["uri"] == null)
                    return String.Empty;

                return (string)this.ResourceToken["vhd"]["uri"];
            }
        } 

        public string StorageAccountName
        {
            get
            {
                if (this.MediaLink == String.Empty)
                    return String.Empty;

                return MediaLink.Split(new char[] { '/' })[2].Split(new char[] { '.' })[0];
            }
        }

        public string StorageAccountContainer
        {
            get
            {
                if (this.MediaLink == String.Empty)
                    return String.Empty;

                return MediaLink.Split(new char[] { '/' })[3];
            }
        }

        public string StorageAccountBlob
        {
            get
            {
                if (this.MediaLink == String.Empty)
                    return String.Empty;

                return MediaLink.Split(new char[] { '/' })[4];
            }
        }

        public StorageAccountType StorageAccountType
        {
            get
            {
                if (_SourceStorageAccount != null)
                    return _SourceStorageAccount.StorageAccountType;
                else
                    return StorageAccountType.Premium_LRS;
            }
        }

        public StorageAccount SourceStorageAccount
        {
            get { return _SourceStorageAccount; }
        }

        public String StorageKey
        {
            get
            {
                if (this.SourceStorageAccount == null || this.SourceStorageAccount.Keys[0] == null)
                    return String.Empty;

                return this.SourceStorageAccount.Keys[0].Value;
            }
        }

        public bool IsEncrypted
        {
            get
            {
                if (this.ResourceToken["encryptionSettings"] == null)
                    return false;

                if (this.ResourceToken["encryptionSettings"]["enabled"] == null)
                    return false;

                return Convert.ToBoolean((string)this.ResourceToken["encryptionSettings"]["enabled"]);
            }
        }

        public string DiskEncryptionKeySourceVaultId
        {
            get
            {
                if (this.ResourceToken["encryptionSettings"] == null)
                    return null;

                if (this.ResourceToken["encryptionSettings"]["diskEncryptionKey"] == null)
                    return null;

                if (this.ResourceToken["encryptionSettings"]["diskEncryptionKey"]["sourceVault"] == null)
                    return null;

                if (this.ResourceToken["encryptionSettings"]["diskEncryptionKey"]["sourceVault"]["id"] == null)
                    return null;

                return (string)this.ResourceToken["encryptionSettings"]["diskEncryptionKey"]["sourceVault"]["id"];
            }
        }
        public string DiskEncryptionKeySecretUrl
        {
            get
            {
                if (this.ResourceToken["encryptionSettings"] == null)
                    return null;

                if (this.ResourceToken["encryptionSettings"]["diskEncryptionKey"] == null)
                    return null;

                if (this.ResourceToken["encryptionSettings"]["diskEncryptionKey"]["secretUrl"] == null)
                    return null;

                return (string)this.ResourceToken["encryptionSettings"]["diskEncryptionKey"]["secretUrl"];
            }
        }

        public bool IsEncryptedWithKeyEncryptionKey
        {
            get
            {
                return this.KeyEncryptionKeyKeyUrl != null;
            }
        }

        public string KeyEncryptionKeySourceVaultId
        {
            get
            {
                if (this.ResourceToken["encryptionSettings"] == null)
                    return null;

                if (this.ResourceToken["encryptionSettings"]["keyEncryptionKey"] == null)
                    return null;

                if (this.ResourceToken["encryptionSettings"]["keyEncryptionKey"]["sourceVault"] == null)
                    return null;

                if (this.ResourceToken["encryptionSettings"]["keyEncryptionKey"]["sourceVault"]["id"] == null)
                    return null;

                return (string)this.ResourceToken["encryptionSettings"]["keyEncryptionKey"]["sourceVault"]["id"];
            }
        }
        public string KeyEncryptionKeyKeyUrl
        {
            get
            {
                if (this.ResourceToken["encryptionSettings"] == null)
                    return null;

                if (this.ResourceToken["encryptionSettings"]["keyEncryptionKey"] == null)
                    return null;

                if (this.ResourceToken["encryptionSettings"]["keyEncryptionKey"]["keyUrl"] == null)
                    return null;

                return (string)this.ResourceToken["encryptionSettings"]["keyEncryptionKey"]["keyUrl"];
            }
        }

        public ManagedDisk SourceManagedDisk { get; internal set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
