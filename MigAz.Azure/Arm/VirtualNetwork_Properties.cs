using System.Collections.Generic;

namespace MigAz.Azure.Arm
{
    public class VirtualNetwork_Properties
    {
        public AddressSpace addressSpace;
        public List<Subnet> subnets;
        public VirtualNetwork_dhcpOptions dhcpOptions;
    }
}
