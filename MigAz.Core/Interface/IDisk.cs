// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Core.Interface
{
    public enum ArmDiskType
    {
        ManagedDisk,
        ClassicDisk
    }

    public interface IDisk
    {
        Int32 DiskSizeGb { get; }
        bool IsEncrypted { get; }
        StorageAccountType StorageAccountType { get; }
        int? Lun { get; }
        String HostCaching { get; }
    }
}

