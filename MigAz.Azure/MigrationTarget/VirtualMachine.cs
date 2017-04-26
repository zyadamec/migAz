using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.MigrationTarget
{
    public class VirtualMachine : IMigrationTarget
    {
        public string TargetName { get; set; }


    }
}
