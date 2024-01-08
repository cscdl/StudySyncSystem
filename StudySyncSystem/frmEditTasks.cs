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

                string query = "SELECT * FROM tblTask WHERE TaskID = @TaskID";
                using (SqlCommand cmd = new SqlCommand(query, connect))
                {
                    cmd.Parameters.AddWithValue("@TaskID", taskID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtTitle.Text = reader["TaskTitle"].ToString();
                            dtpStartDate.Value = Convert.ToDateTime(reader["StartDate"]);
                            dtpEndDate.Value = Convert.ToDateTime(reader["EndDate"]);

                            int categoryID = Convert.ToInt32(reader["CategoryID"]);
                            LoadCategories(); 
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
                using (SqlConnection categoryConnection = new SqlConnection(@"Data Source=DSMARI;Initial Catalog=StudySyncDB;Integrated Security=True"))
                {
                    categoryConnection.Open();

                    string query = "SELECT CategoryID, CategoryName FROM tblCategory";
                    using (SqlCommand cmd = new SqlCommand(query, categoryConnection))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable categoryTable = new DataTable();
                        adapter.Fill(categoryTable);

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
            Close(); 
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                connect.Open();

               
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
