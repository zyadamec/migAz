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
        private AzureEnvironmentType _AzureEnvironmentType = AzureEnvironmentType.Azure;
        private bool _IsUserDefined = true;
        private String _AzureStackAdminManagementUrl = String.Empty;
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


        public AzureEnvironment(AzureEnvironment azureEnvironment)
        {
            this.AzureEnvironmentType = azureEnvironment.AzureEnvironmentType;
            this.Name = "ClonedAzureEnvironment";
            this.AzureStackAdminManagementUrl = azureEnvironment.AzureStackAdminManagementUrl;
            this.ASMServiceManagementUrl = azureEnvironment.ASMServiceManagementUrl;
            this.ARMServiceManagementUrl = azureEnvironment.ARMServiceManagementUrl;
            this.AzureLoginUrl = azureEnvironment.AzureLoginUrl;
            this.StorageEndpointUrl = azureEnvironment.StorageEndpointUrl;
            this.BlobEndpointUrl = azureEnvironment.BlobEndpointUrl;
            this.GraphApiUrl = azureEnvironment.GraphApiUrl;
        }

        #endregion

        public AzureEnvironmentType AzureEnvironmentType
        {
            get { return _AzureEnvironmentType; }
            set
            {
                _AzureEnvironmentType = value;

                switch (value)
                {
                    case AzureEnvironmentType.Azure:
                        if (value == AzureEnvironmentType.Azure)
                            this.AzureStackAdminManagementUrl = String.Empty;
                        break;
                    case AzureEnvironmentType.AzureStack:
#if DEBUG
                        this.AzureStackAdminManagementUrl = "https://adminmanagement.local.azurestack.external";
#endif
                        break;
                }

            }
        }
        public String Name { get; set; }

        public String AzureStackAdminManagementUrl
        {
            get { return _AzureStackAdminManagementUrl; }
            set
            {
                if (_AzureEnvironmentType == AzureEnvironmentType.AzureStack)
                    _AzureStackAdminManagementUrl = value;
                else if (value != String.Empty)
                    throw new ArgumentException("Cannot set Azure Stack Admin Management URL when AzureEnvironmentType is not AzureStack.");
            }
        }

        public String ASMServiceManagementUrl { get; set; }
        public String ARMServiceManagementUrl { get; set; }
        public String AzureLoginUrl { get; set; }
        public String StorageEndpointUrl { get; set; }
        public String BlobEndpointUrl { get; set; }
        public String GraphApiUrl { get; set; }
        public bool IsUserDefined
        {
            get { return _IsUserDefined; }
            private set { _IsUserDefined = value; }
        }

        public override string ToString()
        {
            return this.Name;
        }

        public static List<AzureEnvironment> GetAzureEnvironments()
        {
            List<AzureEnvironment> azureEnvironments = new List<AzureEnvironment>();

            AzureEnvironment azureCloud = new AzureEnvironment(
                    AzureEnvironmentType.Azure,
                    "AzureCloud",
                    "https://login.microsoftonline.com/", // logonUrl
                    "https://graph.windows.net/", //GraphUrl
                    "https://management.core.windows.net/", // ASM
                    "https://management.azure.com/", // ARM
                    "core.windows.net", // Storage
                    "blob.core.windows.net"); // Blob
            azureCloud.IsUserDefined = false;
            azureEnvironments.Add(azureCloud);

            // China Endpoints - https://msdn.microsoft.com/en-us/library/azure/dn578439.aspx
            AzureEnvironment azureChinaCloud = new AzureEnvironment(
                    AzureEnvironmentType.Azure,
                    "AzureChinaCloud",
                    "https://login.chinacloudapi.cn/", // logonUrl
                    "https://graph.chinacloudapi.cn/", //GraphUrl
                    "https://management.core.chinacloudapi.cn/", // ASM
                    "https://management.chinacloudapi.cn/", // ARM
                    "core.chi acloudapi.cn", // Storage
                    "blob.core.chinacloudapi.cn"); // Blob
            azureChinaCloud.IsUserDefined = false;
            azureEnvironments.Add(azureChinaCloud);

            // Germany Endpoints - https://blogs.msdn.microsoft.com/azuregermany/2016/08/18/endpoints-in-microsoft-cloud-germany/
            AzureEnvironment azureGermanCloud = new AzureEnvironment(
                    AzureEnvironmentType.Azure,
                    "AzureGermanCloud",
                    "https://login.microsoftonline.de/", // logonUrl
                    "https://graph.cloudapi.de/", //GraphUrl
                    "https://management.core.cloudapi.de/", // ASM
                    "https://management.microsoftazure.de/", // ARM
                    "core.cloudapi.de", // Storage
                    "blob.core.cloudapi.de"); // Blob
            azureGermanCloud.IsUserDefined = false;
            azureEnvironments.Add(azureGermanCloud);


            // US Gov Endpoints - https://docs.microsoft.com/en-us/azure/azure-government/documentation-government-developer-guide

            AzureEnvironment azureUSGovernment = new AzureEnvironment(
                AzureEnvironmentType.Azure,
                "AzureUSGovernment",
                "https://login-us.microsoftonline.com/", // logonUrl
                "https://graph.windows.net/", //GraphUrl
                "https://management.core.usgovcloudapi.net/", // ASM
                "https://management.usgovcloudapi.net/", // ARM
                "core.usgovcloudapi.net", // Storage
                "blob.core.usgovcloudapi.net"); // Blob
            azureUSGovernment.IsUserDefined = false;
            azureEnvironments.Add(azureUSGovernment);


            return azureEnvironments;
        }
    }
}