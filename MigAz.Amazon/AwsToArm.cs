using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using MigAz.Core.Interface;
using MigAz.AWS.Forms;
using MigAz.AWS.Providers;
using MigAz.Core;
using MigAz.Azure;
using Amazon.EC2.Model;
using MigAz.Azure.UserControls;

namespace MigAz.AWS
{
    public partial class AwsToArm : IMigratorUserControl
    {
        DescribeInstancesResponse instResponse;
        DescribeVpcsResponse vpcResponse;
        DescribeVolumesResponse ebsVolumesResponse;
        private AzureContext _AzureContextARM;
        private AwsToArmSaveSelectionProvider _saveSelectionProvider;
        private AwsToArmSaveSelectionProvider _telemetryProvider;
        //private AwsAppSettingsProvider _appSettingsProvider;
        private PropertyPanel _PropertyPanel;
        private AWS.Generator.AwsToArmGenerator _TemplateGenerator;
        private AwsObjectRetriever _awsObjectRetriever;

        public AwsToArm() : base(null, null) { }

        public AwsToArm(IStatusProvider statusProvider, ILogProvider logProvider, PropertyPanel propertyPanel) 
            : base (statusProvider, logProvider)
        {
            InitializeComponent();

            _PropertyPanel = propertyPanel;
            _saveSelectionProvider = new AwsToArmSaveSelectionProvider();
            _telemetryProvider = new AwsToArmSaveSelectionProvider();
            //_appSettingsProvider = new AwsAppSettingsProvider();
        }












        //private EC2Operation ec2 = null;
        // TODO WHERE?? static IAmazonEC2 service;
        string accessKeyID;
        string secretKeyID;
        string selectedregion;
        // TODO List<Amazon.RegionEndpoint> Regions;

        //private string subscriptionid;
        //private Dictionary<string, string> subscriptionsAndTenants;

        public async Task Bind()
        {
            _AzureContextARM = new AzureContext(LogProvider, StatusProvider, null); // todo needs settings provider
            await azureLoginContextViewer21.Bind(_AzureContextARM);


            //var tokenProvider = new InteractiveTokenProvider();
            //
        }

        private void AwsToArm_Load(object sender, EventArgs e)
        {
            if (LogProvider != null)
                LogProvider.WriteLog("Window_Load", "Program start");
            // TODO instResponse = new DescribeInstancesResponse();
            // this.Text = "migAz AWS (" + Assembly.GetEntryAssembly().GetName().Version.ToString() + ")";

            List<Amazon.RegionEndpoint> regionsList = new List<Amazon.RegionEndpoint>();
            foreach (var region in Amazon.RegionEndpoint.EnumerableAllRegions)
            {
                regionsList.Add(region);
            }

            cmbRegion.DataSource = regionsList;



            AlertIfNewVersionAvailable();
        }

        #region New Version Check

        private async Task AlertIfNewVersionAvailable()
        {
            string currentVersion = "2.2.2.0";
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
                //Authenticate
                authenticate();

                cmbRegion.Enabled = true;

                //Load Items
                Load_Items();

                lblSignInText.Text = $"Signed in as {accessKeyID}";



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        private void Load_Items()
        {
            LogProvider.WriteLog("Load_Items", "Start");

            lvwVirtualNetworks.Items.Clear();
            lvwVirtualMachines.Items.Clear();

            _awsObjectRetriever = new AwsObjectRetriever(accessKeyID, secretKeyID, (Amazon.RegionEndpoint)cmbRegion.SelectedValue, this.LogProvider, this.StatusProvider);
            // todo, not needed in this method_templateGenerator = new TemplateGenerator(_logProvider, _statusProvider, _awsObjectRetriever, telemetryProvider);


            instResponse = getEC2Instances();
            Application.DoEvents();

            StatusProvider.UpdateStatus("BUSY: Getting the VPC details");
            vpcResponse = getVPCs();
            Application.DoEvents();
            //List Instances


            if (instResponse != null)
            {
                StatusProvider.UpdateStatus("BUSY: Processing Instances");
                if (instResponse.Reservations.Count > 0)
                {
                    foreach (var instanceResp in instResponse.Reservations)
                    {
                        foreach (var instance in instanceResp.Instances)
                        {
                            ListViewItem listItem = new ListViewItem(instance.InstanceId);
                            string name = "";
                            foreach (var tag in instance.Tags)
                            {
                                if (tag.Key == "Name")
                                {
                                    name = tag.Value;
                                }
                            }

                            listItem.SubItems.AddRange(new[] { name });
                            lvwVirtualMachines.Items.Add(listItem);
                            Application.DoEvents();
                        }
                    }
                }

                //List VPCs
                StatusProvider.UpdateStatus("BUSY: Processing VPC");
                
                foreach (var vpc in vpcResponse.Vpcs)
                {

                    ListViewItem listItem = new ListViewItem(vpc.VpcId);
                    string VpcName = "";
                    foreach (var tag in vpc.Tags)
                    {
                        if (tag.Key == "Name")
                        {
                            VpcName = tag.Value;
                        }
                    }

                    listItem.SubItems.AddRange(new[] { VpcName });
                    lvwVirtualNetworks.Items.Add(listItem);
                    Application.DoEvents();
                }
            }

            StatusProvider.UpdateStatus("Ready");
        }


        private DescribeVolumesResponse getEbsVolumes()
        {
            return _awsObjectRetriever.Volumes;
        }

        private DescribeVpcsResponse getVPCs()
        {
            return _awsObjectRetriever.Vpcs;
        }

        private DescribeInstancesResponse getEC2Instances()
        {
            return _awsObjectRetriever.Instances;
        }

        private void authenticate()
        {
            AuthenticationForm authForm = new AuthenticationForm();
            authForm.ShowDialog();

            accessKeyID = authForm.GetAWSAccessKeyID();
            secretKeyID = authForm.GetAWSSecretKeyID();
        }

        private void lvwVirtualMachines_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            //if (app.Default.AutoSelectDependencies)
            //{
            //    if (e.Item.Checked)
            //    {
            //        AutoSelectDependencies(e);
            //    }
            //}
        }

  
        private void btnExport_Click(object sender, EventArgs e)
        {
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

            // If save selection option is enabled
            //if (app.Default.SaveSelection)
            //{
            //    StatusProvider.UpdateStatus("BUSY: Reading saved selection");
            //    _saveSelectionProvider.Save(cmbRegion.Text, lvwVirtualNetworks, lvwVirtualMachines);
            //}

            //var templateWriter = new StreamWriter(Path.Combine(txtDestinationFolder.Text, "export.json"));
            //var blobDetailWriter = new StreamWriter(Path.Combine(txtDestinationFolder.Text, "copyblobdetails.json"));

            //_templateGenerator.GenerateTemplate(artefacts, _awsObjectRetriever, templateWriter, teleinfo);

            MessageBox.Show("Template has been generated successfully.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void AutoSelectDependencies(ItemCheckedEventArgs listViewRow)
        {
            // TODO
            string InstanceId = listViewRow.Item.ListView.Items[listViewRow.Item.Index].SubItems[0].Text;

            var availableInstances = _awsObjectRetriever.Instances;
            //var selectedVolumes;
            if (InstanceId != null)
            {

                //var selectedInstances = availableInstances.Reservations[0].Instances.Find(x => x.InstanceId == InstanceId);
                var selectedInstances = _awsObjectRetriever.getInstancebyId(InstanceId);

                foreach (ListViewItem virtualNetwork in lvwVirtualNetworks.Items)
                {
                    if (selectedInstances.Instances[0].VpcId == virtualNetwork.SubItems[0].Text)
                    {
                        virtualNetwork.Checked = true;
                        virtualNetwork.Selected = true;
                    }

                }

            }
        }

        private void cmbRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbRegion.Enabled == true)
            {
                //Load the Region Items
                Load_Items();
            }

            // If save selection option is enabled
            //if (app.Default.SaveSelection)
            //{
            //    StatusProvider.UpdateStatus("BUSY: Reading saved selection");
            //    _saveSelectionProvider.Read(cmbRegion.Text, ref lvwVirtualNetworks, ref lvwVirtualMachines);
            //}

        }
    }
}
