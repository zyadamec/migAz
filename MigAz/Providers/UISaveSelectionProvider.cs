using Newtonsoft.Json;
using System.Windows.Forms;
using MigAz.Azure.Models;
using System.Collections.Generic;
using System.IO;
using System;
using System.Threading.Tasks;
using MigAz.Azure.Asm;
using MigAz.Azure.Interface;
using MigAz.Azure.Arm;
using MigAz.Azure;
using MigAz.Interface;
using MigAz.Azure;

namespace MigAz.Providers
{
    class UISaveSelectionProvider : ISaveSelectionProvider
    {
        string filePath;
        List<SaveSelection> saveSelections;

        public UISaveSelectionProvider()
        {
            string filedir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\MigAz";
            if (!Directory.Exists(filedir)) { Directory.CreateDirectory(filedir); }

            filePath = filedir + "\\MigAz-SaveSelection.json";
        }

        public async Task Save(Guid subscriptionId, List<TreeNode> selectedNodes)
        {
            string jsontext = String.Empty;

            try
            {
                StreamReader saveSelectionReader = new StreamReader(filePath);
                jsontext = saveSelectionReader.ReadToEnd();
                saveSelectionReader.Close();

                saveSelections = JsonConvert.DeserializeObject<List<SaveSelection>>(jsontext);

                // If save selection for this subscription alredy exists, remove it
                if (saveSelections.Exists(x => x.SubscriptionId == subscriptionId))
                {
                    saveSelections.Remove(saveSelections.Find(x => x.SubscriptionId == subscriptionId));
                }
            }
            catch
            {
                // If file does not exist, or invalid, starts a new object
                saveSelections = new List<SaveSelection>();
            }

            SaveSelection saveSelection = new SaveSelection();
            saveSelection.SubscriptionId = subscriptionId;

            saveSelection.VirtualNetworks = new List<SaveSelectionVirtualNetwork>();
            saveSelection.StorageAccounts = new List<SaveSelectioStorageAccount>();
            saveSelection.VirtualMachines = new List<SaveSelectionVirtualMachine>();

            if (selectedNodes != null)
            {
                foreach (TreeNode treeNode in selectedNodes)
                {
                    if (treeNode.Tag != null)
                    {
                        Type tagType = treeNode.Tag.GetType();
                        if (tagType == typeof(AsmVirtualNetwork))
                        {
                            AsmVirtualNetwork asmVirtualNetwork = (AsmVirtualNetwork)treeNode.Tag;

                            SaveSelectionVirtualNetwork saveSelectionVirtualNetwork = new SaveSelectionVirtualNetwork();
                            saveSelectionVirtualNetwork.VirtualNetworkName = asmVirtualNetwork.Name;

                            saveSelection.VirtualNetworks.Add(saveSelectionVirtualNetwork);
                        }
                        else if (tagType == typeof(AsmStorageAccount))
                        {
                            AsmStorageAccount asmStorageAccount = (AsmStorageAccount)treeNode.Tag;

                            SaveSelectioStorageAccount saveSelectionStorageAccount = new SaveSelectioStorageAccount();
                            saveSelectionStorageAccount.StorageAccountName = asmStorageAccount.Name;
                            saveSelectionStorageAccount.TargetStorageAccountName = asmStorageAccount.TargetName;

                            saveSelection.StorageAccounts.Add(saveSelectionStorageAccount);
                        }
                        else if (tagType == typeof(AsmVirtualMachine))
                        {
                            AsmVirtualMachine asmVirtualMachine = (AsmVirtualMachine)treeNode.Tag;

                            SaveSelectionVirtualMachine saveSelectionVirtualMachine = new SaveSelectionVirtualMachine();
                            saveSelectionVirtualMachine.CloudService = asmVirtualMachine.CloudServiceName;
                            saveSelectionVirtualMachine.VirtualMachine = asmVirtualMachine.RoleName;

                            if (asmVirtualMachine.TargetVirtualNetwork != null)
                                saveSelectionVirtualMachine.TargetVirtualNetwork = asmVirtualMachine.TargetVirtualNetwork.Id;

                            if (asmVirtualMachine.TargetSubnet != null)
                                saveSelectionVirtualMachine.TargetSubnet = asmVirtualMachine.TargetSubnet.Id;

                            // Add OS Disk Target Storage Account
                            if (asmVirtualMachine.OSVirtualHardDisk.TargetStorageAccount != null)
                                saveSelectionVirtualMachine.TargetDiskStorageAccounts.Add(asmVirtualMachine.OSVirtualHardDisk.DiskName, asmVirtualMachine.OSVirtualHardDisk.TargetStorageAccount.Id);

                            // Add OS Disk Target Storage Account
                            foreach (AsmDisk asmDataDisk in asmVirtualMachine.DataDisks)
                            {
                                if (asmDataDisk.TargetStorageAccount != null)
                                    saveSelectionVirtualMachine.TargetDiskStorageAccounts.Add(asmDataDisk.DiskName, asmDataDisk.TargetStorageAccount.Id);
                            }

                            saveSelection.VirtualMachines.Add(saveSelectionVirtualMachine);
                        }
                    }
                }
            }

            saveSelections.Add(saveSelection);

            // save blob copy details file
            jsontext = JsonConvert.SerializeObject(saveSelections, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore });

            StreamWriter saveSelectionWriter = new StreamWriter(filePath);
            saveSelectionWriter.Write(jsontext);
            saveSelectionWriter.Close();
        }

        public async Task Read(Guid subscriptionId, AzureRetriever sourceAzureRetreiver, AzureRetriever targetAzureRetreiver, TreeView treeView)
        {
            try
            {
                StreamReader saveSelectionReader = new StreamReader(filePath);
                string jsontext = saveSelectionReader.ReadToEnd();
                saveSelectionReader.Close();

                saveSelections = JsonConvert.DeserializeObject<List<SaveSelection>>(jsontext);
            }
            catch
            {
                // If file does not exist, or invalid, starts a new object
                saveSelections = new List<SaveSelection>();
            }

            if (saveSelections.Exists(x => x.SubscriptionId == subscriptionId))
            {
                SaveSelection saveSelection = saveSelections.Find(x => x.SubscriptionId == subscriptionId);

                foreach (SaveSelectionVirtualNetwork saveSelectionVirtualNetwork in saveSelection.VirtualNetworks)
                {
                    foreach (TreeNode treeNode in treeView.Nodes.Find(saveSelectionVirtualNetwork.VirtualNetworkName, true))
                    {
                        if (treeNode.Tag.GetType() == typeof(AsmVirtualNetwork))
                        {
                            AsmVirtualNetwork asmVirtualNetwork = (AsmVirtualNetwork)treeNode.Tag;

                            treeNode.Checked = true;
                        }
                    }
                }

                foreach (SaveSelectioStorageAccount saveSelectionStorageAccount in saveSelection.StorageAccounts)
                {
                    foreach (TreeNode treeNode in treeView.Nodes.Find(saveSelectionStorageAccount.StorageAccountName, true))
                    {
                        if (treeNode.Tag.GetType() == typeof(AsmStorageAccount))
                        {
                            AsmStorageAccount asmStorageAccount = (AsmStorageAccount)treeNode.Tag;
                            if (saveSelectionStorageAccount.TargetStorageAccountName.Length > 0) // We aren't going to reload a blank name, should it occur, as a name is required
                                asmStorageAccount.TargetName = saveSelectionStorageAccount.TargetStorageAccountName;
                            treeNode.Checked = true;
                        }
                    }
                }

                foreach (SaveSelectionVirtualMachine saveSelectionVirtualMachine in saveSelection.VirtualMachines)
                {
                    foreach (TreeNode virtualMachineNode in treeView.Nodes.Find(saveSelectionVirtualMachine.VirtualMachine, true))
                    {
                        if (virtualMachineNode.Tag != null)
                        {
                            if (virtualMachineNode.Tag.GetType() == typeof(AsmVirtualMachine))
                            {
                                AsmVirtualMachine asmVirtualMachine = (AsmVirtualMachine)virtualMachineNode.Tag;

                                if (asmVirtualMachine.CloudServiceName == saveSelectionVirtualMachine.CloudService && asmVirtualMachine.RoleName == saveSelectionVirtualMachine.VirtualMachine)
                                {
                                    asmVirtualMachine.TargetVirtualNetwork = SeekVirtualNetwork(saveSelectionVirtualMachine.TargetVirtualNetwork, await sourceAzureRetreiver.GetAzureAsmVirtualNetworks(), await targetAzureRetreiver.GetAzureARMVirtualNetworks());
                                    asmVirtualMachine.TargetSubnet = SeekSubnet(saveSelectionVirtualMachine.TargetSubnet, asmVirtualMachine.TargetVirtualNetwork);

                                    if (saveSelectionVirtualMachine.TargetDiskStorageAccounts.ContainsKey(asmVirtualMachine.OSVirtualHardDisk.DiskName))
                                        asmVirtualMachine.OSVirtualHardDisk.TargetStorageAccount = SeekStorageAccount(saveSelectionVirtualMachine.TargetDiskStorageAccounts[asmVirtualMachine.OSVirtualHardDisk.DiskName].ToString(), await sourceAzureRetreiver.GetAzureAsmStorageAccounts(), await targetAzureRetreiver.GetAzureARMStorageAccounts());

                                    foreach (AsmDisk asmDataDisk in asmVirtualMachine.DataDisks)
                                    {
                                        if (saveSelectionVirtualMachine.TargetDiskStorageAccounts.ContainsKey(asmDataDisk.DiskName))
                                            asmDataDisk.TargetStorageAccount = SeekStorageAccount(saveSelectionVirtualMachine.TargetDiskStorageAccounts[asmDataDisk.DiskName].ToString(), await sourceAzureRetreiver.GetAzureAsmStorageAccounts(), await targetAzureRetreiver.GetAzureARMStorageAccounts());
                                    }

                                    virtualMachineNode.Checked = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        private IVirtualNetwork SeekVirtualNetwork(string id, List<AsmVirtualNetwork> asmVirtualNetworks, List<ArmVirtualNetwork> armVirtualNetworks)
        {
            if (asmVirtualNetworks != null)
            {
                foreach (AsmVirtualNetwork asmVirtualNetwork in asmVirtualNetworks)
                {
                    if (asmVirtualNetwork.Id == id)
                        return asmVirtualNetwork;
                }
            }

            if (armVirtualNetworks != null)
            {
                foreach (ArmVirtualNetwork armVirtualNetwork in armVirtualNetworks)
                {
                    if (armVirtualNetwork.Id == id)
                        return armVirtualNetwork;
                }
            }

            return null;
        }

        private ISubnet SeekSubnet(string id, IVirtualNetwork virtualNetwork)
        {
            if (virtualNetwork != null)
            {
                foreach (ISubnet subnet in virtualNetwork.Subnets)
                {
                    if (subnet.Id == id)
                        return subnet;
                }
            }

            return null;
        }

        private IStorageAccount SeekStorageAccount(string id, List<AsmStorageAccount> asmStorageAccounts, List<ArmStorageAccount> armStorageAccounts)
        {
            if (asmStorageAccounts != null)
            {
                foreach (AsmStorageAccount asmStorageAccount in asmStorageAccounts)
                {
                    if (asmStorageAccount.Id == id)
                        return asmStorageAccount;
                }
            }

            if (armStorageAccounts != null)
            {
                foreach (ArmStorageAccount armStorageAccount in armStorageAccounts)
                {
                    if (armStorageAccount.Id == id)
                        return armStorageAccount;
                }
            }

            return null;
        }
    }
}
