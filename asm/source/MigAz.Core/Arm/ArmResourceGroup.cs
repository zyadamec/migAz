using MigAz.Azure;
using MigAz.Azure.Interface;
using System;

namespace MigAz.Azure.Arm
{
    public class ArmResourceGroup
    {
        private String _Name = String.Empty;
        private ILocation _Location;
        private AzureContext _AzureContext;

        private ArmResourceGroup() { }

        public ArmResourceGroup(AzureContext azureContext, String resourceGroupName)
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
