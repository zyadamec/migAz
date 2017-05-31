using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Arm
{
    public class Probe
    {
        private JToken _ProbeToken;
        private LoadBalancer _LoadBalancer;

        private Probe() { }

        public Probe(LoadBalancer loadBalancer, JToken probeToken)
        {
            _ProbeToken = probeToken;
            _LoadBalancer = loadBalancer;
        }

        public string Name
        {
            get { return (string)_ProbeToken["name"]; }
        }


        public String Protocol
        {
            get { return (string)_ProbeToken["properties"]["protocol"]; }
        }

        public Int32 IntervalInSeconds
        {
            get { return Convert.ToInt32((string)_ProbeToken["properties"]["intervalInSeconds"]); }
        }
        public Int32 NumberOfProbes
        {
            get { return Convert.ToInt32((string)_ProbeToken["properties"]["numberOfProbes"]); }
        }
        public Int32 Port
        {
            get { return Convert.ToInt32((string)_ProbeToken["properties"]["port"]); }
        }

        public String RequestPath
        {
            get { return (string)_ProbeToken["properties"]["requestPath"]; }
        }

    }
}
