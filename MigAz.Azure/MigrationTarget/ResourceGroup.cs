using MigAz.Azure.Interface;
using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.MigrationTarget
{
    public class ResourceGroup : IMigrationTarget
    {
        private AzureContext _AzureContext;
        private String _TargetName = String.Empty;
        private ILocation _TargetLocation;

        private ResourceGroup() { }

        public ResourceGroup(AzureContext azureContext)
        {
            this._AzureContext = azureContext;
            _TargetName = "New Resource Group";
        }

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
