using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.MigrationTarget
{
    public class LoadBalancingRule
    {
        private Probe _Probe = null;
        private BackEndAddressPool _BackEndAddressPool = null;
        private String _Name = String.Empty;
        private Int32 _FrontEndPort = 0;
        private Int32 _BackEndPort = 0;
        private String _Protocol = "tcp";
        private LoadBalancer _ParentLoadBalancer = null;

        private LoadBalancingRule() { }

        public LoadBalancingRule(LoadBalancer loadBalancer)
        {
            _ParentLoadBalancer = loadBalancer;
            loadBalancer.LoadBalancingRules.Add(this);
        }

        public LoadBalancer LoadBalancer
        {
            get { return _ParentLoadBalancer; }
        }

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

        public Int32 FrontEndPort
        {
            get { return _FrontEndPort; }
            set { _FrontEndPort = value; }
        }

        public Int32 BackEndPort
        {
            get { return _BackEndPort; }
            set { _BackEndPort = value; }
        }

        public String Protocol
        {
            get { return _Protocol; }
            set { _Protocol = value; }
        }

        public FrontEndIpConfiguration FrontEndIpConfiguration
        {
            get; set;
        }

        public String Name
        {
            get { return _Name; }
            set { _Name = value.Trim().Replace(" ", String.Empty); }
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
