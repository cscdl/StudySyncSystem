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
    public partial class frmLoginAndRegister : Form
    {
        public frmLoginAndRegister()
        {
            InitializeComponent();
        }

        private void btnGOTORegister_Click(object sender, EventArgs e)
        {
            pnlRegister.BringToFront();
        }

        private void btnGOTOLogin_Click(object sender, EventArgs e)
        {
            pnlLogin.BringToFront();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username, userPassword;

            string exampleUsername = "Admin";
            string examplePassword = "Admin123";

            username = txtUsername.Text;
            userPassword = txtPassword.Text;

            if (username == exampleUsername && userPassword == examplePassword)
            {
                MessageBox.Show("Login Successful.");

                //frmMainStudySync studySync = new frmMainStudySync();
                //studySync.Show();
                //this.Hide();

                frmAdmin admin = new frmAdmin();
                admin.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid username or password. Please try again.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void chckShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (chckShowPassword.Checked == true)
            {
                txtPassword.UseSystemPasswordChar = false;
            }
            else
            {
                txtPassword.UseSystemPasswordChar = true;
            }
        }

        private void txtUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtPassword.Focus();
            }
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
