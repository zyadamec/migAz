using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.MigrationTarget
{
    public class NetworkSecurityGroupRule : IMigrationTarget
    {
        // todo now russell, constructors from ASM/ARM
        private string _TargetName = String.Empty;

        public string Type
        {
            get; set;
        }
        public string Direction
        {
            get; set;
        }
        public string Access
        {
            get; set;
        }

        public long Priority
        {
            get; set;
        }

        public string Action
        {
            get; set;
        }

        public string SourceAddressPrefix
        {
            get; set;
        }

        public string DestinationAddressPrefix
        {
            get; set;
        }

        public string SourcePortRange
        {
            get; set;
        }

        public string DestinationPortRange
        {
            get; set;
        }

        public string Protocol
        {
            get; set;
        }

        public bool IsSystemRule
        {
            get;set;
        }

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
