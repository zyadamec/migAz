using MigAz.Core.Interface;
using Newtonsoft.Json.Linq;

namespace MigAz.Azure.Arm
{
    public class Subnet : ISubnet
    {
        private JToken _Subnet;
        private VirtualNetwork _Parent;

        private Subnet() { }

        public Subnet(VirtualNetwork parent, JToken subnet)
        {
            _Parent = parent;
            _Subnet = subnet;
        }

        public string Name
        {
            get { return (string)_Subnet["name"]; }
        }

        public string Id
        {
            get { return (string)_Subnet["id"]; }
        }

        public string TargetId
        {
            get { return this.Id; }
        }

        public string AddressPrefix
        {
            get { return (string)_Subnet["properties"]["addressPrefix"]; }
        }

        public RouteTable RouteTable
        {
            get { return null; } // todo
        }

        public VirtualNetwork Parent
        {
            get { return _Parent; }
        }

        public NetworkSecurityGroup NetworkSecurityGroup
        {
            get
            {
                return null; // TODO
            }
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
