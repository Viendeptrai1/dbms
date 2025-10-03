using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Configuration;
using System.Linq;

namespace dbms
{
    public partial class TwoTierPermissionsForm : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["QLNhapHangConnectionString"].ConnectionString;

        public TwoTierPermissionsForm()
        {
            InitializeComponent();
            InitializeForm();
        }

        private void InitializeForm()
        {
            // Initialize combo boxes
            cmbAction.Items.AddRange(new string[] { "CREATE", "SOFT_DELETE", "HARD_DELETE", "RESTORE" });
            cmbDBRole.Items.AddRange(new string[] { "dbrole_Admin", "dbrole_Seller" });
            
            cmbAction.SelectedIndex = 0;
            cmbDBRole.SelectedIndex = 1; // Default to Seller role
            
            LoadExistingUsers();
        }

        private void LoadExistingUsers()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT 
                            u.Username,
                            u.FullName,
                            r.RoleName,
                            u.IsActive,
                            CASE WHEN u.Username LIKE '%_DELETED_%' THEN 'Deleted' ELSE 'Active' END AS Status
                        FROM Users u
                        LEFT JOIN UsersRoles ur ON u.UserID = ur.UserID
                        LEFT JOIN Roles r ON ur.RoleID = r.RoleID
                        ORDER BY u.Username";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            dgvUsers.DataSource = dt;
                        }
                    }
                }

                // Format grid
                if (dgvUsers.Columns.Count > 0)
                {
                    dgvUsers.Columns["Username"].HeaderText = "Tên đăng nhập";
                    dgvUsers.Columns["FullName"].HeaderText = "Họ tên";
                    dgvUsers.Columns["RoleName"].HeaderText = "Vai trò";
                    dgvUsers.Columns["IsActive"].HeaderText = "Hoạt động";
                    dgvUsers.Columns["Status"].HeaderText = "Trạng thái";
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.ShowError("Lỗi tải danh sách người dùng", ex.Message);
            }
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtUsername.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên đăng nhập!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string action = cmbAction.SelectedItem.ToString();
                
                if (action == "CREATE" && (string.IsNullOrWhiteSpace(txtPassword.Text) || string.IsNullOrWhiteSpace(txtFullName.Text)))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin để tạo user mới!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                ExecuteTwoTierPermissions();
            }
            catch (Exception ex)
            {
                ErrorHandler.ShowError("Lỗi thực thi", ex.Message);
            }
        }

        private void ExecuteTwoTierPermissions()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("dbo.sp_TwoTierPermissions", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 120;

                        // Input parameters
                        cmd.Parameters.AddWithValue("@Action", cmbAction.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@Username", txtUsername.Text.Trim());
                        cmd.Parameters.AddWithValue("@Password", string.IsNullOrWhiteSpace(txtPassword.Text) ? (object)DBNull.Value : txtPassword.Text);
                        cmd.Parameters.AddWithValue("@FullName", string.IsNullOrWhiteSpace(txtFullName.Text) ? (object)DBNull.Value : txtFullName.Text);
                        cmd.Parameters.AddWithValue("@DBRole", cmbDBRole.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@DeleteType", cmbDeleteType.SelectedItem?.ToString() ?? (object)DBNull.Value);

                        // Output parameter
                        SqlParameter messageParam = new SqlParameter("@Message", SqlDbType.NVarChar, -1)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(messageParam);

                        cmd.ExecuteNonQuery();

                        string message = messageParam.Value?.ToString() ?? "Thực thi thành công!";
                        
                        // Display result in rich text box with formatting
                        rtbResult.Clear();
                        rtbResult.Text = message;
                        
                        // Color coding based on result
                        if (message.Contains("✅"))
                        {
                            rtbResult.SelectAll();
                            rtbResult.SelectionColor = System.Drawing.Color.Green;
                        }
                        else if (message.Contains("⚠️") || message.Contains("❌"))
                        {
                            rtbResult.SelectAll();
                            rtbResult.SelectionColor = System.Drawing.Color.Red;
                        }

                        // Refresh user list
                        LoadExistingUsers();
                        
                        // Clear form if successful
                        if (message.Contains("✅"))
                        {
                            ClearForm();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                rtbResult.Clear();
                rtbResult.Text = $"❌ LỖI: {ex.Message}";
                rtbResult.SelectAll();
                rtbResult.SelectionColor = System.Drawing.Color.Red;
            }
        }

        private void ClearForm()
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtFullName.Clear();
            cmbAction.SelectedIndex = 0;
            cmbDBRole.SelectedIndex = 1;
            if (cmbDeleteType.Items.Count > 0)
                cmbDeleteType.SelectedIndex = 0;
        }

        private void cmbAction_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedAction = cmbAction.SelectedItem.ToString();
            
            // Enable/disable controls based on action
            bool isCreate = selectedAction == "CREATE";
            bool isDelete = selectedAction == "SOFT_DELETE" || selectedAction == "HARD_DELETE";
            
            txtPassword.Enabled = isCreate;
            txtFullName.Enabled = isCreate;
            lblPassword.Enabled = isCreate;
            lblFullName.Enabled = isCreate;
            
            // Setup delete type combo
            if (isDelete)
            {
                cmbDeleteType.Items.Clear();
                if (selectedAction == "SOFT_DELETE")
                {
                    cmbDeleteType.Items.Add("SOFT");
                }
                else if (selectedAction == "HARD_DELETE")
                {
                    cmbDeleteType.Items.AddRange(new string[] { "SOFT", "HARD" });
                }
                cmbDeleteType.SelectedIndex = 0;
                cmbDeleteType.Enabled = true;
                lblDeleteType.Enabled = true;
            }
            else
            {
                cmbDeleteType.Enabled = false;
                lblDeleteType.Enabled = false;
            }
        }

        private void dgvUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvUsers.Rows[e.RowIndex];
                txtUsername.Text = row.Cells["Username"].Value?.ToString() ?? "";
                txtFullName.Text = row.Cells["FullName"].Value?.ToString() ?? "";
                
                // Set appropriate action based on user status
                string status = row.Cells["Status"].Value?.ToString() ?? "";
                if (status == "Deleted")
                {
                    cmbAction.SelectedItem = "RESTORE";
                }
                else
                {
                    cmbAction.SelectedItem = "SOFT_DELETE";
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadExistingUsers();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
            rtbResult.Clear();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
