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
        private DataTable originalNotesTable;
        private frmArchivedNotes archivedNotesForm;

        public frmViewNotes()
        {
            InitializeComponent();
        }

        public void SetLoggedInUserID(int userID)
        {
            loggedInUserID = userID;
            dgvNotes.DataSource = RetrieveNotesForLoggedInUser(loggedInUserID);
            originalNotesTable = (DataTable)dgvNotes.DataSource;
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

        private void OpenAddNotesForm()
        {
            frmAddNotes addNotesForm = new frmAddNotes();
            addNotesForm.StartPosition = FormStartPosition.CenterScreen;
            addNotesForm.SetLoggedInUserID(loggedInUserID);

            addNotesForm.DataSaved += FrmAddNotes_DataSaved;
            addNotesForm.Show();
        }


        private int GetLoggedInUserID()
        {
            return loggedInUserID;
        }

        private DataTable RetrieveNotesForLoggedInUser(int userID, bool showArchived = false)
        {
            DataTable notesTable = new DataTable();

            try
            {
                connection.Open();
                string query = $"SELECT NoteID, NoteTitle, NoteContent, DateCreated, IsArchived FROM tblNote WHERE UserID = {userID} AND IsArchived = {(showArchived ? 1 : 0)}";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
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
            originalNotesTable = (DataTable)dgvNotes.DataSource;
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
            originalNotesTable = (DataTable)dgvNotes.DataSource;
        }

        private void FrmEditNotes_DataSaved(object sender, EventArgs e)
        {
            dgvNotes.DataSource = RetrieveNotesForLoggedInUser(loggedInUserID);
            originalNotesTable = (DataTable)dgvNotes.DataSource;
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
                    originalNotesTable = (DataTable)dgvNotes.DataSource;
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
            if (originalNotesTable != null)
            {
                DataRow[] filteredRows = originalNotesTable.Select($"NoteTitle LIKE '%{searchTerm}%'");

                DataTable filteredTable = originalNotesTable.Clone();
                foreach (DataRow row in filteredRows)
                {
                    filteredTable.ImportRow(row);
                }

                dgvNotes.DataSource = filteredTable;
            }
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
                dgvNotes.DataSource = originalNotesTable;
            }
        }

        private void dgvNotes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvNotes.Columns["IsArchived"].Index && e.RowIndex >= 0)
            {
                bool isArchived = !(bool)dgvNotes.Rows[e.RowIndex].Cells["IsArchived"].Value;

                int noteID = Convert.ToInt32(dgvNotes.Rows[e.RowIndex].Cells["NoteID"].Value);
                UpdateArchiveStatus(noteID, isArchived);

                dgvNotes.DataSource = RetrieveNotesForLoggedInUser(loggedInUserID);
                originalNotesTable = (DataTable)dgvNotes.DataSource;
            }
        }

        private void UpdateArchiveStatus(int noteID, bool isArchived)
        {
            try
            {
                connection.Open();

                string query = "UPDATE tblNote SET IsArchived = @IsArchived WHERE NoteID = @NoteID";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@IsArchived", isArchived);
                    cmd.Parameters.AddWithValue("@NoteID", noteID);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating archive status: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void btnViewArchived_Click(object sender, EventArgs e)
        {
            OpenArchivedNotesForm();
        }

        private void OpenArchivedNotesForm()
        {
            archivedNotesForm = new frmArchivedNotes(loggedInUserID);
            archivedNotesForm.StartPosition = FormStartPosition.CenterScreen;
            archivedNotesForm.NoteUnarchived += ArchivedNotesForm_NoteUnarchived;
            archivedNotesForm.Show();
        }

        private void ArchivedNotesForm_NoteUnarchived(object sender, EventArgs e)
        {
            dgvNotes.DataSource = RetrieveNotesForLoggedInUser(loggedInUserID);
            originalNotesTable = (DataTable)dgvNotes.DataSource;
        }
    }
}
