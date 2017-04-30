using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.MigrationTarget
{
    public class NetworkSecurityGroup : IMigrationTarget
    {
        private AzureContext _AzureContext = null;
        private INetworkSecurityGroup _Source;
        private string _TargetName = String.Empty;
        private List<NetworkSecurityGroupRule> _Rules = new List<NetworkSecurityGroupRule>();


        private NetworkSecurityGroup() { }

        public NetworkSecurityGroup(AzureContext azureContext, INetworkSecurityGroup source)
        {
            _AzureContext = azureContext;
            _Source = source;
            this.TargetName = source.Name;
        }

        public List<NetworkSecurityGroupRule> Rules
        {
            get { return _Rules; }
        }
        
        public INetworkSecurityGroup Source
        {
            get { return _Source; }
        }

        public string TargetName
        {
            get { return _TargetName; }
            set { _TargetName = value.Trim(); }
        }

        public string GetFinalTargetName()
        {
            return this.TargetName + _AzureContext.SettingsProvider.NetworkSecurityGroupSuffix;
        }
    }
}
