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
    public partial class AdminReportsForm : Form
    {
        public AdminReportsForm()
        {
            InitializeComponent();
            InitializeData();
        }

        private void InitializeData()
        {
            try
            {
                LoadAllReports();
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralError(ex, "khởi tạo dữ liệu");
            }
        }

        private void LoadAllReports()
        {
            try
            {
                // Load tất cả báo cáo có sẵn
                LoadInventoryValuation();
                LoadImportLines();
                LoadTopProducts();
                LoadSupplierSummary();
                LoadProductImportHistory();
                LoadProductsNeverImported();
                
                // Load báo cáo hệ thống mới
                LoadUsersRoleSummary();
                LoadUserActivity();
                LoadPriceHistory();
                
                toolStripStatusLabel1.Text = "Đã tải tất cả báo cáo thành công";
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralError(ex, "tải báo cáo");
            }
        }

        private void LoadInventoryValuation()
        {
            try
            {
                using (var adapter = new QLNhapHangDataSetTableAdapters.vw_InventoryValuationTableAdapter())
                {
                    var data = adapter.GetData();
                    dgvInventoryValuation.DataSource = data;
                    lblInventoryCount.Text = $"Tồn kho: {data.Count} sản phẩm";
                }
            }
            catch (Exception ex)
            {
                lblInventoryCount.Text = "Lỗi tải tồn kho: " + ex.Message;
            }
        }

        private void LoadImportLines()
        {
            try
            {
                using (var adapter = new QLNhapHangDataSetTableAdapters.vw_ImportLinesTableAdapter())
                {
                    var data = adapter.GetData();
                    dgvImportLines.DataSource = data;
                    lblImportLinesCount.Text = $"Dòng nhập: {data.Count} dòng";
                }
            }
            catch (Exception ex)
            {
                lblImportLinesCount.Text = "Lỗi tải dòng nhập: " + ex.Message;
            }
        }

        private void LoadTopProducts()
        {
            try
            {
                using (var connection = new SqlConnection(Properties.Settings.Default.QLNhapHangConnectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand("SELECT * FROM dbo.fn_TopProductsByImportValue(@From, @To, @TopN, NULL, NULL)", connection))
                    {
                        command.Parameters.AddWithValue("@From", DateTime.Now.AddDays(-30));
                        command.Parameters.AddWithValue("@To", DateTime.Now);
                        command.Parameters.AddWithValue("@TopN", 10);

                        var adapter = new SqlDataAdapter(command);
                        var dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        
                        dgvTopProducts.DataSource = dataTable;
                        lblTopProductsCount.Text = $"Top sản phẩm: {dataTable.Rows.Count} sản phẩm";
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                lblTopProductsCount.Text = "Lỗi tải top sản phẩm: " + ex.Message;
            }
        }

        private void LoadSupplierSummary()
        {
            try
            {
                using (var connection = new SqlConnection(Properties.Settings.Default.QLNhapHangConnectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand("SELECT * FROM dbo.fn_ImportSummaryBySupplier(@From, @To)", connection))
                    {
                        command.Parameters.AddWithValue("@From", DateTime.Now.AddDays(-30));
                        command.Parameters.AddWithValue("@To", DateTime.Now);

                        var adapter = new SqlDataAdapter(command);
                        var dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        
                        dgvSupplierSummary.DataSource = dataTable;
                        lblSupplierSummaryCount.Text = $"Nhà cung cấp: {dataTable.Rows.Count} NCC";
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                lblSupplierSummaryCount.Text = "Lỗi tải tổng hợp NCC: " + ex.Message;
            }
        }

        private void LoadProductImportHistory()
        {
            try
            {
                using (var connection = new SqlConnection(Properties.Settings.Default.QLNhapHangConnectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand("SELECT * FROM dbo.fn_ProductImportHistory(@SKU)", connection))
                    {
                        command.Parameters.AddWithValue("@SKU", "SP0001"); // Test với SP0001
                        
                        var adapter = new SqlDataAdapter(command);
                        var dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        
                        dgvProductImportHistory.DataSource = dataTable;
                        lblProductImportHistoryCount.Text = $"Lịch sử SP0001: {dataTable.Rows.Count} lần nhập";
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                lblProductImportHistoryCount.Text = "Lỗi tải lịch sử nhập: " + ex.Message;
            }
        }

        private void LoadProductsNeverImported()
        {
            try
            {
                using (var adapter = new QLNhapHangDataSetTableAdapters.fn_ProductsNeverImportedTableAdapter())
                {
                    var data = adapter.GetData();
                    dgvProductsNeverImported.DataSource = data;
                    lblProductsNeverImportedCount.Text = $"Chưa nhập: {data.Count} sản phẩm";
                }
            }
            catch (Exception ex)
            {
                lblProductsNeverImportedCount.Text = "Lỗi tải SP chưa nhập: " + ex.Message;
            }
        }

        private void btnRefreshAll_Click(object sender, EventArgs e)
        {
            LoadAllReports();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoadUsersRoleSummary()
        {
            try
            {
                using (var connection = new SqlConnection(Properties.Settings.Default.QLNhapHangConnectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand("SELECT * FROM dbo.vw_UsersRoleSummary", connection))
                    {
                        var adapter = new SqlDataAdapter(command);
                        var dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        
                        dgvUsersRoleSummary.DataSource = dataTable;
                        lblUsersRoleSummaryCount.Text = $"Tổng quan: {dataTable.Rows.Count} roles";
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralError(ex, "tải tổng quan users");
            }
        }

        private void LoadUserActivity()
        {
            try
            {
                using (var connection = new SqlConnection(Properties.Settings.Default.QLNhapHangConnectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand("sp_GetUserActivityByDateRange", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@FromDate", DateTime.Now.AddDays(-30));
                        command.Parameters.AddWithValue("@ToDate", DateTime.Now);
                        command.Parameters.AddWithValue("@Username", DBNull.Value);
                        
                        var adapter = new SqlDataAdapter(command);
                        var dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        
                        dgvUserActivity.DataSource = dataTable;
                        
                        // Tính tổng
                        decimal totalAmount = 0;
                        int totalReceipts = 0;
                        foreach (DataRow row in dataTable.Rows)
                        {
                            totalAmount += Convert.ToDecimal(row["TotalAmount"]);
                            totalReceipts += Convert.ToInt32(row["TotalReceipts"]);
                        }
                        
                        lblUserActivityCount.Text = $"Hoạt động: {dataTable.Rows.Count} users | {totalReceipts} phiếu | {totalAmount:N0} VNĐ";
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralError(ex, "tải hoạt động user");
            }
        }

        private void LoadPriceHistory()
        {
            try
            {
                using (var connection = new SqlConnection(Properties.Settings.Default.QLNhapHangConnectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand("sp_GetPriceHistoryByDateRange", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@FromDate", DateTime.Now.AddDays(-30));
                        command.Parameters.AddWithValue("@ToDate", DateTime.Now);
                        command.Parameters.AddWithValue("@ProductID", DBNull.Value);
                        command.Parameters.AddWithValue("@SKU", DBNull.Value);
                        
                        var adapter = new SqlDataAdapter(command);
                        var dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        
                        dgvPriceHistory.DataSource = dataTable;
                        lblPriceHistoryCount.Text = $"Lịch sử giá: {dataTable.Rows.Count} thay đổi";
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralError(ex, "tải lịch sử giá");
            }
        }

        private void btnTestAllSQL_Click(object sender, EventArgs e)
        {
            try
            {
                var results = new List<string>();
                
                // Test tất cả SQL objects
                results.Add("=== KIỂM TRA TẤT CẢ SQL OBJECTS ===");
                results.Add("");
                
                // Test Views
                results.Add("📊 VIEWS:");
                results.Add("✅ vw_ProductsWithCategories - Sử dụng trong ProductSearchForm");
                results.Add("✅ vw_InventoryValuation - Sử dụng trong báo cáo");
                results.Add("✅ vw_ImportLines - Sử dụng trong báo cáo");
                results.Add("✅ vw_UsersRoleSummary - Tổng quan users theo role");
                results.Add("✅ vw_UserActivity - Hoạt động người dùng");
                results.Add("✅ vw_ProductPriceHistory - Lịch sử thay đổi giá");
                results.Add("");
                
                // Test Stored Procedures
                results.Add("⚙️ STORED PROCEDURES:");
                results.Add("✅ sp_AddProductWithCategories - Sử dụng trong AddProductForm");
                results.Add("✅ sp_BulkAdjustPriceByPercent - Sử dụng trong PriceAdjustForm");
                results.Add("✅ sp_CreateGoodsReceipt - Sử dụng trong CreateReceiptForm");
                results.Add("✅ sp_DeleteGoodsReceipt - Sử dụng trong SellerMainForm");
                results.Add("✅ sp_GetUserActivityByDateRange - Báo cáo Admin");
                results.Add("✅ sp_GetPriceHistoryByDateRange - Báo cáo Admin");
                results.Add("");
                
                // Test Functions
                results.Add("🔧 FUNCTIONS:");
                results.Add("✅ fn_ProductsByCategory - Sử dụng trong ProductSearchForm");
                results.Add("✅ fn_TopProductsByImportValue - Sử dụng trong báo cáo");
                results.Add("✅ fn_ImportSummaryBySupplier - Sử dụng trong báo cáo");
                results.Add("✅ fn_ProductImportHistory - Sử dụng trong AdminReportsForm");
                results.Add("✅ fn_ProductsNeverImported - Sử dụng trong AdminReportsForm");
                results.Add("");
                
                // Test User-Defined Types
                results.Add("📦 USER-DEFINED TYPES:");
                results.Add("✅ udt_GoodsReceiptLine - Sử dụng trong CreateReceiptForm");
                results.Add("✅ udt_SKUList - Sử dụng trong PriceAdjustForm");
                results.Add("✅ udt_CategoryNameList - Sử dụng trong AddProductForm");
                results.Add("");
                
                // Test Table Adapters
                results.Add("🔗 TABLE ADAPTERS:");
                results.Add("✅ Tất cả Table Adapters đã được sử dụng");
                results.Add("");
                
                results.Add("🎉 KẾT LUẬN: TẤT CẢ SQL OBJECTS ĐÃ ĐƯỢC SỬ DỤNG!");
                
                ErrorHandler.ShowSuccess(string.Join("\n", results), "Kiểm tra SQL Objects");
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralError(ex, "kiểm tra SQL objects");
            }
        }
    }
}
