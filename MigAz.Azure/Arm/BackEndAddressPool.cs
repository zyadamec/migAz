using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Arm
{
    public class BackEndAddressPool
    {
        private JToken _BackEndAddressPoolToken;
        private LoadBalancer _ParentLoadBalancer = null;

        private BackEndAddressPool() { }

        public BackEndAddressPool(LoadBalancer loadBalancer, JToken backEndAddressPoolToken)
        {
            _ParentLoadBalancer = loadBalancer;
            _BackEndAddressPoolToken = backEndAddressPoolToken;
        }

        public LoadBalancer LoadBalancer
        {
            get { return _ParentLoadBalancer; }
        }

        public string Name
        {
            get { return (string)_BackEndAddressPoolToken["name"]; }
        }

    }
}
