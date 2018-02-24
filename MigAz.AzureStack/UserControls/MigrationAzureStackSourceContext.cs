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
using MigAz.Azure;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using MigAz.Azure.Forms;

namespace MigAz.AzureStack.UserControls
{
    public partial class MigrationAzureStackSourceContext : UserControl, IMigrationSourceUserControl
    {
        bool _IsAuthenticated = false;
        ILogProvider _LogProvider;
        IStatusProvider _StatusProvider;
        ImageList _ImageList;
        Core.TargetSettings _TargetSettings;
        AzureStackContext _AzureStackContextSource;

        #region Matching Events from AzureContext

        public delegate Task BeforeAzureTenantChangedHandler(AzureContext sender);
        public event BeforeAzureTenantChangedHandler BeforeAzureTenantChange;

        public delegate Task AfterAzureTenantChangedHandler(AzureContext sender);
        public event AfterAzureTenantChangedHandler AfterAzureTenantChange;

        public delegate Task BeforeAzureSubscriptionChangedHandler(AzureContext sender);
        public event BeforeAzureSubscriptionChangedHandler BeforeAzureSubscriptionChange;

        public delegate Task AfterAzureSubscriptionChangedHandler(AzureContext sender);
        public event AfterAzureSubscriptionChangedHandler AfterAzureSubscriptionChange;

        public delegate Task AzureEnvironmentChangedHandler(AzureContext sender);
        public event AzureEnvironmentChangedHandler AzureEnvironmentChanged;

        public delegate Task UserAuthenticatedHandler(AzureContext sender);
        public event UserAuthenticatedHandler UserAuthenticated;

        public delegate Task BeforeUserSignOutHandler();
        public event BeforeUserSignOutHandler BeforeUserSignOut;

        public delegate Task AfterUserSignOutHandler();
        public event AfterUserSignOutHandler AfterUserSignOut;

        #endregion

        #region New Events from MigrationAzureStackSource

        public delegate Task AfterNodeCheckedHandler(Core.MigrationTarget sender);
        public event AfterNodeCheckedHandler AfterNodeChecked;

        public delegate Task AfterNodeUncheckedHandler(Core.MigrationTarget sender);
        public event AfterNodeUncheckedHandler AfterNodeUnchecked;

        public delegate Task AfterNodeChangedHandler(Core.MigrationTarget sender);
        public event AfterNodeChangedHandler AfterNodeChanged;

        public delegate void ClearContextHandler();
        public event ClearContextHandler ClearContext;

        public delegate void AfterContextChangedHandler(MigrationAzureStackSourceContext sender);
        public event AfterContextChangedHandler AfterContextChanged;

        #endregion

        public MigrationAzureStackSourceContext()
        {
            InitializeComponent();
        }

        public async Task Bind(ILogProvider logProvider, IStatusProvider statusProvider, Core.TargetSettings targetSettings, ImageList imageList, PromptBehavior promptBehavior)
        {
            _TargetSettings = targetSettings;
            _LogProvider = logProvider;
            _StatusProvider = statusProvider;
            _ImageList = imageList;

            _AzureStackContextSource = new AzureStackContext(_LogProvider, _StatusProvider, promptBehavior);
            _AzureStackContextSource.AzureEnvironmentChanged += _AzureContext_AzureEnvironmentChanged;
            _AzureStackContextSource.UserAuthenticated += _AzureContext_UserAuthenticated;
            _AzureStackContextSource.BeforeAzureSubscriptionChange += _AzureContext_BeforeAzureSubscriptionChange;
            _AzureStackContextSource.AfterAzureSubscriptionChange += _AzureContext_AfterAzureSubscriptionChange;
            _AzureStackContextSource.BeforeUserSignOut += _AzureContext_BeforeUserSignOut;
            _AzureStackContextSource.AfterUserSignOut += _AzureContext_AfterUserSignOut;
            _AzureStackContextSource.AfterAzureTenantChange += _AzureContext_AfterAzureTenantChange;
            _AzureStackContextSource.BeforeAzureTenantChange += _AzureStackContext_BeforeAzureTenantChange;
            azureStackLoginContextViewer1.AfterContextChanged += AzureStackLoginContextViewerSource_AfterContextChanged;

            treeAzureARM.ImageList = _ImageList;

            await this.azureStackLoginContextViewer1.Bind(_AzureStackContextSource);
        }


        #region Properties

        public bool IsSourceContextAuthenticated
        {
            get { return _IsAuthenticated; }
            set { _IsAuthenticated = value; }
        }

        public AzureStackContext AzureStackContext
        {
            get { return _AzureStackContextSource; }
        }

        #endregion

        public async Task UncheckMigrationTarget(Core.MigrationTarget migrationTarget)
        {
            throw new Exception("Not Here");
        }

        private void MigrationAzureStackSourceContext_Resize(object sender, EventArgs e)
        {
            this.azureStackLoginContextViewer1.Width = this.Width;
            this.treeAzureARM.Width = this.Width - 5;
            this.treeAzureARM.Height = this.Height - 110;
        }


        #region Event Handlers

        private async Task AzureStackLoginContextViewerSource_AfterContextChanged(AzureStackLoginContextViewer sender)
        {
            AfterContextChanged?.Invoke(this);
        }


        private async Task _AzureStackContext_BeforeAzureTenantChange(AzureContext sender)
        {
            BeforeAzureTenantChange?.Invoke(sender);
        }

        private async Task _AzureContext_AfterAzureTenantChange(AzureContext sender)
        {
            //await _AzureContextTargetARM.CopyContext(_AzureStackContext);

            AfterAzureTenantChange?.Invoke(sender);
        }

        private async Task _AzureContext_BeforeAzureSubscriptionChange(AzureContext sender)
        {
            //await SaveSubscriptionSettings(sender.AzureSubscription);
            //await _AzureContextTargetARM.SetSubscriptionContext(null);

            BeforeAzureSubscriptionChange?.Invoke(sender);
        }

        private async Task _AzureContext_AzureEnvironmentChanged(AzureContext sender)
        {
            AzureEnvironmentChanged?.Invoke(sender);
        }


        private async Task _AzureContext_UserAuthenticated(AzureContext sender)
        {
            this.IsSourceContextAuthenticated = true;
            UserAuthenticated?.Invoke(sender);
        }

        private async Task _AzureContext_BeforeUserSignOut()
        {
            //await SaveSubscriptionSettings(this._AzureStackContext.AzureSubscription);

            BeforeUserSignOut?.Invoke();
        }

        private async Task _AzureContext_AfterUserSignOut()
        {
            ResetForm();
            this.IsSourceContextAuthenticated = false;

            //if (_AzureContextTargetARM != null)
            //    await _AzureContextTargetARM.SetSubscriptionContext(null);

            //if (_AzureContextTargetARM != null)
            //    await _AzureContextTargetARM.Logout();

            //AzureStackLoginContextViewerARM.Enabled = false;
            //AzureStackLoginContextViewerARM.Refresh();

            AfterUserSignOut?.Invoke();
        }

        private async Task _AzureContext_AfterAzureSubscriptionChange(AzureContext sender)
        {
            try
            {
                ResetForm();

                if (sender.AzureSubscription != null)
                {
                    await sender.AzureSubscription.InitializeChildrenAsync();
                    await BindArmResources(_TargetSettings);

                    //_AzureStackContext.AzureRetriever.SaveRestCache();
                    //        await ReadSubscriptionSettings(sender.AzureSubscription);
                }
            }
            catch (Exception exc)
            {
                UnhandledExceptionDialog unhandledException = new UnhandledExceptionDialog(_AzureStackContextSource.LogProvider, exc);
                unhandledException.ShowDialog();
            }

            _AzureStackContextSource.StatusProvider.UpdateStatus("Ready");

            AfterAzureSubscriptionChange?.Invoke(sender);
        }

        private async Task BindArmResources(Core.TargetSettings targetSettings)
        {
            //treeAzureARM.Nodes.Clear();

            try
            {
                if (_AzureStackContextSource != null && _AzureStackContextSource.AzureSubscription != null)
                {
                    await _AzureStackContextSource.AzureSubscription.BindArmResources(targetSettings);

                    if (_AzureStackContextSource != null && _AzureStackContextSource.AzureSubscription != null)
                    {
                        TreeNode subscriptionNodeARM = new TreeNode(_AzureStackContextSource.AzureSubscription.Name);
                        subscriptionNodeARM.ImageKey = "Subscription";
                        subscriptionNodeARM.SelectedImageKey = "Subscription";
                        treeAzureARM.Nodes.Add(subscriptionNodeARM);
                        subscriptionNodeARM.Expand();

                        foreach (Azure.MigrationTarget.NetworkSecurityGroup targetNetworkSecurityGroup in _AzureStackContextSource.AzureSubscription.ArmTargetNetworkSecurityGroups)
                        {
                            TreeNode networkSecurityGroupParentNode = GetResourceGroupTreeNode(subscriptionNodeARM, ((Azure.Arm.NetworkSecurityGroup)targetNetworkSecurityGroup.SourceNetworkSecurityGroup).ResourceGroup);

                            TreeNode tnNetworkSecurityGroup = new TreeNode(targetNetworkSecurityGroup.SourceName);
                            tnNetworkSecurityGroup.Name = targetNetworkSecurityGroup.SourceName;
                            tnNetworkSecurityGroup.Tag = targetNetworkSecurityGroup;
                            tnNetworkSecurityGroup.ImageKey = targetNetworkSecurityGroup.ImageKey;
                            tnNetworkSecurityGroup.SelectedImageKey = targetNetworkSecurityGroup.ImageKey;
                            networkSecurityGroupParentNode.Nodes.Add(tnNetworkSecurityGroup);
                        }

                        foreach (Azure.MigrationTarget.PublicIp targetPublicIP in _AzureStackContextSource.AzureSubscription.ArmTargetPublicIPs)
                        {
                            TreeNode publicIpParentNode = GetResourceGroupTreeNode(subscriptionNodeARM, ((Azure.Arm.PublicIP)targetPublicIP.Source).ResourceGroup); ;

                            TreeNode tnPublicIP = new TreeNode(targetPublicIP.SourceName);
                            tnPublicIP.Name = targetPublicIP.SourceName;
                            tnPublicIP.Tag = targetPublicIP;
                            tnPublicIP.ImageKey = targetPublicIP.ImageKey;
                            tnPublicIP.SelectedImageKey = targetPublicIP.ImageKey;
                            publicIpParentNode.Nodes.Add(tnPublicIP);
                        }

                        foreach (Azure.MigrationTarget.RouteTable targetRouteTable in _AzureStackContextSource.AzureSubscription.ArmTargetRouteTables)
                        {
                            TreeNode routeTableParentNode = GetResourceGroupTreeNode(subscriptionNodeARM, ((Azure.Arm.RouteTable)targetRouteTable.Source).ResourceGroup);

                            TreeNode tnRouteTable = new TreeNode(targetRouteTable.SourceName);
                            tnRouteTable.Name = targetRouteTable.SourceName;
                            tnRouteTable.Tag = targetRouteTable;
                            tnRouteTable.ImageKey = targetRouteTable.ImageKey;
                            tnRouteTable.SelectedImageKey = targetRouteTable.ImageKey;
                            routeTableParentNode.Nodes.Add(tnRouteTable);
                        }

                        foreach (Azure.MigrationTarget.VirtualNetwork targetVirtualNetwork in _AzureStackContextSource.AzureSubscription.ArmTargetVirtualNetworks)
                        {
                            TreeNode virtualNetworkParentNode = GetResourceGroupTreeNode(subscriptionNodeARM, ((Azure.Arm.VirtualNetwork)targetVirtualNetwork.SourceVirtualNetwork).ResourceGroup);

                            TreeNode tnVirtualNetwork = new TreeNode(targetVirtualNetwork.SourceName);
                            tnVirtualNetwork.Name = targetVirtualNetwork.SourceName;
                            tnVirtualNetwork.Tag = targetVirtualNetwork;
                            tnVirtualNetwork.ImageKey = targetVirtualNetwork.ImageKey;
                            tnVirtualNetwork.SelectedImageKey = targetVirtualNetwork.ImageKey;
                            virtualNetworkParentNode.Nodes.Add(tnVirtualNetwork);
                        }

                        foreach (Azure.MigrationTarget.StorageAccount targetStorageAccount in _AzureStackContextSource.AzureSubscription.ArmTargetStorageAccounts)
                        {
                            TreeNode storageAccountParentNode = GetResourceGroupTreeNode(subscriptionNodeARM, ((Azure.Arm.StorageAccount)targetStorageAccount.SourceAccount).ResourceGroup);

                            TreeNode tnStorageAccount = new TreeNode(targetStorageAccount.SourceName);
                            tnStorageAccount.Name = targetStorageAccount.SourceName;
                            tnStorageAccount.Tag = targetStorageAccount;
                            tnStorageAccount.ImageKey = targetStorageAccount.ImageKey;
                            tnStorageAccount.SelectedImageKey = targetStorageAccount.ImageKey;
                            storageAccountParentNode.Nodes.Add(tnStorageAccount);
                        }

                        foreach (Azure.MigrationTarget.Disk targetManagedDisk in _AzureStackContextSource.AzureSubscription.ArmTargetManagedDisks)
                        {
                            Azure.Arm.ManagedDisk armManagedDisk = (Azure.Arm.ManagedDisk)targetManagedDisk.SourceDisk;
                            TreeNode managedDiskParentNode = GetResourceGroupTreeNode(subscriptionNodeARM, armManagedDisk.ResourceGroup);

                            TreeNode tnDisk = new TreeNode(targetManagedDisk.SourceName);
                            tnDisk.Name = targetManagedDisk.SourceName;
                            tnDisk.Tag = targetManagedDisk;
                            tnDisk.ImageKey = targetManagedDisk.ImageKey;
                            tnDisk.SelectedImageKey = targetManagedDisk.ImageKey;
                            managedDiskParentNode.Nodes.Add(tnDisk);
                        }

                        foreach (Azure.MigrationTarget.AvailabilitySet targetAvailabilitySet in _AzureStackContextSource.AzureSubscription.ArmTargetAvailabilitySets)
                        {
                            TreeNode availabilitySetParentNode = GetResourceGroupTreeNode(subscriptionNodeARM, ((Azure.Arm.AvailabilitySet)targetAvailabilitySet.SourceAvailabilitySet).ResourceGroup);

                            TreeNode tnAvailabilitySet = new TreeNode(targetAvailabilitySet.SourceName);
                            tnAvailabilitySet.Name = targetAvailabilitySet.SourceName;
                            tnAvailabilitySet.Tag = targetAvailabilitySet;
                            tnAvailabilitySet.ImageKey = targetAvailabilitySet.ImageKey;
                            tnAvailabilitySet.SelectedImageKey = targetAvailabilitySet.ImageKey;
                            availabilitySetParentNode.Nodes.Add(tnAvailabilitySet);
                        }

                        foreach (Azure.MigrationTarget.NetworkInterface targetNetworkInterface in _AzureStackContextSource.AzureSubscription.ArmTargetNetworkInterfaces)
                        {
                            TreeNode tnResourceGroup = GetResourceGroupTreeNode(subscriptionNodeARM, ((Azure.Arm.NetworkInterface)targetNetworkInterface.SourceNetworkInterface).ResourceGroup);

                            TreeNode txNetworkInterface = new TreeNode(targetNetworkInterface.SourceName);
                            txNetworkInterface.Name = targetNetworkInterface.SourceName;
                            txNetworkInterface.Tag = targetNetworkInterface;
                            txNetworkInterface.ImageKey = targetNetworkInterface.ImageKey;
                            txNetworkInterface.SelectedImageKey = targetNetworkInterface.ImageKey;
                            tnResourceGroup.Nodes.Add(txNetworkInterface);
                        }

                        foreach (Azure.MigrationTarget.VirtualMachine targetVirtualMachine in _AzureStackContextSource.AzureSubscription.ArmTargetVirtualMachines)
                        {
                            TreeNode tnResourceGroup = GetResourceGroupTreeNode(subscriptionNodeARM, ((Azure.Arm.VirtualMachine)targetVirtualMachine.Source).ResourceGroup);

                            TreeNode tnVirtualMachine = new TreeNode(targetVirtualMachine.SourceName);
                            tnVirtualMachine.Name = targetVirtualMachine.SourceName;
                            tnVirtualMachine.Tag = targetVirtualMachine;
                            tnVirtualMachine.ImageKey = targetVirtualMachine.ImageKey;
                            tnVirtualMachine.SelectedImageKey = targetVirtualMachine.ImageKey;
                            tnResourceGroup.Nodes.Add(tnVirtualMachine);
                        }

                        foreach (Azure.MigrationTarget.LoadBalancer targetLoadBalancer in _AzureStackContextSource.AzureSubscription.ArmTargetLoadBalancers)
                        {
                            TreeNode networkSecurityGroupParentNode = GetResourceGroupTreeNode(subscriptionNodeARM, ((Azure.Arm.LoadBalancer)targetLoadBalancer.Source).ResourceGroup);

                            TreeNode tnNetworkSecurityGroup = new TreeNode(targetLoadBalancer.SourceName);
                            tnNetworkSecurityGroup.Name = targetLoadBalancer.SourceName;
                            tnNetworkSecurityGroup.Tag = targetLoadBalancer;
                            tnNetworkSecurityGroup.ImageKey = targetLoadBalancer.ImageKey;
                            tnNetworkSecurityGroup.SelectedImageKey = targetLoadBalancer.ImageKey;
                            networkSecurityGroupParentNode.Nodes.Add(tnNetworkSecurityGroup);
                        }

                        //foreach (MigrationTarget.VirtualMachineImage targetVirtualMachineImage in _AzureStackContext.AzureSubscription.ArmTargetVirtualMachineImages)
                        //{
                        //    TreeNode virtualMachineImageParentNode = GetResourceGroupTreeNode(subscriptionNodeARM, ((Azure.Arm.VirtualMachineImage)targetVirtualMachineImage.Source).ResourceGroup);

                        //    TreeNode tnVirtualMachineImage = new TreeNode(targetVirtualMachineImage.SourceName);
                        //    tnVirtualMachineImage.Name = targetVirtualMachineImage.SourceName;
                        //    tnVirtualMachineImage.Tag = targetVirtualMachineImage;
                        //    tnVirtualMachineImage.ImageKey = "VirtualMachineImage";
                        //    tnVirtualMachineImage.SelectedImageKey = "VirtualMachineImage";
                        //    virtualMachineImageParentNode.Nodes.Add(tnVirtualMachineImage);
                        //}

                        subscriptionNodeARM.Expand();
                        //treeAzureARM.Sort();
                        //treeAzureARM.Enabled = true;
                    }
                }
            }
            catch (Exception exc)
            {
                UnhandledExceptionDialog exceptionDialog = new UnhandledExceptionDialog(_AzureStackContextSource.LogProvider, exc);
                exceptionDialog.ShowDialog();
            }

            _AzureStackContextSource.StatusProvider.UpdateStatus("Ready");
        }

        #endregion

        private void ResetForm()
        {
            //if (treeAzureARM != null)
            //{
            //    treeAzureARM.Enabled = false;
            //    treeAzureARM.Nodes.Clear();
            //}

            //if (_SelectedNodes != null)
            //    _SelectedNodes.Clear();

            ClearContext?.Invoke();
        }

        private TreeNode GetResourceGroupTreeNode(TreeNode subscriptionNode, Azure.Arm.ResourceGroup resourceGroup)
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
            return tnResourceGroup;
        }

    }
}
