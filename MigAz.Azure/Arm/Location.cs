// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

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
        private AzureSubscription _AzureSubscription;
        private JToken _LocationToken;
        private List<VMSize> _ArmVmSizes;

        #region Constructors

        private Location() { }

        public Location(AzureSubscription azureSubscription, JToken locationToken)
        {
            this._AzureSubscription = azureSubscription;
            this._LocationToken = locationToken;
        }

        internal async Task InitializeChildrenAsync()
        {
            await this.LoadAzureARMVMSizes();
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

        public List<VMSize> VMSizes
        {
            get { return _ArmVmSizes; }
        }

        #endregion

        #region Methods

        private async Task<List<VMSize>> LoadAzureARMVMSizes()
        {
            AzureContext azureContext = this.AzureSubscription.AzureTenant.AzureContext;

            azureContext.LogProvider.WriteLog("GetAzureARMLocationVMSizes", "Start - Location : " + this.Name);

            if (_ArmVmSizes != null)
                return _ArmVmSizes;

            if (azureContext == null)
                throw new ArgumentNullException("AzureContext is null.  Unable to call Azure API without Azure Context.");
            if (azureContext.TokenProvider == null)
                throw new ArgumentNullException("TokenProvider Context is null.  Unable to call Azure API without TokenProvider.");

            AuthenticationResult armToken = await azureContext.TokenProvider.GetToken(this.AzureSubscription.TokenResourceUrl, this.AzureSubscription.AzureTenant.TenantId);


            // https://docs.microsoft.com/en-us/rest/api/compute/virtualmachines/virtualmachines-list-sizes-region
            string url = this.AzureSubscription.ApiUrl + "subscriptions/" + this.AzureSubscription.SubscriptionId + String.Format(ArmConst.ProviderVMSizes, this.Name) + "?api-version=" + this.AzureSubscription.GetProviderMaxApiVersion("Microsoft.Compute", "locations/vmSizes");
            azureContext.StatusProvider.UpdateStatus("BUSY: Getting ARM Azure VMSizes Location : " + this.ToString());

            AzureRestRequest azureRestRequest = new AzureRestRequest(url, armToken);
            AzureRestResponse azureRestResponse = await azureContext.AzureRetriever.GetAzureRestResponse(azureRestRequest);
            JObject locationsVMSizesJson = JObject.Parse(azureRestResponse.Response);

            azureContext.StatusProvider.UpdateStatus("BUSY: Loading VMSizes for Location: " + this.ToString());

            var VMSizes = from VMSize in locationsVMSizesJson["value"]
                            select VMSize;

            List<VMSize> vmSizes = new List<VMSize>();
            foreach (var VMSize in VMSizes)
            {
                Arm.VMSize armVMSize = new Arm.VMSize(VMSize);
                vmSizes.Add(armVMSize);
            }

            _ArmVmSizes = vmSizes.OrderBy(a => a.Name).ToList();

            azureContext.StatusProvider.UpdateStatus("Ready");

            return _ArmVmSizes;
        }

        public VMSize SeekVmSize(string name)
        {
            if (_ArmVmSizes == null)
                throw new Exception("You must call InitializeChildrenAsync before seeking a VM Size.");

            return _ArmVmSizes.Where(a => a.Name == name).FirstOrDefault();
        }

        public override string ToString()
        {
            return this.DisplayName;
        }

        #endregion
    }
}

