using System;
using System.Windows.Forms;

namespace StudySyncSystem
{
    public partial class frmCalculator : Form
    {
        public frmCalculator()
        {
            InitializeComponent();
        }

        private string option;
        private decimal num1, num2, result;

        private void btn0_Click(object sender, EventArgs e)
        {
            AppendToDisplay("0");
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            AppendToDisplay("1");
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            AppendToDisplay("2");
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            AppendToDisplay("3");
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            AppendToDisplay("4");
        }

        private void btn5_Click(object sender, EventArgs e)
        {
            AppendToDisplay("5");
        }

        private void btn6_Click(object sender, EventArgs e)
        {
            AppendToDisplay("6");
        }

        private void btn7_Click(object sender, EventArgs e)
        {
            AppendToDisplay("7");
        }

        private void btn8_Click(object sender, EventArgs e)
        {
            AppendToDisplay("8");
        }

        private void btn9_Click(object sender, EventArgs e)
        {
            AppendToDisplay("9");
        }

        private void btnMultiply_Click(object sender, EventArgs e)
        {
            SetOperator("*");
        }

        private void btnSubtract_Click(object sender, EventArgs e)
        {
            SetOperator("-");
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            SetOperator("+");
        }

        private void btnDivide_Click(object sender, EventArgs e)
        {
            SetOperator("/");
        }

        private void btnEqual_Click(object sender, EventArgs e)
        {
            CalculateResult();
        }

        private void btnDec_Click(object sender, EventArgs e)
        {
            if (!txtDisplay.Text.Contains("."))
            {
                AppendToDisplay(".");
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearCalculator();
        }

        private void AppendToDisplay(string value)
        {
            txtDisplay.Text += value;
        }

        private void SetOperator(string operatorValue)
        {
            option = operatorValue;
            num1 = decimal.Parse(txtDisplay.Text);
            txtDisplay.Clear();
        }

        private void CalculateResult()
        {
            if (decimal.TryParse(txtDisplay.Text, out num2))
            {
                switch (option)
                {
                    case "+":
                        result = num1 + num2;
                        break;
                    case "-":
                        result = num1 - num2;
                        break;
                    case "*":
                        result = num1 * num2;
                        break;
                    case "/":
                        if (num2 != 0)
                        {
                            result = num1 / num2;
                        }
                        else
                        {
                            MessageBox.Show("Cannot divide by zero.");
                            ClearCalculator();
                            return;
                        }
                        break;
                }

                txtDisplay.Text = result.ToString();
            }
            else
            {
                MessageBox.Show("Invalid input for the second number.");
            }
        }

        private void ClearCalculator()
        {
            txtDisplay.Clear();
            result = 0;
            num1 = 0;
            num2 = 0;
            option = null;
        }
    }
}
