using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MigAz.Providers;
using MigAz.Azure;
using MigAz.Azure.Arm;
using MigAz.Core.Interface;
using MigAz.Azure.Asm;
using MigAz.Azure.Generator.AsmToArm;
using MigAz.Core.Generator;
using MigAz.Core;
using MigAz.Forms;

namespace MigAz.UserControls.Migrators
{
    public partial class AsmToArm : IMigratorUserControl
    {
        #region Variables

        private UISaveSelectionProvider _saveSelectionProvider;
        private TreeNode _SourceAsmNode;
        private TreeNode _SourceArmNode;
        private List<TreeNode> _SelectedNodes = new List<TreeNode>();
        private AsmToArmTelemetryProvider _telemetryProvider;
        private AppSettingsProvider _appSettingsProvider;
        private AzureContext _AzureContextSourceASM;
        private AzureContext _AzureContextTargetARM;
        private Azure.MigrationTarget.ResourceGroup _TargetResourceGroup;
        private PropertyPanel _PropertyPanel;

        #endregion

        #region Constructors

        public AsmToArm() : base(null, null) { }

        public AsmToArm(IStatusProvider statusProvider, ILogProvider logProvider, PropertyPanel propertyPanel) 
            : base (statusProvider, logProvider)
        {
            InitializeComponent();

            _saveSelectionProvider = new UISaveSelectionProvider();
            _telemetryProvider = new AsmToArmTelemetryProvider();
            _appSettingsProvider = new AppSettingsProvider();
            _PropertyPanel = propertyPanel;

            _AzureContextSourceASM = new AzureContext(LogProvider, StatusProvider, _appSettingsProvider);
            _AzureContextSourceASM.AzureEnvironmentChanged += _AzureContextSourceASM_AzureEnvironmentChanged;
            _AzureContextSourceASM.UserAuthenticated += _AzureContextSourceASM_UserAuthenticated;
            _AzureContextSourceASM.BeforeAzureSubscriptionChange += _AzureContextSourceASM_BeforeAzureSubscriptionChange;
            _AzureContextSourceASM.AfterAzureSubscriptionChange += _AzureContextSourceASM_AfterAzureSubscriptionChange;
            _AzureContextSourceASM.BeforeUserSignOut += _AzureContextSourceASM_BeforeUserSignOut;
            _AzureContextSourceASM.AfterUserSignOut += _AzureContextSourceASM_AfterUserSignOut;
            _AzureContextSourceASM.AfterAzureTenantChange += _AzureContextSourceASM_AfterAzureTenantChange;

            _AzureContextTargetARM = new AzureContext(LogProvider, StatusProvider, _appSettingsProvider);
            _AzureContextTargetARM.AfterAzureSubscriptionChange += _AzureContextTargetARM_AfterAzureSubscriptionChange;

            _TargetResourceGroup = new Azure.MigrationTarget.ResourceGroup(this.AzureContextSourceASM);

            azureLoginContextViewerASM.Bind(_AzureContextSourceASM);
            azureLoginContextViewerARM.Bind(_AzureContextTargetARM);

            this.TemplateGenerator = new AzureGenerator(_AzureContextSourceASM.AzureSubscription, _AzureContextTargetARM.AzureSubscription, _TargetResourceGroup, LogProvider, StatusProvider, _telemetryProvider, _appSettingsProvider);
        }

        private async Task _AzureContextTargetARM_AfterAzureSubscriptionChange(AzureContext sender)
        {
            this.TemplateGenerator.TargetSubscription = sender.AzureSubscription;
        }

        private async Task _AzureContextSourceASM_AfterAzureTenantChange(AzureContext sender)
        {
            await _AzureContextTargetARM.CopyContext(_AzureContextSourceASM);
        }

        #endregion

        private async Task _AzureContextSourceASM_BeforeAzureSubscriptionChange(AzureContext sender)
        {
            await SaveSubscriptionSettings(sender.AzureSubscription);
            await _AzureContextTargetARM.SetSubscriptionContext(null);
        }

        private async Task _AzureContextSourceASM_AzureEnvironmentChanged(AzureContext sender)
        {
            app.Default.AzureEnvironment = sender.AzureEnvironment.ToString();
            app.Default.Save();

            if (_AzureContextTargetARM.TokenProvider == null)
                _AzureContextTargetARM.AzureEnvironment = sender.AzureEnvironment;
        }


        private async Task _AzureContextSourceASM_UserAuthenticated(AzureContext sender)
        {
            if (_AzureContextTargetARM.TokenProvider.AuthenticationResult == null)
            {
                await _AzureContextTargetARM.CopyContext(_AzureContextSourceASM);
            }
        }

        private async Task _AzureContextSourceASM_BeforeUserSignOut()
        {
            await SaveSubscriptionSettings(this._AzureContextSourceASM.AzureSubscription);
        }

        private async Task _AzureContextSourceASM_AfterUserSignOut()
        {
            ResetForm();
            await _AzureContextTargetARM.SetSubscriptionContext(null);
            await _AzureContextTargetARM.Logout();
            azureLoginContextViewerARM.Enabled = false;
            azureLoginContextViewerARM.Refresh();
        }

        private async Task _AzureContextSourceASM_AfterAzureSubscriptionChange(AzureContext sender)
        {
            ResetForm();

            try
            {
                if (sender.AzureSubscription != null)
                {
                    if (_AzureContextTargetARM.AzureSubscription == null)
                    {
                        await _AzureContextTargetARM.SetSubscriptionContext(_AzureContextSourceASM.AzureSubscription);
                    }

                    azureLoginContextViewerARM.Enabled = true;

                    this.TemplateGenerator.SourceSubscription = _AzureContextSourceASM.AzureSubscription;
                    this.TemplateGenerator.TargetSubscription = _AzureContextTargetARM.AzureSubscription;

                    #region Bind Source ASM Objects

                    TreeNode subscriptionNodeASM = new TreeNode(sender.AzureSubscription.Name);
                    treeSourceASM.Nodes.Add(subscriptionNodeASM);
                    subscriptionNodeASM.Expand();

                    List<Azure.Asm.VirtualNetwork> asmVirtualNetworks = await _AzureContextSourceASM.AzureRetriever.GetAzureAsmVirtualNetworks();
                    foreach (Azure.Asm.VirtualNetwork asmVirtualNetwork in asmVirtualNetworks)
                    {
                        if (asmVirtualNetwork.HasNonGatewaySubnet)
                        {
                            TreeNode parentNode = GetDataCenterTreeViewNode(subscriptionNodeASM, asmVirtualNetwork.Location, "Virtual Networks");
                            TreeNode tnVirtualNetwork = new TreeNode(asmVirtualNetwork.Name);
                            tnVirtualNetwork.Name = asmVirtualNetwork.Name;
                            tnVirtualNetwork.Tag = asmVirtualNetwork;
                            parentNode.Nodes.Add(tnVirtualNetwork);
                            parentNode.Expand();
                        }
                    }

                    foreach (Azure.Asm.StorageAccount asmStorageAccount in await _AzureContextSourceASM.AzureRetriever.GetAzureAsmStorageAccounts())
                    {
                        TreeNode parentNode = GetDataCenterTreeViewNode(subscriptionNodeASM, asmStorageAccount.GeoPrimaryRegion, "Storage Accounts");
                        TreeNode tnStorageAccount = new TreeNode(asmStorageAccount.Name);
                        tnStorageAccount.Name = tnStorageAccount.Text;
                        tnStorageAccount.Tag = asmStorageAccount;
                        parentNode.Nodes.Add(tnStorageAccount);
                        parentNode.Expand();
                    }

                    List<CloudService> asmCloudServices = await _AzureContextSourceASM.AzureRetriever.GetAzureAsmCloudServices();
                    foreach (CloudService asmCloudService in asmCloudServices)
                    {
                        foreach (Azure.Asm.VirtualMachine asmVirtualMachine in asmCloudService.VirtualMachines)
                        {
                            TreeNode parentNode = GetDataCenterTreeViewNode(subscriptionNodeASM, asmCloudService.Location, "Cloud Services");
                            TreeNode[] cloudServiceNodeSearch = parentNode.Nodes.Find(asmCloudService.ServiceName, false);
                            TreeNode cloudServiceNode = null;
                            if (cloudServiceNodeSearch.Count() == 1)
                            {
                                cloudServiceNode = cloudServiceNodeSearch[0];
                            }

                            if (cloudServiceNode == null)
                            {
                                cloudServiceNode = new TreeNode(asmCloudService.ServiceName);
                                cloudServiceNode.Name = asmCloudService.ServiceName;
                                cloudServiceNode.Tag = asmCloudService;
                                parentNode.Nodes.Add(cloudServiceNode);
                                parentNode.Expand();
                            }

                            TreeNode virtualMachineNode = new TreeNode(asmVirtualMachine.RoleName);
                            virtualMachineNode.Name = asmVirtualMachine.RoleName;
                            virtualMachineNode.Tag = asmVirtualMachine;
                            cloudServiceNode.Nodes.Add(virtualMachineNode);
                            cloudServiceNode.Expand();
                        }
                    }

                    foreach (Azure.Asm.NetworkSecurityGroup asmNetworkSecurityGroup in await _AzureContextSourceASM.AzureRetriever.GetAzureAsmNetworkSecurityGroups())
                    {
                        TreeNode parentNode = GetDataCenterTreeViewNode(subscriptionNodeASM, asmNetworkSecurityGroup.Location, "Network Security Groups");
                        TreeNode tnStorageAccount = new TreeNode(asmNetworkSecurityGroup.Name);
                        tnStorageAccount.Name = tnStorageAccount.Text;
                        tnStorageAccount.Tag = asmNetworkSecurityGroup;
                        parentNode.Nodes.Add(tnStorageAccount);
                        parentNode.Expand();
                    }

                    subscriptionNodeASM.ExpandAll();

                    #endregion

                    #region Bind Source ARM Objects

                    TreeNode subscriptionNodeARM = new TreeNode(sender.AzureSubscription.Name);
                    subscriptionNodeARM.ImageKey = "Subscription";
                    subscriptionNodeARM.SelectedImageKey = "Subscription";
                    treeSourceARM.Nodes.Add(subscriptionNodeARM);
                    subscriptionNodeARM.Expand();

                    List<Azure.Arm.VirtualNetwork> armVirtualNetworks = await _AzureContextSourceASM.AzureRetriever.GetAzureARMVirtualNetworks();
                    foreach (Azure.Arm.VirtualNetwork armVirtualNetwork in armVirtualNetworks)
                    {
                        if (armVirtualNetwork.HasNonGatewaySubnet)
                        {
                            TreeNode virtualNetworkParentNode = subscriptionNodeARM;

                            if (armVirtualNetwork.ResourceGroup != null)
                            {
                                TreeNode tnResourceGroup = GetResourceGroupTreeNode(subscriptionNodeARM, armVirtualNetwork.ResourceGroup);
                                tnResourceGroup.ImageKey = "ResourceGroup";
                                tnResourceGroup.SelectedImageKey = "ResourceGroup";
                                virtualNetworkParentNode = tnResourceGroup;
                            }

                            TreeNode tnVirtualNetwork = new TreeNode(armVirtualNetwork.Name);
                            tnVirtualNetwork.Name = armVirtualNetwork.Name;
                            tnVirtualNetwork.Tag = armVirtualNetwork;
                            tnVirtualNetwork.ImageKey = "VirtualNetwork";
                            tnVirtualNetwork.SelectedImageKey = "VirtualNetwork";
                            virtualNetworkParentNode.Nodes.Add(tnVirtualNetwork);
                            virtualNetworkParentNode.Expand();
                        }
                    }

                    foreach (Azure.Arm.StorageAccount armStorageAccount in await _AzureContextSourceASM.AzureRetriever.GetAzureARMStorageAccounts())
                    {
                        TreeNode storageAccountParentNode = subscriptionNodeARM;

                        if (armStorageAccount.ResourceGroup != null)
                        {
                            TreeNode tnResourceGroup = GetResourceGroupTreeNode(subscriptionNodeARM, armStorageAccount.ResourceGroup);
                            tnResourceGroup.ImageKey = "ResourceGroup";
                            tnResourceGroup.SelectedImageKey = "ResourceGroup";
                            storageAccountParentNode = tnResourceGroup;
                        }

                        TreeNode tnStorageAccount = new TreeNode(armStorageAccount.Name);
                        tnStorageAccount.Name = armStorageAccount.Name;
                        tnStorageAccount.Tag = armStorageAccount;
                        tnStorageAccount.ImageKey = "StorageAccount";
                        tnStorageAccount.SelectedImageKey = "StorageAccount";
                        storageAccountParentNode.Nodes.Add(tnStorageAccount);
                        storageAccountParentNode.Expand();
                    }

                    foreach (Azure.Arm.VirtualMachine armVirtualMachine in await _AzureContextSourceASM.AzureRetriever.GetAzureArmVirtualMachines())
                    {
                        TreeNode virtualMachineParentNode = subscriptionNodeARM;

                        if (armVirtualMachine.ResourceGroup != null)
                        {
                            TreeNode tnResourceGroup = GetResourceGroupTreeNode(subscriptionNodeARM, armVirtualMachine.ResourceGroup);
                            tnResourceGroup.ImageKey = "ResourceGroup";
                            tnResourceGroup.SelectedImageKey = "ResourceGroup";
                            virtualMachineParentNode = tnResourceGroup;
                        }

                        if (armVirtualMachine.AvailabilitySet != null)
                        {
                            TreeNode tnAvailabilitySet = GetAvailabilitySetTreeNode(subscriptionNodeARM, armVirtualMachine.AvailabilitySet);
                            tnAvailabilitySet.ImageKey = "AvailabilitySet";
                            tnAvailabilitySet.SelectedImageKey = "AvailabilitySet";
                            virtualMachineParentNode = tnAvailabilitySet;
                        }

                        TreeNode tnVirtualMachine = new TreeNode(armVirtualMachine.Name);
                        tnVirtualMachine.Name = armVirtualMachine.Name;
                        tnVirtualMachine.Tag = armVirtualMachine;
                        tnVirtualMachine.ImageKey = "VirtualMachine";
                        tnVirtualMachine.SelectedImageKey = "VirtualMachine";
                        virtualMachineParentNode.Nodes.Add(tnVirtualMachine);
                        virtualMachineParentNode.Expand();
                    }

                    subscriptionNodeARM.ExpandAll();

                    #endregion

                    _AzureContextSourceASM.AzureRetriever.SaveRestCache();
                    await ReadSubscriptionSettings(sender.AzureSubscription);

                    treeSourceASM.Enabled = true;
                    treeSourceARM.Enabled = true;
                    treeTargetARM.Enabled = true;
                }
            }
            catch (Exception exc)
            {
                UnhandledExceptionDialog unhandledException = new UnhandledExceptionDialog(LogProvider, exc);
                unhandledException.ShowDialog();
            }
            
            StatusProvider.UpdateStatus("Ready");
        }

        internal void ActivateSourceARMTab()
        {
            tabSourceResources.SelectedTab = tabSourceResources.TabPages[1];
        }

        internal void ChangeAzureContext()
        {
            azureLoginContextViewerASM.ChangeAzureContext();
        }

        private void ResetForm()
        {
            treeSourceASM.Nodes.Clear();
            treeSourceARM.Nodes.Clear();
            treeTargetARM.Nodes.Clear();
            _SelectedNodes.Clear();
            UpdateExportItemsCount();
            _PropertyPanel.Clear();
            treeSourceASM.Enabled = false;
            treeSourceARM.Enabled = false;
            treeTargetARM.Enabled = false;
        }

        #region Properties

        public Azure.MigrationTarget.ResourceGroup TargetResourceGroup
        {
            get { return _TargetResourceGroup; }
        }

        public AzureContext AzureContextSourceASM
        {
            get { return _AzureContextSourceASM; }
        }

        public AzureContext AzureContextTargetARM
        {
            get { return _AzureContextTargetARM; }
        }

        public MigAz.Core.Interface.ITelemetryProvider TelemetryProvider
        {
            get { return _telemetryProvider; }
        }

        internal AppSettingsProvider AppSettingsProviders
        {
            get { return _appSettingsProvider; }
        }

        internal List<TreeNode> SelectedNodes
        {
            get { return _SelectedNodes; }
        }

        #endregion

        #region New Version Check

        private async Task AlertIfNewVersionAvailable()
        {
            string currentVersion = "2.2.0.0";
            VersionCheck versionCheck = new VersionCheck(this.LogProvider);
            string newVersionNumber = await versionCheck.GetAvailableVersion("https://asmtoarmtoolapi.azurewebsites.net/api/version", currentVersion);
            if (versionCheck.IsVersionNewer(currentVersion, newVersionNumber))
            {
                DialogResult dialogresult = MessageBox.Show("New version " + newVersionNumber + " is available at http://aka.ms/MigAz", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        #endregion

        #region ASM TreeView Methods


        private async Task AutoSelectDependencies(TreeNode selectedNode)
        {
            if ((app.Default.AutoSelectDependencies) && (selectedNode.Checked) && (selectedNode.Tag != null))
            {
                if (selectedNode.Tag.GetType() == typeof(Azure.Asm.VirtualMachine))
                {
                    Azure.Asm.VirtualMachine asmVirtualMachine = (Azure.Asm.VirtualMachine)selectedNode.Tag;

                    #region process virtual network
                    if (asmVirtualMachine.VirtualNetworkName != string.Empty)
                    {
                        foreach (TreeNode treeNode in treeSourceASM.Nodes.Find(asmVirtualMachine.VirtualNetworkName, true))
                        {
                            if ((treeNode.Tag != null) && (treeNode.Tag.GetType() == typeof(Azure.Asm.VirtualNetwork)))
                            {
                                if (!treeNode.Checked)
                                    treeNode.Checked = true;
                            }
                        }
                    }

                    #endregion

                    #region OS Disk Storage Account

                    foreach (TreeNode treeNode in treeSourceASM.Nodes.Find(asmVirtualMachine.OSVirtualHardDisk.StorageAccountName, true))
                    {
                        if ((treeNode.Tag != null) && (treeNode.Tag.GetType() == typeof(Azure.Asm.StorageAccount)))
                        {
                            if (!treeNode.Checked)
                                treeNode.Checked = true;
                        }
                    }

                    #endregion

                    #region Data Disk(s) Storage Account(s)

                    foreach (Azure.Asm.Disk dataDisk in asmVirtualMachine.DataDisks)
                    {
                        foreach (TreeNode treeNode in treeSourceASM.Nodes.Find(dataDisk.StorageAccountName, true))
                        {
                            if ((treeNode.Tag != null) && (treeNode.Tag.GetType() == typeof(Azure.Asm.StorageAccount)))
                            {
                                if (!treeNode.Checked)
                                    treeNode.Checked = true;
                            }
                        }
                    }

                    #endregion

                    #region Network Security Group

                    if (asmVirtualMachine.NetworkSecurityGroup != null)
                    {
                        foreach (TreeNode treeNode in treeSourceASM.Nodes.Find(asmVirtualMachine.NetworkSecurityGroup.Name, true))
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

                else if (selectedNode.Tag.GetType() == typeof(Azure.Asm.VirtualNetwork))
                {
                    Azure.Asm.VirtualNetwork asmVirtualNetwork = (Azure.Asm.VirtualNetwork)selectedNode.Tag;

                    foreach (Azure.Asm.Subnet asmSubnet in asmVirtualNetwork.Subnets)
                    {
                        if (asmSubnet.NetworkSecurityGroup != null)
                        {
                            foreach (TreeNode treeNode in treeSourceASM.Nodes.Find(asmSubnet.NetworkSecurityGroup.Name, true))
                            {
                                if ((treeNode.Tag != null) && (treeNode.Tag.GetType() == typeof(Azure.Asm.NetworkSecurityGroup)))
                                {
                                    if (!treeNode.Checked)
                                        treeNode.Checked = true;
                                }
                            }
                        }
                    }
                }

                StatusProvider.UpdateStatus("Ready");
            }
        }

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
                (parentNode.Tag.GetType() == typeof(Azure.Asm.NetworkSecurityGroup) || 
                parentNode.Tag.GetType() == typeof(Azure.Asm.VirtualNetwork) || 
                parentNode.Tag.GetType() == typeof(Azure.Asm.StorageAccount) ||
                parentNode.Tag.GetType() == typeof(Azure.Asm.VirtualMachine) ||
                parentNode.Tag.GetType() == typeof(Azure.Arm.NetworkSecurityGroup) ||
                parentNode.Tag.GetType() == typeof(Azure.Arm.VirtualNetwork) ||
                parentNode.Tag.GetType() == typeof(Azure.Arm.StorageAccount) ||
                parentNode.Tag.GetType() == typeof(Azure.Arm.VirtualMachine)
                ))
                selectedNodes.Add(parentNode);

            foreach (TreeNode childNode in parentNode.Nodes)
            {
                RecursiveNodeSelectedAdd(ref selectedNodes, childNode);
            }
        }
        #endregion

        #region ASM TreeView Events

        private void treeASM_AfterSelect(object sender, TreeViewEventArgs e)
        {
            _PropertyPanel.Clear();
        }

        private async void treeASM_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (_SourceAsmNode == null)
            {
                _SourceAsmNode = e.Node;
            }

            if (e.Node.Checked)
                await AutoSelectDependencies(e.Node);

            TreeNode resultUpdateARMTree = await UpdateARMTree(e.Node);

            if (_SourceAsmNode != null && _SourceAsmNode == e.Node)
            {
                if (e.Node.Checked)
                {
                    await RecursiveCheckToggleDown(e.Node, e.Node.Checked);
                    FillUpIfFullDown(e.Node);
                    treeSourceASM.SelectedNode = e.Node;
                }
                else
                {
                    await RecursiveCheckToggleUp(e.Node, e.Node.Checked);
                    await RecursiveCheckToggleDown(e.Node, e.Node.Checked);
                }

                _SelectedNodes = this.GetSelectedNodes(treeSourceASM);
                UpdateExportItemsCount();
                await this.TemplateGenerator.UpdateArtifacts(GetExportArtifacts());

                _SourceAsmNode = null;

                if (resultUpdateARMTree != null)
                    treeTargetARM.SelectedNode = resultUpdateARMTree;
            }
        }


        private void GetExportArtifactsRecursive(TreeNode parentTreeNode, ref ExportArtifacts exportArtifacts)
        {
            foreach (TreeNode selectedNode in parentTreeNode.Nodes)
            {
                Type tagType = selectedNode.Tag.GetType();

                if (tagType == typeof(Azure.MigrationTarget.VirtualNetwork))
                {
                    exportArtifacts.VirtualNetworks.Add((Azure.MigrationTarget.VirtualNetwork)selectedNode.Tag);
                }
                else if (tagType == typeof(Azure.MigrationTarget.StorageAccount))
                {
                    exportArtifacts.StorageAccounts.Add((Azure.MigrationTarget.StorageAccount)selectedNode.Tag);
                }
                else if (tagType == typeof(Azure.MigrationTarget.NetworkSecurityGroup))
                {
                    exportArtifacts.NetworkSecurityGroups.Add((Azure.MigrationTarget.NetworkSecurityGroup)selectedNode.Tag);
                }
                else if (tagType == typeof(Azure.MigrationTarget.VirtualMachine))
                {
                    exportArtifacts.VirtualMachines.Add((Azure.MigrationTarget.VirtualMachine)selectedNode.Tag);
                }
            }

            foreach (TreeNode treeNode in parentTreeNode.Nodes)
            {
                GetExportArtifactsRecursive(treeNode, ref exportArtifacts);
            }
        }

        private ExportArtifacts GetExportArtifacts()
        {
            ExportArtifacts exportArtifacts = new ExportArtifacts();

            foreach (TreeNode treeNode in treeTargetARM.Nodes)
            {
                GetExportArtifactsRecursive(treeNode, ref exportArtifacts);
            }

            return exportArtifacts;
        }

        #endregion

        #region ARM TreeView Methods

        private async void treeARM_AfterSelect(object sender, TreeViewEventArgs e)
        {
            LogProvider.WriteLog("treeARM_AfterSelect", "Start");
            _SourceArmNode = e.Node;

            _PropertyPanel.Clear();
            _PropertyPanel.ResourceText = String.Empty;
            if (e.Node.Tag != null)
            {
                _PropertyPanel.ResourceText = e.Node.Text;

                if (e.Node.Tag.GetType() == typeof(Azure.MigrationTarget.VirtualMachine))
                {
                    this._PropertyPanel.ResourceImage = imageList1.Images["VirtualMachine"];

                    VirtualMachineProperties properties = new VirtualMachineProperties();
                    properties.LogProvider = LogProvider;
                    properties.AllowManangedDisk = (await _AzureContextSourceASM.AzureRetriever.GetAzureARMManagedDisks() != null);
                    properties.PropertyChanged += Properties_PropertyChanged;
                    await properties.Bind(e.Node, this);
                    _PropertyPanel.PropertyDetailControl = properties;
                }
                else if (e.Node.Tag.GetType() == typeof(Azure.MigrationTarget.NetworkSecurityGroup))
                {
                    this._PropertyPanel.ResourceImage = imageList1.Images["NetworkSecurityGroup"];

                    NetworkSecurityGroupProperties properties = new NetworkSecurityGroupProperties();
                    properties.PropertyChanged += Properties_PropertyChanged;
                    properties.Bind(e.Node, this);
                    _PropertyPanel.PropertyDetailControl = properties;
                }
                if (e.Node.Tag.GetType() == typeof(Azure.MigrationTarget.VirtualNetwork))
                {
                    this._PropertyPanel.ResourceImage = imageList1.Images["VirtualNetwork"];

                    VirtualNetworkProperties properties = new VirtualNetworkProperties();
                    properties.PropertyChanged += Properties_PropertyChanged;
                    properties.Bind(e.Node);
                    _PropertyPanel.PropertyDetailControl = properties;
                }
                else if (e.Node.Tag.GetType() == typeof(Azure.MigrationTarget.Subnet))
                {
                    this._PropertyPanel.ResourceImage = imageList1.Images["VirtualNetwork"];

                    SubnetProperties properties = new SubnetProperties();
                    properties.PropertyChanged += Properties_PropertyChanged;
                    properties.Bind(e.Node);
                    _PropertyPanel.PropertyDetailControl = properties;
                }
                else if (e.Node.Tag.GetType() == typeof(Azure.MigrationTarget.StorageAccount))
                {
                    this._PropertyPanel.ResourceImage = imageList1.Images["StorageAccount"];

                    StorageAccountProperties properties = new StorageAccountProperties();
                    properties.PropertyChanged += Properties_PropertyChanged;
                    properties.Bind(this._AzureContextTargetARM, e.Node);
                    _PropertyPanel.PropertyDetailControl = properties;
                }
                else if (e.Node.Tag.GetType() == typeof(Azure.Asm.Disk) || e.Node.Tag.GetType() == typeof(Azure.Arm.Disk))
                {
                    Azure.Asm.Disk asmDisk = (Azure.Asm.Disk)e.Node.Tag;

                    this._PropertyPanel.ResourceImage = imageList1.Images["Disk"];

                    DiskProperties properties = new DiskProperties();
                    properties.LogProvider = this.LogProvider;
                    properties.AllowManangedDisk = (await _AzureContextSourceASM.AzureRetriever.GetAzureARMManagedDisks() != null);
                    properties.PropertyChanged += Properties_PropertyChanged;
                    properties.Bind(this, e.Node);
                    _PropertyPanel.PropertyDetailControl = properties;
                }
                else if (e.Node.Tag.GetType() == typeof(Azure.MigrationTarget.AvailabilitySet))
                {
                    this._PropertyPanel.ResourceImage = imageList1.Images["AvailabilitySet"];

                    AvailabilitySetProperties properties = new AvailabilitySetProperties();
                    properties.PropertyChanged += Properties_PropertyChanged;
                    properties.Bind(e.Node);
                    _PropertyPanel.PropertyDetailControl = properties;
                }
                else if (e.Node.Tag.GetType() == typeof(Azure.MigrationTarget.ResourceGroup))
                {
                    this._PropertyPanel.ResourceImage = imageList1.Images["ResourceGroup"];

                    ResourceGroupProperties properties = new ResourceGroupProperties();
                    properties.PropertyChanged += Properties_PropertyChanged;
                    await properties.Bind(this, e.Node);
                    _PropertyPanel.PropertyDetailControl = properties;
                }
            }

            _SourceArmNode = null;
            LogProvider.WriteLog("treeARM_AfterSelect", "End");
            StatusProvider.UpdateStatus("Ready");
        }

        private async Task Properties_PropertyChanged()
        {
            if (_SourceAsmNode == null && _SourceArmNode == null) // we are not going to update on every property bind during TreeView updates
                await this.TemplateGenerator.UpdateArtifacts(GetExportArtifacts());
        }

        private async Task<TreeNode> UpdateARMTree(TreeNode asmTreeNode)
        {
            if (asmTreeNode.Tag != null)
            {
                Type tagType = asmTreeNode.Tag.GetType();
                if ((tagType == typeof(Azure.Asm.VirtualNetwork)) ||
                    (tagType == typeof(Azure.Asm.StorageAccount)) ||
                    (tagType == typeof(Azure.Asm.VirtualMachine)) ||
                    (tagType == typeof(Azure.Asm.NetworkSecurityGroup)) ||
                    (tagType == typeof(Azure.Arm.VirtualNetwork)) ||
                    (tagType == typeof(Azure.Arm.StorageAccount)) ||
                    (tagType == typeof(Azure.Arm.VirtualMachine)) ||
                    (tagType == typeof(Azure.Arm.NetworkSecurityGroup)))
                {
                    if (asmTreeNode.Checked)
                    {
                        return await AddASMNodeToARMTree(asmTreeNode);
                    }
                    else
                    {
                        await RemoveASMNodeFromARMTree(asmTreeNode);
                    }
                }
            }

            return null;
        }

        private async Task RemoveASMNodeFromARMTree(TreeNode sourceNode)
        {

            TreeNode targetResourceGroupNode = SeekResourceGroupTreeNode();
            if (targetResourceGroupNode != null)
            {
                TreeNode[] matchingNodes = targetResourceGroupNode.Nodes.Find(sourceNode.Name, true);
                foreach (TreeNode matchingNode in matchingNodes)
                {
                    if (matchingNode.Tag.GetType() == sourceNode.Tag.GetType())
                        await RemoveTreeNodeCascadeUp(matchingNode);
                    else if (matchingNode.Tag.GetType() == typeof(Azure.MigrationTarget.StorageAccount))
                    {
                        if (sourceNode.Tag.GetType() == typeof(Azure.Asm.StorageAccount))
                        {
                            await RemoveTreeNodeCascadeUp(matchingNode);
                        }
                        else if (sourceNode.Tag.GetType() == typeof(Azure.Arm.StorageAccount))
                        {
                            await RemoveTreeNodeCascadeUp(matchingNode);
                        }
                    }
                    else if (matchingNode.Tag.GetType() == typeof(Azure.MigrationTarget.VirtualMachine))
                    {
                        if (sourceNode.Tag.GetType() == typeof(Azure.Asm.VirtualMachine))
                        {
                            await RemoveTreeNodeCascadeUp(matchingNode);
                        }
                        else if (sourceNode.Tag.GetType() == typeof(Azure.Arm.VirtualMachine))
                        {
                            await RemoveTreeNodeCascadeUp(matchingNode);
                        }
                    }
                    else if (matchingNode.Tag.GetType() == typeof(Azure.MigrationTarget.NetworkSecurityGroup))
                    {
                        if (sourceNode.Tag.GetType() == typeof(Azure.Asm.NetworkSecurityGroup))
                        {
                            await RemoveTreeNodeCascadeUp(matchingNode);
                        }
                        else if (sourceNode.Tag.GetType() == typeof(Azure.Arm.NetworkSecurityGroup))
                        {
                            await RemoveTreeNodeCascadeUp(matchingNode);
                        }
                    }
                    else if (matchingNode.Tag.GetType() == typeof(Azure.MigrationTarget.VirtualNetwork))
                    {
                        if (sourceNode.Tag.GetType() == typeof(Azure.Asm.VirtualNetwork))
                        {
                            await RemoveTreeNodeCascadeUp(matchingNode);
                        }
                        else if (sourceNode.Tag.GetType() == typeof(Azure.Arm.VirtualNetwork))
                        {
                            await RemoveTreeNodeCascadeUp(matchingNode);
                        }
                    }
                    else if (matchingNode.Tag.GetType() == typeof(TreeNode))
                    {
                        TreeNode childTreeNode = (TreeNode)matchingNode.Tag;
                        if (sourceNode.Tag.GetType() == childTreeNode.Tag.GetType())
                            await RemoveTreeNodeCascadeUp(matchingNode);
                    }
                }
            }
        }

        private async Task RemoveTreeNodeCascadeUp(TreeNode treeNode)
        {
            TreeNode parentNode = treeNode.Parent;
            treeNode.Remove();
            await RemoveParentWhileNoChildren(parentNode);
        }

        private async Task RemoveParentWhileNoChildren(TreeNode treeNode)
        {
            if (treeNode != null)
            {
                if (treeNode.Nodes.Count == 0)
                {
                    TreeNode parentNode = treeNode.Parent;
                    treeNode.Remove();
                    await RemoveParentWhileNoChildren(parentNode);
                }
            }
        }

        internal TreeNode SeekARMChildTreeNode(string name, string text, object tag, bool allowCreated = false)
        {
            return SeekARMChildTreeNode(this.treeTargetARM.Nodes, name, text, tag, allowCreated);
        }

        internal TreeNode SeekARMChildTreeNode(TreeNodeCollection nodeCollection, string name, string text, object tag, bool allowCreated = false)
        {
            TreeNode[] childNodeMatch = nodeCollection.Find(name, false);

            foreach (TreeNode matchedNode in childNodeMatch)
            {
                if (matchedNode.Tag != null)
                {
                    if (matchedNode.Tag.GetType() == tag.GetType() && matchedNode.Text == text && matchedNode.Name == name)
                        return matchedNode;
                }
            }

            TreeNode childNode = null;
            if (allowCreated)
            {
                childNode = new TreeNode(text);
                childNode.Name = name;
                childNode.Tag = tag;
                if (tag.GetType() == typeof(Azure.MigrationTarget.ResourceGroup))
                {
                    childNode.ImageKey = "ResourceGroup";
                    childNode.SelectedImageKey = "ResourceGroup";
                }
                else if (tag.GetType() == typeof(Azure.MigrationTarget.StorageAccount))
                {
                    childNode.ImageKey = "StorageAccount";
                    childNode.SelectedImageKey = "StorageAccount";
                }
                else if (tag.GetType() == typeof(Azure.MigrationTarget.AvailabilitySet))
                {
                    childNode.ImageKey = "AvailabilitySet";
                    childNode.SelectedImageKey = "AvailabilitySet";
                }
                else if (tag.GetType() == typeof(Azure.MigrationTarget.VirtualMachine))
                {
                    childNode.ImageKey = "VirtualMachine";
                    childNode.SelectedImageKey = "VirtualMachine";
                }
                else if (tag.GetType() == typeof(Azure.MigrationTarget.VirtualNetwork))
                {
                    childNode.ImageKey = "VirtualNetwork";
                    childNode.SelectedImageKey = "VirtualNetwork";
                }
                else if (tag.GetType() == typeof(Azure.MigrationTarget.Subnet))
                {
                    childNode.ImageKey = "VirtualNetwork";
                    childNode.SelectedImageKey = "VirtualNetwork";
                }
                else if (tag.GetType() == typeof(Azure.MigrationTarget.NetworkSecurityGroup))
                {
                    childNode.ImageKey = "NetworkSecurityGroup";
                    childNode.SelectedImageKey = "NetworkSecurityGroup";
                }
                else if (tag.GetType() == typeof(Azure.MigrationTarget.Disk))
                {
                    childNode.ImageKey = "Disk";
                    childNode.SelectedImageKey = "Disk";
                }
                else if (tag.GetType() == typeof(Azure.MigrationTarget.NetworkInterface))
                {
                    childNode.ImageKey = "NetworkInterface";
                    childNode.SelectedImageKey = "NetworkInterface";
                }

                nodeCollection.Add(childNode);
                childNode.ExpandAll();
                return childNode;
            }
            return null;
        }

        private TreeNode GetResourceGroupTreeNode(TreeNode subscriptionNode, ResourceGroup resourceGroup)
        {
            foreach (TreeNode treeNode in subscriptionNode.Nodes)
            {
                if (treeNode.Tag != null)
                {
                    if (treeNode.Tag.GetType() == resourceGroup.GetType() && treeNode.Text == resourceGroup.ToString())
                        return treeNode;
                }
            }

            TreeNode tnResourceGroup = new TreeNode(resourceGroup.ToString());
            tnResourceGroup.Text = resourceGroup.ToString();
            tnResourceGroup.Tag = resourceGroup;
            tnResourceGroup.ImageKey = "ResourceGroup";
            tnResourceGroup.SelectedImageKey = "ResourceGroup";

            subscriptionNode.Nodes.Add(tnResourceGroup);
            tnResourceGroup.Expand();
            return tnResourceGroup;
        }

        private TreeNode GetAvailabilitySetTreeNode(TreeNode subscriptionNode, AvailabilitySet availabilitySet)
        {
            foreach (TreeNode treeNode in subscriptionNode.Nodes)
            {
                if (treeNode.Tag != null)
                {
                    if (treeNode.Tag.GetType() == availabilitySet.GetType() && treeNode.Text == availabilitySet.Name)
                        return treeNode;
                }
            }

            TreeNode tnAvailabilitySet = new TreeNode(availabilitySet.Name);
            tnAvailabilitySet.Text = availabilitySet.Name;
            tnAvailabilitySet.Tag = availabilitySet;
            tnAvailabilitySet.ImageKey = "AvailabilitySet";
            tnAvailabilitySet.SelectedImageKey = "AvailabilitySet";

            subscriptionNode.Nodes.Add(tnAvailabilitySet);
            tnAvailabilitySet.Expand();
            return tnAvailabilitySet;
        }

        private TreeNode GetAvailabilitySetNode(TreeNode subscriptionNode, AvailabilitySet availabilitySet)
        {
            foreach (TreeNode treeNode in subscriptionNode.Nodes)
            {
                if (treeNode.Tag != null)
                {
                    if (treeNode.Tag.GetType() == typeof(Azure.MigrationTarget.AvailabilitySet) && treeNode.Text == availabilitySet.Name)
                        return treeNode;
                }
            }

            TreeNode tnAvailabilitySet = new TreeNode(availabilitySet.Name);
            tnAvailabilitySet.Text = availabilitySet.Name;
            tnAvailabilitySet.Tag = new Azure.MigrationTarget.AvailabilitySet(this.AzureContextTargetARM, availabilitySet);
            tnAvailabilitySet.ImageKey = "AvailabilitySet";
            tnAvailabilitySet.SelectedImageKey = "AvailabilitySet";

            subscriptionNode.Nodes.Add(tnAvailabilitySet);
            tnAvailabilitySet.Expand();
            return tnAvailabilitySet;
        }
        private TreeNode GetAvailabilitySetNode(TreeNode subscriptionNode, Azure.Asm.VirtualMachine virtualMachine)
        {
            string availabilitySetName = String.Empty;

            if (virtualMachine.AvailabilitySetName != String.Empty)
                availabilitySetName = virtualMachine.AvailabilitySetName;
            else
                availabilitySetName = virtualMachine.CloudServiceName;

            foreach (TreeNode treeNode in subscriptionNode.Nodes)
            {
                if (treeNode.Tag != null)
                {
                    if (treeNode.Tag.GetType() == typeof(Azure.MigrationTarget.AvailabilitySet) && treeNode.Text == availabilitySetName)
                        return treeNode;
                }
            }

            TreeNode tnAvailabilitySet = new TreeNode(availabilitySetName);
            tnAvailabilitySet.Text = availabilitySetName;
            tnAvailabilitySet.Tag = new Azure.MigrationTarget.AvailabilitySet(this.AzureContextTargetARM, availabilitySetName);
            tnAvailabilitySet.ImageKey = "AvailabilitySet";
            tnAvailabilitySet.SelectedImageKey = "AvailabilitySet";

            subscriptionNode.Nodes.Add(tnAvailabilitySet);
            tnAvailabilitySet.Expand();
            return tnAvailabilitySet;
        }

        private TreeNode GetDataCenterTreeViewNode(TreeNode subscriptionNode, string dataCenter, string containerName)
        {
            TreeNode dataCenterNode = null;

            foreach (TreeNode treeNode in subscriptionNode.Nodes)
            {
                if (treeNode.Text == dataCenter && treeNode.Tag.ToString() == "DataCenter")
                {
                    dataCenterNode = treeNode;

                    foreach (TreeNode dataCenterContainerNode in treeNode.Nodes)
                    {
                        if (dataCenterContainerNode.Text == containerName)
                            return dataCenterContainerNode;
                    }
                }
            }

            if (dataCenterNode == null)
            {
                dataCenterNode = new TreeNode(dataCenter);
                dataCenterNode.Tag = "DataCenter";
                subscriptionNode.Nodes.Add(dataCenterNode);
                dataCenterNode.Expand();
            }

            TreeNode containerNode = new TreeNode(containerName);
            dataCenterNode.Nodes.Add(containerNode);
            containerNode.Expand();

            return containerNode;
        }

        private void FillUpIfFullDown(TreeNode node)
        {
            if (IsSelectedFullDown(node) && (node.Parent != null))
            {
                node = node.Parent;

                while (node != null)
                {
                    if (AllChildrenChecked(node))
                    {
                        node.Checked = true;
                        node = node.Parent;
                    }
                    else
                        node = null;
                }
            }
        }

        private bool AllChildrenChecked(TreeNode node)
        {
            foreach (TreeNode childNode in node.Nodes)
                if (!childNode.Checked)
                    return false;

            return true;
        }

        private bool IsSelectedFullDown(TreeNode node)
        {
            if (!node.Checked)
                return false;

            foreach (TreeNode childNode in node.Nodes)
            {
                if (!IsSelectedFullDown(childNode))
                    return false;
            }

            return true;
        }

        private async Task RecursiveCheckToggleDown(TreeNode node, bool isChecked)
        {
            if (node.Checked != isChecked)
            {
                node.Checked = isChecked;
            }

            foreach (TreeNode subNode in node.Nodes)
            {
                await RecursiveCheckToggleDown(subNode, isChecked);
            }
        }
        private async Task RecursiveCheckToggleUp(TreeNode node, bool isChecked)
        {
            if (node.Checked != isChecked)
            {
                node.Checked = isChecked;
            }

            if (node.Parent != null)
                await RecursiveCheckToggleUp(node.Parent, isChecked);
        }

        private async Task<TreeNode> AddASMNodeToARMTree(TreeNode parentNode)
        {
            TreeNode targetResourceGroupNode = SeekResourceGroupTreeNode();

            Type tagType = parentNode.Tag.GetType();
            if (tagType == typeof(Azure.Asm.VirtualNetwork))
            {
                Azure.Asm.VirtualNetwork asmVirtualNetwork = (Azure.Asm.VirtualNetwork)parentNode.Tag;
                Azure.MigrationTarget.VirtualNetwork targetVirtualNetwork = new Azure.MigrationTarget.VirtualNetwork(this.AzureContextTargetARM, asmVirtualNetwork);
                TreeNode virtualNetworkNode = SeekARMChildTreeNode(targetResourceGroupNode.Nodes, parentNode.Name, targetVirtualNetwork.ToString(), targetVirtualNetwork, true);

                foreach (Azure.Asm.Subnet asmSubnet in asmVirtualNetwork.Subnets)
                {
                    Azure.MigrationTarget.Subnet targetSubnet = new Azure.MigrationTarget.Subnet(this.AzureContextTargetARM, targetVirtualNetwork, asmSubnet);
                    targetVirtualNetwork.TargetSubnets.Add(targetSubnet);

                    TreeNode subnetNode = SeekARMChildTreeNode(virtualNetworkNode.Nodes, asmSubnet.Name, targetSubnet.ToString(), targetSubnet, true);
                }

                targetResourceGroupNode.ExpandAll();
                return virtualNetworkNode;
            }
            else if (tagType == typeof(Azure.Asm.StorageAccount))
            {
                Azure.Asm.StorageAccount asmStorageAccount = (Azure.Asm.StorageAccount)parentNode.Tag;
                Azure.MigrationTarget.StorageAccount targetStorageAccount = new Azure.MigrationTarget.StorageAccount(_AzureContextTargetARM, asmStorageAccount);

                TreeNode storageAccountNode = SeekARMChildTreeNode(targetResourceGroupNode.Nodes, parentNode.Name, targetStorageAccount.ToString(), targetStorageAccount, true);

                targetResourceGroupNode.ExpandAll();
                return storageAccountNode;
            }
            else if (tagType == typeof(Azure.Asm.NetworkSecurityGroup))
            {
                Azure.Asm.NetworkSecurityGroup asmNetworkSecurityGroup = (Azure.Asm.NetworkSecurityGroup)parentNode.Tag;

                Azure.MigrationTarget.NetworkSecurityGroup targetNetworkSecurityGroup = new Azure.MigrationTarget.NetworkSecurityGroup(this.AzureContextTargetARM, asmNetworkSecurityGroup);
                TreeNode networkSecurityGroupNode = SeekARMChildTreeNode(targetResourceGroupNode.Nodes, asmNetworkSecurityGroup.Name, targetNetworkSecurityGroup.ToString(), targetNetworkSecurityGroup, true);

                targetResourceGroupNode.ExpandAll();
                return networkSecurityGroupNode;
            }
            else if (tagType == typeof(Azure.Arm.VirtualNetwork))
            {
                Azure.Arm.VirtualNetwork armVirtualNetwork = (Azure.Arm.VirtualNetwork)parentNode.Tag;
                Azure.MigrationTarget.VirtualNetwork targetVirtualNetwork = new Azure.MigrationTarget.VirtualNetwork(this.AzureContextTargetARM, armVirtualNetwork);
                TreeNode virtualNetworkNode = SeekARMChildTreeNode(targetResourceGroupNode.Nodes, parentNode.Name, targetVirtualNetwork.ToString(), targetVirtualNetwork, true);

                foreach (Azure.Arm.Subnet armSubnet in armVirtualNetwork.Subnets)
                {
                    Azure.MigrationTarget.Subnet targetSubnet = new Azure.MigrationTarget.Subnet(this.AzureContextTargetARM, targetVirtualNetwork, armSubnet);
                    targetVirtualNetwork.TargetSubnets.Add(targetSubnet);

                    TreeNode subnetNode = SeekARMChildTreeNode(virtualNetworkNode.Nodes, armSubnet.Name, targetSubnet.ToString(), targetSubnet, true);
                }

                targetResourceGroupNode.ExpandAll();
                return virtualNetworkNode;
            }
            else if (tagType == typeof(Azure.Arm.StorageAccount))
            {
                Azure.Arm.StorageAccount armStorageAccount = (Azure.Arm.StorageAccount)parentNode.Tag;
                Azure.MigrationTarget.StorageAccount targetStorageAccount = new Azure.MigrationTarget.StorageAccount(_AzureContextTargetARM, armStorageAccount);

                TreeNode storageAccountNode = SeekARMChildTreeNode(targetResourceGroupNode.Nodes, parentNode.Name, targetStorageAccount.ToString(), targetStorageAccount, true);

                targetResourceGroupNode.ExpandAll();
                return storageAccountNode;
            }
            else if (tagType == typeof(Azure.Asm.VirtualMachine))
            {
                Azure.Asm.VirtualMachine asmVirtualMachine = (Azure.Asm.VirtualMachine)parentNode.Tag;

                TreeNode virtualMachineParentNode = targetResourceGroupNode;
                virtualMachineParentNode = GetAvailabilitySetNode(targetResourceGroupNode, asmVirtualMachine);

                Azure.MigrationTarget.VirtualMachine targetVirtualMachine = new Azure.MigrationTarget.VirtualMachine(this.AzureContextTargetARM, asmVirtualMachine);
                TreeNode virtualMachineNode = SeekARMChildTreeNode(virtualMachineParentNode.Nodes, asmVirtualMachine.RoleName, asmVirtualMachine.RoleName, targetVirtualMachine, true);

                // If Null, default Target Virtual Network and Subnet to be that of the original VNet/Subnet (if found in Migration)
                if (targetVirtualMachine.TargetVirtualNetwork == null)
                {
                    Azure.MigrationTarget.VirtualNetwork targetVirtualNetwork = SeekTargetVirtualNetwork(asmVirtualMachine.SourceVirtualNetwork);
                    targetVirtualMachine.TargetVirtualNetwork = targetVirtualNetwork;

                    if (targetVirtualNetwork != null)
                    {
                        foreach (Azure.MigrationTarget.Subnet targetSubnet in targetVirtualNetwork.TargetSubnets)
                        {
                            if (targetSubnet.Source != null && targetSubnet.Source.GetType() == typeof(Azure.Asm.Subnet))
                            {
                                Azure.Asm.Subnet sourceSubnet = (Azure.Asm.Subnet)targetSubnet.Source;
                                if (sourceSubnet.Name == asmVirtualMachine.SourceSubnet.Name)
                                    targetVirtualMachine.TargetSubnet = targetSubnet;
                            }
                        }
                    }
                }

                if (targetVirtualMachine.OSVirtualHardDisk.TargetStorageAccount == null)
                {
                    targetVirtualMachine.OSVirtualHardDisk.TargetStorageAccount = SeekTargetStorageAccount((Azure.Asm.StorageAccount)targetVirtualMachine.OSVirtualHardDisk.SourceStorageAccount);
                }

                foreach (Azure.MigrationTarget.Disk migrationDataDisk in targetVirtualMachine.DataDisks)
                {
                    if (migrationDataDisk.TargetStorageAccount == null)
                    {
                        migrationDataDisk.TargetStorageAccount = SeekTargetStorageAccount((Azure.Asm.StorageAccount)migrationDataDisk.SourceStorageAccount);
                    }
                }

                foreach (Azure.Asm.Disk asmDataDisk in asmVirtualMachine.DataDisks)
                {
                    Azure.MigrationTarget.Disk migrationDisk = new Azure.MigrationTarget.Disk(asmDataDisk);
                    TreeNode dataDiskNode = SeekARMChildTreeNode(virtualMachineNode.Nodes, migrationDisk.ToString(), migrationDisk.ToString(), migrationDisk, true);
                }

                foreach (Azure.Asm.NetworkInterface asmNetworkInterface in asmVirtualMachine.NetworkInterfaces)
                {
                    Azure.MigrationTarget.NetworkInterface migrationNetworkInterface = new Azure.MigrationTarget.NetworkInterface(this.AzureContextTargetARM, asmNetworkInterface);
                    TreeNode dataDiskNode = SeekARMChildTreeNode(virtualMachineNode.Nodes, migrationNetworkInterface.ToString(), migrationNetworkInterface.ToString(), migrationNetworkInterface, true);
                }

                targetResourceGroupNode.ExpandAll();
                return virtualMachineNode;
            }
            else if (tagType == typeof(Azure.Arm.VirtualMachine))
            {
                Azure.Arm.VirtualMachine armVirtualMachine = (Azure.Arm.VirtualMachine)parentNode.Tag;

                TreeNode virtualMachineParentNode = targetResourceGroupNode;
                if (armVirtualMachine.AvailabilitySet != null)
                {
                    TreeNode targetAvailabilitySetNode = GetAvailabilitySetNode(targetResourceGroupNode, armVirtualMachine.AvailabilitySet);
                    virtualMachineParentNode = targetAvailabilitySetNode;
                }

                Azure.MigrationTarget.VirtualMachine targetVirtualMachine = new Azure.MigrationTarget.VirtualMachine(this.AzureContextTargetARM, armVirtualMachine);
                TreeNode virtualMachineNode = SeekARMChildTreeNode(virtualMachineParentNode.Nodes, armVirtualMachine.Name, targetVirtualMachine.ToString(), targetVirtualMachine, true);

                // If Null, default Target Virtual Network and Subnet to be that of the original VNet/Subnet (if found in Migration)
                if (targetVirtualMachine.TargetVirtualNetwork == null)
                {
                    Azure.MigrationTarget.VirtualNetwork targetVirtualNetwork = SeekTargetVirtualNetwork(armVirtualMachine.PrimaryNetworkInterface.PrimaryIpConfiguration.VirtualNetwork);
                    targetVirtualMachine.TargetVirtualNetwork = targetVirtualNetwork;

                    if (targetVirtualNetwork != null)
                    {
                        foreach (Azure.MigrationTarget.Subnet targetSubnet in targetVirtualNetwork.TargetSubnets)
                        {
                            if (targetSubnet.Source != null && targetSubnet.Source.GetType() == typeof(Azure.Arm.Subnet))
                            {
                                Azure.Arm.Subnet sourceSubnet = (Azure.Arm.Subnet)targetSubnet.Source;
                                if (sourceSubnet.Name == armVirtualMachine.PrimaryNetworkInterface.PrimaryIpConfiguration.Subnet.Name)
                                    targetVirtualMachine.TargetSubnet = targetSubnet;
                            }
                        }
                    }
                }
                
                foreach (Azure.Arm.Disk armDataDisk in armVirtualMachine.DataDisks)
                {
                    Azure.MigrationTarget.Disk migrationDisk = new Azure.MigrationTarget.Disk(armDataDisk);
                    TreeNode dataDiskNode = SeekARMChildTreeNode(virtualMachineNode.Nodes, migrationDisk.ToString(), migrationDisk.ToString(), migrationDisk, true);
                }

                foreach (Azure.Arm.NetworkInterface armNetworkInterface in armVirtualMachine.NetworkInterfaces)
                {
                    if (!armNetworkInterface.IsPrimary)
                    {
                        Azure.MigrationTarget.NetworkInterface migrationNetworkInterface = new Azure.MigrationTarget.NetworkInterface(this.AzureContextTargetARM, armNetworkInterface);
                        TreeNode dataDiskNode = SeekARMChildTreeNode(virtualMachineNode.Nodes, migrationNetworkInterface.ToString(), migrationNetworkInterface.ToString(), migrationNetworkInterface, true);
                    }
                }

                targetResourceGroupNode.ExpandAll();
                return virtualMachineNode;
            }
            else if (tagType == typeof(Azure.Arm.NetworkSecurityGroup))
            {
                Azure.Arm.NetworkSecurityGroup armNetworkSecurityGroup = (Azure.Arm.NetworkSecurityGroup)parentNode.Tag;
                Azure.MigrationTarget.NetworkSecurityGroup targetNetworkSecurityGroup = new Azure.MigrationTarget.NetworkSecurityGroup(this.AzureContextTargetARM, armNetworkSecurityGroup);
                TreeNode networkSecurityGroupNode = SeekARMChildTreeNode(targetResourceGroupNode.Nodes, armNetworkSecurityGroup.Name, targetNetworkSecurityGroup.ToString(), targetNetworkSecurityGroup, true);

                targetResourceGroupNode.ExpandAll();
                return networkSecurityGroupNode;
            }
            else
                throw new Exception("Unhandled Node Type in AddASMNodeToARMTree: " + tagType);

        }

        private Azure.MigrationTarget.StorageAccount SeekTargetStorageAccount(Azure.Asm.StorageAccount sourceStorageAccount)
        {
            if (sourceStorageAccount == null)
                return null;

            TreeNode resourceGroupTreeNode = SeekResourceGroupTreeNode();

            foreach (TreeNode treeNode in resourceGroupTreeNode.Nodes)
            {
                if (treeNode.Tag != null && treeNode.Tag.GetType() == typeof(Azure.MigrationTarget.StorageAccount))
                {
                    Azure.MigrationTarget.StorageAccount targetStorageAccount = (Azure.MigrationTarget.StorageAccount)treeNode.Tag;
                    if (targetStorageAccount.SourceAccount != null && targetStorageAccount.SourceAccount.GetType() == sourceStorageAccount.GetType())
                    {
                        Azure.Asm.StorageAccount sourceAsmStorageAccount = (Azure.Asm.StorageAccount)targetStorageAccount.SourceAccount;
                        if (sourceStorageAccount.Name == sourceAsmStorageAccount.Name)
                            return targetStorageAccount;
                    }
                }
            }

            return null;
        }

        private Azure.MigrationTarget.VirtualNetwork SeekTargetVirtualNetwork(IVirtualNetwork virtualNetwork)
        {
            if (virtualNetwork == null)
                return null;

            TreeNode resourceGroupTreeNode = SeekResourceGroupTreeNode();

            foreach (TreeNode treeNode in resourceGroupTreeNode.Nodes)
            {
                if (treeNode.Tag != null && treeNode.Tag.GetType() == typeof(Azure.MigrationTarget.VirtualNetwork))
                {
                    Azure.MigrationTarget.VirtualNetwork targetVirtualNetwork = (Azure.MigrationTarget.VirtualNetwork)treeNode.Tag;
                    if (targetVirtualNetwork.Source.GetType() == virtualNetwork.GetType())
                    {
                        if (targetVirtualNetwork.Source.GetType() == typeof(Azure.Asm.VirtualNetwork))
                        {
                            Azure.Asm.VirtualNetwork sourceAsmVirtualNetwork = (Azure.Asm.VirtualNetwork)virtualNetwork;
                            Azure.Asm.VirtualNetwork targetAsmVirtualNetwork = (Azure.Asm.VirtualNetwork)targetVirtualNetwork.Source;
                            if (targetAsmVirtualNetwork.Name == sourceAsmVirtualNetwork.Name)
                                return targetVirtualNetwork;
                        }
                        else if (targetVirtualNetwork.Source.GetType() == typeof(Azure.Arm.VirtualNetwork))
                        {
                            Azure.Arm.VirtualNetwork sourceArmVirtualNetwork = (Azure.Arm.VirtualNetwork)virtualNetwork;
                            Azure.Arm.VirtualNetwork targetArmVirtualNetwork = (Azure.Arm.VirtualNetwork)targetVirtualNetwork.Source;
                            if (targetArmVirtualNetwork.Name == sourceArmVirtualNetwork.Name)
                                return targetVirtualNetwork;
                        }
                    }
                }
            }

            return null;
        }

        private TreeNode SeekResourceGroupTreeNode()
        {
            TreeNode targetResourceGroupNode = SeekARMChildTreeNode(treeTargetARM.Nodes, _TargetResourceGroup.ToString(), _TargetResourceGroup.ToString(), _TargetResourceGroup, true);
            return targetResourceGroupNode;
        }

        #endregion

        #region Form Controls

        #region Export Button

        public async void Export()
        {
            await SaveSubscriptionSettings(_AzureContextSourceASM.AzureSubscription);
        }

        #endregion

        #endregion

        #region Form Events

        private async void AsmToArmForm_Load(object sender, EventArgs e)
        {
            LogProvider.WriteLog("AsmToArmForm_Load", "Program start");

            AsmToArmForm_Resize(null, null);
            ResetForm();

            try
            {
                _AzureContextSourceASM.AzureEnvironment = (AzureEnvironment)Enum.Parse(typeof(AzureEnvironment), app.Default.AzureEnvironment);
            }
            catch
            {
                _AzureContextSourceASM.AzureEnvironment = AzureEnvironment.AzureCloud;
            }

            await AlertIfNewVersionAvailable(); // check if there a new version of the app
        }

        private void AsmToArmForm_Resize(object sender, EventArgs e)
        {
            tabSourceResources.Height = 265;
            treeSourceASM.Width = tabSourceResources.Width - 10;
            treeSourceASM.Height = tabSourceResources.Height - 30;
            treeSourceARM.Width = tabSourceResources.Width - 10;
            treeSourceARM.Height = tabSourceResources.Height - 30;
            treeTargetARM.Height = 250;
        }

        #endregion

        #region Subscription Settings Read / Save Methods

        private async Task SaveSubscriptionSettings(AzureSubscription azureSubscription)
        {
            // If save selection option is enabled
            if (app.Default.SaveSelection && azureSubscription != null)
            {
                await _saveSelectionProvider.Save(azureSubscription.SubscriptionId, _SelectedNodes);
            }
        }

        private async Task ReadSubscriptionSettings(AzureSubscription azureSubscription)
        {
            // If save selection option is enabled
            if (app.Default.SaveSelection)
            {
                StatusProvider.UpdateStatus("BUSY: Reading saved selection");
                await _saveSelectionProvider.Read(azureSubscription.SubscriptionId, _AzureContextSourceASM.AzureRetriever, AzureContextTargetARM.AzureRetriever, treeSourceASM);
                UpdateExportItemsCount();
            }
        }

        #endregion

        private void UpdateExportItemsCount()
        {
            Int32 selectedExportCount = 0;

            if (_SelectedNodes != null)
            {
                selectedExportCount = _SelectedNodes.Count();
            }
        }

        private void SeekAlertSourceRecursive(object sourceObject, TreeNodeCollection nodes)
        {
            foreach (TreeNode treeNode in nodes)
            {
                if (treeNode.Tag != null)
                {
                    object nodeObject = null;

                    if (treeNode.Tag.GetType() == typeof(TreeNode))
                    {
                        TreeNode asmTreeNode = (TreeNode)treeNode.Tag;
                        nodeObject = asmTreeNode.Tag;
                    }
                    else
                    {
                        nodeObject = treeNode.Tag;
                    }

                    // Note, this could probably be object compares, but was written this was to get it done.  Possible future change to object compares
                    if (nodeObject.GetType() == sourceObject.GetType())
                    {
                        if (sourceObject.GetType() == typeof(Azure.MigrationTarget.ResourceGroup))
                            treeTargetARM.SelectedNode = treeNode;
                        else if (sourceObject.GetType() == typeof(Azure.Arm.VirtualMachine))
                        {
                            Azure.Arm.VirtualMachine sourceMachine = (Azure.Arm.VirtualMachine)sourceObject;
                            Azure.Arm.VirtualMachine nodeMachine = (Azure.Arm.VirtualMachine)nodeObject;
                            if (sourceMachine.Name == nodeMachine.Name)
                                treeTargetARM.SelectedNode = treeNode;
                        }
                        else if (sourceObject.GetType() == typeof(Azure.Asm.VirtualMachine))
                        {
                            Azure.Asm.VirtualMachine sourceMachine = (Azure.Asm.VirtualMachine)sourceObject;
                            Azure.Asm.VirtualMachine nodeMachine = (Azure.Asm.VirtualMachine)nodeObject;
                            if (sourceMachine.RoleName == nodeMachine.RoleName)
                                treeTargetARM.SelectedNode = treeNode;
                        }
                        else if (sourceObject.GetType() == typeof(Azure.Asm.Disk))
                        {
                            Azure.Asm.Disk sourceDisk = (Azure.Asm.Disk)sourceObject;
                            Azure.Asm.Disk nodeDisk = (Azure.Asm.Disk)nodeObject;
                            if (sourceDisk.DiskName == nodeDisk.DiskName)
                                treeTargetARM.SelectedNode = treeNode;
                        }
                        else if (sourceObject.GetType() == typeof(Azure.Arm.Disk))
                        {
                            Azure.Arm.Disk sourceDisk = (Azure.Arm.Disk)sourceObject;
                            Azure.Arm.Disk nodeDisk = (Azure.Arm.Disk)nodeObject;
                            if (sourceDisk.Name == nodeDisk.Name)
                                treeTargetARM.SelectedNode = treeNode;
                        }
                    }
                }
                SeekAlertSourceRecursive(sourceObject, treeNode.Nodes);
            }
        }

        public override void SeekAlertSource(object sourceObject)
        {
            SeekAlertSourceRecursive(sourceObject, treeTargetARM.Nodes);
        }

        public override void PostTelemetryRecord()
        {
            _telemetryProvider.PostTelemetryRecord((AzureGenerator) this.TemplateGenerator);
        }
    }
}
