using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudySyncSystem
{
    public partial class frmViewTask : Form
    {
        private SqlConnection connection = new SqlConnection(@"Data Source=DSMARI;Initial Catalog=StudySyncDB;Integrated Security=True");
        private int loggedInUserID;
        private frmArchivedTasks archivedTasksForm;

        public frmViewTask()
        {
            InitializeComponent();
        }

        public void SetLoggedInUserID(int userID)
        {
            loggedInUserID = userID;
            SetDataSource();
            LoadTasks();
        }


        private void frmViewTask_Load(object sender, EventArgs e)
        {
            dgvTasks.AutoGenerateColumns = false;
            dgvTasks.Columns["TaskTitle"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvTasks.Columns["TaskStatus"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvTasks.Columns["StartDate"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvTasks.Columns["EndDate"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvTasks.Columns["DateCreated"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvTasks.Columns["IsArchived"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvTasks.Columns["CategoryID"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            SetDataSource();

            cmbFilter.SelectedIndexChanged += cmbFilter_SelectedIndexChanged;
            dgvTasks.CellContentClick += dgvTasks_CellContentClick;
            dgvTasks.EditingControlShowing += dgvTasks_EditingControlShowing;


            LoadTasks();
        }

        private int GetLoggedInUserID()
        {
            return loggedInUserID;
        }

        private void SetDataSource()
        {
            dgvTasks.DataSource = RetrieveTasksForLoggedInUser(loggedInUserID);
        }

        private void OpenAddTasksForm()
        {
            int userID = GetLoggedInUserID();
            frmAddTasks addTasksForm = new frmAddTasks();
            addTasksForm.StartPosition = FormStartPosition.CenterScreen;

            addTasksForm.DataSaved += FrmAddTasks_DataSaved;

            addTasksForm.Show();
        }

        private DataTable RetrieveTasksForLoggedInUser(int userID, bool showArchived = false)
        {
            DataTable todoListTask = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source=DSMARI;Initial Catalog=StudySyncDB;Integrated Security=True"))
                {
                    connection.Open();

                    string query = $"SELECT TaskID, TaskTitle, TaskStatus, StartDate, EndDate, DateCreated, IsArchived, CategoryID FROM tblTask WHERE UserID = @UserID AND IsArchived = {(showArchived ? 1 : 0)}";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userID);

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(todoListTask);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving tasks from the logged-in user: {ex.Message}");
            }

            return todoListTask;
        }

        private void FrmAddTasks_DataSaved(object sender, EventArgs e)
        {
            dgvTasks.DataSource = RetrieveTasksForLoggedInUser(loggedInUserID);
        }



        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            OpenAddTasksForm();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {

            DataGridViewRow selectedRow = dgvTasks.CurrentRow;

            if (selectedRow != null)
            {
                int taskID = Convert.ToInt32(selectedRow.Cells["TaskID"].Value);


                OpenEditTasksForm(taskID);
            }
            else
            {
                MessageBox.Show("Please select a task to edit.");
            }
        }

        private void OpenEditTasksForm(int taskID)
        {
            frmEditTasks editTasksForm = new frmEditTasks(taskID);
            editTasksForm.StartPosition = FormStartPosition.CenterScreen;

            editTasksForm.DataSaved += FrmEditTasks_DataSaved;

            editTasksForm.ShowDialog();


            dgvTasks.DataSource = RetrieveTasksForLoggedInUser(loggedInUserID);
        }

        private void FrmEditTasks_DataSaved(object sender, EventArgs e)
        {
            SetDataSource();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            DataGridViewRow selectedRow = dgvTasks.CurrentRow;

            if (selectedRow != null)
            {
                int taskID = Convert.ToInt32(selectedRow.Cells["TaskID"].Value);


                DialogResult result = MessageBox.Show("Are you sure you want to delete this task?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {

                    DeleteTaskFromDatabase(taskID);


                    dgvTasks.Rows.Remove(selectedRow);
                }
            }
            else
            {
                MessageBox.Show("Please select a task to delete.");
            }
        }
        private void DeleteTaskFromDatabase(int taskID)
        {
            try
            {
                using (SqlConnection deleteConnection = new SqlConnection(@"Data Source=DSMARI;Initial Catalog=StudySyncDB;Integrated Security=True"))
                {
                    deleteConnection.Open();

                    string query = "DELETE FROM tblTask WHERE TaskID = @TaskID";
                    using (SqlCommand cmd = new SqlCommand(query, deleteConnection))
                    {
                        cmd.Parameters.AddWithValue("@TaskID", taskID);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Task deleted successfully!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting task: " + ex.Message);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchCriteria = txtSearch.Text.Trim();

            dgvTasks.DataSource = RetrieveTasksForLoggedInUser(loggedInUserID);
        }

        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedFilter = cmbFilter.SelectedItem.ToString();

            string filterCriteria = GetFilterCriteria(selectedFilter);


            dgvTasks.DataSource = RetrieveTasksForLoggedInUser(loggedInUserID);
        }


        private string GetFilterCriteria(string selectedFilter)
        {
            Dictionary<string, string> filterMap = new Dictionary<string, string>
            {
                { "Pending", "TaskStatus = 'Pending'" },
                { "Complete", "TaskStatus = 'Complete'" },
                { "All Categories", "" },
                { "Upcoming Tasks", "EndDate >= GETDATE()" }
            };


            if (selectedFilter.StartsWith("Category: "))
            {
                return $"AND CategoryID IN(SELECT CategoryID FROM tblCategory WHERE CategoryName = '{selectedFilter.Replace("Category: ", "")}')";
            }

            return filterMap.ContainsKey(selectedFilter) ? filterMap[selectedFilter] : "";
        }

        private void LoadTasks()
        {
            SetDataSource();
        }

        private void dgvTasks_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvTasks.Columns["IsArchived"].Index && e.RowIndex >= 0)
            {
                bool isArchived = !(bool)dgvTasks.Rows[e.RowIndex].Cells["IsArchived"].Value;

                int taskID = Convert.ToInt32(dgvTasks.Rows[e.RowIndex].Cells["TaskID"].Value);
                UpdateArchiveStatus(taskID, isArchived);

                dgvTasks.Rows[e.RowIndex].Cells["IsArchived"].Value = isArchived;
            }
        }

        private void UpdateArchiveStatus(int taskID, bool isArchived)
        {
            try
            {
                using (SqlConnection updateConnection = new SqlConnection(@"Data Source=DSMARI;Initial Catalog=StudySyncDB;Integrated Security=True"))
                {
                    updateConnection.Open();

                    string query = "UPDATE tblTask SET IsArchived = @IsArchived WHERE TaskID = @TaskID";
                    using (SqlCommand cmd = new SqlCommand(query, updateConnection))
                    {
                        cmd.Parameters.AddWithValue("@IsArchived", isArchived);
                        cmd.Parameters.AddWithValue("@TaskID", taskID);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            SetDataSource();
                        }
                        else
                        {
                            MessageBox.Show("No rows were affected. Archive status not updated.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating archive status: " + ex.Message);
            }
        }

        private void btnViewArchived_Click(object sender, EventArgs e)
        {
            OpenArchivedTasksForm();

        }

        private void OpenArchivedTasksForm()
        {
            archivedTasksForm = new frmArchivedTasks(loggedInUserID);
            archivedTasksForm.StartPosition = FormStartPosition.CenterScreen;
            archivedTasksForm.TaskUnarchived += ArchivedTasksForm_TaskUnarchived;
            archivedTasksForm.Show();
        }

        private void ArchivedTasksForm_TaskUnarchived(object sender, EventArgs e)
        {
            dgvTasks.DataSource = RetrieveTasksForLoggedInUser(loggedInUserID);
        }

        private void dgvTasks_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dgvTasks.CurrentCell.ColumnIndex == dgvTasks.Columns["TaskStatus"].Index && e.Control is ComboBox)
            {
                ((ComboBox)e.Control).SelectedValueChanged += ComboBoxTaskStatus_SelectedValueChanged;
            }
        }

        private void ComboBoxTaskStatus_SelectedValueChanged(object sender, EventArgs e)
        {
            string newStatus = ((ComboBox)sender).Text;

            DataGridViewCell cell = dgvTasks.CurrentCell;

            if (cell != null && cell.ColumnIndex == dgvTasks.Columns["TaskStatus"].Index)
            {
                int taskID = Convert.ToInt32(dgvTasks.Rows[cell.RowIndex].Cells["TaskID"].Value);

                UpdateTaskStatusInDatabase(taskID, newStatus);
            }
        }


        private void UpdateTaskStatusInDatabase(int taskID, string newStatus)
        {
            try
            {
                using (SqlConnection updateConnection = new SqlConnection(@"Data Source=DSMARI;Initial Catalog=StudySyncDB;Integrated Security=True"))
                {
                    updateConnection.Open();

                    string query = "UPDATE tblTask SET TaskStatus = @TaskStatus WHERE TaskID = @TaskID";
                    using (SqlCommand cmd = new SqlCommand(query, updateConnection))
                    {
                        cmd.Parameters.AddWithValue("@TaskStatus", newStatus);
                        cmd.Parameters.AddWithValue("@TaskID", taskID);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("TaskStatus updated successfully!");
                        }
                        else
                        {
                            MessageBox.Show("No rows were affected. TaskStatus not updated.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating TaskStatus: " + ex.Message);
            }
        }

    }
}
