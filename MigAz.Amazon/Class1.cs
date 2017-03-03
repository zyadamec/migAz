using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.AWS
{
    class Class1
    {

        //private void Load_Items()
        //{
        //    writeLog("GetToken_Click", "Start");

        //    lvwVirtualNetworks.Items.Clear();
        //    lvwVirtualMachines.Items.Clear();

        //    createEC2Client();


        //    instResponse = getEC2Instances();
        //    Application.DoEvents();

        //    //lblStatus.Text = "BUSY: Getting the VPC details";
        //    vpcResponse = getVPCs();
        //    Application.DoEvents();
        //    //List Instances


        //    if (instResponse != null)
        //    {
        //        lblStatus.Text = "BUSY: Processing Instances";
        //        if (instResponse.Reservations.Count > 0)
        //        {
        //            foreach (var instanceResp in instResponse.Reservations)
        //            {
        //                foreach (var instance in instanceResp.Instances)
        //                {
        //                    ListViewItem listItem = new ListViewItem(instance.InstanceId);
        //                    string name = "";
        //                    foreach (var tag in instance.Tags)
        //                    {
        //                        if (tag.Key == "Name")
        //                        {
        //                            name = tag.Value;
        //                        }
        //                    }

        //                    listItem.SubItems.AddRange(new[] { name });
        //                    lvwVirtualMachines.Items.Add(listItem);
        //                    Application.DoEvents();
        //                }
        //            }
        //        }
        //        //List VPCs
        //        lblStatus.Text = "BUSY: Processing VPC";
        //        foreach (var vpc in vpcResponse.Vpcs)
        //        {

        //            ListViewItem listItem = new ListViewItem(vpc.VpcId);
        //            string VpcName = "";
        //            foreach (var tag in vpc.Tags)
        //            {
        //                if (tag.Key == "Name")
        //                {
        //                    VpcName = tag.Value;
        //                }
        //            }

        //            listItem.SubItems.AddRange(new[] { VpcName });
        //            lvwVirtualNetworks.Items.Add(listItem);
        //            Application.DoEvents();
        //        }
        //    }
        //    btnChoosePath.Enabled = true;
        //    lblStatus.Text = "Ready";

        //}
        //private void createEC2Client()
        //{

        //    _awsObjectRetriever = new AwsObjectRetriever(accessKeyID, secretKeyID, (Amazon.RegionEndpoint)cmbRegion.SelectedValue, _logProvider, _statusProvider);
        //    _templateGenerator = new TemplateGenerator(_logProvider, _statusProvider, _awsObjectRetriever, telemetryProvider);
        //}
        //private DescribeVolumesResponse getEbsVolumes()
        //{
        //    return _awsObjectRetriever.Volumes;
        //}

        //public string GetRegion()
        //{
        //    return selectedregion;
        //}

        //private DescribeVpcsResponse getVPCs()
        //{
        //    return _awsObjectRetriever.Vpcs;
        //}

        //private DescribeInstancesResponse getEC2Instances()
        //{
        //    return _awsObjectRetriever.Instances;
        //}

    }
}
