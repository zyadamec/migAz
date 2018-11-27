// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using MigAz.Azure.MigrationTarget;
using MigAz.Azure.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Interface
{
    public interface IVirtualNetworkTarget
    {
        IMigrationVirtualNetwork TargetVirtualNetwork
        {
            get;
            set;
        }
        IMigrationSubnet TargetSubnet
        {
            get;
            set;
        }
        IPAllocationMethodEnum TargetPrivateIPAllocationMethod
        {
            get;
            set;
        }

        String TargetPrivateIpAddress
        {
            get;
            set;
        }

    }
}

