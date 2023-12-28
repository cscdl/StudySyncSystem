using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace StudySyncSystem
{
    public partial class frmMainStudySync : Form
    {
        private string loggedInUsername;
        private int loggedInUserID;

        bool SideBarExpand;
        int month, year;
        public static int static_month, static_year;
        public frmMainStudySync(int userID)
        {
            InitializeComponent();
            InitializeUI("UIMode");
            loggedInUserID = userID;
            UpdateUsernameLabel(DatabaseHelper.GetFirstNameForUserID(loggedInUserID));
            UpdateNoteCountLabel();
            UpdateTaskCountLabel();

        }
        public static class DatabaseHelper
        {
            // Your other database-related methods go here

            public static string GetFirstNameForUserID(int userID)
            {
                string firstName = string.Empty;

                try
                {
                    using (SqlConnection connection = new SqlConnection(@"Data Source=DSMARI;Initial Catalog=StudySyncDB;Integrated Security=True"))
                    {
                        connection.Open();

                        string query = "SELECT FirstName FROM tblUserInfo WHERE UserID = @UserID";
                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@UserID", userID);
                            object result = cmd.ExecuteScalar();

                            if (result != null)
                            {
                                firstName = result.ToString();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error retrieving first name: " + ex.Message);
                }

                return firstName;


            }
            //test test test
            public static int GetNoteCountForUser(int userID)
            {
                int noteCount = 0;

                try
                {
                    using (SqlConnection connection = new SqlConnection(@"Data Source=DSMARI;Initial Catalog=StudySyncDB;Integrated Security=True"))
                    {
                        connection.Open();

                        string query = "SELECT COUNT(*) FROM tblNote WHERE UserID = @UserID";
                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@UserID", userID);
                            noteCount = (int)cmd.ExecuteScalar();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error retrieving note count: " + ex.Message);
                }

                return noteCount;
            }

            public static int GetTaskCountForUser(int userID)
            {
                int taskCount = 0;

                try
                {
                    using (SqlConnection connection = new SqlConnection(@"Data Source=DSMARI;Initial Catalog=StudySyncDB;Integrated Security=True"))
                    {
                        connection.Open();

                        string query = "SELECT COUNT(*) FROM tblTask WHERE UserID = @UserID";
                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@UserID", userID);
                            taskCount = (int)cmd.ExecuteScalar();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error retrieving task count: " + ex.Message);
                }

                return taskCount;
            }

        }

        private void displayDays()
        {
            DateTime now = DateTime.Now;

            month = now.Month;
            year = now.Year;

            string monthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(month);
            lblMonthYear.Text = monthName + " " + year;

            static_month = month;
            static_year = year;

            DateTime startOfMonth = new DateTime(year, month, 1);

            int days = DateTime.DaysInMonth(year, month);

            int daysOfweek = Convert.ToInt32(startOfMonth.DayOfWeek.ToString("d")) + 1;

            for (int i = 1; i < daysOfweek; i++)
            {
                usrControlBlank usrBlank = new usrControlBlank();
                dayContainer.Controls.Add(usrBlank);
            }

            for (int i = 1; i <= days; i++)
            {
                usrControlDays usrDays = new usrControlDays(loggedInUserID);
                usrDays.days(i);
                dayContainer.Controls.Add(usrDays);
            }
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

            panel15.BackColor = Color.FromArgb(115, 160, 195);
        }

        private void ApplyLightModeColors()
        {
            this.ForeColor = Color.FromArgb(255, 255, 255);
            this.BackColor = Color.FromArgb(0, 8, 20);

            panel15.BackColor = Color.FromArgb(0, 8, 20);
        }

        public void loadform(object Form)
        {
            if (this.pnlMain.Controls.Count > 0)
                this.pnlMain.Controls.RemoveAt(0);

            Form mainStudySync = Form as Form;
            mainStudySync.TopLevel = false;
            mainStudySync.Dock = DockStyle.Fill;

            this.pnlMain.Controls.Add(mainStudySync);
            this.pnlMain.Tag = mainStudySync;

            mainStudySync.Show();
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

        private void frmMainStudySync_Load(object sender, EventArgs e)
        {
            displayDays();
            UpdateNoteCountLabel();

        }

        private void picPrevious_Click(object sender, EventArgs e)
        {
            dayContainer.Controls.Clear();

            month--;
            if (month < 1)
            {
                month = 12;
                year--;
            }

            string monthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(month);
            lblMonthYear.Text = monthName + " " + year;

            static_month = month;
            static_year = year;

            DateTime now = DateTime.Now;

            DateTime startOfMonth = new DateTime(year, month, 1);

            int days = DateTime.DaysInMonth(year, month);

            int daysOfweek = Convert.ToInt32(startOfMonth.DayOfWeek.ToString("d")) + 1;

            for (int i = 1; i < daysOfweek; i++)
            {
                usrControlBlank usrBlank = new usrControlBlank();
                dayContainer.Controls.Add(usrBlank);
            }

            for (int i = 1; i <= days; i++)
            {
                usrControlDays usrDays = new usrControlDays(loggedInUserID);
                usrDays.days(i);
                dayContainer.Controls.Add(usrDays);
            }
        }

        private void picnext_Click(object sender, EventArgs e)
        {
            dayContainer.Controls.Clear();

            month++;
            if (month > 12)
            {
                month = 1;
                year++;
            }

            string monthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(month);
            lblMonthYear.Text = monthName + " " + year;

            static_month = month;
            static_year = year;

            DateTime now = DateTime.Now;

            DateTime startOfMonth = new DateTime(year, month, 1);

            int days = DateTime.DaysInMonth(year, month);

            int daysOfweek = Convert.ToInt32(startOfMonth.DayOfWeek.ToString("d")) + 1;

            for (int i = 1; i < daysOfweek; i++)
            {
                usrControlBlank usrBlank = new usrControlBlank();
                dayContainer.Controls.Add(usrBlank);
            }

            for (int i = 1; i <= days; i++)
            {
                usrControlDays usrDays = new usrControlDays(loggedInUserID);
                usrDays.days(i);
                dayContainer.Controls.Add(usrDays);
            }
        }

        private void btnSettings_Click_1(object sender, EventArgs e)
        {
            InitializeUI("UIMode");
        }

        private void btnAddNotes_Click(object sender, EventArgs e)
        {
            // Create the form
            frmAddNotes addNotesForm = new frmAddNotes();

            // Set the user ID property
            addNotesForm.LoggedInUserID = loggedInUserID;


            // Show the form
            loadform(addNotesForm);
        }



        private void btnLogout_Click(object sender, EventArgs e)
        {
            Hide();
            frmLoginAndRegister loginandregister = new frmLoginAndRegister();
            loginandregister.ShowDialog();

        }

        private void btnPicUser_Click(object sender, EventArgs e)
        {
            loadform(new frmUserProfile());
        }

        private void btnViewTask_Click(object sender, EventArgs e)
        {
            frmViewTask viewTaskForm = new frmViewTask();
            viewTaskForm.SetLoggedInUserID(loggedInUserID);
            viewTaskForm.ShowDialog();
        }

        private void btnViewNotes_Click(object sender, EventArgs e)
        {
            frmViewNotes viewNotesForm = new frmViewNotes();
            viewNotesForm.SetLoggedInUserID(loggedInUserID);
            viewNotesForm.ShowDialog();
        }

        private void btnViewFiles_Click(object sender, EventArgs e)
        {
            frmViewFiles viewFiles = new frmViewFiles();
            viewFiles.ShowDialog();
        }

        private void btnUploadFile_Click(object sender, EventArgs e)
        {
            loadform(new frmUploadFile());
        }

        private void btnDashboard_Click_1(object sender, EventArgs e)
        {
            int userID = GetLoggedInUserID();
            string firstName = DatabaseHelper.GetFirstNameForUserID(userID);

            // Assuming your frmUserDashboard has a method to update the username label
            frmUserDashboard userDash = new frmUserDashboard(userID);
            userDash.UpdateUsernameLabel(firstName);
            loadform(userDash);
        }



        private void panel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnCalculator_Click(object sender, EventArgs e)
        {
            loadform(new frmCalculator());
        }

        private void pnlMain_Paint(object sender, PaintEventArgs e)
        {
            
        }

        public int GetLoggedInUserID()
        {
            return loggedInUserID;
        }


        public void UpdateUsernameLabel(string username)
        {
            loggedInUsername = username;
            lblUsername.Text = loggedInUsername;
            lblName.Text = loggedInUsername;
        }

        private void pnlMain2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        //test test test 

        private void UpdateNoteCountLabel()
        {
            int noteCount = DatabaseHelper.GetNoteCountForUser(loggedInUserID);
            lblTotalNotes.Text = noteCount.ToString();
        }

        private void UpdateTaskCountLabel()
        {
            int taskCount = DatabaseHelper.GetTaskCountForUser(loggedInUserID);
            lblTotalTasks.Text = taskCount.ToString();
        }

    }
}
