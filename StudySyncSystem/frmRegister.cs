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
    public partial class frmRegister : Form
    {
        public frmRegister()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string RegUsername, RegUserPassword, RegConfirmPassword;

            RegUsername = txtRegUsername.Text;
            RegUserPassword = txtRegPassword.Text;
            RegConfirmPassword = txtRegConfirmPassword.Text;

            if (RegUserPassword == RegConfirmPassword)
            {
                MessageBox.Show("Registration Successful!");
            }
            else
            {
                MessageBox.Show("Password does not match! Please try again.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void chckShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (chckShowPassword.Checked == true)
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

        private void btnLogin_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmLogin login = new frmLogin();
            login.Show();
        }

        private void txtRegUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtRegPassword.Focus();
            }
        }

        private void txtRegPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtRegConfirmPassword.Focus();
            }
        }
    }
}
