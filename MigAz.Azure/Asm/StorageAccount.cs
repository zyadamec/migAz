// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using MigAz.Azure.Interface;
using MigAz.Core.Interface;
using System;
using System.Threading.Tasks;
using System.Xml;

namespace MigAz.Azure.Asm
{
    public class StorageAccount : IStorageAccount
    {
        #region Variables

        private XmlNode _XmlNode = null;
        private StorageAccountKeys _AsmStorageAccountKeys;
        private AzureSubscription _AzureSubscription;

        #endregion

        #region Constructors

        private StorageAccount() { }


        public StorageAccount(AzureSubscription azureSubscription, XmlNode xmlNode)
        {
            _AzureSubscription = azureSubscription;
            _XmlNode = xmlNode;
        }

        #endregion

        #region Properties

        public AzureSubscription AzureSubscription
        {
            get { return _AzureSubscription; }
            private set { _AzureSubscription = value; }
        }


        public string AccountType
        {
            get { return _XmlNode.SelectSingleNode("//StorageServiceProperties/AccountType").InnerText; }
        }

        public string PrimaryLocation
        {
            get { return _XmlNode["StorageServiceProperties"]["GeoPrimaryRegion"].InnerText; }
        }

        public string GeoPrimaryRegion
        {
            get {  return _XmlNode.SelectSingleNode("//ExtendedProperties/ExtendedProperty[Name='ResourceLocation']/Value").InnerText; }
        }

        public string Name
        {
            get
            {
                if (_XmlNode.SelectSingleNode("ServiceName") != null)
                    return _XmlNode.SelectSingleNode("ServiceName").InnerText;

                if (_XmlNode.SelectSingleNode("//ServiceName") != null)
                    return _XmlNode.SelectSingleNode("//ServiceName").InnerText;

                throw new ArgumentException("Unable to locate Storage Account Service Name");
            }
        }

        public string Id
        {
            get { return this.Name; }
        }

        public StorageAccountKeys Keys
        {
            get { return _AsmStorageAccountKeys; }
            set { _AsmStorageAccountKeys = value; }
        }

        public string BlobStorageNamespace
        {
            get
            {
                return this.AzureSubscription.AzureEnvironment.BlobEndpointUrl;
            }
        }

        public StorageAccountType StorageAccountType
        {
            get
            {
                if (AccountType == "Premium_LRS")
                    return StorageAccountType.Premium_LRS;
                else
                    return StorageAccountType.Standard_LRS;
            }
        }

        #endregion

        #region Methods

        internal async Task LoadStorageAccountKeysAsync(AzureContext azureContext)
        {
            _AsmStorageAccountKeys = await this.AzureSubscription.GetAzureAsmStorageAccountKeys(this.Name);
        }

        public override string ToString()
        {
            return this.Name;
        }

        #endregion
    }
}

