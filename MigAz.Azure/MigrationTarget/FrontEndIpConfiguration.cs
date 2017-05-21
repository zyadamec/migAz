using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.MigrationTarget
{
    public class FrontEndIpConfiguration
    {
        private String _Name = "default";

        private String _PrivateIPAllocationMethod = "Dynamic";
        private String _PrivateIPAddress = String.Empty;
        private IMigrationVirtualNetwork _TargetVirtualNetwork;
        private IMigrationSubnet _TargetSubnet;

        private PublicIp _PublicIp = null;

        public String Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public String PrivateIPAllocationMethod
        {
            get { return _PrivateIPAllocationMethod; }
            set { _PrivateIPAllocationMethod = value; }
        }

        public String PrivateIPAddress
        {
            get { return _PrivateIPAddress; }
            set { _PrivateIPAddress = value; }
        }

        public IMigrationSubnet TargetSubnet
        {
            get { return _TargetSubnet; }
            set { _TargetSubnet = value; }
        }

        public IMigrationVirtualNetwork TargetVirtualNetwork
        {
            get { return _TargetVirtualNetwork; }
            set { _TargetVirtualNetwork = value; }
        }

        public PublicIp PublicIp
        {
            get { return _PublicIp; }
            set { _PublicIp = value; }
        }
    }
}
