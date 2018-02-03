using MigAz.Azure.Generator.AsmToArm;
using MigAz.Azure.Models;
using MigAz.Core.ArmTemplate;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace MigAz.Providers
{
    public class AzureTelemetryProvider : MigAz.Core.Interface.ITelemetryProvider
    {
        private Dictionary<string,string> GetProcessedItems(AzureGenerator templateResult)
        {
            Dictionary<string, string> processedItems = new Dictionary<string, string>();

            foreach (ArmResource resource in templateResult.Resources)
            {
                if (!processedItems.ContainsKey(resource.type + resource.name))
                    processedItems.Add(resource.type + resource.name, resource.location);
            }

            return processedItems;
        }

        public void PostTelemetryRecord(Guid appSessionGuid, AzureGenerator templateGenerator)
        {
            if (templateGenerator == null)
                throw new ArgumentException("Template Generator cannot be null.");

            TelemetryRecord telemetryrecord = new TelemetryRecord();
            telemetryrecord.AppSessionGuid = appSessionGuid;
            telemetryrecord.ExecutionGuid = templateGenerator.ExecutionGuid;

            if (templateGenerator.SourceSubscription != null)
            {
                telemetryrecord.SourceEnvironment = templateGenerator.SourceSubscription.AzureEnvironment.ToString();
                telemetryrecord.SourceSubscriptionGuid = templateGenerator.SourceSubscription.SubscriptionId;
                telemetryrecord.SourceTenantGuid = templateGenerator.SourceSubscription.AzureAdTenantId;
                telemetryrecord.OfferCategories = templateGenerator.SourceSubscription.offercategories;
            }

            if (templateGenerator.TargetSubscription != null)
            {
                telemetryrecord.TargetEnvironment = templateGenerator.TargetSubscription.AzureEnvironment.ToString();
                telemetryrecord.TargetSubscriptionGuid = templateGenerator.TargetSubscription.SubscriptionId;
                telemetryrecord.TargetTenantGuid = templateGenerator.TargetSubscription.AzureAdTenantId;
            }

            telemetryrecord.SourceVersion = Assembly.GetEntryAssembly().GetName().Version.ToString();
            telemetryrecord.ProcessedResources = this.GetProcessedItems(templateGenerator);

            string jsontext = JsonConvert.SerializeObject(telemetryrecord, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore });
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] data = encoding.GetBytes(jsontext);

            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("https://migaz.azurewebsites.net/api/v2");
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
// DialogResult dialogresult = MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
