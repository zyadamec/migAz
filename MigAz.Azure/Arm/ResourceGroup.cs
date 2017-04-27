using MigAz.Azure;
using MigAz.Azure.Interface;
using MigAz.Core.Interface;
using Newtonsoft.Json.Linq;
using System;

namespace MigAz.Azure.Arm
{
    public class ResourceGroup
    {
        private String _TargetName = String.Empty;
        private ILocation _TargetLocation;
        private AzureContext _AzureContext;

        private JObject _ResourceGroupJson;
        private AzureEnvironment _AzureEnvironment;

        internal ResourceGroup(JObject resourceGroupJson, AzureEnvironment azureEnvironment)
        {
            _ResourceGroupJson = resourceGroupJson;
            _AzureEnvironment = azureEnvironment;
        }

        private ResourceGroup() { }

        public ResourceGroup(AzureContext azureContext, String targetName)
        {
            this._AzureContext = azureContext;
            this.TargetName = targetName;
        }

        public string Name => (string)_ResourceGroupJson["name"];
        public string Location => (string)_ResourceGroupJson["location"];
        public string Id => (string)_ResourceGroupJson["id"];

        public String TargetName
        {
            get { return _TargetName; }
            set { _TargetName = value.Trim(); }
        }
        public ILocation TargetLocation
        {
            get { return _TargetLocation; }
            set { _TargetLocation = value; }
        }

        public string GetFinalTargetName()
        {
            return this.TargetName + this._AzureContext.SettingsProvider.ResourceGroupSuffix;
        }
    }
}
