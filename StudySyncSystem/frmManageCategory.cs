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

        public frmManageCategory(bool isAdmin)
        {
            InitializeComponent();
            dgvManageCategory.CellFormatting += dgvManageCategory_CellFormatting;

            this.isAdmin = isAdmin;

            // Enable or disable buttons based on the user's role
            btnNew.Enabled = isAdmin;
            btnEdit.Enabled = isAdmin;
            btnDelete.Enabled = isAdmin;

            // Load categories into the DataGridView
            dgvManageCategory.AutoGenerateColumns = false;
            dgvManageCategory.Columns["CategoryID"].Visible = false;
            dgvManageCategory.Columns["CategoryName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            // Call the method to bind data to the DataGridView
            BindCategoriesToDataGridView();
        }

        private void dgvManageCategory_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Set a fixed text color for all cells
            e.CellStyle.ForeColor = Color.Black; // Change the text color to green
        }


        private void BindCategoriesToDataGridView()
        {
            // Retrieve categories from the database and set as DataSource
            dgvManageCategory.DataSource = RetrieveCategoriesFromDatabase();
        }


        private DataTable RetrieveCategoriesFromDatabase()
        {
            DataTable categoriesTable = new DataTable();

            try
            {
                connection.Open();
                // Modify the SQL query to select the desired columns from tblCategory
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
                    // Insert the new category into the database
                    InsertCategoryIntoDatabase(newCategoryName);

                    // Refresh the DataGridView to reflect the changes
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

                // Insert the new category into tblCategory
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
                MessageBox.Show("Error adding new category: " + ex.Message);
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
                // Get the selected category
                DataGridViewRow selectedRow = dgvManageCategory.CurrentRow;

                if (selectedRow != null)
                {
                    int categoryID = Convert.ToInt32(selectedRow.Cells["CategoryID"].Value);
                    string currentCategoryName = Convert.ToString(selectedRow.Cells["CategoryName"].Value);

                    string newCategoryName = PromptForCategoryName($"Enter the new name for the category '{currentCategoryName}':");

                    if (!string.IsNullOrEmpty(newCategoryName))
                    {
                        // Update the category in the database
                        UpdateCategoryInDatabase(categoryID, newCategoryName);

                        // Refresh the DataGridView to reflect the changes
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

                // Update the category in tblCategory
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
                MessageBox.Show("Error updating category: " + ex.Message);
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
                // Get the selected category
                DataGridViewRow selectedRow = dgvManageCategory.CurrentRow;

                if (selectedRow != null)
                {
                    int categoryID = Convert.ToInt32(selectedRow.Cells["CategoryID"].Value);
                    string categoryName = Convert.ToString(selectedRow.Cells["CategoryName"].Value);

                    // Confirm with the user before deleting
                    DialogResult result = MessageBox.Show($"Are you sure you want to delete the category '{categoryName}'?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        // Delete the category from the database
                        DeleteCategoryFromDatabase(categoryID);

                        // Refresh the DataGridView to reflect the changes
                        dgvManageCategory.DataSource = RetrieveCategoriesFromDatabase();
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

                // Delete the category from tblCategory
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
                MessageBox.Show("Error deleting category: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }


        



        // Additional logic for using categories in other parts of your application...
    }
}
