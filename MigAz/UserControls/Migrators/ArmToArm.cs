using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using MigAz.Azure;
using MigAz.Models;
using MigAz.Providers;
using MigAz.Core.Interface;
using MigAz.Azure.Arm;

namespace MigAz.UserControls.Migrators
{
    public partial class ArmToArm : IMigratorUserControl
    {
        private AzureRetriever _AzureRetriever;
        private AsmArtefacts _asmArtefacts;
        private AppSettingsProvider _appSettingsProvider;
        private MigAz.Forms.ARM.Providers.UISaveSelectionProvider _saveSelectionProvider;
        private MigAz.Forms.ARM.Interface.ITelemetryProvider _telemetryProvider;
        private AzureContext _AzureContextARM;
        private TreeNode _sourceCascadeNode;

        public ArmToArm() : base(null, null) { }

        public ArmToArm(IStatusProvider statusProvider, ILogProvider logProvider)
            : base(statusProvider, logProvider)
        {
            InitializeComponent();

            _saveSelectionProvider = new MigAz.Forms.ARM.Providers.UISaveSelectionProvider();
            _telemetryProvider = new MigAz.Forms.ARM.Providers.CloudTelemetryProvider();
            _appSettingsProvider = new AppSettingsProvider();

            _AzureContextARM = new AzureContext(LogProvider, StatusProvider, _appSettingsProvider);
            _AzureContextARM.AfterAzureSubscriptionChange += _AzureContextARM_AfterAzureSubscriptionChange;
        }

        private async void ArmToArm_Load(object sender, EventArgs e)
        {
            LogProvider.WriteLog("ArmToArm_Load", "Program start");

            NewVersionAvailable(); // check if there a new version of the app
            await this.azureLoginContextViewer1.Bind(_AzureContextARM);
        }

        public AzureContext AzureContextARM
        {
            get { return _AzureContextARM; }
        }

        private async Task _AzureContextARM_AfterAzureSubscriptionChange(AzureContext sender)
        {
            ResetForm();

            if (sender.AzureSubscription != null)
            {
                //btnAzureContextARM.Enabled = true;

                TreeNode subscriptionNode = new TreeNode(sender.AzureSubscription.Name);
                treeSource.Nodes.Add(subscriptionNode);
                subscriptionNode.Expand();

                foreach (ResourceGroup armResourceGroup in await _AzureContextARM.AzureRetriever.GetAzureARMResourceGroups())
                {

                }

                List<VirtualNetwork> armVirtualNetworks = await _AzureContextARM.AzureRetriever.GetAzureARMVirtualNetworks();
                foreach (VirtualNetwork armVirtualNetwork in armVirtualNetworks)
                {
                    if (armVirtualNetwork.HasNonGatewaySubnet)
                    {
                        TreeNode parentNode = MigAz.Core.TreeView.GetDataCenterTreeViewNode(subscriptionNode, armVirtualNetwork.Location, "Virtual Networks");
                        TreeNode tnVirtualNetwork = new TreeNode(armVirtualNetwork.Name);
                        tnVirtualNetwork.Name = armVirtualNetwork.Name;
                        tnVirtualNetwork.Tag = armVirtualNetwork;
                        parentNode.Nodes.Add(tnVirtualNetwork);
                        parentNode.Expand();
                    }
                }

                foreach (StorageAccount armStorageAccount in await _AzureContextARM.AzureRetriever.GetAzureARMStorageAccounts())
                {
                    TreeNode parentNode = MigAz.Core.TreeView.GetDataCenterTreeViewNode(subscriptionNode, armStorageAccount.PrimaryLocation, "Storage Accounts");
                    TreeNode tnStorageAccount = new TreeNode(armStorageAccount.Name);
                    tnStorageAccount.Name = tnStorageAccount.Text;
                    tnStorageAccount.Tag = armStorageAccount;
                    parentNode.Nodes.Add(tnStorageAccount);
                    parentNode.Expand();
                }

                foreach (VirtualMachine armVirtualMachine in await _AzureContextARM.AzureRetriever.GetAzureArmVirtualMachines())
                {
                    TreeNode parentNode = MigAz.Core.TreeView.GetDataCenterTreeViewNode(subscriptionNode, armVirtualMachine.Location, "Virtual Machines");
                    TreeNode tnVirtualMachine = new TreeNode(armVirtualMachine.Name);
                    tnVirtualMachine.Name = tnVirtualMachine.Text;
                    tnVirtualMachine.Tag = armVirtualMachine;
                    parentNode.Nodes.Add(tnVirtualMachine);
                    parentNode.Expand();
                }




                //List<AsmCloudService> asmCloudServices = await _AzureContextARM.AzureRetriever.GetAzureAsmCloudServices();
                //foreach (AsmCloudService asmCloudService in asmCloudServices)
                //{
                //    foreach (AsmVirtualMachine asmVirtualMachine in asmCloudService.VirtualMachines)
                //    {
                //        TreeNode parentNode = GetDataCenterTreeViewNode(subscriptionNode, asmCloudService.Location, "Cloud Services");
                //        TreeNode[] cloudServiceNodeSearch = parentNode.Nodes.Find(asmCloudService.ServiceName, false);
                //        TreeNode cloudServiceNode = null;
                //        if (cloudServiceNodeSearch.Count() == 1)
                //        {
                //            cloudServiceNode = cloudServiceNodeSearch[0];
                //        }

                //        if (cloudServiceNode == null)
                //        {
                //            cloudServiceNode = new TreeNode(asmCloudService.ServiceName);
                //            cloudServiceNode.Name = asmCloudService.ServiceName;
                //            cloudServiceNode.Tag = asmCloudService;
                //            parentNode.Nodes.Add(cloudServiceNode);
                //            parentNode.Expand();
                //        }

                //        TreeNode virtualMachineNode = new TreeNode(asmVirtualMachine.RoleName);
                //        virtualMachineNode.Name = asmVirtualMachine.RoleName;
                //        virtualMachineNode.Tag = asmVirtualMachine;
                //        cloudServiceNode.Nodes.Add(virtualMachineNode);
                //        cloudServiceNode.Expand();
                //    }
                //}

                //foreach (AsmNetworkSecurityGroup asmNetworkSecurityGroup in await _AzureContextSourceASM.AzureRetriever.GetAzureAsmNetworkSecurityGroups())
                //{
                //    TreeNode parentNode = GetDataCenterTreeViewNode(subscriptionNode, asmNetworkSecurityGroup.Location, "Network Security Groups");
                //    TreeNode tnStorageAccount = new TreeNode(asmNetworkSecurityGroup.Name);
                //    tnStorageAccount.Name = tnStorageAccount.Text;
                //    tnStorageAccount.Tag = asmNetworkSecurityGroup;
                //    parentNode.Nodes.Add(tnStorageAccount);
                //    parentNode.Expand();
                //}

                //subscriptionNode.ExpandAll();
                //await ReadSubscriptionSettings(sender.AzureSubscription);

                treeSource.Enabled = true;
            }

            StatusProvider.UpdateStatus("Ready");
        }

        private void ResetForm()
        {
            treeSource.Nodes.Clear();
            //_SelectedNodes.Clear();
            UpdateExportItemsCount();
            //ClearAzureResourceManagerProperties();
            treeSource.Enabled = false;
        }

        private void Window_FormClosing(object sender, FormClosingEventArgs e)
        {
            // If save selection option is enabled
            if (app.Default.SaveSelection)
            {
                try
                {
                    // TODO
                    //_saveSelectionProvider.Save(Guid.Parse(subscriptionid), lvwVirtualNetworks, lvwStorageAccounts, lvwVirtualMachines);
                }
                catch
                { //Ignore save selection if no objects are selected
                }
            }
        }

        private async void cmbSubscriptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            // TODO, move to User Controls

            //if (cmbSubscriptions.Enabled == true)
            //{

            //    lvwVirtualNetworks.Items.Clear();
            //    lvwStorageAccounts.Items.Clear();
            //    lvwVirtualMachines.Items.Clear();

            //    AzureSubscription selectedAzureSubscription = (AzureSubscription)cmbSubscriptions.SelectedItem;
            //    var token = GetToken(subscriptionsAndTenants[subscriptionid], PromptBehavior.Auto);


            //    //Resource Group listing
            //    foreach (ArmResourceGroup resourceGroup in await _AzureRetriever.GetAzureARMResourceGroups())
            //    {
            //        //Listing Virtual Network from each RG
            //        foreach (ArmVirtualNetwork armVirtualNetwork in await _AzureRetriever.GetAzureARMVirtualNetworks()) // todo, by resource group
            //        {
            //            var listItem = new ListViewItem(resourceGroup.Name);
            //            listItem.SubItems.AddRange(new[] { armVirtualNetwork.Name });
            //            lvwVirtualNetworks.Items.Add(listItem);
            //            Application.DoEvents();
            //        }

            //        //Listing Storage Account from each RG
            //        foreach (ArmStorageAccount armStorageAccount in await _AzureRetriever.GetAzureARMStorageAccounts()) // todo, by resource group
            //        {
            //            var listItem = new ListViewItem(resourceGroup.Name);
            //            listItem.SubItems.AddRange(new[] { armStorageAccount.Name });
            //            lvwStorageAccounts.Items.Add(listItem);
            //            Application.DoEvents();
            //        }

            //        //Listing Virtual Machines from each RG
            //        foreach (ArmVirtualMachine armVirtualMachine in await _AzureRetriever.GetAzureARMVirtualMachines()) // todo, by resource group
            //        {
            //            var listItem = new ListViewItem(resourceGroup.Name);
            //            listItem.SubItems.AddRange(new[] { armVirtualMachine.Name });
            //            lvwVirtualMachines.Items.Add(listItem);
            //            Application.DoEvents();
            //        }
            //    }

            //    if (app.Default.SaveSelection)
            //    {
            //        lblStatus.Text = "BUSY: Reading saved selection";
            //        Application.DoEvents();
            //        _saveSelectionProvider.Read(Guid.Parse(subscriptionid),ref lvwVirtualNetworks, ref lvwStorageAccounts, ref lvwVirtualMachines);
            //    }

            //    lblStatus.Text = "Ready";


            //    writeLog("Subscriptions_SelectionChanged", "End");
            //}
        }

        private void lvwVirtualMachines_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            UpdateExportItemsCount();

            if (app.Default.AutoSelectDependencies)
            {
                if (e.Item.Checked)
                {
                    AutoSelectDependencies(e);
                }
            }

        }

        private void UpdateExportItemsCount()
        {
            //int numofobjects = lvwVirtualNetworks.CheckedItems.Count + lvwStorageAccounts.CheckedItems.Count + lvwVirtualMachines.CheckedItems.Count;
            //btnExport.Text = "Export " + numofobjects.ToString() + " objects";
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            // If save selection option is enabled
            if (app.Default.SaveSelection)
            {
                StatusProvider.UpdateStatus("BUSY: Reading saved selection");

                // TODO _saveSelectionProvider.Save(Guid.Parse(subscriptionid), lvwVirtualNetworks, lvwStorageAccounts, lvwVirtualMachines);
                Application.DoEvents();
            }

            var artefacts = new AsmArtefacts();
            //foreach (var selectedItem in lvwStorageAccounts.CheckedItems)
            //{
            //    var listItem = (ListViewItem)selectedItem;
            //    artefacts.StorageAccounts.Add(
            //   new Storage
            //    {
            //        RGName = listItem.Text,
            //        StorageName = listItem.SubItems[1].Text,
            //    });
            //}

            //foreach (var selectedItem in lvwStorageAccounts.Items)
            //{
            //    var listItem = (ListViewItem)selectedItem;
            //    artefacts.AllStorageAccounts.Add(
            //   new Storage
            //   {
            //       RGName = listItem.Text,
            //       StorageName = listItem.SubItems[1].Text,
            //   });
            //}


            //foreach (var selectedItem in lvwVirtualNetworks.CheckedItems)
            //{
            //    var listItem = (ListViewItem)selectedItem;
            //    artefacts.VirtualNetworks.Add(
            //        new VirtualNW
            //        {
            //            RGName = listItem.Text,
            //            VirtualNWName = listItem.SubItems[1].Text,
            //        });
            //}

            //foreach (var selectedItem in lvwVirtualMachines.CheckedItems)
            //{
            //    var listItem = (ListViewItem)selectedItem;
            //    artefacts.VirtualMachines.Add(
            //        new VirtualMC
            //        {
            //            RGName = listItem.Text,
            //            VirtualMachine = listItem.SubItems[1].Text,
            //        });
            //}

            StatusProvider.UpdateStatus("Done");
        }


        private void NewVersionAvailable()
        {
            //if (MigAz.Core.VersionCheck.NewVersionAvailable(Version.ArmToArm, ""))
            //{
            //    if (version != availableversion)
            //    {
                    DialogResult dialogresult = MessageBox.Show("New version " + "x.x.x.x" + " is available at http://aka.ms/MigAz", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    }
            //}
        }

        private void AutoSelectDependencies(ItemCheckedEventArgs listViewRow)
        {
            // TODO

            //string RGName = listViewRow.Item.ListView.Items[listViewRow.Item.Index].SubItems[0].Text;
            //string virtualMachineName = listViewRow.Item.ListView.Items[listViewRow.Item.Index].SubItems[1].Text;

            //string virtualNetworkName = "";


            //// Get Subscription from ComboBox
            //var token = GetToken(subscriptionsAndTenants[subscriptionid], PromptBehavior.Auto);

            //// Get VM details
            //_AzureRetriever.GetARMVMDetails(subscriptionid, RGName, virtualMachineName, token, out virtualNetworkName);

            //Hashtable virtualmachineinfo = new Hashtable();
            //virtualmachineinfo.Add("resourcegroup", RGName);
            //virtualmachineinfo.Add("virtualmachineName", virtualMachineName);
            //virtualmachineinfo.Add("virtualnetworkname", virtualNetworkName);


            ////Listing VMDetails
            //var VMDetails = _AzureRetriever.GetAzureARMResources("VirtualMachine", subscriptionid, virtualmachineinfo, token, RGName);
            //var virtualmachine = JsonConvert.DeserializeObject<dynamic>(VMDetails);

            //// process virtual network
            //foreach (ListViewItem virtualNetwork in lvwVirtualNetworks.Items)
            //{
            //    string VNSubstring = "/resourceGroups/" + virtualNetwork.SubItems[0].Text + "/providers/Microsoft.Network/virtualNetworks/" + virtualNetwork.SubItems[1].Text + "/";

            //    bool VNSw = virtualNetworkName.Contains(VNSubstring);

            //    if (VNSw == true)
            //    {
            //        virtualNetwork.Checked = true;
            //        virtualNetwork.Selected = true;
            //    }
            //}


            ////Process OS Disk

            //string OSDiskuri = virtualmachine.properties.storageProfile.osDisk.vhd.uri;

            //string[] splitarray = OSDiskuri.Split(new char[] { '/' });
            //string storageaccountname = splitarray[2].Split(new char[] { '.' })[0];

            //foreach (ListViewItem storageAccount in lvwStorageAccounts.Items)
            //{
            //    if (storageAccount.SubItems[1].Text == storageaccountname)
            //     {
            //        lvwStorageAccounts.Items[storageAccount.Index].Checked = true;
            //        lvwStorageAccounts.Items[storageAccount.Index].Selected = true;
            //      }
            //}



            ////Process Data Disks

            //foreach (var DataDisk in virtualmachine.properties.storageProfile.dataDisks)
            //{
            //    string DataDiskuri = DataDisk.vhd.uri;

            //    splitarray = DataDiskuri.Split(new char[] { '/' });
            //    storageaccountname = splitarray[2].Split(new char[] { '.' })[0];

            //    foreach (ListViewItem storageAccount in lvwStorageAccounts.Items)
            //    {
            //        if (storageAccount.SubItems[1].Text == storageaccountname)
            //        {
            //            lvwStorageAccounts.Items[storageAccount.Index].Checked = true;
            //            lvwStorageAccounts.Items[storageAccount.Index].Selected = true;
            //        }
            //    }
            //}


            StatusProvider.UpdateStatus("Ready");
        }

        private async void cmbTenants_SelectedIndexChanged(object sender, EventArgs e)
        {


            // TODO, remove, does context already have proper token?
            //var token = GetToken(Tenid, PromptBehavior.Auto);

            // TODO, should this be subscription in the tenant only?
            //Listing Subscriptions
            foreach (AzureSubscription azureSubscription in await _AzureRetriever.GetAzureARMSubscriptions())
            {
                //cmbSubscriptions.Items.Add(azureSubscription);
            }

            //cmbSubscriptions.Enabled = true;
        }

        public Forms.ARM.Interface.ITelemetryProvider TelemetryProvider
        {
            get { return _telemetryProvider; }
        }

        internal AppSettingsProvider AppSettingsProviders
        {
            get { return _appSettingsProvider; }
        }

        private void treeSource_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private async void treeSource_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (_sourceCascadeNode == null)
            {
                _sourceCascadeNode = e.Node;

                if (e.Node.Checked)
                {
                    await MigAz.Core.TreeView.RecursiveCheckToggleDown(e.Node, e.Node.Checked);
                    MigAz.Core.TreeView.FillUpIfFullDown(e.Node);
                    treeSource.SelectedNode = e.Node;

                    // todo await AutoSelectDependencies(e.Node);
                }
                else
                {
                    await MigAz.Core.TreeView.RecursiveCheckToggleUp(e.Node, e.Node.Checked);
                    await MigAz.Core.TreeView.RecursiveCheckToggleDown(e.Node, e.Node.Checked);
                }

                _sourceCascadeNode = null;

                // todo _SelectedNodes = this.UpdateSelectedNodes();
                // todo                 UpdateExportItemsCount();
                // todo this.TemplateGenerator.UpdateArtifacts(GetAsmArtifacts());
            }

        }
    }
}
