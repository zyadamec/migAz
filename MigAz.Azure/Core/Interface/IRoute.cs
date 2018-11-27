// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Core.Interface
{

    public enum NextHopTypeEnum
    {
        VirtualAppliance,
        VirtualNetworkGateway,
        Internet,
        VnetLocal,
        None
    }

    public interface IRoute
    {
        string Name { get; }
        NextHopTypeEnum NextHopType { get; }
        string AddressPrefix { get; }
        string NextHopIpAddress { get; }
    }
}

