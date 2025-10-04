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
using System.Xml;
using System.Xml.Linq;

namespace dbms
{
    public partial class BatchImportReceiptsForm : Form
    {
        private int currentUserID;
        private string currentUsername;

        public BatchImportReceiptsForm(int userID, string username)
        {
            currentUserID = userID;
            currentUsername = username;
            InitializeComponent();
            InitializeData();
        }

        private void InitializeData()
        {
            lblUserInfo.Text = $"Người thực hiện: {currentUsername}";
            
            // Set default XML template
            txtXMLData.Text = @"<Receipts>
  <Receipt>
    <ReceiptNo>1</ReceiptNo>
    <SupplierID>1</SupplierID>
    <UserID>" + currentUserID + @"</UserID>
    <Notes>Nhập hàng tháng 10</Notes>
    <Lines>
      <Line>
        <ProductID>1</ProductID>
        <Quantity>100</Quantity>
        <ImportPrice>50000</ImportPrice>
      </Line>
      <Line>
        <ProductID>2</ProductID>
        <Quantity>50</Quantity>
        <ImportPrice>75000</ImportPrice>
      </Line>
    </Lines>
  </Receipt>
  <Receipt>
    <ReceiptNo>2</ReceiptNo>
    <SupplierID>2</SupplierID>
    <UserID>" + currentUserID + @"</UserID>
    <Notes>Nhập hàng tháng 10</Notes>
    <Lines>
      <Line>
        <ProductID>3</ProductID>
        <Quantity>200</Quantity>
        <ImportPrice>30000</ImportPrice>
      </Line>
    </Lines>
  </Receipt>
</Receipts>";
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                string xmlData = txtXMLData.Text.Trim();

                if (string.IsNullOrEmpty(xmlData))
                {
                    ErrorHandler.ShowWarning("Vui lòng nhập dữ liệu XML!", "Thiếu dữ liệu");
                    return;
                }

                // Validate XML
                try
                {
                    XDocument.Parse(xmlData);
                }
                catch (XmlException xmlEx)
                {
                    ErrorHandler.ShowWarning($"XML không hợp lệ!\n\nLỗi: {xmlEx.Message}", "Lỗi XML");
                    return;
                }

                var result = ErrorHandler.ShowConfirmation(
                    "Bạn có chắc muốn thực hiện batch import?\n\nHệ thống sẽ tạo nhiều phiếu nhập cùng lúc theo dữ liệu XML.",
                    "Xác nhận batch import");

                if (!result) return;

                // Gọi stored procedure sp_BatchImportReceipts
                using (var connection = new SqlConnection(Properties.Settings.Default.QLNhapHangConnectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand("sp_BatchImportReceipts", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 120; // 2 minutes timeout cho batch import
                        
                        // XML parameter
                        var xmlParam = new SqlParameter("@ReceiptDataXML", SqlDbType.Xml);
                        xmlParam.Value = xmlData;
                        command.Parameters.Add(xmlParam);

                        // Output parameters
                        var processedParam = new SqlParameter("@ProcessedCount", SqlDbType.Int);
                        processedParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(processedParam);

                        var errorParam = new SqlParameter("@ErrorCount", SqlDbType.Int);
                        errorParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(errorParam);

                        var messageParam = new SqlParameter("@Message", SqlDbType.NVarChar, -1);
                        messageParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(messageParam);

                        // Execute
                        command.ExecuteNonQuery();

                        // Get results
                        int processedCount = processedParam.Value != DBNull.Value ? (int)processedParam.Value : 0;
                        int errorCount = errorParam.Value != DBNull.Value ? (int)errorParam.Value : 0;
                        string message = messageParam.Value?.ToString() ?? "";

                        // Display results
                        txtResults.Text = $"=== KẾT QUẢ BATCH IMPORT ===\r\n\r\n";
                        txtResults.Text += $"✅ Số phiếu nhập thành công: {processedCount}\r\n";
                        txtResults.Text += $"❌ Số phiếu nhập lỗi: {errorCount}\r\n\r\n";
                        txtResults.Text += $"Chi tiết:\r\n{message}";

                        if (errorCount == 0 && processedCount > 0)
                        {
                            ErrorHandler.ShowSuccess($"Batch import thành công!\n\nĐã tạo {processedCount} phiếu nhập.");
                        }
                        else if (errorCount > 0)
                        {
                            ErrorHandler.ShowWarning(
                                $"Batch import hoàn tất với một số lỗi!\n\n" +
                                $"Thành công: {processedCount}\n" +
                                $"Lỗi: {errorCount}\n\n" +
                                $"Xem chi tiết trong kết quả bên dưới.",
                                "Có lỗi xảy ra");
                        }
                        else
                        {
                            ErrorHandler.ShowWarning("Không có phiếu nhập nào được xử lý!", "Lỗi");
                        }
                    }
                    connection.Close();
                }
            }
            catch (SqlException sqlEx)
            {
                ErrorHandler.HandleSqlError(sqlEx, "batch import phiếu nhập");
                txtResults.Text = $"LỖI SQL:\r\n{sqlEx.Message}";
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralError(ex, "batch import phiếu nhập");
                txtResults.Text = $"LỖI:\r\n{ex.Message}";
            }
        }

        private void btnLoadFromFile_Click(object sender, EventArgs e)
        {
            try
            {
                using (var openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*";
                    openFileDialog.Title = "Chọn file XML để import";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string xmlContent = System.IO.File.ReadAllText(openFileDialog.FileName);
                        txtXMLData.Text = xmlContent;
                        ErrorHandler.ShowSuccess($"Đã tải file: {System.IO.Path.GetFileName(openFileDialog.FileName)}");
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralError(ex, "tải file XML");
            }
        }

        private void btnSaveTemplate_Click(object sender, EventArgs e)
        {
            try
            {
                using (var saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "XML Files (*.xml)|*.xml";
                    saveFileDialog.Title = "Lưu template XML";
                    saveFileDialog.FileName = "receipt_import_template.xml";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        System.IO.File.WriteAllText(saveFileDialog.FileName, txtXMLData.Text);
                        ErrorHandler.ShowSuccess($"Đã lưu template: {System.IO.Path.GetFileName(saveFileDialog.FileName)}");
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralError(ex, "lưu template XML");
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtXMLData.Clear();
            txtResults.Clear();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            string helpMessage = @"HƯỚNG DẪN SỬ DỤNG BATCH IMPORT

1. CẤU TRÚC XML:
   - <Receipts>: Thẻ gốc chứa nhiều phiếu nhập
   - <Receipt>: Mỗi phiếu nhập
     + ReceiptNo: Số thứ tự (tự đặt, dùng để track)
     + SupplierID: ID nhà cung cấp
     + UserID: ID người tạo (mặc định là user hiện tại)
     + Notes: Ghi chú
     + <Lines>: Danh sách dòng sản phẩm
       * ProductID: ID sản phẩm
       * Quantity: Số lượng
       * ImportPrice: Giá nhập

2. QUY TẮC:
   - XML phải hợp lệ (well-formed)
   - SupplierID và ProductID phải tồn tại trong database
   - Quantity và ImportPrice phải > 0
   - Nếu có lỗi ở 1 phiếu, phiếu đó sẽ bị skip, các phiếu khác vẫn import

3. VÍ DỤ:
   Xem template mẫu trong ô XML Data bên trái

4. THAO TÁC:
   - Tải từ file: Load XML từ file .xml
   - Lưu template: Lưu XML hiện tại thành file
   - Import: Thực hiện batch import
   - Clear: Xóa dữ liệu";

            MessageBox.Show(helpMessage, "Hướng dẫn Batch Import", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
