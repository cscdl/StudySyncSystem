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

            SetDataSource();

            dgvViewFiles.CellContentClick += dgvViewFiles_CellContentClick;
            dgvViewFiles.EditingControlShowing += dgvViewFiles_EditingControlShowing;

            
        }

        private void SetDataSource()
        {
            dgvViewFiles.DataSource = RetrieveFilesForLoggedInUser(loggedInUserID);
        }

        private DataTable RetrieveFilesForLoggedInUser(int userID, bool showArchived = false)
        {
            DataTable todoListFiles = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source=DSMARI;Initial Catalog=StudySyncDB;Integrated Security=True"))
                {
                    connection.Open();

                    string query = $"SELECT FileID, FileName, FilePath FROM tblFile WHERE UserID = @UserID AND IsArchived = {(showArchived ? 1 : 0)}";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userID);

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(todoListFiles);
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
                ToggleArchiveStatus("FileID", dgvViewFiles);
            }
        }

        private void ToggleArchiveStatus(string idColumnName, DataGridView dataGridView)
        {
            DataGridViewRow selectedRow = dataGridView.Rows[dataGridView.CurrentCell.RowIndex];

            bool isArchived = !(bool)selectedRow.Cells["IsArchived"].Value;
            int id = Convert.ToInt32(selectedRow.Cells[idColumnName].Value);

            UpdateArchiveStatusInDatabase(id, isArchived);

            selectedRow.Cells["IsArchived"].Value = isArchived;
        }

        private void UpdateArchiveStatusInDatabase(int id, bool isArchived)
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
                        cmd.Parameters.AddWithValue("@FileID", id);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            SetDataSource();
                        }
                        else
                        {
                            MessageBox.Show("No rows were affected. Archive status not updated.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating file archive status: " + ex.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchCriteria = txtSearch.Text.Trim();
            dgvViewFiles.DataSource = RetrieveFilesForLoggedInUser(loggedInUserID);
        }

        private void btnViewArchived_Click(object sender, EventArgs e)
        {
            OpenArchivedFilesForm();
        }

        private void OpenArchivedFilesForm()
        {
            archivedFilesForm = new frmArchivedFiles(loggedInUserID);
            archivedFilesForm.StartPosition = FormStartPosition.CenterScreen;
            archivedFilesForm.Show();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dgvViewFiles_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dgvViewFiles.CurrentCell.ColumnIndex == dgvViewFiles.Columns["IsArchived"].Index && e.Control is ComboBox)
            {
                ((ComboBox)e.Control).SelectedValueChanged += ComboBoxIsArchived_SelectedValueChanged;
            }
        }

        private void ComboBoxIsArchived_SelectedValueChanged(object sender, EventArgs e)
        {
            ToggleArchiveStatus("FileID", dgvViewFiles);
        }
    }
}
