// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace MigAz.Azure.Core.Interface
{
    public interface ISettingsProvider
    {
        bool AllowTelemetry { get; set; }
        bool BuildEmpty { get; set; }
        string StorageAccountSuffix { get; set; }

        string AvailabilitySetSuffix { get; set; }
        string NetworkInterfaceCardSuffix { get; set; }
        string VirtualNetworkSuffix { get; set; }
        string ResourceGroupSuffix { get; set; }
        string VirtualNetworkGatewaySuffix { get; set; }

        string PublicIPSuffix { get; set; }

        string NetworkSecurityGroupSuffix { get; set; }

        string LoadBalancerSuffix { get; set; }

        string VirtualMachineSuffix { get; set; }
    }
}

