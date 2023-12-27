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
            // Set AutoGenerateColumns to false
            dgvNotes.AutoGenerateColumns = false;

            // Check if "NoteID" column exists before setting visibility
            if (dgvNotes.Columns.Contains("NoteID"))
            {
                dgvNotes.Columns["NoteID"].Visible = false;
            }

            // Set AutoSizeMode for other columns
            dgvNotes.Columns["NoteTitle"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvNotes.Columns["NoteContent"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvNotes.Columns["DateCreated"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvNotes.Columns["IsArchived"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            // Bind data to the DataGridView (optional, depending on when you want to load the data)
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

            // Subscribe to the DataSaved event
            addNotesForm.DataSaved += FrmAddNotes_DataSaved;

            addNotesForm.Show();
        }
        

        private DataTable RetrieveNotesForLoggedInUser(int userID)
        {
            DataTable notesTable = new DataTable();

            try
            {
                connection.Open();
                // Modify the SELECT statement to filter notes by UserID
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
            // Get the selected note
            DataGridViewRow selectedRow = dgvNotes.CurrentRow;

            if (selectedRow != null)
            {
                int noteID = Convert.ToInt32(selectedRow.Cells["NoteID"].Value);

                // Open the edit form with the selected note
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
            // Subscribe to the DataSaved event
            editNotesForm.DataSaved += FrmEditNotes_DataSaved;

            editNotesForm.ShowDialog();

            // Refresh the DataGridView after editing
            dgvNotes.DataSource = RetrieveNotesForLoggedInUser(loggedInUserID);
        }

        private void FrmEditNotes_DataSaved(object sender, EventArgs e)
        {
            // You can handle any actions after data is saved in the edit form
            // For example, refresh the DataGridView or perform additional tasks
            dgvNotes.DataSource = RetrieveNotesForLoggedInUser(loggedInUserID);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Get the selected note
            DataGridViewRow selectedRow = dgvNotes.CurrentRow;

            if (selectedRow != null)
            {
                int noteID = Convert.ToInt32(selectedRow.Cells["NoteID"].Value);

                // Confirm with the user before deleting
                DialogResult result = MessageBox.Show("Are you sure you want to delete this note?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    // Delete the note from the database
                    DeleteNoteFromDatabase(noteID);

                    // Remove the selected row from the DataGridView
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

                // Delete the note from the database
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

    }
}