using System;

namespace MigAz.Azure.Arm
{
    public class LoadBalancer : ArmResource
    {
        public LoadBalancer(Guid executionGuid) : base(executionGuid)
        {
            type = "Microsoft.Network/loadBalancers";
            apiVersion = "2015-06-15";
        }
    }
}
