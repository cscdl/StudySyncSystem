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

namespace StudySyncSystem
{
    public partial class frmUploadFile : Form
    {
        List<string> listFiles = new List<string>();
        

        public frmUploadFile()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            listFiles.Clear();
            listView.Items.Clear();
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    txtPath.Text = fbd.SelectedPath;
                    foreach (string item in Directory.GetFiles(fbd.SelectedPath))
                    {
                        imageList.Images.Add(System.Drawing.Icon.ExtractAssociatedIcon(item));
                        FileInfo fileInfo = new FileInfo(item);
                        listFiles.Add(fileInfo.FullName);
                        listView.Items.Add(fileInfo.Name, imageList.Images.Count - 1);
                    }
                }
            }
        }

        private void listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView.FocusedItem != null)
            {
                Process.Start(listFiles[listView.FocusedItem.Index]);
            }
        }
    }
}
