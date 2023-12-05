using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace StudySyncSystem
{
    public partial class frmLoginAndRegister : Form
    {
        SqlConnection connect = new SqlConnection(@"Data Source=DESKTOP-H473JT7\SQLEXPRESS01;Initial Catalog=StudySyncDB;Integrated Security=True");

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
            frmMainStudySync mss = new frmMainStudySync();
            mss.ShowDialog();
            this.Hide();

            //frmAdmin a = new frmAdmin();
            //a.ShowDialog();
            //this.Hide();

        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
           
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

        private void chckShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (chckShowPassword1.Checked == true)
            {
                txtPassword.UseSystemPasswordChar = false;
            }
            else
            {
                txtPassword.UseSystemPasswordChar = true;
            }
        }

    }       
}
