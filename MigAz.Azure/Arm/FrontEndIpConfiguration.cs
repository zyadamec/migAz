using MigAz.Core.Interface;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Arm
{
    public class FrontEndIpConfiguration
    {
        private JToken _FrontEndIpConfigurationToken;

        private VirtualNetwork _VirtualNetwork;
        private Subnet _Subnet;

        //private PublicIp _PublicIp = null;

        private LoadBalancer _ParentLoadBalancer = null;

        private FrontEndIpConfiguration() { }

        public FrontEndIpConfiguration(LoadBalancer loadBalancer, JToken frontEndIpConfigurationToken)
        {
            _ParentLoadBalancer = loadBalancer;
            _FrontEndIpConfigurationToken = frontEndIpConfigurationToken;
        }

        public LoadBalancer LoadBalancer
        {
            get { return _ParentLoadBalancer; }
        }


        public string Name
        {
            get { return (string)_FrontEndIpConfigurationToken["name"]; }
        }

        public string PrivateIPAllocationMethod
        {
            get { return (string)_FrontEndIpConfigurationToken["properties"]["privateIPAllocationMethod"]; }
        }

        public string PrivateIPAddress
        {
            get { return (string)_FrontEndIpConfigurationToken["properties"]["privateIPAddress"]; }
        }

        public VirtualNetwork VirtualNetwork
        {
            get { return _VirtualNetwork; }
            set { _VirtualNetwork = value; }
        }

        public Subnet Subnet
        {
            get { return _Subnet; }
            set { _Subnet = value; }
        }

        //public PublicIp PublicIp
        //{
        //    get { return _PublicIp; }
        //    set { _PublicIp = value; }
        //}
    }
}
