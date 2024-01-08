using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace StudySyncSystem
{
    public partial class frmAdminProfile : Form
    {
        private int adminID;

        public frmAdminProfile(int adminID)
        {
            InitializeComponent();
            this.adminID = adminID;
            LoadAdminData(adminID);
        }

        private void LoadAdminData(int adminId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source=DSMARI;Initial Catalog=StudySyncDB;Integrated Security=True"))
                {
                    connection.Open();

                    string query = "SELECT u.Username, ui.FirstName, ui.LastName, ui.Address, ui.PhoneNumber FROM tblUser u INNER JOIN tblUserInfo ui ON u.UserID = ui.UserID WHERE u.UserID = @AdminID AND u.UserType = 'admin'";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@AdminID", adminId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                lblUsername.Text = reader["Username"].ToString();
                                lblFullName.Text = $"{reader["FirstName"]} {reader["LastName"]}".Trim();
                                lblAddress.Text = reader["Address"].ToString();
                                lblMobileNum.Text = reader["PhoneNumber"].ToString();
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
                using (SqlConnection connection = new SqlConnection(@"Data Source=DSMARI;Initial Catalog=StudySyncDB;Integrated Security=True"))
                {
                    connection.Open();

                    string updateUsernameQuery = "UPDATE tblUser SET Username = @Username WHERE UserID = @AdminID AND UserType = 'admin'";
                    using (SqlCommand updateUsernameCmd = new SqlCommand(updateUsernameQuery, connection))
                    {
                        updateUsernameCmd.Parameters.AddWithValue("@AdminID", adminID);
                        updateUsernameCmd.Parameters.AddWithValue("@Username", txtEditUsername.Text);
                        updateUsernameCmd.ExecuteNonQuery();
                    }

                    string updatePasswordQuery = "UPDATE tblUser SET Password = @Password WHERE UserID = @AdminID AND UserType = 'admin'";
                    using (SqlCommand updatePasswordCmd = new SqlCommand(updatePasswordQuery, connection))
                    {
                        updatePasswordCmd.Parameters.AddWithValue("@AdminID", adminID);
                        updatePasswordCmd.Parameters.AddWithValue("@Password", txtEditPassword.Text); 
                        updatePasswordCmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Changes saved successfully!");

                    
                    LoadAdminData(adminID);
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
