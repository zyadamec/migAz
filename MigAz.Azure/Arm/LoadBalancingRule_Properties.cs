namespace MigAz.Azure.Arm
{
    public class LoadBalancingRule_Properties
    {
        public Reference frontendIPConfiguration;
        public Reference backendAddressPool;
        public Reference probe;
        public string protocol;
        public long frontendPort;
        public long backendPort;
        public long idleTimeoutInMinutes = 15;
        public string loadDistribution = "SourceIP";
        public bool enableFloatingIP = false;
    }
}
