using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.MigrationTarget
{
    public class Probe
    {
        private String _Name = String.Empty;
        private String _Protocol = "tcp";
        private String _RequestPath = String.Empty;
        private Int32 _NumberOfProbes = 2;
        private Int32 _IntervalInSeconds = 15;

        public String Name
        {
            get { return _Name; }
            set { _Name = value.Trim(); }
        }

        public Int32 Port { get; set; }

        public String Protocol
        {
            get { return _Protocol; }
            set { _Protocol = value; }
        }

        public Int32 IntervalInSeconds
        {
            get { return _IntervalInSeconds; }
            set { _IntervalInSeconds = value; }
        }
        public Int32 NumberOfProbes
        {
            get { return _NumberOfProbes; }
            set { _NumberOfProbes = value; }
        }

        public String RequestPath
        {
            get { return _RequestPath; }
            set { _RequestPath = value; }
        }
    }
}
