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

namespace MigAz.Azure.UserControls
{
    public partial class PropertyPanel : UserControl
    {
        private TargetTreeView _TargetTreeView;
        private Core.MigrationTarget _MigrationTarget;

        public delegate Task AfterPropertyChanged(Core.MigrationTarget migrationTarget);
        public event AfterPropertyChanged PropertyChanged;

        public PropertyPanel()
        {
            InitializeComponent();
        }

        public IStatusProvider StatusProvider
        {
            get; set;
        }

        public TargetTreeView TargetTreeView
        {
            get { return _TargetTreeView; }
            private set { _TargetTreeView = value; }
        }

        public Core.MigrationTarget MigrationTarget
        {
            get { return _MigrationTarget; }
            private set { _MigrationTarget = value; }
        }

        public Image ResourceImage
        {
            get { return pictureBox1.Image; }
            private set { pictureBox1.Image = value; }
        }

        public string ResourceText
        {
            get { return lblAzureObjectName.Text; }
            private set { lblAzureObjectName.Text = value; }
        }

        public Control PropertyDetailControl
        {
            get
            {
                if (this.pnlProperties.Controls.Count == 0)
                    return null;
                else
                    return this.pnlProperties.Controls[0];
            }
            private set
            {
                this.pnlProperties.Controls.Clear();

                if (value != null)
                {
                    this.pnlProperties.Controls.Add(value);
                }
            }
        }

        public void Clear()
        {
            this._MigrationTarget = null;
            this.ResourceImage = null;
            this.ResourceText = String.Empty;
            this.pnlProperties.Controls.Clear();
        }

        private void PropertyPanel_Resize(object sender, EventArgs e)
        {
            groupBox1.Width = this.Width - 5;
            groupBox1.Height = this.Height - 5;
        }

        private void groupBox1_Resize(object sender, EventArgs e)
        {
            pnlProperties.Width = groupBox1.Width - 15;
            pnlProperties.Height = groupBox1.Height - 90;
        }

        public bool IsBound
        {
            get { return _TargetTreeView != null && _MigrationTarget != null; }
        }

        public async Task Bind(ResourceGroup resourceGroup, List<Arm.Location> locations)
        {
            this.BindCommon(resourceGroup);

            ResourceGroupProperties properties = new ResourceGroupProperties();
            properties.PropertyChanged += Properties_PropertyChanged;
            await properties.Bind(resourceGroup, this.TargetTreeView, locations);
            this.PropertyDetailControl = properties;
        }

        public async Task Bind(NetworkInterface networkInterface, List<Arm.VirtualNetwork> existingVirtualNetworksInTargetLocation)
        {
            this.BindCommon(networkInterface);

            NetworkInterfaceProperties properties = new NetworkInterfaceProperties();
            properties.PropertyChanged += Properties_PropertyChanged;
            await properties.Bind(_TargetTreeView, networkInterface, existingVirtualNetworksInTargetLocation);
            this.PropertyDetailControl = properties;
        }
        public async Task Bind(LoadBalancer loadBalancer, List<Arm.VirtualNetwork> existingVirtualNetworksInTargetLocation)
        {
            this.BindCommon(loadBalancer);

            LoadBalancerProperties properties = new LoadBalancerProperties();
            properties.PropertyChanged += Properties_PropertyChanged;
            await properties.Bind(loadBalancer, _TargetTreeView, existingVirtualNetworksInTargetLocation);
            this.PropertyDetailControl = properties;
        }

        public async Task Bind(Disk disk, List<Arm.StorageAccount> existingStorageAccountsInTargetLocation)
        {
            this.BindCommon(disk);

            DiskProperties properties = new DiskProperties();
            properties.PropertyChanged += Properties_PropertyChanged;
            await properties.Bind(disk, _TargetTreeView, existingStorageAccountsInTargetLocation);
            this.PropertyDetailControl = properties;
        }

        public void BindCommon(Core.MigrationTarget migrationTarget)
        {
            this.Clear();
            this.MigrationTarget = migrationTarget;
            this.ResourceText = migrationTarget.ToString();
            this.ResourceImage = imageList1.Images[migrationTarget.ImageKey];
        }

        public async Task Bind(TargetTreeView targetTreeView, Core.MigrationTarget migrationTarget)
        {
            if (targetTreeView == null)
                throw new ArgumentException("TargetTreeView Property must be provided.");

            if (migrationTarget == null)
                throw new ArgumentException("MigrationTarget Property must be provided.");

            targetTreeView.LogProvider.WriteLog("PropertyPanel Bind", "Start");

            this.Clear();
            this._TargetTreeView = targetTreeView;
            this._MigrationTarget = migrationTarget;

            this.ResourceText = migrationTarget.ToString();
            this.ResourceImage = imageList1.Images[migrationTarget.ImageKey];


            if (migrationTarget.GetType() == typeof(VirtualMachine))
            {
                VirtualMachineProperties properties = new VirtualMachineProperties();
                properties.PropertyChanged += Properties_PropertyChanged;
                await properties.Bind(_TargetTreeView, (VirtualMachine)migrationTarget);
                this.PropertyDetailControl = properties;
            }
            else if (migrationTarget.GetType() == typeof(NetworkSecurityGroup))
            {
                NetworkSecurityGroupProperties properties = new NetworkSecurityGroupProperties();
                properties.PropertyChanged += Properties_PropertyChanged;
                properties.Bind((NetworkSecurityGroup)migrationTarget, _TargetTreeView);
                this.PropertyDetailControl = properties;
            }
            if (migrationTarget.GetType() == typeof(VirtualNetwork))
            {
                VirtualNetworkProperties properties = new VirtualNetworkProperties();
                properties.PropertyChanged += Properties_PropertyChanged;
                properties.Bind((VirtualNetwork)migrationTarget, _TargetTreeView);
                this.PropertyDetailControl = properties;
            }
            else if (migrationTarget.GetType() == typeof(Subnet))
            {
                SubnetProperties properties = new SubnetProperties();
                properties.PropertyChanged += Properties_PropertyChanged;
                properties.Bind(_TargetTreeView, (Subnet)migrationTarget);
                this.PropertyDetailControl = properties;
            }
            else if (migrationTarget.GetType() == typeof(StorageAccount))
            {
                StorageAccountProperties properties = new StorageAccountProperties();
                properties.PropertyChanged += Properties_PropertyChanged;
                properties.Bind((StorageAccount)migrationTarget, _TargetTreeView);
                this.PropertyDetailControl = properties;
            }
            else if (migrationTarget.GetType() == typeof(AvailabilitySet))
            {
                AvailabilitySetProperties properties = new AvailabilitySetProperties();
                properties.PropertyChanged += Properties_PropertyChanged;
                properties.Bind(_TargetTreeView, (AvailabilitySet)migrationTarget);
                this.PropertyDetailControl = properties;
            }
            else if (migrationTarget.GetType() == typeof(PublicIp))
            {
                PublicIpProperties properties = new PublicIpProperties();
                properties.PropertyChanged += Properties_PropertyChanged;
                properties.Bind((PublicIp)migrationTarget, _TargetTreeView);
                this.PropertyDetailControl = properties;
            }
            else if (migrationTarget.GetType() == typeof(RouteTable))
            {
                RouteTableProperties properties = new RouteTableProperties();
                properties.PropertyChanged += Properties_PropertyChanged;
                properties.Bind(_TargetTreeView, (RouteTable)migrationTarget);
                this.PropertyDetailControl = properties;
            }
            //else if (migrationTarget.GetType() == typeof(VirtualMachineImage))
            //{
            //    VirtualMachineImageProperties properties = new VirtualMachineImageProperties();
            //    properties.PropertyChanged += Properties_PropertyChanged;
            //    properties.Bind(_TargetTreeView, (VirtualMachineImage)migrationTarget, _TargetTreeView);
            //    this.PropertyDetailControl = properties;
            //}

            targetTreeView.LogProvider.WriteLog("PropertyPanel Bind", "End");
        }

        private async Task Properties_PropertyChanged()
        {
            await PropertyChanged(_MigrationTarget);
        }
    }
}
