using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudySyncSystem
{
    public partial class frmLoading : Form
    {
        public frmLoading()
        {
            InitializeComponent();
        }

        private void timerLoading_Tick(object sender, EventArgs e)
        {
            if (progressBar1.Value < 100)
            {
                progressBar1.Value += 1;

                lblPercent.Text = progressBar1.Value.ToString() + "%";
            }
            else
            {
                timerLoading.Stop();

                frmLogin login = new frmLogin();
                login.Show();

                this.Hide();
            }
        }

        private void frmLoading_Load(object sender, EventArgs e)
        {
            timerLoading.Start();
        }
    }
}
