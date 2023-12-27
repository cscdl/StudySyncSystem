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
        public frmViewTask()
        {
            InitializeComponent();
        }

        public void SetLoggedInUserID(int userID)
        {
            loggedInUserID = userID;
            dgvTasks.DataSource = RetrieveTasksForLoggedInUser(loggedInUserID);
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

            dgvTasks.DataSource = RetrieveTasksForLoggedInUser(loggedInUserID);
        }

        private int GetLoggedInUserID()
        {
            return loggedInUserID;
        }

        private void OpenAddTasksForm()
        {
            int userID = GetLoggedInUserID();
            frmAddTasks addTasksForm = new frmAddTasks();
            addTasksForm.StartPosition = FormStartPosition.CenterScreen;

            // Subscribe to the DataSaved event
            addTasksForm.DataSaved += FrmAddTasks_DataSaved;

            addTasksForm.Show();
        }

        private DataTable RetrieveTasksForLoggedInUser(int userID)
        {
            DataTable todoListTask = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source=DSMARI;Initial Catalog=StudySyncDB;Integrated Security=True"))
                {
                    connection.Open();
                    // Modify the SELECT statement to filter notes by UserID
                    SqlDataAdapter adapter = new SqlDataAdapter($"SELECT TaskID, TaskTitle, TaskStatus, StartDate, EndDate, DateCreated, IsArchived, CategoryID FROM tblTask WHERE UserID = {userID}", connection);
                    adapter.Fill(todoListTask);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving tasks from the logged-in user: " + ex.Message);
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
            // Get the selected note
            DataGridViewRow selectedRow = dgvTasks.CurrentRow;

            if (selectedRow != null)
            {
                int taskID = Convert.ToInt32(selectedRow.Cells["TaskID"].Value);

                // Open the edit form with the selected note
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
            // Subscribe to the DataSaved event
            editTasksForm.DataSaved += FrmEditTasks_DataSaved;

            editTasksForm.ShowDialog();

            // Refresh the DataGridView after editing
            dgvTasks.DataSource = RetrieveTasksForLoggedInUser(loggedInUserID);
        }

        private void FrmEditTasks_DataSaved(object sender, EventArgs e)
        {
            // You can handle any actions after data is saved in the edit form
            // For example, refresh the DataGridView or perform additional tasks
            dgvTasks.DataSource = RetrieveTasksForLoggedInUser(loggedInUserID);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Get the selected note
            DataGridViewRow selectedRow = dgvTasks.CurrentRow;

            if (selectedRow != null)
            {
                int taskID = Convert.ToInt32(selectedRow.Cells["TaskID"].Value);

                // Confirm with the user before deleting
                DialogResult result = MessageBox.Show("Are you sure you want to delete this task?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    // Delete the task from the database
                    DeleteTaskFromDatabase(taskID);

                    // Remove the selected row from the DataGridView
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

                    // Delete the task from the database
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
    }
}
