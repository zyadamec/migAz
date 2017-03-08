using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MigAz.Tests.Fakes;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Threading.Tasks;
using MigAz.Azure.Arm;
using MigAz.Azure.Models;
using MigAz.Azure.Generator;
using MigAz.Azure;
using MigAz.Azure.Generator.AsmToArm;

namespace MigAz.Tests
{
    [TestClass]
    public class VirtualMachineTests
    {
        
        private async Task<JObject> GenerateSingleVMTemplate()

        {
            AzureContext azureContextUSCommercial = TestHelper.SetupAzureContext();
            AsmToArmGenerator templateGenerator = TestHelper.SetupTemplateGenerator(azureContextUSCommercial);
            FakeAzureRetriever azureContextUSCommercialRetriever = (FakeAzureRetriever)azureContextUSCommercial.AzureRetriever;
            azureContextUSCommercialRetriever.LoadDocuments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestDocs\\VM1"));

            var artifacts = new AsmArtifacts();
            artifacts.VirtualMachines.Add((await azureContextUSCommercialRetriever.GetAzureAsmCloudServices())[0].VirtualMachines[0]);
            TestHelper.SetTargetSubnets(artifacts);

            AsmToArmGenerator tempalteResult = await templateGenerator.GenerateTemplate(TestHelper.GetTestAzureSubscription(), TestHelper.GetTestAzureSubscription(), artifacts, await TestHelper.GetTargetResourceGroup(azureContextUSCommercial), AppDomain.CurrentDomain.BaseDirectory);

            return tempalteResult.GenerateTemplate();
        }

        [TestMethod]
        public async Task VMDiskUrlsAreCorrectlyUpdated()
        {
            var templateJson = await GenerateSingleVMTemplate();
            var vmResource = templateJson["resources"].Where(j => j["type"].Value<string>() == "Microsoft.Compute/virtualMachines").Single();
            Assert.AreEqual("myservice-vm", vmResource["name"]);

            var osDisk = vmResource["properties"]["storageProfile"]["osDisk"];
            Assert.AreEqual("https://mystoragev2.blob.core.windows.net/vhds/myservice-myservice-os-1445207070064.vhd", osDisk["vhd"]["uri"].Value<string>());
        }

        [TestMethod]
        public async Task AvailabilitySetNameIsBasedOnCloudServceName()
        {
            var templateJson = await GenerateSingleVMTemplate();

            string expectedASName = "myservice";
            string expectedASId = $"[concat(" + ArmConst.ResourceGroupId + ", '" + ArmConst.ProviderAvailabilitySets + expectedASName + "')]";

            var vmResource = templateJson["resources"].Where(j => j["type"].Value<string>() == "Microsoft.Compute/virtualMachines").Single();
            Assert.AreEqual(expectedASId, vmResource["properties"]["availabilitySet"]["id"].Value<string>());
            Assert.AreEqual(expectedASId, vmResource["dependsOn"][1].Value<string>());

            var asResource = templateJson["resources"].Where(j => j["type"].Value<string>() == "Microsoft.Compute/availabilitySets").Single();
            Assert.AreEqual(expectedASName, asResource["name"].Value<string>());
        }

        [TestMethod]
        public async Task ValidateSingleVMWithDataDisksNotInVnet()
        {
            AzureContext azureContextUSCommercial = TestHelper.SetupAzureContext();
            AsmToArmGenerator templateGenerator = TestHelper.SetupTemplateGenerator(azureContextUSCommercial);
            FakeAzureRetriever azureContextUSCommercialRetriever = (FakeAzureRetriever)azureContextUSCommercial.AzureRetriever;
            azureContextUSCommercialRetriever.LoadDocuments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestDocs\\VM2"));

            var artifacts = new AsmArtifacts();
            artifacts.VirtualMachines.Add((await azureContextUSCommercialRetriever.GetAzureAsmCloudServices())[0].VirtualMachines[0]);
            TestHelper.SetTargetSubnets(artifacts);

            AsmToArmGenerator templateResult = await templateGenerator.GenerateTemplate(TestHelper.GetTestAzureSubscription(), TestHelper.GetTestAzureSubscription(), artifacts, await TestHelper.GetTargetResourceGroup(azureContextUSCommercial), AppDomain.CurrentDomain.BaseDirectory);

            JObject templateJson = templateResult.GenerateTemplate();

            // Validate VNET
            var vnets = templateJson["resources"].Children().Where(
                r => r["type"].Value<string>() == "Microsoft.Network/virtualNetworks");
            Assert.AreEqual(0, vnets.Count());

            // Validate VM
            var vmResource = templateJson["resources"].Where(
                j => j["type"].Value<string>() == "Microsoft.Compute/virtualMachines").Single();
            Assert.AreEqual("myasmvm-vm", vmResource["name"].Value<string>());

            // Validate disks
            var dataDisks = (JArray)vmResource["properties"]["storageProfile"]["dataDisks"];
            Assert.AreEqual(2, dataDisks.Count);
            Assert.AreEqual("Disk1", dataDisks[0]["name"].Value<string>());
            Assert.AreEqual("Disk2", dataDisks[1]["name"].Value<string>());
        }
        [TestMethod]
        public async Task ValidateVMInVnetButNotInTargetVNet()
        {
            AzureContext azureContextUSCommercial = TestHelper.SetupAzureContext();
            AsmToArmGenerator templateGenerator = TestHelper.SetupTemplateGenerator(azureContextUSCommercial);
            FakeAzureRetriever azureContextUSCommercialRetriever = (FakeAzureRetriever)azureContextUSCommercial.AzureRetriever;
            azureContextUSCommercialRetriever.LoadDocuments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestDocs\\VM3"));

            var artifacts = new AsmArtifacts();
            artifacts.VirtualMachines.Add((await azureContextUSCommercialRetriever.GetAzureAsmCloudServices())[0].VirtualMachines[0]);

            bool exceptionThrown = false;
            try
            {
                AsmToArmGenerator templateResult = await templateGenerator.GenerateTemplate(TestHelper.GetTestAzureSubscription(), TestHelper.GetTestAzureSubscription(), artifacts, await TestHelper.GetTargetResourceGroup(azureContextUSCommercial), AppDomain.CurrentDomain.BaseDirectory);
            }
            catch (Exception exc)
            {
                if (exc.GetType() == typeof(ArgumentException) && exc.Message == "Target Virtual Network for ASM Virtual Machine 'VM3' must be specified.")
                    exceptionThrown = true;
            }

            Assert.IsFalse(!exceptionThrown, "Did not receive Null Target Virtual Network Argument Exception");
        }

        [TestMethod]
        public async Task ValidateVMInExistingArmSubnetId()
        {
            AzureContext azureContextUSCommercial = TestHelper.SetupAzureContext();
            AsmToArmGenerator templateGenerator = TestHelper.SetupTemplateGenerator(azureContextUSCommercial);
            FakeAzureRetriever azureContextUSCommercialRetriever = (FakeAzureRetriever)azureContextUSCommercial.AzureRetriever;
            azureContextUSCommercialRetriever.LoadDocuments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestDocs\\VM3"));

            var artifacts = new AsmArtifacts();
            artifacts.VirtualMachines.Add((await azureContextUSCommercialRetriever.GetAzureAsmCloudServices())[0].VirtualMachines[0]);
            TestHelper.SetTargetSubnets(artifacts);

            AsmToArmGenerator templateReault = await templateGenerator.GenerateTemplate(TestHelper.GetTestAzureSubscription(), TestHelper.GetTestAzureSubscription(), artifacts, await TestHelper.GetTargetResourceGroup(azureContextUSCommercial), AppDomain.CurrentDomain.BaseDirectory);
            JObject templateJson = templateReault.GenerateTemplate();

            // Validate VM
            var vmResource = templateJson["resources"].Where(
                j => j["type"].Value<string>() == "Microsoft.Compute/virtualMachines").Single();
            Assert.AreEqual("VM3-vm", vmResource["name"].Value<string>());
            StringAssert.Contains(vmResource["properties"]["networkProfile"]["networkInterfaces"][0]["id"].Value<string>(),
                "'" + ArmConst.ProviderNetworkInterfaces + "VM3-nic'");

            // Validate NIC
            var nicResource = templateJson["resources"].Where(
                j => j["type"].Value<string>() == "Microsoft.Network/networkInterfaces").Single();
            Assert.AreEqual("VM3-nic", nicResource["name"].Value<string>());
            StringAssert.Contains(nicResource["properties"]["ipConfigurations"][0]["properties"]["subnet"]["id"].Value<string>(),
                "/subscriptions/22222222-2222-2222-2222-222222222222/resourceGroups/dummygroup-rg/providers/Microsoft.Network/virtualNetworks/DummyVNet/subnets/subnet01");
        }

        [TestMethod]
        public async Task ValidateDiskNamescapeChangeAcrossAzureEnvironments()
        {
            AzureContext azureContextUSCommercial = TestHelper.SetupAzureContext();
            AzureContext azureContextUSGovernment = TestHelper.SetupAzureContext(AzureEnvironment.AzureUSGovernment);
            AsmToArmGenerator templateGenerator = TestHelper.SetupTemplateGenerator(azureContextUSCommercial);
            FakeAzureRetriever azureContextUSCommercialRetriever = (FakeAzureRetriever)azureContextUSCommercial.AzureRetriever;
            azureContextUSCommercialRetriever.LoadDocuments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestDocs\\VM3"));
            FakeAzureRetriever azureContextUSGovernmentRetriever = (FakeAzureRetriever)azureContextUSGovernment.AzureRetriever;
            azureContextUSGovernmentRetriever.LoadDocuments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestDocs\\i"));

            var artifacts = new AsmArtifacts();
            artifacts.VirtualMachines.Add((await azureContextUSCommercialRetriever.GetAzureAsmCloudServices())[0].VirtualMachines[0]);

            artifacts.VirtualMachines[0].OSVirtualHardDisk.TargetStorageAccount = await azureContextUSGovernmentRetriever.GetAzureAsmStorageAccount("targetstorage");

            Assert.AreEqual(artifacts.VirtualMachines[0].OSVirtualHardDisk.MediaLink, "https://mystorage.blob.core.windows.net/vhds/mydisk.vhd");
            Assert.AreEqual(artifacts.VirtualMachines[0].OSVirtualHardDisk.TargetMediaLink, "https://targetstoragev2.blob.core.usgovcloudapi.net/vhds/mydisk.vhd");
        }
    }
}
