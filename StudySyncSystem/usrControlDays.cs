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
        public static string static_day;
        public usrControlDays()
        {
            InitializeComponent();
        }

        public void days(int numday)
        {
            lblDays.Text = numday + "";
        }

        private void usrControlDays_Click(object sender, EventArgs e)
        {
            static_day = lblDays.Text;

            frmAddTasks addTasks = new frmAddTasks();
            addTasks.ShowDialog();
        }

        private void usrControlDays_Load(object sender, EventArgs e)
        {

        }
    }
}
