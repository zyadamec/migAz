using MigAz.Core.Interface;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Arm
{
    public class LoadBalancer : ArmResource, ILoadBalancer
    {
        private List<FrontEndIpConfiguration> _FrontEndIpConfigurations = new List<FrontEndIpConfiguration>();
        private List<BackEndAddressPool> _BackEndAddressPool = new List<BackEndAddressPool>();
        private List<LoadBalancingRule> _LoadBalancingRules = new List<LoadBalancingRule>();
        private List<Probe> _Probes = new List<Probe>();

        public LoadBalancer(JToken resourceToken) : base(resourceToken)
        {
            foreach (JToken frontEndIpConfigurationToken in ResourceToken["properties"]["frontendIPConfigurations"])
            {
                FrontEndIpConfiguration frontEndIpConfiguration = new FrontEndIpConfiguration(this, frontEndIpConfigurationToken);
                _FrontEndIpConfigurations.Add(frontEndIpConfiguration);
            }

            foreach (JToken backendAddressPoolToken in ResourceToken["properties"]["backendAddressPools"])
            {
                BackEndAddressPool backEndAddressPool = new BackEndAddressPool(this, backendAddressPoolToken);
                _BackEndAddressPool.Add(backEndAddressPool);
            }

            foreach (JToken loadBalancingRuleToken in ResourceToken["properties"]["loadBalancingRules"])
            {
                LoadBalancingRule loadBalancingRule = new LoadBalancingRule(this, loadBalancingRuleToken);
                _LoadBalancingRules.Add(loadBalancingRule);
            }

            foreach (JToken probeToken in ResourceToken["properties"]["probes"])
            {
                Probe probe = new Probe(this, probeToken);
                _Probes.Add(probe);
            }
        }

        public List<FrontEndIpConfiguration> FrontEndIpConfigurations
        {
            get { return _FrontEndIpConfigurations; }
        }

        public List<BackEndAddressPool> BackEndAddressPools
        {
            get { return _BackEndAddressPool; }
        }

        public List<LoadBalancingRule> LoadBalancingRules
        {
            get { return _LoadBalancingRules; }
        }

        public List<Probe> Probes
        {
            get { return _Probes; }
        }
    }
}
