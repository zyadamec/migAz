using MigAz.Azure.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MigAz.Azure.Asm
{
    public class StorageAccount : IStorageAccount
    {
        #region Variables

        private AzureContext _AzureContext;
        private XmlNode _XmlNode = null;
        private StorageAccountKeys _AsmStorageAccountKeys;
        private string _TargetName;

        #endregion

        #region Constructors

        private StorageAccount() { }


        public StorageAccount(AzureContext azureContext, XmlNode xmlNode)
        {
            _AzureContext = azureContext;
            _XmlNode = xmlNode;

            this.TargetName = this.Name;
        }

        #endregion

        #region Properties

        public string AccountType
        {
            get { return _XmlNode.SelectSingleNode("//StorageServiceProperties/AccountType").InnerText; }
        }

        public string GeoPrimaryRegion
        {
            get { return _XmlNode["StorageServiceProperties"]["GeoPrimaryRegion"].InnerText; }
        }

        public string Location
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

        public string TargetName
        {
            get { return _TargetName; }
            set { _TargetName = value; }
        }

        public string BlobStorageNamespace
        {
            get
            {
                return AzureServiceUrls.GetBlobEndpointUrl(_AzureContext.AzureEnvironment);
            }
        }

        #endregion

        #region Methods

        internal async Task LoadStorageAccountKeysAsynch()
        {
            _AsmStorageAccountKeys = await this._AzureContext.AzureRetriever.GetAzureAsmStorageAccountKeys(this.Name);
        }

        public override string ToString()
        {
            return this.GetFinalTargetName();
        }

        public string GetFinalTargetName()
        {
            if (this.TargetName.Length + this._AzureContext.SettingsProvider.StorageAccountSuffix.Length > 24)
                return this.TargetName.Substring(0, 24 - this._AzureContext.SettingsProvider.StorageAccountSuffix.Length) + this._AzureContext.SettingsProvider.StorageAccountSuffix;
            else
                return this.TargetName + this._AzureContext.SettingsProvider.StorageAccountSuffix;
        }

        #endregion
    }
}
