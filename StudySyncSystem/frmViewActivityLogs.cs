using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace StudySyncSystem
{
    public partial class frmViewActivityLogs : Form
    {
        DataTable activityLog = new DataTable();
        DataTable originalActivityLogTable;

        public frmViewActivityLogs()
        {
            InitializeComponent();
        }


        private void frmViewActivityLogs_Load(object sender, EventArgs e)
        {
            dgvActivityLog.AutoGenerateColumns = false;
            dgvActivityLog.Columns["DateCreated"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss";

            PopulateActivityLogData();
            originalActivityLogTable = activityLog.Copy();
        }

        private void PopulateActivityLogData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source=DSMARI;Initial Catalog=StudySyncDB;Integrated Security=True"))
                {
                    connection.Open();

                    string query = @"
                        SELECT
                            l.LogID,
                            l.LogType,
                            l.UserID,
                            l.DateCreated,
                            CONCAT(
                                COALESCE(n.NoteTitle, ''),
                                COALESCE(t.TaskTitle, ''),
                                COALESCE(f.FileName, '')
                            ) AS Title
                        FROM
                            tblNoteLog l
                            LEFT JOIN tblUser u ON l.UserID = u.UserID
                            LEFT JOIN tblNote n ON l.RelatedID = n.NoteID
                            LEFT JOIN tblTask t ON l.RelatedID = t.TaskID
                            LEFT JOIN tblFile f ON l.RelatedID = f.FileID

                        UNION ALL

                        SELECT
                            l.LogID,
                            l.LogType,
                            l.UserID,
                            l.DateCreated,
                            CONCAT(
                                COALESCE(n.NoteTitle, ''),
                                COALESCE(t.TaskTitle, ''),
                                COALESCE(f.FileName, '')
                            ) AS Title
                        FROM
                            tblTaskLog l
                            LEFT JOIN tblUser u ON l.UserID = u.UserID
                            LEFT JOIN tblNote n ON l.RelatedID = n.NoteID
                            LEFT JOIN tblTask t ON l.RelatedID = t.TaskID
                            LEFT JOIN tblFile f ON l.RelatedID = f.FileID

                        UNION ALL

                        SELECT
                            l.LogID,
                            l.LogType,
                            l.UserID,
                            l.DateCreated,
                            CONCAT(
                                COALESCE(n.NoteTitle, ''),
                                COALESCE(t.TaskTitle, ''),
                                COALESCE(f.FileName, '')
                            ) AS Title
                        FROM
                            tblFileLog l
                            LEFT JOIN tblUser u ON l.UserID = u.UserID
                            LEFT JOIN tblNote n ON l.RelatedID = n.NoteID
                            LEFT JOIN tblTask t ON l.RelatedID = t.TaskID
                            LEFT JOIN tblFile f ON l.RelatedID = f.FileID
                        ORDER BY
                            l.DateCreated DESC;
                    ";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        adapter.Fill(activityLog);
                    }
                }
                dgvActivityLog.DataSource = activityLog;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving activity log data: " + ex.Message);
            }
        }

        private void dgvActivityLog_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value == DBNull.Value)
            {
                e.Value = "";
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();

            if (originalActivityLogTable != null)
            {
                DataRow[] filteredRows = originalActivityLogTable.Select($"LogType LIKE '%{searchTerm}%'");

                DataTable filteredTable = originalActivityLogTable.Clone();
                foreach (DataRow row in filteredRows)
                {
                    filteredTable.ImportRow(row);
                }

                dgvActivityLog.DataSource = filteredTable;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
