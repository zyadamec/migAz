using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MigAz.Core
{
    public class MigAzTreeView
    {

        public static TreeNode GetDataCenterTreeViewNode(TreeNode subscriptionNode, string dataCenter, string containerName)
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

            if (containerName == "Virtual Networks")
            {
                TreeNode tnVirtualNetworks = new TreeNode("Virtual Networks");
                dataCenterNode.Nodes.Add(tnVirtualNetworks);
                tnVirtualNetworks.Expand();

                return tnVirtualNetworks;
            }
            else if (containerName == "Storage Accounts")
            {
                TreeNode tnStorageAccounts = new TreeNode("Storage Accounts");
                dataCenterNode.Nodes.Add(tnStorageAccounts);
                tnStorageAccounts.Expand();

                return tnStorageAccounts;
            }
            else if (containerName == "Cloud Services")
            {
                TreeNode tnCloudServicesNode = new TreeNode("Cloud Services");
                dataCenterNode.Nodes.Add(tnCloudServicesNode);
                tnCloudServicesNode.Expand();

                return tnCloudServicesNode;
            }
            else if (containerName == "Virtual Machines")
            {
                TreeNode tnVirtualMachinesNode = new TreeNode("Virtual Machines");
                dataCenterNode.Nodes.Add(tnVirtualMachinesNode);
                tnVirtualMachinesNode.Expand();

                return tnVirtualMachinesNode;
            }
            else if (containerName == "Network Security Groups")
            {
                TreeNode tnNetworkSecurityGroupsNode = new TreeNode("Network Security Groups");
                dataCenterNode.Nodes.Add(tnNetworkSecurityGroupsNode);
                tnNetworkSecurityGroupsNode.Expand();

                return tnNetworkSecurityGroupsNode;
            }
            else
            {
                throw new ArgumentException("Unknown Tree Node Container Name: " + containerName);
            }

            return null;
        }

        public static void FillUpIfFullDown(TreeNode node)
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

        public static bool AllChildrenChecked(TreeNode node)
        {
            foreach (TreeNode childNode in node.Nodes)
                if (!childNode.Checked)
                    return false;

            return true;
        }

        public static bool IsSelectedFullDown(TreeNode node)
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

        public async static Task RecursiveCheckToggleDown(TreeNode node, bool isChecked)
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
        public async static Task RecursiveCheckToggleUp(TreeNode node, bool isChecked)
        {
            if (node.Checked != isChecked)
            {
                node.Checked = isChecked;
            }

            if (node.Parent != null)
                await RecursiveCheckToggleUp(node.Parent, isChecked);
        }

    }
}
