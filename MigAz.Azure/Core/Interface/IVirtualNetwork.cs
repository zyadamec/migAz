// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace MigAz.Azure.Core.Interface
{
    public interface IVirtualNetwork
    {
        string Id { get; }
        List<ISubnet> Subnets { get; }
        ISubnet GatewaySubnet { get; }
        List<string> AddressPrefixes { get; }
        List<string> DnsServers { get; }
        string Name { get; }
    }
}

