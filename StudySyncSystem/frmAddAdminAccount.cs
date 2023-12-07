using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudySyncSystem
{
    public partial class frmAddAdminAccount : Form
    {
        public frmAddAdminAccount()
        {
            InitializeComponent();
        }

        private void chckShowPassword1_CheckedChanged(object sender, EventArgs e)
        {
            if (chckShowPassword1.Checked == true)
            {
                txtRegPassword.UseSystemPasswordChar = false;
                txtRegConfirmPassword.UseSystemPasswordChar = false;
            }
            else
            {
                txtRegPassword.UseSystemPasswordChar = true;
                txtRegConfirmPassword.UseSystemPasswordChar = true;
            }
        }
    }
}
