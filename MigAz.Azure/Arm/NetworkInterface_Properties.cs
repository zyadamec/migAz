using System.Collections.Generic;

namespace MigAz.Azure.Arm
{
    public class NetworkInterface_Properties
    {
        public List<IpConfiguration> ipConfigurations;
        public bool enableIPForwarding = false;
        public Reference NetworkSecurityGroup;
    }
}
