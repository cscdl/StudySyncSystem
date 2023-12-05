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
    public partial class frmViewTask : Form
    {
        public frmViewTask()
        {
            InitializeComponent();
        }

        DataTable todoListTask = new DataTable();

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            Close();
        }

        private void frmViewTask_Load(object sender, EventArgs e)
        {
            // create columns for task
            todoListTask.Columns.Add("Task Name");
            todoListTask.Columns.Add("Date Started");
            todoListTask.Columns.Add("End Date");
            todoListTask.Columns.Add("Status");

            dataGridView1.DataSource = todoListTask;
        }
    }
}
