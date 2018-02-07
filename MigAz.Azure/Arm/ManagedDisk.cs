using Microsoft.IdentityModel.Clients.ActiveDirectory;
using MigAz.Azure.Interface;
using MigAz.Core.ArmTemplate;
using MigAz.Core.Interface;
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

        public Int32 Lun
        {
            get
            {
                try
                {
                    Int32 lun = -1;
                    Int32.TryParse((string)this.ResourceToken["lun"], out lun);

                    return lun;
                }
                catch (System.NullReferenceException)
                {
                    return -1;
                }
            }
        }
        public string HostCaching
        {
            get
            {
                if (this.ResourceToken["caching"] == null)
                    return String.Empty;

                return (string)this.ResourceToken["caching"];
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

        public StorageAccountType StorageAccountType
        {
            get
            {
                if (AccountType == "Premium_LRS")
                    return StorageAccountType.Premium_LRS;
                else
                    return StorageAccountType.Standard_LRS;
            }
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

        public async Task<string> GetSASUrlAsync(int tokenDurationSeconds)
        {
            if (this._AzureContext != null && this._AzureContext.LogProvider != null)
                _AzureContext.LogProvider.WriteLog("GetSASUrlAsync", "Start Disk '" + this.Name + "'");

            if (this._AzureContext != null && this._AzureContext.StatusProvider != null)
                _AzureContext.StatusProvider.UpdateStatus("Getting Access SAS for Managed Disk '" + this.Name + "'");

            if (_AzureContext.AzureSubscription.SubscriptionId == Guid.Empty)
                return String.Empty;

            // https://docs.microsoft.com/en-us/rest/api/compute/manageddisks/disks/disks-grant-access
            string url = "/subscriptions/" + _AzureContext.AzureSubscription.SubscriptionId + "/resourceGroups/" + this.ResourceGroup.Name + ArmConst.ProviderManagedDisks + this.Name + "/BeginGetAccess?api-version=2017-03-30";
            string strAccessSAS = String.Empty;

            AuthenticationResult authenticationResult = await _AzureContext.TokenProvider.GetToken(_AzureContext.AzureServiceUrls.GetARMServiceManagementUrl(), _AzureContext.AzureSubscription.AzureAdTenantId, _AzureContext.PromptBehavior);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticationResult.AccessToken);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.BaseAddress = new Uri(_AzureContext.AzureServiceUrls.GetARMServiceManagementUrl());

                if (this._AzureContext != null && this._AzureContext.LogProvider != null)
                    _AzureContext.LogProvider.WriteLog("GetSASUrlAsync", "Disk '" + this.Name + "' PostAsync " + url + "");

                IEnumerable<string> requestId;
                using (var response = await client.PostAsJsonAsync(url,
                        new
                        {
                            access = "Read",
                            durationInSeconds = tokenDurationSeconds.ToString()
                        })
                    )
                {
                    //response.EnsureSuccessStatusCode();
                    response.Headers.TryGetValues("x-ms-request-id", out requestId);
                }

                String diskOperationStatus = "InProgress";

                while (diskOperationStatus == "InProgress")
                { 
                    string url2 = "/subscriptions/" + _AzureContext.AzureSubscription.SubscriptionId + "/providers/Microsoft.Compute/locations/" + this.ResourceGroup.Location + "/DiskOperations/" + requestId.ToList<string>()[0].ToString() + "?api-version=2017-03-30";

                    if (this._AzureContext != null && this._AzureContext.LogProvider != null)
                        _AzureContext.LogProvider.WriteLog("GetSASUrlAsync", "Disk '" + this.Name + "' GetAsync " + url2 + "");

                    using (var response2 = await client.GetAsync(url2))
                    {
                        //response2.EnsureSuccessStatusCode();
                        string responseString = await response2.Content.ReadAsStringAsync();
                        JObject responseJson = JObject.Parse(responseString);

                        diskOperationStatus = responseJson["status"].ToString();

                        if (this._AzureContext != null && this._AzureContext.LogProvider != null)
                            _AzureContext.LogProvider.WriteLog("GetSASUrlAsync", "Disk '" + this.Name + "' Disk Operation Status " + diskOperationStatus + "");

                        if (diskOperationStatus == "InProgress")
                        {
                            System.Threading.Thread.Sleep(1000);
                        }
                        else if (diskOperationStatus == "Succeeded")
                        {
                            strAccessSAS = responseJson["properties"]["output"]["accessSAS"].ToString();

                            if (this._AzureContext != null && this._AzureContext.LogProvider != null)
                                _AzureContext.LogProvider.WriteLog("GetSASUrlAsync", "Disk '" + this.Name +  "' Obtained AccessSAS " + strAccessSAS + "");
                        }
                    }
                }
            }

            if (this._AzureContext != null && this._AzureContext.LogProvider != null)
                _AzureContext.LogProvider.WriteLog("GetSASUrlAsync", "End Disk '" + this.Name + "'");

            return strAccessSAS;
        }
    }
}
