using System.Collections.Generic;

namespace MigAz.Azure.Arm
{
    public class IpConfiguration_Properties
    {
        public string privateIPAllocationMethod = "Dynamic";
        public string privateIPAddress;
        public Reference publicIPAddress;
        public Reference subnet;
        public List<Reference> loadBalancerBackendAddressPools;
        public List<Reference> loadBalancerInboundNatRules;
    }
}
