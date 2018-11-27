// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using MigAz.Azure.Core.Interface;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        public bool IsAcceleratedNetworkingSupported
        {
            get
            {
                // https://azure.microsoft.com/en-us/blog/maximize-your-vm-s-performance-with-accelerated-networking-now-generally-available-for-both-windows-and-linux/
                // Supported VM series include D/DSv2, D/DSv3, E/ESv3, F/FS, FSv2, and Ms/Mms.

                Regex regExAcceleratedNetworking = new Regex(@"(_D[0-9]+|_DS[0-9]+_V2|_DS[0-9]+_V3|_E[0-9]+|_ES[0-9]+_V3|_F[0-9]+|_FS[0-9]+|_FS[0-9]+_V2|_MS[0-9]+|_MMS[0-9]+)", RegexOptions.IgnoreCase);
                return regExAcceleratedNetworking.IsMatch(this.Name);
            }
        }
    }
}

