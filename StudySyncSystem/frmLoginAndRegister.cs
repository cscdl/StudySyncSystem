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
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            if (connect.State != ConnectionState.Open)
            {
                try
                {
                    connect.Open();

                    // Check if the user exists in the 'admin' table
                    string selectAdminQuery = "SELECT * FROM admin WHERE username = @username AND password = @password";
                    using (SqlCommand cmdAdmin = new SqlCommand(selectAdminQuery, connect))
                    {
                        cmdAdmin.Parameters.AddWithValue("@username", username);
                        cmdAdmin.Parameters.AddWithValue("@password", password);

                        using (SqlDataAdapter adapterAdmin = new SqlDataAdapter(cmdAdmin))
                        {
                            DataTable tableAdmin = new DataTable();
                            adapterAdmin.Fill(tableAdmin);

                            if (tableAdmin.Rows.Count >= 1)
                            {
                                MessageBox.Show("Logged In successfully as Admin", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                // Handle admin login logic
                                // For example: Open admin form
                                frmAdmin admin = new frmAdmin();
                                admin.ShowDialog();
                                this.Hide();
                                return;
                            }
                        }
                    }

                    // Check if the user exists in the 'user' table
                    string selectUserQuery = "SELECT * FROM [user] WHERE Username = @username AND Password = @password";
                    using (SqlCommand cmdUser = new SqlCommand(selectUserQuery, connect))
                    {
                        cmdUser.Parameters.AddWithValue("@username", username);
                        cmdUser.Parameters.AddWithValue("@password", password);

                        using (SqlDataAdapter adapterUser = new SqlDataAdapter(cmdUser))
                        {
                            DataTable tableUser = new DataTable();
                            adapterUser.Fill(tableUser);

                            if (tableUser.Rows.Count >= 1)
                            {
                                string userRole = tableUser.Rows[0]["Role"].ToString();
                                MessageBox.Show($"Logged In successfully as {userRole}", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                // Handle user login logic
                                // For example: Open main form
                                if (userRole.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                                {
                                    frmAdmin adminForm = new frmAdmin();
                                    adminForm.Show();
                                }
                                else
                                {
                                    frmMainStudySync mainForm = new frmMainStudySync();
                                    mainForm.Show();
                                }

                                this.Hide();
                                return;
                            }
                        }
                    }

                    // If no match found in both admin and user tables
                    MessageBox.Show("Incorrect Username/Password", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error Connecting: " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connect.Close();
                }


            }

        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string regUsername = txtRegUsername.Text;
            string regPassword = txtRegPassword.Text;
            string regConfirmPassword = txtRegConfirmPassword.Text;

            // Get the selected role from the ComboBox
            string selectedRole = cmbRoles.SelectedItem?.ToString();

            // Determine the role based on the selected item
            string role = selectedRole.Equals("Admin", StringComparison.OrdinalIgnoreCase) ? "Admin" : "User";

            if (regPassword == regConfirmPassword)
            {
                if (connect.State != ConnectionState.Open)
                {
                    try
                    {
                        connect.Open();

                        // Check if the username already exists in the 'user' table
                        string checkUsernameQuery = "SELECT COUNT(*) FROM [user] WHERE Username = @username";
                        using (SqlCommand checkUsernameCmd = new SqlCommand(checkUsernameQuery, connect))
                        {
                            checkUsernameCmd.Parameters.AddWithValue("@username", regUsername);
                            int count = Convert.ToInt32(checkUsernameCmd.ExecuteScalar());

                            if (count > 0)
                            {
                                MessageBox.Show("Username already exists. Please choose a different username.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return;
                            }
                        }

                        // Insert the new user into the 'user' table with the specified role
                        string insertUserQuery = "INSERT INTO [user] (Username, Password, Role) VALUES (@username, @password, @role)";
                        using (SqlCommand insertUserCmd = new SqlCommand(insertUserQuery, connect))
                        {
                            insertUserCmd.Parameters.AddWithValue("@username", regUsername);
                            insertUserCmd.Parameters.AddWithValue("@password", regPassword);
                            insertUserCmd.Parameters.AddWithValue("@role", role);

                            int rowsAffected = insertUserCmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Registration Successful!", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("Registration failed. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error Connecting: " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        connect.Close();
                    }
                }
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
