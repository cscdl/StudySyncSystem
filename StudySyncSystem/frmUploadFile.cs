using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace StudySyncSystem
{
    public partial class frmUploadFile : Form
    {
        // Declare a class-level DataTable
        private DataTable dTable;


        public frmUploadFile()
        {
            InitializeComponent();
            InitializeDataTable();

        }

        // Initialize the DataTable with the required column when the form loads
        private void InitializeDataTable()
        {
            dTable = new DataTable();
            dTable.Columns.Add("PDF Title", typeof(string));
            dTable.Columns.Add("File Type", typeof(string)); // Add File Type column
            dgvDisplayTextFile.DataSource = dTable; // Set the DataTable as the DataGridView's data source
        }

        private void DisplayFileTitles(List<string> filePaths, string fileType)
        {
            foreach (var filePath in filePaths)
            {
                // Get the file name without extension as the title
                string title = System.IO.Path.GetFileNameWithoutExtension(filePath);

                // Add the title and file type to the existing DataTable
                DataRow row = dTable.NewRow();
                row["File Title"] = title;
                row["File Type"] = fileType; // Set File Type based on the fileType parameter
                dTable.Rows.Add(row);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "PDF Files|*.pdf|Word Documents|*.doc;*.docx|Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp",
                Title = "Select File(s)",
                Multiselect = true
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (openFileDialog.FileNames.Length > 0)
                {
                    List<string> fileNames = openFileDialog.FileNames.ToList();
                    string fileType = openFileDialog.FilterIndex == 1 ? "PDF" : "Word Document : Image";
                    DisplayFileTitles(fileNames, fileType);
                    txtFile.Text = string.Join(", ", fileNames.Select(System.IO.Path.GetFileNameWithoutExtension));
                    MessageBox.Show("Files successfully uploaded!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No files selected!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
