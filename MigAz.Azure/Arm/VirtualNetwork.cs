using System;

namespace MigAz.Azure.Arm
{
    public class VirtualNetwork : ArmResource
    {
        public VirtualNetwork(Guid executionGuid) : base(executionGuid)
        {
            type = "Microsoft.Network/virtualNetworks";
            apiVersion = "2015-06-15";
        }
    }
}
