using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace StudySyncSystem
{
    public partial class frmViewUsersAndAdmins : Form
    {
        private SqlConnection connection = new SqlConnection(@"Data Source=DSMARI;Initial Catalog=StudySyncDB;Integrated Security=True");
        private DataTable originalUsersTable;

        public frmViewUsersAndAdmins()
        {
            InitializeComponent();
        }

        private void frmViewUsersAndAdmins_Load(object sender, EventArgs e)
        {
            dgvUsers.Columns["Username"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvUsers.Columns["UserType"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            originalUsersTable = RetrieveUsersFromDatabase();
            dgvUsers.DataSource = originalUsersTable;
        }

        private DataTable RetrieveUsersFromDatabase()
        {
            DataTable usersTable = new DataTable();

            try
            {
                connection.Open();
                string query = "SELECT UserID, Username, UserType FROM tblUser";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(usersTable);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving users from the database: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return usersTable;
        }

        
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                SearchUsers(searchTerm);
            }
            else
            {
                dgvUsers.DataSource = originalUsersTable;
            }
        }

        private void SearchUsers(string searchTerm)
        {
            if (originalUsersTable != null)
            {
                DataView dv = originalUsersTable.DefaultView;
                dv.RowFilter = $"Username LIKE '%{searchTerm}%'";
                dgvUsers.DataSource = dv.ToTable();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
