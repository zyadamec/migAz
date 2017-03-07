using System.Collections;
using System.Collections.Generic;

namespace MigAz.Azure.Arm
{
    public class LoadBalancer_Properties
    {
        public List<FrontendIPConfiguration> frontendIPConfigurations;
        public List<Hashtable> backendAddressPools;
        public List<InboundNatRule> inboundNatRules;
        public List<LoadBalancingRule> loadBalancingRules;
        public List<Probe> probes;
    }
}
