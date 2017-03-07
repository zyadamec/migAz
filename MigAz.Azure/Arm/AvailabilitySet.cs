using System;

namespace MigAz.Azure.Arm
{
    public class AvailabilitySet : ArmResource
    {
        public AvailabilitySet(Guid executionGuid) : base(executionGuid)
        {
            type = "Microsoft.Compute/availabilitySets";
            apiVersion = "2015-06-15";
        }
    }
}
