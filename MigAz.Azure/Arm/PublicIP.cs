// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using MigAz.Core;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Arm
{
    public class PublicIP : ArmResource, IMigrationPublicIp
    {
        public PublicIP(AzureSubscription azureSubscription, JToken resourceToken) : base(azureSubscription, resourceToken)
        {
        }

        public string IpAddress => (string)this.ResourceToken["properties"]["ipAddress"];
        public string PublicIPAddressVersion => (string)this.ResourceToken["properties"]["publicIPAddressVersion"];
        public string PublicIPAllocationMethod => (string)this.ResourceToken["properties"]["publicIPAllocationMethod"];
        public string IdleTimeoutInMinutes => (string)this.ResourceToken["properties"]["idleTimeoutInMinutes"];
        public string DomainNameLabel
        {
            get
            {
                if (this.ResourceToken["properties"]["dnsSettings"] == null || this.ResourceToken["properties"]["dnsSettings"]["domainNameLabel"] == null)
                    return String.Empty;

                return (string) this.ResourceToken["properties"]["dnsSettings"]["domainNameLabel"];
            }
        }

        public string IpConfigurationId
        {
            get
            {
                if (this.ResourceToken["properties"]["ipConfiguration"] == null || this.ResourceToken["properties"]["ipConfiguration"]["id"] == null)
                    return String.Empty;

                return (string)this.ResourceToken["properties"]["ipConfiguration"]["id"];
            }
        }

        public string FQDN
        {
            get
            {
                if (this.ResourceToken["properties"]["dnsSettings"] == null || this.ResourceToken["properties"]["dnsSettings"]["fqdn"] == null)
                    return String.Empty;

                return (string)this.ResourceToken["properties"]["dnsSettings"]["fqdn"];
            }
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}

