using System.Collections.Generic;
using System;

namespace MigAz.Azure.Arm
{
    public class VirtualMachine : ArmResource
    {
        public List<ArmResource> resources;
        public VirtualMachine(Guid executionGuid) : base(executionGuid)
        {
            type = "Microsoft.Compute/virtualMachines";
            apiVersion = "2015-06-15";
        }
    }
}
