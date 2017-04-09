using MigAz.Core.Interface;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Arm
{
    public class ManagedDisk : Core.ArmTemplate.ManagedDisk
    {
        private JToken _ManagedDisk;

        private ManagedDisk() : base(Guid.Empty) { }

        public ManagedDisk(JToken managedDisk) : base(Guid.Empty)
        {
            _ManagedDisk = managedDisk;
        }
    }
}
