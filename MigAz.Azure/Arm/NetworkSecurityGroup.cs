using System;

namespace MigAz.Azure.Arm
{
    public class NetworkSecurityGroup : ArmResource
    {
        public NetworkSecurityGroup(Guid executionGuid) : base(executionGuid)
        {
            type = "Microsoft.Network/networkSecurityGroups";
            apiVersion = "2015-06-15";
        }
    }
}
