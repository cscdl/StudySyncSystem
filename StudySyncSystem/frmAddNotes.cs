using System;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;
using iTextSharp.text.pdf;
using iTextSharp.text;

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

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(content))
            {
                MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                connect.Open();

                DateTime currentDate = DateTime.Now;

                string insertNoteQuery = "INSERT INTO tblNote (NoteTitle, NoteContent, DateCreated, IsArchived, UserID) " +
                                         "VALUES (@NoteTitle, @NoteContent, @DateCreated, 0, @UserID); SELECT SCOPE_IDENTITY();";
                int newNoteID;

                using (SqlCommand cmd = new SqlCommand(insertNoteQuery, connect))
                {
                    cmd.Parameters.AddWithValue("@NoteTitle", title);
                    cmd.Parameters.AddWithValue("@NoteContent", content);
                    cmd.Parameters.AddWithValue("@DateCreated", currentDate);
                    cmd.Parameters.AddWithValue("@UserID", loggedInUserID);

                    newNoteID = Convert.ToInt32(cmd.ExecuteScalar());
                }

                string insertLogQuery = "INSERT INTO tblNoteLog (LogType, UserID, DateCreated, RelatedID) " +
                                        "VALUES (@LogType, @UserID, @DateCreated, @RelatedID)";
                using (SqlCommand cmd = new SqlCommand(insertLogQuery, connect))
                {
                    cmd.Parameters.AddWithValue("@LogType", "Note Created");
                    cmd.Parameters.AddWithValue("@UserID", loggedInUserID);
                    cmd.Parameters.AddWithValue("@DateCreated", currentDate);
                    cmd.Parameters.AddWithValue("@RelatedID", newNoteID);

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Note saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                OnDataSaved(EventArgs.Empty);

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

        public void SetLoggedInUserID(int userID)
        {
            loggedInUserID = userID;
        }

        private void btnSavePdfFile_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "PDF file|*.pdf", ValidateNames = true })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    iTextSharp.text.Document doc = new iTextSharp.text.Document(PageSize.A4.Rotate());

                    try
                    {
                        PdfWriter.GetInstance(doc, new FileStream(sfd.FileName, FileMode.Create));
                        doc.Open();
                        doc.Add(new iTextSharp.text.Paragraph(richTxtNewNote.Text));

                        MessageBox.Show("PDF file successfully created!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        doc.Close();
                    }
                }
            }
        }
    }
}
