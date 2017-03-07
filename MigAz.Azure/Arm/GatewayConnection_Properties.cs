namespace MigAz.Azure.Arm
{
    public class GatewayConnection_Properties
    {
        public Reference virtualNetworkGateway1;
        public Reference localNetworkGateway2;
        public string connectionType;
        public Reference peer;
        public long routingWeight = 10;
        public string sharedKey;
    }
}
