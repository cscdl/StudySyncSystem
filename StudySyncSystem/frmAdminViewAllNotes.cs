using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace StudySyncSystem
{
    public partial class frmAdminViewAllNotes : Form
    {
        private SqlConnection connection = new SqlConnection(@"Data Source=DSMARI;Initial Catalog=StudySyncDB;Integrated Security=True");
        private DataTable originalNotesTable;

        public frmAdminViewAllNotes()
        {
            InitializeComponent();
        }

        private void frmAdminViewAllNotes_Load(object sender, EventArgs e)
        {
            dgvAllNotes.AutoGenerateColumns = false;

            if (dgvAllNotes.Columns.Contains("NoteID"))
            {
                dgvAllNotes.Columns["NoteID"].Visible = false;
            }

            dgvAllNotes.Columns["NoteTitle"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvAllNotes.Columns["DateCreated"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvAllNotes.Columns["UserID"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvAllNotes.Columns["IsArchived"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            originalNotesTable = RetrieveAllNotes();
            dgvAllNotes.DataSource = originalNotesTable; 
        }

        private DataTable RetrieveAllNotes()
        {
            DataTable notesTable = new DataTable();

            try
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT NoteID, NoteTitle, DateCreated, IsArchived, UserID FROM tblNote", connection);
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();

            if (originalNotesTable != null)
            {
                DataRow[] filteredRows = originalNotesTable.Select($"NoteTitle LIKE '%{searchTerm}%'");

                DataTable filteredTable = originalNotesTable.Clone();
                foreach (DataRow row in filteredRows)
                {
                    filteredTable.ImportRow(row);
                }

                dgvAllNotes.DataSource = filteredTable;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
