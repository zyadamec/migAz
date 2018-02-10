using MigAz.Core;
using MigAz.Core.Interface;
using System;

namespace MigAz.Azure.MigrationTarget
{
    public class NetworkSecurityGroupRule : Core.MigrationTarget
    {
        private AzureContext _AzureContext;
        private string _TargetName = String.Empty;
        private string _TargetNameResult = String.Empty;
        private bool _IsSystemRule = false;

        public NetworkSecurityGroupRule(Asm.NetworkSecurityGroupRule asmRule, TargetSettings targetSettings)
        {
            this.SetTargetName(asmRule.Name, targetSettings);
            this.Access = asmRule.Action;
            this.DestinationAddressPrefix = asmRule.DestinationAddressPrefix;
            this.DestinationPortRange = asmRule.DestinationPortRange;
            this.Direction = asmRule.Type;
            this.IsSystemRule = asmRule.IsSystemRule;
            this.Priority = asmRule.Priority;
            this.Protocol = asmRule.Protocol;
            this.SourceAddressPrefix = asmRule.SourceAddressPrefix;
            this.SourcePortRange = asmRule.SourcePortRange;
        }

        public NetworkSecurityGroupRule(Arm.NetworkSecurityGroupRule armRule, TargetSettings targetSettings)
        {
            this.SetTargetName(armRule.Name, targetSettings);
            this.Access = armRule.Access;
            this.DestinationAddressPrefix = armRule.DestinationAddressPrefix;
            this.DestinationPortRange = armRule.DestinationPortRange;
            this.Direction = armRule.Direction;
            this.Priority = armRule.Priority;
            this.Protocol = armRule.Protocol;
            this.SourcePortRange = armRule.SourcePortRange;
            this.SourceAddressPrefix = armRule.SourceAddressPrefix;
        }

        public string Direction
        {
            get; set;
        }

        public long Priority
        {
            get; set;
        }

        public string Access
        {
            get; set;
        }

        public string SourceAddressPrefix
        {
            get; set;
        }

        public string DestinationAddressPrefix
        {
            get; set;
        }

        public string SourcePortRange
        {
            get; set;
        }

        public string DestinationPortRange
        {
            get; set;
        }

        public string Protocol
        {
            get; set;
        }

        public bool IsSystemRule
        {
            get { return _IsSystemRule; }
            set { _IsSystemRule = value; }
        }

        public String SourceName
        {
            get
            {
                return String.Empty;
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

        public override void SetTargetName(string targetName, TargetSettings targetSettings)
        {
            _TargetName = targetName.Trim().Replace(" ", String.Empty);
            _TargetNameResult = _TargetName;
        }

        public override string ToString()
        {
            return this.TargetNameResult;
        }
    }
}
