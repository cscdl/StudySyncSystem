using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudySyncSystem
{
    public partial class frmAdmin : Form
    {
        private string loggedInUsername;
        private int loggedInUserID;

        bool SideBarExpand;

        public frmAdmin(int userID)
        {
            InitializeComponent();
            InitializeUI("UIMode");
            loggedInUserID = userID;

        }

        public void loadform(object Form)
        {
            if (this.pnlMain.Controls.Count > 0)
                this.pnlMain.Controls.RemoveAt(0);

            Form mainAdmin = Form as Form;
            mainAdmin.TopLevel = false;
            mainAdmin.Dock = DockStyle.Fill;

            this.pnlMain.Controls.Add(mainAdmin);
            this.pnlMain.Tag = mainAdmin;

            mainAdmin.Show();
        }

        private void InitializeUI(string key)
        {
            var uiMode = ConfigurationManager.AppSettings[key];

            if (uiMode == "light")
            {
                btnSettings.Text = "Dark Mode";
                ApplyDarkModeColors();
                ConfigurationManager.AppSettings[key] = "dark";
            }
            else
            {
                btnSettings.Text = "Light Mode";
                ApplyLightModeColors();
                ConfigurationManager.AppSettings[key] = "light";
            }
        }

        private void ApplyDarkModeColors()
        {
            this.ForeColor = Color.FromArgb(0, 8, 20);
            this.BackColor = Color.FromArgb(115, 160, 195);

            
        }

        private void ApplyLightModeColors()
        {
            this.ForeColor = Color.FromArgb(255, 255, 255);
            this.BackColor = Color.FromArgb(0, 8, 20);

           
        }

        private void tmrSideBar_Tick(object sender, EventArgs e)
        {
            if (SideBarExpand)
            {
                pnlSideBar.Width -= 10;
                if (pnlSideBar.Width == pnlSideBar.MinimumSize.Width)
                {
                    SideBarExpand = false;
                    tmrSideBar.Stop();
                }
            }
            else
            {
                pnlSideBar.Width += 10;
                if (pnlSideBar.Width == pnlSideBar.MaximumSize.Width)
                {
                    SideBarExpand = true;
                    tmrSideBar.Stop();
                }
            }
        }

        private void picBoxDashboard_Click(object sender, EventArgs e)
        {
            tmrSideBar.Start();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            InitializeUI("UIMode");
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            loadform(new frmAdminDashboard());
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            loadform(new frmAbout());
        }

        private void btnAddAdminAccount_Click(object sender, EventArgs e)
        {
            loadform(new frmAddAdminAccount());
        }

        private void btnPicUser_Click(object sender, EventArgs e)
        {
            int adminID = GetLoggedInAdminID();

            MessageBox.Show($"AdminID to be passed: {adminID}");
            frmAdminProfile adminProfileForm = new frmAdminProfile(adminID);
            loadform(adminProfileForm);
        }

        private int GetLoggedInAdminID()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source=DSMARI;Initial Catalog=StudySyncDB;Integrated Security=True"))
                {
                    connection.Open();

                    string query = "SELECT UserID FROM tblUser WHERE Username = @Username AND UserType = 'admin'"; 
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Username", loggedInUsername);

                        object result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            return (int)result;
                        }
                        else
                        {
                            MessageBox.Show("Admin not found.");
                            return -1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving admin ID: " + ex.Message);
                return -1;
            }
        }


        private void btnLogout_Click(object sender, EventArgs e)
        {
            Hide();
            frmLoginAndRegister loginandregister = new frmLoginAndRegister();
            loginandregister.ShowDialog();
        }

        private void btnManageCategories_Click(object sender, EventArgs e)
        {
            bool isAdmin = true; 

            frmManageCategory manageCategoryForm = new frmManageCategory(isAdmin);
            loadform(manageCategoryForm);
        }

        public void UpdateUsernameLabel(string username)
        {
            loggedInUsername = username;
            adminName.Text = loggedInUsername;
        }
    }
}
