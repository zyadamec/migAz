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
