using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Models
{
    public class CloudServiceVM
    {
        public string CloudService { get; set; }
        public string VirtualMachine { get; set; }
    }

    public class VirtualNW
    {
        public string RGName { get; set; }
        public string VirtualNWName { get; set; }
    }

    public class Storage
    {
        public string RGName { get; set; }
        public string StorageName { get; set; }
    }
    public class VirtualMC
    {
        public string RGName { get; set; }
        public string VirtualMachine { get; set; }
    }
}
