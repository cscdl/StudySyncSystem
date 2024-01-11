using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace StudySyncSystem
{
    public partial class frmArchivedFiles : Form
    {
        public event EventHandler ItemUnarchived;
        private int loggedInUserID;
        private DataTable originalArchivedItemsTable;

        public frmArchivedFiles(int userID)
        {
            InitializeComponent();
            loggedInUserID = userID;
            dgvArchivedFiles.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;

            RetrieveArchivedItems(userID);
            ItemUnarchived += FrmArchivedFiles_ItemUnarchived;
        }

        private void RetrieveArchivedItems(int userID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source=DSMARI;Initial Catalog=StudySyncDB;Integrated Security=True"))
                {
                    connection.Open();
                    string query = $"SELECT FileID, FileName, FilePath, IsArchived, CategoryID FROM tblFile WHERE UserID = {userID} AND IsArchived = 1";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    originalArchivedItemsTable = new DataTable();
                    adapter.Fill(originalArchivedItemsTable);
                    dgvArchivedFiles.DataSource = originalArchivedItemsTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving archived items: " + ex.Message);
            }
        }

        private void UnarchiveItemInDatabase(int itemID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source=DSMARI;Initial Catalog=StudySyncDB;Integrated Security=True"))
                {
                    connection.Open();
                    string query = "UPDATE tblFile SET IsArchived = 0 WHERE FileID = @ItemID";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@ItemID", itemID);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Item unarchived successfully!");
                    RemoveUnarchivedItemFromGrid(itemID);
                    OnItemUnarchived();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error unarchiving item: " + ex.Message);
            }
        }

        private void FrmArchivedFiles_ItemUnarchived(object sender, EventArgs e)
        {
            RetrieveArchivedItems(loggedInUserID);
        }

        private void RemoveUnarchivedItemFromGrid(int itemID)
        {
            foreach (DataGridViewRow row in dgvArchivedFiles.Rows)
            {
                if (Convert.ToInt32(row.Cells["FileID"].Value) == itemID)
                {
                    dgvArchivedFiles.Rows.Remove(row);
                    break;
                }
            }
        }

        private void OnItemUnarchived()
        {
            ItemUnarchived?.Invoke(this, EventArgs.Empty);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();

            if (originalArchivedItemsTable != null)
            {
                DataRow[] filteredRows = originalArchivedItemsTable.Select($"FileName LIKE '%{searchTerm}%'");

                DataTable filteredTable = originalArchivedItemsTable.Clone();
                foreach (DataRow row in filteredRows)
                {
                    filteredTable.ImportRow(row);
                }

                dgvArchivedFiles.DataSource = filteredTable;
            }
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
                        UnarchiveItemInDatabase(fileID);
                    }
                }
                else
                {
                    MessageBox.Show("Column 'FileID' not found in the DataGridView. Check the column name.");
                }
            }
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
