using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Remoting.Contexts;
using System.Windows.Forms;

namespace StudySyncSystem
{
    public partial class frmAddTasks : Form
    {
        private SqlConnection connection = new SqlConnection(@"Data Source=DSMARI;Initial Catalog=StudySyncDB;Integrated Security=True");
        private int loggedInUserID;
        public event EventHandler DataSaved;

        public frmAddTasks(int loggedInUserID)
        {
            InitializeComponent();
            this.loggedInUserID = loggedInUserID;
        }

        public int LoggedInUserID
        {
            set { loggedInUserID = value; }
        }

        private void frmAddTasks_Load(object sender, EventArgs e)
        {
            LoadCategories();

            dtpStartDate.Value = DateTime.Today;
            dtpEndDate.Value = DateTime.Today.AddDays(1);
        }

        private void LoadCategories()
        {
            try
            {
                connection.Open();

                string query = "SELECT CategoryID, CategoryName FROM tblCategory";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable categoryTable = new DataTable();
                adapter.Fill(categoryTable);

                cbCategory.DataSource = categoryTable;
                cbCategory.DisplayMember = "CategoryName";
                cbCategory.ValueMember = "CategoryID";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading categories: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string title = txtTitle.Text;
            SaveTask();
        }

        private void SaveTask()
        {
            try
            {
                connection.Open();
                DateTime currentDate = DateTime.Now;

                string query = "INSERT INTO tblTask (TaskTitle, TaskStatus, StartDate, EndDate, UserID, DateCreated, IsArchived, CategoryID) " +
                                "VALUES (@TaskTitle, @TaskStatus, @StartDate, @EndDate, @UserID, @DateCreated, 0, @CategoryID); SELECT SCOPE_IDENTITY();";

                int newTaskID;

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@TaskTitle", txtTitle.Text);
                    cmd.Parameters.AddWithValue("@TaskStatus", "Pending");
                    cmd.Parameters.AddWithValue("@UserID", loggedInUserID);
                    cmd.Parameters.AddWithValue("@DateCreated", DateTime.Now);
                    cmd.Parameters.AddWithValue("@StartDate", dtpStartDate.Value);
                    cmd.Parameters.AddWithValue("@EndDate", dtpEndDate.Value);
                    cmd.Parameters.AddWithValue("@CategoryID", cbCategory.SelectedValue);

                    newTaskID = Convert.ToInt32(cmd.ExecuteScalar());

                    MessageBox.Show("Task saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    OnDataSaved(EventArgs.Empty);
                }

                LogActivity("Task", newTaskID);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving data to the database: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void LogActivity(string logType, int relatedID)
        {
            try
            {
                string logQuery = "INSERT INTO tblTaskLog (LogType, UserID, DateCreated, RelatedID) " +
                                  "VALUES (@LogType, @UserID, @DateCreated, @RelatedID)";

                using (SqlCommand logCmd = new SqlCommand(logQuery, connection))
                {
                    logCmd.Parameters.AddWithValue("@LogType", "Task Created");
                    logCmd.Parameters.AddWithValue("@UserID", loggedInUserID);
                    logCmd.Parameters.AddWithValue("@DateCreated", DateTime.Now);
                    logCmd.Parameters.AddWithValue("@RelatedID", relatedID);

                    logCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error logging activity: " + ex.Message);
            }
        }


        protected virtual void OnDataSaved(EventArgs e)
        {
            DataSaved?.Invoke(this, e);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
