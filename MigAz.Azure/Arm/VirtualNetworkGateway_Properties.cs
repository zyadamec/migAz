using System.Collections.Generic;

namespace MigAz.Azure.Arm
{
    public class VirtualNetworkGateway_Properties
    {
        public List<IpConfiguration> ipConfigurations;
        public VirtualNetworkGateway_Sku sku;
        public string gatewayType; // VPN or ER
        public string vpnType; // RouteBased or PolicyBased
        public string enableBgp = "false";
        public VPNClientConfiguration vpnClientConfiguration;
    }
}
