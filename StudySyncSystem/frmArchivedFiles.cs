using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace StudySyncSystem
{
    public partial class frmArchivedFiles : Form
    {
        public event EventHandler FileUnarchived;
        private int loggedInUserID;

        public frmArchivedFiles(int userID)
        {
            InitializeComponent();
            loggedInUserID = userID;
            RetrieveArchivedFiles(userID);
        }

        private void RetrieveArchivedFiles(int userID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source=DSMARI;Initial Catalog=StudySyncDB;Integrated Security=True"))
                {
                    connection.Open();
                    // Updated query to include CategoryID from tblFile
                    string query = $"SELECT FileID, FileName, IsArchived, CategoryID FROM tblFile WHERE UserID = {userID} AND IsArchived = 1";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable archivedFilesTable = new DataTable();
                    adapter.Fill(archivedFilesTable);
                    dgvArchivedFiles.DataSource = archivedFilesTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving archived files: " + ex.Message);
            }
        }

        private void UnarchiveFileInDatabase(int fileID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source=DSMARI;Initial Catalog=StudySyncDB;Integrated Security=True"))
                {
                    connection.Open();
                    string query = "UPDATE tblFile SET IsArchived = 0 WHERE FileID = @FileID";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@FileID", fileID);
                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("File unarchived successfully!");
                    RemoveUnarchivedFileFromGrid(fileID);
                    OnFileUnarchived();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error unarchiving file: " + ex.Message);
            }
        }

        private void RemoveUnarchivedFileFromGrid(int fileID)
        {
            foreach (DataGridViewRow row in dgvArchivedFiles.Rows)
            {
                if (Convert.ToInt32(row.Cells["FileID"].Value) == fileID)
                {
                    dgvArchivedFiles.Rows.Remove(row);
                    break;
                }
            }
        }

        protected virtual void OnFileUnarchived()
        {
            FileUnarchived?.Invoke(this, EventArgs.Empty);
        }

        private int GetColumnIndexByName(DataGridView dataGridView, string columnName)
        {
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                if (column.Name.Equals(columnName, StringComparison.OrdinalIgnoreCase))
                {
                    return column.Index;
                }
            }
            return -1;
        }

        private void dgvArchivedFiles_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dgvArchivedFiles.Columns["Unarchive"].Index)
            {
                int fileIDColumnIndex = GetColumnIndexByName(dgvArchivedFiles, "FileID");
                if (fileIDColumnIndex != -1)
                {
                    int fileID = Convert.ToInt32(dgvArchivedFiles.Rows[e.RowIndex].Cells[fileIDColumnIndex].Value);
                    DialogResult result = MessageBox.Show("Are you sure you want to unarchive this file?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {
                        UnarchiveFileInDatabase(fileID);
                    }
                }
                else
                {
                    MessageBox.Show("Column 'FileID' not found in the DataGridView. Check the column name.");
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
