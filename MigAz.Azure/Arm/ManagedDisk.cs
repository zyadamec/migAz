using MigAz.Azure.Interface;
using MigAz.Core.ArmTemplate;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Arm
{
    public class ManagedDisk : ArmResource, IArmDisk
    {
        private VirtualMachine _VirtualMachine;
        private AzureContext _AzureContext;

        private ManagedDisk() : base(null) { }

        public ManagedDisk(AzureContext azureContext, VirtualMachine virtualMachine, JToken resourceToken) : base(resourceToken)
        {
            _VirtualMachine = virtualMachine;
            _AzureContext = azureContext;
        }
        public ManagedDisk(AzureContext azureContext, JToken resourceToken) : base(resourceToken)
        {
            _AzureContext = azureContext;
        }

        public new async Task InitializeChildrenAsync(AzureContext azureContext)
        {
            await base.InitializeChildrenAsync(azureContext);
        }

        #region Properties

        public VirtualMachine ParentVirtualMachine
        {
            get { return _VirtualMachine; }
        }

        public string Type
        {
            get { return (string)ResourceToken["type"]; }
        }

        public Int32 DiskSizeGb
        {
            get
            {
                try
                {
                    Int32 diskSizeGb = 0;
                    Int32.TryParse((string)this.ResourceToken["properties"]["diskSizeGB"], out diskSizeGb);

                    return diskSizeGb;
                }
                catch (System.NullReferenceException)
                {
                    return 0;
                }
            }
        }

        public string OwnerId
        {
            get { return (string)ResourceToken["properties"]["ownerId"]; }
        }

        public string ProvisioningState
        {
            get { return (string)ResourceToken["properties"]["provisioningState"]; }
        }

        public string DiskState
        {
            get { return (string)ResourceToken["properties"]["diskState"]; }
        }
        public string TimeCreated
        {
            get { return (string)ResourceToken["properties"]["timeCreated"]; }
        }
        public string AccountType
        {
            get { return (string)ResourceToken["properties"]["accountType"]; }
        }

        public string CreateOption
        {
            get { return (string)ResourceToken["properties"]["creationData"]["createOption"]; }
        }

        public string SourceUri
        {
            get { return (string)ResourceToken["properties"]["creationData"]["sourceUri"]; }
        }

        public bool IsEncrypted
        {
            get
            {
                // todo now russell, this is not verified, only copied code
                if (this.ResourceToken["encryptionSettings"] == null)
                    return false;

                if (this.ResourceToken["encryptionSettings"]["enabled"] == null)
                    return false;

                return Convert.ToBoolean((string)this.ResourceToken["encryptionSettings"]["enabled"]);
            }
        }

        #endregion

        public override string ToString()
        {
            return this.Name;
        }

        public async Task<string> GetSASUrlAsync()
        {
            // https://docs.microsoft.com/en-us/rest/api/compute/manageddisks/disks/disks-grant-access
            string url = "/subscriptions/" + _AzureContext.AzureSubscription.SubscriptionId + "/resourceGroups/" + this.ResourceGroup.Name + ArmConst.ProviderManagedDisks + this.Name + "/BeginGetAccess?api-version=2016-04-30-preview";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _AzureContext.TokenProvider.AccessToken);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.BaseAddress = new Uri(_AzureContext.AzureServiceUrls.GetARMServiceManagementUrl());

                IEnumerable<string> requestId;
                using (var response = await client.PostAsJsonAsync(url,
                        new
                        {
                            access = "Read",
                            durationInSeconds = "3600"
                        })
                    )
                {
                    response.EnsureSuccessStatusCode();
                    response.Headers.TryGetValues("x-ms-request-id", out requestId);
                }

                string url2 = "/subscriptions/" + _AzureContext.AzureSubscription.SubscriptionId + "/providers/Microsoft.Compute/locations/westus/DiskOperations/" + requestId.ToList<string>()[0].ToString() + "?api-version=2016-04-30-preview";
                using (var response2 = await client.GetAsync(url2))
                {
                    response2.EnsureSuccessStatusCode();
                    string y2 = await response2.Content.ReadAsStringAsync();
                    JObject j = JObject.Parse(y2);

                    return j["properties"]["output"]["accessSAS"].ToString();
                }
            }

            return string.Empty;
        }
    }
}
