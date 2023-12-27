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
            // Set the UserInput property when the user clicks "Confirm"
            UserInput = txtInput.Text;
            DialogResult = DialogResult.OK;
            Close();
        }
    }

}
