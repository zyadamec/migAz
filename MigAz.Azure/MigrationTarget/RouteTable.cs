using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.MigrationTarget
{
    public class RouteTable : IMigrationTarget
    {
        private AzureContext _AzureContext;
        private String _TargetName = String.Empty;
        private List<Route> _Routes = new List<Route>();

        private RouteTable() { }

        public RouteTable(AzureContext azureContext)
        {
            this._AzureContext = azureContext;
        }

        public String TargetName
        {
            get { return _TargetName; }
            set { _TargetName = value.Trim(); }
        }

        public String SourceName
        {
            get { return String.Empty; }
        }

        public override string ToString()
        {
            return this.TargetName;
        }

        public List<Route> Routes
        {
            get { return _Routes; }
        }
    }
}
