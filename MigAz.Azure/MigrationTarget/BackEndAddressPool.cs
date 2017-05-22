using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.MigrationTarget
{
    public class BackEndAddressPool
    {
        private String _Name = "default";
        private LoadBalancer _ParentLoadBalancer = null;

        private BackEndAddressPool() { }

        public BackEndAddressPool(LoadBalancer loadBalancer)
        {
            _ParentLoadBalancer = loadBalancer;
            loadBalancer.BackEndAddressPools.Add(this);
        }

        public LoadBalancer LoadBalancer
        {
            get { return _ParentLoadBalancer; }
        }

        public String Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
    }
}
