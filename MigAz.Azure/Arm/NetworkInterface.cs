using System;

namespace MigAz.Azure.Arm
{
    public class NetworkInterface : ArmResource
    {
        public NetworkInterface(Guid executionGuid) : base(executionGuid)
        {
            type = "Microsoft.Network/networkInterfaces";
            apiVersion = "2015-06-15";
        }
    }
}
