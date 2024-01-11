using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace StudySyncSystem
{
    public partial class frmEditFile : Form
    {
        
        private int fileID;
        private SqlConnection connect = new SqlConnection(@"Data Source=DSMARI;Initial Catalog=StudySyncDB;Integrated Security=True");

        public event EventHandler DataSaved;

        public frmEditFile(int fileID)
        {
            InitializeComponent();
            this.fileID = fileID;
            LoadFileData();
        }

        private void LoadFileData()
        {
            try
            {
                if (connect.State != ConnectionState.Open)
                {
                    connect.Open();
                }

                string query = "SELECT * FROM tblFile WHERE FileID = @FileID";

                using (SqlCommand cmd = new SqlCommand(query, connect))
                {
                    cmd.Parameters.AddWithValue("@FileID", fileID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtFile.Text = reader["FileName"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading file data: " + ex.Message);
            }
            finally
            {
                if (connect.State == ConnectionState.Open)
                {
                    connect.Close();
                }
            }
        }

        private void OnDataSaved()
        {
            DataSaved?.Invoke(this, EventArgs.Empty);
            Close();
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection updateConnection = new SqlConnection(@"Data Source=DSMARI;Initial Catalog=StudySyncDB;Integrated Security=True"))
                {
                    updateConnection.Open();

                    string updateQuery = "UPDATE tblFile SET FileName = @FileName WHERE FileID = @FileID";

                    using (SqlCommand cmd = new SqlCommand(updateQuery, updateConnection))
                    {
                        cmd.Parameters.AddWithValue("@FileID", fileID);
                        cmd.Parameters.AddWithValue("@FileName", txtFile.Text);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("File updated successfully!");
                        OnDataSaved();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating file: " + ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
