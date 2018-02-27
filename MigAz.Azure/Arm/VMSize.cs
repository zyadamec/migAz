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
    public class VMSize
    {
        private JToken _VMSizeToken;

        public VMSize(JToken resourceToken)
        {
            _VMSizeToken = resourceToken;
        }

        public string Name => (string)_VMSizeToken["name"];
        public Int32 NumberOfCores => (Int32)_VMSizeToken["numberOfCores"];
        public Int32 osDiskSizeInMB => (Int32)_VMSizeToken["osDiskSizeInMB"];
        public Int32 resourceDiskSizeInMB => (Int32)_VMSizeToken["resourceDiskSizeInMB"];
        public Int32 memoryInMB => (Int32)_VMSizeToken["memoryInMB"];
        public Int32 maxDataDiskCount => (Int32)_VMSizeToken["maxDataDiskCount"];

        public override string ToString()
        {
            return this.Name;
        }

        public bool IsStorageTypeSupported(StorageAccountType storageAccountType)
        {
            if (storageAccountType == StorageAccountType.Standard_LRS)
                return true;
            else if (storageAccountType == StorageAccountType.Premium_LRS)
            {
                // TODO : maybe use a regex pattern to check VM size ?
                return (
                    this.Name.Contains("_B") ||
                    this.Name.Contains("_DS") ||
                    this.Name.Contains("_GS") ||
                    (this.Name.Contains("_L") && this.Name.EndsWith("s")) ||
                    (this.Name.Contains("_F") && this.Name.EndsWith("s")) ||
                    (this.Name.StartsWith("Standard_D") && this.Name.EndsWith("s_v3"))
                    );
            }

            return false; // Unknown Enum Type
            
        }

    }
}

