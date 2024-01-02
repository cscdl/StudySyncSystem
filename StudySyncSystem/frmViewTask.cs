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
        private string searchCriteria;
        private string categoryFilter;

        public frmViewTask()
        {
            InitializeComponent();
        }

        public void SetLoggedInUserID(int userID)
        {
            loggedInUserID = userID;
            dgvTasks.DataSource = RetrieveTasksForLoggedInUser(loggedInUserID, searchCriteria, categoryFilter);

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

            dgvTasks.DataSource = RetrieveTasksForLoggedInUser(loggedInUserID, searchCriteria, categoryFilter);
            
            cmbFilter.SelectedIndexChanged += cmbFilter_SelectedIndexChanged;

            LoadTasks();

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

            addTasksForm.DataSaved += FrmAddTasks_DataSaved;

            addTasksForm.Show();
        }

        private DataTable RetrieveTasksForLoggedInUser(int userID, string searchCriteria, string categoryFilter)
        {
            DataTable todoListTask = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source=DSMARI;Initial Catalog=StudySyncDB;Integrated Security=True"))
                {
                    connection.Open();

                    string query = "SELECT TaskID, TaskTitle, TaskStatus, StartDate, EndDate, DateCreated, IsArchived, CategoryID FROM tblTask WHERE UserID = @UserID";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userID);

                        StringBuilder queryBuilder = new StringBuilder(query);

                        if (!string.IsNullOrEmpty(searchCriteria))
                        {
                            queryBuilder.Append("AND(TaskTitle LIKE '%' + @SearchCriteria + '%' OR TaskStatus LIKE '%' + @SearchCriteria + '%')");
                            cmd.Parameters.AddWithValue("@SearchCriteria", searchCriteria);
                        }

                        if (!string.IsNullOrEmpty(categoryFilter) && categoryFilter != "All")
                        {
                            queryBuilder.Append("AND CategoryID IN(SELECT CategoryID FROM tblCategory WHERE CategoryName = @CategoryFilter)");
                            cmd.Parameters.AddWithValue("@CategoryFilter", categoryFilter);
                        }

                        
                        cmd.CommandText = queryBuilder.ToString();

                    
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(todoListTask);
                    }
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
            dgvTasks.DataSource = RetrieveTasksForLoggedInUser(loggedInUserID, searchCriteria, categoryFilter);
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

           
            dgvTasks.DataSource = RetrieveTasksForLoggedInUser(loggedInUserID, searchCriteria, categoryFilter);
        }

        private void FrmEditTasks_DataSaved(object sender, EventArgs e)
        {
            dgvTasks.DataSource = RetrieveTasksForLoggedInUser(loggedInUserID, searchCriteria, categoryFilter);
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

            dgvTasks.DataSource = RetrieveTasksForLoggedInUser(loggedInUserID, searchCriteria, categoryFilter);
        }

        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedFilter = cmbFilter.SelectedItem.ToString();

            string filterCriteria = GetFilterCriteria(selectedFilter);

        
            dgvTasks.DataSource = RetrieveTasksForLoggedInUser(loggedInUserID, searchCriteria, filterCriteria);
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
            dgvTasks.DataSource = RetrieveTasksForLoggedInUser(loggedInUserID, "", "");
        }
    }
}
