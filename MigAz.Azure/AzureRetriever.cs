using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using MigAz.Azure.Asm;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using MigAz.Core.ArmTemplate;
using System.Windows.Forms;
using Newtonsoft.Json;
using MigAz.Core.Interface;
using MigAz.Azure.Arm;

namespace MigAz.Azure
{
    public class AzureRetriever
    {
        private AzureContext _AzureContext;
        private object _lockObject = new object();
        private AzureSubscription _AzureSubscription = null;
        private List<AzureSubscription> _AzureSubscriptions;

        public delegate void OnRestResultHandler(AzureRestResponse response);
        public event OnRestResultHandler OnRestResult;

        // ARM Object Cache (Subscription Context Specific)
        private List<AzureTenant> _ArmTenants;
        private List<AzureSubscription> _ArmSubscriptions;

        private Dictionary<string, AzureRestResponse> _RestApiCache = new Dictionary<string, AzureRestResponse>();
        private Dictionary<AzureSubscription, AzureSubscriptionResourceCache> _AzureSubscriptionResourceCaches = new Dictionary<AzureSubscription, AzureSubscriptionResourceCache>();

        private AzureRetriever() { }

        public AzureRetriever(AzureContext azureContext)
        {
            _AzureContext = azureContext;
        }

        public AzureSubscription SubscriptionContext
        {
            get { return _AzureSubscription; }
        }

        public void ClearCache()
        {
            _RestApiCache = new Dictionary<string, AzureRestResponse>();
            _ArmTenants = null;
            _ArmSubscriptions = null;
        }

        public void LoadRestCache(string filepath)
        {
            StreamReader reader = new StreamReader(filepath);
            _RestApiCache = JsonConvert.DeserializeObject<Dictionary<string, AzureRestResponse>>(reader.ReadToEnd());
        }

        public void SaveRestCache()
        {
            string jsontext = JsonConvert.SerializeObject(_RestApiCache, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore });

            string filedir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\MigAz";
            if (!Directory.Exists(filedir)) { Directory.CreateDirectory(filedir); }

            string filePath = filedir + "\\AzureRestResponse-" + this._AzureContext.AzureEnvironment.ToString() + "-" + this._AzureContext.AzureSubscription.SubscriptionId + ".json";

            StreamWriter saveSelectionWriter = new StreamWriter(filePath);
            saveSelectionWriter.Write(jsontext);
            saveSelectionWriter.Close();
        }

       

        private void writeRetreiverResultToLog(Guid requestGuid, string method, string url, string xml)
        {
            lock (_lockObject)
            {
                string logfilepath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\MigAz\\MigAz-XML-" + string.Format("{0:yyyyMMdd}", DateTime.Now) + ".log";
                string text = DateTime.Now.ToString() + "  " + requestGuid.ToString() + "  " + url + Environment.NewLine;
                File.AppendAllText(logfilepath, text);
                File.AppendAllText(logfilepath, xml + Environment.NewLine);
                File.AppendAllText(logfilepath, Environment.NewLine);
            }

            _AzureContext.LogProvider.WriteLog(method, requestGuid.ToString() + " Received REST Response");
        }



        public async Task SetSubscriptionContext(AzureSubscription azureSubscription)
        {
            _AzureSubscription = azureSubscription;

            if (_AzureContext.TokenProvider == null)
                _AzureContext.TokenProvider = new AzureTokenProvider(_AzureContext.AzureServiceUrls.GetAzureLoginUrl(), _AzureContext.LogProvider);
            else
            {
            }
            // russell await this._AzureContext.TokenProvider.GetToken(_AzureContext.AzureServiceUrls.GetASMServiceManagementUrl(), azureSubscription.AzureAdTenantId);
        }


        public async Task<List<AzureTenant>> GetAzureARMTenants()
        {
            _AzureContext.LogProvider.WriteLog("GetAzureARMTenants", "Start");

            if (_ArmTenants != null)
                return _ArmTenants;

            if (_AzureContext == null)
                throw new ArgumentNullException("AzureContext is null.  Unable to call Azure API without Azure Context.");
            if (_AzureContext.TokenProvider == null)
                throw new ArgumentNullException("TokenProvider Context is null.  Unable to call Azure API without TokenProvider.");

            AuthenticationResult tenantAuthenticationResult = await _AzureContext.TokenProvider.GetToken(_AzureContext.AzureServiceUrls.GetARMServiceManagementUrl(), Guid.Empty);

            String tenantUrl = _AzureContext.AzureServiceUrls.GetARMServiceManagementUrl() + "tenants?api-version=2015-01-01";
            _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting Tenants...");

            AzureRestRequest azureRestRequest = new AzureRestRequest(tenantUrl, tenantAuthenticationResult.AccessToken, "GET", true);
            AzureRestResponse azureRestResponse = await _AzureContext.AzureRetriever.GetAzureRestResponse(azureRestRequest);
            JObject tenantsJson = JObject.Parse(azureRestResponse.Response);

            var tenants = from tenant in tenantsJson["value"]
                          select tenant;

            _ArmTenants = new List<AzureTenant>();

            foreach (JObject tenantJson in tenants)
            {
                AzureTenant azureTenant = new AzureTenant(tenantJson, _AzureContext);
                await azureTenant.InitializeChildren();
                _ArmTenants.Add(azureTenant);
            }

            return _ArmTenants;
        }

        public async Task<List<AzureDomain>> GetAzureARMDomains(AzureTenant azureTenant)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureARMDomains", "Start");

            if (_AzureContext == null)
                throw new ArgumentNullException("AzureContext is null.  Unable to call Azure API without Azure Context.");
            if (_AzureContext.TokenProvider == null)
                throw new ArgumentNullException("TokenProvider Context is null.  Unable to call Azure API without TokenProvider.");

            String domainUrl = _AzureContext.AzureServiceUrls.GetGraphApiUrl() + "myorganization/domains?api-version=1.6";

            AuthenticationResult tenantAuthenticationResult = await _AzureContext.TokenProvider.GetToken(_AzureContext.AzureServiceUrls.GetGraphApiUrl(), azureTenant.TenantId);

            _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting Tenant Domain details from Graph...");

            AzureRestRequest azureRestRequest = new AzureRestRequest(domainUrl, tenantAuthenticationResult.AccessToken, "GET", false);
            AzureRestResponse azureRestResponse = await _AzureContext.AzureRetriever.GetAzureRestResponse(azureRestRequest);
            JObject domainsJson = JObject.Parse(azureRestResponse.Response);

            var domains = from domain in domainsJson["value"]
                          select domain;

            List<AzureDomain> armTenantDomains = new List<AzureDomain>();

            foreach (JObject domainJson in domains)
            {
                AzureDomain azureDomain = new AzureDomain(domainJson, _AzureContext);
                armTenantDomains.Add(azureDomain);
            }

            return armTenantDomains;
        }

        public async Task<List<AzureSubscription>> GetAzureARMSubscriptions(AzureTenant azureTenant)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureARMSubscriptions", "Start - azureTenant: " + azureTenant.ToString());

            String subscriptionsUrl = _AzureContext.AzureServiceUrls.GetARMServiceManagementUrl() + "subscriptions?api-version=2015-01-01";
            AuthenticationResult authenticationResult = await _AzureContext.TokenProvider.GetToken(_AzureContext.AzureServiceUrls.GetARMServiceManagementUrl(), azureTenant.TenantId);

            _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting Subscriptions...");

            AzureRestRequest azureRestRequest = new AzureRestRequest(subscriptionsUrl, authenticationResult.AccessToken, "GET", false);
            AzureRestResponse azureRestResponse = await _AzureContext.AzureRetriever.GetAzureRestResponse(azureRestRequest);
            JObject subscriptionsJson = JObject.Parse(azureRestResponse.Response);

            var subscriptions = from subscription in subscriptionsJson["value"]
                                select subscription;

            List<AzureSubscription> tenantSubscriptions = new List<AzureSubscription>();

            foreach (JObject azureSubscriptionJson in subscriptions)
            {
                AzureSubscription azureSubscription = new AzureSubscription(_AzureContext, azureSubscriptionJson, azureTenant, _AzureContext.AzureEnvironment);
                tenantSubscriptions.Add(azureSubscription);
            }

            return tenantSubscriptions;
        }

        //public async Task<List<AzureSubscription>> GetSubscriptions()
        //{
        //    if (_AzureSubscriptions != null)
        //        return _AzureSubscriptions;


        //    String subscriptionsUrl = _AzureContext.AzureServiceUrls.GetASMServiceManagementUrl() + "subscriptions";
        //    _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting Subscriptions...");

        //    AzureRestRequest azureRestRequest = new AzureRestRequest(subscriptionsUrl, _AzureContext.TokenProvider.AccessToken);
        //    azureRestRequest.Headers.Add("x-ms-version", "2015-04-01");
        //    AzureRestResponse azureRestResponse = await _AzureContext.AzureRetriever.GetAzureRestResponse(azureRestRequest);

        //    XmlDocument subscriptionsXml = AzureSubscription.RemoveXmlns(azureRestResponse.Response);

        //    _AzureSubscriptions = new List<AzureSubscription>();
        //    foreach (XmlNode subscriptionXml in subscriptionsXml.SelectNodes("//Subscription"))
        //    {
        //        AzureSubscription azureSubscription = new AzureSubscription(_AzureContext, subscriptionXml, this._AzureContext.AzureEnvironment);
        //        _AzureSubscriptions.Add(azureSubscription);
        //    }

        //    return _AzureSubscriptions;
        //}

        public async Task<List<AzureSubscription>> GetAzureARMSubscriptions(Guid tenantGuid)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureARMSubscriptions", "Start");

            if (_ArmSubscriptions != null)
                return _ArmSubscriptions;

            String subscriptionsUrl = _AzureContext.AzureServiceUrls.GetARMServiceManagementUrl() + "subscriptions?api-version=2015-01-01";

            AuthenticationResult armToken = await _AzureContext.TokenProvider.GetToken(_AzureContext.AzureServiceUrls.GetARMServiceManagementUrl(), tenantGuid);

            _AzureContext.StatusProvider.UpdateStatus("BUSY: Getting Subscriptions...");

            AzureRestRequest azureRestRequest = new AzureRestRequest(subscriptionsUrl, armToken.AccessToken, "GET", true);
            AzureRestResponse azureRestResponse = await _AzureContext.AzureRetriever.GetAzureRestResponse(azureRestRequest);
            JObject subscriptionsJson = JObject.Parse(azureRestResponse.Response);

            var subscriptions = from subscription in subscriptionsJson["value"]
                                select subscription;

            _ArmSubscriptions = new List<AzureSubscription>();

            foreach (JObject azureSubscriptionJson in subscriptions)
            {
                AzureSubscription azureSubscription = new AzureSubscription(_AzureContext, azureSubscriptionJson, null, _AzureContext.AzureEnvironment);
                _ArmSubscriptions.Add(azureSubscription);
            }

            return _ArmSubscriptions;
        }



        public async Task<AzureRestResponse> GetAzureRestResponse(AzureRestRequest azureRestRequest)
        {
            _AzureContext.LogProvider.WriteLog("GetAzureRestResponse", azureRestRequest.RequestGuid.ToString() + " Url: " + azureRestRequest.Url);

            if (azureRestRequest.UseCached && _RestApiCache.ContainsKey(azureRestRequest.Url))
            {
                _AzureContext.LogProvider.WriteLog("GetAzureRestResponse", azureRestRequest.RequestGuid.ToString() + " Using Cached Response");
                _AzureContext.LogProvider.WriteLog("GetAzureRestResponse", azureRestRequest.RequestGuid.ToString() + " End REST Request");
                AzureRestResponse cachedRestResponse = (AzureRestResponse)_RestApiCache[azureRestRequest.Url];
                return cachedRestResponse;
            }

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(azureRestRequest.Url);
            request.Method = azureRestRequest.Method;

            if (azureRestRequest.AccessToken != String.Empty)
            {
                string authorizationHeader = "Bearer " + azureRestRequest.AccessToken;
                request.Headers.Add(HttpRequestHeader.Authorization, authorizationHeader);
                writeRetreiverResultToLog(azureRestRequest.RequestGuid, "GetAzureRestResponse", azureRestRequest.Url, authorizationHeader);
            }

            if (request.Method == "POST")
                request.ContentLength = 0;

            foreach (String headerKey in azureRestRequest.Headers.Keys)
            {
                request.Headers.Add(headerKey, azureRestRequest.Headers[headerKey]);
            }

            string webRequesetResult = String.Empty;
            try
            {
                _AzureContext.LogProvider.WriteLog("GetAzureRestResponse", azureRestRequest.RequestGuid.ToString() + " " + azureRestRequest.Method + " " + azureRestRequest.Url);

                // Retry Guidlines for 500 series with Backoff Timer - https://msdn.microsoft.com/en-us/library/azure/jj878112.aspx  https://msdn.microsoft.com/en-us/library/azure/gg185909.aspx
                HttpWebResponse response = null;
                const Int32 maxRetrySecond = 32;
                Int32 retrySeconds = 1;
                bool boolRetryGetResponse = true;
                while (boolRetryGetResponse)
                {
                    try
                    {
                        response = (HttpWebResponse)await request.GetResponseAsync();
                        boolRetryGetResponse = false;
                    }
                    catch (WebException webException)
                    {
                        _AzureContext.LogProvider.WriteLog("GetAzureRestResponse", azureRestRequest.RequestGuid.ToString() + " EXCEPTION " + webException.Message);

                        HttpWebResponse exceptionResponse = (HttpWebResponse)webException.Response;

                        if ((exceptionResponse != null) &&
                            (
                                (int)exceptionResponse.StatusCode == 429 || // 429 Too Many Requests
                                ((int)exceptionResponse.StatusCode >= 500 && (int)exceptionResponse.StatusCode <= 599)
                            )
                            )
                        {
                            DateTime sleepUntil = DateTime.Now.AddSeconds(retrySeconds);
                            string sleepMessage = "Sleeping for " + retrySeconds.ToString() + " second(s) (until " + sleepUntil.ToString() + ") before web request retry.";

                            _AzureContext.LogProvider.WriteLog("GetAzureRestResponse", azureRestRequest.RequestGuid.ToString() + " " + sleepMessage);
                            _AzureContext.StatusProvider.UpdateStatus(sleepMessage);
                            while (DateTime.Now < sleepUntil)
                            {
                                Application.DoEvents();
                            }
                            retrySeconds = retrySeconds * 2;

                            if (retrySeconds > maxRetrySecond)
                            {
                                _AzureContext.LogProvider.WriteLog("GetAzureRestResponse", azureRestRequest.RequestGuid.ToString() + " Too many retry.");
                                _AzureContext.StatusProvider.UpdateStatus("Too many retry.");
                                throw webException;
                                // too many retry -> throw exception 
                            }

                            _AzureContext.LogProvider.WriteLog("GetAzureRestResponse", azureRestRequest.RequestGuid.ToString() + " Initiating retry of Web Request.");
                            _AzureContext.StatusProvider.UpdateStatus("Initiating retry of Web Request.");
                        }
                        else
                            throw webException;
                    }
                }

                webRequesetResult = new StreamReader(response.GetResponseStream()).ReadToEnd();

                writeRetreiverResultToLog(azureRestRequest.RequestGuid, "GetAzureRestResponse", azureRestRequest.Url, webRequesetResult);
                _AzureContext.LogProvider.WriteLog("GetAzureRestResponse", azureRestRequest.RequestGuid.ToString() + "  Status Code " + response.StatusCode);
            }
            catch (Exception exception)
            {
                _AzureContext.LogProvider.WriteLog("GetAzureRestResponse", azureRestRequest.RequestGuid.ToString() + azureRestRequest.Url + "  EXCEPTION " + exception.Message);
                throw exception;
            }

            _AzureContext.LogProvider.WriteLog("GetAzureRestResponse", azureRestRequest.RequestGuid.ToString() + " End REST Request");

            AzureRestResponse azureRestResponse = new AzureRestResponse(azureRestRequest, webRequesetResult);

            if (!_RestApiCache.ContainsKey(azureRestRequest.Url))
                _RestApiCache.Add(azureRestRequest.Url, azureRestResponse);

            OnRestResult?.Invoke(azureRestResponse);

            return azureRestResponse;
        }

        

    
    }
}
