using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace StudySyncSystem
{
    public partial class frmViewPendingTask : Form
    {
        private int loggedInUserID;

        public frmViewPendingTask(int userID)
        {
            InitializeComponent();
            loggedInUserID = userID;
            SetDataSource();
        }

        private void SetDataSource()
        {
            dgvPendingTasks.AutoGenerateColumns = false;
            dgvPendingTasks.Columns["TaskID"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvPendingTasks.Columns["TaskTitle"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvPendingTasks.Columns["StartDate"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvPendingTasks.Columns["EndDate"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            

            dgvPendingTasks.DataSource = RetrievePendingTasksForLoggedInUser(loggedInUserID);
        }

        private DataTable RetrievePendingTasksForLoggedInUser(int userID)
        {
            DataTable pendingTasks = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source=DSMARI;Initial Catalog=StudySyncDB;Integrated Security=True"))
                {
                    connection.Open();

                    string query = $"SELECT TaskID, TaskTitle, TaskStatus, StartDate, EndDate, DateCreated, IsArchived, CategoryID FROM tblTask WHERE UserID = @UserID AND TaskStatus = 'Pending'";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userID);

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(pendingTasks);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving pending tasks from the logged-in user: {ex.Message}");
            }

            return pendingTasks;
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
