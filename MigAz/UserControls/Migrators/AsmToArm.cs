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
        private ResourceGroup _TargetResourceGroup;
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

            _TargetResourceGroup = new ResourceGroup(this.AzureContextSourceASM, "Target Resource Group");

            azureLoginContextViewerASM.Bind(_AzureContextSourceASM);
            azureLoginContextViewerARM.Bind(_AzureContextTargetARM);

            this.TemplateGenerator = new AsmToArmGenerator(_AzureContextSourceASM.AzureSubscription, _AzureContextTargetARM.AzureSubscription, _TargetResourceGroup, LogProvider, StatusProvider, _telemetryProvider, _appSettingsProvider);
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

                    TreeNode subscriptionNode = new TreeNode(sender.AzureSubscription.Name);
                    treeASM.Nodes.Add(subscriptionNode);
                    subscriptionNode.Expand();

                    List<Azure.Asm.VirtualNetwork> asmVirtualNetworks = await _AzureContextSourceASM.AzureRetriever.GetAzureAsmVirtualNetworks();
                    foreach (Azure.Asm.VirtualNetwork asmVirtualNetwork in asmVirtualNetworks)
                    {
                        if (asmVirtualNetwork.HasNonGatewaySubnet)
                        {
                            TreeNode parentNode = MigAzTreeView.GetDataCenterTreeViewNode(subscriptionNode, asmVirtualNetwork.Location, "Virtual Networks");
                            TreeNode tnVirtualNetwork = new TreeNode(asmVirtualNetwork.Name);
                            tnVirtualNetwork.Name = asmVirtualNetwork.Name;
                            tnVirtualNetwork.Tag = asmVirtualNetwork;
                            parentNode.Nodes.Add(tnVirtualNetwork);
                            parentNode.Expand();
                        }
                    }

                    foreach (Azure.Asm.StorageAccount asmStorageAccount in await _AzureContextSourceASM.AzureRetriever.GetAzureAsmStorageAccounts())
                    {
                        TreeNode parentNode = MigAzTreeView.GetDataCenterTreeViewNode(subscriptionNode, asmStorageAccount.GeoPrimaryRegion, "Storage Accounts");
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
                            TreeNode parentNode = MigAzTreeView.GetDataCenterTreeViewNode(subscriptionNode, asmCloudService.Location, "Cloud Services");
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
                        TreeNode parentNode = MigAzTreeView.GetDataCenterTreeViewNode(subscriptionNode, asmNetworkSecurityGroup.Location, "Network Security Groups");
                        TreeNode tnStorageAccount = new TreeNode(asmNetworkSecurityGroup.Name);
                        tnStorageAccount.Name = tnStorageAccount.Text;
                        tnStorageAccount.Tag = asmNetworkSecurityGroup;
                        parentNode.Nodes.Add(tnStorageAccount);
                        parentNode.Expand();
                    }

                    subscriptionNode.ExpandAll();
                    await ReadSubscriptionSettings(sender.AzureSubscription);

                    treeARM.Enabled = true;
                    treeASM.Enabled = true;
                }
            }
            catch (Exception exc)
            {
                UnhandledExceptionDialog unhandledException = new UnhandledExceptionDialog(LogProvider, exc);
                unhandledException.ShowDialog();
            }
            
            StatusProvider.UpdateStatus("Ready");
        }

        internal void ChangeAzureContext()
        {
            azureLoginContextViewerASM.ChangeAzureContext();
        }

        private void ResetForm()
        {
            treeASM.Nodes.Clear();
            treeARM.Nodes.Clear();
            _SelectedNodes.Clear();
            UpdateExportItemsCount();
            _PropertyPanel.Clear();
            treeARM.Enabled = false;
            treeASM.Enabled = false;
        }

        #region Properties

        public ResourceGroup TargetResourceGroup
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
            string currentVersion = "2.0.0.0";
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
                        foreach (TreeNode treeNode in treeASM.Nodes.Find(asmVirtualMachine.VirtualNetworkName, true))
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

                    foreach (TreeNode treeNode in treeASM.Nodes.Find(asmVirtualMachine.OSVirtualHardDisk.StorageAccountName, true))
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
                        foreach (TreeNode treeNode in treeASM.Nodes.Find(dataDisk.StorageAccountName, true))
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
                        foreach (TreeNode treeNode in treeASM.Nodes.Find(asmVirtualMachine.NetworkSecurityGroup.Name, true))
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
                            foreach (TreeNode treeNode in treeASM.Nodes.Find(asmSubnet.NetworkSecurityGroup.Name, true))
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
            if (parentNode.Checked && parentNode.Tag != null && (parentNode.Tag.GetType() == typeof(Azure.Asm.NetworkSecurityGroup) || parentNode.Tag.GetType() == typeof(Azure.Asm.VirtualNetwork) || parentNode.Tag.GetType() == typeof(Azure.Asm.StorageAccount) || parentNode.Tag.GetType() == typeof(Azure.Asm.VirtualMachine)))
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

            if (e.Node.Tag != null)
            {
                if (e.Node.Tag.GetType() == typeof(Azure.Asm.VirtualMachine))
                {
                    Azure.Asm.VirtualMachine asmVirtualMachine = (Azure.Asm.VirtualMachine)e.Node.Tag;

                    if (asmVirtualMachine.TargetVirtualNetwork == null)
                    {

                    }

                    if (asmVirtualMachine.OSVirtualHardDisk.TargetStorageAccount == null)
                    {

                    }

                    foreach (Azure.Asm.Disk asmDisk in asmVirtualMachine.DataDisks)
                    {
                        if (asmDisk.TargetStorageAccount == null)
                        {

                        }
                    }
                }
            }

            TreeNode resultUpdateARMTree = await UpdateARMTree(e.Node);

            if (_SourceAsmNode != null && _SourceAsmNode == e.Node)
            {
                if (e.Node.Checked)
                {
                    await MigAzTreeView.RecursiveCheckToggleDown(e.Node, e.Node.Checked);
                    MigAzTreeView.FillUpIfFullDown(e.Node);
                    treeASM.SelectedNode = e.Node;
                }
                else
                {
                    await MigAzTreeView.RecursiveCheckToggleUp(e.Node, e.Node.Checked);
                    await MigAzTreeView.RecursiveCheckToggleDown(e.Node, e.Node.Checked);
                }

                _SelectedNodes = this.GetSelectedNodes(treeASM);
                UpdateExportItemsCount();
                await this.TemplateGenerator.UpdateArtifacts(GetAsmArtifacts());

                _SourceAsmNode = null;

                if (resultUpdateARMTree != null)
                    treeARM.SelectedNode = resultUpdateARMTree;
            }
        }

        private ExportArtifacts GetAsmArtifacts()
        {
            ExportArtifacts artifacts = new ExportArtifacts();
            foreach (TreeNode selectedNode in _SelectedNodes)
            {
                Type tagType = selectedNode.Tag.GetType();
                if (tagType == typeof(Azure.Asm.NetworkSecurityGroup))
                {
                    artifacts.NetworkSecurityGroups.Add((INetworkSecurityGroup)selectedNode.Tag);
                }
                else if (tagType == typeof(Azure.Asm.VirtualNetwork))
                {
                    artifacts.VirtualNetworks.Add((IVirtualNetwork)selectedNode.Tag);
                }
                else if (tagType == typeof(Azure.Asm.StorageAccount))
                {
                    artifacts.StorageAccounts.Add((IStorageAccount)selectedNode.Tag);
                }
                else if (tagType == typeof(Azure.Asm.VirtualMachine))
                {
                    artifacts.VirtualMachines.Add((IVirtualMachine)selectedNode.Tag);
                }
            }

            return artifacts;
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
                if (e.Node.Tag.GetType() == typeof(TreeNode))
                {
                    TreeNode asmTreeNode = (TreeNode)e.Node.Tag;

                    if (asmTreeNode.Tag != null)
                    {
                        _PropertyPanel.ResourceText = e.Node.Text;

                        if (asmTreeNode.Tag.GetType() == typeof(Azure.Asm.VirtualNetwork))
                        {
                            this._PropertyPanel.ResourceImage = imageList1.Images["VirtualNetwork"];

                            VirtualNetworkProperties properties = new VirtualNetworkProperties();
                            properties.PropertyChanged += Properties_PropertyChanged;
                            properties.Bind(e.Node);
                            _PropertyPanel.PropertyDetailControl = properties;
                        }
                        else if (asmTreeNode.Tag.GetType() == typeof(Azure.Asm.StorageAccount))
                        {
                            this._PropertyPanel.ResourceImage = imageList1.Images["StorageAccount"];

                            Azure.Asm.StorageAccount storageAccount = (Azure.Asm.StorageAccount)asmTreeNode.Tag;
                            _PropertyPanel.ResourceText = storageAccount.Name;

                            StorageAccountProperties properties = new StorageAccountProperties();
                            properties.PropertyChanged += Properties_PropertyChanged;
                            properties.Bind(this._AzureContextTargetARM, e.Node);
                            _PropertyPanel.PropertyDetailControl = properties;
                        }
                        else if (asmTreeNode.Tag.GetType() == typeof(Azure.Asm.VirtualMachine))
                        {
                            this._PropertyPanel.ResourceImage = imageList1.Images["VirtualMachine"];

                            VirtualMachineProperties properties = new VirtualMachineProperties(this.LogProvider);
                            properties.AllowManangedDisk = (await _AzureContextSourceASM.AzureRetriever.GetAzureARMManagedDisks() != null);
                            properties.PropertyChanged += Properties_PropertyChanged;
                            await properties.Bind(e.Node, this);
                            _PropertyPanel.PropertyDetailControl = properties;
                        }
                        else if (asmTreeNode.Tag.GetType() == typeof(Azure.Asm.NetworkSecurityGroup))
                        {
                            this._PropertyPanel.ResourceImage = imageList1.Images["NetworkSecurityGroup"];

                            NetworkSecurityGroupProperties properties = new NetworkSecurityGroupProperties();
                            properties.PropertyChanged += Properties_PropertyChanged;
                            properties.Bind(e.Node, this);
                            _PropertyPanel.PropertyDetailControl = properties;
                        }
                    }
                }
                if (e.Node.Tag.GetType() == typeof(Azure.Asm.Subnet))
                {
                    this._PropertyPanel.ResourceImage = imageList1.Images["VirtualNetwork"];

                    Azure.Asm.Subnet asmSubnet = (Azure.Asm.Subnet)e.Node.Tag;

                    SubnetProperties properties = new SubnetProperties();
                    properties.PropertyChanged += Properties_PropertyChanged;
                    properties.Bind(e.Node);
                    _PropertyPanel.PropertyDetailControl = properties;
                }
                else if (e.Node.Tag.GetType() == typeof(Azure.Asm.Disk))
                {
                    Azure.Asm.Disk asmDisk = (Azure.Asm.Disk)e.Node.Tag;

                    this._PropertyPanel.ResourceImage = imageList1.Images["Disk"];

                    DiskProperties properties = new DiskProperties(this.LogProvider);
                    properties.AllowManangedDisk = (await _AzureContextSourceASM.AzureRetriever.GetAzureARMManagedDisks() != null);
                    properties.PropertyChanged += Properties_PropertyChanged;
                    properties.Bind(this, e.Node);
                    _PropertyPanel.PropertyDetailControl = properties;
                }
                else if (e.Node.Tag.GetType() == typeof(AvailabilitySet))
                {
                    this._PropertyPanel.ResourceImage = imageList1.Images["AvailabilitySet"];

                    AvailabilitySetProperties properties = new AvailabilitySetProperties();
                    properties.PropertyChanged += Properties_PropertyChanged;
                    properties.Bind(e.Node);
                    _PropertyPanel.PropertyDetailControl = properties;
                }
                else if (e.Node.Tag.GetType() == typeof(ResourceGroup))
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
                await this.TemplateGenerator.UpdateArtifacts(GetAsmArtifacts());
        }

        private async Task<TreeNode> UpdateARMTree(TreeNode asmTreeNode)
        {
            if (asmTreeNode.Tag != null)
            {
                Type tagType = asmTreeNode.Tag.GetType();
                if ((tagType == typeof(Azure.Asm.VirtualNetwork)) ||
                    (tagType == typeof(Azure.Asm.StorageAccount)) ||
                    (tagType == typeof(Azure.Asm.VirtualMachine)) ||
                    (tagType == typeof(Azure.Asm.NetworkSecurityGroup)))
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

        private async Task RemoveASMNodeFromARMTree(TreeNode asmTreeNode)
        {
            TreeNode targetResourceGroupNode = SeekARMChildTreeNode(treeARM.Nodes, _TargetResourceGroup.Name, _TargetResourceGroup.Name, _TargetResourceGroup);
            if (targetResourceGroupNode != null)
            {
                TreeNode[] matchingNodes = targetResourceGroupNode.Nodes.Find(asmTreeNode.Name, true);
                foreach (TreeNode matchingNode in matchingNodes)
                {
                    if (matchingNode.Tag.GetType() == asmTreeNode.Tag.GetType())
                        await RemoveTreeNodeCascadeUp(matchingNode);
                    else if (matchingNode.Tag.GetType() == typeof(TreeNode))
                    {
                        TreeNode sourceNode = (TreeNode)matchingNode.Tag;
                        if (sourceNode.Tag.GetType() == asmTreeNode.Tag.GetType())
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
            return SeekARMChildTreeNode(this.treeARM.Nodes, name, text, tag, allowCreated);
        }

        internal TreeNode SeekARMChildTreeNode(TreeNodeCollection nodeCollection, string name, string text, object tag, bool allowCreated = false)
        {
            TreeNode[] childNodeMatch = nodeCollection.Find(name, false);
            TreeNode childNode = null;
            if (childNodeMatch.Count() == 0)
            {
                if (allowCreated)
                {
                    childNode = new TreeNode(text);
                    childNode.Name = name;
                    childNode.Tag = tag;
                    nodeCollection.Add(childNode);
                    childNode.ExpandAll();
                    return childNode;
                }
                return null;
            }
            else
                return childNodeMatch[0];
        }

        private async Task<TreeNode> AddASMNodeToARMTree(TreeNode asmTreeNode)
        {
            TreeNode targetResourceGroupNode = SeekARMChildTreeNode(treeARM.Nodes, _TargetResourceGroup.Name, _TargetResourceGroup.Name, _TargetResourceGroup, true);

            Type tagType = asmTreeNode.Tag.GetType();
            if (tagType == typeof(Azure.Asm.VirtualNetwork))
            {
                Azure.Asm.VirtualNetwork asmVirtualNetwork = (Azure.Asm.VirtualNetwork)asmTreeNode.Tag;
                TreeNode virtualNetworksNode = SeekARMChildTreeNode(targetResourceGroupNode.Nodes, "Virtual Networks", "Virtual Networks", "Virtual Networks", true);
                TreeNode virtualNetworkNode = SeekARMChildTreeNode(virtualNetworksNode.Nodes, asmTreeNode.Name, asmVirtualNetwork.GetFinalTargetName(), asmTreeNode, true);

                foreach (Azure.Asm.Subnet asmSubnet in asmVirtualNetwork.Subnets)
                {
                    // Property dialog not made available for Gateway Subnet
                    if (!asmSubnet.IsGatewaySubnet)
                    {
                        TreeNode subnetNode = SeekARMChildTreeNode(virtualNetworkNode.Nodes, asmSubnet.Name, asmSubnet.Name, asmSubnet, true);
                    }
                }

                virtualNetworkNode.ExpandAll();
                return virtualNetworkNode;
            }
            else if (tagType == typeof(Azure.Asm.StorageAccount))
            {
                Azure.Asm.StorageAccount asmStorageAccount = (Azure.Asm.StorageAccount)asmTreeNode.Tag;

                TreeNode storageAccountsNode = SeekARMChildTreeNode(targetResourceGroupNode.Nodes, "Storage Accounts", "Storage Accounts", "Storage Accounts", true);
                TreeNode storageAccountNode = SeekARMChildTreeNode(storageAccountsNode.Nodes, asmTreeNode.Name, asmStorageAccount.GetFinalTargetName(), asmTreeNode, true);
                return storageAccountNode;
            }
            else if (tagType == typeof(Azure.Asm.VirtualMachine))
            {
                Azure.Asm.VirtualMachine asmVirtualMachine = (Azure.Asm.VirtualMachine)asmTreeNode.Tag;
                TreeNode availabilitySets = SeekARMChildTreeNode(targetResourceGroupNode.Nodes, "Availability Sets", "Availability Sets", "Availability Sets", true);
                TreeNode availabilitySet = SeekARMChildTreeNode(availabilitySets.Nodes, asmVirtualMachine.TargetAvailabilitySet.TargetName, asmVirtualMachine.TargetAvailabilitySet.GetFinalTargetName(), asmVirtualMachine.TargetAvailabilitySet, true);
                TreeNode virtualMachineNode = SeekARMChildTreeNode(availabilitySet.Nodes, asmVirtualMachine.RoleName, asmVirtualMachine.RoleName, asmTreeNode, true);

                foreach (Azure.Asm.Disk asmDataDisk in asmVirtualMachine.DataDisks)
                {
                    TreeNode dataDiskNode = SeekARMChildTreeNode(virtualMachineNode.Nodes, asmDataDisk.DiskName, asmDataDisk.DiskName, asmDataDisk, true);
                }

                foreach (Azure.Asm.NetworkInterface asmNetworkInterface in asmVirtualMachine.NetworkInterfaces)
                {
                    TreeNode dataDiskNode = SeekARMChildTreeNode(virtualMachineNode.Nodes, asmNetworkInterface.Name, asmNetworkInterface.Name, asmNetworkInterface, true);
                }

                return virtualMachineNode;
            }
            else if (tagType == typeof(Azure.Asm.NetworkSecurityGroup))
            {
                Azure.Asm.NetworkSecurityGroup asmNetworkSecurityGroup = (Azure.Asm.NetworkSecurityGroup)asmTreeNode.Tag;
                TreeNode networkSecurityGroups = SeekARMChildTreeNode(targetResourceGroupNode.Nodes, "Network Security Groups", "Network Security Groups", "Network Security Groups", true);
                TreeNode networkSecurityGroupNode = SeekARMChildTreeNode(networkSecurityGroups.Nodes, asmNetworkSecurityGroup.Name, asmNetworkSecurityGroup.Name, asmTreeNode, true);
                return networkSecurityGroupNode;
            }
            else
                throw new Exception("Unhandled Node Type: " + tagType);
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
            treeASM.Height = this.Height - 195;
            treeARM.Height = treeASM.Height;
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
                await _saveSelectionProvider.Read(azureSubscription.SubscriptionId, _AzureContextSourceASM.AzureRetriever, AzureContextTargetARM.AzureRetriever, treeASM);
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
                    if (treeNode.Tag.GetType() == typeof(TreeNode))
                    {
                        TreeNode asmTreeNode = (TreeNode)treeNode.Tag;

                        if (asmTreeNode.Tag.GetType() == sourceObject.GetType())
                            treeARM.SelectedNode = treeNode;
                    }
                    else
                    {
                        if (treeNode.Tag.GetType() == sourceObject.GetType())
                            treeARM.SelectedNode = treeNode;
                    }
                }
                SeekAlertSourceRecursive(sourceObject, treeNode.Nodes);
            }
        }

        public override void SeekAlertSource(object sourceObject)
        {
            SeekAlertSourceRecursive(sourceObject, treeARM.Nodes);
        }

        public override void PostTelemetryRecord()
        {
            _telemetryProvider.PostTelemetryRecord((AsmToArmGenerator) this.TemplateGenerator);
        }
    }
}
