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

        private void DisplayPdfTitles(List<string> filePaths)
        {
            foreach (var filePath in filePaths)
            {
                // Get the file name without extension as the title
                string title = System.IO.Path.GetFileNameWithoutExtension(filePath);

                // Add the title and file type to the existing DataTable
                DataRow row = dTable.NewRow();
                row["PDF Title"] = title;
                row["File Type"] = "PDF"; // Set File Type as PDF for each file
                dTable.Rows.Add(row);
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "PDF Files|*.pdf";
            openFileDialog.Title = "Select a File";
            openFileDialog.Multiselect = true;  // Allow multiple file selection

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                List<string> fileNames = new List<string>(openFileDialog.FileNames);
                DisplayPdfTitles(fileNames);

                MessageBox.Show("Files successfully uploaded!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

    }
}
