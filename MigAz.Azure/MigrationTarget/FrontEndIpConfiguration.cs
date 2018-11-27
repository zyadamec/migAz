// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using MigAz.Azure.Interface;
using MigAz.Azure.Core;
using MigAz.Azure.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.MigrationTarget
{
    public enum IPAllocationMethodEnum
    {
        Dynamic,
        Static
    }

    public class FrontEndIpConfiguration : IVirtualNetworkTarget
    {
        private String _Name = "default";
        private IPAllocationMethodEnum _TargetPrivateIPAllocationMethod = IPAllocationMethodEnum.Dynamic;
        private String _TargetStaticIpAddress = String.Empty;
        private IMigrationPublicIp _PublicIp = null;

        private LoadBalancer _ParentLoadBalancer = null;
        private Arm.FrontEndIpConfiguration _Source;

        private FrontEndIpConfiguration() { }

        public FrontEndIpConfiguration(LoadBalancer loadBalancer)
        {
            _ParentLoadBalancer = loadBalancer;
            loadBalancer.FrontEndIpConfigurations.Add(this);
        }

        public FrontEndIpConfiguration(LoadBalancer loadBalancer, Arm.FrontEndIpConfiguration armFrontEndIpConfiguration)
        {
            _ParentLoadBalancer = loadBalancer;
            _Source = armFrontEndIpConfiguration;

            this.Name = armFrontEndIpConfiguration.Name;
            if (armFrontEndIpConfiguration.PrivateIPAllocationMethod.Trim().ToLower() == "static")
                this.TargetPrivateIPAllocationMethod = IPAllocationMethodEnum.Static;
            else
                this.TargetPrivateIPAllocationMethod = IPAllocationMethodEnum.Dynamic;
            this.TargetPrivateIpAddress = armFrontEndIpConfiguration.PrivateIPAddress;
            this.TargetVirtualNetwork = armFrontEndIpConfiguration.VirtualNetwork;
            this.TargetSubnet = armFrontEndIpConfiguration.Subnet;
        }

        #region IVirtualNetworkTarget Interface Implementation
        public IMigrationVirtualNetwork TargetVirtualNetwork { get; set; }
        public IMigrationSubnet TargetSubnet { get; set; }
        public IPAllocationMethodEnum TargetPrivateIPAllocationMethod { get; set; }
        public string TargetPrivateIpAddress { get; set; }

        #endregion

        public LoadBalancer LoadBalancer
        {
            get { return _ParentLoadBalancer; }
        }

        public Arm.FrontEndIpConfiguration Source
        {
            get { return _Source; }
        }

        public String Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public IMigrationPublicIp PublicIp
        {
            get { return _PublicIp; }
            set
            {
                _PublicIp = value;

                if (value != null)
                    this.LoadBalancer.LoadBalancerType = LoadBalancerType.Public;
            }
        }
    }
}

