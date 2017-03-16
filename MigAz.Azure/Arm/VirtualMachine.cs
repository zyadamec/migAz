using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Arm
{
    public class VirtualMachine : Core.ArmTemplate.VirtualMachine // ,  TODO IVirtualMachine
    {
        private JToken _VirtualMachine;

        private VirtualMachine() : base(Guid.Empty) { }

        public VirtualMachine(JToken virtualMachine) : base(Guid.Empty)
        {
            _VirtualMachine = virtualMachine;
        }

        public string Name => (string)_VirtualMachine["name"];
        public string Location => (string)_VirtualMachine["location"];

        public ResourceGroup ResourceGroup
        {
            get { return null; } // TODO
        }

        public IEnumerable<NetworkInterfaceCard> NetworkInterfaces { get; internal set; }
    }
}
