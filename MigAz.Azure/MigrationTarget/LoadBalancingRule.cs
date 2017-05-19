using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.MigrationTarget
{
    public class LoadBalancingRule : InboundNatRule
    {




        public override string ToString()
        {
            return this.Name;
        }
    }
}
