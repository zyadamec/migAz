// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Core.ArmTemplate
{
    public class ArmConst
    {
        public const string ResourceGroupId = "resourceGroup().id";
        public const string Locations = "/locations";
        public const string GatewaySubnetName = "GatewaySubnet";

        public const string MicrosoftCompute = "Microsoft.Compute";
        public const string ProviderAvailabilitySets = "/providers/" + MicrosoftCompute + "/availabilitySets/";
        public const string ProviderVirtualMachineImages = "/providers/" + MicrosoftCompute + "/images/";
        public const string ProviderVMSizes = "/providers/" + MicrosoftCompute + "/locations/{0}/vmSizes/";
        public const string ProviderManagedDisks = "/providers/" + MicrosoftCompute + "/disks/";
        public const string ProviderVirtualMachines = "/providers/" + MicrosoftCompute + "/virtualMachines/";

        public const string MicrosoftStorage = "Microsoft.Storage";
        public const string ProviderStorage = "/providers/" + MicrosoftStorage + "/";
        public const string ProviderStorageAccounts = "/providers/" + MicrosoftStorage + "/storageAccounts/";
        public const string TypeStorageAccount = MicrosoftStorage + "/storageAccounts";

        public const string MicrosoftNetwork = "Microsoft.Network";
        public const string ProviderVirtualNetwork = "/providers/" + MicrosoftNetwork + "/virtualNetworks/";
        public const string ProviderLoadBalancers = "/providers/" + MicrosoftNetwork + "/loadBalancers/";
        public const string ProviderPublicIpAddress = "/providers/" + MicrosoftNetwork + "/publicIPAddresses/";
        public const string ProviderNetworkSecurityGroups = "/providers/" + MicrosoftNetwork + "/networkSecurityGroups/";
        public const string ProviderRouteTables = "/providers/" + MicrosoftNetwork + "/routeTables/";
        public const string ProviderLocalNetworkGateways = "/providers/" + MicrosoftNetwork + "/localNetworkGateways/";
        public const string ProviderVirtualNetworkGateways = "/providers/" + MicrosoftNetwork + "/virtualNetworkGateways/";
        public const string ProviderNetworkInterfaces = "/providers/" + MicrosoftNetwork + "/networkInterfaces/";
        public const string ProviderExpressRouteCircuits = "/providers/" + MicrosoftNetwork + "/expressRouteCircuits/";
        public const string ProviderGatewayConnection = "/providers/" + MicrosoftNetwork + "/connections/";
    }
}

