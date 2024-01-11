using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace StudySyncSystem
{
    public partial class frmUploadFile : Form
    {
        private DataTable dTable;
        private int loggedInUserID;
        private int newFileID;
        public event EventHandler FileUploaded;


        public frmUploadFile(int loggedInUserID)
        {
            InitializeComponent();
            InitializeDataTable();

            this.loggedInUserID = loggedInUserID;

            dgvDisplayTextFile.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;


            LoadCategories();
        }

        private void InitializeDataTable()
        {
            dTable = new DataTable();
            dTable.Columns.Add("File Name", typeof(string));
            dTable.Columns.Add("File Type", typeof(string));
            dgvDisplayTextFile.DataSource = dTable;
        }

        private void LoadCategories()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source=DSMARI;Initial Catalog=StudySyncDB;Integrated Security=True"))
                {
                    connection.Open();

                    string query = "SELECT CategoryID, CategoryName FROM tblCategory";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable categoryTable = new DataTable();
                    adapter.Fill(categoryTable);

                    cbCategory.DataSource = categoryTable;
                    cbCategory.DisplayMember = "CategoryName";
                    cbCategory.ValueMember = "CategoryID";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading categories: " + ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "PDF Files|*.pdf|Word Documents|*.doc;*.docx|Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp",
                Title = "Select File(s)",
                Multiselect = true
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (openFileDialog.FileNames.Length > 0)
                {
                    // Clear existing text in txtFile
                    txtFile.Text = "";

                    foreach (var filePath in openFileDialog.FileNames)
                    {
                        string title = Path.GetFileNameWithoutExtension(filePath);

                        txtFile.Text += title + Environment.NewLine;

                        string fileType = openFileDialog.FilterIndex == 1 ? "PDF" : "Word Document : Image";

                        DataRow row = dTable.NewRow();
                        row["File Name"] = title;
                        row["File Type"] = fileType;
                        dTable.Rows.Add(row);

                        InsertFileToDatabase(title, fileType, filePath);
                        LogActivity("File Uploaded", newFileID);
                    }

                    MessageBox.Show("Files successfully uploaded!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No files selected!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }


        private void InsertFileToDatabase(string title, string fileType, string filePath)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source=DSMARI;Initial Catalog=StudySyncDB;Integrated Security=True"))
                {
                    connection.Open();

                    string query = "INSERT INTO tblFile (FileName, FilePath, UserID, DateCreated, IsArchived, CategoryID) " +
                                   "VALUES (@FileName, @FilePath, @UserID, @DateCreated, @IsArchived, @CategoryID); " +
                                   "SELECT SCOPE_IDENTITY();";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@FileName", title);
                        cmd.Parameters.AddWithValue("@FilePath", filePath);
                        cmd.Parameters.AddWithValue("@UserID", loggedInUserID);
                        cmd.Parameters.AddWithValue("@DateCreated", DateTime.Now);
                        cmd.Parameters.AddWithValue("@IsArchived", 0);
                        cmd.Parameters.AddWithValue("@CategoryID", cbCategory.SelectedValue);

                        newFileID = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inserting file to database: " + ex.Message);
            }
        }


        private void LogActivity(string logType, int relatedID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source=DSMARI;Initial Catalog=StudySyncDB;Integrated Security=True"))
                {
                    connection.Open();

                    string logQuery = "INSERT INTO tblFileLog (LogType, UserID, DateCreated, RelatedID) " +
                                      "VALUES (@LogType, @UserID, @DateCreated, @RelatedID)";

                    using (SqlCommand logCmd = new SqlCommand(logQuery, connection))
                    {
                        logCmd.Parameters.AddWithValue("@LogType", logType);
                        logCmd.Parameters.AddWithValue("@UserID", loggedInUserID);
                        logCmd.Parameters.AddWithValue("@DateCreated", DateTime.Now);
                        logCmd.Parameters.AddWithValue("@RelatedID", relatedID);

                        logCmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error logging activity: " + ex.Message);
            }
        }
    }
}
