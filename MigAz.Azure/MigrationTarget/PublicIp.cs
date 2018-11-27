// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using MigAz.Azure.Core;
using MigAz.Azure.Core.ArmTemplate;
using MigAz.Azure.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.MigrationTarget
{
    public class PublicIp : Core.MigrationTarget, IMigrationPublicIp
    {
        private String _SourceName = String.Empty;
        private String _DomainNameLabel = String.Empty;
        private IPAllocationMethodEnum _IPAllocationMethod = IPAllocationMethodEnum.Dynamic;
        private Arm.PublicIP _Source;

        #region Constructors

        public PublicIp() : base(ArmConst.MicrosoftNetwork, ArmConst.PublicIPAddresses) { }

        public PublicIp(string targetName, TargetSettings targetSettings) : base(ArmConst.MicrosoftNetwork, ArmConst.PublicIPAddresses)
        {
            this.SetTargetName(targetName, targetSettings);
        }

        public PublicIp(Arm.PublicIP armPublicIP, TargetSettings targetSettings) : base(ArmConst.MicrosoftNetwork, ArmConst.PublicIPAddresses)
        {
            this._Source = armPublicIP;
            this.SourceName = armPublicIP.Name;
            this.SetTargetName(armPublicIP.Name, targetSettings);

            this.DomainNameLabel = armPublicIP.DomainNameLabel;

            if (String.Compare(armPublicIP.PublicIPAllocationMethod, "Static", true) == 0)
                _IPAllocationMethod = IPAllocationMethodEnum.Static;
        }

        #endregion

        public String DomainNameLabel
        {
            get { return _DomainNameLabel; }
            set { _DomainNameLabel = value.ToLower(); }
        }

        public IPAllocationMethodEnum IPAllocationMethod
        {
            get { return _IPAllocationMethod; }
            set { _IPAllocationMethod = value; }
        }

        public Arm.PublicIP Source
        {
            get { return _Source; }
        }

        public String SourceName
        {
            get { return _SourceName; }
            set { _SourceName = value; }
        }

        public override string ImageKey { get { return "PublicIp"; } }

        public override string FriendlyObjectName { get { return "Public IP"; } }



        public override void SetTargetName(string targetName, TargetSettings targetSettings)
        {
            this.TargetName = targetName.Trim().Replace(" ", String.Empty);
            this.TargetNameResult = this.TargetName;
        }
    }
}

