using System;

namespace MigAz.Azure.Arm
{
    public class VirtualNetworkGateway : ArmResource
    {
        public VirtualNetworkGateway(Guid executionGuid) : base(executionGuid)
        {
            type = "Microsoft.Network/virtualNetworkGateways";
            apiVersion = "2015-06-15";
        }
    }
}
