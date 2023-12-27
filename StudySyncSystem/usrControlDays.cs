using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudySyncSystem
{
    public partial class usrControlDays : UserControl
    {
        private int loggedInUserID;
        public static string static_day;


        public usrControlDays(int userID)
        {
            InitializeComponent();
            loggedInUserID = userID;
        }

        public void days(int numday)
        {
            lblDays.Text = numday + "";
        }

        private void usrControlDays_Click(object sender, EventArgs e)
        {
            static_day = lblDays.Text;

            // Create the form
            frmAddTasks addTasksForm = new frmAddTasks();

            // Set the user ID property
            addTasksForm.LoggedInUserID = loggedInUserID;

            addTasksForm.Show();
        }

        private void usrControlDays_Load(object sender, EventArgs e)
        {

        }
    }
}
