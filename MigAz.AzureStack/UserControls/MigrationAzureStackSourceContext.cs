using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MigAz.Core.Interface;

namespace MigAz.AzureStack.UserControls
{
    public partial class MigrationAzureStackSourceContext : UserControl, IMigrationSourceUserControl
    {
        private bool _IsAuthenticated = false;

        public MigrationAzureStackSourceContext()
        {
            InitializeComponent();
        }


        public bool IsSourceContextAuthenticated
        {
            get { return _IsAuthenticated; }
            set { _IsAuthenticated = value; }
        }
    }
}
