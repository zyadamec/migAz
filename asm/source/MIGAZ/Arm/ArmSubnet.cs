using MIGAZ.Interface;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIGAZ.Arm
{
    public class ArmSubnet : ISubnet
    {
        private JToken _Subnet;
        private ArmVirtualNetwork _Parent;

        private ArmSubnet() { }

        public ArmSubnet(ArmVirtualNetwork parent, JToken subnet)
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

        public ArmVirtualNetwork Parent
        {
            get { return _Parent; }
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
