using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace MigAz.Azure.Arm
{
    public class AvailabilitySet
    {
        private JToken _AvailabilitySet;
        private List<VirtualMachine> _VirtualMachines = new List<VirtualMachine>();

        public AvailabilitySet(JToken availabilitySet)
        {
            this._AvailabilitySet = availabilitySet;
        }

        public string Id => (string)_AvailabilitySet["id"];
        public string Name => (string)_AvailabilitySet["name"];
        public string Location => (string)_AvailabilitySet["location"];
        public Int32 PlatformUpdateDomainCount => (Int32)_AvailabilitySet["properties"]["platformUpdateDomainCount"];
        public Int32 PlatformFaultDomainCount => (Int32)_AvailabilitySet["properties"]["platformFaultDomainCount"];
        public string SkuName => (string)_AvailabilitySet["sku"]["name"];

        public List<VirtualMachine> VirtualMachines
        {
            get { return _VirtualMachines; }
        }


    }
}
