using MigAz.Core.Interface;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Arm
{
    public class LoadBalancingRule
    {
        private JToken _LoadBalancingRuleToken;
        private LoadBalancer _ParentLoadBalancer = null;
        //private Probe _Probe = null;
        private BackEndAddressPool _BackEndAddressPool = null;

        private LoadBalancingRule() { }

        public LoadBalancingRule(LoadBalancer loadBalancer, JToken loadBalancingRuleToken)
        {
            _ParentLoadBalancer = loadBalancer;
            _LoadBalancingRuleToken = loadBalancingRuleToken;
        }

        public LoadBalancer LoadBalancer
        {
            get { return _ParentLoadBalancer; }
        }

        //public Probe Probe
        //{
        //    get { return _Probe; }
        //}

        public BackEndAddressPool BackEndAddressPool
        {
            get { return _BackEndAddressPool; }
        }

        public bool EnableFloatingIP
        {
            get { return Convert.ToBoolean((string)_LoadBalancingRuleToken["properties"]["enableFloatingIP"]); }
        }

        public Int32 IdleTimeoutInMinutes
        {
            get { return Convert.ToInt32((string)_LoadBalancingRuleToken["properties"]["idleTimeoutInMinutes"]); }
        }

        public Int32 FrontEndPort
        {
            get { return Convert.ToInt32((string)_LoadBalancingRuleToken["properties"]["frontendPort"]); }
        }

        public Int32 BackEndPort
        {
            get { return Convert.ToInt32((string)_LoadBalancingRuleToken["properties"]["backendPort"]); }
        }

        public String Protocol
        {
            get { return (string)_LoadBalancingRuleToken["properties"]["protocol"]; }
        }

        public FrontEndIpConfiguration FrontEndIpConfiguration
        {
            get;
        }

        public string Name
        {
            get { return (string)_LoadBalancingRuleToken["name"]; }
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
