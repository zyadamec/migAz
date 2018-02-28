// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.MigrationTarget
{
    public class ManagedDiskStorage : IStorageTarget
    {
        private StorageAccountType _StorageAccountType = StorageAccountType.Premium_LRS;

        public ManagedDiskStorage(IDisk sourceDisk)
        {
            if (sourceDisk != null)
            {
                _StorageAccountType = sourceDisk.StorageAccountType;
            }
        }

        public string BlobStorageNamespace
        {
            get
            {
                return String.Empty;
            }
        }

        public StorageAccountType StorageAccountType
        {
            get { return _StorageAccountType; }
            set { _StorageAccountType = value; }
        }
    }
}

