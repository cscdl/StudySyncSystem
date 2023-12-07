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
    public partial class frmUserDashboard : Form
    {
        public frmUserDashboard()
        {
            InitializeComponent();
        }

        private void btnViewNotes_Click(object sender, EventArgs e)
        {
            frmViewNotes viewNotes = new frmViewNotes();
            viewNotes.ShowDialog();
        }

        private void btnViewTask_Click(object sender, EventArgs e)
        {
            frmViewTask viewTask = new frmViewTask();
            viewTask.ShowDialog();
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
    }
}
