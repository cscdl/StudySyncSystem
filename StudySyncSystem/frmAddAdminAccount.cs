using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
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

        private void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                connect.Open();

                string newAdminUsername = txtRegAdUsername.Text;
                string newAdminPassword = txtRegAdPassword.Text;
                string newAdminFirstName = txtRegAdFirstName.Text;
                string newAdminLastName = txtRegAdLastName.Text;
                string newAdminAddress = txtRegAdAddress.Text;
                string newAdminPhoneNumber = txtRegAdMobileNum.Text;

                if (!IsValidUsername(newAdminUsername) || !IsValidPassword(newAdminPassword) || !IsValidName(newAdminFirstName) || !IsValidName(newAdminLastName) || !IsValidAddress(newAdminAddress) || !IsValidPhoneNumber(newAdminPhoneNumber))
                {
                    MessageBox.Show("Invalid input. Please check the provided information.");
                    return;
                }

                if (IsAdminUsernameAlreadyRegistered(newAdminUsername))
                {
                    MessageBox.Show("Admin account with this username already exists. Please choose a different username.");
                    return;
                }

                string insertAdminQuery = "INSERT INTO tblUser (Username, Password, UserType) VALUES (@Username, @Password, @UserType); SELECT SCOPE_IDENTITY();";
                SqlCommand cmd = new SqlCommand(insertAdminQuery, connect);
                cmd.Parameters.AddWithValue("@Username", newAdminUsername);
                cmd.Parameters.AddWithValue("@Password", newAdminPassword);
                cmd.Parameters.AddWithValue("@UserType", "admin");

                int newAdminID = Convert.ToInt32(cmd.ExecuteScalar());

                string insertAdminInfoQuery = "INSERT INTO tblUserInfo (UserID, FirstName, LastName, Address, PhoneNumber) VALUES (@UserID, @FirstName, @LastName, @Address, @PhoneNumber)";
                SqlCommand adminInfoCmd = new SqlCommand(insertAdminInfoQuery, connect);
                adminInfoCmd.Parameters.AddWithValue("@UserID", newAdminID);
                adminInfoCmd.Parameters.AddWithValue("@FirstName", newAdminFirstName);
                adminInfoCmd.Parameters.AddWithValue("@LastName", newAdminLastName);
                adminInfoCmd.Parameters.AddWithValue("@Address", newAdminAddress);
                adminInfoCmd.Parameters.AddWithValue("@PhoneNumber", newAdminPhoneNumber);

                adminInfoCmd.ExecuteNonQuery();

                MessageBox.Show("New admin added successfully!");

                txtRegAdPassword.Clear();
                txtRegConfirmPassword.Clear();
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


        private bool IsValidUsername(string newAdminUsername)
        {
            string pattern = @"^[a-zA-Z0-9_]{3,20}$";
            return Regex.IsMatch(newAdminUsername, pattern);
        }

        private bool IsValidPassword(string newAdminPassword)
        {
            string pattern = @"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$";
            return Regex.IsMatch(newAdminPassword, pattern);
        }

        private bool IsValidName(string newAdminFirstName)
        {
            string pattern = @"^[a-zA-Z]+$";
            return Regex.IsMatch(newAdminFirstName, pattern);
        }


        private bool IsValidAddress(string newAdminAddress)
        {
            string pattern = @"^[a-zA-Z0-9\s,.-]+$";
            return Regex.IsMatch(newAdminAddress, pattern);
        }

        private bool IsValidPhoneNumber(string newAdminPhoneNumber)
        {
            string pattern = @"^[0-9]+$";
            return Regex.IsMatch(newAdminPhoneNumber, pattern);
        }

        private bool IsAdminUsernameAlreadyRegistered(string username)
        {
            try
            {
                using (SqlCommand checkUsernameCmd = new SqlCommand("SELECT COUNT(*) FROM tblUser WHERE Username = @Username AND UserType = 'admin'", connect))
                {
                    checkUsernameCmd.Parameters.AddWithValue("@Username", username);
                    int count = Convert.ToInt32(checkUsernameCmd.ExecuteScalar());
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error checking admin username: " + ex.Message);
                return false;
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

