using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MigAz.Azure.UserControls;

namespace MigAz.MigrationTarget
{
    public partial class MigrationTargetAzure : UserControl
    {
        public delegate Task AfterTargetSelectedHandler(TreeNode sender);
        public event AfterTargetSelectedHandler AfterTargetSelected;


        public MigrationTargetAzure()
        {
            InitializeComponent();

            treeTargetARM.AfterTargetSelected += TreeTargetARM_AfterTargetSelected;
        }

        private void TreeTargetARM_AfterTargetSelected()
        {
            AfterTargetSelected?.Invoke(this.treeTargetARM.SelectedNode);
        }

        public void Bind(PropertyPanel propertyPanel)
        {
            this.treeTargetARM.PropertyPanel = propertyPanel;
        }

        public TargetTreeView TargetTreeView
        {
            get { return this.treeTargetARM; }
        }
    }
}
