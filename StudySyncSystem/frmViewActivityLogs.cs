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
    public partial class frmViewActivityLogs : Form
    {
        public frmViewActivityLogs()
        {
            InitializeComponent();
        }

        DataTable activityLog = new DataTable();
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmViewActivityLogs_Load(object sender, EventArgs e)
        {
            // create columns for activity log
            activityLog.Columns.Add("Title");
            activityLog.Columns.Add("Date Created");

            dgvActivityLogs.DataSource = activityLog;
        }
    }
}
