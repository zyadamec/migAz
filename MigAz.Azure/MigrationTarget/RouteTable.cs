using MigAz.Core;
using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.MigrationTarget
{
    public class RouteTable : Core.MigrationTarget
    {
        private AzureContext _AzureContext;
        private IRouteTable _SourceRouteTable;
        private String _TargetName = String.Empty;
        private string _TargetNameResult = String.Empty;
        private List<Route> _Routes = new List<Route>();

        private RouteTable() { }

         public RouteTable(IRouteTable source, TargetSettings targetSettings)
        {
            _SourceRouteTable = source;
            this.SetTargetName(source.Name, targetSettings);
        }

        public RouteTable(Arm.RouteTable source, TargetSettings targetSettings)
        {
            _SourceRouteTable = source;
            this.SetTargetName(source.Name, targetSettings);

            foreach (Arm.Route route in source.Routes)
            {
                _Routes.Add(new Route(route, targetSettings));
            }
        }

        public IRouteTable Source
        {
            get { return _SourceRouteTable; }
        }

        public String SourceName
        {
            get
            {
                if (_SourceRouteTable == null)
                    return String.Empty;

                return _SourceRouteTable.Name;
            }
        }

        public List<Route> Routes
        {
            get { return _Routes; }
        }

        public string TargetName
        {
            get { return _TargetName; }
        }

        public string TargetNameResult
        {
            get { return _TargetNameResult; }
        }

        public override void SetTargetName(string targetName, TargetSettings targetSettings)
        {
            _TargetName = targetName.Trim().Replace(" ", String.Empty);
            _TargetNameResult = _TargetName + targetSettings.AvailabilitySetSuffix;
        }

        public override string ToString()
        {
            return this.TargetNameResult;
        }
    }
}
