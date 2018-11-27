using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.MigrationTarget
{
    public enum VirtualNetworkGatewayType
    {
        ExpressRoute,
        Vpn
    }

    public enum VirtualNetworkGatewayVpnType
    {
        PolicyBased, // Dynamic Gateway
        RouteBased // Static Gateway
    }

    // https://docs.microsoft.com/en-us/azure/vpn-gateway/vpn-gateway-about-vpngateways#gwsku
    public enum VirtualNetworkGatewaySkuType
    {
        Basic,
        VpnGw1,
        VpnGw2,
        VpnGw3
    }

}
