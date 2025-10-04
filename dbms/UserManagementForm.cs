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
using System.Security.Cryptography;

namespace dbms
{
    public partial class UserManagementForm : Form
    {
        private int currentUserID;
        private string currentUsername;
        private string currentUserRole;

        public UserManagementForm(int userID, string username, string userRole)
        {
            InitializeComponent();
            currentUserID = userID;
            currentUsername = username;
            currentUserRole = userRole;
            InitializeData();
        }

        private void InitializeData()
        {
            try
            {
                LoadUsers();
                LoadRoles();
                LoadUserRoles();
                
                // Đặt tiêu đề form
                this.Text = $"Quản lý User - {currentUsername} ({currentUserRole})";
            }
            catch (SqlException sqlEx)
            {
                ErrorHandler.HandleSqlError(sqlEx, "khởi tạo dữ liệu");
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralError(ex, "khởi tạo dữ liệu");
            }
        }

        private void LoadUsers()
        {
            try
            {
                using (var connection = new SqlConnection(Properties.Settings.Default.QLNhapHangConnectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand("sp_GetFullUserInfo", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        
                        var adapter = new SqlDataAdapter(command);
                        var dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        
                        dgvUsers.DataSource = dataTable;
                        
                        // Định dạng cột
                        if (dgvUsers.Columns.Count > 0)
                        {
                            if (dgvUsers.Columns.Contains("Status"))
                            {
                                dgvUsers.Columns["Status"].Width = 100;
                            }
                            
                            if (dgvUsers.Columns.Contains("AppRole"))
                                dgvUsers.Columns["AppRole"].Width = 100;
                                
                            if (dgvUsers.Columns.Contains("Username"))
                                dgvUsers.Columns["Username"].Width = 150;
                                
                            if (dgvUsers.Columns.Contains("HasSQLLogin"))
                            {
                                dgvUsers.Columns["HasSQLLogin"].HeaderText = "SQL Login";
                                dgvUsers.Columns["HasSQLLogin"].Width = 80;
                            }
                                
                            if (dgvUsers.Columns.Contains("HasDBUser"))
                            {
                                dgvUsers.Columns["HasDBUser"].HeaderText = "DB User";
                                dgvUsers.Columns["HasDBUser"].Width = 80;
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (SqlException sqlEx)
            {
                ErrorHandler.HandleSqlError(sqlEx, "tải danh sách user");
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralError(ex, "tải danh sách user");
            }
        }

        private void LoadRoles()
        {
            try
            {
                using (var adapter = new QLNhapHangDataSetTableAdapters.RolesTableAdapter())
                {
                    var data = adapter.GetData();
                    cmbRoles.DataSource = data;
                    cmbRoles.DisplayMember = "RoleName";
                    cmbRoles.ValueMember = "RoleID";
                }
            }
            catch (SqlException sqlEx)
            {
                ErrorHandler.HandleSqlError(sqlEx, "tải danh sách roles");
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralError(ex, "tải danh sách roles");
            }
        }

        private void LoadUserRoles()
        {
            try
            {
                using (var adapter = new QLNhapHangDataSetTableAdapters.UsersRolesTableAdapter())
                {
                    var data = adapter.GetData();
                    dgvUserRoles.DataSource = data;
                }
            }
            catch (SqlException sqlEx)
            {
                ErrorHandler.HandleSqlError(sqlEx, "tải user roles");
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralError(ex, "tải user roles");
            }
        }

        private void btnCreateUser_Click(object sender, EventArgs e)
        {
            try
            {
                string username = txtUsername.Text.Trim();
                string password = txtPassword.Text;
                string fullName = txtFullName.Text.Trim();
                string roleName = cmbRoles.Text;

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(roleName))
                {
                    ErrorHandler.ShowWarning("Vui lòng điền đầy đủ thông tin!", "Thiếu thông tin");
                    return;
                }

                using (var connection = new SqlConnection(Properties.Settings.Default.QLNhapHangConnectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand("sp_CreateUserWithSQLLogin", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", password);
                        command.Parameters.AddWithValue("@FullName", fullName);
                        command.Parameters.AddWithValue("@RoleName", roleName);
                        command.Parameters.AddWithValue("@IsActive", true);
                        
                        var newUserIDParam = new SqlParameter("@NewUserID", SqlDbType.Int);
                        newUserIDParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(newUserIDParam);
                        
                        var messageParam = new SqlParameter("@Message", SqlDbType.NVarChar, -1);
                        messageParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(messageParam);
                        
                        command.ExecuteNonQuery();
                        
                        string resultMessage = messageParam.Value.ToString();
                        
                        // Xử lý kết quả từ stored procedure
                        if (resultMessage.ToLower().Contains("thành công"))
                        {
                            ErrorHandler.ShowSuccess(resultMessage);
                            // Clear form
                            txtUsername.Clear();
                            txtPassword.Clear();
                            txtFullName.Clear();
                            cmbRoles.SelectedIndex = -1;
                            LoadUsers();
                        }
                        else
                        {
                            ErrorHandler.HandleStoredProcedureResult(resultMessage, "tạo user");
                        }
                    }
                    connection.Close();
                }
            }
            catch (SqlException sqlEx)
            {
                ErrorHandler.HandleSqlError(sqlEx, "tạo user");
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralError(ex, "tạo user");
            }
        }

        private void btnRevokeAll_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvUsers.SelectedRows.Count == 0)
                {
                    ErrorHandler.ShowWarning("Vui lòng chọn user cần revoke quyền!", "Chưa chọn user");
                    return;
                }

                string username = dgvUsers.SelectedRows[0].Cells["Username"].Value.ToString();

                var result = ErrorHandler.ShowConfirmation($"Bạn có chắc muốn REVOKE TẤT CẢ QUYỀN của user '{username}'?\n\nHành động này sẽ:\n- Xóa tất cả roles của user\n- Revoke tất cả database permissions\n- User sẽ không thể truy cập hệ thống!", 
                    "CẢNH BÁO NGUY HIỂM");

                if (result)
                {
                    using (var connection = new SqlConnection(Properties.Settings.Default.QLNhapHangConnectionString))
                    {
                        connection.Open();
                        using (var command = new SqlCommand("sp_RevokeAllUserPermissions", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@Username", username);
                            
                            var messageParam = new SqlParameter("@Message", SqlDbType.NVarChar, 200);
                            messageParam.Direction = ParameterDirection.Output;
                            command.Parameters.Add(messageParam);
                            
                            command.ExecuteNonQuery();
                            
                            string resultMessage = messageParam.Value.ToString();
                            
                            // Xử lý kết quả từ stored procedure
                            if (resultMessage.ToLower().Contains("thành công"))
                            {
                                ErrorHandler.ShowSuccess(resultMessage);
                                LoadUsers();
                            }
                            else
                            {
                                ErrorHandler.HandleStoredProcedureResult(resultMessage, "revoke quyền user");
                            }
                        }
                        connection.Close();
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                ErrorHandler.HandleSqlError(sqlEx, "revoke quyền user");
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralError(ex, "revoke quyền user");
            }
        }

        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvUsers.SelectedRows.Count == 0)
                {
                    ErrorHandler.ShowWarning("Vui lòng chọn user cần xóa!", "Chưa chọn user");
                    return;
                }

                string username = dgvUsers.SelectedRows[0].Cells["Username"].Value.ToString();

                // DIALOG LỰA CHỌN HARD/SOFT DELETE
                using (var deleteDialog = new Form())
                {
                    deleteDialog.Text = "Chọn loại xóa";
                    deleteDialog.Size = new Size(500, 350);
                    deleteDialog.StartPosition = FormStartPosition.CenterParent;
                    deleteDialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                    deleteDialog.MaximizeBox = false;
                    deleteDialog.MinimizeBox = false;

                    var lblTitle = new Label
                    {
                        Text = $"Xóa user: {username}",
                        Font = new Font("Microsoft YaHei UI", 12, FontStyle.Bold),
                        Location = new Point(20, 20),
                        Size = new Size(450, 30)
                    };

                    var grpSoft = new GroupBox
                    {
                        Text = "🔒 SOFT DELETE (Khuyến nghị)",
                        Location = new Point(20, 60),
                        Size = new Size(450, 90),
                        Font = new Font("Microsoft YaHei UI", 9, FontStyle.Bold)
                    };
                    var lblSoft = new Label
                    {
                        Text = "• User không thể đăng nhập\n" +
                               "• Dữ liệu được giữ lại (lịch sử)\n" +
                               "• SQL Login bị DISABLE\n" +
                               "• Có thể khôi phục sau",
                        Location = new Point(10, 25),
                        Size = new Size(430, 60),
                        Font = new Font("Microsoft YaHei UI", 9)
                    };
                    grpSoft.Controls.Add(lblSoft);

                    var grpHard = new GroupBox
                    {
                        Text = "HARD DELETE (Nguy hiểm)",
                        Location = new Point(20, 160),
                        Size = new Size(450, 90),
                        Font = new Font("Microsoft YaHei UI", 9, FontStyle.Bold),
                        ForeColor = Color.Red
                    };
                    var lblHard = new Label
                    {
                        Text = "• XÓA VĨNH VIỄN khỏi database\n" +
                               "• SQL Login/User bị xóa\n" +
                               "• Tất cả quyền bị thu hồi\n" +
                               "• KHÔNG THỂ HOÀN TÁC!",
                        Location = new Point(10, 25),
                        Size = new Size(430, 60),
                        Font = new Font("Microsoft YaHei UI", 9)
                    };
                    grpHard.Controls.Add(lblHard);

                    var btnSoft = new Button
                    {
                        Text = "Soft Delete",
                        Location = new Point(100, 270),
                        Size = new Size(120, 35),
                        DialogResult = DialogResult.Yes,
                        Font = new Font("Microsoft YaHei UI", 10)
                    };

                    var btnHard = new Button
                    {
                        Text = "Hard Delete",
                        Location = new Point(230, 270),
                        Size = new Size(120, 35),
                        DialogResult = DialogResult.Retry,
                        Font = new Font("Microsoft YaHei UI", 10),
                        BackColor = Color.IndianRed
                    };

                    var btnCancel = new Button
                    {
                        Text = "Hủy",
                        Location = new Point(360, 270),
                        Size = new Size(100, 35),
                        DialogResult = DialogResult.Cancel,
                        Font = new Font("Microsoft YaHei UI", 10)
                    };

                    deleteDialog.Controls.AddRange(new Control[] { lblTitle, grpSoft, grpHard, btnSoft, btnHard, btnCancel });

                    var dialogResult = deleteDialog.ShowDialog();

                    if (dialogResult == DialogResult.Cancel)
                        return;

                    string deleteType = dialogResult == DialogResult.Yes ? "SOFT" : "HARD";

                    // XÁC NHẬN LẦN CUỐI
                    var confirmMsg = deleteType == "SOFT" 
                        ? $"Xác nhận SOFT DELETE user '{username}'?" 
                        : $"CẢNH BÁO\n\nBạn có CHẮC CHẮN muốn HARD DELETE user '{username}'?\n\nHành động này KHÔNG THỂ HOÀN TÁC!";

                    var confirm = ErrorHandler.ShowConfirmation(confirmMsg, "Xác nhận xóa user");

                    if (confirm)
                    {
                        using (var connection = new SqlConnection(Properties.Settings.Default.QLNhapHangConnectionString))
                        {
                            connection.Open();
                            using (var command = new SqlCommand("sp_DeleteUserComplete", connection))
                            {
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.AddWithValue("@Username", username);
                                command.Parameters.AddWithValue("@CurrentUsername", currentUsername);
                                command.Parameters.AddWithValue("@DeleteType", deleteType);
                                
                                var messageParam = new SqlParameter("@Message", SqlDbType.NVarChar, -1);
                                messageParam.Direction = ParameterDirection.Output;
                                command.Parameters.Add(messageParam);
                                
                                command.ExecuteNonQuery();
                                
                                string resultMessage = messageParam.Value.ToString();
                                
                                // Xử lý kết quả từ stored procedure
                                if (resultMessage.ToLower().Contains("thành công"))
                                {
                                    ErrorHandler.ShowSuccess(resultMessage);
                                    LoadUsers();
                                }
                                else
                                {
                                    ErrorHandler.HandleStoredProcedureResult(resultMessage, "xóa user");
                                }
                            }
                            connection.Close();
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                ErrorHandler.HandleSqlError(sqlEx, "xóa user");
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralError(ex, "xóa user");
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadUsers();
            LoadUserRoles();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnChangeRole_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvUsers.SelectedRows.Count == 0)
                {
                    ErrorHandler.ShowWarning("Vui lòng chọn user cần thay đổi role!", "Chưa chọn user");
                    return;
                }

                string username = dgvUsers.SelectedRows[0].Cells["Username"].Value.ToString();
                string currentRole = dgvUsers.SelectedRows[0].Cells["AppRole"].Value?.ToString() ?? "";

                // Mở form quản lý roles với admin username
                using (var roleForm = new ManageUserRolesForm(username, currentRole, currentUsername))
                {
                    if (roleForm.ShowDialog() == DialogResult.OK)
                    {
                        LoadUsers();
                        LoadUserRoles();
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                ErrorHandler.HandleSqlError(sqlEx, "thay đổi role user");
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralError(ex, "thay đổi role user");
            }
        }

        private void btnToggleStatus_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvUsers.SelectedRows.Count == 0)
                {
                    ErrorHandler.ShowWarning("Vui lòng chọn user cần thay đổi trạng thái!", "Chưa chọn user");
                    return;
                }

                int userID = Convert.ToInt32(dgvUsers.SelectedRows[0].Cells["UserID"].Value);
                string username = dgvUsers.SelectedRows[0].Cells["Username"].Value.ToString();
                bool currentStatus = Convert.ToBoolean(dgvUsers.SelectedRows[0].Cells["IsActive"].Value);
                
                string action = currentStatus ? "vô hiệu hóa" : "kích hoạt";
                
                var result = ErrorHandler.ShowConfirmation($"Bạn có chắc muốn {action} user '{username}'?", 
                    $"Xác nhận {action}");

                if (result)
                {
                    using (var connection = new SqlConnection(Properties.Settings.Default.QLNhapHangConnectionString))
                    {
                        connection.Open();
                        using (var command = new SqlCommand("sp_ToggleUserStatus", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@UserID", userID);
                            command.Parameters.AddWithValue("@CurrentUsername", currentUsername);
                            
                            var messageParam = new SqlParameter("@Message", SqlDbType.NVarChar, 200);
                            messageParam.Direction = ParameterDirection.Output;
                            command.Parameters.Add(messageParam);
                            
                            command.ExecuteNonQuery();
                            
                            string resultMessage = messageParam.Value.ToString();
                            
                            // Xử lý kết quả từ stored procedure
                            if (resultMessage.ToLower().Contains("thành công"))
                            {
                                ErrorHandler.ShowSuccess(resultMessage);
                                LoadUsers();
                            }
                            else
                            {
                                ErrorHandler.HandleStoredProcedureResult(resultMessage, "thay đổi trạng thái người dùng");
                            }
                        }
                        connection.Close();
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                ErrorHandler.HandleSqlError(sqlEx, "thay đổi trạng thái user");
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralError(ex, "thay đổi trạng thái user");
            }
        }

        private void btnCreateSQLLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvUsers.SelectedRows.Count == 0)
                {
                    ErrorHandler.ShowWarning("Vui lòng chọn user cần tạo SQL Login!", "Chưa chọn user");
                    return;
                }

                string username = dgvUsers.SelectedRows[0].Cells["Username"].Value.ToString();
                bool hasSQLLogin = dgvUsers.SelectedRows[0].Cells["HasSQLLogin"]?.Value != null && 
                                   Convert.ToBoolean(dgvUsers.SelectedRows[0].Cells["HasSQLLogin"].Value);

                if (hasSQLLogin)
                {
                    ErrorHandler.ShowWarning($"User '{username}' đã có SQL Login rồi!", "SQL Login đã tồn tại");
                    return;
                }

                // Tạo dialog để nhập password
                using (var passwordDialog = new Form())
                {
                    passwordDialog.Text = $"Tạo SQL Login cho: {username}";
                    passwordDialog.Size = new Size(450, 220);
                    passwordDialog.StartPosition = FormStartPosition.CenterParent;
                    passwordDialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                    passwordDialog.MaximizeBox = false;
                    passwordDialog.MinimizeBox = false;

                    var lblTitle = new Label
                    {
                        Text = $"Tạo SQL Login cho user: {username}",
                        Font = new Font("Microsoft YaHei UI", 11, FontStyle.Bold),
                        Location = new Point(20, 20),
                        Size = new Size(400, 25)
                    };

                    var lblPassword = new Label
                    {
                        Text = "Mật khẩu SQL Login:",
                        Font = new Font("Microsoft YaHei UI", 10),
                        Location = new Point(20, 60),
                        Size = new Size(160, 25)
                    };

                    var txtPassword = new TextBox
                    {
                        Font = new Font("Microsoft YaHei UI", 10),
                        Location = new Point(190, 58),
                        Size = new Size(220, 25),
                        PasswordChar = '*'
                    };

                    var lblConfirm = new Label
                    {
                        Text = "Xác nhận mật khẩu:",
                        Font = new Font("Microsoft YaHei UI", 10),
                        Location = new Point(20, 100),
                        Size = new Size(160, 25)
                    };

                    var txtConfirm = new TextBox
                    {
                        Font = new Font("Microsoft YaHei UI", 10),
                        Location = new Point(190, 98),
                        Size = new Size(220, 25),
                        PasswordChar = '*'
                    };

                    var btnOK = new Button
                    {
                        Text = "Tạo Login",
                        Location = new Point(200, 140),
                        Size = new Size(100, 30),
                        DialogResult = DialogResult.OK,
                        Font = new Font("Microsoft YaHei UI", 10),
                        BackColor = Color.FromArgb(40, 167, 69),
                        ForeColor = Color.White,
                        FlatStyle = FlatStyle.Flat
                    };

                    var btnCancel = new Button
                    {
                        Text = "Hủy",
                        Location = new Point(310, 140),
                        Size = new Size(100, 30),
                        DialogResult = DialogResult.Cancel,
                        Font = new Font("Microsoft YaHei UI", 10)
                    };

                    passwordDialog.Controls.AddRange(new Control[] { lblTitle, lblPassword, txtPassword, lblConfirm, txtConfirm, btnOK, btnCancel });

                    if (passwordDialog.ShowDialog() == DialogResult.OK)
                    {
                        string password = txtPassword.Text;
                        string confirmPassword = txtConfirm.Text;

                        if (string.IsNullOrEmpty(password))
                        {
                            ErrorHandler.ShowWarning("Vui lòng nhập mật khẩu!", "Thiếu thông tin");
                            return;
                        }

                        if (password != confirmPassword)
                        {
                            ErrorHandler.ShowWarning("Mật khẩu xác nhận không khớp!", "Mật khẩu không khớp");
                            return;
                        }

                        if (password.Length < 6)
                        {
                            ErrorHandler.ShowWarning("Mật khẩu phải có ít nhất 6 ký tự!", "Mật khẩu quá ngắn");
                            return;
                        }

                        // Gọi stored procedure sp_CreateSQLLoginForExistingUser
                        using (var connection = new SqlConnection(Properties.Settings.Default.QLNhapHangConnectionString))
                        {
                            connection.Open();
                            using (var command = new SqlCommand("sp_CreateSQLLoginForExistingUser", connection))
                            {
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.AddWithValue("@Username", username);
                                command.Parameters.AddWithValue("@Password", password);

                                var messageParam = new SqlParameter("@Message", SqlDbType.NVarChar, -1);
                                messageParam.Direction = ParameterDirection.Output;
                                command.Parameters.Add(messageParam);

                                command.ExecuteNonQuery();

                                string resultMessage = messageParam.Value.ToString();

                                // Xử lý kết quả từ stored procedure
                                if (resultMessage.Contains("THÀNH CÔNG") || resultMessage.Contains("thành công") || resultMessage.Contains("✅"))
                                {
                                    ErrorHandler.ShowSuccess(resultMessage);
                                    LoadUsers();
                                }
                                else
                                {
                                    ErrorHandler.HandleStoredProcedureResult(resultMessage, "tạo SQL Login");
                                }
                            }
                            connection.Close();
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                ErrorHandler.HandleSqlError(sqlEx, "tạo SQL Login");
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralError(ex, "tạo SQL Login");
            }
        }

    }
}
