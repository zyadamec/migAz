using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MIGAZ.Generator;
using MIGAZ.Tests.Fakes;
using System.IO;
using MIGAZ.Models;
using System.Xml;
using System.Collections;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Threading.Tasks;
using MIGAZ.Azure;

namespace MIGAZ.Tests
{
    [TestClass]
    public class StorageTests
    {
        [TestMethod]
        public async Task ValidateSingleStorageAccount()
        {
            AzureContext azureContextUSCommercial = TestHelper.SetupAzureContext();
            TemplateGenerator templateGenerator = TestHelper.SetupTemplateGenerator(azureContextUSCommercial);
            FakeAzureRetriever azureContextUSCommercialRetriever = (FakeAzureRetriever)azureContextUSCommercial.AzureRetriever;
            azureContextUSCommercialRetriever.LoadDocuments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestDocs\\Storage1"));
            
            var artifacts = new AsmArtifacts();
            artifacts.StorageAccounts.Add(await azureContextUSCommercialRetriever.GetAzureAsmStorageAccount("mystorage"));

            TemplateResult templateResult = await templateGenerator.GenerateTemplate(TestHelper.GetTestAzureSubscription(), TestHelper.GetTestAzureSubscription(), artifacts, await TestHelper.GetTargetResourceGroup(azureContextUSCommercial), AppDomain.CurrentDomain.BaseDirectory);

            JObject templateJson = templateResult.GenerateTemplate();
            Assert.AreEqual(1, templateJson["resources"].Children().Count());
            var resource = templateJson["resources"].Single();
            Assert.AreEqual("Microsoft.Storage/storageAccounts", resource["type"].Value<string>());
            Assert.AreEqual("mystoragev2", resource["name"].Value<string>());
            Assert.AreEqual((await azureContextUSCommercialRetriever.GetAzureASMLocations())[0].Name, resource["location"].Value<string>());
            Assert.AreEqual("Standard_LRS", resource["properties"]["accountType"].Value<string>());

        }
    }
}
