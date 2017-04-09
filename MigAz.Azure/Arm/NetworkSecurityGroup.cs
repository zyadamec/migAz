using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Arm
{
    public class NetworkSecurityGroup  : INetworkSecurityGroup
    {
        private List<NetworkSecurityGroupRule> _Rules = new List<NetworkSecurityGroupRule>();

        public string Name => "TODO";

        public List<NetworkSecurityGroupRule> Rules => _Rules;
    }
}
