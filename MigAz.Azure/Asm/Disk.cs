// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using MigAz.Core.Interface;
using System;
using System.Threading.Tasks;
using System.Xml;

namespace MigAz.Azure.Asm
{
    public class Disk : IDisk
    {
        private AzureSubscription _AzureSubscription;
        private XmlNode _DataDiskNode;
        private StorageAccount _SourceStorageAccount;
        private String _TargetName = String.Empty;

        public Disk(AzureSubscription azureSubscription, XmlNode dataDiskNode)
        {
            this._AzureSubscription = azureSubscription;
            this._DataDiskNode = dataDiskNode;
        }

        public async Task InitializeChildrenAsync()
        {
            _SourceStorageAccount = await this.AzureSubscription.GetAzureAsmStorageAccount(this.StorageAccountName);
        }

        #region Properties

        public AzureSubscription AzureSubscription
        {
            get { return _AzureSubscription; }
        }

        public string SourceImageName
        {
            get { return _DataDiskNode.SelectSingleNode("SourceImageName").InnerText; }
        }

        public string MediaLink
        {
            get { return _DataDiskNode.SelectSingleNode("MediaLink").InnerText; }
        }

        public string DiskName
        {
            get { return _DataDiskNode.SelectSingleNode("DiskName").InnerText; }
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

        public Int32 DiskSizeGb
        {
            get
            {
                if (_DataDiskNode.SelectSingleNode("LogicalDiskSizeInGB") == null)
                    return 0;

                return Int32.Parse(_DataDiskNode.SelectSingleNode("LogicalDiskSizeInGB").InnerText);
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

        public String StorageKey
        {
            get
            {
                if (this.SourceStorageAccount == null || this.SourceStorageAccount.Keys == null)
                    return String.Empty;

                return this.SourceStorageAccount.Keys.Primary;
            }
        }

        public bool IsEncrypted
        {
            get { return false; }
        }

        #endregion

        public override string ToString()
        {
            return this.DiskName;
        }
    }
}

