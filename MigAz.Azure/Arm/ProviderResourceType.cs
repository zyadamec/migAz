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
    public class ProviderResourceType
    {
        private JToken _ProviderResourceTypeToken;
        private Provider _Provider;
        private List<String> _ApiVersions;
        private List<Location> _Locations;

        private ProviderResourceType() { }

        public ProviderResourceType(Provider provider, JToken resourceToken)
        {
            _ProviderResourceTypeToken = resourceToken;
            _Provider = provider;

            _ApiVersions = new List<string>();
            var apiVersionsJson = from apiVersionJson in _ProviderResourceTypeToken["apiVersions"]
                                    select apiVersionJson;

            foreach (String apiVersion in apiVersionsJson)
            {
                _ApiVersions.Add(apiVersion);
            }

            _Locations = new List<Location>();
            var locatinsJson = from locationJson in _ProviderResourceTypeToken["locations"]
                                  select locationJson;

            foreach (String locationString in locatinsJson)
            {
                Location resourceTypeLocation = this.Provider.AzureSubscription.GetAzureARMLocation(locationString);

                if (resourceTypeLocation != null)
                    _Locations.Add(resourceTypeLocation);
            }
        }

        public Provider Provider
        {
            get { return _Provider; }
        }

        public String MaxApiVersion
        {
            get
            {
                return _ApiVersions.OrderByDescending(a => a).FirstOrDefault();
            }
        }

        public string ResourceType => (string)_ProviderResourceTypeToken["resourceType"];

        public List<String> ApiVersions => _ApiVersions;
        public List<Location> Locations => _Locations;

        public override string ToString()
        {
            return this.ResourceType;
        }

        public bool IsLocationSupported(Location location)
        {
            return this.Locations.Contains(location);
        }
    }
}

