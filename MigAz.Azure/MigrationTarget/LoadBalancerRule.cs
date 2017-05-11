using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.MigrationTarget
{
    public class LoadBalancerRule : IMigrationTarget
    {


        private string _TargetName = String.Empty;
        
        public string LoadBalancedEndpointSetName
        { get; set; }

        public String SourceName
        {
            get
            {
                return String.Empty;
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
