using MIGAZ.Azure;
using MIGAZ.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MIGAZ.Asm
{
    public class AsmDisk
    {
        private XmlNode _DataDiskNode;
        private AzureContext _AzureContext;
        private AsmStorageAccount _SourceStorageAccount;
        private IStorageAccount _TargetStorageAccount;
        private String _TargetName = String.Empty;

        public AsmDisk(AzureContext azureContext, XmlNode dataDiskNode)
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

                if (this.TargetStorageAccount.GetType() == typeof(AsmStorageAccount))
                {
                    AsmStorageAccount targetStorageAccount = (AsmStorageAccount)this.TargetStorageAccount;
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

        public AsmStorageAccount SourceStorageAccount
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
