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
        public String SourceName
        {
            get
            {
                return String.Empty;
            }
        }
    }
}
