using System;

namespace MigAz.Azure.Arm
{
    public class GatewayConnection : ArmResource
    {
        public GatewayConnection(Guid executionGuid) : base(executionGuid)
        {
            type = "Microsoft.Network/connections";
            apiVersion = "2015-06-15";
        }
    }
}
