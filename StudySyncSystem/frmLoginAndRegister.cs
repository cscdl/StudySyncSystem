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
                        frmAdmin adminDashboard = new frmAdmin();
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
