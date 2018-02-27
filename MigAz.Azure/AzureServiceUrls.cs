using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure
{
    public class AzureServiceUrls
    {
        private AzureContext _AzureContext = null;
        private AzureEnvironment? _AzureEnvironment = null;
        private ILogProvider _LogProvider;

        private AzureServiceUrls() { }

        public AzureServiceUrls(AzureEnvironment azureEnvironment, ILogProvider logProvider)
        {
            _AzureEnvironment = azureEnvironment;
            _LogProvider = logProvider;
        }

        public AzureServiceUrls(AzureContext azureContext)
        {
            if (azureContext == null)
                throw new ArgumentException("Invalid Azure Context.");

            _AzureContext = azureContext;
        }

        private Dictionary<string, string[]> _serviceUrls = new Dictionary<string, string[]>
        {
            {  "AzureCloud", new[] { "https://management.core.windows.net/", "https://management.azure.com/", "https://login.microsoftonline.com/", "core.windows.net", "blob.core.windows.net", "https://graph.windows.net/" } },
            // China Endpoints - https://msdn.microsoft.com/en-us/library/azure/dn578439.aspx
            {  "AzureChinaCloud", new[] { "https://management.core.chinacloudapi.cn/", "https://management.chinacloudapi.cn/", "https://login.chinacloudapi.cn/", "core.chi acloudapi.cn", "blob.core.chinacloudapi.cn", "https://graph.chinacloudapi.cn/" } },
            // Germany Endpoints - https://blogs.msdn.microsoft.com/azuregermany/2016/08/18/endpoints-in-microsoft-cloud-germany/
            {  "AzureGermanCloud", new[] { "https://management.core.cloudapi.de/", "https://management.microsoftazure.de/",  "https://login.microsoftonline.de/", "core.cloudapi.de", "blob.core.cloudapi.de", "https://graph.cloudapi.de/" } },
            // US Gov Endpoints - https://docs.microsoft.com/en-us/azure/azure-government/documentation-government-developer-guide
            {  "AzureUSGovernment", new[] { "https://management.core.usgovcloudapi.net/", "https://management.usgovcloudapi.net/", "https://login-us.microsoftonline.com/", "core.usgovcloudapi.net", "blob.core.usgovcloudapi.net", "https://graph.windows.net/" } },
        };

        private ILogProvider LogProviderContext()
        {
            if (_LogProvider != null)
                return _LogProvider;
            else if (_AzureContext != null && _AzureContext.LogProvider != null)
                return _AzureContext.LogProvider;
            else
                return null;
        }

        private AzureEnvironment AzureEnvironmentContext()
        {
            if (_AzureEnvironment != null)
                return _AzureEnvironment.Value;
            else if (_AzureContext != null)
                return _AzureContext.AzureEnvironment;
            else
                return AzureEnvironment.AzureCloud;
        }

        public string GetASMServiceManagementUrl()
        {
            if (LogProviderContext() != null)
                LogProviderContext().WriteLog("AzureServiceUrls", "GetASMServiceManagementUrl " + AzureEnvironmentContext().ToString() + " " + _serviceUrls[AzureEnvironmentContext().ToString()][0]);

            return _serviceUrls[AzureEnvironmentContext().ToString()][0];
        }

        public string GetARMServiceManagementUrl()
        {
            if (LogProviderContext() != null)
                LogProviderContext().WriteLog("AzureServiceUrls", "GetARMServiceManagementUrl " + AzureEnvironmentContext().ToString() + " " + _serviceUrls[AzureEnvironmentContext().ToString()][1]);

            return _serviceUrls[AzureEnvironmentContext().ToString()][1];
        }

        public string GetAzureLoginUrl()
        {
            if (LogProviderContext() != null)
                LogProviderContext().WriteLog("AzureServiceUrls", "GetAzureLoginUrl " + AzureEnvironmentContext().ToString() + " " + _serviceUrls[AzureEnvironmentContext().ToString()][2]);

            return _serviceUrls[AzureEnvironmentContext().ToString()][2];
        }

        public string GetStorageEndpointUrl()
        {
            if (LogProviderContext() != null)
                LogProviderContext().WriteLog("AzureServiceUrls", "GetStorageEndpointUrl " + AzureEnvironmentContext().ToString() + " " + _serviceUrls[AzureEnvironmentContext().ToString()][3]);

            return _serviceUrls[AzureEnvironmentContext().ToString()][3];
        }

        public string GetBlobEndpointUrl()
        {
            if (LogProviderContext() != null)
                LogProviderContext().WriteLog("AzureServiceUrls", "GetBlobEndpointUrl " + AzureEnvironmentContext().ToString() + " " + _serviceUrls[AzureEnvironmentContext().ToString()][4]);

            return _serviceUrls[AzureEnvironmentContext().ToString()][4];
        }

        public string GetGraphApiUrl()
        {
            if (LogProviderContext() != null)
                LogProviderContext().WriteLog("AzureServiceUrls", "GetGraphApiUrl " + AzureEnvironmentContext().ToString() + " " + _serviceUrls[AzureEnvironmentContext().ToString()][5]);

            return _serviceUrls[AzureEnvironmentContext().ToString()][5];
        }
    }
}
