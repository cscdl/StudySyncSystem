using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace StudySyncSystem
{
    public partial class frmArchivedNotes : Form
    {
        public event EventHandler NoteUnarchived;
        private int loggedInUserID;

        public frmArchivedNotes(int userID)
        {
            InitializeComponent();
            loggedInUserID = userID;

            RetrieveArchivedNotes();
        }

        private void RetrieveArchivedNotes()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source=DSMARI;Initial Catalog=StudySyncDB;Integrated Security=True"))
                {
                    connection.Open();

                    string query = $"SELECT NoteID, NoteTitle, NoteContent, DateCreated, IsArchived FROM tblNote WHERE UserID = {loggedInUserID} AND IsArchived = 1";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);

                    DataTable archivedNotesTable = new DataTable();
                    adapter.Fill(archivedNotesTable);

                    dgvArchivedNotes.DataSource = archivedNotesTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving archived notes: " + ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dgvArchivedNotes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dgvArchivedNotes.Columns["Unarchive"].Index)
            {
                int noteIDColumnIndex = GetColumnIndexByName(dgvArchivedNotes, "NoteID");

                if (noteIDColumnIndex != -1)
                {
                    int noteID = Convert.ToInt32(dgvArchivedNotes.Rows[e.RowIndex].Cells[noteIDColumnIndex].Value);

                    DialogResult result = MessageBox.Show("Are you sure you want to unarchive this note?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        UnarchiveNoteInDatabase(noteID);
                        RetrieveArchivedNotes();
                    }
                }
                else
                {
                    MessageBox.Show("Column 'NoteID' not found in the DataGridView. Check the column name.");
                }
            }
        }

        private void UnarchiveNoteInDatabase(int noteID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source=DSMARI;Initial Catalog=StudySyncDB;Integrated Security=True"))
                {
                    connection.Open();

                    string query = "UPDATE tblNote SET IsArchived = 0 WHERE NoteID = @NoteID";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@NoteID", noteID);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Note unarchived successfully!");

                    OnNoteUnarchived();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error unarchiving note: " + ex.Message);
            }
        }

        protected virtual void OnNoteUnarchived()
        {
            NoteUnarchived?.Invoke(this, EventArgs.Empty);
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
    }
}
