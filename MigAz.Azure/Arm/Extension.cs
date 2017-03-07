using System;

namespace MigAz.Azure.Arm
{
    public class Extension : ArmResource
    {
        public Extension(Guid executionGuid) : base(executionGuid)
        {
            type = "extensions";
            apiVersion = "2015-06-15";
        }
    }
}
