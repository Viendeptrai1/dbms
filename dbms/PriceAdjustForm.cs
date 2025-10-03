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
    public partial class PriceAdjustForm : Form
    {
        public PriceAdjustForm()
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
                    string categoriesQuery = "SELECT CategoryName FROM dbo.Categories ORDER BY CategoryName";
                    using (var command = new SqlCommand(categoriesQuery, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cmbCategory.Items.Add(reader["CategoryName"].ToString());
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

        private void btnApply_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate input
                if (!decimal.TryParse(txtPercent.Text, out decimal percent))
                {
                    ErrorHandler.ShowWarning("Vui lòng nhập phần trăm hợp lệ!", "⚠️ Phần trăm không hợp lệ");
                    txtPercent.Focus();
                    return;
                }

                if (cmbCategory.SelectedIndex == -1)
                {
                    ErrorHandler.ShowWarning("Vui lòng chọn danh mục!", "⚠️ Thiếu thông tin");
                    return;
                }

                // Lấy danh sách SKU theo category
                var skuTable = new DataTable();
                skuTable.Columns.Add("SKU", typeof(string));

                using (var connection = new SqlConnection(Properties.Settings.Default.QLNhapHangConnectionString))
                {
                    connection.Open();
                    
                    // Lấy SKU của tất cả sản phẩm trong category
                    string skuQuery = @"
                        SELECT p.SKU 
                        FROM dbo.Products p
                        JOIN dbo.ProductCategories pc ON p.ProductID = pc.ProductID
                        JOIN dbo.Categories c ON pc.CategoryID = c.CategoryID
                        WHERE c.CategoryName = @CategoryName";
                    
                    using (var skuCmd = new SqlCommand(skuQuery, connection))
                    {
                        skuCmd.Parameters.AddWithValue("@CategoryName", cmbCategory.SelectedItem.ToString());
                        using (var reader = skuCmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                skuTable.Rows.Add(reader["SKU"].ToString());
                            }
                        }
                    }

                    if (skuTable.Rows.Count == 0)
                    {
                        ErrorHandler.ShowWarning("Không tìm thấy sản phẩm nào trong danh mục này!", "⚠️ Danh mục trống");
                        connection.Close();
                        return;
                    }

                    // Gọi stored procedure
                    using (var command = new SqlCommand("sp_BulkAdjustPriceByPercent", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@PercentAdjustment", percent);
                        command.Parameters.AddWithValue("@Reason", $"Điều chỉnh giá theo danh mục {cmbCategory.SelectedItem}");
                        command.Parameters.AddWithValue("@ChangedBy", 1); // Admin user ID
                        command.Parameters.AddWithValue("@Message", "").Direction = ParameterDirection.Output;

                        var skuParam = new SqlParameter("@SKUList", SqlDbType.Structured);
                        skuParam.TypeName = "dbo.udt_SKUList";
                        skuParam.Value = skuTable;
                        command.Parameters.Add(skuParam);

                        command.ExecuteNonQuery();

                        var message = command.Parameters["@Message"].Value.ToString();
                        
                        // Xử lý kết quả từ stored procedure
                        if (message.ToLower().Contains("thành công"))
                        {
                            ErrorHandler.ShowSuccess(message);
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                            // Hiển thị lỗi từ stored procedure
                            ErrorHandler.HandleStoredProcedureResult(message, "điều chỉnh giá");
                        }
                    }
                    connection.Close();
                }
            }
            catch (SqlException sqlEx)
            {
                ErrorHandler.HandleSqlError(sqlEx, "điều chỉnh giá");
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralError(ex, "điều chỉnh giá");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
