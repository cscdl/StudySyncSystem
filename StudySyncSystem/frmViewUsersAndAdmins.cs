using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace StudySyncSystem
{
    public partial class frmViewUsersAndAdmins : Form
    {
        private SqlConnection connection = new SqlConnection(@"Data Source=DSMARI;Initial Catalog=StudySyncDB;Integrated Security=True");

        public frmViewUsersAndAdmins()
        {
            InitializeComponent();
        }

        private void frmViewUsersAndAdmins_Load(object sender, EventArgs e)
        {
            // Set AutoSizeMode for other columns
            dgvUsers.Columns["Username"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvUsers.Columns["UserType"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
   

            // Bind data to the DataGridView
            dgvUsers.DataSource = RetrieveUsersFromDatabase();
        }

        private DataTable RetrieveUsersFromDatabase()
        {
            DataTable usersTable = new DataTable();

            try
            {
                connection.Open();
                // Modify the SQL query to select the desired columns from tblUser
                string query = "SELECT UserID, Username, UserType FROM tblUser";

                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                adapter.Fill(usersTable);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving users from the database: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return usersTable;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Get the selected user
            DataGridViewRow selectedRow = dgvUsers.CurrentRow;

            if (selectedRow != null)
            {
                int userID = Convert.ToInt32(selectedRow.Cells["UserID"].Value);

                // Confirm with the user before deleting
                DialogResult result = MessageBox.Show("Are you sure you want to delete this user?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    // Delete the user from the database
                    DeleteUserFromDatabase(userID);

                    // Remove the selected row from the DataGridView
                    dgvUsers.Rows.Remove(selectedRow);
                }
            }
            else
            {
                MessageBox.Show("Please select a user to delete.");
            }
        }

        private void DeleteUserFromDatabase(int userID)
        {
            try
            {
                connection.Open();

                // Delete the user from the database
                string query = "DELETE FROM tblUser WHERE UserID = @UserID";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@UserID", userID);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("User deleted successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting user: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        
    }
}
