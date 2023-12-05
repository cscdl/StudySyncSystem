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
    public partial class frmAddTasks : Form
    {
        public frmAddTasks()
        {
            InitializeComponent();
        }

        private void frmAddTasks_Load(object sender, EventArgs e)
        {
            lblDate.Text = frmMainStudySync.static_month + "/" + usrControlDays.static_day + "/" + frmMainStudySync.static_year;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
