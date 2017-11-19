using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MigAz.Azure.Generator.AsmToArm;
using MigAz.Tests.Fakes;
using System.IO;
using MigAz.Azure.Models;
using System.Xml;
using System.Collections;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Threading.Tasks;
using MigAz.Azure;
using MigAz.Azure.Generator;
using MigAz.Core.Generator;
using MIGAZ.Tests.Fakes;
using System.Collections.Generic;

namespace MigAz.Tests
{
    [TestClass]
    public class StorageTests
    {
        [TestMethod]
        public async Task LoadASMObjectsFromSampleOfflineFile()
        {
            string restResponseFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestDocs\\NewTest1\\AsmObjectsOffline.json");
            AzureContext azureContextUSCommercial = await TestHelper.SetupAzureContext(restResponseFile);
            await azureContextUSCommercial.AzureRetriever.BindAsmResources();

            AzureGenerator templateGenerator = await TestHelper.SetupTemplateGenerator(azureContextUSCommercial);

            var artifacts = new ExportArtifacts();
            artifacts.ResourceGroup = await TestHelper.GetTargetResourceGroup(azureContextUSCommercial);
            //foreach (Azure.MigrationTarget.StorageAccount s in azureContextUSCommercial.AzureRetriever.AsmTargetStorageAccounts)
            //{
            //    artifacts.StorageAccounts.Add(s);
            //}

            await templateGenerator.UpdateArtifacts(artifacts);
            Assert.IsFalse(templateGenerator.HasErrors, "Template Generation cannot occur as the are error(s).");

            await templateGenerator.GenerateStreams();

            JObject templateJson = JObject.Parse(await templateGenerator.GetTemplateString());

            Assert.AreEqual(0, templateJson["resources"].Children().Count());

            //var resource = templateJson["resources"].Single();
            //Assert.AreEqual("Microsoft.Storage/storageAccounts", resource["type"].Value<string>());
            //Assert.AreEqual("asmtest8155v2", resource["name"].Value<string>());
            //Assert.AreEqual("[resourceGroup().location]", resource["location"].Value<string>());
            //Assert.AreEqual("Standard_LRS", resource["properties"]["accountType"].Value<string>());
        }

        [TestMethod]
        public async Task LoadARMObjectsFromSampleOfflineFile()
        {
            string restResponseFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestDocs\\NewTest1\\ArmObjectsOffline.json");
            AzureContext azureContextUSCommercial = await TestHelper.SetupAzureContext(restResponseFile);
            await azureContextUSCommercial.AzureRetriever.BindArmResources();

            AzureGenerator templateGenerator = await TestHelper.SetupTemplateGenerator(azureContextUSCommercial);

            var artifacts = new ExportArtifacts();
            artifacts.ResourceGroup = await TestHelper.GetTargetResourceGroup(azureContextUSCommercial);

            //foreach (Azure.MigrationTarget.StorageAccount s in azureContextUSCommercial.AzureRetriever.ArmTargetStorageAccounts)
            //{
            //    artifacts.StorageAccounts.Add(s);
            //}

            await templateGenerator.UpdateArtifacts(artifacts);
            Assert.IsFalse(templateGenerator.HasErrors, "Template Generation cannot occur as the are error(s).");

            await templateGenerator.GenerateStreams();

            JObject templateJson = JObject.Parse(await templateGenerator.GetTemplateString());

            Assert.AreEqual(0, templateJson["resources"].Children().Count());

            //var resource = templateJson["resources"].First();
            //Assert.AreEqual("Microsoft.Storage/storageAccounts", resource["type"].Value<string>());
            //Assert.AreEqual("manageddiskdiag857v2", resource["name"].Value<string>());
            //Assert.AreEqual("[resourceGroup().location]", resource["location"].Value<string>());
            //Assert.AreEqual("Standard_LRS", resource["properties"]["accountType"].Value<string>());
        }
    [TestMethod]
        public async Task LoadARMObjectsFromSampleOfflineFile2()
        {
            string restResponseFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestDocs\\NewTest1\\temp.json");
            AzureContext azureContextUSCommercial = await TestHelper.SetupAzureContext(Core.Interface.AzureEnvironment.AzureCloud, restResponseFile);
            await azureContextUSCommercial.AzureRetriever.BindArmResources();

            AzureGenerator templateGenerator = await TestHelper.SetupTemplateGenerator(azureContextUSCommercial);

            var artifacts = new ExportArtifacts();
            artifacts.ResourceGroup = await TestHelper.GetTargetResourceGroup(azureContextUSCommercial);


            artifacts.VirtualMachines.Add(azureContextUSCommercial.AzureRetriever.ArmTargetVirtualMachines[0]);
            artifacts.VirtualMachines[0].OSVirtualHardDisk.DiskSizeInGB = 128;

            await templateGenerator.UpdateArtifacts(artifacts);

            Assert.IsNotNull(templateGenerator.SeekAlert("Network Interface Card (NIC) 'manageddisk01549-nic' utilizes Network Security Group (NSG) 'ManagedDisk01-nsg-nsg', but the NSG resource is not added into the migration template."));
            artifacts.NetworkSecurityGroups.Add(azureContextUSCommercial.AzureRetriever.ArmTargetNetworkSecurityGroups[0]);
            await templateGenerator.UpdateArtifacts(artifacts);
            Assert.IsNull(templateGenerator.SeekAlert("Network Interface Card (NIC) 'manageddisk01549-nic' utilizes Network Security Group (NSG) 'ManagedDisk01-nsg-nsg', but the NSG resource is not added into the migration template."));

            Assert.IsNotNull(templateGenerator.SeekAlert("Target Virtual Network 'ManagedDiskvnet-vnet' for Virtual Machine 'ManagedDisk01-vm' Network Interface 'manageddisk01549-nic' is invalid, as it is not included in the migration / template."));
            artifacts.VirtualNetworks.Add(azureContextUSCommercial.AzureRetriever.ArmTargetVirtualNetworks[0]);
            await templateGenerator.UpdateArtifacts(artifacts);
            Assert.IsNull(templateGenerator.SeekAlert("Target Virtual Network 'ManagedDiskvnet-vnet' for Virtual Machine 'ManagedDisk01-vm' Network Interface 'manageddisk01549-nic' is invalid, as it is not included in the migration / template."));

            Assert.IsNotNull(templateGenerator.SeekAlert("Network Interface Card (NIC) 'manageddisk01549-nic' IP Configuration 'ipconfig1' utilizes Public IP 'ManagedDisk01-ip', but the Public IP resource is not added into the migration template."));
            artifacts.PublicIPs.Add(azureContextUSCommercial.AzureRetriever.ArmTargetPublicIPs[0]);
            await templateGenerator.UpdateArtifacts(artifacts);
            Assert.IsNull(templateGenerator.SeekAlert("Network Interface Card (NIC) 'manageddisk01549-nic' IP Configuration 'ipconfig1' utilizes Public IP 'ManagedDisk01-ip', but the Public IP resource is not added into the migration template."));

            Assert.IsNotNull(templateGenerator.SeekAlert("Virtual Machine 'ManagedDisk01' references Managed Disk 'ManagedDisk01_OsDisk_1_e901d155e5404b6a912afb22e7a804a6' which has not been added as an export resource."));
            artifacts.Disks.Add(azureContextUSCommercial.AzureRetriever.ArmTargetManagedDisks[1]);
            await templateGenerator.UpdateArtifacts(artifacts);
            Assert.IsNull(templateGenerator.SeekAlert("Virtual Machine 'ManagedDisk01' references Managed Disk 'ManagedDisk01_OsDisk_1_e901d155e5404b6a912afb22e7a804a6' which has not been added as an export resource."));

            Assert.IsNotNull(templateGenerator.SeekAlert("Virtual Machine 'ManagedDisk01' references Managed Disk 'ManagedDataDisk01' which has not been added as an export resource."));
            artifacts.Disks.Add(azureContextUSCommercial.AzureRetriever.ArmTargetManagedDisks[0]);
            await templateGenerator.UpdateArtifacts(artifacts);
            Assert.IsNull(templateGenerator.SeekAlert("Virtual Machine 'ManagedDisk01' references Managed Disk 'ManagedDataDisk01' which has not been added as an export resource."));

            Assert.IsNotNull(templateGenerator.SeekAlert("Network Interface Card (NIC) 'manageddisk01549-nic' is used by Virtual Machine 'ManagedDisk01-vm', but is not included in the exported resources."));
            artifacts.NetworkInterfaces.Add(azureContextUSCommercial.AzureRetriever.ArmTargetNetworkInterfaces[0]);
            await templateGenerator.UpdateArtifacts(artifacts);
            Assert.IsNull(templateGenerator.SeekAlert("Network Interface Card (NIC) 'manageddisk01549-nic' is used by Virtual Machine 'ManagedDisk01-vm', but is not included in the exported resources."));

            Assert.IsFalse(templateGenerator.HasErrors, "Template Generation cannot occur as the are error(s).");

            await templateGenerator.GenerateStreams();

            JObject templateJson = JObject.Parse(await templateGenerator.GetTemplateString());

            Assert.AreEqual(6, templateJson["resources"].Children().Count());

        //    var resource = templateJson["resources"].First();
        //    Assert.AreEqual("Microsoft.Storage/storageAccounts", resource["type"].Value<string>());
        //    Assert.AreEqual("manageddiskdiag857v2", resource["name"].Value<string>());
        //    Assert.AreEqual("[resourceGroup().location]", resource["location"].Value<string>());
        //    Assert.AreEqual("Standard_LRS", resource["properties"]["accountType"].Value<string>());
        }
    }
}
