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
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải nhà cung cấp: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddLine_Click(object sender, EventArgs e)
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

                if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity <= 0)
                {
                    MessageBox.Show("Vui lòng nhập số lượng hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtQuantity.Focus();
                    return;
                }

                if (!decimal.TryParse(txtImportPrice.Text, out decimal importPrice) || importPrice <= 0)
                {
                    MessageBox.Show("Vui lòng nhập giá nhập hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm dòng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("Vui lòng chọn dòng cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    MessageBox.Show("Vui lòng chọn nhà cung cấp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (receiptLines.Count == 0)
                {
                    MessageBox.Show("Vui lòng thêm ít nhất một dòng sản phẩm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                        command.Parameters.AddWithValue("@ReceiptID", 0).Direction = ParameterDirection.Output;
                        command.Parameters.AddWithValue("@Message", "").Direction = ParameterDirection.Output;

                        var linesParam = new SqlParameter("@ReceiptLines", SqlDbType.Structured);
                        linesParam.TypeName = "dbo.udt_GoodsReceiptLine";
                        linesParam.Value = linesTable;
                        command.Parameters.Add(linesParam);

                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();

                        var newReceiptID = command.Parameters["@ReceiptID"].Value;
                        var message = command.Parameters["@Message"].Value.ToString();
                        
                        MessageBox.Show($"Tạo phiếu nhập thành công!\nReceiptID: {newReceiptID}\n{message}", 
                            "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tạo phiếu nhập: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
