using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace StudySyncSystem
{
    public partial class frmEditNotes : Form
    {
        private int noteID;
        private SqlConnection connect = new SqlConnection(@"Data Source=DSMARI;Initial Catalog=StudySyncDB;Integrated Security=True");

        public event EventHandler DataSaved;

        public frmEditNotes(int noteID)
        {
            InitializeComponent();
            this.noteID = noteID;
            LoadNoteData();
        }

        private void LoadNoteData()
        {
            try
            {
                connect.Open();

                string query = "SELECT * FROM tblNote WHERE NoteID = @NoteID";
                using (SqlCommand cmd = new SqlCommand(query, connect))
                {
                    cmd.Parameters.AddWithValue("@NoteID", noteID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtTitle.Text = reader["NoteTitle"].ToString();
                            richTxtEditNote.Text = reader["NoteContent"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading note data: " + ex.Message);
            }
            finally
            {
                connect.Close();
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
                if (string.IsNullOrWhiteSpace(txtTitle.Text))
                {
                    MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                connect.Open();

                string query = "UPDATE tblNote SET NoteTitle = @NoteTitle, NoteContent = @NoteContent WHERE NoteID = @NoteID";
                using (SqlCommand cmd = new SqlCommand(query, connect))
                {
                    cmd.Parameters.AddWithValue("@NoteID", noteID);
                    cmd.Parameters.AddWithValue("@NoteTitle", txtTitle.Text);
                    cmd.Parameters.AddWithValue("@NoteContent", richTxtEditNote.Text);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Note updated successfully!");
                    OnDataSaved();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating note: " + ex.Message);
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
