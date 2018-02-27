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

        private ProviderResourceType() { }

        public ProviderResourceType(JToken resourceToken, Provider provider)
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

        public override string ToString()
        {
            return this.ResourceType;
        }
    }
}

