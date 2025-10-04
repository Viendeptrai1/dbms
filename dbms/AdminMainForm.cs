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
    public partial class AdminMainForm : Form
    {
        private QLNhapHangDataSet dataSet;
        private SqlConnection connection;
        private int currentUserID;
        private string currentUsername;
        private string currentUserRole;

        public AdminMainForm()
        {
            InitializeComponent();
            InitializeDatabase();
        }

        public void SetUserInfo(int userID, string username, string userRole)
        {
            currentUserID = userID;
            currentUsername = username;
            currentUserRole = userRole;
            lblUserInfo.Text = $"Xin chào: {username} ({userRole})";
        }

        private void InitializeDatabase()
        {
            try
            {
                connection = new SqlConnection(Properties.Settings.Default.QLNhapHangConnectionString);
                dataSet = new QLNhapHangDataSet();
                
                connection.Open();
                connection.Close();
                
                lblConnectionStatus.Text = "Kết nối: Thành công";
                lblConnectionStatus.ForeColor = Color.Green;
                
                LoadInitialData();
            }
            catch (Exception ex)
            {
                lblConnectionStatus.Text = "Kết nối: Lỗi - " + ex.Message;
                lblConnectionStatus.ForeColor = Color.Red;
                ErrorHandler.HandleGeneralError(ex, "kết nối database");
            }
        }

        private void LoadInitialData()
        {
            try
            {
                LoadUsers();
                LoadRoles();
                LoadUsersRoles();
                
                toolStripStatusLabel1.Text = "Dữ liệu đã được tải thành công";
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralError(ex, "tải dữ liệu");
            }
        }

        #region Form Events
        private void Form1_Load(object sender, EventArgs e) { }
        #endregion

        #region User Management Events
        private void btnRefreshUsers_Click(object sender, EventArgs e)
        {
            LoadUsers();
            LoadRoles();
            LoadUsersRoles();
        }

        private void btnManageUserRoles_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvUsers.SelectedRows.Count > 0)
                {
                    var userID = (int)dgvUsers.SelectedRows[0].Cells["UserID"].Value;
                    var username = dgvUsers.SelectedRows[0].Cells["Username"].Value.ToString();
                    
                    var manageRolesForm = new ManageUserRolesForm(userID, username);
                    if (manageRolesForm.ShowDialog() == DialogResult.OK)
                    {
                        LoadUsers();
                        LoadUsersRoles();
                        ErrorHandler.ShowSuccess("Cập nhật vai trò thành công!");
                    }
                }
                else
                {
                    ErrorHandler.ShowWarning("Vui lòng chọn người dùng cần quản lý vai trò!", "⚠️ Chưa chọn người dùng");
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralError(ex, "quản lý vai trò");
            }
        }

        private void btnToggleUserStatus_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvUsers.SelectedRows.Count > 0)
                {
                    var userID = (int)dgvUsers.SelectedRows[0].Cells["UserID"].Value;
                    var username = dgvUsers.SelectedRows[0].Cells["Username"].Value.ToString();
                    var isActive = (bool)dgvUsers.SelectedRows[0].Cells["IsActive"].Value;
                    
                    var newStatus = !isActive;
                    var action = newStatus ? "kích hoạt" : "khóa";
                    
                    var result = ErrorHandler.ShowConfirmation($"Bạn có chắc muốn {action} tài khoản '{username}'?", 
                        $"Xác nhận {action} tài khoản");
                    
                    if (result)
                    {
                        // Sử dụng stored procedure mới
                        using (var command = new SqlCommand("sp_ToggleUserStatus", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@Username", username);
                            
                            var messageParam = new SqlParameter("@Message", SqlDbType.NVarChar, 200);
                            messageParam.Direction = ParameterDirection.Output;
                            command.Parameters.Add(messageParam);

                            connection.Open();
                            command.ExecuteNonQuery();
                            connection.Close();

                            string resultMessage = messageParam.Value.ToString();
                            // Sử dụng ErrorHandler để xử lý kết quả
                            if (ErrorHandler.HandleStoredProcedureResult(resultMessage, "thay đổi trạng thái người dùng"))
                            {
                                LoadUsers();
                            }
                        }
                    }
                }
                else
                {
                    ErrorHandler.ShowWarning("Vui lòng chọn người dùng cần thay đổi trạng thái!", "⚠️ Chưa chọn người dùng");
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralError(ex, "thay đổi trạng thái người dùng");
            }
            finally
            {
                if (connection.State == ConnectionState.Open) connection.Close();
            }
        }
        #endregion

        #region Admin Access to All Forms
        private void btnOpenProductSearch_Click(object sender, EventArgs e)
        {
            try
            {
                var productSearchForm = new ProductSearchForm();
                productSearchForm.ShowDialog();
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralError(ex, "mở form tra cứu");
            }
        }

        private void btnOpenAddProduct_Click(object sender, EventArgs e)
        {
            try
            {
                var addProductForm = new AddProductForm();
                if (addProductForm.ShowDialog() == DialogResult.OK)
                {
                    LoadUsers(); // Refresh data
                    ErrorHandler.ShowSuccess("Thêm sản phẩm thành công!");
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralError(ex, "mở form thêm sản phẩm");
            }
        }

        private void btnOpenPriceAdjust_Click(object sender, EventArgs e)
        {
            try
            {
                var priceAdjustForm = new PriceAdjustForm();
                if (priceAdjustForm.ShowDialog() == DialogResult.OK)
                {
                    LoadUsers(); // Refresh data
                    ErrorHandler.ShowSuccess("Điều chỉnh giá thành công!");
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralError(ex, "mở form điều chỉnh giá");
            }
        }

        private void btnOpenCreateReceipt_Click(object sender, EventArgs e)
        {
            try
            {
                var createReceiptForm = new CreateReceiptForm(currentUserID, currentUsername);
                if (createReceiptForm.ShowDialog() == DialogResult.OK)
                {
                    LoadUsers(); // Refresh data
                    ErrorHandler.ShowSuccess("Tạo phiếu nhập thành công!");
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralError(ex, "mở form tạo phiếu nhập");
            }
        }

        private void btnOpenReports_Click(object sender, EventArgs e)
        {
            try
            {
                var reportsForm = new AdminReportsForm();
                reportsForm.ShowDialog();
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralError(ex, "mở form báo cáo");
            }
        }

        private void btnUserManagement_Click(object sender, EventArgs e)
        {
            try
            {
                var userManagementForm = new UserManagementForm(currentUserID, currentUsername, currentUserRole);
                userManagementForm.ShowDialog();
                LoadUsers(); // Refresh data after closing
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralError(ex, "mở form quản lý user");
            }
        }
        #endregion

        #region Menu Events
        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = ErrorHandler.ShowConfirmation("Bạn có chắc muốn đăng xuất?", "Xác nhận đăng xuất");
            
            if (result)
            {
                this.Close();
                Application.Restart();
            }
        }

        private void thoátToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = ErrorHandler.ShowConfirmation("Bạn có chắc muốn thoát ứng dụng?", "Xác nhận thoát");
            
            if (result)
            {
                Application.Exit();
            }
        }
        #endregion

        #region Data Loading Methods
        private void LoadUsers()
        {
            try
            {
                // Sử dụng stored procedure mới để lấy users với roles
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
                                dgvUsers.Columns["IsActive"].DefaultCellStyle.ForeColor = Color.Green;
                            
                            if (dgvUsers.Columns.Contains("Roles"))
                                dgvUsers.Columns["Roles"].Width = 200;
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralError(ex, "tải người dùng");
            }
        }

        private void LoadRoles()
        {
            try
            {
                using (var adapter = new QLNhapHangDataSetTableAdapters.RolesTableAdapter())
                {
                    var data = adapter.GetData();
                    dgvRoles.DataSource = data;
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralError(ex, "tải vai trò");
            }
        }

        private void LoadUsersRoles()
        {
            try
            {
                using (var adapter = new QLNhapHangDataSetTableAdapters.UsersRolesTableAdapter())
                {
                    var data = adapter.GetData();
                    dgvUsersRoles.DataSource = data;
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralError(ex, "tải phân quyền");
            }
        }

        // New menu handlers for integrated features
        private void btnMonthlyReport_Click(object sender, EventArgs e)
        {
            try
            {
                MonthlyReportForm form = new MonthlyReportForm();
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralError(ex, "mở báo cáo tháng");
            }
        }

        private void btnTwoTierPermissions_Click(object sender, EventArgs e)
        {
            try
            {
                TwoTierPermissionsForm form = new TwoTierPermissionsForm();
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralError(ex, "mở quản lý phân quyền 2 cấp");
            }
        }

        private void btnProductDashboard_Click(object sender, EventArgs e)
        {
            try
            {
                ProductPerformanceDashboardForm form = new ProductPerformanceDashboardForm();
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralError(ex, "mở dashboard sản phẩm");
            }
        }

        private void btnBatchImport_Click(object sender, EventArgs e)
        {
            try
            {
                BatchImportReceiptsForm form = new BatchImportReceiptsForm(currentUserID, currentUsername);
                form.ShowDialog();
                // Refresh data sau khi batch import
                LoadUsers();
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralError(ex, "mở batch import");
            }
        }
        #endregion
    }
}
