using System;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

namespace StudySyncSystem
{
    public partial class frmTaskDetails : Form
    {
        private int loggedInUserID;
        private string selectedDate;

        public frmTaskDetails(int userID, string date)
        {
            InitializeComponent();
            loggedInUserID = userID;
            selectedDate = date;
            LoadTaskDetails();
        }

        private void LoadTaskDetails()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source=DSMARI;Initial Catalog=StudySyncDB;Integrated Security=True"))
                {
                    connection.Open();

                    string query = "SELECT TaskTitle, TaskStatus, StartDate, EndDate, DateCreated, IsArchived, CategoryID FROM tblTask WHERE UserID = @UserID AND CONVERT(VARCHAR(10), EndDate, 103) = @SelectedDate";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserID", loggedInUserID);
                        cmd.Parameters.AddWithValue("@SelectedDate", selectedDate);

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable taskDetails = new DataTable();
                        adapter.Fill(taskDetails);

                        dgvTaskDetails.DataSource = taskDetails;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading task details: {ex.Message}");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
