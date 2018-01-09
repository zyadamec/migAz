using MigAz.Core.Interface;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Arm
{
    public class Route : ArmResource, IRoute
    {
        private RouteTable _RouteTable;

        private Route() : base(null) { }

        public Route(RouteTable routeTable, JToken resourceToken) : base(resourceToken)
        {
            _RouteTable = routeTable;
        }

        public string AddressPrefix
        {
            get { return (string)ResourceToken["properties"]["addressPrefix"]; }
        }
        public NextHopTypeEnum NextHopType
        {
            get
            {
                NextHopTypeEnum nextHopType;
                Enum.TryParse<NextHopTypeEnum>((string)ResourceToken["properties"]["nextHopType"], out nextHopType);
                return nextHopType;
            }
        }
        public string NextHopIpAddress
        {
            get { return (string)ResourceToken["properties"]["nextHopIpAddress"]; }
        }
    }
}
