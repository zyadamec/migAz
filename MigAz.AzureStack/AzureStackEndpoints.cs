using MigAz.Azure;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.AzureStack
{
    public class AzureStackEndpoints
    {
        AzureRestResponse _AzureRestResponse;
        JObject _MetadataEndpoints;

        #region Constructors

        private AzureStackEndpoints() { }

        internal AzureStackEndpoints(AzureRestResponse azureStackMetadataEndpoints)
        {
            _AzureRestResponse = azureStackMetadataEndpoints;
            _MetadataEndpoints = JObject.Parse(_AzureRestResponse.Response);
        }

        #endregion

        #region Properties

        public string GalleryEndpoint
        {
            get { return _MetadataEndpoints["galleryEndpoint"].ToString(); }
        }
        public string GraphEndpoint
        {
            get { return _MetadataEndpoints["graphEndpoint"].ToString(); }
        }
        public string PortalEndpoint
        {
            get { return _MetadataEndpoints["portalEndpoint"].ToString(); }
        }

        public string LoginEndpoint
        {
            get { return _MetadataEndpoints["authentication"]["loginEndpoint"].ToString(); }
        }

        public string Audiences
        {
            get { return _MetadataEndpoints["authentication"]["audiences"][0].ToString(); }
        }

        #endregion
    }
}