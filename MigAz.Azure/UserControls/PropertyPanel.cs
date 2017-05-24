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

namespace MigAz.Azure.UserControls
{
    public partial class PropertyPanel : UserControl
    {
        public PropertyPanel()
        {
            InitializeComponent();
        }

        public ILogProvider LogProvider
        {
            get; set;
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

        public async void Bind(IMigrationTarget migrationTarget)
        {
            this.Clear();

            if (migrationTarget != null)
            {
                this.ResourceText = migrationTarget.ToString();

                if (migrationTarget.GetType() == typeof(Azure.MigrationTarget.VirtualMachine))
                {
                    this.ResourceImage = imageList1.Images["VirtualMachine"];

                    VirtualMachineProperties properties = new VirtualMachineProperties();
                    properties.LogProvider = LogProvider;
                    properties.AllowManangedDisk = false;
                    properties.PropertyChanged += Properties_PropertyChanged;
                    // todo now asap russell await properties.Bind(migrationTarget, this);
                    this.PropertyDetailControl = properties;
                }
                else if (migrationTarget.GetType() == typeof(Azure.MigrationTarget.NetworkSecurityGroup))
                {
                    this.ResourceImage = imageList1.Images["NetworkSecurityGroup"];

                    NetworkSecurityGroupProperties properties = new NetworkSecurityGroupProperties();
                    properties.PropertyChanged += Properties_PropertyChanged;
                    // todo now asap russell properties.Bind(migrationTarget);
                    this.PropertyDetailControl = properties;
                }
                if (migrationTarget.GetType() == typeof(Azure.MigrationTarget.VirtualNetwork))
                {
                    this.ResourceImage = imageList1.Images["VirtualNetwork"];

                    VirtualNetworkProperties properties = new VirtualNetworkProperties();
                    properties.PropertyChanged += Properties_PropertyChanged;
                    // todo now asap russell properties.Bind(e.Node);
                    this.PropertyDetailControl = properties;
                }
                else if (migrationTarget.GetType() == typeof(Azure.MigrationTarget.Subnet))
                {
                    this.ResourceImage = imageList1.Images["VirtualNetwork"];

                    SubnetProperties properties = new SubnetProperties();
                    properties.PropertyChanged += Properties_PropertyChanged;
                    // todo now asap russell properties.Bind(e.Node);
                    this.PropertyDetailControl = properties;
                }
                else if (migrationTarget.GetType() == typeof(Azure.MigrationTarget.StorageAccount))
                {
                    this.ResourceImage = imageList1.Images["StorageAccount"];

                    StorageAccountProperties properties = new StorageAccountProperties();
                    properties.PropertyChanged += Properties_PropertyChanged;
                    // todo now asap russell properties.Bind(this._AzureContextTargetARM, migrationTarget);
                    this.PropertyDetailControl = properties;
                }
                else if (migrationTarget.GetType() == typeof(Azure.MigrationTarget.Disk))
                {
                    Azure.MigrationTarget.Disk migrationDisk = (Azure.MigrationTarget.Disk)migrationTarget;

                    this.ResourceImage = imageList1.Images["Disk"];

                    DiskProperties properties = new DiskProperties();
                    properties.LogProvider = this.LogProvider;
                    properties.AllowManangedDisk = false;
                    properties.PropertyChanged += Properties_PropertyChanged;
                    // todo now asap russell properties.Bind(this, e.Node);
                    this.PropertyDetailControl = properties;
                }
                else if (migrationTarget.GetType() == typeof(Azure.MigrationTarget.AvailabilitySet))
                {
                    this.ResourceImage = imageList1.Images["AvailabilitySet"];

                    AvailabilitySetProperties properties = new AvailabilitySetProperties();
                    properties.PropertyChanged += Properties_PropertyChanged;
                    // todo now asap russell properties.Bind(e.Node);
                    this.PropertyDetailControl = properties;
                }
                else if (migrationTarget.GetType() == typeof(Azure.MigrationTarget.NetworkInterface))
                {
                    this.ResourceImage = imageList1.Images["NetworkInterface"];

                    NetworkInterfaceProperties properties = new NetworkInterfaceProperties();
                    properties.PropertyChanged += Properties_PropertyChanged;
                    // todo now asap russell properties.Bind(this, e.Node);
                    this.PropertyDetailControl = properties;
                }
                else if (migrationTarget.GetType() == typeof(Azure.MigrationTarget.ResourceGroup))
                {
                    this.ResourceImage = imageList1.Images["ResourceGroup"];

                    ResourceGroupProperties properties = new ResourceGroupProperties();
                    properties.PropertyChanged += Properties_PropertyChanged;
                    // todo now asap russell await properties.Bind(_AzureContextARM, migrationTarget);
                    this.PropertyDetailControl = properties;
                }
                else if (migrationTarget.GetType() == typeof(Azure.MigrationTarget.LoadBalancer))
                {
                    this.ResourceImage = imageList1.Images["LoadBalancer"];

                    LoadBalancerProperties properties = new LoadBalancerProperties();
                    properties.PropertyChanged += Properties_PropertyChanged;
                    // todo now asap russell await properties.Bind(this, e.Node);
                    this.PropertyDetailControl = properties;
                }
                else if (migrationTarget.GetType() == typeof(Azure.MigrationTarget.PublicIp))
                {
                    this.ResourceImage = imageList1.Images["PublicIp"];

                    PublicIpProperties properties = new PublicIpProperties();
                    properties.PropertyChanged += Properties_PropertyChanged;
                    // todo now asap russell properties.Bind(e.Node);
                    this.PropertyDetailControl = properties;
                }
            }
        }

        private Task Properties_PropertyChanged()
        {
            throw new NotImplementedException();
        }
    }
}
