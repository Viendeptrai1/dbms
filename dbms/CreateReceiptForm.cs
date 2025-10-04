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
    public partial class CreateReceiptForm : Form
    {
        private int userID;
        private string username;
        private List<ReceiptLine> receiptLines;

        public CreateReceiptForm(int userID, string username)
        {
            this.userID = userID;
            this.username = username;
            this.receiptLines = new List<ReceiptLine>();
            InitializeComponent();
            InitializeData();
        }

        private void InitializeData()
        {
            lblUserInfo.Text = $"Nhân viên: {username}";
            dtpReceiptDate.Value = DateTime.Now;
            
            // Load suppliers
            try
            {
                using (var connection = new SqlConnection(Properties.Settings.Default.QLNhapHangConnectionString))
                {
                    connection.Open();
                    
                    string suppliersQuery = "SELECT SupplierName FROM dbo.Suppliers ORDER BY SupplierName";
                    using (var command = new SqlCommand(suppliersQuery, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cmbSupplier.Items.Add(reader["SupplierName"].ToString());
                            }
                        }
                    }
                    
                    connection.Close();
                }
            }
            catch (SqlException sqlEx)
            {
                ErrorHandler.HandleSqlError(sqlEx, "tải danh sách nhà cung cấp");
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralError(ex, "tải danh sách nhà cung cấp");
            }
        }

        private void btnAddLine_Click(object sender, EventArgs e)
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

                if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity <= 0)
                {
                    ErrorHandler.ShowWarning("Vui lòng nhập số lượng hợp lệ (> 0)!", "⚠️ Số lượng không hợp lệ");
                    txtQuantity.Focus();
                    return;
                }

                if (!decimal.TryParse(txtImportPrice.Text, out decimal importPrice) || importPrice <= 0)
                {
                    ErrorHandler.ShowWarning("Vui lòng nhập giá nhập hợp lệ (> 0)!", "⚠️ Giá nhập không hợp lệ");
                    txtImportPrice.Focus();
                    return;
                }

                // Thêm dòng vào danh sách
                var line = new ReceiptLine
                {
                    ProductSKU = txtSKU.Text.Trim(),
                    Quantity = quantity,
                    ImportPrice = importPrice
                };

                receiptLines.Add(line);
                RefreshLinesList();
                
                // Clear input
                txtSKU.Clear();
                txtQuantity.Clear();
                txtImportPrice.Clear();
            }
            catch (SqlException sqlEx)
            {
                ErrorHandler.HandleSqlError(sqlEx, "thêm dòng sản phẩm");
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralError(ex, "thêm dòng sản phẩm");
            }
        }

        private void btnRemoveLine_Click(object sender, EventArgs e)
        {
            if (lstLines.SelectedIndex >= 0)
            {
                receiptLines.RemoveAt(lstLines.SelectedIndex);
                RefreshLinesList();
            }
            else
            {
                ErrorHandler.ShowWarning("Vui lòng chọn dòng cần xóa!", "⚠️ Chưa chọn dòng");
            }
        }

        private void RefreshLinesList()
        {
            lstLines.Items.Clear();
            decimal totalAmount = 0;
            
            foreach (var line in receiptLines)
            {
                var itemText = $"{line.ProductSKU} - SL: {line.Quantity} - Giá: {line.ImportPrice:N0}";
                lstLines.Items.Add(itemText);
                totalAmount += line.Quantity * line.ImportPrice;
            }
            
            lblTotalAmount.Text = $"Tổng tiền: {totalAmount:N2} VNĐ";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate input
                if (cmbSupplier.SelectedIndex == -1)
                {
                    ErrorHandler.ShowWarning("Vui lòng chọn nhà cung cấp!", "⚠️ Thiếu thông tin");
                    return;
                }

                if (receiptLines.Count == 0)
                {
                    ErrorHandler.ShowWarning("Vui lòng thêm ít nhất một dòng sản phẩm!", "⚠️ Phiếu nhập trống");
                    return;
                }

                // Lấy SupplierID từ tên nhà cung cấp
                int supplierID;
                using (var connection = new SqlConnection(Properties.Settings.Default.QLNhapHangConnectionString))
                {
                    connection.Open();
                    string supplierQuery = "SELECT SupplierID FROM dbo.Suppliers WHERE SupplierName = @SupplierName";
                    using (var supplierCmd = new SqlCommand(supplierQuery, connection))
                    {
                        supplierCmd.Parameters.AddWithValue("@SupplierName", cmbSupplier.SelectedItem.ToString());
                        supplierID = Convert.ToInt32(supplierCmd.ExecuteScalar());
                    }
                    connection.Close();
                }

                // Tạo TVP cho lines (chỉ cần ProductID, Quantity, ImportPrice)
                var linesTable = new DataTable();
                linesTable.Columns.Add("ProductID", typeof(int));
                linesTable.Columns.Add("Quantity", typeof(int));
                linesTable.Columns.Add("ImportPrice", typeof(decimal));
                
                // Lấy ProductID từ SKU và thêm vào TVP
                using (var connection = new SqlConnection(Properties.Settings.Default.QLNhapHangConnectionString))
                {
                    connection.Open();
                    foreach (var line in receiptLines)
                    {
                        string productQuery = "SELECT ProductID FROM dbo.Products WHERE SKU = @SKU";
                        using (var productCmd = new SqlCommand(productQuery, connection))
                        {
                            productCmd.Parameters.AddWithValue("@SKU", line.ProductSKU);
                            var productID = productCmd.ExecuteScalar();
                            if (productID != null)
                            {
                                linesTable.Rows.Add(Convert.ToInt32(productID), line.Quantity, line.ImportPrice);
                            }
                        }
                    }
                    connection.Close();
                }

                // Gọi stored procedure
                using (var connection = new SqlConnection(Properties.Settings.Default.QLNhapHangConnectionString))
                {
                    using (var command = new SqlCommand("sp_CreateGoodsReceipt", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@SupplierID", supplierID);
                        command.Parameters.AddWithValue("@UserID", userID);
                        command.Parameters.AddWithValue("@Notes", ""); // Không có field txtNotes
                        
                        // Output parameters - PHẢI SET SIZE!
                        var receiptIDParam = command.Parameters.Add("@ReceiptID", SqlDbType.Int);
                        receiptIDParam.Direction = ParameterDirection.Output;
                        
                        var messageParam = command.Parameters.Add("@Message", SqlDbType.NVarChar, 200);
                        messageParam.Direction = ParameterDirection.Output;

                        var linesParam = new SqlParameter("@ReceiptLines", SqlDbType.Structured);
                        linesParam.TypeName = "dbo.udt_GoodsReceiptLine";
                        linesParam.Value = linesTable;
                        command.Parameters.Add(linesParam);

                        connection.Open();
                        
                        // DEBUG: Log trước khi execute
                        MessageBox.Show($"Sẽ gọi SP với:\n" +
                            $"SupplierID: {command.Parameters["@SupplierID"].Value}\n" +
                            $"UserID: {command.Parameters["@UserID"].Value}\n" +
                            $"Lines count: {linesTable.Rows.Count}",
                            "DEBUG: Before Execute", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                        command.ExecuteNonQuery();
                        connection.Close();

                        var receiptIDValue = command.Parameters["@ReceiptID"].Value;
                        var messageValue = command.Parameters["@Message"].Value;
                        
                        // DEBUG: Log sau khi execute
                        MessageBox.Show($"Kết quả từ SP:\n" +
                            $"ReceiptID type: {receiptIDValue?.GetType().Name ?? "null"}\n" +
                            $"ReceiptID value: {receiptIDValue ?? "null"}\n" +
                            $"Message type: {messageValue?.GetType().Name ?? "null"}\n" +
                            $"Message value: [{messageValue ?? "null"}]\n" +
                            $"Message length: {messageValue?.ToString().Length ?? 0}",
                            "DEBUG: After Execute", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                        // Debug: Check if values are null
                        if (receiptIDValue == null || receiptIDValue == DBNull.Value)
                        {
                            MessageBox.Show("Lỗi: Không nhận được ReceiptID từ stored procedure", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        
                        if (messageValue == null || messageValue == DBNull.Value)
                        {
                            MessageBox.Show("Lỗi: Không nhận được Message từ stored procedure", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        
                        var newReceiptID = receiptIDValue;
                        var message = messageValue.ToString();
                        
                        // Xử lý kết quả từ stored procedure
                        if (!string.IsNullOrEmpty(message) && message.ToLower().Contains("thành công"))
                        {
                            ErrorHandler.ShowSuccess($"Tạo phiếu nhập thành công!\nMã phiếu: {newReceiptID}");
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                            // Hiển thị lỗi từ stored procedure với full message
                            MessageBox.Show($"Lỗi tạo phiếu nhập:\n\n{message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                // Hiển thị FULL SQL error
                string fullError = $"SQL Error:\n\n";
                fullError += $"Message: {sqlEx.Message}\n";
                fullError += $"Number: {sqlEx.Number}\n";
                fullError += $"State: {sqlEx.State}\n";
                fullError += $"Class: {sqlEx.Class}\n";
                if (sqlEx.InnerException != null)
                {
                    fullError += $"\nInner Exception: {sqlEx.InnerException.Message}";
                }
                MessageBox.Show(fullError, "SQL Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                // Hiển thị FULL error
                string fullError = $"Error:\n\n";
                fullError += $"Type: {ex.GetType().Name}\n";
                fullError += $"Message: {ex.Message}\n";
                fullError += $"StackTrace:\n{ex.StackTrace}";
                if (ex.InnerException != null)
                {
                    fullError += $"\n\nInner Exception: {ex.InnerException.Message}";
                }
                MessageBox.Show(fullError, "Error Detail", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private class ReceiptLine
        {
            public string ProductSKU { get; set; }
            public int Quantity { get; set; }
            public decimal ImportPrice { get; set; }
        }
    }
}
