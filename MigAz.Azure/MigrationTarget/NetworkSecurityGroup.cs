// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using MigAz.Azure.Core;
using MigAz.Azure.Core.ArmTemplate;
using MigAz.Azure.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.MigrationTarget
{
    public class NetworkSecurityGroup : Core.MigrationTarget
    {
        private INetworkSecurityGroup _SourceNetworkSecurityGroup;
        private List<NetworkSecurityGroupRule> _Rules = new List<NetworkSecurityGroupRule>();

        private NetworkSecurityGroup() : base(ArmConst.MicrosoftNetwork, ArmConst.NetworkSecurityGroups, null) { }

        public NetworkSecurityGroup(Asm.NetworkSecurityGroup source, TargetSettings targetSettings, ILogProvider logProvider) : base(ArmConst.MicrosoftNetwork, ArmConst.NetworkSecurityGroups, logProvider)
        {
            _SourceNetworkSecurityGroup = source;
            this.SetTargetName(source.Name, targetSettings);

            foreach (Asm.NetworkSecurityGroupRule sourceRule in source.Rules)
            {
                NetworkSecurityGroupRule targetRule = new NetworkSecurityGroupRule(sourceRule, targetSettings, this.LogProvider);
                this.Rules.Add(targetRule);
            }
        }

        public NetworkSecurityGroup(Arm.NetworkSecurityGroup source, TargetSettings targetSettings, ILogProvider logProvider) : base(ArmConst.MicrosoftNetwork, ArmConst.NetworkSecurityGroups, logProvider)
        {
            _SourceNetworkSecurityGroup = source;
            this.SetTargetName(source.Name, targetSettings);

            foreach (Arm.NetworkSecurityGroupRule sourceRule in source.Rules)
            {
                NetworkSecurityGroupRule targetRule = new NetworkSecurityGroupRule(sourceRule, targetSettings, this.LogProvider);
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

        public override string ImageKey { get { return "NetworkSecurityGroup"; } }

        public override string FriendlyObjectName { get { return "Network Security Group"; } }


        public override void SetTargetName(string targetName, TargetSettings targetSettings)
        {
            this.TargetName = targetName.Trim().Replace(" ", String.Empty);
            this.TargetNameResult = this.TargetName + targetSettings.NetworkSecurityGroupSuffix;
        }
    }
}

