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
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace StudySyncSystem
{
    public partial class frmLoginAndRegister : Form
    {

        SqlConnection connect = new SqlConnection(@"Data Source=DSMARI;Initial Catalog=StudySyncDB;Integrated Security=True");
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

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.");
                return;
            }

            try
            {
                connect.Open();

                string query = "SELECT UserID, UserType FROM tblUser WHERE Username = @Username AND Password = @Password";
                SqlCommand cmd = new SqlCommand(query, connect);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read()) 
                {
                    int userID = reader.GetInt32(0);
                    string userType = reader.GetString(1);
                    

                    reader.Close(); 

                    if (userType.ToLower() == "admin")
                    {
                        frmAdmin adminDashboard = new frmAdmin(userID);
                        string selectFirstNameQuery = "SELECT FirstName FROM tblUserInfo WHERE UserID = @UserID";
                        SqlCommand selectFirstNameCmd = new SqlCommand(selectFirstNameQuery, connect);
                        selectFirstNameCmd.Parameters.AddWithValue("@UserID", userID);
                        string firstName = Convert.ToString(selectFirstNameCmd.ExecuteScalar());
                        adminDashboard.UpdateUsernameLabel(firstName);
                        adminDashboard.ShowDialog();
                        this.Hide();
                    }
                    else
                    {
                        string selectFirstNameQuery = "SELECT FirstName FROM tblUserInfo WHERE UserID = @UserID";
                        SqlCommand selectFirstNameCmd = new SqlCommand(selectFirstNameQuery, connect);
                        selectFirstNameCmd.Parameters.AddWithValue("@UserID", userID);

                        string firstName = Convert.ToString(selectFirstNameCmd.ExecuteScalar());

                        frmMainStudySync mainStudySync = new frmMainStudySync(userID); 
                        mainStudySync.UpdateUsernameLabel(firstName);
                        mainStudySync.ShowDialog();
                        this.Hide();
                    }

                }
                else
                {
                    MessageBox.Show("Invalid username or password. Please try again.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during login: " + ex.Message);
            }
            finally
            {
                connect.Close();
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string regUsername = txtRegUsername.Text.Trim();
            string regPassword = txtRegPassword.Text;
            string regConfirmPassword = txtRegConfirmPassword.Text;
            string regFirstName = txtRegFirstName.Text.Trim();
            string regLastName = txtRegLastName.Text.Trim();
            string regAddress = txtRegAddress.Text.Trim();
            string regPhoneNumber = txtRegPhoneNumber.Text.Trim();

            if (string.IsNullOrEmpty(regUsername) || string.IsNullOrEmpty(regPassword) || string.IsNullOrEmpty(regConfirmPassword) ||
                string.IsNullOrEmpty(regFirstName) || string.IsNullOrEmpty(regLastName) || string.IsNullOrEmpty(regAddress) ||
                string.IsNullOrEmpty(regPhoneNumber))
            {
                MessageBox.Show("Please fill in all registration fields.");
                return;
            }

            if (regPassword != regConfirmPassword)
            {
                MessageBox.Show("Password and confirm password do not match.");
                return;
            }

            if (ContainsNumbers(regFirstName) || ContainsNumbers(regLastName))
            {
                MessageBox.Show("First Name and Last Name should not contain numbers.");
                return;
            }

            if (!IsValidAddress(regAddress))
            {
                MessageBox.Show("Invalid Address format.");
                return;
            }

            if (!IsValidMobileNumber(regPhoneNumber))
            {
                MessageBox.Show("Invalid Mobile Number format.");
                return;
            }

            if (!IsValidUsername(regUsername))
            {
                MessageBox.Show("Invalid Username format.");
                return;
            }

            if (!IsValidPassword(regPassword))
            {
                MessageBox.Show("Invalid Password format.");
                return;
            }

            if (IsUsernameAlreadyRegistered(regUsername))
            {
                MessageBox.Show("Username already exists. Please choose a different username.");
                return;
            }

            if (txtRegUsername.Text.ToLower() == "admin" && txtRegPassword.Text == "adminpw")
            {
                MessageBox.Show("Admin account registered successfully!");
            }
            else
            {
                try
                {
                    connect.Open();
                    string query = "INSERT INTO tblUser (Username, Password, UserType) VALUES (@Username, @Password, @UserType); SELECT SCOPE_IDENTITY();";
                    SqlCommand cmd = new SqlCommand(query, connect);
                    cmd.Parameters.AddWithValue("@Username", txtRegUsername.Text);
                    cmd.Parameters.AddWithValue("@Password", txtRegPassword.Text);
                    cmd.Parameters.AddWithValue("@UserType", "user"); 

                    int newUserID = Convert.ToInt32(cmd.ExecuteScalar());

                    string insertUserInfoQuery = "INSERT INTO tblUserInfo (FirstName, LastName, Address, PhoneNumber, Birthday, UserID) VALUES (@FirstName, @LastName, @Address, @PhoneNumber, @Birthday, @UserID)";
                    SqlCommand userInfoCmd = new SqlCommand(insertUserInfoQuery, connect);
                    userInfoCmd.Parameters.AddWithValue("@FirstName", txtRegFirstName.Text);
                    userInfoCmd.Parameters.AddWithValue("@LastName", txtRegLastName.Text);
                    userInfoCmd.Parameters.AddWithValue("@Address", txtRegAddress.Text);
                    userInfoCmd.Parameters.AddWithValue("@PhoneNumber", txtRegPhoneNumber.Text);
                    userInfoCmd.Parameters.AddWithValue("@Birthday", Convert.ToDateTime(dtpRegBirthday.Text));
                    userInfoCmd.Parameters.AddWithValue("@UserID", newUserID);

                    userInfoCmd.ExecuteNonQuery();

                    DialogResult confirmationResult = MessageBox.Show("Please make sure the entered details are correct, as they cannot be edited later. Do you want to proceed?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (confirmationResult == DialogResult.No)
                    {
                        return;
                    }

                    MessageBox.Show("User registered successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error during registration: " + ex.Message);
                }
                finally
                {
                    connect.Close();
                }
            }

        }

        private bool ContainsNumbers(string input)
        {
            return input.Any(char.IsDigit);
        }

        private bool IsValidAddress(string address)
        {
            string pattern = @"^[a-zA-Z0-9\s,.'-]{3,}$";
            return Regex.IsMatch(address, pattern);
        }

        private bool IsValidMobileNumber(string mobileNumber)
        {
            string pattern = @"^\d{11}$";
            return Regex.IsMatch(mobileNumber, pattern);
        }

        private bool IsValidUsername(string username)
        {
            string pattern = @"^[a-zA-Z0-9_]{3,20}$";
            return Regex.IsMatch(username, pattern);
        }

        private bool IsValidPassword(string password)
        {
            string pattern = @"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$";
            return Regex.IsMatch(password, pattern);
        }

        private bool IsUsernameAlreadyRegistered(string username)
        {
            try
            {
                using (SqlConnection checkConnection = new SqlConnection(@"Data Source=DSMARI;Initial Catalog=StudySyncDB;Integrated Security=True"))
                {
                    checkConnection.Open();

                    using (SqlCommand checkUsernameCmd = new SqlCommand("SELECT COUNT(*) FROM tblUser WHERE Username = @Username", checkConnection))
                    {
                        checkUsernameCmd.Parameters.AddWithValue("@Username", username);
                        int count = Convert.ToInt32(checkUsernameCmd.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error checking username: " + ex.Message);
                return false;
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


        private void chckShowPassword1_CheckedChanged_1(object sender, EventArgs e)
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnClose2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

    }
}
