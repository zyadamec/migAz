// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using MigAz.Azure.Interface;
using MigAz.Core;
using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.MigrationTarget
{
    public class ResourceGroup : Core.MigrationTarget
    {
        private Arm.Location _TargetLocation;

        public ResourceGroup(TargetSettings targetSettings)
        {
            this.SetTargetName("NewResourceGroup", targetSettings);
        }

        public String SourceName
        {
            get { return String.Empty; }
        }


        public Arm.Location TargetLocation
        {
            get { return _TargetLocation; }
            set { _TargetLocation = value; }
        }

        public override string ImageKey { get { return "ResourceGroup"; } }

        public override string FriendlyObjectName { get { return "Resource Group"; } }


        public override void SetTargetName(string targetName, TargetSettings targetSettings)
        {
            this.TargetName = targetName.Trim().Replace(" ", String.Empty);
            this.TargetNameResult = this.TargetName + targetSettings.AvailabilitySetSuffix;
        }

    }
}

