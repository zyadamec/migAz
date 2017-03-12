using MigAz.Core.Interface;
using System;
using System.Threading.Tasks;
using System.Xml;

namespace MigAz.Azure.Asm
{
    public class Disk
    {
        private XmlNode _DataDiskNode;
        private AzureContext _AzureContext;
        private StorageAccount _SourceStorageAccount;
        private IStorageAccount _TargetStorageAccount;
        private String _TargetName = String.Empty;

        public Disk(AzureContext azureContext, XmlNode dataDiskNode)
        {
            this._AzureContext = azureContext;
            this._DataDiskNode = dataDiskNode;

            this.TargetName = this.DiskName;
        }

        public async Task InitializeChildren()
        {
            _SourceStorageAccount = await _AzureContext.AzureRetriever.GetAzureAsmStorageAccount(StorageAccountName);
        }

        #region Properties

        public string SourceImageName
        {
            get { return _DataDiskNode.SelectSingleNode("SourceImageName").InnerText; }
        }

        public string MediaLink
        {
            get { return _DataDiskNode.SelectSingleNode("MediaLink").InnerText; }
        }

        public string TargetMediaLink
        {
            get
            {
                string targetMediaLink = this.MediaLink;

                if (this.TargetStorageAccount.GetType() == typeof(StorageAccount))
                {
                    StorageAccount targetStorageAccount = (StorageAccount)this.TargetStorageAccount;
                    targetMediaLink = targetMediaLink.Replace(this.SourceStorageAccount.Name + "." + this.SourceStorageAccount.BlobStorageNamespace, targetStorageAccount.GetFinalTargetName() + "." + targetStorageAccount.BlobStorageNamespace);
                }
                else
                    targetMediaLink = targetMediaLink.Replace(this.SourceStorageAccount.Name + "." + this.SourceStorageAccount.BlobStorageNamespace, this.TargetStorageAccount.Name + "." + this.TargetStorageAccount.BlobStorageNamespace);

                targetMediaLink = targetMediaLink.Replace(this.DiskName, this.TargetName);

                return targetMediaLink;
            }
        }

        public string DiskName
        {
            get { return _DataDiskNode.SelectSingleNode("DiskName").InnerText; }
        }

        public string TargetName
        {
            get { return _TargetName; }
            set { _TargetName = value; }
        }

        public Int64? Lun
        {
            get
            {
                if (_DataDiskNode.SelectSingleNode("Lun") == null)
                    return null;

                return Int64.Parse(_DataDiskNode.SelectSingleNode("Lun").InnerText);
            }
        }

        public string HostCaching
        {
            get { return _DataDiskNode.SelectSingleNode("HostCaching").InnerText; }
        }

        public Int64 DiskSizeInGB
        {
            get { return Int64.Parse(_DataDiskNode.SelectSingleNode("LogicalDiskSizeInGB").InnerText); }
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

        public IStorageAccount TargetStorageAccount
        {
            get { return _TargetStorageAccount; }
            set { _TargetStorageAccount = value; }
        }

        #endregion
    }
}
