using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Arm
{

    public class NetworkInterfaceCard
    {
        private JToken _NetworkInterface;
        private VirtualNetwork _VirtualNetwork;
        private Subnet _Subnet;

        private NetworkInterfaceCard() { }

        public NetworkInterfaceCard(JToken networkInterfaceToken)
        {
            _NetworkInterface = networkInterfaceToken;
        }

        public string Id => (string)_NetworkInterface["id"];
        public bool IsPrimary => Convert.ToBoolean((string)_NetworkInterface["properties"]["primary"]);
        public VirtualNetwork VirtualNetwork => _VirtualNetwork;
        public Subnet Subnet => _Subnet;
    }
}
