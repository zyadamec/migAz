using MigAz.Azure;
using MigAz.Azure.Interface;
using MigAz.Core.Interface;
using Newtonsoft.Json.Linq;
using System;

namespace MigAz.Azure.Arm
{
    public class ResourceGroup
    {
        private String _Name = String.Empty;
        private ILocation _Location;
        private AzureContext _AzureContext;

        private JObject _ResourceGroupJson;
        private AzureEnvironment _AzureEnvironment;

        internal ResourceGroup(JObject resourceGroupJson, AzureEnvironment azureEnvironment)
        {
            _ResourceGroupJson = resourceGroupJson;
            _AzureEnvironment = azureEnvironment;
        }

        private ResourceGroup() { }

        public ResourceGroup(AzureContext azureContext, String resourceGroupName)
        {
            this._AzureContext = azureContext;
            this.Name = resourceGroupName;
        }

        public String Name
        {
            get { return _Name; }
            set { _Name = value.Trim(); }
        }
        public ILocation Location
        {
            get { return _Location; }
            set { _Location = value; }
        }

        public string GetFinalTargetName()
        {
            return this.Name + this._AzureContext.SettingsProvider.ResourceGroupSuffix;
        }
    }
}
