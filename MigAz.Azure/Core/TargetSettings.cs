// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using MigAz.Azure.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Core
{
    public class TargetSettings
    {
        public string VirtualMachineSuffix;
        public string StorageAccountSuffix;
        public string NetworkInterfaceCardSuffix;
        public string AvailabilitySetSuffix;
        public string NetworkSecurityGroupSuffix;
        public string VirtualNetworkSuffix;
        public ArmDiskType DefaultTargetDiskType;
    }
}

