using Newtonsoft.Json.Linq;
using MigAz.Azure.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;
using MigAz.Core.Interface;
using MigAz.Core.ArmTemplate;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Linq;

namespace MigAz.Azure.Arm
{
    public class Location : ILocation
    {
        private AzureContext _AzureContext;
        private AzureSubscription _AzureSubscription;
        private JToken _LocationToken;
        private List<VMSize> _ArmVmSizes;

        #region Constructors

        private Location() { }

        public Location(AzureContext azureContext, AzureSubscription azureSubscription, JToken locationToken)
        {
            this._AzureContext = azureContext;
            this._AzureSubscription = azureSubscription;
            this._LocationToken = locationToken;
        }

        internal async Task InitializeChildrenAsync()
        {
            await this.GetAzureARMVMSizes();
        }

        #endregion

        #region Properties

        public string Id
        {
            get { return (string)_LocationToken["id"]; }
        }

        public string DisplayName
        {
            get { return (string)_LocationToken["displayName"]; }
        }

        public string Longitude
        {
            get { return (string)_LocationToken["longitude"]; }
        }

        public string Latitude
        {
            get { return (string)_LocationToken["latitude"]; }
        }

        public string Name
        {
            get { return (string)_LocationToken["name"]; }
        }


        public AzureSubscription AzureSubscription
        {
            get { return _AzureSubscription; }
        }

        #endregion

        #region Methods

        public async Task<List<VMSize>> GetAzureARMVMSizes()
        {
            _AzureContext.LogProvider.WriteLog("GetAzureARMLocationVMSizes", "Start - Location : " + this.Name);

            if (_ArmVmSizes != null)
                return _ArmVmSizes;

            if (_AzureContext == null)
                throw new ArgumentNullException("AzureContext is null.  Unable to call Azure API without Azure Context.");
            if (_AzureContext.TokenProvider == null)
                throw new ArgumentNullException("TokenProvider Context is null.  Unable to call Azure API without TokenProvider.");

            AuthenticationResult armToken = await _AzureContext.TokenProvider.GetToken(this.AzureSubscription.TokenResourceUrl, this.AzureSubscription.AzureAdTenantId);
            List<Provider> providers = await this.AzureSubscription.GetResourceManagerProviders();

            Provider p = (await this.AzureSubscription.GetResourceManagerProviders()).Where(a => a.Namespace == "Microsoft.Compute").FirstOrDefault();
            ProviderResourceType prt = p.ResourceTypes.Where(a => a.ResourceType == "locations/vmSizes").FirstOrDefault();


            // https://docs.microsoft.com/en-us/rest/api/compute/virtualmachines/virtualmachines-list-sizes-region
            string url = this.AzureSubscription.ApiUrl + "subscriptions/" + this.AzureSubscription.SubscriptionId + String.Format(ArmConst.ProviderVMSizes, this.Name) + "?api-version=" + prt.MaxApiVersion;
            _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting ARM Azure VMSizes for Subscription: " + this.ToString() + " Location : " + this.ToString());

            AzureRestRequest azureRestRequest = new AzureRestRequest(url, armToken);
            AzureRestResponse azureRestResponse = await _AzureContext.AzureRetriever.GetAzureRestResponse(azureRestRequest);
            JObject locationsVMSizesJson = JObject.Parse(azureRestResponse.Response);

            _AzureContext.StatusProvider.UpdateStatus("BUSY: Loading VMSizes for Subscription: " + this.ToString() + " Location : " + this.ToString());

            var VMSizes = from VMSize in locationsVMSizesJson["value"]
                          select VMSize;

            List<VMSize> vmSizes = new List<VMSize>();
            foreach (var VMSize in VMSizes)
            {
                Arm.VMSize armVMSize = new Arm.VMSize(VMSize);
                vmSizes.Add(armVMSize);
            }

            _ArmVmSizes = vmSizes.OrderBy(a => a.Name).ToList();
            return _ArmVmSizes;
        }

        public async Task<VMSize> SeekVmSize(string name)
        {
            List<VMSize> vmSizes = await this.GetAzureARMVMSizes();

            if (vmSizes == null)
                return null;
            else 
                return vmSizes.Where(a => a.Name == name).FirstOrDefault();
        }

        public override string ToString()
        {
            return this.DisplayName;
        }

        #endregion
    }
}
