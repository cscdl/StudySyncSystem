using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace StudySyncSystem
{
    public partial class frmViewUpcomingTasks : Form
    {
        private int loggedInUserID;
        private DataTable originalUpcomingTasksTable;

        public frmViewUpcomingTasks(int userID)
        {
            InitializeComponent();
            loggedInUserID = userID;
            SetDataSource();
            originalUpcomingTasksTable = (DataTable)dgvUpcomingTasks.DataSource;
            dgvUpcomingTasks.CellFormatting += DgvUpcomingTasks_CellFormatting;

            // Hook up the event handler for the overdue button
            btnOverdue.Click += btnOverdue_Click;
        }

        private void SetDataSource()
        {
            dgvUpcomingTasks.AutoGenerateColumns = false;

            DataTable upcomingTasks = RetrieveUpcomingTasksForLoggedInUser(loggedInUserID);

            if (!upcomingTasks.Columns.Contains("Priority"))
            {
                DataColumn priorityColumn = new DataColumn("Priority", typeof(string));
                upcomingTasks.Columns.Add(priorityColumn);
            }

            // Debugging: Print the number of rows before processing
            Console.WriteLine($"Number of rows before processing: {upcomingTasks.Rows.Count}");

            foreach (DataRow row in upcomingTasks.Rows)
            {
                DateTime endDate = Convert.ToDateTime(row["EndDate"]);
                string taskStatus = row["TaskStatus"].ToString();

                // Separate the conditions for better clarity
                if (taskStatus == "Pending")
                {
                    if (endDate < DateTime.Now)
                    {
                        row["Priority"] = "Overdue";

                        // Add a debugging statement to print task details
                        Console.WriteLine($"Task {row["TaskID"]} is Overdue. End Date: {endDate}, Current Date: {DateTime.Now}");
                    }
                    else if (endDate >= DateTime.Now && endDate < DateTime.Now.AddDays(2))
                    {
                        row["Priority"] = "High";
                    }
                    else
                    {
                        row["Priority"] = "Low";
                    }
                }
            }

            Console.WriteLine($"Number of rows after processing: {upcomingTasks.Rows.Count}");

            foreach (DataRow row in upcomingTasks.Rows)
            {
                Console.WriteLine($"Task {row["TaskID"]}, Priority: {row["Priority"]}");
            }

            SortAndColorDataGridView(upcomingTasks);
        }

        private void btnOverdue_Click(object sender, EventArgs e)
        {
            LoadOverdueTasks();
        }

        private DataTable RetrieveUpcomingTasksForLoggedInUser(int userID)
        {
            DataTable upcomingTasks = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source=DSMARI;Initial Catalog=StudySyncDB;Integrated Security=True"))
                {
                    connection.Open();

                    string query = $"SELECT TaskID, TaskTitle, TaskStatus, StartDate, EndDate, DateCreated, IsArchived, CategoryID FROM tblTask WHERE UserID = @UserID AND TaskStatus = 'Pending' AND EndDate >= GETDATE()";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userID);

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(upcomingTasks);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving upcoming tasks from the logged-in user: {ex.Message}");
            }

            return upcomingTasks;
        }

        private void SortAndColorDataGridView(DataTable table)
        {
            if (table != null)
            {
                table.DefaultView.Sort = "EndDate ASC";  // Sort based on EndDate in ascending order
                DataTable sortedTable = table.DefaultView.ToTable();

                dgvUpcomingTasks.DataSource = sortedTable;

                if (!dgvUpcomingTasks.Columns.Contains("Priority"))
                {
                    DataGridViewTextBoxColumn priorityColumn = new DataGridViewTextBoxColumn();
                    priorityColumn.Name = "Priority";
                    priorityColumn.HeaderText = "Priority";
                    dgvUpcomingTasks.Columns.Add(priorityColumn);
                }
            }
        }

        private void LoadOverdueTasks()
        {
            DataTable overdueTasks = RetrieveOverdueTasksForLoggedInUser(loggedInUserID);

            if (!overdueTasks.Columns.Contains("Priority"))
            {
                DataColumn priorityColumn = new DataColumn("Priority", typeof(string));
                overdueTasks.Columns.Add(priorityColumn);
            }

            foreach (DataRow row in overdueTasks.Rows)
            {
                row["Priority"] = "Overdue";
            }

            dgvUpcomingTasks.DataSource = overdueTasks;
        }

        private DataTable RetrieveOverdueTasksForLoggedInUser(int userID)
        {
            DataTable overdueTasks = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source=DSMARI;Initial Catalog=StudySyncDB;Integrated Security=True"))
                {
                    connection.Open();

                    string query = $"SELECT TaskID, TaskTitle, TaskStatus, StartDate, EndDate, DateCreated, IsArchived, CategoryID FROM tblTask WHERE UserID = @UserID AND TaskStatus = 'Pending' AND EndDate < GETDATE()";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userID);

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(overdueTasks);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving overdue tasks from the logged-in user: {ex.Message}");
            }

            return overdueTasks;
        }

        private void DgvUpcomingTasks_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == dgvUpcomingTasks.Columns["Priority"].Index && e.RowIndex >= 0)
            {
                string columnName = dgvUpcomingTasks.Columns[e.ColumnIndex].Name;
                if (columnName == "Priority")
                {
                    string priority = dgvUpcomingTasks["Priority", e.RowIndex].Value?.ToString();
                    if (priority != null)
                    {
                        Color priorityColor = GetPriorityColor(priority);
                        e.CellStyle.BackColor = priorityColor;
                    }
                }
            }
        }

        private Color GetPriorityColor(string priority)
        {
            switch (priority)
            {
                case "High":
                    return Color.Red;
                case "Overdue":
                    return Color.Orange;
                case "Low":
                    return Color.Yellow;
                default:
                    return Color.White;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
