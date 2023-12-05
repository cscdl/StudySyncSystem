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
    public partial class frmViewNotes : Form
    {
        public frmViewNotes()
        {
            InitializeComponent();
        }

        DataTable todoListNotes = new DataTable();
        private void frmViewNotes_Load(object sender, EventArgs e)
        {
            // create columns for task
            todoListNotes.Columns.Add("Title");
            todoListNotes.Columns.Add("Notes");

            dataGridView1.DataSource = todoListNotes;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
