namespace MigAz.Azure.Arm
{
    public class SecurityRule_Properties
    {
        public string description;
        public string protocol;
        public string sourcePortRange;
        public string destinationPortRange;
        public string sourceAddressPrefix;
        public string destinationAddressPrefix;
        public string access;
        public long priority;
        public string direction;
    }
}
