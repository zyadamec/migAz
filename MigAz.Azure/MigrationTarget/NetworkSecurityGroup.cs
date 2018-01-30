using MigAz.Core;
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
        private INetworkSecurityGroup _SourceNetworkSecurityGroup;
        private string _TargetName = String.Empty;
        private string _TargetNameResult = String.Empty;
        private List<NetworkSecurityGroupRule> _Rules = new List<NetworkSecurityGroupRule>();

        private NetworkSecurityGroup() { }

        public NetworkSecurityGroup(Asm.NetworkSecurityGroup source, TargetSettings targetSettings)
        {
            _SourceNetworkSecurityGroup = source;
            this.SetTargetName(source.Name, targetSettings);

            foreach (Asm.NetworkSecurityGroupRule sourceRule in source.Rules)
            {
                NetworkSecurityGroupRule targetRule = new NetworkSecurityGroupRule(sourceRule, targetSettings);
                this.Rules.Add(targetRule);
            }
        }

        public NetworkSecurityGroup(Arm.NetworkSecurityGroup source, TargetSettings targetSettings)
        {
            _SourceNetworkSecurityGroup = source;
            this.SetTargetName(source.Name, targetSettings);

            foreach (Arm.NetworkSecurityGroupRule sourceRule in source.Rules)
            {
                NetworkSecurityGroupRule targetRule = new NetworkSecurityGroupRule(sourceRule, targetSettings);
                this.Rules.Add(targetRule);
            }
        }

        public static NetworkSecurityGroup SeekNetworkSecurityGroup(List<NetworkSecurityGroup> networkSecurityGroups, string sourceName)
        {
            foreach (NetworkSecurityGroup networkSecurityGroup in networkSecurityGroups)
            {
                if (networkSecurityGroup.SourceName == sourceName)
                    return networkSecurityGroup;
            }

            return null;
        }


        public List<NetworkSecurityGroupRule> Rules
        {
            get { return _Rules; }
        }
        
        public INetworkSecurityGroup SourceNetworkSecurityGroup
        {
            get { return _SourceNetworkSecurityGroup; }
        }

        public String SourceName
        {
            get
            {
                if (this.SourceNetworkSecurityGroup == null)
                    return String.Empty;
                else
                    return this.SourceNetworkSecurityGroup.ToString();
            }
        }

        public string TargetName
        {
            get { return _TargetName; }
        }

        public string TargetNameResult
        {
            get { return _TargetNameResult; }
        }

        public void SetTargetName(string targetName, TargetSettings targetSettings)
        {
            _TargetName = targetName.Trim().Replace(" ", String.Empty);
            _TargetNameResult = _TargetName + targetSettings.NetworkSecurityGroupSuffix;
        }

        public override string ToString()
        {
            return this.TargetNameResult;
        }
    }
}
