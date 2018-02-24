using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Arm
{
    public class Provider
    {
        private JToken _ProviderToken;
        private AzureSubscription _AzureSubscription;
        private List<ProviderResourceType> _ResourceTypes;

        public Provider(JToken resourceToken, AzureSubscription azureSubscription)
        {
            _ProviderToken = resourceToken;
            _AzureSubscription = azureSubscription;

            _ResourceTypes = new List<ProviderResourceType>();

            var resourceTypesJson = from resourceTypeJson in _ProviderToken["resourceTypes"]
                            select resourceTypeJson;

            foreach (var resourceTypeJson in resourceTypesJson)
            {
                ProviderResourceType armProviderResourcetype = new ProviderResourceType(resourceTypeJson, this);
                _ResourceTypes.Add(armProviderResourcetype);
            }
        }

        public AzureSubscription AzureSubscription
        {
            get { return _AzureSubscription; }
        }

        public string Id => (string)_ProviderToken["id"];
        public string Namespace => (string)_ProviderToken["namespace"];
        public string RegistrationState => (string)_ProviderToken["registrationState"];
        public List<ProviderResourceType> ResourceTypes => _ResourceTypes;

        public override string ToString()
        {
            return this.Namespace;
        }
    }
}
