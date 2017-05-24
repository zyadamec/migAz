using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using MigAz.Core.Interface;
using MigAz.AWS.Providers;
using MigAz.Core;
using MigAz.Azure;
using MigAz.Azure.UserControls;
using MigAz.Azure.Generator.AsmToArm;
using MigAz.AWS.Generator;

namespace MigAz.UserControls.Migrators
{
    public partial class AwsToArm : IMigratorUserControl
    {
        private AzureContext _AzureContextARM;
        private AwsToArmSaveSelectionProvider _saveSelectionProvider;
        private AwsToArmSaveSelectionProvider _telemetryProvider;
        //private AwsAppSettingsProvider _appSettingsProvider;
        private PropertyPanel _PropertyPanel;

        //private EC2Operation ec2 = null;
        // TODO WHERE?? static IAmazonEC2 service;
        private AzureContext _AzureContextTargetARM;
        private Azure.MigrationTarget.ResourceGroup _TargetResourceGroup;
        private TreeNode _SourceAwsNode = null;

        public AwsToArm() : base(null, null) { }

        public AwsToArm(IStatusProvider statusProvider, ILogProvider logProvider, PropertyPanel propertyPanel) 
            : base (statusProvider, logProvider)
        {
            InitializeComponent();

            _PropertyPanel = propertyPanel;
            _saveSelectionProvider = new AwsToArmSaveSelectionProvider();
            _telemetryProvider = new AwsToArmSaveSelectionProvider();
            //_appSettingsProvider = new AwsAppSettingsProvider();

            _AzureContextTargetARM = new AzureContext(LogProvider, StatusProvider, null); // _appSettingsProvider);
            _AzureContextTargetARM.AfterAzureSubscriptionChange += _AzureContextTargetARM_AfterAzureSubscriptionChange;

            _TargetResourceGroup = new Azure.MigrationTarget.ResourceGroup(this._AzureContextTargetARM);

            azureLoginContextViewer21.Bind(_AzureContextTargetARM);

            this.TemplateGenerator = new AzureGenerator(_AzureContextTargetARM.AzureSubscription, _AzureContextTargetARM.AzureSubscription, _TargetResourceGroup, LogProvider, StatusProvider, null, null); // _telemetryProvider, _appSettingsProvider);

            this._PropertyPanel.LogProvider = this.LogProvider;
            this._PropertyPanel.AzureContext = _AzureContextTargetARM;
        }

        private Task _AzureContextTargetARM_AfterAzureSubscriptionChange(AzureContext sender)
        {
            throw new NotImplementedException();
        }

        public async Task Bind()
        {
            _AzureContextARM = new AzureContext(LogProvider, StatusProvider, null); // todo needs settings provider
            await azureLoginContextViewer21.Bind(_AzureContextARM);

            //var tokenProvider = new InteractiveTokenProvider();
        }

        private void AwsToArm_Load(object sender, EventArgs e)
        {
            if (LogProvider != null)
                LogProvider.WriteLog("Window_Load", "Program start");
            // TODO instResponse = new DescribeInstancesResponse();
            // this.Text = "migAz AWS (" + Assembly.GetEntryAssembly().GetName().Version.ToString() + ")";





            AlertIfNewVersionAvailable();
        }

        #region New Version Check

        private async Task AlertIfNewVersionAvailable()
        {
            string currentVersion = "2.2.3.0";
            VersionCheck versionCheck = new VersionCheck(this.LogProvider);
            string newVersionNumber = await versionCheck.GetAvailableVersion("https://api.migaz.tools/v1/version/AWStoARM", currentVersion);
            if (versionCheck.IsVersionNewer(currentVersion, newVersionNumber))
            {
                DialogResult dialogresult = MessageBox.Show("New version " + newVersionNumber + " is available at http://aka.ms/MigAz", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        #endregion

        //TODO CHECK
        private void Window_FormClosing(object sender, FormClosingEventArgs e)
        {
            // If save selection option is enabled
            //if (app.Default.SaveSelection)
            //{
            //    _saveSelectionProvider.Save(cmbRegion.Text, lvwVirtualNetworks, lvwVirtualMachines);
            //}
        }

        private void btnGetToken_Click(object sender, EventArgs e)
        {
            LogProvider.WriteLog("GetToken_Click", "Start");

            try
            {
                //Load Items
                Load_Items();

                //lblSignInText.Text = $"Signed in as {accessKeyID}";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        private void Load_Items()
        {
            LogProvider.WriteLog("Load_Items", "Start");

            treeSourceAWS.Nodes.Clear();
            treeTargetARM.Nodes.Clear();

            TreeNode subscriptionNode = new TreeNode("AWS Subscription");
            treeSourceAWS.Nodes.Add(subscriptionNode);

            AWSRetriever awsRetriever = new AWSRetriever();
            awsRetriever.toname(accessKeyTextBox.Text.Trim(), secretKeyTextBox.Text.Trim(), subscriptionNode, this.LogProvider, this.StatusProvider);

            subscriptionNode.ExpandAll();

            StatusProvider.UpdateStatus("Ready");
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









































 



        private void AutoSelectDependencies(ItemCheckedEventArgs listViewRow)
        {
            // TODO
            //    string InstanceId = listViewRow.Item.ListView.Items[listViewRow.Item.Index].SubItems[0].Text;

            //    var availableInstances = _awsObjectRetriever.Instances;
            //    //var selectedVolumes;
            //    if (InstanceId != null)
            //    {

            //        //var selectedInstances = availableInstances.Reservations[0].Instances.Find(x => x.InstanceId == InstanceId);
            //        var selectedInstances = _awsObjectRetriever.getInstancebyId(InstanceId);

            //        //foreach (ListViewItem virtualNetwork in lvwVirtualNetworks.Items)
            //        //{
            //        //    if (selectedInstances.Instances[0].VpcId == virtualNetwork.SubItems[0].Text)
            //        //    {
            //        //        virtualNetwork.Checked = true;
            //        //        virtualNetwork.Selected = true;
            //        //    }

            //        //}

            //    }
        }

        private void cmbRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Load the Region Items
            Load_Items();

            // If save selection option is enabled
            //if (app.Default.SaveSelection)
            //{
            //    StatusProvider.UpdateStatus("BUSY: Reading saved selection");
            //    _saveSelectionProvider.Read(cmbRegion.Text, ref lvwVirtualNetworks, ref lvwVirtualMachines);
            //}

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            Load_Items();
        }

        private void AwsToArm_Resize(object sender, EventArgs e)
        {
            treeSourceAWS.Height = this.Height - 150;
            treeTargetARM.Height = this.Height - 150;
        }

        private async void treeSourceAWS_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (_SourceAwsNode == null)
            {
                _SourceAwsNode = e.Node;
            }

            //if (e.Node.Checked)
            //    await AutoSelectDependencies(e.Node);

            TreeNode resultUpdateARMTree = null;

            if (e.Node.Tag != null)
            {
                Type tagType = e.Node.Tag.GetType();
                if ((tagType == typeof(Azure.MigrationTarget.VirtualNetwork)) ||
                    (tagType == typeof(Azure.MigrationTarget.StorageAccount)) ||
                    (tagType == typeof(Azure.MigrationTarget.VirtualMachine)) ||
                    (tagType == typeof(Azure.MigrationTarget.LoadBalancer)) ||
                    (tagType == typeof(Azure.MigrationTarget.PublicIp)) ||
                    (tagType == typeof(Azure.MigrationTarget.NetworkSecurityGroup)))
                {
                    if (e.Node.Checked)
                    {
                        resultUpdateARMTree = await AddMigrationTargetToTargetTree((MigAz.Core.Interface.IMigrationTarget)e.Node.Tag);
                    }
                    else
                    {
                        await RemoveASMNodeFromARMTree((MigAz.Core.Interface.IMigrationTarget)e.Node.Tag);
                    }
                }
            }

            if (_SourceAwsNode != null && _SourceAwsNode == e.Node)
            {
                if (e.Node.Checked)
                {
                    await RecursiveCheckToggleDown(e.Node, e.Node.Checked);
                    FillUpIfFullDown(e.Node);
                    treeSourceAWS.SelectedNode = e.Node;
                }
                else
                {
                    await RecursiveCheckToggleUp(e.Node, e.Node.Checked);
                    await RecursiveCheckToggleDown(e.Node, e.Node.Checked);
                }

                //_SelectedNodes = this.GetSelectedNodes(treeSourceASM);
                //UpdateExportItemsCount();
                await this.TemplateGenerator.UpdateArtifacts(GetExportArtifacts());

                _SourceAwsNode = null;

                if (resultUpdateARMTree != null)
                    treeTargetARM.SelectedNode = resultUpdateARMTree;
            }
        }

        private async Task<TreeNode> AddMigrationTargetToTargetTree(IMigrationTarget parentNode)
        {
            if (parentNode == null)
                throw new ArgumentNullException("Migration Target cannot be null.");

            TreeNode targetResourceGroupNode = SeekResourceGroupTreeNode();

            if (parentNode.GetType() == typeof(Azure.MigrationTarget.VirtualNetwork))
            {
                Azure.MigrationTarget.VirtualNetwork targetVirtualNetwork = (Azure.MigrationTarget.VirtualNetwork)parentNode;
                TreeNode virtualNetworkNode = SeekARMChildTreeNode(targetResourceGroupNode.Nodes, targetVirtualNetwork.SourceName, targetVirtualNetwork.ToString(), targetVirtualNetwork, true);

                foreach (Azure.MigrationTarget.Subnet targetSubnet in targetVirtualNetwork.TargetSubnets)
                {
                    TreeNode subnetNode = SeekARMChildTreeNode(virtualNetworkNode.Nodes, targetVirtualNetwork.ToString(), targetSubnet.ToString(), targetSubnet, true);
                }

                targetResourceGroupNode.ExpandAll();
                return virtualNetworkNode;
            }
            else if (parentNode.GetType() == typeof(Azure.MigrationTarget.StorageAccount))
            {
                Azure.MigrationTarget.StorageAccount targetStorageAccount = (Azure.MigrationTarget.StorageAccount)parentNode;

                TreeNode storageAccountNode = SeekARMChildTreeNode(targetResourceGroupNode.Nodes, targetStorageAccount.SourceName, targetStorageAccount.ToString(), targetStorageAccount, true);

                targetResourceGroupNode.ExpandAll();
                return storageAccountNode;
            }
            else if (parentNode.GetType() == typeof(Azure.MigrationTarget.NetworkSecurityGroup))
            {
                Azure.MigrationTarget.NetworkSecurityGroup targetNetworkSecurityGroup = (Azure.MigrationTarget.NetworkSecurityGroup)parentNode;
                TreeNode networkSecurityGroupNode = SeekARMChildTreeNode(targetResourceGroupNode.Nodes, targetNetworkSecurityGroup.SourceName, targetNetworkSecurityGroup.ToString(), targetNetworkSecurityGroup, true);

                targetResourceGroupNode.ExpandAll();
                return networkSecurityGroupNode;
            }
            else if (parentNode.GetType() == typeof(Azure.MigrationTarget.LoadBalancer))
            {
                Azure.MigrationTarget.LoadBalancer targetLoadBalancer = (Azure.MigrationTarget.LoadBalancer)parentNode;
                TreeNode targetLoadBalancerNode = SeekARMChildTreeNode(targetResourceGroupNode.Nodes, targetLoadBalancer.SourceName, targetLoadBalancer.ToString(), targetLoadBalancer, true);

                targetResourceGroupNode.ExpandAll();
                return targetLoadBalancerNode;
            }
            else if (parentNode.GetType() == typeof(Azure.MigrationTarget.PublicIp))
            {
                Azure.MigrationTarget.PublicIp targetPublicIp = (Azure.MigrationTarget.PublicIp)parentNode;
                TreeNode targetPublicIpNode = SeekARMChildTreeNode(targetResourceGroupNode.Nodes, targetPublicIp.SourceName, targetPublicIp.ToString(), targetPublicIp, true);

                targetResourceGroupNode.ExpandAll();
                return targetPublicIpNode;
            }
            else if (parentNode.GetType() == typeof(Azure.MigrationTarget.VirtualMachine))
            {
                Azure.MigrationTarget.VirtualMachine targetVirtualMachine = (Azure.MigrationTarget.VirtualMachine)parentNode;

                TreeNode virtualMachineParentNode = targetResourceGroupNode;
                TreeNode targetAvailabilitySetNode = null;

                // https://docs.microsoft.com/en-us/azure/virtual-machines/windows/manage-availability
                if (targetVirtualMachine.TargetAvailabilitySet != null)
                {
                    targetAvailabilitySetNode = GetTargetAvailabilitySetNode(targetResourceGroupNode, targetVirtualMachine.TargetAvailabilitySet);
                    virtualMachineParentNode = targetAvailabilitySetNode;
                }

                TreeNode virtualMachineNode = SeekARMChildTreeNode(virtualMachineParentNode.Nodes, targetVirtualMachine.SourceName, targetVirtualMachine.ToString(), targetVirtualMachine, true);

                foreach (Azure.MigrationTarget.Disk targetDisk in targetVirtualMachine.DataDisks)
                {
                    TreeNode dataDiskNode = SeekARMChildTreeNode(virtualMachineNode.Nodes, targetDisk.ToString(), targetDisk.ToString(), targetDisk, true);
                }

                foreach (Azure.MigrationTarget.NetworkInterface targetNetworkInterface in targetVirtualMachine.NetworkInterfaces)
                {
                    TreeNode networkInterfaceNode = SeekARMChildTreeNode(virtualMachineNode.Nodes, targetNetworkInterface.ToString(), targetNetworkInterface.ToString(), targetNetworkInterface, true);
                }

                targetResourceGroupNode.ExpandAll();
                return virtualMachineNode;
            }
            else
                throw new Exception("Unhandled Node Type in AddMigrationTargetToTargetTree: " + parentNode.GetType());

        }

        private async Task RemoveASMNodeFromARMTree(IMigrationTarget migrationTarget)
        {

            TreeNode targetResourceGroupNode = SeekResourceGroupTreeNode();
            if (targetResourceGroupNode != null)
            {
                TreeNode[] matchingNodes = targetResourceGroupNode.Nodes.Find(migrationTarget.SourceName, true);
                foreach (TreeNode matchingNode in matchingNodes)
                {
                    if (matchingNode.Tag.GetType() == migrationTarget.GetType())
                        await RemoveTreeNodeCascadeUp(matchingNode);
                    else if (matchingNode.Tag.GetType() == typeof(Azure.MigrationTarget.StorageAccount))
                    {
                        if (migrationTarget.GetType() == typeof(Azure.Asm.StorageAccount))
                        {
                            await RemoveTreeNodeCascadeUp(matchingNode);
                        }
                        else if (migrationTarget.GetType() == typeof(Azure.Arm.StorageAccount))
                        {
                            await RemoveTreeNodeCascadeUp(matchingNode);
                        }
                    }
                    else if (matchingNode.Tag.GetType() == typeof(Azure.MigrationTarget.VirtualMachine))
                    {
                        if (migrationTarget.GetType() == typeof(Azure.Asm.VirtualMachine))
                        {
                            await RemoveTreeNodeCascadeUp(matchingNode);
                        }
                        else if (migrationTarget.GetType() == typeof(Azure.Arm.VirtualMachine))
                        {
                            await RemoveTreeNodeCascadeUp(matchingNode);
                        }
                    }
                    else if (matchingNode.Tag.GetType() == typeof(Azure.MigrationTarget.NetworkSecurityGroup))
                    {
                        if (migrationTarget.GetType() == typeof(Azure.Asm.NetworkSecurityGroup))
                        {
                            await RemoveTreeNodeCascadeUp(matchingNode);
                        }
                        else if (migrationTarget.GetType() == typeof(Azure.Arm.NetworkSecurityGroup))
                        {
                            await RemoveTreeNodeCascadeUp(matchingNode);
                        }
                    }
                    else if (matchingNode.Tag.GetType() == typeof(Azure.MigrationTarget.VirtualNetwork))
                    {
                        if (migrationTarget.GetType() == typeof(Azure.Asm.VirtualNetwork))
                        {
                            await RemoveTreeNodeCascadeUp(matchingNode);
                        }
                        else if (migrationTarget.GetType() == typeof(Azure.Arm.VirtualNetwork))
                        {
                            await RemoveTreeNodeCascadeUp(matchingNode);
                        }
                    }
                    else if (matchingNode.Tag.GetType() == typeof(TreeNode))
                    {
                        TreeNode childTreeNode = (TreeNode)matchingNode.Tag;
                        if (migrationTarget.GetType() == childTreeNode.Tag.GetType())
                            await RemoveTreeNodeCascadeUp(matchingNode);
                    }
                }
            }
        }

        private TreeNode SeekResourceGroupTreeNode()
        {
            TreeNode targetResourceGroupNode = SeekARMChildTreeNode(treeTargetARM.Nodes, _TargetResourceGroup.ToString(), _TargetResourceGroup.ToString(), _TargetResourceGroup, true);
            return targetResourceGroupNode;
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
                else if (tag.GetType() == typeof(Azure.MigrationTarget.LoadBalancer))
                {
                    childNode.ImageKey = "LoadBalancer";
                    childNode.SelectedImageKey = "LoadBalancer";
                }
                else if (tag.GetType() == typeof(Azure.MigrationTarget.PublicIp))
                {
                    childNode.ImageKey = "PublicIp";
                    childNode.SelectedImageKey = "PublicIp";
                }
                else
                    throw new ArgumentException("Unknown node tag type: " + tag.GetType().ToString());

                nodeCollection.Add(childNode);
                childNode.ExpandAll();
                return childNode;
            }
            return null;
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

        private TreeNode GetTargetAvailabilitySetNode(TreeNode subscriptionNode, Azure.MigrationTarget.AvailabilitySet targetAvailabilitySet)
        {
            foreach (TreeNode treeNode in subscriptionNode.Nodes)
            {
                if (treeNode.Tag != null)
                {
                    if (treeNode.Tag.GetType() == typeof(Azure.MigrationTarget.AvailabilitySet) && treeNode.Text == targetAvailabilitySet.ToString())
                        return treeNode;
                }
            }

            TreeNode tnAvailabilitySet = new TreeNode(targetAvailabilitySet.ToString());
            tnAvailabilitySet.Text = targetAvailabilitySet.ToString();
            tnAvailabilitySet.Tag = targetAvailabilitySet;
            tnAvailabilitySet.ImageKey = "AvailabilitySet";
            tnAvailabilitySet.SelectedImageKey = "AvailabilitySet";

            subscriptionNode.Nodes.Add(tnAvailabilitySet);
            tnAvailabilitySet.Expand();
            return tnAvailabilitySet;
        }

        private async void treeTargetARM_AfterSelect(object sender, TreeViewEventArgs e)
        {
            LogProvider.WriteLog("treeARM_AfterSelect", "Start");

            _PropertyPanel.Bind(e.Node);

            LogProvider.WriteLog("treeARM_AfterSelect", "End");
            StatusProvider.UpdateStatus("Ready");
        }

        private async Task Properties_PropertyChanged()
        {
            if (_SourceAwsNode == null) // we are not going to update on every property bind during TreeView updates
                await this.TemplateGenerator.UpdateArtifacts(GetExportArtifacts());
        }
    }
}
