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
        private Probe _Probe = null;
        private BackEndAddressPool _BackEndAddressPool = null;

        public Probe Probe
        {
            get { return _Probe; }
            set { _Probe = value; }
        }

        public BackEndAddressPool BackEndAddressPool
        {
            get { return _BackEndAddressPool; }
            set { _BackEndAddressPool = value; }
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
