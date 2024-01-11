using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace StudySyncSystem
{
    public partial class frmManageCategory : Form
    {
        private SqlConnection connection = new SqlConnection(@"Data Source=DSMARI;Initial Catalog=StudySyncDB;Integrated Security=True");
        private bool isAdmin;
        private DataTable originalCategoriesTable;

        public frmManageCategory(bool isAdmin)
        {
            InitializeComponent();
            dgvManageCategory.CellFormatting += dgvManageCategory_CellFormatting;

            this.isAdmin = isAdmin;

            btnNew.Enabled = isAdmin;
            btnEdit.Enabled = isAdmin;
            btnDelete.Enabled = isAdmin;

            dgvManageCategory.AutoGenerateColumns = false;
            dgvManageCategory.Columns["CategoryID"].Visible = false;
            dgvManageCategory.Columns["CategoryName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            BindCategoriesToDataGridView();
        }

        private void dgvManageCategory_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            e.CellStyle.ForeColor = Color.Black;
        }

        private void BindCategoriesToDataGridView()
        {
            originalCategoriesTable = RetrieveCategoriesFromDatabase();
            dgvManageCategory.DataSource = originalCategoriesTable;
        }

        private DataTable RetrieveCategoriesFromDatabase()
        {
            DataTable categoriesTable = new DataTable();

            try
            {
                connection.Open();
                string query = "SELECT CategoryID, CategoryName FROM tblCategory";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                adapter.Fill(categoriesTable);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving categories from the database: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return categoriesTable;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            if (isAdmin)
            {
                string newCategoryName = PromptForCategoryName("Enter the new category name:");

                if (!string.IsNullOrEmpty(newCategoryName))
                {
                    InsertCategoryIntoDatabase(newCategoryName);

                    BindCategoriesToDataGridView();
                }
            }
            else
            {
                MessageBox.Show("You don't have permission to add a new category.");
            }
        }

        private string PromptForCategoryName(string prompt)
        {
            using (var inputForm = new frmInputCategory(prompt))
            {
                if (inputForm.ShowDialog() == DialogResult.OK)
                {
                    return inputForm.UserInput;
                }
            }

            return string.Empty;
        }

        private void InsertCategoryIntoDatabase(string categoryName)
        {
            try
            {
                connection.Open();

                string query = "INSERT INTO tblCategory (CategoryName) VALUES (@CategoryName)";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@CategoryName", categoryName);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("New category added successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding a new category: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (isAdmin)
            {
                DataGridViewRow selectedRow = dgvManageCategory.CurrentRow;

                if (selectedRow != null)
                {
                    int categoryID = Convert.ToInt32(selectedRow.Cells["CategoryID"].Value);
                    string currentCategoryName = Convert.ToString(selectedRow.Cells["CategoryName"].Value);

                    string newCategoryName = PromptForCategoryName($"Enter the new name for the category '{currentCategoryName}':");

                    if (!string.IsNullOrEmpty(newCategoryName))
                    {
                        UpdateCategoryInDatabase(categoryID, newCategoryName);

                        BindCategoriesToDataGridView();
                    }
                }
                else
                {
                    MessageBox.Show("Please select a category to edit.");
                }
            }
            else
            {
                MessageBox.Show("You don't have permission to edit a category.");
            }
        }

        private void UpdateCategoryInDatabase(int categoryID, string newCategoryName)
        {
            try
            {
                connection.Open();

                string query = "UPDATE tblCategory SET CategoryName = @CategoryName WHERE CategoryID = @CategoryID";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@CategoryID", categoryID);
                    cmd.Parameters.AddWithValue("@CategoryName", newCategoryName);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Category updated successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating the category: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (isAdmin)
            {
                DataGridViewRow selectedRow = dgvManageCategory.CurrentRow;

                if (selectedRow != null)
                {
                    int categoryID = Convert.ToInt32(selectedRow.Cells["CategoryID"].Value);
                    string categoryName = Convert.ToString(selectedRow.Cells["CategoryName"].Value);

                    DialogResult result = MessageBox.Show($"Are you sure you want to delete the category '{categoryName}'?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        DeleteCategoryFromDatabase(categoryID);

                        BindCategoriesToDataGridView();
                    }
                }
                else
                {
                    MessageBox.Show("Please select a category to delete.");
                }
            }
            else
            {
                MessageBox.Show("You don't have permission to delete a category.");
            }
        }

        private void DeleteCategoryFromDatabase(int categoryID)
        {
            try
            {
                connection.Open();

                string query = "DELETE FROM tblCategory WHERE CategoryID = @CategoryID";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@CategoryID", categoryID);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Category deleted successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting the category: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();

            if (originalCategoriesTable != null)
            {
                DataRow[] filteredRows = originalCategoriesTable.Select($"CategoryName LIKE '%{searchTerm}%'");

                DataTable filteredTable = originalCategoriesTable.Clone();
                foreach (DataRow row in filteredRows)
                {
                    filteredTable.ImportRow(row);
                }

                dgvManageCategory.DataSource = filteredTable;
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
