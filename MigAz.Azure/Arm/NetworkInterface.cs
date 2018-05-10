// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using MigAz.Core.Interface;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Arm
{

    public class NetworkInterface : ArmResource, INetworkInterface
    {
        private VirtualMachine _VirtualMachine;
        private List<NetworkInterfaceIpConfiguration> _NetworkInterfaceIpConfigurations = new List<NetworkInterfaceIpConfiguration>();

        private NetworkInterface() : base(null, null) { }

        public NetworkInterface(AzureSubscription azureSubscription, JToken resourceToken) : base(azureSubscription, resourceToken)
        {
            foreach (JToken networkInterfaceIpConfigurationToken in ResourceToken["properties"]["ipConfigurations"])
            {
                NetworkInterfaceIpConfiguration networkInterfaceIpConfiguration = new NetworkInterfaceIpConfiguration(azureSubscription, networkInterfaceIpConfigurationToken);
                _NetworkInterfaceIpConfigurations.Add(networkInterfaceIpConfiguration);
            }
        }

        internal async override Task InitializeChildrenAsync()
        {
            await base.InitializeChildrenAsync();

            foreach (NetworkInterfaceIpConfiguration networkInterfaceIpConfiguration in this.NetworkInterfaceIpConfigurations)
            {
                await networkInterfaceIpConfiguration.InitializeChildrenAsync();
            }

            if (this.NetworkSecurityGroupId != String.Empty)
                this.NetworkSecurityGroup = await this.AzureSubscription.GetAzureARMNetworkSecurityGroup(this.NetworkSecurityGroupId);
        }

        public bool EnableIPForwarding
        {
            get
            {
                try
                {
                    if (ResourceToken["properties"]["enableIPForwarding"] == null)
                        return false;

                    return (bool)ResourceToken["properties"]["enableIPForwarding"];
                }
                catch
                {
                    return false;
                }
            }
        }
        public bool EnableAcceleratedNetworking
        {
            get
            {
                try
                {
                    if (ResourceToken["properties"]["enableAcceleratedNetworking"] == null)
                        return false;

                    return (bool)ResourceToken["properties"]["enableAcceleratedNetworking"];
                }
                catch
                {
                    return false;
                }
            }
        }

        public Guid ResourceGuid => new Guid((string)ResourceToken["properties"]["resourceGuid"]);
        public bool IsPrimary => Convert.ToBoolean((string)ResourceToken["properties"]["primary"]);
        public VirtualMachine VirtualMachine
        {
            get { return _VirtualMachine; }
            set { _VirtualMachine = value; }
        }

        public List<NetworkInterfaceIpConfiguration> NetworkInterfaceIpConfigurations
        {
            get { return _NetworkInterfaceIpConfigurations; }
        }

        public NetworkInterfaceIpConfiguration PrimaryIpConfiguration
        {
            get
            {
                foreach (NetworkInterfaceIpConfiguration ipConfiguration in this._NetworkInterfaceIpConfigurations)
                {
                    if (ipConfiguration.IsPrimary)
                        return ipConfiguration;
                }

                return null;
            }
        }

        public override string ToString()
        {
            return this.Name;
        }

        private string NetworkSecurityGroupId
        {
            get
            {
                if (this.ResourceToken["properties"]["networkSecurityGroup"] == null)
                    return String.Empty;

                return (string)this.ResourceToken["properties"]["networkSecurityGroup"]["id"];
            }
        }

        public NetworkSecurityGroup NetworkSecurityGroup
        {
            get;
            private set;
        }
    }
}

