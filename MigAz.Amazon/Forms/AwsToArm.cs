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
using MigAz.Azure.Generator.AsmToArm;

namespace MigAz.AWS.Forms
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
        private AwsObjectRetriever _awsObjectRetriever;
        //private EC2Operation ec2 = null;
        // TODO WHERE?? static IAmazonEC2 service;
        private AzureContext _AzureContextTargetARM;
        private Azure.MigrationTarget.ResourceGroup _TargetResourceGroup;


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

            treeSource.Nodes.Clear();
            treeTargetARM.Nodes.Clear();

            TreeNode subscriptionNode = new TreeNode("AWS Subscription");
            treeSource.Nodes.Add(subscriptionNode);

            List<Amazon.RegionEndpoint> regionsList = new List<Amazon.RegionEndpoint>();
            foreach (var region in Amazon.RegionEndpoint.EnumerableAllRegions)
            {
                try
                {
                    _awsObjectRetriever = new AwsObjectRetriever(accessKeyTextBox.Text.Trim(), secretKeyTextBox.Text.Trim(), region, this.LogProvider, this.StatusProvider);
                    //// todo, not needed in this method_templateGenerator = new TemplateGenerator(_logProvider, _statusProvider, _awsObjectRetriever, telemetryProvider);


                    instResponse = getEC2Instances();
                    Application.DoEvents();

                    StatusProvider.UpdateStatus("BUSY: Getting the VPC details");
                    vpcResponse = getVPCs();
                    Application.DoEvents();

                    TreeNode amazonRegionNode = new TreeNode(region.DisplayName);
                    amazonRegionNode.Text = region.DisplayName;
                    amazonRegionNode.Tag = region;

                    if (instResponse != null)
                    {
                        StatusProvider.UpdateStatus("BUSY: Processing Instances");
                        if (instResponse.Reservations.Count > 0)
                        {
                            foreach (var instanceResp in instResponse.Reservations)
                            {
                                foreach (var instance in instanceResp.Instances)
                                {
                                    string name = "";
                                    foreach (var tag in instance.Tags)
                                    {
                                        if (tag.Key == "Name")
                                        {
                                            name = tag.Value;
                                        }
                                    }

                                    TreeNode instanceTreeNode = new TreeNode(instance.InstanceId + " - " + name);
                                    instanceTreeNode.Tag = instance;
                                    amazonRegionNode.Nodes.Add(instanceTreeNode);

                                    Application.DoEvents();
                                }
                            }
                        }
                    }

                    //List VPCs
                    StatusProvider.UpdateStatus("BUSY: Processing VPC");

                    foreach (var vpc in vpcResponse.Vpcs)
                    {
                        string VpcName = "";
                        foreach (var tag in vpc.Tags)
                        {
                            if (tag.Key == "Name")
                            {
                                VpcName = tag.Value;
                            }
                        }

                        TreeNode vpcTreeNode = new TreeNode(vpc.VpcId + " - " + VpcName);
                        vpcTreeNode.Tag = vpc;
                        amazonRegionNode.Nodes.Add(vpcTreeNode);

                        Application.DoEvents();
                    }

                    if (amazonRegionNode.Nodes.Count > 0)
                        subscriptionNode.Nodes.Add(amazonRegionNode);
                    else
                        this.LogProvider.WriteLog("Load_Items", "Not adding Amazon Region '" + region.DisplayName + "' to Source Node list, as it contains no resources to export.");
                }
                catch (Exception exc)
                {
                    this.LogProvider.WriteLog("Load_Items", "AWS Exception - " + region.DisplayName + ": " + exc.Message);
                }
            }

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

  
        //private void btnExport_Click(object sender, EventArgs e)
        //{
        //    Hashtable teleinfo = new Hashtable();
        //    //teleinfo.Add("Region", cmbRegion.Text);
        //    teleinfo.Add("AccessKey", accessKeyTextBox.Text.Trim());

        //    var artefacts = new AWSExportArtifacts();

        //    //foreach (var selectedItem in lvwVirtualNetworks.CheckedItems)
        //    //{
        //    //    var listItem = (ListViewItem)selectedItem;
        //    //    artefacts.VirtualNetworks.Add(new VPC(listItem.Text, listItem.SubItems[1].Text));
        //    //}

        //    //foreach (var selectedItem in lvwVirtualMachines.CheckedItems)
        //    //{
        //    //    var listItem = (ListViewItem)selectedItem;
        //    //    artefacts.Instances.Add(new Ec2Instance(listItem.Text, listItem.SubItems[1].Text));
        //    //}

        //    // If save selection option is enabled
        //    //if (app.Default.SaveSelection)
        //    //{
        //    //    StatusProvider.UpdateStatus("BUSY: Reading saved selection");
        //    //    _saveSelectionProvider.Save(cmbRegion.Text, lvwVirtualNetworks, lvwVirtualMachines);
        //    //}

        //    //var templateWriter = new StreamWriter(Path.Combine(txtDestinationFolder.Text, "export.json"));
        //    //var blobDetailWriter = new StreamWriter(Path.Combine(txtDestinationFolder.Text, "copyblobdetails.json"));

        //    //_templateGenerator.GenerateTemplate(artefacts, _awsObjectRetriever, templateWriter, teleinfo);

        //    MessageBox.Show("Template has been generated successfully.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        //}

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

                //foreach (ListViewItem virtualNetwork in lvwVirtualNetworks.Items)
                //{
                //    if (selectedInstances.Instances[0].VpcId == virtualNetwork.SubItems[0].Text)
                //    {
                //        virtualNetwork.Checked = true;
                //        virtualNetwork.Selected = true;
                //    }

                //}

            }
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
    }
}
