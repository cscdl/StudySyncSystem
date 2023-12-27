using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace StudySyncSystem
{
    public partial class frmAdminViewAllNotes : Form
    {
        private SqlConnection connection = new SqlConnection(@"Data Source=DSMARI;Initial Catalog=StudySyncDB;Integrated Security=True");

        public frmAdminViewAllNotes()
        {
            InitializeComponent();
        }

        private void frmAdminViewAllNotes_Load(object sender, EventArgs e)
        {
            // Set AutoGenerateColumns to false
            dgvAllNotes.AutoGenerateColumns = false;

            // Check if "NoteID" column exists before setting visibility
            if (dgvAllNotes.Columns.Contains("NoteID"))
            {
                dgvAllNotes.Columns["NoteID"].Visible = false;
            }

            // Set AutoSizeMode for other columns
            dgvAllNotes.Columns["NoteTitle"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvAllNotes.Columns["NoteContent"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvAllNotes.Columns["DateCreated"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvAllNotes.Columns["IsArchived"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            // Bind data to the DataGridView
            dgvAllNotes.DataSource = RetrieveAllNotes();
        }

        private DataTable RetrieveAllNotes()
        {
            DataTable notesTable = new DataTable();

            try
            {
                connection.Open();
                // Select all notes from the database
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT NoteID, NoteTitle, NoteContent, DateCreated, IsArchived, UserID FROM tblNote", connection);
                adapter.Fill(notesTable);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving notes from the database: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return notesTable;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        
    }
}
