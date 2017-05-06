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
        public string Name
        {
            get; set;
        }

        public string Type
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

        public override string ToString()
        {
            return this.Name;
        }
    }
}
