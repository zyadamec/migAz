using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure
{
    public class AzureRestResponse
    {

        public Guid RequestGuid { get; private set; }
        public String Url { get; private set; }
        public String AuthenticationToken { get; private set; }
        public String Response { get; private set; }

        private AzureRestResponse() { }

        public AzureRestResponse(Guid requestGuid, string url, string authenticationToken, string response)
        {
            this.RequestGuid = requestGuid;
            this.Url = url;
            this.AuthenticationToken = authenticationToken;
            this.Response = response;
        }

    }
}
