using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace StudySyncSystem
{
    public partial class frmViewFiles : Form
    {
        private SqlConnection connection = new SqlConnection(@"Data Source=DSMARI;Initial Catalog=StudySyncDB;Integrated Security=True");
        private int loggedInUserID;
        private frmArchivedFiles archivedFilesForm;
        private DataTable originalFilesTable;



        public frmViewFiles()
        {
            InitializeComponent();
        }

        public void SetLoggedInUserID(int userID)
        {
            loggedInUserID = userID;
            SetDataSource();
        }

        private void frmViewFiles_Load(object sender, EventArgs e)
        {
            dgvViewFiles.AutoGenerateColumns = false;
            dgvViewFiles.Columns["FileName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvViewFiles.Columns["FilePath"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            if (dgvViewFiles.Columns.Contains("IsArchived"))
            {
                DataGridViewCheckBoxColumn archivedColumn = dgvViewFiles.Columns["IsArchived"] as DataGridViewCheckBoxColumn;
                if (archivedColumn != null)
                {
                    archivedColumn.ReadOnly = true;
                    archivedColumn.DefaultCellStyle.NullValue = false;
                }
            }

            SetDataSource();

            dgvViewFiles.CellContentClick += dgvViewFiles_CellContentClick;
            dgvViewFiles.DataBindingComplete += dgvViewFiles_DataBindingComplete;

        }


        private void SetDataSource()
        {
            originalFilesTable = RetrieveFilesForLoggedInUser(loggedInUserID);
            dgvViewFiles.DataSource = originalFilesTable;
        }


        private DataTable RetrieveFilesForLoggedInUser(int userID, bool showArchived = false)
        {
            DataTable todoListFiles = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source=DSMARI;Initial Catalog=StudySyncDB;Integrated Security=True"))
                {
                    connection.Open();

                    string query = $"SELECT FileID, FileName, FilePath, CategoryID FROM tblFile WHERE UserID = @UserID AND IsArchived = {(showArchived ? 1 : 0)}";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userID);

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(todoListFiles);

                        originalFilesTable = todoListFiles.Copy();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving files from the logged-in user: {ex.Message}");
            }

            return todoListFiles;
        }



        private void FrmUploadFile_FileUploaded(object sender, EventArgs e)
        {
            SetDataSource();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            OpenUploadFileForm();
        }

        private void OpenUploadFileForm()
        {
            frmUploadFile uploadFileForm = new frmUploadFile(loggedInUserID);
            uploadFileForm.StartPosition = FormStartPosition.CenterScreen;

            uploadFileForm.FileUploaded += FrmUploadFile_FileUploaded;

            uploadFileForm.Show();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteSelectedRow("FileID", "FileName", dgvViewFiles);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            EditSelectedRow("FileID", dgvViewFiles);
        }

        private void DeleteSelectedRow(string idColumnName, string nameColumnName, DataGridView dataGridView)
        {
            DataGridViewRow selectedRow = dataGridView.CurrentRow;

            if (selectedRow != null)
            {
                int id = Convert.ToInt32(selectedRow.Cells[idColumnName].Value);
                DialogResult result = MessageBox.Show($"Are you sure you want to delete {nameColumnName}?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    DeleteFromDatabase(id, "tblFile", idColumnName);
                    dataGridView.Rows.Remove(selectedRow);
                }
            }
            else
            {
                MessageBox.Show($"Please select a {nameColumnName.ToLower()} to delete.");
            }
        }

        private void DeleteFromDatabase(int id, string tableName, string idColumnName)
        {
            try
            {
                using (SqlConnection deleteConnection = new SqlConnection(@"Data Source=DSMARI;Initial Catalog=StudySyncDB;Integrated Security=True"))
                {
                    deleteConnection.Open();

                    string query = $"DELETE FROM {tableName} WHERE {idColumnName} = @{idColumnName}";
                    using (SqlCommand cmd = new SqlCommand(query, deleteConnection))
                    {
                        cmd.Parameters.AddWithValue($"@{idColumnName}", id);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show($"{idColumnName} deleted successfully!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting {idColumnName.ToLower()}: {ex.Message}");
            }
        }

        private void EditSelectedRow(string idColumnName, DataGridView dataGridView)
        {
            DataGridViewRow selectedRow = dataGridView.CurrentRow;

            if (selectedRow != null)
            {
                int id = Convert.ToInt32(selectedRow.Cells[idColumnName].Value);
                OpenEditFileForm(id);
            }
            else
            {
                MessageBox.Show($"Please select a {idColumnName.ToLower()} to edit.");
            }
        }

        private void OpenEditFileForm(int fileID)
        {
            frmEditFile editFileForm = new frmEditFile(fileID);
            editFileForm.StartPosition = FormStartPosition.CenterScreen;

            editFileForm.DataSaved += FrmEditFile_FileEdited;

            editFileForm.Show();
        }

        private void FrmEditFile_FileEdited(object sender, EventArgs e)
        {
            SetDataSource();
        }

        private void dgvViewFiles_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvViewFiles.Columns["IsArchived"].Index && e.RowIndex >= 0)
            {
                if (dgvViewFiles.Rows[e.RowIndex].Cells["IsArchived"].Value != null)
                {
                    bool isArchived = !(bool)dgvViewFiles.Rows[e.RowIndex].Cells["IsArchived"].Value;
                    int fileID = Convert.ToInt32(dgvViewFiles.Rows[e.RowIndex].Cells["FileID"].Value);

                    UpdateArchiveStatusInDatabase(fileID, isArchived);

                    dgvViewFiles.Rows[e.RowIndex].Cells["IsArchived"].Value = isArchived;
                }
                else
                {
                    MessageBox.Show("Cell value is null. Unable to toggle archive status.");
                }
            }
            else if (e.ColumnIndex == dgvViewFiles.Columns["FilePath"].Index && e.RowIndex >= 0)
            {
                string filePath = dgvViewFiles.Rows[e.RowIndex].Cells["FilePath"].Value.ToString();

                try
                {
                    System.Diagnostics.Process.Start(filePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error opening file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }



        private void UpdateArchiveStatusInDatabase(int fileID, bool isArchived)
        {
            try
            {
                using (SqlConnection updateConnection = new SqlConnection(@"Data Source=DSMARI;Initial Catalog=StudySyncDB;Integrated Security=True"))
                {
                    updateConnection.Open();

                    string query = "UPDATE tblFile SET IsArchived = @IsArchived WHERE FileID = @FileID";
                    using (SqlCommand cmd = new SqlCommand(query, updateConnection))
                    {
                        cmd.Parameters.AddWithValue("@IsArchived", isArchived);
                        cmd.Parameters.AddWithValue("@FileID", fileID);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            RemoveArchivedFilesFromGrid(fileID);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating file archive status: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RemoveArchivedFilesFromGrid(int fileID)
        {
            foreach (DataGridViewRow row in dgvViewFiles.Rows)
            {
                if (Convert.ToInt32(row.Cells["FileID"].Value) == fileID)
                {
                    dgvViewFiles.Rows.Remove(row);
                    break;
                }
            }
        }


        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchCriteria = txtSearch.Text.Trim();

            if (originalFilesTable != null)
            {
                DataRow[] filteredRows = originalFilesTable.Select($"FileName LIKE '%{searchCriteria}%'");

                DataTable filteredTable = originalFilesTable.Clone();
                foreach (DataRow row in filteredRows)
                {
                    filteredTable.ImportRow(row);
                }

                dgvViewFiles.DataSource = filteredTable;
            }
        }


        private void btnViewArchived_Click(object sender, EventArgs e)
        {
            OpenArchivedFilesForm();
        }


        private void OpenArchivedFilesForm()
        {
            archivedFilesForm = new frmArchivedFiles(loggedInUserID);
            archivedFilesForm.StartPosition = FormStartPosition.CenterScreen;
            archivedFilesForm.ItemUnarchived += ArchivedFilesForm_ItemUnarchived;
            archivedFilesForm.Show();
        }

        private void ArchivedFilesForm_ItemUnarchived(object sender, EventArgs e)
        {
            SetDataSource();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dgvViewFiles_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgvViewFiles.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value == null || cell.Value == DBNull.Value)
                    {
                        if (cell.OwningColumn.Name == "IsArchived")
                        {
                            cell.Value = false;
                        }
                    }
                }
            }
        }
    }
}
