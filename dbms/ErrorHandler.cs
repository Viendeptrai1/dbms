using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace dbms
{
    /// <summary>
    /// Class xử lý và hiển thị thông báo lỗi từ SQL Server một cách chính xác
    /// </summary>
    public static class ErrorHandler
    {
        /// <summary>
        /// Xử lý SqlException và hiển thị thông báo phù hợp
        /// </summary>
        /// <param name="sqlEx">SqlException từ SQL Server</param>
        /// <param name="context">Ngữ cảnh thao tác (vd: "tạo phiếu nhập", "thêm sản phẩm")</param>
        public static void HandleSqlError(SqlException sqlEx, string context = "thực hiện thao tác")
        {
            string message = sqlEx.Message;
            string title;
            MessageBoxIcon icon;

            // Phân loại thông báo lỗi dựa trên nội dung
            if (message.Contains("Vi phạm quy tắc"))
            {
                title = "Vi phạm quy tắc nghiệp vụ";
                icon = MessageBoxIcon.Warning;
            }
            else if (message.Contains("Cảnh báo"))
            {
                title = "Cảnh báo hệ thống";
                icon = MessageBoxIcon.Warning;
            }
            else if (message.Contains("không được") || message.Contains("không thể"))
            {
                title = "Hành động không được phép";
                icon = MessageBoxIcon.Stop;
            }
            else if (message.Contains("không tồn tại") || message.Contains("không hợp lệ"))
            {
                title = "Dữ liệu không hợp lệ";
                icon = MessageBoxIcon.Error;
            }
            else if (message.Contains("Tồn kho âm"))
            {
                title = "Lỗi tồn kho";
                icon = MessageBoxIcon.Error;
            }
            else if (message.Contains("DDL bị chặn"))
            {
                title = "Bảo vệ cơ sở dữ liệu";
                icon = MessageBoxIcon.Stop;
            }
            else if (message.Contains("đã tồn tại"))
            {
                title = "Dữ liệu trùng lặp";
                icon = MessageBoxIcon.Warning;
            }
            else
            {
                title = $"Lỗi {context}";
                icon = MessageBoxIcon.Error;
                message = $"Lỗi hệ thống: {message}";
            }

            MessageBox.Show(message, title, MessageBoxButtons.OK, icon);
        }

        /// <summary>
        /// Xử lý Exception chung
        /// </summary>
        /// <param name="ex">Exception</param>
        /// <param name="context">Ngữ cảnh thao tác</param>
        public static void HandleGeneralError(Exception ex, string context = "thực hiện thao tác")
        {
            string message = $"Lỗi không xác định khi {context}:\n{ex.Message}";
            MessageBox.Show(message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Hiển thị thông báo thành công
        /// </summary>
        /// <param name="message">Nội dung thông báo</param>
        /// <param name="title">Tiêu đề (mặc định: "Thành công")</param>
        public static void ShowSuccess(string message, string title = "Thành công")
        {
            // Đảm bảo message không null và có độ dài phù hợp
            if (string.IsNullOrEmpty(message))
                message = "Thao tác thành công";
                
            // Thêm line breaks nếu message quá dài
            if (message.Length > 100)
            {
                message = message.Replace(". ", ".\n");
            }
            
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Hiển thị thông báo cảnh báo
        /// </summary>
        /// <param name="message">Nội dung thông báo</param>
        /// <param name="title">Tiêu đề (mặc định: "Cảnh báo")</param>
        public static void ShowWarning(string message, string title = "Cảnh báo")
        {
            // Đảm bảo message không null và có độ dài phù hợp
            if (string.IsNullOrEmpty(message))
                message = "Thông báo trống";
                
            // Thêm line breaks nếu message quá dài
            if (message.Length > 100)
            {
                message = message.Replace(". ", ".\n");
            }
            
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Hiển thị thông báo lỗi
        /// </summary>
        /// <param name="title">Tiêu đề lỗi</param>
        /// <param name="message">Nội dung lỗi</param>
        public static void ShowError(string title, string message)
        {
            // Đảm bảo message không null và có độ dài phù hợp
            if (string.IsNullOrEmpty(message))
                message = "Lỗi không xác định";
                
            // Thêm line breaks nếu message quá dài
            if (message.Length > 100)
            {
                message = message.Replace(". ", ".\n");
            }
            
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Hiển thị hộp thoại xác nhận
        /// </summary>
        /// <param name="message">Nội dung câu hỏi</param>
        /// <param name="title">Tiêu đề (mặc định: "Xác nhận")</param>
        /// <returns>true nếu user chọn Yes, false nếu chọn No</returns>
        public static bool ShowConfirmation(string message, string title = "Xác nhận")
        {
            // Đảm bảo message không null và có độ dài phù hợp
            if (string.IsNullOrEmpty(message))
                message = "Bạn có chắc chắn?";
                
            // Thêm line breaks nếu message quá dài
            if (message.Length > 100)
            {
                message = message.Replace(". ", ".\n");
            }
            
            var result = MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return result == DialogResult.Yes;
        }

        /// <summary>
        /// Xử lý kết quả từ stored procedure với output parameter @Message - CHỈ XỬ LÝ LỖI
        /// </summary>
        /// <param name="resultMessage">Nội dung từ @Message parameter</param>
        /// <param name="context">Ngữ cảnh thao tác</param>
        /// <returns>true nếu thành công, false nếu có lỗi</returns>
        public static bool HandleStoredProcedureResult(string resultMessage, string context = "thực hiện thao tác")
        {
            if (string.IsNullOrEmpty(resultMessage))
            {
                ShowWarning($"Không có thông báo từ hệ thống khi {context}.");
                return false;
            }

            // CHỈ XỬ LÝ LỖI - không tự động hiển thị thành công
            if (resultMessage.Contains("Lỗi") || resultMessage.Contains("lỗi") || 
                resultMessage.Contains("Vi phạm") || resultMessage.Contains("không thể") ||
                resultMessage.Contains("không được") || resultMessage.Contains("không tồn tại") ||
                resultMessage.Contains("Cảnh báo") || resultMessage.Contains("cảnh báo") ||
                resultMessage.Contains("RAISERROR") || resultMessage.Contains("Error") ||
                resultMessage.Contains("Failed") || resultMessage.Contains("Invalid") ||
                resultMessage.Contains("bị từ chối") || resultMessage.Contains("bị hủy") ||
                resultMessage.Contains("không hợp lệ") || resultMessage.Contains("không đúng"))
            {
                // Xử lý như thông báo lỗi từ SQL
                if (resultMessage.Contains("Vi phạm quy tắc"))
                {
                    MessageBox.Show(resultMessage, "Vi phạm quy tắc nghiệp vụ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (resultMessage.Contains("Cảnh báo") || resultMessage.Contains("cảnh báo"))
                {
                    MessageBox.Show(resultMessage, "Cảnh báo hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (resultMessage.Contains("không được") || resultMessage.Contains("không thể") || resultMessage.Contains("bị từ chối"))
                {
                    MessageBox.Show(resultMessage, "Hành động không được phép", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                else if (resultMessage.Contains("không tồn tại") || resultMessage.Contains("không hợp lệ"))
                {
                    MessageBox.Show(resultMessage, "Dữ liệu không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show(resultMessage, $"Lỗi {context}", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return false;
            }

            // Kiểm tra thành công
            if (resultMessage.ToLower().Contains("thành công") || 
                resultMessage.ToLower().Contains("hoàn thành") ||
                resultMessage.ToLower().Contains("hoàn tất") ||
                resultMessage.Contains("Success") || resultMessage.Contains("SUCCESS"))
            {
                return true; // Không hiển thị, để form tự xử lý
            }

            // Nếu không rõ ràng, hiển thị như thông báo thông thường
            ShowWarning($"Thông báo từ hệ thống: {resultMessage}", "Thông báo");
            return false;
        }
    }
}
