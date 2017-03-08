using MigAz.Azure.Interface;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace MigAz.Azure.Arm
{
    public class ArmStorageAccount : Core.ArmTemplate.StorageAccount, IStorageAccount
    {
        private AzureContext _AzureContext;
        private JToken _StorageAccount;
        private List<ArmStorageAccountKey> _StorageAccountKeys = new List<ArmStorageAccountKey>();

        private ArmStorageAccount() : base(Guid.Empty) { }

        public ArmStorageAccount(
            AzureContext azureContext,
            JToken storageAccount) : base(Guid.Empty)
        {
            _AzureContext = azureContext;
            _StorageAccount = storageAccount;
        }

        public List<ArmStorageAccountKey> Keys
        {
            get { return _StorageAccountKeys; }
        }

        public string Name
        {
            get { return (string)_StorageAccount["name"]; }
        }

        public string Id
        {
            get { return (string)_StorageAccount["id"]; }
        }

        public string Location
        {
            get { return (string)_StorageAccount["location"]; }
        }

        public string Kind
        {
            get { return (string)_StorageAccount["kind"]; }
        }

        public string CreationTime
        {
            get { return (string)_StorageAccount["properties"]["creationTime"]; }
        }

        public string PrimaryLocation
        {
            get { return (string)_StorageAccount["properties"]["primaryLocation"]; }
        }

        public string PrimaryEndpointBlob
        {
            get { return (string)_StorageAccount["properties"]["primaryEndpoints"]["blob"]; }
        }

        public string PrimaryEndpointFile
        {
            get { return (string)_StorageAccount["properties"]["primaryEndpoints"]["file"]; }
        }

        public string PrimaryEndpointQueue
        {
            get { return (string)_StorageAccount["properties"]["primaryEndpoints"]["queue"]; }
        }

        public string PrimaryEndpointTable
        {
            get { return (string)_StorageAccount["properties"]["primaryEndpoints"]["table"]; }
        }

        public string PrimaryStatus
        {
            get { return (string)_StorageAccount["properties"]["statusOfPrimary"]; }
        }

        public string SecondaryLocation
        {
            get { return (string)_StorageAccount["properties"]["secondaryLocation"]; }
        }

        public string SecondaryEndpointBlob
        {
            get { return (string)_StorageAccount["properties"]["secondaryEndpoints"]["blob"]; }
        }

        public string SecondaryEndpointQueue
        {
            get { return (string)_StorageAccount["properties"]["secondaryEndpoints"]["queue"]; }
        }

        public string SecondaryEndpointTable
        {
            get { return (string)_StorageAccount["properties"]["secondaryEndpoints"]["table"]; }
        }

        public string SecondaryStatus
        {
            get { return (string)_StorageAccount["properties"]["statusOfSecondary"]; }
        }

        public string SkuName
        {
            get { return (string)_StorageAccount["sku"]["name"]; }
        }

        public string SkuTier
        {
            get { return (string)_StorageAccount["sku"]["tier"]; }
        }

        public object ResourceGroup
        {
            get { return this.Id.Split('/')[4]; }
        }

        public string BlobStorageNamespace
        {
            get
            {
                return AzureServiceUrls.GetBlobEndpointUrl(_AzureContext.AzureEnvironment);
            }
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
