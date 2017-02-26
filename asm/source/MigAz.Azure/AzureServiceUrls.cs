using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure
{
    static class AzureServiceUrls
    {
        private static Dictionary<string, string[]> _serviceUrls = new Dictionary<string, string[]>
        {
            {  "AzureCloud", new[] { "https://management.core.windows.net/", "https://management.azure.com/", "https://login.microsoftonline.com/", "blob.core.windows.net" } },
            {  "AzureChinaCloud", new[] { "https://management.core.chinacloudapi.cn/", "https://management.core.chinacloudapi.cn/", "https://login.chinacloudapi.cn/", "blob.core.chinacloudapi.cn" } },
            {  "AzureGermanCloud", new[] { "https://management.core.cloudapi.de/", "https://management.core.cloudapi.de/",  "https://login.microsoftonline.de/", "blob.core.cloudapi.de" } },
            {  "AzureUSGovernment", new[] { "https://management.core.usgovcloudapi.net/", "https://management.core.usgovcloudapi.net/", "https://login-us.microsoftonline.com/", "blob.core.usgovcloudapi.net" } },
        };

        public static string GetASMServiceManagementUrl(AzureEnvironment azureEnvironment)
        {
            return _serviceUrls[azureEnvironment.ToString()][0];
        }

        public static string GetARMServiceManagementUrl(AzureEnvironment azureEnvironment)
        {
            return _serviceUrls[azureEnvironment.ToString()][1];
        }

        public static string GetLoginUrl(AzureEnvironment azureEnvironment)
        {
            return _serviceUrls[azureEnvironment.ToString()][2];
        }

        public static string GetBlobEndpointUrl(AzureEnvironment azureEnvironment)
        {
            return _serviceUrls[azureEnvironment.ToString()][3];
        }
    }
}
