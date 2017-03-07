namespace MigAz.Azure.Arm
{
    public class InboundNatRule_Properties
    {
        public long frontendPort;
        public long backendPort;
        public string protocol;
        public Reference frontendIPConfiguration;
    }
}
