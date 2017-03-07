using System;

namespace MigAz.Azure.Arm
{
    public class RouteTable : ArmResource
    {
        public RouteTable(Guid executionGuid) : base(executionGuid)
        {
            type = "Microsoft.Network/routeTables";
            apiVersion = "2015-06-15";
        }
    }
}
