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
    public enum VirtualNetworkGatewayType
    {
        ExpressRoute,
        Vpn
    }

    public enum VirtualNetworkGatewayVpnType
    {
        PolicyBased, // Dynamic Gateway
        RouteBased // Static Gateway
    }

    public class VirtualNetworkGateway : Core.MigrationTarget, IMigrationVirtualNetworkGateway
    {
        private IVirtualNetworkGateway _SourceVirtualNetworkGateway;
        private List<Subnet> _TargetSubnets = new List<Subnet>();
        private VirtualNetworkGatewayType _GatewayType = VirtualNetworkGatewayType.Vpn;
        private bool _EnableBgp = false;
        private VirtualNetworkGatewayVpnType _VirtualNetworkGatewayVpnType = VirtualNetworkGatewayVpnType.RouteBased;

        #region Constructors

        private VirtualNetworkGateway() : base(ArmConst.MicrosoftNetwork, ArmConst.LoadBalancers) { }

        public VirtualNetworkGateway(IVirtualNetworkGateway virtualNetworkGateway, TargetSettings targetSettings) : base(ArmConst.MicrosoftNetwork, ArmConst.LoadBalancers)
        {
            this._SourceVirtualNetworkGateway = virtualNetworkGateway;
            this.SetTargetName(this.SourceName, targetSettings);
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

        public String SkuName
        {
            get { return "Basic"; }
        }
        public String SkuTier
        {
            get { return "Basic"; }
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

