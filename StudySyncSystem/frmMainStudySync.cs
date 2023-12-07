using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
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
        bool SideBarExpand;
        int month, year;
        public static int static_month, static_year;
        public frmMainStudySync()
        {
            InitializeComponent();
            InitializeUI("UIMode");
            
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
                usrControlDays usrDays = new usrControlDays();
                usrDays.days(i);
                dayContainer.Controls.Add(usrDays);
            }
        }

        private void InitializeUI(string key)
        {
            try
            {
                var uiMode = ConfigurationManager.AppSettings[key];

                if (uiMode == "light")
                {
                    btnSettings.Text = "Dark Mode";
                    this.ForeColor = Color.FromArgb(0, 8, 20);
                    this.BackColor = Color.FromArgb(239,229,220);
                    ConfigurationManager.AppSettings[key] = "dark";
                }
                else
                {
                    btnSettings.Text = "Light Mode";
                    this.ForeColor = Color.FromArgb(239, 229, 220);
                    this.BackColor = Color.FromArgb(0, 8, 20);
                    ConfigurationManager.AppSettings[key] = "light";
                }
            }
            catch (Exception e)
            {
                throw;
            }
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
                usrControlDays usrDays = new usrControlDays();
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
                usrControlDays usrDays = new usrControlDays();
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
            loadform(new frmAddNotes());
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
            frmViewTask viewTask = new frmViewTask();
            viewTask.ShowDialog();
        }

        private void btnViewNotes_Click(object sender, EventArgs e)
        {
            frmViewNotes viewNotes = new frmViewNotes();
            viewNotes.ShowDialog();
        }

        private void btnViewFiles_Click(object sender, EventArgs e)
        {
            frmViewFiles viewFiles = new frmViewFiles();
            viewFiles.ShowDialog();
        }

        private void btnEditNotes_Click(object sender, EventArgs e)
        {
            loadform(new frmEditNotes());
        }

        private void btnUploadFile_Click(object sender, EventArgs e)
        {
            loadform(new frmUploadFile());
        }

        private void btnDashboard_Click_1(object sender, EventArgs e)
        {
            loadform(new frmUserDashboard());
        }

        

    }
}
