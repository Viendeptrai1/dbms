using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace dbms
{
    public partial class AddProductForm : Form
    {
        public AddProductForm()
        {
            InitializeComponent();
            InitializeData();
        }

        private void InitializeData()
        {
            try
            {
                using (var connection = new SqlConnection(Properties.Settings.Default.QLNhapHangConnectionString))
                {
                    connection.Open();
                    
                    // Load danh mục
                    string categoriesQuery = "SELECT CategoryID, CategoryName FROM dbo.Categories ORDER BY CategoryName";
                    using (var command = new SqlCommand(categoriesQuery, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                clbCategories.Items.Add(reader["CategoryName"].ToString());
                            }
                        }
                    }
                    
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh mục: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate input
                if (string.IsNullOrEmpty(txtSKU.Text.Trim()))
                {
                    MessageBox.Show("Vui lòng nhập SKU!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSKU.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(txtProductName.Text.Trim()))
                {
                    MessageBox.Show("Vui lòng nhập tên sản phẩm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtProductName.Focus();
                    return;
                }

                if (!decimal.TryParse(txtSellingPrice.Text, out decimal sellingPrice) || sellingPrice <= 0)
                {
                    MessageBox.Show("Vui lòng nhập giá bán hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSellingPrice.Focus();
                    return;
                }


                if (clbCategories.CheckedItems.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn ít nhất một danh mục!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Tạo TVP cho categories
                var categoryTable = new DataTable();
                categoryTable.Columns.Add("CategoryName", typeof(string));
                
                foreach (string categoryName in clbCategories.CheckedItems)
                {
                    categoryTable.Rows.Add(categoryName);
                }

                // Gọi stored procedure
                using (var connection = new SqlConnection(Properties.Settings.Default.QLNhapHangConnectionString))
                {
                    using (var command = new SqlCommand("sp_AddProductWithCategories", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@SKU", txtSKU.Text.Trim());
                        command.Parameters.AddWithValue("@ProductName", txtProductName.Text.Trim());
                        command.Parameters.AddWithValue("@SellingPrice", sellingPrice);
                        command.Parameters.AddWithValue("@ProductID", 0).Direction = ParameterDirection.Output;
                        command.Parameters.AddWithValue("@Message", "").Direction = ParameterDirection.Output;

                        var categoryParam = new SqlParameter("@CategoryNames", SqlDbType.Structured);
                        categoryParam.TypeName = "dbo.udt_CategoryNameList";
                        categoryParam.Value = categoryTable;
                        command.Parameters.Add(categoryParam);

                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();

                        var newProductID = command.Parameters["@ProductID"].Value;
                        var message = command.Parameters["@Message"].Value.ToString();
                        MessageBox.Show($"Thêm sản phẩm thành công!\nProductID: {newProductID}\n{message}", 
                            "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
