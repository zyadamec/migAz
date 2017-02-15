using MIGAZ.Asm;
using MIGAZ.Interface;
using MIGAZ.Models;
using MIGAZ.Models.ARM;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MIGAZ.Generator
{
    public class CloudTelemetryProvider : ITelemetryProvider
    {
        private Dictionary<string,string> GetProcessedItems(TemplateResult templateResult)
        {
            Dictionary<string, string> processedItems = new Dictionary<string, string>();

            foreach (ArmResource resource in templateResult.Resources)
            {
                if (!processedItems.ContainsKey(resource.type + resource.name))
                    processedItems.Add(resource.type + resource.name, resource.location);
            }

            return processedItems;
        }

        public void PostTelemetryRecord(TemplateResult templateResult)
        {
            TelemetryRecord telemetryrecord = new TelemetryRecord();
            telemetryrecord.ExecutionId = templateResult.ExecutionGuid;
            telemetryrecord.SubscriptionId = templateResult.SourceSubscription.SubscriptionId;
            telemetryrecord.TenantId = templateResult.SourceSubscription.AzureAdTenantId;
            telemetryrecord.OfferCategories = templateResult.SourceSubscription.offercategories;
            telemetryrecord.SourceVersion = Assembly.GetEntryAssembly().GetName().Version.ToString();
            telemetryrecord.ProcessedResources = this.GetProcessedItems(templateResult);

            string jsontext = JsonConvert.SerializeObject(telemetryrecord, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore });
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] data = encoding.GetBytes(jsontext);

            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("https://asmtoarmtoolapi.azurewebsites.net/api/telemetry");
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = data.Length;

                Stream stream = request.GetRequestStream();
                stream.Write(data, 0, data.Length);
                stream.Close();

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string result = new StreamReader(response.GetResponseStream()).ReadToEnd();

                //TelemetryRecord mytelemetry = (TelemetryRecord)JsonConvert.DeserializeObject(jsontext, typeof(TelemetryRecord));
            }
            catch (Exception exception)
            {
                DialogResult dialogresult = MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
