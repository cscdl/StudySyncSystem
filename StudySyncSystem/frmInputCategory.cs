using System;
using System.Windows.Forms;

namespace StudySyncSystem
{
    public partial class frmInputCategory : Form
    {
        public string UserInput { get; private set; }

        public frmInputCategory(string prompt)
        {
            InitializeComponent();
            lblPrompt.Text = prompt;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            UserInput = txtInput.Text;

            if (string.IsNullOrWhiteSpace(txtInput.Text))
            {
                MessageBox.Show("Please enter a valid input.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }

}
