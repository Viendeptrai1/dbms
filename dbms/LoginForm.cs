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
    public partial class LoginForm : Form
    {
        public string Username { get; private set; }
        public string UserRole { get; private set; }
        public int UserID { get; private set; }

        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ErrorHandler.ShowWarning("Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu!", "Thiếu thông tin");
                return;
            }

            try
            {
                if (ValidateUser(username, password))
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    ErrorHandler.ShowWarning("Tên đăng nhập hoặc mật khẩu không đúng!", "Lỗi đăng nhập");
                    txtPassword.Clear();
                    txtUsername.Focus();
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralError(ex, "kết nối database");
            }
        }

        private bool ValidateUser(string username, string password)
        {
            using (var connection = new SqlConnection(Properties.Settings.Default.QLNhapHangConnectionString))
            {
                connection.Open();
                
                // Kiểm tra thông tin đăng nhập - so sánh plain text password
                string query = @"
                    SELECT u.UserID, u.Username, u.FullName, u.IsActive, r.RoleName
                    FROM dbo.Users u
                    JOIN dbo.UsersRoles ur ON u.UserID = ur.UserID
                    JOIN dbo.Roles r ON ur.RoleID = r.RoleID
                    WHERE u.Username = @Username 
                    AND u.Password = @Password
                    AND u.IsActive = 1";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            UserID = Convert.ToInt32(reader["UserID"]);
                            Username = reader["Username"].ToString();
                            UserRole = reader["RoleName"].ToString();
                            
                            reader.Close();
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnLogin_Click(sender, e);
            }
        }

    }
}
