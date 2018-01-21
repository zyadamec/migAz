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
using MigAz.Azure;

namespace MigAz.MigrationTarget
{
    public partial class MigrationTargetAzure : UserControl
    {
        public delegate Task AfterTargetSelectedHandler(TreeNode sender);
        public event AfterTargetSelectedHandler AfterTargetSelected;

        public delegate Task AfterResourceValidationHandler();
        public event AfterResourceValidationHandler AfterResourceValidation;

        public MigrationTargetAzure()
        {
            InitializeComponent();

            treeTargetARM.AfterTargetSelected += TreeTargetARM_AfterTargetSelected;
            treeTargetARM.AfterResourceValidation += TreeTargetARM_AfterResourceValidation;
        }

        private async Task TreeTargetARM_AfterResourceValidation()
        {
            await AfterResourceValidation?.Invoke();
        }

        public ImageList ImageList
        {
            get { return treeTargetARM.ImageList; }
            set { treeTargetARM.ImageList = value; }
        }

        private void TreeTargetARM_AfterTargetSelected()
        {
            AfterTargetSelected?.Invoke(this.TargetTreeView.SelectedNode);
        }

        public void Bind(PropertyPanel propertyPanel)
        {
            this.treeTargetARM.PropertyPanel = propertyPanel;
        }

        public void Clear()
        {
            this.treeTargetARM.Clear();
        }

        public TargetTreeView TargetTreeView
        {
            get { return this.treeTargetARM; }
        }

        private void MigrationTargetAzure_Resize(object sender, EventArgs e)
        {
            this.treeTargetARM.Width = this.Width;
            this.treeTargetARM.Height = this.Height - 125;
            azureLoginContextViewerARM.Width = this.Width;
        }
    }
}
