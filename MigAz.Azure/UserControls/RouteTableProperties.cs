using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MigAz.Azure.MigrationTarget;

namespace MigAz.Azure.UserControls
{
    public partial class RouteTableProperties : UserControl
    {
        private TargetTreeView _TargetTreeView;
        private RouteTable _RouteTable;
        private bool _IsBinding = false;

        public delegate Task AfterPropertyChanged();
        public event AfterPropertyChanged PropertyChanged;

        public RouteTableProperties()
        {
            InitializeComponent();
        }

        internal void Bind(RouteTable targetRouteTable, TargetTreeView targetTreeView)
        {
            try
            {
                _IsBinding = true;
                _TargetTreeView = targetTreeView;
                _RouteTable = targetRouteTable;

                lblSourceName.Text = _RouteTable.SourceName;
                txtTargetName.Text = targetRouteTable.TargetName;
            }
            finally
            {
                _IsBinding = false;
            }
        }

        private void txtTargetName_TextChanged(object sender, EventArgs e)
        {
            TextBox txtSender = (TextBox)sender;

            _RouteTable.SetTargetName(txtSender.Text, _TargetTreeView.TargetSettings);

            if (!_IsBinding)
                PropertyChanged?.Invoke();
        }

        private void txtTargetName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
