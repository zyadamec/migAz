// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure
{
    public enum AzureEnvironmentType
    {
        Azure,
        AzureStack
    }

    public class AzureEnvironment
    {
        #region Constructors

        public AzureEnvironment()
        {
            this.Name = "NewAzureEnvironment";
            this.AzureEnvironmentType = AzureEnvironmentType.Azure;
        }

        public AzureEnvironment(
            AzureEnvironmentType azureEnvironmentType,
            String name,
            String azureLoginUrl,
            String graphApiUrl,
            String asmServiceManagementUrl, 
            String armServiceManagementUrl,
            String storageEndpointUrl,
            String blobEndpointUrl)
        {
            this.AzureEnvironmentType = azureEnvironmentType;
            this.Name = name;
            this.ASMServiceManagementUrl = asmServiceManagementUrl;
            this.ARMServiceManagementUrl = armServiceManagementUrl;
            this.AzureLoginUrl = azureLoginUrl;
            this.StorageEndpointUrl = storageEndpointUrl;
            this.BlobEndpointUrl = blobEndpointUrl;
            this.GraphApiUrl = graphApiUrl;
        }

        #endregion

        public AzureEnvironmentType AzureEnvironmentType { get; set; }
        public String Name { get; set; }
        public String ASMServiceManagementUrl { get; set; }
        public String ARMServiceManagementUrl { get; set; }
        public String AzureLoginUrl { get; set; }
        public String StorageEndpointUrl { get; set; }
        public String BlobEndpointUrl { get; set; }
        public String GraphApiUrl { get; set; }

        public override string ToString()
        {
            return this.Name;
        }

        public static List<AzureEnvironment> GetAzureEnvironments()
        {
            List<AzureEnvironment> azureEnvironments = new List<AzureEnvironment>();

            azureEnvironments.Add(
                new AzureEnvironment(
                    AzureEnvironmentType.Azure,
                    "AzureCloud",
                    "https://login.microsoftonline.com/", // logonUrl
                    "https://graph.windows.net/", //GraphUrl
                    "https://management.core.windows.net/", // ASM
                    "https://management.azure.com/", // ARM
                    "core.windows.net", // Storage
                    "blob.core.windows.net") // Blob
                );

            // China Endpoints - https://msdn.microsoft.com/en-us/library/azure/dn578439.aspx
            azureEnvironments.Add(
                new AzureEnvironment(
                    AzureEnvironmentType.Azure,
                    "AzureChinaCloud",
                    "https://login.chinacloudapi.cn/", // logonUrl
                    "https://graph.chinacloudapi.cn/", //GraphUrl
                    "https://management.core.chinacloudapi.cn/", // ASM
                    "https://management.chinacloudapi.cn/", // ARM
                    "core.chi acloudapi.cn", // Storage
                    "blob.core.chinacloudapi.cn") // Blob
                );

            // Germany Endpoints - https://blogs.msdn.microsoft.com/azuregermany/2016/08/18/endpoints-in-microsoft-cloud-germany/
            azureEnvironments.Add(
                new AzureEnvironment(
                    AzureEnvironmentType.Azure,
                    "AzureGermanCloud",
                    "https://login.microsoftonline.de/", // logonUrl
                    "https://graph.cloudapi.de/", //GraphUrl
                    "https://management.core.cloudapi.de/", // ASM
                    "https://management.microsoftazure.de/", // ARM
                    "core.cloudapi.de", // Storage
                    "blob.core.cloudapi.de") // Blob
                );

            // US Gov Endpoints - https://docs.microsoft.com/en-us/azure/azure-government/documentation-government-developer-guide
            azureEnvironments.Add(
                new AzureEnvironment(
                    AzureEnvironmentType.Azure,
                    "AzureUSGovernment",
                    "https://login-us.microsoftonline.com/", // logonUrl
                    "https://graph.windows.net/", //GraphUrl
                    "https://management.core.usgovcloudapi.net/", // ASM
                    "https://management.usgovcloudapi.net/", // ARM
                    "core.usgovcloudapi.net", // Storage
                    "blob.core.usgovcloudapi.net") // Blob
                );

            return azureEnvironments;
        }
    }
}