using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace StudySyncSystem
{
    public partial class frmAdminProfile : Form
    {
        private int loggedInAdminID;

        public frmAdminProfile(int adminID)
        {
            InitializeComponent();
            this.loggedInAdminID = adminID;
            LoadAdminData(loggedInAdminID);
        }

        private void LoadAdminData(int adminId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source=DSMARI;Initial Catalog=StudySyncDB;Integrated Security=True"))
                {
                    connection.Open();

                    string query = "SELECT u.Username, ui.FirstName, ui.LastName, ui.Address, ui.PhoneNumber, u.UserType FROM tblUser u INNER JOIN tblUserInfo ui ON u.UserID = ui.UserID WHERE u.UserID = @UserID AND u.UserType = 'admin'";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserID", adminId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                lblUsername.Text = reader["Username"].ToString();
                                lblFullName.Text = $"{reader["FirstName"]} {reader["LastName"]}".Trim();
                                lblAddress.Text = reader["Address"].ToString();
                                lblMobileNum.Text = reader["PhoneNumber"].ToString();

                                string userType = reader["UserType"].ToString();

                            }

                            else
                            {
                                MessageBox.Show("Admin not found.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving admin data: " + ex.Message);
            }
        }

        private void btnSaveChanges_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtEditUsername.Text) || string.IsNullOrWhiteSpace(txtEditPassword.Text))
                {
                    MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (SqlConnection connection = new SqlConnection(@"Data Source=DSMARI;Initial Catalog=StudySyncDB;Integrated Security=True"))
                {
                    connection.Open();

                    string updateUsernameQuery = "UPDATE tblUser SET Username = @Username WHERE UserID = @UserID AND UserType = 'admin'";
                    using (SqlCommand updateUsernameCmd = new SqlCommand(updateUsernameQuery, connection))
                    {
                        updateUsernameCmd.Parameters.AddWithValue("@UserID", loggedInAdminID);
                        updateUsernameCmd.Parameters.AddWithValue("@Username", txtEditUsername.Text);
                        updateUsernameCmd.ExecuteNonQuery();
                    }

                    string updatePasswordQuery = "UPDATE tblUser SET Password = @Password WHERE UserID = @UserID AND UserType = 'admin'";
                    using (SqlCommand updatePasswordCmd = new SqlCommand(updatePasswordQuery, connection))
                    {
                        updatePasswordCmd.Parameters.AddWithValue("@UserID", loggedInAdminID);
                        updatePasswordCmd.Parameters.AddWithValue("@Password", txtEditPassword.Text); 
                        updatePasswordCmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Changes saved successfully!");

                    
                    LoadAdminData(loggedInAdminID);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving changes: " + ex.Message);
            }
        }

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            Close();
        }
    }
}
