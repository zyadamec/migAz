// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

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
            set
            {
                if (value == null || value == String.Empty)
                {
                    txtIpAddress.Text = String.Empty;
                    return;
                }

                string newValue = value;
                if (newValue.Contains("/"))
                {
                    newValue = newValue.Substring(0, newValue.IndexOf("/"));
                }

                txtIpAddress.Text = newValue;
            }
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
            int decimalCharCount = txtIpAddress.Text.Count(f => f == '.');

            if (e.KeyChar == 8) // Backspace
            {
                return;
            }
            if (e.KeyChar == 46) // .
            {
                // IPv4 Address can only have 3 '.' characters.  If already 3, there can't be more
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
            else if (char.IsDigit(e.KeyChar))
            {
                if (e.KeyChar == 48) // 0
                {
                    if (txtIpAddress.SelectionStart == 0) // First character of IP Address cannot be a 0
                    {
                        e.Handled = true;
                        return;
                    }
                    else if (txtIpAddress.SelectionStart > 0)
                    {
                        // Check if the character that exists prior to the caret/cursor location is a '.', if so, we can't have a 0 to start the next number
                        if (txtIpAddress.Text.Substring(txtIpAddress.SelectionStart - 1, 1) == ".")
                        {
                            e.Handled = true;
                            return;
                        }
                    }
                }

                int charsToLeftUpToThreeMax = Math.Min(3, txtIpAddress.SelectionStart);

                // Start by getting up to 3 characters to the left of the caret/cursor location
                string previousChars = txtIpAddress.Text.Substring(txtIpAddress.SelectionStart - charsToLeftUpToThreeMax, charsToLeftUpToThreeMax);
                if (previousChars.LastIndexOf('.') >= 0)
                {
                    previousChars = previousChars.Substring(previousChars.LastIndexOf('.') + 1);
                }

                if (previousChars.Count() >= 3) // If there are already 3 numbers in this IP second, we can't add a 4th
                {
                    if (decimalCharCount >= 3)
                    {
                        e.Handled = true;
                        return;
                    }
                    else
                    {
                        txtIpAddress.Text += ".";
                        txtIpAddress.SelectionStart = txtIpAddress.TextLength;
                        return;
                    }
                }

                string strPreviousCharsWithNewChar = previousChars + e.KeyChar;
                int intNewIpSectionValue = 999;
                int.TryParse(strPreviousCharsWithNewChar, out intNewIpSectionValue);
                if (intNewIpSectionValue > 255)
                {
                    if (decimalCharCount >= 3)
                    {
                        e.Handled = true;
                        return;
                    }
                    else
                    {
                        txtIpAddress.Text += ".";
                        txtIpAddress.SelectionStart = txtIpAddress.TextLength;
                        return;
                    }
                }
            }
            else
            {
                e.Handled = true;
                return;
            }
        }
    }
}

