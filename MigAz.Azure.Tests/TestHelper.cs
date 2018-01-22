using MigAz.Azure;
using MigAz.Tests.Fakes;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigAz.Azure.Asm;
using MigAz.Azure.Arm;
using MigAz.Azure.Generator.AsmToArm;
using MigAz.Core.Interface;
using MigAz.Core.Generator;
using MIGAZ.Tests.Fakes;
using MigAz.Core;

namespace MigAz.Tests
{
    static class TestHelper
    {
        public const string TenantId = "11111111-1111-1111-1111-111111111111";
        public const string SubscriptionId = "22222222-2222-2222-2222-222222222222";
        public static ISubscription GetTestAzureSubscription()
        {
            return new FakeAzureSubscription();
        }
        public static async Task<AzureContext> SetupAzureContext(string restResponseFile)
        {
            return await SetupAzureContext(AzureEnvironment.AzureCloud, restResponseFile);
        }

        public static async Task<AzureContext> SetupAzureContext(AzureEnvironment azureEnvironment, string restResponseFile)
        {
            ILogProvider logProvider = new FakeLogProvider();
            IStatusProvider statusProvider = new FakeStatusProvider();
            TargetSettings settingsProvider = new FakeSettingsProvider().GetTargetSettings();
            AzureContext azureContext = new AzureContext(logProvider, statusProvider, settingsProvider);
            azureContext.AzureEnvironment = azureEnvironment;
            azureContext.TokenProvider = new FakeTokenProvider();
            azureContext.AzureRetriever = new TestRetriever(azureContext);
            azureContext.AzureRetriever.LoadRestCache(restResponseFile);

            List<AzureSubscription> subscriptions = await azureContext.AzureRetriever.GetAzureARMSubscriptions();
            await azureContext.SetSubscriptionContext(subscriptions[0]);
            await azureContext.AzureRetriever.SetSubscriptionContext(subscriptions[0]);

            return azureContext;
        }
       
        public static async Task<AzureGenerator> SetupTemplateGenerator(AzureContext azureContext)
        {
            ITelemetryProvider telemetryProvider = new FakeTelemetryProvider();
            return new AzureGenerator(TestHelper.GetTestAzureSubscription(), TestHelper.GetTestAzureSubscription(), azureContext.LogProvider, azureContext.StatusProvider, telemetryProvider);
        }

        public static JObject GetJsonData(MemoryStream closedStream)
        {
            var newStream = new MemoryStream(closedStream.ToArray());
            var reader = new StreamReader(newStream);
            var templateText = reader.ReadToEnd();
            return JObject.Parse(templateText);
        }

        internal static async Task<Azure.MigrationTarget.ResourceGroup> GetTargetResourceGroup(AzureContext azureContext)
        {
            List<Azure.Arm.Location> azureLocations = await azureContext.AzureSubscription.GetAzureARMLocations();
            Azure.MigrationTarget.ResourceGroup targetResourceGroup = new Azure.MigrationTarget.ResourceGroup();
            targetResourceGroup.TargetLocation = azureLocations[0];
            return targetResourceGroup;
        }
    }
}
