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
            catch (SqlException sqlEx)
            {
                ErrorHandler.HandleSqlError(sqlEx, "tải danh sách danh mục");
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralError(ex, "tải danh sách danh mục");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate input
                if (string.IsNullOrEmpty(txtSKU.Text.Trim()))
                {
                    ErrorHandler.ShowWarning("Vui lòng nhập SKU!", "⚠️ Thiếu thông tin");
                    txtSKU.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(txtProductName.Text.Trim()))
                {
                    ErrorHandler.ShowWarning("Vui lòng nhập tên sản phẩm!", "⚠️ Thiếu thông tin");
                    txtProductName.Focus();
                    return;
                }

                if (!decimal.TryParse(txtSellingPrice.Text, out decimal sellingPrice) || sellingPrice <= 0)
                {
                    ErrorHandler.ShowWarning("Vui lòng nhập giá bán hợp lệ (> 0)!", "⚠️ Giá bán không hợp lệ");
                    txtSellingPrice.Focus();
                    return;
                }

                if (clbCategories.CheckedItems.Count == 0)
                {
                    ErrorHandler.ShowWarning("Vui lòng chọn ít nhất một danh mục!", "⚠️ Thiếu danh mục");
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
                        
                        // Xử lý kết quả từ stored procedure
                        if (message.ToLower().Contains("thành công"))
                        {
                            ErrorHandler.ShowSuccess($"Thêm sản phẩm thành công!\nMã sản phẩm: {newProductID}");
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                            // Hiển thị lỗi từ stored procedure
                            ErrorHandler.HandleStoredProcedureResult(message, "thêm sản phẩm");
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                ErrorHandler.HandleSqlError(sqlEx, "thêm sản phẩm");
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralError(ex, "thêm sản phẩm");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
