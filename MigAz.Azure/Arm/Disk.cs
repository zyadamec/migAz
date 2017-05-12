using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using MigAz.Core.Interface;

namespace MigAz.Azure.Arm
{
    public class Disk : IDisk
    {
        private AzureContext _AzureContext;
        private JToken jToken;
        private StorageAccount _SourceStorageAccount = null;

        public Disk(AzureContext azureContext, JToken jToken)
        {
            this._AzureContext = azureContext;
            this.jToken = jToken;
        }

        public async Task InitializeChildrenAsync()
        {
            _SourceStorageAccount = await _AzureContext.AzureRetriever.GetAzureARMStorageAccount(StorageAccountName);
        }

        public string Name => (string)this.jToken["name"];
        public string CreateOption => (string)this.jToken["createOption"];
        public string Caching => (string)this.jToken["caching"];
        public int DiskSizeGb => Convert.ToInt32((string)this.jToken["diskSizeGB"]);

        public int Lun => Convert.ToInt32((string)jToken["lun"]);

        public string MediaLink
        {
            get
            {
                if (this.jToken["vhd"] == null)
                    return String.Empty;
                if (this.jToken["vhd"]["uri"] == null)
                    return String.Empty;

                return (string)this.jToken["vhd"]["uri"];
            }
        } 

        public string StorageAccountName
        {
            get
            {
                return MediaLink.Split(new char[] { '/' })[2].Split(new char[] { '.' })[0];
            }
        }

        public string StorageAccountContainer
        {
            get
            {
                return MediaLink.Split(new char[] { '/' })[3];
            }
        }

        public string StorageAccountBlob
        {
            get
            {
                return MediaLink.Split(new char[] { '/' })[4];
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

        public override string ToString()
        {
            return this.Name;
        }
    }
}
