using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Arm
{
    public class PublicIP : ArmResource
    {
        private JToken _AvailabilitySet;

        public PublicIP(JToken resourceToken) : base(resourceToken)
        {
        }
    }
}
