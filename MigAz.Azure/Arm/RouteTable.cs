using MigAz.Core.Interface;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Arm
{
    public class RouteTable : ArmResource, IRouteTable
    {
        private RouteTable() : base(null) { }

        public RouteTable(JToken resourceToken) : base(resourceToken)
        {
        }

        private List<Route> _Routes = new List<Route>();

        public List<Route> Routes => _Routes; // todo  routenode in resource.properties.routes)

        public override string ToString()
        {
            return this.Name;
        }
    }
}
