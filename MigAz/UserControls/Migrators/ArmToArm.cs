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
using MigAz.Core;
using MigAz.Core.Generator;

namespace MigAz.UserControls.Migrators
{
    public partial class ArmToArm : IMigratorUserControl
    {
        private AzureRetriever _AzureRetriever;
        private List<TreeNode> _SelectedNodes = new List<TreeNode>();
        private AppSettingsProvider _appSettingsProvider;
        private MigAz.Forms.ARM.Providers.UISaveSelectionProvider _saveSelectionProvider;
        private ArmToArmTelemetryProvider _telemetryProvider;
        private AzureContext _AzureContextARM;
        private TreeNode _sourceCascadeNode;
        private ResourceGroup _TargetResourceGroup;
        private PropertyPanel _PropertyPanel;

        public ArmToArm() : base(null, null) { }

        public ArmToArm(IStatusProvider statusProvider, ILogProvider logProvider, PropertyPanel propertyPanel)
            : base(statusProvider, logProvider)
        {
            InitializeComponent();

            _saveSelectionProvider = new MigAz.Forms.ARM.Providers.UISaveSelectionProvider();
            _telemetryProvider = new ArmToArmTelemetryProvider();
            _appSettingsProvider = new AppSettingsProvider();
            _PropertyPanel = propertyPanel;

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

            AlertIfNewVersionAvailable(); // check if there a new version of the app
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
                        TreeNode parentNode = MigAzTreeView.GetDataCenterTreeViewNode(subscriptionNode, armVirtualNetwork.Location, "Virtual Networks");
                        TreeNode tnVirtualNetwork = new TreeNode(armVirtualNetwork.Name);
                        tnVirtualNetwork.Name = armVirtualNetwork.Name;
                        tnVirtualNetwork.Tag = armVirtualNetwork;
                        parentNode.Nodes.Add(tnVirtualNetwork);
                        parentNode.Expand();
                    }
                }

                foreach (StorageAccount armStorageAccount in await _AzureContextARM.AzureRetriever.GetAzureARMStorageAccounts())
                {
                    TreeNode parentNode = MigAzTreeView.GetDataCenterTreeViewNode(subscriptionNode, armStorageAccount.PrimaryLocation, "Storage Accounts");
                    TreeNode tnStorageAccount = new TreeNode(armStorageAccount.Name);
                    tnStorageAccount.Name = tnStorageAccount.Text;
                    tnStorageAccount.Tag = armStorageAccount;
                    parentNode.Nodes.Add(tnStorageAccount);
                    parentNode.Expand();
                }

                foreach (VirtualMachine armVirtualMachine in await _AzureContextARM.AzureRetriever.GetAzureArmVirtualMachines())
                {
                    TreeNode parentNode = MigAzTreeView.GetDataCenterTreeViewNode(subscriptionNode, armVirtualMachine.Location, "Virtual Machines");
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

        #region New Version Check

        private async Task AlertIfNewVersionAvailable()
        {
            string currentVersion = "2.0.0.0";
            VersionCheck versionCheck = new VersionCheck(this.LogProvider);
            string newVersionNumber = await versionCheck.GetAvailableVersion("https://api.migaz.tools/v1/version/ARMtoARM", currentVersion);
            if (versionCheck.IsVersionNewer(currentVersion, newVersionNumber))
            {
                DialogResult dialogresult = MessageBox.Show("New version " + newVersionNumber + " is available at http://aka.ms/MigAz", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        #endregion

        private List<TreeNode> GetSelectedNodes(TreeView treeView)
        {
            List<TreeNode> selectedNodes = new List<TreeNode>();
            foreach (TreeNode treeNode in treeView.Nodes)
            {
                RecursiveNodeSelectedAdd(ref selectedNodes, treeNode);
            }
            return selectedNodes;
        }

        private void RecursiveNodeSelectedAdd(ref List<TreeNode> selectedNodes, TreeNode parentNode)
        {
            if (parentNode.Checked && parentNode.Tag != null && 
                (parentNode.Tag.GetType() == typeof(Azure.Arm.NetworkSecurityGroup) || 
                parentNode.Tag.GetType() == typeof(Azure.Arm.VirtualNetwork) || 
                parentNode.Tag.GetType() == typeof(Azure.Arm.StorageAccount) || 
                parentNode.Tag.GetType() == typeof(Azure.Arm.VirtualMachine))
                )
                selectedNodes.Add(parentNode);

            foreach (TreeNode childNode in parentNode.Nodes)
            {
                RecursiveNodeSelectedAdd(ref selectedNodes, childNode);
            }
        }

        private async Task AutoSelectDependencies(TreeNode selectedNode)
        {
            if ((app.Default.AutoSelectDependencies) && (selectedNode.Checked) && (selectedNode.Tag != null))
            {
                if (selectedNode.Tag.GetType() == typeof(Azure.Arm.VirtualMachine))
                {
                    Azure.Arm.VirtualMachine armVirtualMachine = (Azure.Arm.VirtualMachine)selectedNode.Tag;

                    #region process virtual network
                    if (armVirtualMachine.VirtualNetwork != null)
                    {
                        foreach (TreeNode treeNode in treeSource.Nodes.Find(armVirtualMachine.VirtualNetwork.Name, true))
                        {
                            if ((treeNode.Tag != null) && (treeNode.Tag.GetType() == typeof(Azure.Arm.VirtualNetwork)))
                            {
                                if (!treeNode.Checked)
                                    treeNode.Checked = true;
                            }
                        }
                    }

                    #endregion

                    #region OS Disk Storage Account

                    foreach (TreeNode treeNode in treeSource.Nodes.Find(armVirtualMachine.OSVirtualHardDisk.StorageAccountName, true))
                    {
                        if ((treeNode.Tag != null) && (treeNode.Tag.GetType() == typeof(Azure.Arm.StorageAccount)))
                        {
                            if (!treeNode.Checked)
                                treeNode.Checked = true;
                        }
                    }

                    #endregion

                    #region Data Disk(s) Storage Account(s)

                    foreach (Disk dataDisk in armVirtualMachine.DataDisks)
                    {
                        foreach (TreeNode treeNode in treeSource.Nodes.Find(dataDisk.StorageAccountName, true))
                        {
                            if ((treeNode.Tag != null) && (treeNode.Tag.GetType() == typeof(Azure.Arm.StorageAccount)))
                            {
                                if (!treeNode.Checked)
                                    treeNode.Checked = true;
                            }
                        }
                    }

                    #endregion

                    #region Network Security Group

                    if (armVirtualMachine.NetworkSecurityGroup != null)
                    {
                        foreach (TreeNode treeNode in treeSource.Nodes.Find(armVirtualMachine.NetworkSecurityGroup.Name, true))
                        {
                            if ((treeNode.Tag != null) && (treeNode.Tag.GetType() == typeof(Azure.Asm.NetworkSecurityGroup)))
                            {
                                if (!treeNode.Checked)
                                    treeNode.Checked = true;
                            }
                        }
                    }

                    #endregion

                }

                else if (selectedNode.Tag.GetType() == typeof(Azure.Arm.VirtualNetwork))
                {
                    Azure.Arm.VirtualNetwork armVirtualNetwork = (Azure.Arm.VirtualNetwork)selectedNode.Tag;

                    foreach (Azure.Arm.Subnet armSubnet in armVirtualNetwork.Subnets)
                    {
                        if (armSubnet.NetworkSecurityGroup != null)
                        {
                            foreach (TreeNode treeNode in treeSource.Nodes.Find(armSubnet.NetworkSecurityGroup.Name, true))
                            {
                                if ((treeNode.Tag != null) && (treeNode.Tag.GetType() == typeof(Azure.Arm.NetworkSecurityGroup)))
                                {
                                    if (!treeNode.Checked)
                                        treeNode.Checked = true;
                                }
                            }
                        }
                    }
                }
            }

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
                    await MigAzTreeView.RecursiveCheckToggleDown(e.Node, e.Node.Checked);
                    MigAzTreeView.FillUpIfFullDown(e.Node);
                    treeSource.SelectedNode = e.Node;

                    await AutoSelectDependencies(e.Node);
                }
                else
                {
                    await MigAzTreeView.RecursiveCheckToggleUp(e.Node, e.Node.Checked);
                    await MigAzTreeView.RecursiveCheckToggleDown(e.Node, e.Node.Checked);
                }

                _sourceCascadeNode = null;

                _SelectedNodes = this.GetSelectedNodes(treeSource);
                // todo                 UpdateExportItemsCount();
                this.TemplateGenerator.UpdateArtifacts(GetArmArtifacts());
            }

        }
        private ExportArtifacts GetArmArtifacts()
        {
            ExportArtifacts artifacts = new ExportArtifacts();
            foreach (TreeNode selectedNode in _SelectedNodes)
            {
                Type tagType = selectedNode.Tag.GetType();
                if (tagType == typeof(Azure.Arm.NetworkSecurityGroup))
                {
                    artifacts.NetworkSecurityGroups.Add((INetworkSecurityGroup)selectedNode.Tag);
                }
                else if (tagType == typeof(Azure.Arm.VirtualNetwork))
                {
                    artifacts.VirtualNetworks.Add((IVirtualNetwork)selectedNode.Tag);
                }
                else if (tagType == typeof(Azure.Arm.StorageAccount))
                {
                    artifacts.StorageAccounts.Add((IStorageAccount)selectedNode.Tag);
                }
                else if (tagType == typeof(Azure.Arm.VirtualMachine))
                {
                    artifacts.VirtualMachines.Add((IVirtualMachine)selectedNode.Tag);
                }
            }

            return artifacts;
        }

        private void treeSource_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Tag != null)
            {
                if (e.Node.Tag.GetType() == typeof(Azure.Arm.VirtualMachine))
                {
                    Azure.Arm.VirtualMachine armVirtualMachine = (Azure.Arm.VirtualMachine)e.Node.Tag;
                    if (armVirtualMachine.OSVirtualHardDisk.VhdUri == String.Empty)
                    {
                        LogProvider.WriteLog("treeSource_BeforeCheck", "VM '" + armVirtualMachine.Name + "' contains one or more mananged disks which is not yet supported.");
                        e.Cancel = true;
                    }
                }
            }
        }
    }
}
