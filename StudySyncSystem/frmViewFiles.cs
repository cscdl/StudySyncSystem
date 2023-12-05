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
    public partial class frmViewFiles : Form
    {
        public frmViewFiles()
        {
            InitializeComponent();
        }

        DataTable todoListFiles = new DataTable();

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmViewFiles_Load(object sender, EventArgs e)
        {
            // create columns for task
            todoListFiles.Columns.Add("File Name");
            todoListFiles.Columns.Add("File");

            dataGridView1.DataSource = todoListFiles;
        }
    }
}
