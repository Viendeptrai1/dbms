using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dbms
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // Hiển thị form đăng nhập
            using (var loginForm = new LoginForm())
            {
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    // Đăng nhập thành công, mở form chính tương ứng với vai trò
                    Form mainForm = null;
                    
                    switch (loginForm.UserRole)
                    {
                        case "Admin":
                            mainForm = new AdminMainForm();
                            break;
                        case "Seller":
                            mainForm = new SellerMainForm();
                            break;
                        case "User":
                            mainForm = new ProductSearchForm(); // Form tra cứu cho User
                            break;
                        default:
                            MessageBox.Show("Vai trò không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                    }
                    
                    if (mainForm != null)
                    {
                        // Truyền thông tin user vào form chính
                        if (mainForm is AdminMainForm adminForm)
                        {
                            adminForm.SetUserInfo(loginForm.UserID, loginForm.Username, loginForm.UserRole);
                        }
                        else if (mainForm is SellerMainForm sellerForm)
                        {
                            sellerForm.SetUserInfo(loginForm.UserID, loginForm.Username, loginForm.UserRole);
                        }
                        
                        Application.Run(mainForm);
                    }
                }
                else
                {
                    // Người dùng hủy đăng nhập
                    Application.Exit();
                }
            }
        }
    }
}
