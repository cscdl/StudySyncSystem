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
    public partial class frmViewUsersAndAdmins : Form
    {
        public frmViewUsersAndAdmins()
        {
            InitializeComponent();
        }

        DataTable todoListUsers = new DataTable();
        private void frmViewUsersAndAdmins_Load(object sender, EventArgs e)
        {
            // create columns for users and admins
            todoListUsers.Columns.Add("Username");
            todoListUsers.Columns.Add("Name");
            todoListUsers.Columns.Add("User Type");

            dgvUsers.DataSource = todoListUsers;

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
