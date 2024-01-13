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
        private bool isClicked = false;

        public usrControlDays(int userID)
        {
            InitializeComponent();
            loggedInUserID = userID;
            this.Click += usrControlDays_Click;
        }

        public void days(int numday)
        {
            lblDays.Text = numday + "";
        }

        private void usrControlDays_Click(object sender, EventArgs e)
        {
            isClicked = true;
            static_day = lblDays.Text;

            frmAddTasks addTasksForm = new frmAddTasks(loggedInUserID);

            addTasksForm.LoggedInUserID = loggedInUserID;
            addTasksForm.FormClosed += AddTasksForm_FormClosed;

            addTasksForm.Show();
            this.Invalidate();
        }

        private void AddTasksForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            isClicked = false;
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (isClicked)
            {
                ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle, Color.White, ButtonBorderStyle.Solid);
            }
        }

        private void usrControlDays_Load(object sender, EventArgs e)
        {

        }
    }
}
