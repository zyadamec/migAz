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

        public String Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
    }
}
