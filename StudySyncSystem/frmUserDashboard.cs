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
            frmViewFiles viewFiles = new frmViewFiles();
            viewFiles.ShowDialog();
        }

        private void btnPendingTask_Click(object sender, EventArgs e)
        {
            frmViewPendingTask viewPendingTask = new frmViewPendingTask();
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
