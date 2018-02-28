// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Arm
{
    public class ArmResource
    {
        private JToken _ResourceToken;
        private AzureSubscription _AzureSubscription;
        private Location _Location;

        private ArmResource() { }

        internal ArmResource(AzureSubscription azureSubscription, JToken resourceToken)
        {
            _AzureSubscription = azureSubscription;
            _ResourceToken = resourceToken;
        }
        internal virtual async Task InitializeChildrenAsync()
        {
            this.ResourceGroup = await this.AzureSubscription.GetAzureARMResourceGroup(this.Id);

            if (this.LocationString != null && this.LocationString.Length > 0)
                this.Location = this.AzureSubscription.GetAzureARMLocation(this.LocationString);

            return;
        }

        internal void SetResourceToken(JToken resourceToken)
        {
            _ResourceToken = resourceToken;
        }

        public AzureSubscription AzureSubscription
        {
            get { return _AzureSubscription; }
        }

        public JToken ResourceToken => _ResourceToken;
        public string Name => (string)_ResourceToken["name"];
        public string Id => (string)_ResourceToken["id"];
        private string LocationString => (string)_ResourceToken["location"];

        public Location Location
        {
            get
            {
                return _Location;
            }
            private set
            {
                _Location = value;
            }
        }

        public ResourceGroup ResourceGroup { get; internal set; }
    }
}

