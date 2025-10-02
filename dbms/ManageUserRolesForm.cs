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
        private int userID;
        private string username;
        private List<int> currentRoles;
        private List<int> availableRoles;

        public ManageUserRolesForm(int userID, string username)
        {
            this.userID = userID;
            this.username = username;
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
                MessageBox.Show("Lỗi tải dữ liệu vai trò: " + ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                using (var connection = new SqlConnection(Properties.Settings.Default.QLNhapHangConnectionString))
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // Xóa tất cả vai trò hiện tại
                            string deleteQuery = "DELETE FROM dbo.UsersRoles WHERE UserID = @UserID";
                            using (var deleteCommand = new SqlCommand(deleteQuery, connection, transaction))
                            {
                                deleteCommand.Parameters.AddWithValue("@UserID", userID);
                                deleteCommand.ExecuteNonQuery();
                            }
                            
                            // Thêm vai trò mới được chọn
                            string insertQuery = "INSERT INTO dbo.UsersRoles (UserID, RoleID) VALUES (@UserID, @RoleID)";
                            for (int i = 0; i < clbRoles.Items.Count; i++)
                            {
                                if (clbRoles.GetItemChecked(i))
                                {
                                    using (var insertCommand = new SqlCommand(insertQuery, connection, transaction))
                                    {
                                        insertCommand.Parameters.AddWithValue("@UserID", userID);
                                        insertCommand.Parameters.AddWithValue("@RoleID", availableRoles[i]);
                                        insertCommand.ExecuteNonQuery();
                                    }
                                }
                            }
                            
                            transaction.Commit();
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật vai trò: " + ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
