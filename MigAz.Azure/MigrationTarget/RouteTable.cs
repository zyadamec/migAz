// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using MigAz.Core;
using MigAz.Core.ArmTemplate;
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
        private IRouteTable _SourceRouteTable;
        private List<Route> _Routes = new List<Route>();

        #region Constructors

        private RouteTable() : base(ArmConst.MicrosoftNetwork, ArmConst.RouteTables) { }

         public RouteTable(IRouteTable source, TargetSettings targetSettings) : base(ArmConst.MicrosoftNetwork, ArmConst.RouteTables)
        {
            _SourceRouteTable = source;
            this.SetTargetName(source.Name, targetSettings);
        }

        public RouteTable(Arm.RouteTable source, TargetSettings targetSettings) : base(ArmConst.MicrosoftNetwork, ArmConst.RouteTables)
        {
            _SourceRouteTable = source;
            this.SetTargetName(source.Name, targetSettings);

            foreach (Arm.Route route in source.Routes)
            {
                _Routes.Add(new Route(route, targetSettings));
            }
        }

        #endregion

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

        public override string ImageKey { get { return "RouteTable"; } }

        public override string FriendlyObjectName { get { return "Route Table"; } }



        public override void SetTargetName(string targetName, TargetSettings targetSettings)
        {
            this.TargetName = targetName.Trim().Replace(" ", String.Empty);
            this.TargetNameResult = this.TargetName + targetSettings.AvailabilitySetSuffix;
        }
    }
}

