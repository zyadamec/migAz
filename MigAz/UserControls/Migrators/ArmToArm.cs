using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using MigAz.Azure;
using MigAz.Models;
using MigAz.Providers;
using MigAz.Core.Interface;
using MigAz.Azure.Arm;
using MigAz.Azure.Generator.ArmToArm;

namespace MigAz.UserControls.Migrators
{
    public partial class ArmToArm : IMigratorUserControl
    {
        private AzureRetriever _AzureRetriever;
        private AsmArtefacts _asmArtefacts;
        private AppSettingsProvider _appSettingsProvider;
        private MigAz.Forms.ARM.Providers.UISaveSelectionProvider _saveSelectionProvider;
        private ArmToArmTelemetryProvider _telemetryProvider;
        private AzureContext _AzureContextARM;
        private TreeNode _sourceCascadeNode;
        private ResourceGroup _TargetResourceGroup;

        public ArmToArm() : base(null, null) { }

        public ArmToArm(IStatusProvider statusProvider, ILogProvider logProvider)
            : base(statusProvider, logProvider)
        {
            InitializeComponent();

            _saveSelectionProvider = new MigAz.Forms.ARM.Providers.UISaveSelectionProvider();
            _telemetryProvider = new ArmToArmTelemetryProvider();
            _appSettingsProvider = new AppSettingsProvider();

            _AzureContextARM = new AzureContext(LogProvider, StatusProvider, _appSettingsProvider);
            _AzureContextARM.AfterAzureSubscriptionChange += _AzureContextARM_AfterAzureSubscriptionChange;
            azureLoginContextViewer1.Bind(_AzureContextARM);

            _TargetResourceGroup = new ResourceGroup(this._AzureContextARM, "Target Resource Group");

            this.TemplateGenerator = new ArmToArmGenerator(_AzureContextARM.AzureSubscription, _AzureContextARM.AzureSubscription, _TargetResourceGroup, LogProvider, StatusProvider, _telemetryProvider, _appSettingsProvider);
        }

        private async void ArmToArm_Load(object sender, EventArgs e)
        {
            LogProvider.WriteLog("ArmToArm_Load", "Program start");

            try
            {
                _AzureContextARM.AzureEnvironment = (AzureEnvironment)Enum.Parse(typeof(AzureEnvironment), app.Default.AzureEnvironment);
            }
            catch
            {
                _AzureContextARM.AzureEnvironment = AzureEnvironment.AzureCloud;
            }

            NewVersionAvailable(); // check if there a new version of the app
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

                subscriptionNode.ExpandAll();

                //    if (app.Default.SaveSelection)
                //    {
                //        lblStatus.Text = "BUSY: Reading saved selection";
                //        Application.DoEvents();
                //        _saveSelectionProvider.Read(Guid.Parse(subscriptionid),ref lvwVirtualNetworks, ref lvwStorageAccounts, ref lvwVirtualMachines);
                //    }

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

        private void UpdateExportItemsCount()
        {
            //int numofobjects = lvwVirtualNetworks.CheckedItems.Count + lvwStorageAccounts.CheckedItems.Count + lvwVirtualMachines.CheckedItems.Count;
            //btnExport.Text = "Export " + numofobjects.ToString() + " objects";
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

        private async Task AutoSelectDependencies(TreeNode selectedNode)
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

        public ITelemetryProvider TelemetryProvider
        {
            get { return _telemetryProvider; }
        }

        internal AppSettingsProvider AppSettingsProviders
        {
            get { return _appSettingsProvider; }
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

                    await AutoSelectDependencies(e.Node);
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
