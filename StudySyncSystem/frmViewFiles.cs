using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace StudySyncSystem
{
    public partial class frmViewFiles : Form
    {
        private DataTable todoListFiles = new DataTable();
        private int loggedInUserID;
        private int categoryID;

        public frmViewFiles(int userID, int categoryID)
        {
            InitializeComponent();
            this.loggedInUserID = userID;
            this.categoryID = categoryID;
        }

        private void frmViewFiles_Load(object sender, EventArgs e)
        {
            todoListFiles.Columns.Add("File Name");
            todoListFiles.Columns.Add("File Path");

            dgvViewFiles.DataSource = todoListFiles;

            LoadFiles();
        }

        private void LoadFiles()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source=DSMARI;Initial Catalog=StudySyncDB;Integrated Security=True"))
                {
                    connection.Open();

                    string query = $"SELECT FileID, FileName, FilePath FROM tblFile WHERE UserID = @UserID AND CategoryID = @CategoryID AND IsArchived = 0";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserID", loggedInUserID);
                        cmd.Parameters.AddWithValue("@CategoryID", categoryID);

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(todoListFiles);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving files: {ex.Message}");
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            OpenUploadFileForm();
        }

        private void OpenUploadFileForm()
        {
            frmUploadFile uploadFileForm = new frmUploadFile(loggedInUserID, categoryID);
            uploadFileForm.StartPosition = FormStartPosition.CenterScreen;

            uploadFileForm.FileUploaded += UploadFileForm_FileUploaded;

            if (uploadFileForm.ShowDialog() == DialogResult.OK)
            {
                // Refresh the files after a new file is uploaded
                todoListFiles.Clear();
                LoadFiles();
            }
        }

        private void UploadFileForm_FileUploaded(object sender, EventArgs e)
        {
            // Handle any logic you need after a file is uploaded
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            DataGridViewRow selectedRow = dgvViewFiles.CurrentRow;

            if (selectedRow != null)
            {
                int fileID = Convert.ToInt32(selectedRow.Cells["FileID"].Value);
                OpenEditFileForm(fileID);
            }
            else
            {
                MessageBox.Show("Please select a file to edit.");
            }
        }

        private void OpenEditFileForm(int fileID)
        {
            // Implement the logic to open the edit file form, similar to the OpenUploadFileForm method
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DataGridViewRow selectedRow = dgvViewFiles.CurrentRow;

            if (selectedRow != null)
            {
                int fileID = Convert.ToInt32(selectedRow.Cells["FileID"].Value);
                DialogResult result = MessageBox.Show("Are you sure you want to delete this file?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    DeleteFileFromDatabase(fileID);
                    todoListFiles.Rows.Remove(((DataRowView)selectedRow.DataBoundItem).Row); // Remove the row from the DataTable
                }
            }
            else
            {
                MessageBox.Show("Please select a file to delete.");
            }
        }


        private void DeleteFileFromDatabase(int fileID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source=DSMARI;Initial Catalog=StudySyncDB;Integrated Security=True"))
                {
                    connection.Open();

                    string query = "DELETE FROM tblFile WHERE FileID = @FileID";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@FileID", fileID);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting file: {ex.Message}");
            }
        }

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            Close();
        }
    }
}
