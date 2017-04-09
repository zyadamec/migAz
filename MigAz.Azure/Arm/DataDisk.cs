using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Arm
{
    public class DataDisk : Disk
    {
        private JToken jToken;

        public DataDisk(JToken jToken) : base(jToken)
        {
            this.jToken = jToken;
        }

        public int Lun => Convert.ToInt32((string)jToken["lun"]);
        public int DiskSizeGb => Convert.ToInt32((string)jToken["diskSizeGB"]);

    }
}
