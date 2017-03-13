using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Arm
{
    public class VirtualMachine
    {

        public string Name
        {
            get { return "TODO"; }
        }

        public ResourceGroup ResourceGroup
        {
            get { return null; } // TODO
        }

        public IEnumerable<NetworkInterfaceCard> NetworkInterfaces { get; internal set; }
    }
}
