using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace StudySyncSystem
{
    public partial class frmAddNotes : Form
    {
        private SqlConnection connect = new SqlConnection(@"Data Source=DSMARI;Initial Catalog=StudySyncDB;Integrated Security=True");
        private int loggedInUserID;
        public event EventHandler DataSaved;


        public frmAddNotes()
        {
            InitializeComponent();
        }

        public int LoggedInUserID
        {
            set { loggedInUserID = value; }
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            string title = txtTitle.Text;
            string content = richTxtNewNote.Text;

            try
            {
                connect.Open();

                DateTime currentDate = DateTime.Now;

                string insertQuery = "INSERT INTO tblNote (NoteTitle, NoteContent, DateCreated, IsArchived, UserID) " +
                                     "VALUES (@NoteTitle, @NoteContent, @DateCreated, 0, @UserID)";
                using (SqlCommand cmd = new SqlCommand(insertQuery, connect))
                {
                    cmd.Parameters.AddWithValue("@NoteTitle", title);
                    cmd.Parameters.AddWithValue("@NoteContent", content);
                    cmd.Parameters.AddWithValue("@DateCreated", currentDate);
                    cmd.Parameters.AddWithValue("@UserID", loggedInUserID);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Note saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    OnDataSaved(EventArgs.Empty);

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving data to the database: " + ex.Message);
            }
            finally
            {
                connect.Close();
            }
        }

        protected virtual void OnDataSaved(EventArgs e)
        {
            DataSaved?.Invoke(this, e);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
