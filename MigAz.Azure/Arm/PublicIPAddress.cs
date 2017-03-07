using System;

namespace MigAz.Azure.Arm
{
    public class PublicIPAddress : ArmResource
    {
        public PublicIPAddress(Guid executionGuid) : base(executionGuid)
        {
            type = "Microsoft.Network/publicIPAddresses";
            apiVersion = "2015-06-15";
        }
    }
}
