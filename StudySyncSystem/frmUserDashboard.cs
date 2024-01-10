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
    public partial class frmUserDashboard : Form
    {
        private string loggedInUsername;
        private int loggedInUserID;

        public frmUserDashboard(int userID)
        {
            InitializeComponent();
            loggedInUserID = userID;

            UpdateUsernameLabel(DatabaseHelper.GetFirstNameForUserID(loggedInUserID));
            UpdateNoteCountLabel();
            UpdateTaskCountLabel();

        }
        public void UpdateUsernameLabel(string username)
        {
            loggedInUsername = username;
            lblUsername.Text = loggedInUsername;
        }



        private void btnViewNotes_Click(object sender, EventArgs e)
        {
            frmViewNotes viewNotesForm = new frmViewNotes();
            viewNotesForm.SetLoggedInUserID(loggedInUserID);
            viewNotesForm.ShowDialog();
        }



        private void btnViewTask_Click(object sender, EventArgs e)
        {
            frmViewTask viewTaskForm = new frmViewTask();
            viewTaskForm.SetLoggedInUserID(loggedInUserID);
            viewTaskForm.ShowDialog();
        }

        private void btnViewFiles_Click(object sender, EventArgs e)
        {
            int userID = GetLoggedInUserID();
            int categoryID = GetCategoryID();

            frmViewFiles viewFiles = new frmViewFiles(userID, categoryID);
            viewFiles.ShowDialog();
        }

        private int GetCategoryID()
        {
            int categoryID = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source=DSMARI;Initial Catalog=StudySyncDB;Integrated Security=True"))
                {
                    connection.Open();

                    string query = "SELECT TOP 1 CategoryID FROM tblCategory WHERE UserID = @UserID";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserID", GetLoggedInUserID());
                        categoryID = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving category ID: " + ex.Message);
            }

            return categoryID;
        }

        public int GetLoggedInUserID()
        {
            return loggedInUserID;
        }

        private void btnPendingTask_Click(object sender, EventArgs e)
        {
            int userID = loggedInUserID;

            frmViewPendingTask viewPendingTask = new frmViewPendingTask(userID);
            viewPendingTask.ShowDialog();
        }


        private void UpdateNoteCountLabel()
        {
            int noteCount = DatabaseHelper.GetNoteCountForUser(loggedInUserID);
            lblTotalNotes.Text = noteCount.ToString();
        }

        private void UpdateTaskCountLabel()
        {
            int taskCount = DatabaseHelper.GetTaskCountForUser(loggedInUserID);
            lblTotalTask.Text = taskCount.ToString();
        }
    }
}
