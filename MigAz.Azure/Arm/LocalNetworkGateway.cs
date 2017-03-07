using System;

namespace MigAz.Azure.Arm
{
    public class LocalNetworkGateway : ArmResource
    {
        public LocalNetworkGateway(Guid executionGuid) : base(executionGuid)
        {
            type = "Microsoft.Network/localNetworkGateways";
            apiVersion = "2015-06-15";
        }
    }
}
