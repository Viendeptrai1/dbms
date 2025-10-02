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
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khởi tạo dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadUsers()
        {
            try
            {
                using (var connection = new SqlConnection(Properties.Settings.Default.QLNhapHangConnectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand("sp_GetUsersWithRoles", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        
                        var adapter = new SqlDataAdapter(command);
                        var dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        
                        dgvUsers.DataSource = dataTable;
                        
                        // Định dạng cột
                        if (dgvUsers.Columns.Count > 0)
                        {
                            if (dgvUsers.Columns.Contains("IsActive"))
                            {
                                dgvUsers.Columns["IsActive"].DefaultCellStyle.ForeColor = Color.Green;
                                dgvUsers.Columns["IsActive"].Width = 80;
                            }
                            
                            if (dgvUsers.Columns.Contains("Roles"))
                                dgvUsers.Columns["Roles"].Width = 200;
                                
                            if (dgvUsers.Columns.Contains("Username"))
                                dgvUsers.Columns["Username"].Width = 150;
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách user: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách roles: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải user roles: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (var connection = new SqlConnection(Properties.Settings.Default.QLNhapHangConnectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand("sp_CreateUserWithRole", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", password); // Truyền password plain text
                        command.Parameters.AddWithValue("@FullName", fullName);
                        command.Parameters.AddWithValue("@RoleName", roleName);
                        command.Parameters.AddWithValue("@IsActive", true);
                        
                        var newUserIDParam = new SqlParameter("@NewUserID", SqlDbType.Int);
                        newUserIDParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(newUserIDParam);
                        
                        var messageParam = new SqlParameter("@Message", SqlDbType.NVarChar, 200);
                        messageParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(messageParam);
                        
                        command.ExecuteNonQuery();
                        
                        string resultMessage = messageParam.Value.ToString();
                        int newUserID = (int)newUserIDParam.Value;
                        
                        MessageBox.Show(resultMessage, "Kết quả", MessageBoxButtons.OK, 
                            resultMessage.Contains("thành công") ? MessageBoxIcon.Information : MessageBoxIcon.Error);
                        
                        if (resultMessage.Contains("thành công"))
                        {
                            // Clear form
                            txtUsername.Clear();
                            txtPassword.Clear();
                            txtFullName.Clear();
                            cmbRoles.SelectedIndex = 0;
                            
                            LoadUsers();
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tạo user: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnChangeRole_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvUsers.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn user cần thay đổi role!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string username = dgvUsers.SelectedRows[0].Cells["Username"].Value.ToString();
                string newRole = cmbNewRole.Text;

                if (string.IsNullOrEmpty(newRole))
                {
                    MessageBox.Show("Vui lòng chọn role mới!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var result = MessageBox.Show($"Bạn có chắc muốn thay đổi role của '{username}' thành '{newRole}'?", 
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    using (var connection = new SqlConnection(Properties.Settings.Default.QLNhapHangConnectionString))
                    {
                        connection.Open();
                        using (var command = new SqlCommand("sp_ChangeUserRole", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@Username", username);
                            command.Parameters.AddWithValue("@NewRoleName", newRole);
                            
                            var messageParam = new SqlParameter("@Message", SqlDbType.NVarChar, 200);
                            messageParam.Direction = ParameterDirection.Output;
                            command.Parameters.Add(messageParam);
                            
                            command.ExecuteNonQuery();
                            
                            string resultMessage = messageParam.Value.ToString();
                            MessageBox.Show(resultMessage, "Kết quả", MessageBoxButtons.OK, 
                                resultMessage.Contains("thành công") ? MessageBoxIcon.Information : MessageBoxIcon.Error);
                            
                            LoadUsers();
                        }
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thay đổi role: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnToggleStatus_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvUsers.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn user cần thay đổi trạng thái!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string username = dgvUsers.SelectedRows[0].Cells["Username"].Value.ToString();
                bool currentStatus = Convert.ToBoolean(dgvUsers.SelectedRows[0].Cells["IsActive"].Value);

                string action = currentStatus ? "khóa" : "mở khóa";
                var result = MessageBox.Show($"Bạn có chắc muốn {action} user '{username}'?", 
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    using (var connection = new SqlConnection(Properties.Settings.Default.QLNhapHangConnectionString))
                    {
                        connection.Open();
                        using (var command = new SqlCommand("sp_ToggleUserStatus", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@Username", username);
                            
                            var messageParam = new SqlParameter("@Message", SqlDbType.NVarChar, 200);
                            messageParam.Direction = ParameterDirection.Output;
                            command.Parameters.Add(messageParam);
                            
                            command.ExecuteNonQuery();
                            
                            string resultMessage = messageParam.Value.ToString();
                            MessageBox.Show(resultMessage, "Kết quả", MessageBoxButtons.OK, 
                                resultMessage.Contains("thành công") ? MessageBoxIcon.Information : MessageBoxIcon.Error);
                            
                            LoadUsers();
                        }
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thay đổi trạng thái user: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRevokeAll_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvUsers.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn user cần revoke quyền!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string username = dgvUsers.SelectedRows[0].Cells["Username"].Value.ToString();

                var result = MessageBox.Show($"Bạn có chắc muốn REVOKE TẤT CẢ QUYỀN của user '{username}'?\n\nHành động này sẽ:\n- Xóa tất cả roles của user\n- Revoke tất cả database permissions\n- User sẽ không thể truy cập hệ thống!", 
                    "CẢNH BÁO NGUY HIỂM", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
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
                            MessageBox.Show(resultMessage, "Kết quả", MessageBoxButtons.OK, 
                                resultMessage.Contains("thành công") ? MessageBoxIcon.Information : MessageBoxIcon.Error);
                            
                            LoadUsers();
                        }
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi revoke quyền: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvUsers.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn user cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string username = dgvUsers.SelectedRows[0].Cells["Username"].Value.ToString();

                var result = MessageBox.Show($"Bạn có chắc muốn XÓA user '{username}'?\n\nHành động này sẽ:\n- Xóa user khỏi hệ thống\n- Xóa tất cả roles\n- Xóa database user\n- KHÔNG THỂ HOÀN TÁC!", 
                    "CẢNH BÁO XÓA", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    using (var connection = new SqlConnection(Properties.Settings.Default.QLNhapHangConnectionString))
                    {
                        connection.Open();
                        using (var command = new SqlCommand("sp_DeleteUser", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@Username", username);
                            
                            var messageParam = new SqlParameter("@Message", SqlDbType.NVarChar, 200);
                            messageParam.Direction = ParameterDirection.Output;
                            command.Parameters.Add(messageParam);
                            
                            command.ExecuteNonQuery();
                            
                            string resultMessage = messageParam.Value.ToString();
                            MessageBox.Show(resultMessage, "Kết quả", MessageBoxButtons.OK, 
                                resultMessage.Contains("thành công") ? MessageBoxIcon.Information : MessageBoxIcon.Error);
                            
                            LoadUsers();
                        }
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa user: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

    }
}
