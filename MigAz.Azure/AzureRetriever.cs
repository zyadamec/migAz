// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

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
using MigAz.Azure.Interface;

namespace MigAz.Azure
{
    public class AzureRetriever
    {
        private ILogProvider _LogProvider;
        private IStatusProvider _StatusProvider;
        private object _lockObject = new object();

        public delegate void OnRestResultHandler(AzureRestResponse response);
        public event OnRestResultHandler OnRestResult;

        private Dictionary<string, AzureRestResponse> _RestApiCache = new Dictionary<string, AzureRestResponse>();
        private Dictionary<AzureSubscription, AzureSubscriptionResourceCache> _AzureSubscriptionResourceCaches = new Dictionary<AzureSubscription, AzureSubscriptionResourceCache>();

        private AzureRetriever() { }

        public AzureRetriever(ILogProvider logProvider, IStatusProvider statusProvider)
        {
            _LogProvider = logProvider;
            _StatusProvider = statusProvider;
        }

        public ILogProvider LogProvider
        {
            get { return _LogProvider; }
        }

        public IStatusProvider StatusProvider
        {
            get { return _StatusProvider; }
        }

        public void ClearCache()
        {
            _RestApiCache = new Dictionary<string, AzureRestResponse>();
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

            string filePath = filedir + "\\AzureRestResponse-" + DateTime.UtcNow.ToShortTimeString() + ".json";

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

            _LogProvider.WriteLog(method, requestGuid.ToString() + " Received REST Response");
        }


        public async Task<AzureRestResponse> GetAzureRestResponse(AzureRestRequest azureRestRequest)
        {
            _LogProvider.WriteLog("GetAzureRestResponse", azureRestRequest.RequestGuid.ToString() + " Url: " + azureRestRequest.Url);

            if (azureRestRequest.UseCached && _RestApiCache.ContainsKey(azureRestRequest.Url))
            {
                _LogProvider.WriteLog("GetAzureRestResponse", azureRestRequest.RequestGuid.ToString() + " Using Cached Response");
                _LogProvider.WriteLog("GetAzureRestResponse", azureRestRequest.RequestGuid.ToString() + " End REST Request");
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
                _LogProvider.WriteLog("GetAzureRestResponse", azureRestRequest.RequestGuid.ToString() + " " + azureRestRequest.Method + " " + azureRestRequest.Url);

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
                        _LogProvider.WriteLog("GetAzureRestResponse", azureRestRequest.RequestGuid.ToString() + " EXCEPTION " + webException.Message);

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

                            _LogProvider.WriteLog("GetAzureRestResponse", azureRestRequest.RequestGuid.ToString() + " " + sleepMessage);
                            _StatusProvider.UpdateStatus(sleepMessage);
                            while (DateTime.Now < sleepUntil)
                            {
                                Application.DoEvents();
                            }
                            retrySeconds = retrySeconds * 2;

                            if (retrySeconds > maxRetrySecond)
                            {
                                _LogProvider.WriteLog("GetAzureRestResponse", azureRestRequest.RequestGuid.ToString() + " Too many retry.");
                                _StatusProvider.UpdateStatus("Too many retry.");
                                throw webException;
                                // too many retry -> throw exception 
                            }

                            _LogProvider.WriteLog("GetAzureRestResponse", azureRestRequest.RequestGuid.ToString() + " Initiating retry of Web Request.");
                            _StatusProvider.UpdateStatus("Initiating retry of Web Request.");
                        }
                        else
                            throw webException;
                    }
                }

                webRequesetResult = new StreamReader(response.GetResponseStream()).ReadToEnd();

                writeRetreiverResultToLog(azureRestRequest.RequestGuid, "GetAzureRestResponse", azureRestRequest.Url, webRequesetResult);
                _LogProvider.WriteLog("GetAzureRestResponse", azureRestRequest.RequestGuid.ToString() + "  Status Code " + response.StatusCode);
            }
            catch (Exception exception)
            {
                _LogProvider.WriteLog("GetAzureRestResponse", azureRestRequest.RequestGuid.ToString() + azureRestRequest.Url + "  EXCEPTION " + exception.Message);
                throw exception;
            }

            _LogProvider.WriteLog("GetAzureRestResponse", azureRestRequest.RequestGuid.ToString() + " End REST Request");

            AzureRestResponse azureRestResponse = new AzureRestResponse(azureRestRequest, webRequesetResult);

            if (!_RestApiCache.ContainsKey(azureRestRequest.Url))
                _RestApiCache.Add(azureRestRequest.Url, azureRestResponse);

            OnRestResult?.Invoke(azureRestResponse);

            return azureRestResponse;
        }
    }
}

