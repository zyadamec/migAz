using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MigAz.Azure.UserControls
{
    public partial class IPv4AddressBox : UserControl
    {
        public delegate void AfterTextChanged(object sender);
        public new event AfterTextChanged TextChanged;

        public IPv4AddressBox()
        {
            InitializeComponent();
        }

        #region Properties

        public override string Text
        {
            get { return txtIpAddress.Text; }
            set { txtIpAddress.Text = value; }
        }

        public new bool Enabled
        {
            get { return txtIpAddress.Enabled; }
            set { txtIpAddress.Enabled = value; }
        }

        #endregion

        private void txtIpAddress_TextChanged(object sender, EventArgs e)
        {
            TextChanged?.Invoke(this);
        }

        private void txtIpAddress_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 46) // .
            {
                // IPv4 Address can only have 3 '.' characters.  If already 3, there can't be more
                int decimalCharCount = txtIpAddress.Text.Count(f => f == '.');
                if (decimalCharCount >= 3)
                {
                    e.Handled = true;
                    return;
                }

                // Determine if the character to the left or right is already a '.'
                // if so, there can't be another '.' together

                // Character to left, if exists
                if (txtIpAddress.SelectionStart > 0) // has to be greater than 0 to have a character to the left
                {
                    if (txtIpAddress.Text.Substring(txtIpAddress.SelectionStart - 1, 1) == ".")
                    {
                        e.Handled = true;
                        return;
                    }
                }

                if (txtIpAddress.SelectionStart < txtIpAddress.TextLength) // Has to be less to have a character available to the right
                {
                    if (txtIpAddress.Text.Substring(txtIpAddress.SelectionStart, 1) == ".")
                    {
                        e.Handled = true;
                        return;
                    }
                }
            }

            if (txtIpAddress.SelectionStart == 0) // First character of IP Address cannot be a 0
            {
                e.Handled = true;
                return;
            }
            else if (txtIpAddress.SelectionStart > 0)
            {
                // Check if the character that exists prior to the caret/cursor location is a '.', if so, we can't have a 0 to start the next number
                int i = 0;
            }



            else if (e.KeyChar == 48) // 0
            {

            }

            if (char.IsDigit(e.KeyChar))
                return;
            
            if (char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
