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
    public partial class ManageUserRolesForm : Form
    {
        private readonly int userID;
        private readonly string username;
        private readonly string currentAdminUsername;
        private readonly List<int> currentRoles;
        private readonly List<int> availableRoles;

        public ManageUserRolesForm(int userID, string username)
        {
            this.userID = userID;
            this.username = username;
            this.currentAdminUsername = "admin"; // Default, should be passed from calling form
            this.currentRoles = new List<int>();
            this.availableRoles = new List<int>();
            InitializeComponent();
            InitializeData();
        }

        // Constructor overload cho UserManagementForm with admin context
        public ManageUserRolesForm(string username, string currentRole, string adminUsername = "admin")
        {
            this.userID = 0; // Sẽ tìm từ username
            this.username = username;
            this.currentAdminUsername = adminUsername;
            this.currentRoles = new List<int>();
            this.availableRoles = new List<int>();
            InitializeComponent();
            InitializeData();
        }

        private void InitializeData()
        {
            lblUserInfo.Text = $"Quản lý vai trò cho: {username}";
            
            try
            {
                using (var connection = new SqlConnection(Properties.Settings.Default.QLNhapHangConnectionString))
                {
                    connection.Open();
                    
                    // Load tất cả vai trò
                    string rolesQuery = "SELECT RoleID, RoleName FROM dbo.Roles ORDER BY RoleName";
                    using (var command = new SqlCommand(rolesQuery, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                availableRoles.Add(Convert.ToInt32(reader["RoleID"]));
                                clbRoles.Items.Add(reader["RoleName"].ToString());
                            }
                        }
                    }
                    
                    // Load vai trò hiện tại của user
                    string userRolesQuery = "SELECT RoleID FROM dbo.UsersRoles WHERE UserID = @UserID";
                    using (var command = new SqlCommand(userRolesQuery, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", userID);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                currentRoles.Add(Convert.ToInt32(reader["RoleID"]));
                            }
                        }
                    }
                    
                    connection.Close();
                    
                    // Check các vai trò hiện tại
                    for (int i = 0; i < availableRoles.Count; i++)
                    {
                        if (currentRoles.Contains(availableRoles[i]))
                        {
                            clbRoles.SetItemChecked(i, true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralError(ex, "tải dữ liệu vai trò");
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra xem có chọn role nào không
                int checkedCount = 0;
                string selectedRoleName = "";
                
                for (int i = 0; i < clbRoles.Items.Count; i++)
                {
                    if (clbRoles.GetItemChecked(i))
                    {
                        checkedCount++;
                        selectedRoleName = clbRoles.Items[i].ToString();
                    }
                }
                
                if (checkedCount == 0)
                {
                    ErrorHandler.ShowWarning("Vui lòng chọn ít nhất một role!", "Chưa chọn role");
                    return;
                }
                
                if (checkedCount > 1)
                {
                    ErrorHandler.ShowWarning("Chỉ được chọn một role duy nhất!\n\nHệ thống sử dụng Two-Tier Permission nên mỗi user chỉ có 1 role.", 
                        "Chọn nhiều role");
                    return;
                }
                
                // Sử dụng sp_ChangeUserRoleComplete
                using (var connection = new SqlConnection(Properties.Settings.Default.QLNhapHangConnectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand("sp_ChangeUserRoleComplete", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@NewRoleName", selectedRoleName);
                        command.Parameters.AddWithValue("@CurrentUsername", currentAdminUsername);
                        
                        var messageParam = new SqlParameter("@Message", SqlDbType.NVarChar, -1);
                        messageParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(messageParam);
                        
                        command.ExecuteNonQuery();
                        
                        string resultMessage = messageParam.Value.ToString();
                        
                        // Xử lý kết quả từ stored procedure
                        if (resultMessage.Contains("THÀNH CÔNG") || resultMessage.Contains("thành công"))
                        {
                            ErrorHandler.ShowSuccess(resultMessage);
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                            ErrorHandler.HandleStoredProcedureResult(resultMessage, "thay đổi role");
                        }
                    }
                    connection.Close();
                }
            }
            catch (SqlException sqlEx)
            {
                ErrorHandler.HandleSqlError(sqlEx, "cập nhật vai trò");
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralError(ex, "cập nhật vai trò");
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
