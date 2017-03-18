using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using MigAz.Core.Interface;
using MigAz.Azure;
using MigAz.AWS;
using MigAz.AWS.Forms;
using MigAz.AWS.Providers;

namespace MigAz.UserControls.Migrators
{
    public partial class AwsToArm : IMigratorUserControl
    {
        private AzureContext _AzureContextARM;
        //private EC2Operation ec2 = null;
        // TODO WHERE?? static IAmazonEC2 service;
        // TODO DescribeInstancesResponse instResponse;
        // TODO DescribeVpcsResponse vpcResponse;
        // TODO DescribeVolumesResponse ebsVolumesResponse;
        string accessKeyID;
        string secretKeyID;
        string selectedregion;
        // TODO List<Amazon.RegionEndpoint> Regions;

        //private string subscriptionid;
        //private Dictionary<string, string> subscriptionsAndTenants;
        private AwsRetriever _awsRetriever;
        private AWS.Generator.AwsToArmGenerator _TemplateGenerator;
        private AwsToArmSaveSelectionProvider _saveSelectionProvider;
        private AwsObjectRetriever _awsObjectRetriever;
        private dynamic telemetryProvider;
        private Forms.AWS.Providers.AppSettingsProvider _appSettingsProvider;

        public AwsToArm() : base(null,null) { }

        public AwsToArm(IStatusProvider statusProvider, ILogProvider logProvider)
            : base(statusProvider, logProvider)
        {
            InitializeComponent();
        }

        public async Task Bind()
        {
            _AzureContextARM = new AzureContext(LogProvider, StatusProvider, _appSettingsProvider);
            await azureLoginContextViewer21.Bind(_AzureContextARM);

            //telemetryProvider = new Forms.AWS.Provider.CloudTelemetryProvider();
            //saveSelectionProvider = new Forms.AWS.Provider.UISaveSelectionProvider();

            // TODO
            //Regions = new List<Amazon.RegionEndpoint>();
            //foreach (var region in Amazon.RegionEndpoint.EnumerableAllRegions)
            //{
            //    Regions.Add(region);
            //}



            //var tokenProvider = new InteractiveTokenProvider();
            //
        }

        private void AwsToArm_Load(object sender, EventArgs e)
        {
            if (LogProvider != null)
                LogProvider.WriteLog("Window_Load", "Program start");
            // TODO instResponse = new DescribeInstancesResponse();
           // this.Text = "migAz AWS (" + Assembly.GetEntryAssembly().GetName().Version.ToString() + ")";

            // TODO cmbRegion.DataSource = Regions;
            //NewVersionAvailable(); // check if there a new version of the app
        }

        //TODO CHECK
        private void Window_FormClosing(object sender, FormClosingEventArgs e)
        {
            // If save selection option is enabled
            if (app.Default.SaveSelection)
            {
                _saveSelectionProvider.Save(cmbRegion.Text, lvwVirtualNetworks, lvwVirtualMachines);
            }
        }

        private void btnGetToken_Click(object sender, EventArgs e)
        {
            LogProvider.WriteLog("GetToken_Click", "Start");

            try
            {
                //Authenticate
                authenticate();

                cmbRegion.Enabled = true;

                //Load Items
                // TODO Load_Items();

                lblSignInText.Text = $"Signed in as {accessKeyID}";



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }




        private void authenticate()
        {
            AuthenticationForm authForm = new AuthenticationForm();

            accessKeyID = authForm.GetAWSAccessKeyID();
            secretKeyID = authForm.GetAWSSecretKeyID();
        }



        private void lvwVirtualNetworks_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            UpdateExportItemsCount();
        }

        private void lvwStorageAccounts_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            UpdateExportItemsCount();
        }

        private void lvwVirtualMachines_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            UpdateExportItemsCount();

            if (app.Default.AutoSelectDependencies)
            {
                if (e.Item.Checked)
                {
                    AutoSelectDependencies(e);
                }
            }
        }

        private void UpdateExportItemsCount()
        {
            int numofobjects = lvwVirtualNetworks.CheckedItems.Count + lvwVirtualMachines.CheckedItems.Count;
            btnExport.Text = "Export " + numofobjects.ToString() + " objects";
        }

        private void txtDestinationFolder_TextChanged(object sender, EventArgs e)
        {
            if (txtDestinationFolder.Text == "")
                btnExport.Enabled = false;
            else
                btnExport.Enabled = true;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            btnExport.Enabled = false;

            Hashtable teleinfo = new Hashtable();
            teleinfo.Add("Region", cmbRegion.Text);
            teleinfo.Add("AccessKey", accessKeyID);

            var artefacts = new AWSExportArtifacts();

            foreach (var selectedItem in lvwVirtualNetworks.CheckedItems)
            {
                var listItem = (ListViewItem)selectedItem;
                artefacts.VirtualNetworks.Add(new VPC(listItem.Text, listItem.SubItems[1].Text));
            }

            foreach (var selectedItem in lvwVirtualMachines.CheckedItems)
            {
                var listItem = (ListViewItem)selectedItem;
                artefacts.Instances.Add(new Ec2Instance(listItem.Text, listItem.SubItems[1].Text));
            }


            // TODO Move

            //if (!Directory.Exists(txtDestinationFolder.Text))
            //{
            //    MessageBox.Show("The chosen output folder does not exist.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //}
            //else
            //{
            //    // If save selection option is enabled
            //    if (app.Default.SaveSelection)
            //    {
            //        StatusProvider.UpdateStatus("BUSY: Reading saved selection");
            //        _saveSelectionProvider.Save(cmbRegion.Text, lvwVirtualNetworks, lvwVirtualMachines);
            //    }

            //    var templateWriter = new StreamWriter(Path.Combine(txtDestinationFolder.Text, "export.json"));
            //    //var blobDetailWriter = new StreamWriter(Path.Combine(txtDestinationFolder.Text, "copyblobdetails.json"));

            //    _templateGenerator.GenerateTemplate(artefacts, _awsObjectRetriever, templateWriter, teleinfo);

            //    MessageBox.Show("Template has been generated successfully.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}

            btnExport.Enabled = true;
        }

        private void NewVersionAvailable()
        {
            //try
            //{
                //if (version != availableversion)
                //{
                    DialogResult dialogresult = MessageBox.Show("New version " + "x.x.x.x" + " is available at http://aka.ms/MIGAZ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    }
            //}
            //catch (Exception exception)
            //{
            //    DialogResult dialogresult = MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }

        private void AutoSelectDependencies(ItemCheckedEventArgs listViewRow)
        {
            // TODO
            //string InstanceId = listViewRow.Item.ListView.Items[listViewRow.Item.Index].SubItems[0].Text;

            //var availableInstances = _awsObjectRetriever.Instances;
            ////var selectedVolumes;
            //if (InstanceId != null)
            //{

            //    //var selectedInstances = availableInstances.Reservations[0].Instances.Find(x => x.InstanceId == InstanceId);
            //    var selectedInstances = _awsObjectRetriever.getInstancebyId(InstanceId);

            //    foreach (ListViewItem virtualNetwork in lvwVirtualNetworks.Items)
            //    {
            //            if (selectedInstances.Instances[0].VpcId == virtualNetwork.SubItems[0].Text)
            //            {
            //                virtualNetwork.Checked = true;
            //                virtualNetwork.Selected = true;
            //            }

            //    }

            //}
        }

        private void cmbRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbRegion.Enabled == true)
            {
                //Load the Region Items
                // TODO Load_Items();
            }

            // If save selection option is enabled
            if (app.Default.SaveSelection)
            {
                StatusProvider.UpdateStatus("BUSY: Reading saved selection");
                _saveSelectionProvider.Read(cmbRegion.Text, ref lvwVirtualNetworks, ref lvwVirtualMachines);
            }

        }



        private void lvwVirtualMachines_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

    }
}
