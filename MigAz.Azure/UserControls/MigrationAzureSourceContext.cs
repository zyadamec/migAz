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
using MigAz.Azure.Interface;
using System.Net;
using MigAz.Azure.Arm;
using MigAz.Azure.Forms;
using MigAz.Core;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace MigAz.Azure.UserControls
{
    public partial class MigrationAzureSourceContext : UserControl, IMigrationSourceUserControl
    {
        private AzureContext _AzureContextSource;
        private List<TreeNode> _SelectedNodes = new List<TreeNode>();
        private TreeNode _SourceAsmNode;
        private TargetSettings _TargetSettings;
        private ImageList _ImageList;
        private ArmDiskType _DefaultTargetDiskType = ArmDiskType.ManagedDisk;
        private bool _AutoSelectDependencies = true;
        private bool _IsAuthenticated = false;
        private ILogProvider _LogProvider;
        private IStatusProvider _StatusProvider;

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

        #region New Events from MigrationSourceAzure

        public delegate Task AfterNodeCheckedHandler(Core.MigrationTarget sender);
        public event AfterNodeCheckedHandler AfterNodeChecked;

        public delegate Task AfterNodeUncheckedHandler(Core.MigrationTarget sender);
        public event AfterNodeUncheckedHandler AfterNodeUnchecked;

        public delegate Task AfterNodeChangedHandler(Core.MigrationTarget sender);
        public event AfterNodeChangedHandler AfterNodeChanged;

        public delegate void ClearContextHandler();
        public event ClearContextHandler ClearContext;

        public delegate void AfterContextChangedHandler(UserControl sender);
        public event AfterContextChangedHandler AfterContextChanged;

        #endregion

        #region Constructor(s)

        public MigrationAzureSourceContext()
        {
            InitializeComponent();
            cmbAzureResourceTypeSource.SelectedIndex = 0;

            treeAzureASM.Left = 0;
            treeAzureASM.Width = this.Width;
            treeAzureARM.Left = 0;
            treeAzureARM.Width = this.Width;
        }

        #endregion

        #region Properties

        public ILogProvider LogProvider
        {
            get { return _LogProvider; }
        }

        public bool IsSourceContextAuthenticated
        {
            get { return _IsAuthenticated; }
            set { _IsAuthenticated = value; }
        }
        public AzureContext AzureContext
        {
            get { return _AzureContextSource; }
        }

        public ArmDiskType DefaultTargetDiskType
        {
            get { return _DefaultTargetDiskType; }
            set { _DefaultTargetDiskType = value; }
        }

        public bool AutoSelectDependencies
        {
            get { return _AutoSelectDependencies; }
            set { _AutoSelectDependencies = value; }
        }

        #endregion

        #region Methods

        public async Task Bind(IStatusProvider statusProvider, ILogProvider logProvider, TargetSettings targetSettings, ImageList imageList, PromptBehavior promptBehavior)
        {
            _TargetSettings = targetSettings;
            _LogProvider = logProvider;
            _StatusProvider = statusProvider;
            _ImageList = imageList;

            _AzureContextSource = new AzureContext(logProvider, statusProvider, promptBehavior);
            _AzureContextSource.AzureEnvironmentChanged += _AzureContext_AzureEnvironmentChanged;
            _AzureContextSource.UserAuthenticated += _AzureContext_UserAuthenticated;
            _AzureContextSource.BeforeAzureSubscriptionChange += _AzureContext_BeforeAzureSubscriptionChange;
            _AzureContextSource.AfterAzureSubscriptionChange += _AzureContext_AfterAzureSubscriptionChange;
            _AzureContextSource.BeforeUserSignOut += _AzureContext_BeforeUserSignOut;
            _AzureContextSource.AfterUserSignOut += _AzureContext_AfterUserSignOut;
            _AzureContextSource.AfterAzureTenantChange += _AzureContext_AfterAzureTenantChange;
            _AzureContextSource.BeforeAzureTenantChange += _AzureContextSource_BeforeAzureTenantChange;
            azureLoginContextViewerSource.AfterContextChanged += AzureLoginContextViewerSource_AfterContextChanged;

            await azureLoginContextViewerSource.Bind(_AzureContextSource);

            treeAzureARM.ImageList = _ImageList;
        }

        private void ResetForm()
        {
            if (treeAzureASM != null)
            {
                treeAzureASM.Enabled = false;
                treeAzureASM.Nodes.Clear();
            }

            if (treeAzureARM != null)
            {
                treeAzureARM.Enabled = false;
                treeAzureARM.Nodes.Clear();
            }

            if (_SelectedNodes != null)
                _SelectedNodes.Clear();

            ClearContext?.Invoke();
        }

        #endregion

        #region Event Handlers

        private async Task AzureLoginContextViewerSource_AfterContextChanged(AzureLoginContextViewer sender)
        {
            AfterContextChanged?.Invoke(this);
        }


        private async Task _AzureContextSource_BeforeAzureTenantChange(AzureContext sender)
        {
            BeforeAzureTenantChange?.Invoke(sender);
        }

        private async Task _AzureContext_AfterAzureTenantChange(AzureContext sender)
        {
            //await _AzureContextTargetARM.CopyContext(_AzureContextSource);

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
            //await SaveSubscriptionSettings(this._AzureContextSource.AzureSubscription);

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

            //azureLoginContextViewerARM.Enabled = false;
            //azureLoginContextViewerARM.Refresh();

            AfterUserSignOut?.Invoke();
        }

        private async Task _AzureContext_AfterAzureSubscriptionChange(AzureContext sender)
        {
            try
            {
                ResetForm();

                if (sender.AzureSubscription != null)
                {
                    await sender.AzureSubscription.InitializeChildrenAsync(false);

                    switch (cmbAzureResourceTypeSource.SelectedItem.ToString())
                    {
                        case "Azure Service Management (ASM / Classic)":
                            await BindAsmResources(_TargetSettings);
                            break;
                        case "Azure Resource Manager (ARM)":
                            await BindArmResources(_TargetSettings);
                            break;
                        default:
                            throw new ArgumentException("Unexpected Source Resource Tab: " + cmbAzureResourceTypeSource.SelectedValue);
                    }

                    _AzureContextSource.AzureRetriever.SaveRestCache();
                    //        await ReadSubscriptionSettings(sender.AzureSubscription);
                }
            }
            catch (Exception exc)
            {
                UnhandledExceptionDialog unhandledException = new UnhandledExceptionDialog(_AzureContextSource.LogProvider, exc);
                unhandledException.ShowDialog();
            }

            _AzureContextSource.StatusProvider.UpdateStatus("Ready");

            AfterAzureSubscriptionChange?.Invoke(sender);
        }


        private async Task BindAsmResources(TargetSettings targetSettings)
        {
            treeAzureASM.Nodes.Clear();

            try
            {
                if (_AzureContextSource != null && _AzureContextSource.AzureSubscription != null)
                {

                    await _AzureContextSource.AzureSubscription.BindAsmResources(targetSettings);

                    if (_AzureContextSource != null && _AzureContextSource.AzureSubscription != null)
                    {
                        TreeNode subscriptionNodeASM = new TreeNode(_AzureContextSource.AzureSubscription.Name);
                        treeAzureASM.Nodes.Add(subscriptionNodeASM);
                        subscriptionNodeASM.Expand();

                        foreach (Azure.MigrationTarget.NetworkSecurityGroup targetNetworkSecurityGroup in _AzureContextSource.AzureSubscription.AsmTargetNetworkSecurityGroups)
                        {
                            Azure.Asm.NetworkSecurityGroup asmNetworkSecurityGroup = (Azure.Asm.NetworkSecurityGroup)targetNetworkSecurityGroup.SourceNetworkSecurityGroup;
                            TreeNode parentNode = GetDataCenterTreeViewNode(subscriptionNodeASM, asmNetworkSecurityGroup.Location, "Network Security Groups");

                            TreeNode tnNetworkSecurityGroup = new TreeNode(targetNetworkSecurityGroup.SourceName);
                            tnNetworkSecurityGroup.Name = targetNetworkSecurityGroup.SourceName;
                            tnNetworkSecurityGroup.Tag = targetNetworkSecurityGroup;
                            parentNode.Nodes.Add(tnNetworkSecurityGroup);
                            parentNode.Expand();
                        }

                        foreach (Azure.MigrationTarget.VirtualNetwork targetVirtualNetwork in _AzureContextSource.AzureSubscription.AsmTargetVirtualNetworks)
                        {
                            Azure.Asm.VirtualNetwork asmVirtualNetwork = (Azure.Asm.VirtualNetwork)targetVirtualNetwork.SourceVirtualNetwork;
                            TreeNode parentNode = GetDataCenterTreeViewNode(subscriptionNodeASM, asmVirtualNetwork.Location, "Virtual Networks");

                            TreeNode tnVirtualNetwork = new TreeNode(targetVirtualNetwork.SourceName);
                            tnVirtualNetwork.Name = targetVirtualNetwork.SourceName;
                            tnVirtualNetwork.Text = targetVirtualNetwork.SourceName;
                            tnVirtualNetwork.Tag = targetVirtualNetwork;
                            parentNode.Nodes.Add(tnVirtualNetwork);
                            parentNode.Expand();
                        }

                        foreach (Azure.MigrationTarget.StorageAccount targetStorageAccount in _AzureContextSource.AzureSubscription.AsmTargetStorageAccounts)
                        {
                            TreeNode parentNode = GetDataCenterTreeViewNode(subscriptionNodeASM, targetStorageAccount.SourceAccount.PrimaryLocation, "Storage Accounts");

                            TreeNode tnStorageAccount = new TreeNode(targetStorageAccount.SourceName);
                            tnStorageAccount.Name = targetStorageAccount.SourceName;
                            tnStorageAccount.Tag = targetStorageAccount;
                            parentNode.Nodes.Add(tnStorageAccount);
                            parentNode.Expand();
                        }

                        foreach (Azure.MigrationTarget.VirtualMachine targetVirtualMachine in _AzureContextSource.AzureSubscription.AsmTargetVirtualMachines)
                        {
                            Azure.Asm.VirtualMachine asmVirtualMachine = (Azure.Asm.VirtualMachine)targetVirtualMachine.Source;
                            TreeNode parentNode = GetDataCenterTreeViewNode(subscriptionNodeASM, asmVirtualMachine.Location, "Cloud Services");
                            TreeNode[] cloudServiceNodeSearch = parentNode.Nodes.Find(targetVirtualMachine.TargetAvailabilitySet.TargetName, false);
                            TreeNode cloudServiceNode = null;
                            if (cloudServiceNodeSearch.Count() == 1)
                            {
                                cloudServiceNode = cloudServiceNodeSearch[0];
                            }

                            cloudServiceNode = new TreeNode(targetVirtualMachine.TargetAvailabilitySet.TargetName);
                            cloudServiceNode.Name = targetVirtualMachine.TargetAvailabilitySet.TargetName;
                            cloudServiceNode.Tag = targetVirtualMachine.TargetAvailabilitySet;
                            parentNode.Nodes.Add(cloudServiceNode);
                            parentNode.Expand();

                            TreeNode virtualMachineNode = new TreeNode(targetVirtualMachine.SourceName);
                            virtualMachineNode.Name = targetVirtualMachine.SourceName;
                            virtualMachineNode.Tag = targetVirtualMachine;
                            cloudServiceNode.Nodes.Add(virtualMachineNode);
                            cloudServiceNode.Expand();

                            foreach (Azure.MigrationTarget.NetworkInterface targetNetworkInterface in targetVirtualMachine.NetworkInterfaces)
                            {
                                if (targetNetworkInterface.BackEndAddressPool != null && targetNetworkInterface.BackEndAddressPool.LoadBalancer != null)
                                {
                                    TreeNode loadBalancerNode = new TreeNode(targetNetworkInterface.BackEndAddressPool.LoadBalancer.SourceName);
                                    loadBalancerNode.Name = targetNetworkInterface.BackEndAddressPool.LoadBalancer.SourceName;
                                    loadBalancerNode.Tag = targetNetworkInterface.BackEndAddressPool.LoadBalancer;
                                    cloudServiceNode.Nodes.Add(loadBalancerNode);
                                    cloudServiceNode.Expand();

                                    foreach (Azure.MigrationTarget.FrontEndIpConfiguration frontEnd in targetNetworkInterface.BackEndAddressPool.LoadBalancer.FrontEndIpConfigurations)
                                    {
                                        if (frontEnd.PublicIp != null) // if external load balancer
                                        {
                                            TreeNode publicIPAddressNode = new TreeNode(frontEnd.PublicIp.SourceName);
                                            publicIPAddressNode.Name = frontEnd.PublicIp.SourceName;
                                            publicIPAddressNode.Tag = frontEnd.PublicIp;
                                            cloudServiceNode.Nodes.Add(publicIPAddressNode);
                                            cloudServiceNode.Expand();
                                        }
                                    }
                                }
                            }
                        }

                        subscriptionNodeASM.Expand();
                        treeAzureASM.Enabled = true;
                    }
                }
            }
            catch (Exception exc)
            {
                if (exc.GetType() == typeof(System.Net.WebException))
                {
                    System.Net.WebException webException = (System.Net.WebException)exc;
                    if (webException.Response != null)
                    {
                        HttpWebResponse exceptionResponse = (HttpWebResponse)webException.Response;
                        if (exceptionResponse.StatusCode == HttpStatusCode.Forbidden)
                        {
                            ASM403ForbiddenExceptionDialog forbiddenDialog = new ASM403ForbiddenExceptionDialog(_AzureContextSource.LogProvider, exc);
                            return;
                        }
                    }
                }

                UnhandledExceptionDialog exceptionDialog = new UnhandledExceptionDialog(_AzureContextSource.LogProvider, exc);
                exceptionDialog.ShowDialog();
            }

            _AzureContextSource.StatusProvider.UpdateStatus("Ready");
        }

        private async Task BindArmResources(TargetSettings targetSettings)
        {
            treeAzureARM.Nodes.Clear();

            try
            {
                if (_AzureContextSource != null && _AzureContextSource.AzureSubscription != null)
                {
                    await _AzureContextSource.AzureSubscription.BindArmResources(targetSettings);

                    if (_AzureContextSource != null && _AzureContextSource.AzureSubscription != null)
                    {
                        TreeNode subscriptionNodeARM = new TreeNode(_AzureContextSource.AzureSubscription.Name);
                        subscriptionNodeARM.ImageKey = "Subscription";
                        subscriptionNodeARM.SelectedImageKey = "Subscription";
                        treeAzureARM.Nodes.Add(subscriptionNodeARM);
                        subscriptionNodeARM.Expand();

                        foreach (MigrationTarget.NetworkSecurityGroup targetNetworkSecurityGroup in _AzureContextSource.AzureSubscription.ArmTargetNetworkSecurityGroups)
                        {
                            TreeNode networkSecurityGroupParentNode = GetResourceGroupTreeNode(subscriptionNodeARM, ((Azure.Arm.NetworkSecurityGroup)targetNetworkSecurityGroup.SourceNetworkSecurityGroup).ResourceGroup);

                            TreeNode tnNetworkSecurityGroup = new TreeNode(targetNetworkSecurityGroup.SourceName);
                            tnNetworkSecurityGroup.Name = targetNetworkSecurityGroup.SourceName;
                            tnNetworkSecurityGroup.Tag = targetNetworkSecurityGroup;
                            tnNetworkSecurityGroup.ImageKey = targetNetworkSecurityGroup.ImageKey;
                            tnNetworkSecurityGroup.SelectedImageKey = targetNetworkSecurityGroup.ImageKey;
                            networkSecurityGroupParentNode.Nodes.Add(tnNetworkSecurityGroup);
                        }

                        foreach (MigrationTarget.PublicIp targetPublicIP in _AzureContextSource.AzureSubscription.ArmTargetPublicIPs)
                        {
                            TreeNode publicIpParentNode = GetResourceGroupTreeNode(subscriptionNodeARM, ((Azure.Arm.PublicIP)targetPublicIP.Source).ResourceGroup); ;

                            TreeNode tnPublicIP = new TreeNode(targetPublicIP.SourceName);
                            tnPublicIP.Name = targetPublicIP.SourceName;
                            tnPublicIP.Tag = targetPublicIP;
                            tnPublicIP.ImageKey = targetPublicIP.ImageKey;
                            tnPublicIP.SelectedImageKey = targetPublicIP.ImageKey;
                            publicIpParentNode.Nodes.Add(tnPublicIP);
                        }

                        foreach (MigrationTarget.RouteTable targetRouteTable in _AzureContextSource.AzureSubscription.ArmTargetRouteTables)
                        {
                            TreeNode routeTableParentNode = GetResourceGroupTreeNode(subscriptionNodeARM, ((Azure.Arm.RouteTable)targetRouteTable.Source).ResourceGroup);

                            TreeNode tnRouteTable = new TreeNode(targetRouteTable.SourceName);
                            tnRouteTable.Name = targetRouteTable.SourceName;
                            tnRouteTable.Tag = targetRouteTable;
                            tnRouteTable.ImageKey = targetRouteTable.ImageKey;
                            tnRouteTable.SelectedImageKey = targetRouteTable.ImageKey;
                            routeTableParentNode.Nodes.Add(tnRouteTable);
                        }

                        foreach (MigrationTarget.VirtualNetwork targetVirtualNetwork in _AzureContextSource.AzureSubscription.ArmTargetVirtualNetworks)
                        {
                            TreeNode virtualNetworkParentNode = GetResourceGroupTreeNode(subscriptionNodeARM, ((Azure.Arm.VirtualNetwork)targetVirtualNetwork.SourceVirtualNetwork).ResourceGroup);

                            TreeNode tnVirtualNetwork = new TreeNode(targetVirtualNetwork.SourceName);
                            tnVirtualNetwork.Name = targetVirtualNetwork.SourceName;
                            tnVirtualNetwork.Tag = targetVirtualNetwork;
                            tnVirtualNetwork.ImageKey = targetVirtualNetwork.ImageKey;
                            tnVirtualNetwork.SelectedImageKey = targetVirtualNetwork.ImageKey;
                            virtualNetworkParentNode.Nodes.Add(tnVirtualNetwork);
                        }

                        foreach (MigrationTarget.StorageAccount targetStorageAccount in _AzureContextSource.AzureSubscription.ArmTargetStorageAccounts)
                        {
                            TreeNode storageAccountParentNode = GetResourceGroupTreeNode(subscriptionNodeARM, ((Azure.Arm.StorageAccount)targetStorageAccount.SourceAccount).ResourceGroup);

                            TreeNode tnStorageAccount = new TreeNode(targetStorageAccount.SourceName);
                            tnStorageAccount.Name = targetStorageAccount.SourceName;
                            tnStorageAccount.Tag = targetStorageAccount;
                            tnStorageAccount.ImageKey = targetStorageAccount.ImageKey;
                            tnStorageAccount.SelectedImageKey = targetStorageAccount.ImageKey;
                            storageAccountParentNode.Nodes.Add(tnStorageAccount);
                        }

                        foreach (MigrationTarget.Disk targetManagedDisk in _AzureContextSource.AzureSubscription.ArmTargetManagedDisks)
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

                        foreach (MigrationTarget.AvailabilitySet targetAvailabilitySet in _AzureContextSource.AzureSubscription.ArmTargetAvailabilitySets)
                        {
                            TreeNode availabilitySetParentNode = GetResourceGroupTreeNode(subscriptionNodeARM, ((Azure.Arm.AvailabilitySet)targetAvailabilitySet.SourceAvailabilitySet).ResourceGroup);

                            TreeNode tnAvailabilitySet = new TreeNode(targetAvailabilitySet.SourceName);
                            tnAvailabilitySet.Name = targetAvailabilitySet.SourceName;
                            tnAvailabilitySet.Tag = targetAvailabilitySet;
                            tnAvailabilitySet.ImageKey = targetAvailabilitySet.ImageKey;
                            tnAvailabilitySet.SelectedImageKey = targetAvailabilitySet.ImageKey;
                            availabilitySetParentNode.Nodes.Add(tnAvailabilitySet);
                        }

                        foreach (MigrationTarget.NetworkInterface targetNetworkInterface in _AzureContextSource.AzureSubscription.ArmTargetNetworkInterfaces)
                        {
                            TreeNode tnResourceGroup = GetResourceGroupTreeNode(subscriptionNodeARM, ((Azure.Arm.NetworkInterface)targetNetworkInterface.SourceNetworkInterface).ResourceGroup);

                            TreeNode txNetworkInterface = new TreeNode(targetNetworkInterface.SourceName);
                            txNetworkInterface.Name = targetNetworkInterface.SourceName;
                            txNetworkInterface.Tag = targetNetworkInterface;
                            txNetworkInterface.ImageKey = targetNetworkInterface.ImageKey;
                            txNetworkInterface.SelectedImageKey = targetNetworkInterface.ImageKey;
                            tnResourceGroup.Nodes.Add(txNetworkInterface);
                        }

                        foreach (MigrationTarget.VirtualMachine targetVirtualMachine in _AzureContextSource.AzureSubscription.ArmTargetVirtualMachines)
                        {
                            TreeNode tnResourceGroup = GetResourceGroupTreeNode(subscriptionNodeARM, ((Azure.Arm.VirtualMachine)targetVirtualMachine.Source).ResourceGroup);

                            TreeNode tnVirtualMachine = new TreeNode(targetVirtualMachine.SourceName);
                            tnVirtualMachine.Name = targetVirtualMachine.SourceName;
                            tnVirtualMachine.Tag = targetVirtualMachine;
                            tnVirtualMachine.ImageKey = targetVirtualMachine.ImageKey;
                            tnVirtualMachine.SelectedImageKey = targetVirtualMachine.ImageKey;
                            tnResourceGroup.Nodes.Add(tnVirtualMachine);
                        }

                        foreach (MigrationTarget.LoadBalancer targetLoadBalancer in _AzureContextSource.AzureSubscription.ArmTargetLoadBalancers)
                        {
                            TreeNode networkSecurityGroupParentNode = GetResourceGroupTreeNode(subscriptionNodeARM, ((Azure.Arm.LoadBalancer)targetLoadBalancer.Source).ResourceGroup);

                            TreeNode tnNetworkSecurityGroup = new TreeNode(targetLoadBalancer.SourceName);
                            tnNetworkSecurityGroup.Name = targetLoadBalancer.SourceName;
                            tnNetworkSecurityGroup.Tag = targetLoadBalancer;
                            tnNetworkSecurityGroup.ImageKey = targetLoadBalancer.ImageKey;
                            tnNetworkSecurityGroup.SelectedImageKey = targetLoadBalancer.ImageKey;
                            networkSecurityGroupParentNode.Nodes.Add(tnNetworkSecurityGroup);
                        }

                        //foreach (MigrationTarget.VirtualMachineImage targetVirtualMachineImage in _AzureContextSource.AzureSubscription.ArmTargetVirtualMachineImages)
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
                        treeAzureARM.Sort();
                        treeAzureARM.Enabled = true;
                    }
                }
            }
            catch (Exception exc)
            {
                UnhandledExceptionDialog exceptionDialog = new UnhandledExceptionDialog(_AzureContextSource.LogProvider, exc);
                exceptionDialog.ShowDialog();
            }

            _AzureContextSource.StatusProvider.UpdateStatus("Ready");
        }

        #endregion

        #region Source Resource TreeView Methods

        private async Task SelectDependencies(TreeNode selectedNode)
        {
            if (this.AutoSelectDependencies && (selectedNode.Checked) && (selectedNode.Tag != null))
            {
                if (selectedNode.Tag.GetType() == typeof(Azure.MigrationTarget.AvailabilitySet))
                {
                    Azure.MigrationTarget.AvailabilitySet targetAvailabilitySet = (Azure.MigrationTarget.AvailabilitySet)selectedNode.Tag;

                    foreach (Azure.MigrationTarget.VirtualMachine targetVirtualMachine in targetAvailabilitySet.TargetVirtualMachines)
                    {
                        foreach (TreeNode treeNode in selectedNode.TreeView.Nodes.Find(targetVirtualMachine.TargetName, true))
                        {
                            if ((treeNode.Tag != null) && (treeNode.Tag.GetType() == typeof(Azure.MigrationTarget.VirtualMachine)))
                            {
                                if (!treeNode.Checked)
                                    treeNode.Checked = true;
                            }
                        }
                    }
                }
                if (selectedNode.Tag.GetType() == typeof(Azure.MigrationTarget.VirtualMachine))
                {
                    Azure.MigrationTarget.VirtualMachine targetVirtualMachine = (Azure.MigrationTarget.VirtualMachine)selectedNode.Tag;

                    if (targetVirtualMachine.Source != null)
                    {
                        if (targetVirtualMachine.Source.GetType() == typeof(Azure.Asm.VirtualMachine))
                        {
                            Azure.Asm.VirtualMachine asmVirtualMachine = (Azure.Asm.VirtualMachine)targetVirtualMachine.Source;

                            #region process virtual network

                            foreach (Azure.MigrationTarget.NetworkInterface networkInterface in targetVirtualMachine.NetworkInterfaces)
                            {
                                #region Auto Select Virtual Network from each IpConfiguration

                                foreach (Azure.MigrationTarget.NetworkInterfaceIpConfiguration ipConfiguration in networkInterface.TargetNetworkInterfaceIpConfigurations)
                                {
                                    if (ipConfiguration.TargetVirtualNetwork != null && ipConfiguration.TargetVirtualNetwork.GetType() == typeof(Azure.MigrationTarget.VirtualNetwork))
                                    {
                                        Azure.MigrationTarget.VirtualNetwork targetVirtualNetwork = (Azure.MigrationTarget.VirtualNetwork)ipConfiguration.TargetVirtualNetwork;
                                        foreach (TreeNode treeNode in selectedNode.TreeView.Nodes.Find(targetVirtualNetwork.SourceName, true))
                                        {
                                            if ((treeNode.Tag != null) && (treeNode.Tag.GetType() == typeof(Azure.MigrationTarget.VirtualNetwork)))
                                            {
                                                if (!treeNode.Checked)
                                                    treeNode.Checked = true;
                                            }
                                        }
                                    }
                                }

                                #endregion

                                #region Auto Select Network Security Group

                                if (asmVirtualMachine.NetworkSecurityGroup != null)
                                {
                                    foreach (TreeNode treeNode in selectedNode.TreeView.Nodes.Find(asmVirtualMachine.NetworkSecurityGroup.Name, true))
                                    {
                                        if ((treeNode.Tag != null) && (treeNode.Tag.GetType() == typeof(Azure.MigrationTarget.NetworkSecurityGroup)))
                                        {
                                            if (!treeNode.Checked)
                                                treeNode.Checked = true;
                                        }
                                    }
                                }
                                #endregion
                            }

                            #endregion

                            #region OS Disk Storage Account

                            if (this.DefaultTargetDiskType == ArmDiskType.ClassicDisk)
                            {
                                foreach (TreeNode treeNode in selectedNode.TreeView.Nodes.Find(asmVirtualMachine.OSVirtualHardDisk.StorageAccountName, true))
                                {
                                    if ((treeNode.Tag != null) && (treeNode.Tag.GetType() == typeof(Azure.MigrationTarget.StorageAccount)))
                                    {
                                        if (!treeNode.Checked)
                                            treeNode.Checked = true;
                                    }
                                }
                            }

                            #endregion

                            #region Data Disk(s) Storage Account(s)

                            if (this.DefaultTargetDiskType == ArmDiskType.ClassicDisk)
                            {

                                foreach (Azure.Asm.Disk dataDisk in asmVirtualMachine.DataDisks)
                                {
                                    foreach (TreeNode treeNode in selectedNode.TreeView.Nodes.Find(dataDisk.StorageAccountName, true))
                                    {
                                        if ((treeNode.Tag != null) && (treeNode.Tag.GetType() == typeof(Azure.MigrationTarget.StorageAccount)))
                                        {
                                            if (!treeNode.Checked)
                                                treeNode.Checked = true;
                                        }
                                    }
                                }
                            }

                            #endregion
                        }

                        else if (targetVirtualMachine.Source.GetType() == typeof(Azure.Arm.VirtualMachine))
                        {
                            Azure.Arm.VirtualMachine armVirtualMachine = (Azure.Arm.VirtualMachine)targetVirtualMachine.Source;

                            #region process virtual network

                            foreach (Azure.Arm.NetworkInterface networkInterface in armVirtualMachine.NetworkInterfaces)
                            {
                                foreach (Azure.Arm.NetworkInterfaceIpConfiguration ipConfiguration in networkInterface.NetworkInterfaceIpConfigurations)
                                {
                                    if (ipConfiguration.VirtualNetwork != null)
                                    {
                                        foreach (TreeNode treeNode in selectedNode.TreeView.Nodes.Find(ipConfiguration.VirtualNetwork.Name, true))
                                        {
                                            if ((treeNode.Tag != null) && (treeNode.Tag.GetType() == typeof(Azure.MigrationTarget.VirtualNetwork)))
                                            {
                                                if (!treeNode.Checked)
                                                    treeNode.Checked = true;
                                            }
                                        }
                                    }
                                }
                            }

                            #endregion

                            #region process virtual network

                            foreach (Azure.Arm.NetworkInterface networkInterface in armVirtualMachine.NetworkInterfaces)
                            {
                                foreach (Azure.Arm.NetworkInterfaceIpConfiguration ipConfiguration in networkInterface.NetworkInterfaceIpConfigurations)
                                {
                                    if (ipConfiguration.VirtualNetwork != null)
                                    {
                                        foreach (TreeNode treeNode in selectedNode.TreeView.Nodes.Find(ipConfiguration.VirtualNetwork.Name, true))
                                        {
                                            if ((treeNode.Tag != null) && (treeNode.Tag.GetType() == typeof(Azure.MigrationTarget.VirtualNetwork)))
                                            {
                                                if (!treeNode.Checked)
                                                    treeNode.Checked = true;
                                            }
                                        }
                                    }
                                }
                            }

                            #endregion

                            #region process managed disks

                            if (armVirtualMachine.OSVirtualHardDisk.GetType() == typeof(Azure.Arm.ManagedDisk))
                            {
                                foreach (TreeNode treeNode in selectedNode.TreeView.Nodes.Find(((Azure.Arm.ManagedDisk)armVirtualMachine.OSVirtualHardDisk).Name, true))
                                {
                                    if ((treeNode.Tag != null) && (treeNode.Tag.GetType() == typeof(Azure.MigrationTarget.Disk)))
                                    {
                                        if (!treeNode.Checked)
                                            treeNode.Checked = true;
                                    }
                                }
                            }

                            foreach (Azure.Interface.IArmDisk dataDisk in armVirtualMachine.DataDisks)
                            {
                                if (dataDisk.GetType() == typeof(Azure.Arm.ManagedDisk))
                                {
                                    Azure.Arm.ManagedDisk managedDisk = (Azure.Arm.ManagedDisk)dataDisk;

                                    foreach (TreeNode treeNode in selectedNode.TreeView.Nodes.Find(managedDisk.Name, true))
                                    {
                                        if ((treeNode.Tag != null) && (treeNode.Tag.GetType() == typeof(Azure.MigrationTarget.Disk)))
                                        {
                                            if (!treeNode.Checked)
                                                treeNode.Checked = true;
                                        }
                                    }
                                }
                            }

                            #endregion

                            #region OS Disk Storage Account

                            if (armVirtualMachine.OSVirtualHardDisk.GetType() == typeof(Azure.Arm.ClassicDisk)) // Disk in a Storage Account, not a Managed Disk
                            {
                                foreach (TreeNode treeNode in selectedNode.TreeView.Nodes.Find(((Azure.Arm.ClassicDisk)armVirtualMachine.OSVirtualHardDisk).StorageAccountName, true))
                                {
                                    if ((treeNode.Tag != null) && (treeNode.Tag.GetType() == typeof(Azure.MigrationTarget.StorageAccount)))
                                    {
                                        if (!treeNode.Checked)
                                            treeNode.Checked = true;
                                    }
                                }
                            }

                            #endregion

                            #region Data Disk(s) Storage Account(s)

                            foreach (IArmDisk dataDisk in armVirtualMachine.DataDisks)
                            {
                                if (dataDisk.GetType() == typeof(Azure.Arm.ClassicDisk))
                                {
                                    Azure.Arm.ClassicDisk classicDisk = (Azure.Arm.ClassicDisk)dataDisk;
                                    foreach (TreeNode treeNode in selectedNode.TreeView.Nodes.Find(classicDisk.StorageAccountName, true))
                                    {
                                        if ((treeNode.Tag != null) && (treeNode.Tag.GetType() == typeof(Azure.MigrationTarget.StorageAccount)))
                                        {
                                            if (!treeNode.Checked)
                                                treeNode.Checked = true;
                                        }
                                    }
                                }
                                else if (dataDisk.GetType() == typeof(Azure.Arm.ManagedDisk))
                                {
                                    Azure.Arm.ManagedDisk managedDisk = (Azure.Arm.ManagedDisk)dataDisk;
                                    foreach (TreeNode treeNode in selectedNode.TreeView.Nodes.Find(managedDisk.Name, true))
                                    {
                                        if ((treeNode.Tag != null) && (treeNode.Tag.GetType() == typeof(Azure.MigrationTarget.Disk)))
                                        {
                                            if (!treeNode.Checked)
                                                treeNode.Checked = true;
                                        }
                                    }
                                }
                            }

                            #endregion

                            #region Network Interface Card(s)

                            foreach (Azure.Arm.NetworkInterface networkInterface in armVirtualMachine.NetworkInterfaces)
                            {
                                foreach (TreeNode treeNode in selectedNode.TreeView.Nodes.Find(networkInterface.Name, true))
                                {
                                    if ((treeNode.Tag != null) && (treeNode.Tag.GetType() == typeof(Azure.MigrationTarget.NetworkInterface)))
                                    {
                                        if (!treeNode.Checked)
                                            treeNode.Checked = true;
                                    }
                                }

                                if (networkInterface.NetworkSecurityGroup != null)
                                {
                                    foreach (TreeNode treeNode in selectedNode.TreeView.Nodes.Find(networkInterface.NetworkSecurityGroup.Name, true))
                                    {
                                        if ((treeNode.Tag != null) && (treeNode.Tag.GetType() == typeof(Azure.MigrationTarget.NetworkSecurityGroup)))
                                        {
                                            if (!treeNode.Checked)
                                                treeNode.Checked = true;
                                        }
                                    }
                                }

                                foreach (Azure.Arm.NetworkInterfaceIpConfiguration ipConfiguration in networkInterface.NetworkInterfaceIpConfigurations)
                                {
                                    if (ipConfiguration.BackEndAddressPool != null && ipConfiguration.BackEndAddressPool.LoadBalancer != null)
                                    {
                                        foreach (TreeNode treeNode in selectedNode.TreeView.Nodes.Find(ipConfiguration.BackEndAddressPool.LoadBalancer.Name, true))
                                        {
                                            if ((treeNode.Tag != null) && (treeNode.Tag.GetType() == typeof(Azure.MigrationTarget.LoadBalancer)))
                                            {
                                                if (!treeNode.Checked)
                                                    treeNode.Checked = true;
                                            }
                                        }
                                    }

                                    if (ipConfiguration.PublicIP != null)
                                    {
                                        foreach (TreeNode treeNode in selectedNode.TreeView.Nodes.Find(ipConfiguration.PublicIP.Name, true))
                                        {
                                            if ((treeNode.Tag != null) && (treeNode.Tag.GetType() == typeof(Azure.MigrationTarget.PublicIp)))
                                            {
                                                if (!treeNode.Checked)
                                                    treeNode.Checked = true;
                                            }
                                        }
                                    }
                                }
                            }

                            #endregion
                        }
                    }
                }
                else if (selectedNode.Tag.GetType() == typeof(Azure.MigrationTarget.VirtualNetwork))
                {
                    Azure.MigrationTarget.VirtualNetwork targetVirtualNetwork = (Azure.MigrationTarget.VirtualNetwork)selectedNode.Tag;

                    foreach (Azure.MigrationTarget.Subnet targetSubnet in targetVirtualNetwork.TargetSubnets)
                    {
                        if (targetSubnet.NetworkSecurityGroup != null)
                        {
                            if (targetSubnet.NetworkSecurityGroup.SourceNetworkSecurityGroup != null)
                            {
                                if (targetSubnet.NetworkSecurityGroup.SourceNetworkSecurityGroup.GetType() == typeof(Azure.Asm.NetworkSecurityGroup))
                                {
                                    foreach (TreeNode treeNode in selectedNode.TreeView.Nodes.Find(targetSubnet.NetworkSecurityGroup.SourceName, true))
                                    {
                                        if ((treeNode.Tag != null) && (treeNode.Tag.GetType() == typeof(Azure.MigrationTarget.NetworkSecurityGroup)))
                                        {
                                            if (!treeNode.Checked)
                                                treeNode.Checked = true;
                                        }
                                    }
                                }
                                else if (targetSubnet.NetworkSecurityGroup.SourceNetworkSecurityGroup.GetType() == typeof(Azure.Arm.NetworkSecurityGroup))
                                {
                                    foreach (TreeNode treeNode in selectedNode.TreeView.Nodes.Find(targetSubnet.NetworkSecurityGroup.SourceName, true))
                                    {
                                        if ((treeNode.Tag != null) && (treeNode.Tag.GetType() == typeof(Azure.MigrationTarget.NetworkSecurityGroup)))
                                        {
                                            if (!treeNode.Checked)
                                                treeNode.Checked = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else if (selectedNode.Tag.GetType() == typeof(Azure.MigrationTarget.LoadBalancer))
                {
                    Azure.MigrationTarget.LoadBalancer targetLoadBalancer = (Azure.MigrationTarget.LoadBalancer)selectedNode.Tag;

                    if (targetLoadBalancer.Source != null)
                    {
                        if (targetLoadBalancer.Source.GetType() == typeof(Azure.Arm.LoadBalancer))
                        {
                            Azure.Arm.LoadBalancer armLoadBalaner = (Azure.Arm.LoadBalancer)targetLoadBalancer.Source;

                            foreach (Azure.Arm.FrontEndIpConfiguration frontEndIpConfiguration in armLoadBalaner.FrontEndIpConfigurations)
                            {
                                if (frontEndIpConfiguration.PublicIP != null)
                                {
                                    foreach (TreeNode treeNode in selectedNode.TreeView.Nodes.Find(frontEndIpConfiguration.PublicIP.Name, true))
                                    {
                                        if ((treeNode.Tag != null) && (treeNode.Tag.GetType() == typeof(Azure.MigrationTarget.PublicIp)))
                                        {
                                            if (!treeNode.Checked)
                                                treeNode.Checked = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                _AzureContextSource.StatusProvider.UpdateStatus("Ready");
            }
        }

        private async void treeAzureResourcesSource_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (_SourceAsmNode == null)
            {
                _SourceAsmNode = e.Node;
            }

            if (e.Node.Checked)
                await SelectDependencies(e.Node);

            TreeNode resultUpdateARMTree = null;

            Core.MigrationTarget migrationTarget = null;
            if (e.Node.Tag != null && e.Node.Tag.GetType().BaseType == typeof(Core.MigrationTarget))
            {
                migrationTarget = (Core.MigrationTarget)e.Node.Tag;

                if (e.Node.Checked)
                {
                    resultUpdateARMTree = e.Node;
                    AfterNodeChecked?.Invoke(migrationTarget);
                }
                else
                {
                    AfterNodeUnchecked?.Invoke(migrationTarget);
                }
            }

            if (_SourceAsmNode != null && _SourceAsmNode == e.Node)
            {
                if (e.Node.Checked)
                {
                    await RecursiveCheckToggleDown(e.Node, e.Node.Checked);
                    FillUpIfFullDown(e.Node);
                    treeAzureARM.SelectedNode = e.Node;
                }
                else
                {
                    await RecursiveCheckToggleUp(e.Node, e.Node.Checked);
                    await RecursiveCheckToggleDown(e.Node, e.Node.Checked);
                }

                _SelectedNodes = this.GetSelectedNodes(treeAzureARM);

                _SourceAsmNode = null;

                AfterNodeChanged?.Invoke(migrationTarget);
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
                (parentNode.Tag.GetType() == typeof(Azure.MigrationTarget.NetworkSecurityGroup) ||
                parentNode.Tag.GetType() == typeof(Azure.MigrationTarget.VirtualNetwork) ||
                parentNode.Tag.GetType() == typeof(Azure.MigrationTarget.StorageAccount) ||
                parentNode.Tag.GetType() == typeof(Azure.MigrationTarget.VirtualMachine)
                ))
                selectedNodes.Add(parentNode);

            foreach (TreeNode childNode in parentNode.Nodes)
            {
                RecursiveNodeSelectedAdd(ref selectedNodes, childNode);
            }
        }

        #endregion

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
            return tnResourceGroup;
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


        private void cmbAzureResourceTypeSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            treeAzureASM.Nodes.Clear();
            treeAzureARM.Nodes.Clear();

            switch (cmbAzureResourceTypeSource.SelectedItem.ToString())
            {
                case "Azure Service Management (ASM / Classic)":
                    treeAzureASM.Enabled = true;
                    treeAzureASM.Visible = true;
                    treeAzureARM.Enabled = false;
                    treeAzureARM.Visible = false;

                    BindAsmResources(_TargetSettings);

                    break;
                case "Azure Resource Manager (ARM)":
                    treeAzureASM.Enabled = false;
                    treeAzureASM.Visible = false;
                    treeAzureARM.Enabled = true;
                    treeAzureARM.Visible = true;

                    BindArmResources(_TargetSettings);

                    break;
                default:
                    throw new ArgumentException("Unknown Azure Resource Type Source: " + cmbAzureResourceTypeSource.SelectedItem.ToString());

            }

            ClearContext?.Invoke();
        }

        private void MigrationSourceAzure_Resize(object sender, EventArgs e)
        {
            treeAzureARM.Height = this.Height - 135;
            treeAzureARM.Width = this.Width;
            treeAzureASM.Height = this.Height - 135;
            treeAzureASM.Width = this.Width;
            azureLoginContextViewerSource.Width = this.Width;
        }

        public async Task UncheckMigrationTarget(Core.MigrationTarget migrationTarget)
        {
            LogProvider.WriteLog("UncheckMigrationTarget", "Start");

            TreeNode sourceMigrationNode = null;
            
            if (treeAzureARM.Enabled)
            {
                LogProvider.WriteLog("UncheckMigrationTarget", "Seeking Originating MigrationTarget TreeNode in treeAzureARM");
                sourceMigrationNode = TargetTreeView.SeekMigrationTargetTreeNode(treeAzureARM.Nodes, migrationTarget);
            }
            else if (treeAzureASM.Enabled)
            {
                LogProvider.WriteLog("UncheckMigrationTarget", "Seeking Originating MigrationTarget TreeNode in treeAzureASM");
                sourceMigrationNode = TargetTreeView.SeekMigrationTargetTreeNode(treeAzureASM.Nodes, migrationTarget);
            }

            if (sourceMigrationNode == null)
            {
                LogProvider.WriteLog("UncheckMigrationTarget", "Originating MigrationTarget TreeNode not found.");
            }
            else
            {
                LogProvider.WriteLog("UncheckMigrationTarget", "Seeking Originating MigrationTarget TreeNode");
                sourceMigrationNode.Checked = false;
            }

            LogProvider.WriteLog("UncheckMigrationTarget", "End");
        }
    }
}
