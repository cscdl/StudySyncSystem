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
    public partial class frmEditTasks : Form
    {
        private int taskID;
        private SqlConnection connect = new SqlConnection(@"Data Source=DSMARI;Initial Catalog=StudySyncDB;Integrated Security=True");

        public event EventHandler DataSaved;

        public frmEditTasks(int taskID)
        {
            InitializeComponent();
            this.taskID = taskID;
            LoadTaskData();
        }

        private void LoadTaskData()
        {
            try
            {
                if (connect.State != ConnectionState.Open)
                {
                    connect.Open();
                }

                // Retrieve task data based on the taskID
                string query = "SELECT * FROM tblTask WHERE TaskID = @TaskID";
                using (SqlCommand cmd = new SqlCommand(query, connect))
                {
                    cmd.Parameters.AddWithValue("@TaskID", taskID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Load existing task data into controls
                            txtTitle.Text = reader["TaskTitle"].ToString();
                            dtpStartDate.Value = Convert.ToDateTime(reader["StartDate"]);
                            dtpEndDate.Value = Convert.ToDateTime(reader["EndDate"]);

                            // Load category into ComboBox
                            int categoryID = Convert.ToInt32(reader["CategoryID"]);
                            LoadCategories(); // You need to have a method to populate the ComboBox with categories
                            cbCategory.SelectedValue = categoryID;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading task data: " + ex.Message);
            }
            finally
            {
                // Always close the connection in the finally block
                if (connect.State == ConnectionState.Open)
                {
                    connect.Close();
                }
            }
        }


        private void LoadCategories()
        {
            try
            {
                // Use a separate connection for loading categories
                using (SqlConnection categoryConnection = new SqlConnection(@"Data Source=DSMARI;Initial Catalog=StudySyncDB;Integrated Security=True"))
                {
                    categoryConnection.Open();

                    // Retrieve category data from tblCategory
                    string query = "SELECT CategoryID, CategoryName FROM tblCategory";
                    using (SqlCommand cmd = new SqlCommand(query, categoryConnection))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable categoryTable = new DataTable();
                        adapter.Fill(categoryTable);

                        // Display category names in the ComboBox
                        cbCategory.DataSource = categoryTable;
                        cbCategory.DisplayMember = "CategoryName";
                        cbCategory.ValueMember = "CategoryID";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading categories: " + ex.Message);
            }
        }




        private void OnDataSaved()
        {
            DataSaved?.Invoke(this, EventArgs.Empty);
            Close(); // Close the form after saving
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                connect.Open();

                // Update the existing task with new data
                string query = "UPDATE tblTask SET TaskTitle = @TaskTitle, StartDate = @StartDate, EndDate = @EndDate, CategoryID = @CategoryID WHERE TaskID = @TaskID";
                using (SqlCommand cmd = new SqlCommand(query, connect))
                {
                    cmd.Parameters.AddWithValue("@TaskID", taskID);
                    cmd.Parameters.AddWithValue("@TaskTitle", txtTitle.Text);
                    cmd.Parameters.AddWithValue("@StartDate", dtpStartDate.Value);
                    cmd.Parameters.AddWithValue("@EndDate", dtpEndDate.Value);
                    cmd.Parameters.AddWithValue("@CategoryID", Convert.ToInt32(cbCategory.SelectedValue));

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Task updated successfully!");
                    OnDataSaved();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating task: " + ex.Message);
            }
            finally
            {
                connect.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }

}
