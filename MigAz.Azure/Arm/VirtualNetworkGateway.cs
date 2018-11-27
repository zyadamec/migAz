// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using MigAz.Azure.MigrationTarget;
using MigAz.Azure.Core;
using MigAz.Azure.Core.ArmTemplate;
using MigAz.Azure.Core.Interface;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Arm
{
    public class VirtualNetworkGateway : ArmResource, IVirtualNetworkGateway
    {
        private List<IpConfiguration> _IpConfigurations = new List<IpConfiguration>();

        private VirtualNetworkGateway() : base(null, null) { }

        public VirtualNetworkGateway(AzureSubscription azureSubscription, JToken resourceToken) : base(azureSubscription, resourceToken)
        {
        }

        public new async Task InitializeChildrenAsync()
        {
            await base.InitializeChildrenAsync();
        }

        #region Properties

        public string SkuName
        {
            get { return (string)ResourceToken["properties"]["sku"]["name"]; }
        }

        public string SkuTier
        {
            get { return (string)ResourceToken["properties"]["sku"]["tier"]; }
        }
        public int SkuCapacity
        {
            get
            {
                if (ResourceToken["properties"]["sku"]["capacity"] == null)
                    return 2; // Default?

                return Convert.ToInt16((string)ResourceToken["properties"]["sku"]["capacity"]);
            }
        }
        public List<IpConfiguration> IpConfigurations
        {
            get { return _IpConfigurations; }
        }

        public string GatewayType
        {
            get { return (string)ResourceToken["properties"]["gatewayType"]; }
        }
        public string VpnType
        {
            get { return (string)ResourceToken["properties"]["vpnType"]; }
        }
        public bool EnableBgp
        {
            get { return Convert.ToBoolean((string)ResourceToken["properties"]["enableBgp"]); }
        }
        public bool ActiveActive
        {
            get { return Convert.ToBoolean((string)ResourceToken["properties"]["activeActive"]); }
        }
        #endregion


        public override string ToString()
        {
            return this.Name;
        }
    }
}

