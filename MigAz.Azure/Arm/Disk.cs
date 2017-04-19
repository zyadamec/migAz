using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace MigAz.Azure.Arm
{
    public class Disk
    {
        private JToken jToken;
        
        public Disk(JToken jToken)
        {
            this.jToken = jToken;
        }

        public string Name => (string)this.jToken["name"];
        public string CreateOption => (string)this.jToken["createOption"];
        public string Caching => (string)this.jToken["caching"];
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

        public string StorageAccountName => VhdUri == String.Empty ? String.Empty : VhdUri.Substring(VhdUri.IndexOf("://") + 3, VhdUri.IndexOf(".") - VhdUri.IndexOf("://") - 3);

        public StorageAccount TargetStorageAccount { get; set; }
    }
}
