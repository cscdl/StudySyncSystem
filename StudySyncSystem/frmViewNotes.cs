using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace StudySyncSystem
{
    public partial class frmViewNotes : Form
    {
        private SqlConnection connection = new SqlConnection(@"Data Source=DSMARI;Initial Catalog=StudySyncDB;Integrated Security=True");
        private int loggedInUserID;

        public frmViewNotes()
        {
            InitializeComponent();
        }

        public void SetLoggedInUserID(int userID)
        {
            loggedInUserID = userID;
            dgvNotes.DataSource = RetrieveNotesForLoggedInUser(loggedInUserID);
        }

        private void frmViewNotes_Load(object sender, EventArgs e)
        {
            dgvNotes.AutoGenerateColumns = false;

            if (dgvNotes.Columns.Contains("NoteID"))
            {
                dgvNotes.Columns["NoteID"].Visible = false;
            }

            dgvNotes.Columns["NoteTitle"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvNotes.Columns["NoteContent"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvNotes.Columns["DateCreated"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvNotes.Columns["IsArchived"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

             dgvNotes.DataSource = RetrieveNotesForLoggedInUser(loggedInUserID);
           
        }


        private int GetLoggedInUserID()
        {
            return loggedInUserID;
        }

        private void OpenAddNotesForm()
        {
            int userID = GetLoggedInUserID();
            frmAddNotes addNotesForm = new frmAddNotes();
            addNotesForm.StartPosition = FormStartPosition.CenterScreen;

            addNotesForm.DataSaved += FrmAddNotes_DataSaved;

            addNotesForm.Show();
        }


        private DataTable RetrieveNotesForLoggedInUser(int userID)
        {
            DataTable notesTable = new DataTable();

            try
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter($"SELECT NoteID, NoteTitle, NoteContent, DateCreated, IsArchived FROM tblNote WHERE UserID = {userID}", connection);
                adapter.Fill(notesTable);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving notes from the logged-in user: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return notesTable;
        }
        private void FrmAddNotes_DataSaved(object sender, EventArgs e)
        {
            dgvNotes.DataSource = RetrieveNotesForLoggedInUser(loggedInUserID);
        }


        private void btnNew_Click(object sender, EventArgs e)
        {
            OpenAddNotesForm();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            DataGridViewRow selectedRow = dgvNotes.CurrentRow;

            if (selectedRow != null)
            {
                int noteID = Convert.ToInt32(selectedRow.Cells["NoteID"].Value);

                OpenEditNotesForm(noteID);
            }
            else
            {
                MessageBox.Show("Please select a note to edit.");
            }
        }


        private void OpenEditNotesForm(int noteID)
        {
            frmEditNotes editNotesForm = new frmEditNotes(noteID);
            editNotesForm.StartPosition = FormStartPosition.CenterScreen;
            editNotesForm.DataSaved += FrmEditNotes_DataSaved;

            editNotesForm.ShowDialog();

            dgvNotes.DataSource = RetrieveNotesForLoggedInUser(loggedInUserID);
        }

        private void FrmEditNotes_DataSaved(object sender, EventArgs e)
        {
            dgvNotes.DataSource = RetrieveNotesForLoggedInUser(loggedInUserID);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DataGridViewRow selectedRow = dgvNotes.CurrentRow;

            if (selectedRow != null)
            {
                int noteID = Convert.ToInt32(selectedRow.Cells["NoteID"].Value);

                DialogResult result = MessageBox.Show("Are you sure you want to delete this note?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    DeleteNoteFromDatabase(noteID);

                    dgvNotes.Rows.Remove(selectedRow);
                }
            }
            else
            {
                MessageBox.Show("Please select a note to delete.");
            }
        }

        private void DeleteNoteFromDatabase(int noteID)
        {
            try
            {
                connection.Open();

                string query = "DELETE FROM tblNote WHERE NoteID = @NoteID";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@NoteID", noteID);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Note deleted successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting note: " + ex.Message);
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


        private void SearchNotes(string searchTerm)
        {
            DataTable notesTable = new DataTable();

            try
            {
                connection.Open();
                string query = $"SELECT NoteID, NoteTitle, NoteContent, DateCreated, IsArchived FROM tblNote WHERE UserID = {loggedInUserID} AND NoteTitle LIKE @SearchTerm";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                adapter.SelectCommand.Parameters.AddWithValue("@SearchTerm", $"%{searchTerm}%");
                adapter.Fill(notesTable);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error searching notes: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            dgvNotes.DataSource = notesTable;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                SearchNotes(searchTerm);
            }
            else
            {
                dgvNotes.DataSource = RetrieveNotesForLoggedInUser(loggedInUserID);
            }
        }

    }
}