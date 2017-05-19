using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.MigrationTarget
{
    public class LoadBalancer : IMigrationTarget
    {
        private string _TargetName = String.Empty;
        private ILoadBalancer _source;
        private List<LoadBalancerRule> _Rules = new List<LoadBalancerRule>();
        private IMigrationVirtualNetwork _TargetVirtualNetwork;
        private IMigrationSubnet _TargetSubnet;


        public LoadBalancer(Arm.LoadBalancer sourceLoadBalancer)
        {
            this.Source = sourceLoadBalancer;
            this.TargetName = sourceLoadBalancer.Name;
        }

        public ILoadBalancer Source
        {
            get { return _source; }
            set { _source = value; }
        }

        public List<LoadBalancerRule> Rules
        {
            get { return _Rules; }
        }

        public String SourceName
        {
            get
            {
                if (this.Source == null)
                    return String.Empty;
                else
                    return this.Source.Name;
            }
        }

        public String StaticVirtualNetworkIPAddress
        {
            get;set;
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

        public string TargetName
        {
            get { return _TargetName; }
            set { _TargetName = value.Trim().Replace(" ", String.Empty); }
        }

        public override string ToString()
        {
            return this.TargetName;
        }
    }
}
