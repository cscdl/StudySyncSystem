using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace StudySyncSystem
{
    public partial class frmArchivedTasks : Form
    {
        public event EventHandler TaskUnarchived;
        private int loggedInUserID;

        public frmArchivedTasks(int userID)
        {
            InitializeComponent();
            loggedInUserID = userID;

            RetrieveArchivedTasks();
        }

        private void RetrieveArchivedTasks()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source=DSMARI;Initial Catalog=StudySyncDB;Integrated Security=True"))
                {
                    connection.Open();

                    string query = $"SELECT TaskID, TaskTitle, TaskStatus, StartDate, EndDate, DateCreated, IsArchived, CategoryID FROM tblTask WHERE UserID = {loggedInUserID} AND IsArchived = 1";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);

                    DataTable archivedTasksTable = new DataTable();
                    adapter.Fill(archivedTasksTable);

                    dgvArchivedTasks.DataSource = archivedTasksTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving archived tasks: " + ex.Message);
            }
        }

        private void UnarchiveTaskInDatabase(int taskID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source=DSMARI;Initial Catalog=StudySyncDB;Integrated Security=True"))
                {
                    connection.Open();

                    string query = "UPDATE tblTask SET IsArchived = 0 WHERE TaskID = @TaskID";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@TaskID", taskID);
                        cmd.ExecuteNonQuery();
                    }

                    // Notify about successful unarchiving
                    MessageBox.Show("Task unarchived successfully!");

                    // Remove the unarchived task from the DataGridView
                    RemoveUnarchivedTaskFromGrid(taskID);

                    // Trigger the event to notify other forms
                    OnTaskUnarchived();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error unarchiving task: " + ex.Message);
            }
        }

        private void RemoveUnarchivedTaskFromGrid(int taskID)
        {
            // Find and remove the row with the given taskID from the DataGridView
            foreach (DataGridViewRow row in dgvArchivedTasks.Rows)
            {
                if (Convert.ToInt32(row.Cells["TaskID"].Value) == taskID)
                {
                    dgvArchivedTasks.Rows.Remove(row);
                    break;
                }
            }
        }

        protected virtual void OnTaskUnarchived()
        {
            TaskUnarchived?.Invoke(this, EventArgs.Empty);
        }

        private int GetColumnIndexByName(DataGridView dataGridView, string columnName)
        {
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                if (column.Name.Equals(columnName, StringComparison.OrdinalIgnoreCase))
                {
                    return column.Index;
                }
            }
            return -1;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dgvArchivedTasks_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dgvArchivedTasks.Columns["Unarchive"].Index)
            {
                int taskIDColumnIndex = GetColumnIndexByName(dgvArchivedTasks, "TaskID");

                if (taskIDColumnIndex != -1)
                {
                    int taskID = Convert.ToInt32(dgvArchivedTasks.Rows[e.RowIndex].Cells[taskIDColumnIndex].Value);

                    DialogResult result = MessageBox.Show("Are you sure you want to unarchive this task?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        UnarchiveTaskInDatabase(taskID);
                    }
                }
                else
                {
                    MessageBox.Show("Column 'TaskID' not found in the DataGridView. Check the column name.");
                }
            }
        }
    }
}
