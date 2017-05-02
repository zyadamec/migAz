using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.MigrationTarget
{
    public class LoadBalancerRule : IMigrationTarget
    {
        public string Name
        { get; set; }

        public string LoadBalancedEndpointSetName
        { get; set; }
    }
}
