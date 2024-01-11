using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace StudySyncSystem
{
    public partial class frmViewCompletedTasks : Form
    {
        private int loggedInUserID;
        private DataTable originalCompletedTasksTable;

        public frmViewCompletedTasks(int userID)
        {
            InitializeComponent();
            loggedInUserID = userID;
            SetDataSource();
            originalCompletedTasksTable = (DataTable)dgvCompletedTasks.DataSource;
        }

        private void SetDataSource()
        {
            dgvCompletedTasks.AutoGenerateColumns = false;
            dgvCompletedTasks.Columns["TaskID"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvCompletedTasks.Columns["TaskTitle"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvCompletedTasks.Columns["StartDate"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvCompletedTasks.Columns["EndDate"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgvCompletedTasks.DataSource = RetrieveCompletedTasksForLoggedInUser(loggedInUserID);
        }

        private DataTable RetrieveCompletedTasksForLoggedInUser(int userID)
        {
            DataTable completedTasks = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source=DSMARI;Initial Catalog=StudySyncDB;Integrated Security=True"))
                {
                    connection.Open();

                    string query = $"SELECT TaskID, TaskTitle, TaskStatus, StartDate, EndDate, DateCreated, IsArchived, CategoryID FROM tblTask WHERE UserID = @UserID AND TaskStatus = 'Complete'";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userID);

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(completedTasks);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving completed tasks: {ex.Message}");
            }

            return completedTasks;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();

            if (originalCompletedTasksTable != null)
            {
                DataRow[] filteredRows = originalCompletedTasksTable.Select($"TaskTitle LIKE '%{searchTerm}%'");

                DataTable filteredTable = originalCompletedTasksTable.Clone();
                foreach (DataRow row in filteredRows)
                {
                    filteredTable.ImportRow(row);
                }

                dgvCompletedTasks.DataSource = filteredTable;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        
    }
}
