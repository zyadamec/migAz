using MigAz.Azure.Asm;
using MigAz.Azure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MigAzASM.Interface
{
    public interface ISaveSelectionProvider
    {
        Task Save(Guid subscriptionId, List<TreeNode> treeView);
        Task Read(Guid subscriptionId, AzureRetriever sourceAzureRetreiver, AzureRetriever targetAzureRetreiver, TreeView treeView);
    }
}
