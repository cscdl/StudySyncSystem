using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace StudySyncSystem
{
    public partial class frmAddAdminAccount : Form
    {
        SqlConnection connect = new SqlConnection(@"Data Source=DSMARI;Initial Catalog=StudySyncDB;Integrated Security=True");

        public frmAddAdminAccount()
        {
            InitializeComponent();
        }

        private void btnRegisterAdmin_Click(object sender, EventArgs e)
        {
            try
            {
                connect.Open();

                // Assuming you have UI elements to get the new admin's information
                string newAdminUsername = txtRegAdUsername.Text;
                string newAdminPassword = txtRegAdPassword.Text;
                string newAdminFirstName = txtRegAdFirstName.Text;
                string newAdminLastName = txtRegAdLastName.Text;
                string newAdminAddress = txtRegAdAddress.Text;
                string newAdminPhoneNumber = txtRegAdMobileNum.Text;

                // Perform the insertion into tblUser for the new admin
                string insertAdminQuery = "INSERT INTO tblUser (Username, Password, UserType) VALUES (@Username, @Password, @UserType); SELECT SCOPE_IDENTITY();";
                SqlCommand cmd = new SqlCommand(insertAdminQuery, connect);
                cmd.Parameters.AddWithValue("@Username", newAdminUsername);
                cmd.Parameters.AddWithValue("@Password", newAdminPassword);
                cmd.Parameters.AddWithValue("@UserType", "admin"); // Set UserType to 'admin' for administrators

                // Execute the query and get the new AdminID
                int newAdminID = Convert.ToInt32(cmd.ExecuteScalar());

                // Now, insert the admin info into tblUserInfo (assuming this table holds additional info for admins)
                string insertAdminInfoQuery = "INSERT INTO tblUserInfo (UserID, FirstName, LastName, Address, PhoneNumber) VALUES (@UserID, @FirstName, @LastName, @Address, @PhoneNumber)";
                SqlCommand adminInfoCmd = new SqlCommand(insertAdminInfoQuery, connect);
                adminInfoCmd.Parameters.AddWithValue("@UserID", newAdminID);
                adminInfoCmd.Parameters.AddWithValue("@FirstName", newAdminFirstName);
                adminInfoCmd.Parameters.AddWithValue("@LastName", newAdminLastName);
                adminInfoCmd.Parameters.AddWithValue("@Address", newAdminAddress);
                adminInfoCmd.Parameters.AddWithValue("@PhoneNumber", newAdminPhoneNumber);

                adminInfoCmd.ExecuteNonQuery();

                MessageBox.Show("New admin added successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding new admin: " + ex.Message);
            }
            finally
            {
                connect.Close();
            }
        }

        private void chckShowPassword1_CheckedChanged(object sender, EventArgs e)
        {
            if (chckShowPassword1.Checked == true)
            {
                txtRegAdPassword.UseSystemPasswordChar = false;
                txtRegConfirmPassword.UseSystemPasswordChar = false;
            }
            else
            {
                txtRegAdPassword.UseSystemPasswordChar = true;
                txtRegConfirmPassword.UseSystemPasswordChar = true;
            }
        }
    }
}

