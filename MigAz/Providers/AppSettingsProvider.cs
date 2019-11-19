// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using MigAz.Azure.UserControls;
using MigAz.Azure.Core;
using MigAz.Azure.Core.Interface;

namespace MigAz.Providers
{
    public class AppSettingsProvider : ISettingsProvider
    {
        public bool AllowTelemetry
        {
            get
            {
                return false; //todonowasap app.Default.AllowTelemetry;
            }

            set
            {
                //todonowasap app.Default.AllowTelemetry = value;
                //todonowasap app.Default.Save();
            }
        }

        public bool BuildEmpty
        {
            get
            {
                return false; //todonowasap app.Default.BuildEmpty;
            }

            set
            {
                //todonowasap app.Default.BuildEmpty = value;
                //todonowasap app.Default.Save();
            }
        }

        public string StorageAccountSuffix
        {
            get
            {
                return "v2";//todonowasap app.Default.StorageAccountSuffix;
            }

            set
            {
                //todonowasap app.Default.StorageAccountSuffix = value;
                //todonowasap app.Default.Save();
            }
        }
        public string AvailabilitySetSuffix
        {
            get
            {
                return String.Empty; //todonowasap app.Default.AvailabilitySetSuffix;
            }

            set
            {
                //todonowasap app.Default.AvailabilitySetSuffix = value;
                //todonowasap app.Default.Save();
            }
        }

        public string NetworkInterfaceCardSuffix
        {
            get
            {
                return String.Empty; //todonowasap return app.Default.NetworkInterfaceCardSuffix;
            }

            set
            {
                //app.Default.NetworkInterfaceCardSuffix = value;
                //app.Default.Save();
            }
        }

        public string VirtualNetworkSuffix
        {
            get 
            {
                return String.Empty; //todonowasap return app.Default.VirtualNetworkSuffix; 
            }
            set
            {
                //app.Default.VirtualNetworkSuffix = value;
                //app.Default.Save();
            }
        }
        public string ResourceGroupSuffix
        {
            get 
            {
                return String.Empty; //todonowasap return app.Default.ResourceGroupSuffix;
            }
            set
            {
                //app.Default.ResourceGroupSuffix = value;
                //app.Default.Save();
            }
        }

        public string VirtualNetworkGatewaySuffix
        {
            get 
            {
                return String.Empty; //todonowasap return app.Default.VirtualNetworkGatewaySuffix; 
            }
            set
            {
                //app.Default.VirtualNetworkGatewaySuffix = value;
                //app.Default.Save();
            }
        }
        public string PublicIPSuffix
        {
            get 
            {
                return String.Empty; //todonowasap return app.Default.PublicIPSuffix; 
            }
            set
            {
                //app.Default.PublicIPSuffix = value;
                //app.Default.Save();
            }
        }

        public string NetworkSecurityGroupSuffix
        {
            get 
            {
                return String.Empty; //todonowasap     return app.Default.NetworkSecurityGroupSuffix; 
            }
            set
            {
                //app.Default.NetworkSecurityGroupSuffix = value;
                //app.Default.Save();
            }
        }

        public string LoadBalancerSuffix
        {
            get 
            {
                return String.Empty; //todonowasap return app.Default.LoadBalancerSuffix;
            }
            set
            {
                //app.Default.LoadBalancerSuffix = value;
                //app.Default.Save();
            }
        }

        public string VirtualMachineSuffix
        {
            get
            {
                return String.Empty; //todonowasap                 return app.Default.VirtualMachineSuffix; 
            }
            set
            {
                //app.Default.VirtualMachineSuffix = value;
                //app.Default.Save();
            }
        }

        internal TargetSettings GetTargetSettings()
        {
            TargetSettings targetSettings = new TargetSettings();
            targetSettings.AvailabilitySetSuffix = this.AvailabilitySetSuffix;
            targetSettings.NetworkInterfaceCardSuffix = this.NetworkInterfaceCardSuffix;
            targetSettings.NetworkSecurityGroupSuffix = this.NetworkSecurityGroupSuffix;
            targetSettings.StorageAccountSuffix = this.StorageAccountSuffix;
            targetSettings.VirtualMachineSuffix = this.VirtualMachineSuffix;
            targetSettings.VirtualNetworkSuffix = this.VirtualNetworkSuffix;

            return targetSettings;
        }
    }
}

