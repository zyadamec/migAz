using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.EC2.Model;
using System.Windows.Forms;
using MigAz.Core.Interface;
using Amazon.ElasticLoadBalancing.Model;
using Amazon.ElasticLoadBalancing;

namespace MigAz.AWS.Generator
{
    public class AWSRetriever
    {
        private AwsObjectRetriever _awsObjectRetriever;
        private IAmazonElasticLoadBalancing _LBservice;
        private ILogProvider _LogProvider;
        private IStatusProvider _StatusProvider;

        private AWSRetriever() { }

        public AWSRetriever(ILogProvider logProvider, IStatusProvider statusProvider)
        {
            _LogProvider = logProvider;
            _StatusProvider = statusProvider;
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

        public DescribeLoadBalancersResponse GetAllLBs()
        {
            _StatusProvider.UpdateStatus("BUSY: Getting Loadbalancer details...");
            //  var x = _LBservice.DescribeLoadBalancers();
            return _LBservice.DescribeLoadBalancers();
        }

        private IAmazonElasticLoadBalancing createLBClient(string accessKeyID, string secretKeyID, Amazon.RegionEndpoint region)
        {
            try
            {
                _LBservice = new AmazonElasticLoadBalancingClient(accessKeyID, secretKeyID, new AmazonElasticLoadBalancingConfig()
                {
                    RegionEndpoint = region
                });
                return _LBservice;
            }
            catch (AmazonElasticLoadBalancingException ex)
            {
                throw ex;
            }
        }



        public void toname(string accessKey, string secretKey, TreeNode subscriptionNode)
        {
            List<Amazon.RegionEndpoint> regionsList = new List<Amazon.RegionEndpoint>();
            foreach (var region in Amazon.RegionEndpoint.EnumerableAllRegions)
            {
                try
                {
                    _awsObjectRetriever = new AwsObjectRetriever(accessKey, secretKey, region, _LogProvider, _StatusProvider);
                    //// todo, not needed in this method_templateGenerator = new TemplateGenerator(_logProvider, _statusProvider, _awsObjectRetriever, telemetryProvider);

                    TreeNode amazonRegionNode = new TreeNode(region.DisplayName);
                    amazonRegionNode.Text = region.DisplayName;
                    amazonRegionNode.Tag = region;

                    //DescribeVolumesResponse ebsVolumesResponse;

                    _StatusProvider.UpdateStatus("BUSY: Getting the VPC details");
                    DescribeVpcsResponse vpcResponse = getVPCs();
                    Application.DoEvents();

                    _StatusProvider.UpdateStatus("BUSY: Processing VPC");
                    foreach (var vpc in vpcResponse.Vpcs)
                    {
                        MigAz.AWS.MigrationSource.VirtualNetwork sourceVirtualNetwork = new MigrationSource.VirtualNetwork(_awsObjectRetriever, vpc);
                        Azure.MigrationTarget.VirtualNetwork targetVirtualNetwork = new Azure.MigrationTarget.VirtualNetwork(sourceVirtualNetwork);

                        TreeNode vpcTreeNode = new TreeNode(sourceVirtualNetwork.Id + " - " + sourceVirtualNetwork.Name);
                        vpcTreeNode.Tag = targetVirtualNetwork;
                        amazonRegionNode.Nodes.Add(vpcTreeNode);

                        Application.DoEvents();
                    }

                    DescribeInstancesResponse instResponse = getEC2Instances();
                    Application.DoEvents();

                    if (instResponse != null)
                    {
                        _StatusProvider.UpdateStatus("BUSY: Processing Instances");
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




                    if (amazonRegionNode.Nodes.Count > 0)
                        subscriptionNode.Nodes.Add(amazonRegionNode);
                    else
                        _LogProvider.WriteLog("Load_Items", "Not adding Amazon Region '" + region.DisplayName + "' to Source Node list, as it contains no resources to export.");
                }
                catch (Exception exc)
                {
                    _LogProvider.WriteLog("Load_Items", "AWS Exception - " + region.DisplayName + ": " + exc.Message);
                }
            }
        }
    }
}
