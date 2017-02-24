using MIGAZ.Core.Azure;
using MIGAZ.Core.Interface;
using MIGAZ.Core.Models.ARM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIGAZ.Core.Arm
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
