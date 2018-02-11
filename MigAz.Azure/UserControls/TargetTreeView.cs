using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MigAz.Core.Interface;
using MigAz.Azure.MigrationTarget;
using MigAz.Core.Generator;
using MigAz.Core;

namespace MigAz.Azure.UserControls
{
    public partial class TargetTreeView : UserControl
    {

        private ExportArtifacts _ExportArtifacts;
        private ResourceGroup _TargetResourceGroup;
        private TargetSettings _TargetSettings;

        public delegate Task AfterTargetSelectedHandler(TargetTreeView sender, TreeNode selectedNode);
        public event AfterTargetSelectedHandler AfterTargetSelected;

        public delegate Task AfterNewTargetResourceAddedHandler(TargetTreeView sender, TreeNode selectedNode);
        public event AfterNewTargetResourceAddedHandler AfterNewTargetResourceAdded;

        public delegate Task AfterExportArtifactRefreshHandler(TargetTreeView sender);
        public event AfterExportArtifactRefreshHandler AfterExportArtifactRefresh;

        public delegate Task AfterSourceNodeRemovedHandler(TargetTreeView sender, TreeNode removedNode);
        public event AfterSourceNodeRemovedHandler AfterSourceNodeRemoved;

        public TargetTreeView()
        {
            InitializeComponent();
        }

        public ILogProvider LogProvider
        {
            get; set;
        }

        public IStatusProvider StatusProvider
        {
            get; set;
        }

        public ImageList ImageList
        {
            get { return this.treeTargetARM.ImageList; }
            set
            {
                this.treeTargetARM.ImageList = value;
                this.btnNewAvailabilitySet.ImageList = value;
            }
        }

        public ResourceGroup TargetResourceGroup
        {
            get { return _TargetResourceGroup; }
        }

        public TreeNodeCollection Nodes
        {
            get { return this.treeTargetARM.Nodes; }
        }

        public TreeNode SelectedNode
        {
            get { return this.treeTargetARM.SelectedNode; }
            set { this.treeTargetARM.SelectedNode = value; }
        }

        public TreeNode ResourceGroupNode
        {
            get
            {
                if (this.Nodes.Count == 1)
                    return this.Nodes[0];
                else
                    return null;
            }
        }

        public new bool Enabled
        {
            get { return treeTargetARM.Enabled; }
            set
            {
                treeTargetARM.Enabled = value;
                panel1.Enabled = value;
            }
        }

        public ISettingsProvider SettingsProvider { get; set; }
        public bool HasStorageAccount
        {
            get
            {
                bool exists = false;

                foreach (TreeNode treeNode in treeTargetARM.Nodes)
                {
                    exists = this.ContainTagTypeRecursive(treeNode, typeof(StorageAccount));
                    if (exists)
                        return true;
                }

                return exists;
            }
        }

        private bool ContainTagTypeRecursive(TreeNode parentTreeNode, System.Type tagType)
        {
            if (parentTreeNode.Tag != null && parentTreeNode.Tag.GetType() == tagType)
                return true;
            else
            {
                foreach (TreeNode selectedNode in parentTreeNode.Nodes)
                {
                    if (ContainTagTypeRecursive(selectedNode, tagType))
                        return true;
                }

                return false;
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
                //else if (tagType == typeof(Azure.MigrationTarget.StorageAccount))
                //{
                //    exportArtifacts.StorageAccounts.Add((Azure.MigrationTarget.StorageAccount)selectedNode.Tag);
                //}
                else if (tagType == typeof(Azure.MigrationTarget.NetworkSecurityGroup))
                {
                    exportArtifacts.NetworkSecurityGroups.Add((Azure.MigrationTarget.NetworkSecurityGroup)selectedNode.Tag);
                }
                else if (tagType == typeof(Azure.MigrationTarget.AvailabilitySet))
                {
                    exportArtifacts.AvailablitySets.Add((Azure.MigrationTarget.AvailabilitySet)selectedNode.Tag);
                }
                else if (tagType == typeof(Azure.MigrationTarget.VirtualMachine))
                {
                    exportArtifacts.VirtualMachines.Add((Azure.MigrationTarget.VirtualMachine)selectedNode.Tag);
                }
                else if (tagType == typeof(Azure.MigrationTarget.LoadBalancer))
                {
                    exportArtifacts.LoadBalancers.Add((Azure.MigrationTarget.LoadBalancer)selectedNode.Tag);
                }
                else if (tagType == typeof(Azure.MigrationTarget.PublicIp))
                {
                    exportArtifacts.PublicIPs.Add((Azure.MigrationTarget.PublicIp)selectedNode.Tag);
                }
                else if (tagType == typeof(Azure.MigrationTarget.Disk))
                {
                    Azure.MigrationTarget.Disk targetDisk = (Azure.MigrationTarget.Disk)selectedNode.Tag;
                    if (targetDisk.TargetStorage != null && targetDisk.TargetStorage.GetType() == typeof(Azure.MigrationTarget.ManagedDiskStorage))
                    {
                        // We are only adding the Target Disk into the Export Artifacts as a Disk object if the Target Disk is targeted as a Managed Disk.
                        // Otherwise, it is a Classic Disk and will be generated as part of the Virtual Machine JSON (does not have a stand alone Disk object).
                        exportArtifacts.Disks.Add((Azure.MigrationTarget.Disk)selectedNode.Tag);
                    }
                }
                else if (tagType == typeof(Azure.MigrationTarget.NetworkInterface))
                {
                    exportArtifacts.NetworkInterfaces.Add((Azure.MigrationTarget.NetworkInterface)selectedNode.Tag);
                }
                else if (tagType == typeof(Azure.MigrationTarget.RouteTable))
                {
                    exportArtifacts.RouteTables.Add((Azure.MigrationTarget.RouteTable)selectedNode.Tag);
                }
            }

            foreach (TreeNode treeNode in parentTreeNode.Nodes)
            {
                GetExportArtifactsRecursive(treeNode, ref exportArtifacts);
            }
        }

        public async Task RefreshExportArtifacts()
        {
            _ExportArtifacts = new ExportArtifacts();

            if (this.Nodes.Count == 1 && this.Nodes[0].Tag != null)
                _ExportArtifacts.ResourceGroup = (MigrationTarget.ResourceGroup)this.Nodes[0].Tag;

            foreach (TreeNode treeNode in treeTargetARM.Nodes)
            {
                GetExportArtifactsRecursive(treeNode, ref _ExportArtifacts);
            }

            await _ExportArtifacts.ValidateAzureResources();
            await AfterExportArtifactRefresh?.Invoke(this);
        }

        public ExportArtifacts ExportArtifacts
        {
            get
            {
                if (_ExportArtifacts == null)
                    return null;

                if (_ExportArtifacts.HasErrors)
                    return null;
                else
                    return _ExportArtifacts;
            }
        }

        internal void TransitionToManagedDisk(TreeNode targetTreeNode)
        {
            if (targetTreeNode.Tag == null || targetTreeNode.Tag.GetType() != typeof(Azure.MigrationTarget.Disk))
                throw new ArgumentException("Invalid Treenode.  Treenode Tag must contain a Migration Disk object.");

            Azure.MigrationTarget.Disk targetDisk = (Azure.MigrationTarget.Disk)targetTreeNode.Tag;

            if (targetDisk.SourceDisk != null)
            {
                TreeNode diskParentNode = targetTreeNode.Parent;

                while (diskParentNode != null && diskParentNode.Tag != null && diskParentNode.Tag.GetType() != typeof(Azure.MigrationTarget.ResourceGroup))
                {
                    diskParentNode = diskParentNode.Parent;
                }

                // Move the disk treenode to be a child under the resource group, not under the VM, as we are making it a Managed Disk
                if (diskParentNode != null)
                {
                    treeTargetARM.Nodes.Remove(targetTreeNode);
                    diskParentNode.Nodes.Add(targetTreeNode);
                    treeTargetARM.SelectedNode = targetTreeNode;
                }
            }
        }

        internal void TransitionToClassicDisk(TreeNode targetTreeNode)
        {
            if (targetTreeNode.Tag == null || targetTreeNode.Tag.GetType() != typeof(Azure.MigrationTarget.Disk))
                throw new ArgumentException("Invalid Treenode.  Treenode Tag must contain a Migration Disk object.");

            Azure.MigrationTarget.Disk targetDisk = (Azure.MigrationTarget.Disk)targetTreeNode.Tag;

            if (targetDisk.SourceDisk != null)
            {
                TreeNode diskParentNode = targetTreeNode.Parent;

                // Move the disk treenode to be a child under the resource group, not under the VM, as we are making it a Managed Disk
                if (targetTreeNode.Parent.Tag != null && targetTreeNode.Parent.Tag.GetType() == typeof(Azure.MigrationTarget.ResourceGroup))
                {
                    // Seek the Virtual Machine Tree Node
                    TreeNode virtualMachineNode = SeekARMChildTreeNode(targetTreeNode.Parent.Nodes, targetDisk.ParentVirtualMachine.ToString(), targetDisk.ParentVirtualMachine.ToString(), targetDisk.ParentVirtualMachine, false);

                    if (virtualMachineNode != null)
                    {
                        treeTargetARM.Nodes.Remove(targetTreeNode);
                        virtualMachineNode.Nodes.Add(targetTreeNode);
                        treeTargetARM.SelectedNode = targetTreeNode;
                    }
                }
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
                    if (nodeObject != null && sourceObject != null)
                    {
                        if (nodeObject.GetType() == sourceObject.GetType() && sourceObject.ToString() == nodeObject.ToString())
                                treeTargetARM.SelectedNode = treeNode;
                    }
                }

                SeekAlertSourceRecursive(sourceObject, treeNode.Nodes);
            }
        }

        public TreeNode SeekARMChildTreeNode(string name, string text, object tag, bool allowCreated = false)
        {
            return SeekARMChildTreeNode(this.treeTargetARM.Nodes, name, text, tag, allowCreated);
        }

        public TreeNode SeekARMChildTreeNode(TreeNodeCollection nodeCollection, string name, string text, object tag, bool allowCreated = false)
        {
            TreeNode[] childNodeMatch = nodeCollection.Find(name, true);

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
                else if (tag.GetType() == typeof(Azure.MigrationTarget.RouteTable))
                {
                    childNode.ImageKey = "RouteTable";
                    childNode.SelectedImageKey = "RouteTable";
                }
                //else if (tag.GetType() == typeof(Azure.MigrationTarget.VirtualMachineImage))
                //{
                //    childNode.ImageKey = "VirtualMachineImage";
                //    childNode.SelectedImageKey = "VirtualMachineImage";
                //}
                else
                    throw new ArgumentException("Unknown node tag type: " + tag.GetType().ToString());

                nodeCollection.Add(childNode);
                childNode.ExpandAll();
                return childNode;
            }
            return null;
        }

        private TreeNode SeekResourceGroupTreeNode()
        {
            TreeNode targetResourceGroupNode = SeekARMChildTreeNode(treeTargetARM.Nodes, _TargetResourceGroup.ToString(), _TargetResourceGroup.ToString(), _TargetResourceGroup, true);
            return targetResourceGroupNode;
        }

        public async Task<TreeNode> AddMigrationTarget(Core.MigrationTarget parentNode)
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
            else if (parentNode.GetType() == typeof(Azure.MigrationTarget.NetworkInterface))
            {
                Azure.MigrationTarget.NetworkInterface targetNetworkInterface = (Azure.MigrationTarget.NetworkInterface)parentNode;
                TreeNode targetNetworkInterfaceNode = SeekARMChildTreeNode(targetResourceGroupNode.Nodes, targetNetworkInterface.ToString(), targetNetworkInterface.ToString(), targetNetworkInterface, true);

                targetResourceGroupNode.ExpandAll();
                return targetNetworkInterfaceNode;
            }
            else if (parentNode.GetType() == typeof(Azure.MigrationTarget.AvailabilitySet))
            {
                Azure.MigrationTarget.AvailabilitySet targetAvailabilitySet = (Azure.MigrationTarget.AvailabilitySet)parentNode;
                TreeNode targetAvailabilitySetNode = SeekARMChildTreeNode(targetResourceGroupNode.Nodes, targetAvailabilitySet.ToString(), targetAvailabilitySet.ToString(), targetAvailabilitySet, true);

                targetResourceGroupNode.ExpandAll();
                return targetAvailabilitySetNode;
            }
            else if (parentNode.GetType() == typeof(Azure.MigrationTarget.VirtualMachine))
            {
                Azure.MigrationTarget.VirtualMachine targetVirtualMachine = (Azure.MigrationTarget.VirtualMachine)parentNode;

                TreeNode virtualMachineParentNode = targetResourceGroupNode;
                TreeNode virtualMachineNode = SeekARMChildTreeNode(virtualMachineParentNode.Nodes, targetVirtualMachine.SourceName, targetVirtualMachine.ToString(), targetVirtualMachine, true);

                if (targetVirtualMachine.TargetAvailabilitySet != null)
                {
                    if (targetVirtualMachine.TargetAvailabilitySet.SourceAvailabilitySet != null && targetVirtualMachine.TargetAvailabilitySet.SourceAvailabilitySet.GetType() == typeof(Asm.CloudService))
                    {
                        // Adding under Virtual Machine, as it is not a managed disk
                        TreeNode dataDiskNode = SeekARMChildTreeNode(targetResourceGroupNode.Nodes, targetVirtualMachine.TargetAvailabilitySet.ToString(), targetVirtualMachine.TargetAvailabilitySet.ToString(), targetVirtualMachine.TargetAvailabilitySet, true);
                    }
                }

                if (targetVirtualMachine.OSVirtualHardDisk.IsUnmanagedDisk)
                {
                    // Adding under Virtual Machine, as it is not a managed disk
                    TreeNode dataDiskNode = SeekARMChildTreeNode(virtualMachineNode.Nodes, targetVirtualMachine.OSVirtualHardDisk.ToString(), targetVirtualMachine.OSVirtualHardDisk.ToString(), targetVirtualMachine.OSVirtualHardDisk, true);
                }
                else
                {
                    // Under Resource Group, as it is a managed Disk
                    TreeNode dataDiskNode = SeekARMChildTreeNode(virtualMachineParentNode.Nodes, targetVirtualMachine.OSVirtualHardDisk.ToString(), targetVirtualMachine.OSVirtualHardDisk.ToString(), targetVirtualMachine.OSVirtualHardDisk, true);
                }

                foreach (Azure.MigrationTarget.Disk targetDisk in targetVirtualMachine.DataDisks)
                {
                    if (targetDisk.IsUnmanagedDisk)
                    {
                        // Adding under Virtual Machine, as it is not a managed disk
                        TreeNode dataDiskNode = SeekARMChildTreeNode(virtualMachineNode.Nodes, targetDisk.ToString(), targetDisk.ToString(), targetDisk, true);
                    }
                    else
                    {
                        // Under Resource Group, as it is a managed Disk
                        TreeNode dataDiskNode = SeekARMChildTreeNode(virtualMachineParentNode.Nodes, targetDisk.ToString(), targetDisk.ToString(), targetDisk, true);
                    }
                }

                foreach (Azure.MigrationTarget.NetworkInterface targetNetworkInterface in targetVirtualMachine.NetworkInterfaces)
                {
                    if (targetNetworkInterface.SourceNetworkInterface != null && targetNetworkInterface.SourceNetworkInterface.GetType() == typeof(Azure.Asm.NetworkInterface))
                    {
                        // We are only adding as a child node if it is an ASM Network Interface, otherwise we expect this to follow ARM convention in which NIC is a first class object in the resource group (not embededded under the VM).
                        TreeNode networkInterfaceNode = SeekARMChildTreeNode(targetResourceGroupNode.Nodes, targetNetworkInterface.ToString(), targetNetworkInterface.ToString(), targetNetworkInterface, true);
                    }
                }

                targetResourceGroupNode.ExpandAll();
                return virtualMachineNode;
            }
            else if (parentNode.GetType() == typeof(Azure.MigrationTarget.Disk))
            {
                Azure.MigrationTarget.Disk targetDisk = (Azure.MigrationTarget.Disk)parentNode;
                TreeNode targetDiskNode = SeekARMChildTreeNode(targetResourceGroupNode.Nodes, targetDisk.SourceName, targetDisk.ToString(), targetDisk, true);

                targetResourceGroupNode.ExpandAll();
                return targetDiskNode;
            }
            //else if (parentNode.GetType() == typeof(Azure.MigrationTarget.VirtualMachineImage))
            //{
            //    Azure.MigrationTarget.VirtualMachineImage targetVirtualMachineImage = (Azure.MigrationTarget.VirtualMachineImage)parentNode;
            //    TreeNode targetVirtualMachineImageNode = SeekARMChildTreeNode(targetResourceGroupNode.Nodes, targetVirtualMachineImage.SourceName, targetVirtualMachineImage.ToString(), targetVirtualMachineImage, true);

            //    targetResourceGroupNode.ExpandAll();
            //    return targetVirtualMachineImageNode;
            //}
            else if (parentNode.GetType() == typeof(Azure.MigrationTarget.NetworkInterface))
            {
                Azure.MigrationTarget.NetworkInterface targetNetworkInterface = (Azure.MigrationTarget.NetworkInterface)parentNode;
                TreeNode targetNetworkInterfaceNode = SeekARMChildTreeNode(targetResourceGroupNode.Nodes, targetNetworkInterface.SourceName, targetNetworkInterface.ToString(), targetNetworkInterface, true);

                targetResourceGroupNode.ExpandAll();
                return targetNetworkInterfaceNode;
            }
            else if (parentNode.GetType() == typeof(Azure.MigrationTarget.RouteTable))
            {
                Azure.MigrationTarget.RouteTable targetRouteTable = (Azure.MigrationTarget.RouteTable)parentNode;
                TreeNode targetRouteTableNode = SeekARMChildTreeNode(targetResourceGroupNode.Nodes, targetRouteTable.SourceName, targetRouteTable.ToString(), targetRouteTable, true);

                targetRouteTableNode.ExpandAll();
                return targetRouteTableNode;
            }
            else
                throw new Exception("Unhandled Node Type in AddMigrationTargetToTargetTree: " + parentNode.GetType());

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

        public TreeNode SeekMigrationTargetTreeNode(Core.MigrationTarget migrationTarget)
        {
            return SeekMigrationTargetTreeNode(this.treeTargetARM.Nodes, migrationTarget);
        }

        public static TreeNode SeekMigrationTargetTreeNode(TreeNodeCollection treeNodeCollection, Core.MigrationTarget migrationTarget)
        {
            foreach (TreeNode treeNode in treeNodeCollection)
            {
                TreeNode matchingNode = SeekMigrationTargetTreeNodeRecursive(treeNode, migrationTarget);
                if (matchingNode != null)
                    return matchingNode;
            }

            return null;
        }

        private static TreeNode SeekMigrationTargetTreeNodeRecursive(TreeNode treeNode, Core.MigrationTarget migrationTarget)
        {
            if (treeNode.Tag != null && treeNode.Tag.GetType().BaseType == typeof(Core.MigrationTarget))
            {
                Core.MigrationTarget treeNodeMigrationTarget = (Core.MigrationTarget)treeNode.Tag;
                if (treeNodeMigrationTarget == migrationTarget)
                    return treeNode;
            }

            foreach (TreeNode treeNodeChild in treeNode.Nodes)
            {
                TreeNode treeNodeChildMatch = SeekMigrationTargetTreeNodeRecursive(treeNodeChild, migrationTarget);
                if (treeNodeChildMatch != null)
                    return treeNodeChildMatch;
            }

            return null;
        }

        public async Task RemoveMigrationTarget(Core.MigrationTarget migrationTarget)
        {
            TreeNode matchingNode = SeekMigrationTargetTreeNode(migrationTarget);
            if (matchingNode != null)
            {
                Core.MigrationTarget matchingNodeMigrationTarget = (Core.MigrationTarget)matchingNode.Tag;
                if (matchingNodeMigrationTarget == migrationTarget)
                {
                    if (matchingNode.Tag.GetType() == typeof(Azure.MigrationTarget.VirtualMachine))
                    {
                        await RemoveTreeNodeCascadeUp(matchingNode);

                        Azure.MigrationTarget.VirtualMachine targetVirtualMachine = (Azure.MigrationTarget.VirtualMachine)matchingNode.Tag;

                        // if there are VMs that have originating Classic Disks, we may need to clean them up from the Resource Group if they were being moved to a Managed Disk (not under the VM in the Treeview)
                        await RemoveAsmDiskTurnedManagedDiskFromARMTree(targetVirtualMachine.OSVirtualHardDisk);
                        foreach (MigrationTarget.Disk disk in targetVirtualMachine.DataDisks)
                        {
                            await RemoveAsmDiskTurnedManagedDiskFromARMTree(disk);
                        }
                        foreach (MigrationTarget.NetworkInterface networkInterface in targetVirtualMachine.NetworkInterfaces)
                        {
                            await RemoveAsmNetworkTurnedArmNetworkFromARMTree(networkInterface);
                        }
                        await RemoveCloudServiceTurnedAvailabilitySetFromARMTree(targetVirtualMachine.TargetAvailabilitySet);
                    }
                    else if (matchingNode.Tag.GetType() == migrationTarget.GetType())
                        await RemoveTreeNodeCascadeUp(matchingNode);
                    else if (matchingNode.Tag.GetType() == typeof(TreeNode))
                    {
                        TreeNode childTreeNode = (TreeNode)matchingNode.Tag;
                        if (migrationTarget.GetType() == childTreeNode.Tag.GetType())
                            await RemoveTreeNodeCascadeUp(matchingNode);
                    }
                }
            }
        }

        private async Task RemoveAsmDiskTurnedManagedDiskFromARMTree(Disk disk)
        {
            if (disk.SourceDisk != null && (disk.SourceDisk.GetType() == typeof(Azure.Asm.Disk) || disk.SourceDisk.GetType() == typeof(Azure.Arm.ClassicDisk)))
            {
                

                TreeNode targetResourceGroupNode = SeekResourceGroupTreeNode();
                if (targetResourceGroupNode != null)
                {
                    TreeNode[] matchingNodes = targetResourceGroupNode.Nodes.Find(disk.TargetName, true);
                    foreach (TreeNode matchingNode in matchingNodes)
                    {
                        if (matchingNode.Tag != null && matchingNode.Tag.GetType() == typeof(Azure.MigrationTarget.Disk) && String.Compare(((Azure.MigrationTarget.Disk)matchingNode.Tag).SourceName, disk.SourceName, true) == 0)
                        {
                            await RemoveTreeNodeCascadeUp(matchingNode);
                        }
                    }
                }
            }
        }

        private async Task RemoveAsmNetworkTurnedArmNetworkFromARMTree(NetworkInterface networkInterface)
        {
            if (networkInterface.SourceNetworkInterface != null && networkInterface.SourceNetworkInterface.GetType() == typeof(Azure.Asm.NetworkInterface))
            {
                TreeNode targetResourceGroupNode = SeekResourceGroupTreeNode();
                if (targetResourceGroupNode != null)
                {
                    TreeNode[] matchingNodes = targetResourceGroupNode.Nodes.Find(networkInterface.ToString(), true);
                    foreach (TreeNode matchingNode in matchingNodes)
                    {
                        if (matchingNode.Tag != null && matchingNode.Tag.GetType() == typeof(Azure.MigrationTarget.NetworkInterface) && String.Compare(((Azure.MigrationTarget.NetworkInterface)matchingNode.Tag).SourceName, networkInterface.SourceName, true) == 0)
                        {
                            await RemoveTreeNodeCascadeUp(matchingNode);
                        }
                    }
                }
            }
        }

        private async Task RemoveCloudServiceTurnedAvailabilitySetFromARMTree(AvailabilitySet availabilitySet)
        {
            if (availabilitySet != null)
            {
                if (availabilitySet.SourceAvailabilitySet != null && availabilitySet.SourceAvailabilitySet.GetType() == typeof(Azure.Asm.CloudService))
                {
                    TreeNode targetResourceGroupNode = SeekResourceGroupTreeNode();
                    if (targetResourceGroupNode != null)
                    {
                        TreeNode[] matchingNodes = targetResourceGroupNode.Nodes.Find(availabilitySet.ToString(), true);
                        foreach (TreeNode matchingNode in matchingNodes)
                        {
                            if (matchingNode.Tag != null && matchingNode.Tag.GetType() == typeof(Azure.MigrationTarget.AvailabilitySet) && String.Compare(((Azure.MigrationTarget.AvailabilitySet)matchingNode.Tag).SourceName, availabilitySet.SourceName, true) == 0)
                            {
                                await RemoveTreeNodeCascadeUp(matchingNode);
                            }
                        }
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

        public void SeekAlertSource(object sourceObject)
        {
            SeekAlertSourceRecursive(sourceObject, treeTargetARM.Nodes);
        }


        private void TargetTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            AfterTargetSelected?.Invoke(this, e.Node);
        }

        private void TargetTreeView_Resize(object sender, EventArgs e)
        {
            treeTargetARM.Width = this.Width - panel1.Width - 10 - 10;
            treeTargetARM.Height = this.Height - 40;
            panel1.Left = treeTargetARM.Left + treeTargetARM.Width + 10;
            lblAddNew.Left = panel1.Left;
        }

        public List<Azure.MigrationTarget.VirtualNetwork> GetVirtualNetworksInMigration()
        {
            List<Azure.MigrationTarget.VirtualNetwork> _TargetVirtualNetworks = new List<MigrationTarget.VirtualNetwork>();

            TreeNode targetResourceGroupNode = this.ResourceGroupNode;

            if (targetResourceGroupNode != null)
            {
                foreach (TreeNode treeNode in targetResourceGroupNode.Nodes)
                {
                    if (treeNode.Tag != null && treeNode.Tag.GetType() == typeof(Azure.MigrationTarget.VirtualNetwork))
                    {
                        Azure.MigrationTarget.VirtualNetwork targetVirtualNetwork = (Azure.MigrationTarget.VirtualNetwork)treeNode.Tag;
                        _TargetVirtualNetworks.Add(targetVirtualNetwork);
                    }
                }
            }

            return _TargetVirtualNetworks;
        }

        public void Clear()
        {
            treeTargetARM.Nodes.Clear();
        }

        public List<MigAzGeneratorAlert> Alerts
        {
            get
            {
                if (_ExportArtifacts == null)
                    return new List<MigAzGeneratorAlert>();
                else
                    return _ExportArtifacts.Alerts;
            }
        }

        public bool HasErrors
        {
            get
            {
                if (_ExportArtifacts == null)
                    return false;
                else
                    return _ExportArtifacts.HasErrors;
            }
        }

        public TargetSettings TargetSettings
        {
            get { return _TargetSettings; }
            set
            {
                _TargetSettings = value;
                if (value != null)
                    if (_TargetResourceGroup == null)
                        _TargetResourceGroup = new ResourceGroup(value);
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            // Code has been slightly strucutured with anticipate of need for reuse across different object types
            string baseResourceName = "AvailabilitySet";
            int counter = 0;
            bool nameExists = true;

            string newResourceName = "New" + baseResourceName;
            while (nameExists)
            {
                if (counter > 0)
                    newResourceName = "New" + baseResourceName + counter.ToString();

                TreeNode[] matchingNodes = treeTargetARM.Nodes.Find(newResourceName, true);
                nameExists = matchingNodes.Count() > 0;
                counter++;
            }

            AvailabilitySet availabilitySet = new AvailabilitySet(newResourceName, this.TargetSettings);
            TreeNode newNode = this.AddMigrationTarget(availabilitySet).Result;

            await this.RefreshExportArtifacts();
            AfterNewTargetResourceAdded?.Invoke(this, newNode);
        }

        public static int GetChildNodeCount(TreeNode treeNode)
        {
            int childCount = 0;

            foreach (TreeNode childNode in treeNode.Nodes)
            {
                childCount += GetChildNodeCountRecursive(childNode);
            }

            return childCount;
        }

        private static int GetChildNodeCountRecursive(TreeNode treeNode)
        {
            int childCount = 0;

            childCount += 1; // Count this node
            foreach (TreeNode childNode in treeNode.Nodes)
            {
                childCount += GetChildNodeCountRecursive(childNode);
            }

            return childCount;
        }

        private async void treeTargetARM_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (treeTargetARM.SelectedNode != null)
                {
                    Core.MigrationTarget migrationTarget = (Core.MigrationTarget)treeTargetARM.SelectedNode.Tag;

                    int countChildNodes = GetChildNodeCount(treeTargetARM.SelectedNode);
                    String strChildNodeCount = String.Empty;
                    if (countChildNodes > 0)
                        strChildNodeCount = " **AND** " + countChildNodes.ToString() + " child resource(s)";

                    string deleteConfirmationText = String.Format("Are you sure you want to remove {0} '{1}'{2} as a target resource?", new string[] { migrationTarget.GetType().ToString(), migrationTarget.ToString(), strChildNodeCount });
                    if (MessageBox.Show(deleteConfirmationText, "Remove Target Resource(s)", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        await RemoveNodeStartingWithLastChildrenBackUpTree(treeTargetARM, treeTargetARM.SelectedNode);
                    }
                }
            }
        }

        private async Task RemoveNodeStartingWithLastChildrenBackUpTree(TreeView treeTargetARM, TreeNode selectedNode)
        {
            if (selectedNode != null)
            {
                foreach (TreeNode childNode in selectedNode.Nodes)
                {
                    await RemoveNodeStartingWithLastChildrenBackUpTree(treeTargetARM, childNode);
                }

                treeTargetARM.Nodes.Remove(selectedNode);

                AfterSourceNodeRemoved?.Invoke(this, selectedNode);
            }
        }

        private async void treeTargetARM_DragDrop(object sender, DragEventArgs e)
        {
            string methodName = "treeTargetARM_DragDrop";
            
            this.LogProvider.WriteLog(methodName, "Start");

            if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false))
            {
                Point pt = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));
                TreeNode destinationNode = ((TreeView)sender).GetNodeAt(pt);
                TreeNode sourceNode = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");

                if (sourceNode != null && sourceNode.Tag != null && sourceNode.Tag.GetType().BaseType == typeof(Core.MigrationTarget) && destinationNode != null && destinationNode.Tag != null && destinationNode.Tag.GetType().BaseType == typeof(Core.MigrationTarget))
                {
                    bool isMigAzTargetDragDropHandled = false;
                    Core.MigrationTarget sourceNodeTarget = (Core.MigrationTarget)sourceNode.Tag;
                    Core.MigrationTarget destinationNodeTarget = (Core.MigrationTarget)destinationNode.Tag;
                    this.LogProvider.WriteLog(methodName, "Source Node Tag - Name '" + sourceNodeTarget.ToString() + "' Type '" + sourceNodeTarget.GetType() + "'");
                    this.LogProvider.WriteLog(methodName, "Target Node Tag - Name '" + destinationNodeTarget.ToString() + "' Type '" + destinationNodeTarget.GetType() + "'");

                    if (sourceNodeTarget.GetType() == typeof(VirtualMachine))
                    {
                        VirtualMachine virtualMachine = (VirtualMachine)sourceNodeTarget;

                        if (destinationNodeTarget.GetType() == typeof(AvailabilitySet))
                        {
                            AvailabilitySet availabilitySet = (AvailabilitySet)destinationNodeTarget;
                            
                            // Update Virtual Machine to reflect ownership in new target Availability Set
                            virtualMachine.TargetAvailabilitySet = availabilitySet;

                            isMigAzTargetDragDropHandled = true;
                        }
                    }

                    if (!isMigAzTargetDragDropHandled)
                        this.LogProvider.WriteLog(methodName, "Drag from " + sourceNodeTarget.GetType().ToString() + " to " + destinationNodeTarget.GetType().ToString() + " was not handled.  No DragDrop action taken.");
                    else
                    {
                        await this.RefreshExportArtifacts();
                    }
                }
                else
                {
                    if (sourceNode == null)
                        this.LogProvider.WriteLog(methodName, "SourceNode is null.  No DragDrop action taken.");
                    else if (sourceNode.Tag == null)
                        this.LogProvider.WriteLog(methodName, "SourceNode exists, but Tag is null.  No DragDrop action taken.");
                    else if (sourceNode.Tag != null)
                        this.LogProvider.WriteLog(methodName, "SourceNode exists, Tag is not null, but not of type IMigrationTarget.  No DragDrop action taken.");

                    if (destinationNode == null)
                        this.LogProvider.WriteLog(methodName, "DestinationNode is null.  No DragDrop action taken.");
                    else if (destinationNode.Tag == null)
                        this.LogProvider.WriteLog(methodName, "DestinationNode exists, but Tag is null.  No DragDrop action taken.");
                    else if (destinationNode.Tag != null)
                        this.LogProvider.WriteLog(methodName, "DestinationNode exists, Tag is not null, but not of type IMigrationTarget.  No DragDrop action taken.");
                }
            }

            this.LogProvider.WriteLog(methodName, "End");
        }

        private void treeTargetARM_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void treeTargetARM_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }
    }
}
