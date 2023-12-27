using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudySyncSystem
{
    public partial class frmAdminDashboard : Form
    {
        private string loggedInUserFirstName;

        public frmAdminDashboard()
        {
            InitializeComponent();
        }

        public void SetLoggedInUserFirstName(string firstName)
        {
            loggedInUserFirstName = firstName;
            adminName.Text = loggedInUserFirstName;

        }

        private void btnViewUsersAndAdmins_Click(object sender, EventArgs e)
        {
            frmViewUsersAndAdmins usersAndAdmins = new frmViewUsersAndAdmins();
            usersAndAdmins.ShowDialog();
        }

        private void btnViewActivityLogs_Click(object sender, EventArgs e)
        {
            frmViewActivityLogs activityLogs = new frmViewActivityLogs();
            activityLogs.ShowDialog();
        }

        private void btnViewAllNotes_Click(object sender, EventArgs e)
        {
            frmAdminViewAllNotes allNotes = new frmAdminViewAllNotes();
            allNotes.ShowDialog();
        }
    }
}
