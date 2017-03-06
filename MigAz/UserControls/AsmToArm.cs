using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MigAz.Providers;
using MigAz.Azure;
using MigAz.Azure.Arm;
using MigAz.Core.Interface;
using MigAz.Forms.ARM.Interface;
using MigAz.Azure.Asm;
using System.Net;
using System.IO;
using System.Reflection;
using MigAz.Forms.ASM;

namespace MigAz.UserControls
{
    public partial class AsmToArm : UserControl
    {
        #region Variables

        private UISaveSelectionProvider _saveSelectionProvider;
        private ILogProvider _logProvider;
        private IStatusProvider _statusProvider;
        private TreeNode _sourceCascadeNode;
        private List<TreeNode> _SelectedNodes = new List<TreeNode>();
        private CloudTelemetryProvider _telemetryProvider;
        private AppSettingsProvider _appSettingsProvider;
        private AzureContext _AzureContextSourceASM;
        private AzureContext _AzureContextTargetARM;
        private ArmResourceGroup _TargetResourceGroup;

        #endregion

        #region Constructors

        public AsmToArm()
        {
            InitializeComponent();
        }

        public void Bind(IStatusProvider statusProvider, ILogProvider logProvider)
        {
            _statusProvider = statusProvider;
            _logProvider = logProvider;
            _saveSelectionProvider = new UISaveSelectionProvider();
            _telemetryProvider = new CloudTelemetryProvider();
            _appSettingsProvider = new AppSettingsProvider();

            _AzureContextSourceASM = new AzureContext(_logProvider, _statusProvider, _appSettingsProvider);
            azureLoginContextViewer21.Bind(_AzureContextSourceASM);
            _AzureContextSourceASM.AzureEnvironmentChanged += _AzureContextSourceASM_AzureEnvironmentChanged;
            _AzureContextSourceASM.UserAuthenticated += _AzureContextSourceASM_UserAuthenticated;
            _AzureContextSourceASM.BeforeAzureSubscriptionChange += _AzureContextSourceASM_BeforeAzureSubscriptionChange;
            _AzureContextSourceASM.AfterAzureSubscriptionChange += _AzureContextSourceASM_AfterAzureSubscriptionChange;
            _AzureContextSourceASM.BeforeUserSignOut += _AzureContextSourceASM_BeforeUserSignOut;
            _AzureContextSourceASM.AfterUserSignOut += _AzureContextSourceASM_AfterUserSignOut;

            _AzureContextTargetARM = new AzureContext(_logProvider, _statusProvider, _appSettingsProvider);
            azureLoginContextViewer2.Bind(_AzureContextTargetARM);
            _TargetResourceGroup = new ArmResourceGroup(this.AzureContextSourceASM, "Target Resource Group");
        }

        #endregion

        private async Task _AzureContextSourceASM_BeforeAzureSubscriptionChange(AzureContext sender)
        {
            await SaveSubscriptionSettings(sender.AzureSubscription);
            // todo if (ASMTokenProvider ==  ARMTokenProvider)

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
            if (_AzureContextTargetARM.TokenProvider == null)
            {
                _AzureContextTargetARM.TokenProvider = _AzureContextSourceASM.TokenProvider;
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
            azureLoginContextViewer2.Enabled = false;
            azureLoginContextViewer2.Refresh();
        }

        private async Task _AzureContextSourceASM_AfterAzureSubscriptionChange(AzureContext sender)
        {
            ResetForm();

            if (sender.AzureSubscription != null)
            {
                if (_AzureContextTargetARM.AzureSubscription == null)
                {
                    await _AzureContextTargetARM.SetSubscriptionContext(_AzureContextSourceASM.AzureSubscription);
                }

                azureLoginContextViewer2.Enabled = true;

                TreeNode subscriptionNode = new TreeNode(sender.AzureSubscription.Name);
                treeASM.Nodes.Add(subscriptionNode);
                subscriptionNode.Expand();

                List<AsmVirtualNetwork> asmVirtualNetworks = await _AzureContextSourceASM.AzureRetriever.GetAzureAsmVirtualNetworks();
                foreach (AsmVirtualNetwork asmVirtualNetwork in asmVirtualNetworks)
                {
                    if (asmVirtualNetwork.HasNonGatewaySubnet)
                    {
                        TreeNode parentNode = MigAz.Core.TreeView.GetDataCenterTreeViewNode(subscriptionNode, asmVirtualNetwork.Location, "Virtual Networks");
                        TreeNode tnVirtualNetwork = new TreeNode(asmVirtualNetwork.Name);
                        tnVirtualNetwork.Name = asmVirtualNetwork.Name;
                        tnVirtualNetwork.Tag = asmVirtualNetwork;
                        parentNode.Nodes.Add(tnVirtualNetwork);
                        parentNode.Expand();
                    }
                }

                foreach (AsmStorageAccount asmStorageAccount in await _AzureContextSourceASM.AzureRetriever.GetAzureAsmStorageAccounts())
                {
                    TreeNode parentNode = MigAz.Core.TreeView.GetDataCenterTreeViewNode(subscriptionNode, asmStorageAccount.GeoPrimaryRegion, "Storage Accounts");
                    TreeNode tnStorageAccount = new TreeNode(asmStorageAccount.Name);
                    tnStorageAccount.Name = tnStorageAccount.Text;
                    tnStorageAccount.Tag = asmStorageAccount;
                    parentNode.Nodes.Add(tnStorageAccount);
                    parentNode.Expand();
                }

                List<AsmCloudService> asmCloudServices = await _AzureContextSourceASM.AzureRetriever.GetAzureAsmCloudServices();
                foreach (AsmCloudService asmCloudService in asmCloudServices)
                {
                    foreach (AsmVirtualMachine asmVirtualMachine in asmCloudService.VirtualMachines)
                    {
                        TreeNode parentNode = MigAz.Core.TreeView.GetDataCenterTreeViewNode(subscriptionNode, asmCloudService.Location, "Cloud Services");
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

                foreach (AsmNetworkSecurityGroup asmNetworkSecurityGroup in await _AzureContextSourceASM.AzureRetriever.GetAzureAsmNetworkSecurityGroups())
                {
                    TreeNode parentNode = MigAz.Core.TreeView.GetDataCenterTreeViewNode(subscriptionNode, asmNetworkSecurityGroup.Location, "Network Security Groups");
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

            _statusProvider.UpdateStatus("Ready");
        }

        private void ResetForm()
        {
            treeASM.Nodes.Clear();
            treeARM.Nodes.Clear();
            _SelectedNodes.Clear();
            UpdateExportItemsCount();
            ClearAzureResourceManagerProperties();
            treeARM.Enabled = false;
            treeASM.Enabled = false;
        }

        #region Properties

        public ArmResourceGroup TargetResourceGroup
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

        public ILogProvider LogProvider
        {
            get { return _logProvider; }
        }

        public IStatusProvider StatusProvider
        {
            get { return _statusProvider; }
        }

        public Azure.Interface.ITelemetryProvider TelemetryProvider
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

        private async Task NewVersionAvailable()
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("https://asmtoarmtoolapi.azurewebsites.net/api/version");
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";

            try
            {
                HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
                string result = new StreamReader(response.GetResponseStream()).ReadToEnd();

                string version = "\"" + Assembly.GetEntryAssembly().GetName().Version.ToString() + "\"";
                string availableversion = result.ToString();

                if (version != availableversion)
                {
                    DialogResult dialogresult = MessageBox.Show("New version " + availableversion + " is available at http://aka.ms/MigAz", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception exception)
            {
                DialogResult dialogresult = MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region ASM TreeView Methods


        private async Task AutoSelectDependencies(TreeNode selectedNode)
        {
            if ((app.Default.AutoSelectDependencies) && (selectedNode.Checked) && (selectedNode.Tag != null))
            {
                if (selectedNode.Tag.GetType() == typeof(AsmVirtualMachine))
                {
                    AsmVirtualMachine asmVirtualMachine = (AsmVirtualMachine)selectedNode.Tag;

                    #region process virtual network
                    if (asmVirtualMachine.VirtualNetworkName != String.Empty)
                    {
                        foreach (TreeNode treeNode in treeASM.Nodes.Find(asmVirtualMachine.VirtualNetworkName, true))
                        {
                            if ((treeNode.Tag != null) && (treeNode.Tag.GetType() == typeof(AsmVirtualNetwork)))
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
                        if ((treeNode.Tag != null) && (treeNode.Tag.GetType() == typeof(AsmStorageAccount)))
                        {
                            if (!treeNode.Checked)
                                treeNode.Checked = true;
                        }
                    }

                    #endregion

                    #region Data Disk(s) Storage Account(s)

                    foreach (AsmDisk dataDisk in asmVirtualMachine.DataDisks)
                    {
                        foreach (TreeNode treeNode in treeASM.Nodes.Find(dataDisk.StorageAccountName, true))
                        {
                            if ((treeNode.Tag != null) && (treeNode.Tag.GetType() == typeof(AsmStorageAccount)))
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
                            if ((treeNode.Tag != null) && (treeNode.Tag.GetType() == typeof(AsmNetworkSecurityGroup)))
                            {
                                if (!treeNode.Checked)
                                    treeNode.Checked = true;
                            }
                        }
                    }

                    #endregion

                }

                else if (selectedNode.Tag.GetType() == typeof(AsmVirtualNetwork))
                {
                    AsmVirtualNetwork asmVirtualNetwork = (AsmVirtualNetwork)selectedNode.Tag;

                    foreach (AsmSubnet asmSubnet in asmVirtualNetwork.Subnets)
                    {
                        if (asmSubnet.NetworkSecurityGroup != null)
                        {
                            foreach (TreeNode treeNode in treeASM.Nodes.Find(asmSubnet.NetworkSecurityGroup.Name, true))
                            {
                                if ((treeNode.Tag != null) && (treeNode.Tag.GetType() == typeof(AsmNetworkSecurityGroup)))
                                {
                                    if (!treeNode.Checked)
                                        treeNode.Checked = true;
                                }
                            }
                        }
                    }
                }

                _statusProvider.UpdateStatus("Ready");
            }
        }

        private List<TreeNode> UpdateSelectedNodes()
        {
            List<TreeNode> selectedNodes = new List<TreeNode>();
            foreach (TreeNode treeNode in treeASM.Nodes)
            {
                RecursiveNodeSelectedAdd(ref selectedNodes, treeNode);
            }
            return selectedNodes;
        }

        private void RecursiveNodeSelectedAdd(ref List<TreeNode> selectedNodes, TreeNode parentNode)
        {
            if (parentNode.Checked && parentNode.Tag != null && (parentNode.Tag.GetType() == typeof(AsmNetworkSecurityGroup) || parentNode.Tag.GetType() == typeof(AsmVirtualNetwork) || parentNode.Tag.GetType() == typeof(AsmStorageAccount) || parentNode.Tag.GetType() == typeof(AsmVirtualMachine)))
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
            ClearAzureResourceManagerProperties();
        }

        private async void treeASM_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (_sourceCascadeNode == null)
            {
                _sourceCascadeNode = e.Node;

                if (e.Node.Checked)
                {
                    await MigAz.Core.TreeView.RecursiveCheckToggleDown(e.Node, e.Node.Checked);
                    MigAz.Core.TreeView.FillUpIfFullDown(e.Node);
                    treeASM.SelectedNode = e.Node;

                    await AutoSelectDependencies(e.Node);
                }
                else
                {
                    await MigAz.Core.TreeView.RecursiveCheckToggleUp(e.Node, e.Node.Checked);
                    await MigAz.Core.TreeView.RecursiveCheckToggleDown(e.Node, e.Node.Checked);
                }

                _sourceCascadeNode = null;

                _SelectedNodes = this.UpdateSelectedNodes();
                UpdateExportItemsCount();
            }

            await UpdateARMTree(e.Node);
        }

        #endregion

        #region ARM TreeView Methods

        private async void treeARM_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ClearAzureResourceManagerProperties();

            lblAzureObjectName.Text = String.Empty;

            if (e.Node.Tag != null)
            {
                if (e.Node.Tag.GetType() == typeof(TreeNode))
                {
                    TreeNode asmTreeNode = (TreeNode)e.Node.Tag;

                    if (asmTreeNode.Tag != null)
                    {
                        lblAzureObjectName.Text = e.Node.Text;

                        if (asmTreeNode.Tag.GetType() == typeof(AsmVirtualNetwork))
                        {
                            pictureBox1.Image = imageList1.Images["VirtualNetwork"];

                            VirtualNetworkProperties properties = new VirtualNetworkProperties();
                            properties.Bind(e.Node);
                            panel1.Controls.Add(properties);
                        }
                        else if (asmTreeNode.Tag.GetType() == typeof(AsmStorageAccount))
                        {
                            pictureBox1.Image = imageList1.Images["StorageAccount"];

                            AsmStorageAccount storageAccount = (AsmStorageAccount)asmTreeNode.Tag;
                            lblAzureObjectName.Text = storageAccount.Name;

                            StorageAccountProperties properties = new StorageAccountProperties();
                            properties.Bind(this._AzureContextTargetARM, e.Node);
                            panel1.Controls.Add(properties);
                        }
                        else if (asmTreeNode.Tag.GetType() == typeof(AsmVirtualMachine))
                        {
                            pictureBox1.Image = imageList1.Images["VirtualMachine"];

                            VirtualMachineProperties properties = new VirtualMachineProperties();
                            await properties.Bind(e.Node, this);
                            panel1.Controls.Add(properties);
                        }
                        else if (asmTreeNode.Tag.GetType() == typeof(AsmNetworkSecurityGroup))
                        {
                            pictureBox1.Image = imageList1.Images["NetworkSecurityGroup"];

                            NetworkSecurityGroupProperties properties = new NetworkSecurityGroupProperties();
                            properties.Bind(e.Node, this);
                            panel1.Controls.Add(properties);
                        }
                    }
                }
                if (e.Node.Tag.GetType() == typeof(AsmSubnet))
                {
                    pictureBox1.Image = imageList1.Images["VirtualNetwork"];

                    AsmSubnet asmSubnet = (AsmSubnet)e.Node.Tag;

                    SubnetProperties properties = new SubnetProperties();
                    properties.Bind(e.Node);
                    panel1.Controls.Add(properties);
                }
                else if (e.Node.Tag.GetType() == typeof(AsmDisk))
                {
                    AsmDisk asmDisk = (AsmDisk)e.Node.Tag;

                    pictureBox1.Image = imageList1.Images["Disk"];

                    DiskProperties properties = new DiskProperties();
                    properties.Bind(this, e.Node);
                    panel1.Controls.Add(properties);
                }
                else if (e.Node.Tag.GetType() == typeof(ArmAvailabilitySet))
                {
                    pictureBox1.Image = imageList1.Images["AvailabilitySet"];

                    AvailabilitySetProperties properties = new AvailabilitySetProperties();
                    properties.Bind(e.Node);
                    panel1.Controls.Add(properties);
                }
                else if (e.Node.Tag.GetType() == typeof(ArmResourceGroup))
                {
                    pictureBox1.Image = imageList1.Images["ResourceGroup"];

                    ResourceGroupProperties properties = new ResourceGroupProperties();
                    await properties.Bind(this, e.Node);
                    panel1.Controls.Add(properties);
                }
            }

            _statusProvider.UpdateStatus("Ready");
        }

        private async Task UpdateARMTree(TreeNode asmTreeNode)
        {
            if (asmTreeNode.Tag != null)
            {
                Type tagType = asmTreeNode.Tag.GetType();
                if ((tagType == typeof(AsmVirtualNetwork)) ||
                    (tagType == typeof(AsmStorageAccount)) ||
                    (tagType == typeof(AsmVirtualMachine)) ||
                    (tagType == typeof(AsmNetworkSecurityGroup)))
                {
                    if (asmTreeNode.Checked)
                        await AddASMNodeToARMTree(asmTreeNode);
                    else
                        await RemoveASMNodeFromARMTree(asmTreeNode);
                }
            }
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

        private async Task AddASMNodeToARMTree(TreeNode asmTreeNode)
        {
            TreeNode targetResourceGroupNode = SeekARMChildTreeNode(treeARM.Nodes, _TargetResourceGroup.Name, _TargetResourceGroup.Name, _TargetResourceGroup, true);

            Type tagType = asmTreeNode.Tag.GetType();
            if (tagType == typeof(AsmVirtualNetwork))
            {
                AsmVirtualNetwork asmVirtualNetwork = (AsmVirtualNetwork)asmTreeNode.Tag;
                TreeNode virtualNetworksNode = SeekARMChildTreeNode(targetResourceGroupNode.Nodes, "Virtual Networks", "Virtual Networks", "Virtual Networks", true);
                TreeNode virtualNetworkNode = SeekARMChildTreeNode(virtualNetworksNode.Nodes, asmTreeNode.Name, asmVirtualNetwork.GetFinalTargetName(), asmTreeNode, true);

                foreach (AsmSubnet asmSubnet in asmVirtualNetwork.Subnets)
                {
                    // Property dialog not made available for Gateway Subnet
                    if (!asmSubnet.IsGatewaySubnet)
                    {
                        TreeNode subnetNode = SeekARMChildTreeNode(virtualNetworkNode.Nodes, asmSubnet.Name, asmSubnet.Name, asmSubnet, true);
                    }
                }

                virtualNetworkNode.ExpandAll();
                treeARM.SelectedNode = virtualNetworkNode;
                treeARM.Focus();
            }
            else if (tagType == typeof(AsmStorageAccount))
            {
                AsmStorageAccount asmStorageAccount = (AsmStorageAccount)asmTreeNode.Tag;

                TreeNode storageAccountsNode = SeekARMChildTreeNode(targetResourceGroupNode.Nodes, "Storage Accounts", "Storage Accounts", "Storage Accounts", true);
                TreeNode storageAccountNode = SeekARMChildTreeNode(storageAccountsNode.Nodes, asmTreeNode.Name, asmStorageAccount.GetFinalTargetName(), asmTreeNode, true);
                treeARM.SelectedNode = storageAccountNode;
                treeARM.Focus();
            }
            else if (tagType == typeof(AsmVirtualMachine))
            {
                AsmVirtualMachine asmVirtualMachine = (AsmVirtualMachine)asmTreeNode.Tag;
                TreeNode availabilitySets = SeekARMChildTreeNode(targetResourceGroupNode.Nodes, "Availability Sets", "Availability Sets", "Availability Sets", true);
                TreeNode availabilitySet = SeekARMChildTreeNode(availabilitySets.Nodes, asmVirtualMachine.TargetAvailabilitySet.TargetName, asmVirtualMachine.TargetAvailabilitySet.GetFinalTargetName(), asmVirtualMachine.TargetAvailabilitySet, true);
                TreeNode virtualMachineNode = SeekARMChildTreeNode(availabilitySet.Nodes, asmVirtualMachine.RoleName, asmVirtualMachine.RoleName, asmTreeNode, true);

                foreach (AsmDisk asmDataDisk in asmVirtualMachine.DataDisks)
                {
                    TreeNode dataDiskNode = SeekARMChildTreeNode(virtualMachineNode.Nodes, asmDataDisk.DiskName, asmDataDisk.DiskName, asmDataDisk, true);
                }

                foreach (AsmNetworkInterface asmNetworkInterface in asmVirtualMachine.NetworkInterfaces)
                {
                    TreeNode dataDiskNode = SeekARMChildTreeNode(virtualMachineNode.Nodes, asmNetworkInterface.Name, asmNetworkInterface.Name, asmNetworkInterface, true);
                }

                treeARM.SelectedNode = virtualMachineNode;
                treeARM.Focus();
            }
            else if (tagType == typeof(AsmNetworkSecurityGroup))
            {
                AsmNetworkSecurityGroup asmNetworkSecurityGroup = (AsmNetworkSecurityGroup)asmTreeNode.Tag;
                TreeNode networkSecurityGroups = SeekARMChildTreeNode(targetResourceGroupNode.Nodes, "Network Security Groups", "Network Security Groups", "Network Security Groups", true);
                TreeNode networkSecurityGroupNode = SeekARMChildTreeNode(networkSecurityGroups.Nodes, asmNetworkSecurityGroup.Name, asmNetworkSecurityGroup.Name, asmTreeNode, true);

                treeARM.SelectedNode = networkSecurityGroupNode;
                treeARM.Focus();
            }
            else
                throw new Exception("Unhandled Node Type: " + tagType);
        }

        #endregion

        #region Form Controls

        #region ToolStrip Events

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://aka.ms/MigAz");
        }

        #endregion

        #region Options Button

        private void btnOptions_Click(object sender, EventArgs e)
        {
            Forms.formOptions formoptions = new Forms.formOptions();
            formoptions.ShowDialog(this);
        }

        #endregion

        #region Export Button

        private bool RecursiveHealthCheckNode(TreeNode treeNode)
        {
            if (treeNode.Tag != null)
            {
                if (treeNode.Tag.GetType() == typeof(ArmResourceGroup))
                {
                    ArmResourceGroup armResourceGroup = (ArmResourceGroup)treeNode.Tag;

                    if (armResourceGroup.Location == null)
                    {
                        treeARM.SelectedNode = treeNode;
                        this.Refresh();
                        MessageBox.Show("Target Location must be selected before exporting.");
                        return false;
                    }
                }
                else if (treeNode.Tag.GetType() == typeof(TreeNode)) // Tag is the sourced ASM TreeNode
                {
                    TreeNode asmTreeNode = (TreeNode)treeNode.Tag;

                    if (asmTreeNode.Tag != null)
                    {
                        if (asmTreeNode.Tag.GetType() == typeof(AsmNetworkSecurityGroup))
                        {
                            AsmNetworkSecurityGroup asmNetworkSecurityGroup = (AsmNetworkSecurityGroup)asmTreeNode.Tag;

                            // Validate the Target Name is not blank
                            if (asmNetworkSecurityGroup.TargetName == String.Empty)
                            {
                                treeARM.SelectedNode = treeNode;
                                this.Refresh();
                                MessageBox.Show("Target Name must be selected before exporting.");
                                return false;
                            }
                        }
                        else if (asmTreeNode.Tag.GetType() == typeof(AsmVirtualNetwork))
                        {
                            AsmVirtualNetwork asmVirtualNetwork = (AsmVirtualNetwork)asmTreeNode.Tag;

                            // Validate the Target Name is not blank
                            if (asmVirtualNetwork.TargetName == String.Empty)
                            {
                                treeARM.SelectedNode = treeNode;
                                this.Refresh();
                                MessageBox.Show("Target Name must be selected before exporting.");
                                return false;
                            }

                            foreach (TreeNode virtualNetworkNode in treeNode.Parent.Nodes)
                            {
                                TreeNode asmVirtualNetworkNode = (TreeNode)virtualNetworkNode.Tag;
                                AsmVirtualNetwork asmVirtualNetworkCompare = (AsmVirtualNetwork)asmVirtualNetworkNode.Tag;

                                if (asmVirtualNetworkCompare.Name != asmVirtualNetwork.Name && asmVirtualNetworkCompare.TargetName == asmVirtualNetwork.TargetName)
                                {
                                    treeARM.SelectedNode = treeNode;
                                    this.Refresh();
                                    MessageBox.Show("Target Names must be unique to migrate Virtual Networks.");
                                    return false;
                                }

                            }
                        }
                        else if (asmTreeNode.Tag.GetType() == typeof(AsmVirtualMachine))
                        {
                            AsmVirtualMachine asmVirtualMachine = (AsmVirtualMachine)asmTreeNode.Tag;

                            if (asmVirtualMachine.TargetName == String.Empty)
                            {
                                treeARM.SelectedNode = treeNode;
                                this.Refresh();
                                MessageBox.Show("Target Name for Virtual Machine must be specified.");
                                return false;
                            }

                            if (asmVirtualMachine.TargetAvailabilitySet == null)
                            {
                                treeARM.SelectedNode = treeNode;
                                this.Refresh();
                                MessageBox.Show("Target Availability Set for Virtual Machine must be specified.");
                                return false;
                            }

                            if (asmVirtualMachine.TargetVirtualNetwork == null)
                            {
                                treeARM.SelectedNode = treeNode;
                                this.Refresh();
                                MessageBox.Show("Target Virtual Network for Virtual Machine must be specified.");
                                return false;
                            }

                            if (asmVirtualMachine.TargetSubnet == null)
                            {
                                treeARM.SelectedNode = treeNode;
                                this.Refresh();
                                MessageBox.Show("Target Subnet for Virtual Machine must be specified.");
                                return false;
                            }

                            if (asmVirtualMachine.OSVirtualHardDisk.TargetStorageAccount == null)
                            {
                                treeARM.SelectedNode = treeNode;
                                this.Refresh();
                                MessageBox.Show("Target VM OS Disk Storage Account must be specified.");
                                return false;
                            }
                        }
                        else if (asmTreeNode.Tag.GetType() == typeof(AsmStorageAccount))
                        {
                            AsmStorageAccount asmStorageAccount = (AsmStorageAccount)asmTreeNode.Tag;
                            if (asmStorageAccount.TargetName == String.Empty)
                            {
                                treeARM.SelectedNode = treeNode;
                                this.Refresh();
                                MessageBox.Show("Target Storage Account Name must be specified.");
                                return false;
                            }
                        }
                    }
                }
                else if (treeNode.Tag.GetType() == typeof(AsmSubnet))
                {
                    AsmSubnet asmSubnet = (AsmSubnet)treeNode.Tag;
                    if (asmSubnet.TargetName == String.Empty)
                    {
                        treeARM.SelectedNode = treeNode;
                        this.Refresh();
                        MessageBox.Show("Target Subnet Name must be specified.");
                        return false;
                    }
                }
                else if (treeNode.Tag.GetType() == typeof(ArmAvailabilitySet))
                {
                    ArmAvailabilitySet armAvailabilitySet = (ArmAvailabilitySet)treeNode.Tag;
                    if (armAvailabilitySet.TargetName == String.Empty)
                    {
                        treeARM.SelectedNode = treeNode;
                        this.Refresh();
                        MessageBox.Show("Target Availability Set Name must be specified.");
                        return false;
                    }
                }
                else if (treeNode.Tag.GetType() == typeof(AsmDisk))
                {
                    AsmDisk asmDisk = (AsmDisk)treeNode.Tag;

                    if (asmDisk.TargetStorageAccount == null)
                    {
                        treeARM.SelectedNode = treeNode;
                        this.Refresh();
                        MessageBox.Show("Target VM Data Disk Storage Account must be specified.");
                        return false;
                    }
                }
            }

            foreach (TreeNode childNode in treeNode.Nodes)
            {
                bool nodeResult = RecursiveHealthCheckNode(childNode);
                if (nodeResult == false)
                    return false;
            }

            return true;
        }


        private async Task<bool> ExecutePreExportHealthCheck(bool focusFirstError)
        {
            foreach (TreeNode treeNode in treeARM.Nodes)
            {
                bool nodeResult = RecursiveHealthCheckNode(treeNode);
                if (nodeResult == false)
                    return false;
            }

            return true;
        }

        private async void btnExport_Click(object sender, EventArgs e)
        {
            btnExport.Enabled = false;

            if (!await ExecutePreExportHealthCheck(true))
            {
                btnExport.Enabled = true;
                return;
            }

            await SaveSubscriptionSettings(_AzureContextSourceASM.AzureSubscription);

            PreExportDialog preExportDialog = new PreExportDialog(this.LogProvider, this.StatusProvider, this.TelemetryProvider, this.AppSettingsProviders, this.AzureContextSourceASM.AzureSubscription, this.AzureContextSourceASM.AzureSubscription, this.TargetResourceGroup, this.SelectedNodes);
            preExportDialog.ShowDialog(this);

            btnExport.Enabled = true;
        }

        #endregion

        #endregion

        #region Form Events

        private async void AsmToArmForm_Load(object sender, EventArgs e)
        {
            _logProvider.WriteLog("AsmToArmForm_Load", "Program start");

            this.Text = "MigAz (" + Assembly.GetEntryAssembly().GetName().Version.ToString() + ")";
            lblAzureObjectName.Text = String.Empty;
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

            await NewVersionAvailable(); // check if there a new version of the app
        }

        private async void AsmToArmForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            await SaveSubscriptionSettings(_AzureContextSourceASM.AzureSubscription);
        }

        private void AsmToArmForm_Resize(object sender, EventArgs e)
        {
            treeASM.Height = this.Height - 195;
            treeARM.Height = treeASM.Height;
            groupBox1.Height = treeARM.Height - btnExport.Height - btnOptions.Height - 10;
            btnExport.Top = groupBox1.Top + groupBox1.Height + 10;
            btnOptions.Top = btnExport.Top + btnExport.Height + 10;
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
                _statusProvider.UpdateStatus("BUSY: Reading saved selection");
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

            btnExport.Text = "Export " + selectedExportCount.ToString() + " objects";
            btnExport.Enabled = _SelectedNodes.Count() > 0;
        }

        private void ClearAzureResourceManagerProperties()
        {
            panel1.Controls.Clear();
            pictureBox1.Image = null;
            lblAzureObjectName.Text = String.Empty;
        }

        private void groupBox1_Resize(object sender, EventArgs e)
        {
            panel1.Height = groupBox1.Height - 95;
        }
    }
}
