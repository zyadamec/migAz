using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using MigAz.Core.Interface;

namespace MigAz.Azure.Arm
{
    public class Disk
    {
        private JToken jToken;
        private StorageAccount _SourceStorageAccount;
        private IStorageTarget _TargetStorageAccount;

        public Disk(JToken jToken)
        {
            this.jToken = jToken;
        }

        public string Name => (string)this.jToken["name"];
        public string CreateOption => (string)this.jToken["createOption"];
        public string Caching => (string)this.jToken["caching"];
        public int DiskSizeGb => Convert.ToInt32((string)this.jToken["diskSizeGB"]);

        public int Lun => Convert.ToInt32((string)jToken["lun"]);

        public string VhdUri
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

        //public string StorageAccountName => VhdUri == String.Empty ? String.Empty : VhdUri.Substring(VhdUri.IndexOf("://") + 3, VhdUri.IndexOf(".") - VhdUri.IndexOf("://") - 3);

        public string StorageAccountName
        {
            get
            {
                return VhdUri.Split(new char[] { '/' })[2].Split(new char[] { '.' })[0];
            }
        }

        public string StorageAccountContainer
        {
            get
            {
                return VhdUri.Split(new char[] { '/' })[3];
            }
        }

        public string StorageAccountBlob
        {
            get
            {
                return VhdUri.Split(new char[] { '/' })[4];
            }
        }

        public StorageAccount SourceStorageAccount
        {
            get { return _SourceStorageAccount; }
        }

    }
}
