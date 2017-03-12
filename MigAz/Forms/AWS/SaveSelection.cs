using System;
using System.Collections.Generic;

namespace MigAz.Forms.AWS
{
    public class SaveSelection
    {
        public string AWSRegion;
        public List<string> VirtualNetworks;
        public List<SaveSelectioVirtualMachine> VirtualMachines;
    }

    public class SaveSelectioVirtualMachine
    {
        public string InstanceId;
        public string InstanceName;
    }
}
