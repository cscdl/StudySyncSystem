using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static StudySyncSystem.frmMainStudySync;

namespace StudySyncSystem
{
    public partial class frmUserProfile : Form
    {
        private int loggedInUserID;

        public frmUserProfile(int userID)
        {
            InitializeComponent();
            loggedInUserID = userID;
            LoadUserData(loggedInUserID);
        }

        private void LoadUserData(int userId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source=DSMARI;Initial Catalog=StudySyncDB;Integrated Security=True"))
                {
                    connection.Open();

                    string query = "SELECT u.Username, ui.FirstName, ui.LastName, ui.Address, ui.Birthday, ui.PhoneNumber FROM tblUser u INNER JOIN tblUserInfo ui ON u.UserID = ui.UserID WHERE u.UserID = @UserID";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Display user information in labels
                                lblUsername.Text =  reader["Username"].ToString();
                                lblFullName.Text = $"{reader["FirstName"]} {reader["LastName"]}".Trim();
                                lblAddress.Text = reader["Address"].ToString();
                                lblBirthday.Text = reader["Birthday"].ToString();
                                lblMobileNum.Text = reader["PhoneNumber"].ToString();
                            }
                            else
                            {
                                MessageBox.Show("User not found");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving user data: " + ex.Message);
            }
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSaveChanges_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source=DSMARI;Initial Catalog=StudySyncDB;Integrated Security=True"))
                {
                    connection.Open();

                    // Update the username
                    string updateUsernameQuery = "UPDATE tblUser SET Username = @Username WHERE UserID = @UserID";
                    using (SqlCommand updateUsernameCmd = new SqlCommand(updateUsernameQuery, connection))
                    {
                        updateUsernameCmd.Parameters.AddWithValue("@Username", txtEditUsername.Text);
                        updateUsernameCmd.Parameters.AddWithValue("@UserID", loggedInUserID);
                        updateUsernameCmd.ExecuteNonQuery();
                    }

                    // Update the password
                    string updatePasswordQuery = "UPDATE tblUser SET Password = @Password WHERE UserID = @UserID";
                    using (SqlCommand updatePasswordCmd = new SqlCommand(updatePasswordQuery, connection))
                    {
                        updatePasswordCmd.Parameters.AddWithValue("@Password", txtEditPassword.Text); // Make sure to hash the password before storing it in the database
                        updatePasswordCmd.Parameters.AddWithValue("@UserID", loggedInUserID);
                        updatePasswordCmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Changes saved successfully!");

                    // Reload user data to update labels
                    LoadUserData(loggedInUserID);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving changes: " + ex.Message);
            }
        }

    }
}
