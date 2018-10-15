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
    
    public class VirtualNetworkGateway : Core.MigrationTarget, IMigrationVirtualNetworkGateway
    {
        private IVirtualNetworkGateway _SourceVirtualNetworkGateway;
        private List<Subnet> _TargetSubnets = new List<Subnet>();
        private VirtualNetworkGatewayType _GatewayType = VirtualNetworkGatewayType.Vpn;
        private bool _EnableBgp = false;
        private bool _ActiveActive = false;
        private VirtualNetworkGatewaySkuType _SkuName = VirtualNetworkGatewaySkuType.Basic;
        private VirtualNetworkGatewaySkuType _SkuTier = VirtualNetworkGatewaySkuType.Basic;
        private int _SkuCapacity = 2;
        private VirtualNetworkGatewayVpnType _VirtualNetworkGatewayVpnType = VirtualNetworkGatewayVpnType.RouteBased;

        #region Constructors

        private VirtualNetworkGateway() : base(ArmConst.MicrosoftNetwork, ArmConst.LoadBalancers) { }

        public VirtualNetworkGateway(IVirtualNetworkGateway virtualNetworkGateway, TargetSettings targetSettings) : base(ArmConst.MicrosoftNetwork, ArmConst.LoadBalancers)
        {
            this._SourceVirtualNetworkGateway = virtualNetworkGateway;
            this.SetTargetName(this.SourceName, targetSettings);
        }

        public VirtualNetworkGateway(Arm.VirtualNetworkGateway virtualNetworkGateway, TargetSettings targetSettings) : base(ArmConst.MicrosoftNetwork, ArmConst.LoadBalancers)
        {
            this._SourceVirtualNetworkGateway = virtualNetworkGateway;
            this.SetTargetName(this.SourceName, targetSettings);
            this.EnableBgp = virtualNetworkGateway.EnableBgp;
            this.ActiveActive = virtualNetworkGateway.ActiveActive;

            switch (virtualNetworkGateway.GatewayType)
            {
                case "w":
                    this.GatewayType = VirtualNetworkGatewayType.Vpn;
                    break;
                case "f":
                default:
                    this.GatewayType = VirtualNetworkGatewayType.Vpn;
                    break;
            }

            switch (virtualNetworkGateway.VpnType)
            {
                case "w":
                    this.VpnType = VirtualNetworkGatewayVpnType.PolicyBased;
                    break;
                case "f":
                default:
                    this.VpnType = VirtualNetworkGatewayVpnType.RouteBased;
                    break;
            }

            switch (virtualNetworkGateway.SkuName)
            {
                case "1":
                    this.SkuName = VirtualNetworkGatewaySkuType.VpnGw1;
                    break;
                case "2":
                    this.SkuName = VirtualNetworkGatewaySkuType.VpnGw2;
                    break;
                case "3":
                    this.SkuName = VirtualNetworkGatewaySkuType.VpnGw3;
                    break;
                case "f":
                default:
                    this.SkuName = VirtualNetworkGatewaySkuType.Basic;
                    break;
            }

            switch (virtualNetworkGateway.SkuName)
            {
                case "1":
                    this.SkuTier = VirtualNetworkGatewaySkuType.VpnGw1;
                    break;
                case "2":
                    this.SkuTier = VirtualNetworkGatewaySkuType.VpnGw2;
                    break;
                case "3":
                    this.SkuTier = VirtualNetworkGatewaySkuType.VpnGw3;
                    break;
                case "f":
                default:
                    this.SkuTier = VirtualNetworkGatewaySkuType.Basic;
                    break;
            }

            this.SkuCapacity = virtualNetworkGateway.SkuCapacity;

            foreach (IpConfiguration gatewayIpConfiguration in virtualNetworkGateway.IpConfigurations)
            {

            }
        }

        #endregion

        public IVirtualNetworkGateway SourceVirtualNetworkGateway { get { return _SourceVirtualNetworkGateway; } }

        public String SourceName
        {
            get
            {
                if (this.SourceVirtualNetworkGateway == null)
                    return String.Empty;
                else
                    return this.SourceVirtualNetworkGateway.ToString();
            }
        }

        public VirtualNetworkGatewaySkuType SkuName
        {
            get { return _SkuName; }
            set { _SkuName = value; }
        }
        public VirtualNetworkGatewaySkuType SkuTier
        {
            get { return _SkuTier; }
            set { _SkuTier = value; }
        }
        public int SkuCapacity
        {
            get { return _SkuCapacity; }
            set { _SkuCapacity = value; }
        }

        public VirtualNetworkGatewayType GatewayType
        {
            get { return _GatewayType; }
            set { _GatewayType = value; }
        }

        public bool EnableBgp
        {
            get { return _EnableBgp; }
            set { _EnableBgp = value; }
        }
        public bool ActiveActive
        {
            get { return _ActiveActive; }
            set { _ActiveActive = value; }
        }

        public VirtualNetworkGatewayVpnType VpnType
        {
            get { return _VirtualNetworkGatewayVpnType; }
            set { _VirtualNetworkGatewayVpnType = value; }
        }

        public override string ImageKey { get { return "VirtualNetworkGateway"; } }

        public override string FriendlyObjectName { get { return "Virtual Network Gateway"; } }


        public override void SetTargetName(string targetName, TargetSettings targetSettings)
        {
            this.TargetName = targetName.Trim().Replace(" ", String.Empty);
            this.TargetNameResult = this.TargetName;
        }

    }
}

