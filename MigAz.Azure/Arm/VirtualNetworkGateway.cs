// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using MigAz.Core.Interface;
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
        private VirtualNetworkGateway() : base(null, null) { }

        public VirtualNetworkGateway(AzureSubscription azureSubscription, JToken resourceToken) : base(azureSubscription, resourceToken)
        {
        }

        public string gatewayType => (string)ResourceToken["properties"]["gatewayType"];
        public string vpnType => (string)ResourceToken["properties"]["vpnType"];
        public string enableBgp => (string)ResourceToken["properties"]["enableBgp"];
        public string activeActive => (string)ResourceToken["properties"]["activeActive"];
        public string skuName => (string)ResourceToken["properties"]["sku"]["name"];
        public string skuTier => (string)ResourceToken["properties"]["sku"]["tier"];
        public string skuCapacity => (string)ResourceToken["properties"]["sku"]["capacity"];

        public new async Task InitializeChildrenAsync()
        {
            await base.InitializeChildrenAsync();
        }



        public override string ToString()
        {
            return this.Name;
        }
    }
}

