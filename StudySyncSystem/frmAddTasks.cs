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

        public frmAddTasks()
        {
            InitializeComponent();
        }

        // Property to set the user ID
        public int LoggedInUserID
        {
            set { loggedInUserID = value; }
        }

        private void frmAddTasks_Load(object sender, EventArgs e)
        {
            // Load categories into the ComboBox
            LoadCategories();

            // Set the default date values or initialize them based on your requirements
            // For example:
            dtpStartDate.Value = DateTime.Today;
            dtpEndDate.Value = DateTime.Today.AddDays(1);
        }

        private void LoadCategories()
        {
            try
            {
                connection.Open();

                // Modify the SQL query to select categories created by the admin
                string query = "SELECT CategoryID, CategoryName FROM tblCategory";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable categoryTable = new DataTable();
                adapter.Fill(categoryTable);

                // Display category names in the ComboBox
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

                // Insert the new task into tblTask
                string query = "INSERT INTO tblTask (TaskTitle, TaskStatus, StartDate, EndDate, UserID, DateCreated, IsArchived, CategoryID) " +
                               "VALUES (@TaskTitle, @TaskStatus, @StartDate, @EndDate, @UserID, @DateCreated, 0, @CategoryID)";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@TaskTitle", txtTitle.Text);
                    // You may need to set TaskStatus, UserID, DateCreated, and IsArchived based on your requirements
                    // For example:
                    cmd.Parameters.AddWithValue("@TaskStatus", "Pending");
                    cmd.Parameters.AddWithValue("@UserID", loggedInUserID); // Get the logged-in user ID
                    cmd.Parameters.AddWithValue("@DateCreated", DateTime.Now);
                    cmd.Parameters.AddWithValue("@StartDate", dtpStartDate.Value); // Get StartDate from the DateTimePicker
                    cmd.Parameters.AddWithValue("@EndDate", dtpEndDate.Value);     // Get EndDate from the DateTimePicker
                    cmd.Parameters.AddWithValue("@CategoryID", cbCategory.SelectedValue);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Task saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    OnDataSaved(EventArgs.Empty);
                }
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
