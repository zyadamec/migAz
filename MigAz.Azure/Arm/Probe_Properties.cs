namespace MigAz.Azure.Arm
{
    public class Probe_Properties
    {
        public string protocol;
        public long port;
        public long intervalInSeconds = 15;
        public long numberOfProbes = 2;
        public string requestPath;
    }
}
