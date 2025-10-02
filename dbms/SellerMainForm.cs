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
    public partial class SellerMainForm : Form
    {
        private QLNhapHangDataSet dataSet;
        private SqlConnection connection;
        private int currentUserID;
        private string currentUsername;
        private string currentUserRole;

        public SellerMainForm()
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
                MessageBox.Show("Lỗi kết nối database: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadInitialData()
        {
            try
            {
                LoadProducts();
                LoadCategories();
                LoadGoodsReceipts();
                LoadGoodsReceiptDetails();
                LoadSuppliers();
                
                toolStripStatusLabel1.Text = "Dữ liệu đã được tải thành công";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region Form Events
        private void Form1_Load(object sender, EventArgs e) { }
        #endregion

        #region Products Tab Events
        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            try
            {
                var addProductForm = new AddProductForm();
                if (addProductForm.ShowDialog() == DialogResult.OK)
                {
                    LoadProducts();
                    LoadCategories();
                    MessageBox.Show("Thêm sản phẩm thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdatePrice_Click(object sender, EventArgs e)
        {
            try
            {
                var priceAdjustForm = new PriceAdjustForm();
                if (priceAdjustForm.ShowDialog() == DialogResult.OK)
                {
                    LoadProducts();
                    MessageBox.Show("Điều chỉnh giá thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi điều chỉnh giá: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefreshProducts_Click(object sender, EventArgs e)
        {
            LoadProducts();
            LoadCategories();
        }
        #endregion

        #region Goods Receipt Tab Events
        private void btnCreateReceipt_Click(object sender, EventArgs e)
        {
            try
            {
                var createReceiptForm = new CreateReceiptForm(currentUserID, currentUsername);
                if (createReceiptForm.ShowDialog() == DialogResult.OK)
                {
                    LoadGoodsReceipts();
                    LoadGoodsReceiptDetails();
                    LoadProducts(); // Refresh để thấy tồn kho mới
                    MessageBox.Show("Tạo phiếu nhập thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tạo phiếu nhập: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeleteReceipt_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvGoodsReceipts.SelectedRows.Count > 0)
                {
                    var receiptId = dgvGoodsReceipts.SelectedRows[0].Cells["ReceiptID"].Value;
                    
                    var result = MessageBox.Show($"Bạn có chắc muốn xóa phiếu nhập ID {receiptId}?", 
                        "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    
                    if (result == DialogResult.Yes)
                    {
                        using (var command = new SqlCommand("sp_DeleteGoodsReceipt", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@ReceiptID", receiptId);

                            connection.Open();
                            command.ExecuteNonQuery();
                            connection.Close();

                            MessageBox.Show("Xóa phiếu nhập thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            
                            LoadGoodsReceipts();
                            LoadGoodsReceiptDetails();
                            LoadProducts();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn phiếu nhập cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa phiếu nhập: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (connection.State == ConnectionState.Open) connection.Close();
            }
        }

        private void btnRefreshReceipts_Click(object sender, EventArgs e)
        {
            LoadGoodsReceipts();
            LoadGoodsReceiptDetails();
        }
        #endregion

        #region Reports Tab Events
        private void btnLoadInventory_Click(object sender, EventArgs e)
        {
            try
            {
                using (var adapter = new QLNhapHangDataSetTableAdapters.vw_InventoryValuationTableAdapter())
                {
                    var data = adapter.GetData();
                    dgvInventoryValuation.DataSource = data;
                    toolStripStatusLabel1.Text = $"Đã tải {data.Count} sản phẩm từ báo cáo định giá tồn kho";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải báo cáo tồn kho: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLoadImportLines_Click(object sender, EventArgs e)
        {
            try
            {
                using (var adapter = new QLNhapHangDataSetTableAdapters.vw_ImportLinesTableAdapter())
                {
                    var data = adapter.GetData();
                    dgvImportLines.DataSource = data;
                    toolStripStatusLabel1.Text = $"Đã tải {data.Count} dòng nhập hàng";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dòng nhập hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLoadTopProducts_Click(object sender, EventArgs e)
        {
            try
            {
                using (var command = new SqlCommand("SELECT * FROM dbo.fn_TopProductsByImportValue(@From, @To, @TopN, NULL, NULL)", connection))
                {
                    command.Parameters.AddWithValue("@From", DateTime.Now.AddDays(-30));
                    command.Parameters.AddWithValue("@To", DateTime.Now);
                    command.Parameters.AddWithValue("@TopN", 10);

                    var adapter = new SqlDataAdapter(command);
                    var dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    
                    dgvTopProducts.DataSource = dataTable;
                    toolStripStatusLabel1.Text = $"Đã tải {dataTable.Rows.Count} sản phẩm top nhập hàng";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải top sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLoadSupplierSummary_Click(object sender, EventArgs e)
        {
            try
            {
                using (var command = new SqlCommand("SELECT * FROM dbo.fn_ImportSummaryBySupplier(@From, @To)", connection))
                {
                    command.Parameters.AddWithValue("@From", DateTime.Now.AddDays(-30));
                    command.Parameters.AddWithValue("@To", DateTime.Now);

                    connection.Open();
                    var result = command.ExecuteReader();
                    
                    var summary = new List<string>();
                    while (result.Read())
                    {
                        summary.Add($"{result["SupplierName"]}: {result["TotalValue"]} - {result["TotalReceipts"]} phiếu");
                    }
                    result.Close();
                    connection.Close();

                    MessageBox.Show("Tổng hợp nhà cung cấp (30 ngày qua):\n" + string.Join("\n", summary), 
                        "Báo cáo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải tổng hợp nhà cung cấp: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (connection.State == ConnectionState.Open) connection.Close();
            }
        }
        #endregion

        #region Menu Events
        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc muốn đăng xuất?", "Xác nhận", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (result == DialogResult.Yes)
            {
                this.Close();
                Application.Restart();
            }
        }

        private void thoátToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc muốn thoát ứng dụng?", "Xác nhận", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
        #endregion

        #region Data Loading Methods
        private void LoadProducts()
        {
            try
            {
                using (var adapter = new QLNhapHangDataSetTableAdapters.ProductsTableAdapter())
                {
                    var data = adapter.GetData();
                    dgvProducts.DataSource = data;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadCategories()
        {
            try
            {
                using (var adapter = new QLNhapHangDataSetTableAdapters.CategoriesTableAdapter())
                {
                    var data = adapter.GetData();
                    dgvCategories.DataSource = data;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh mục: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadGoodsReceipts()
        {
            try
            {
                using (var adapter = new QLNhapHangDataSetTableAdapters.GoodsReceiptsTableAdapter())
                {
                    var data = adapter.GetData();
                    dgvGoodsReceipts.DataSource = data;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải phiếu nhập: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadGoodsReceiptDetails()
        {
            try
            {
                using (var adapter = new QLNhapHangDataSetTableAdapters.GoodsReceiptDetailsTableAdapter())
                {
                    var data = adapter.GetData();
                    dgvGoodsReceiptDetails.DataSource = data;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải chi tiết phiếu nhập: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadSuppliers()
        {
            try
            {
                using (var adapter = new QLNhapHangDataSetTableAdapters.SuppliersTableAdapter())
                {
                    var data = adapter.GetData();
                    dgvSuppliers.DataSource = data;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải nhà cung cấp: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Advanced Features Menu
        private void smartPricingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SmartPricingForm form = new SmartPricingForm(currentUserID);
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi mở Smart Pricing: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void reorderSuggestionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ReorderSuggestionsForm form = new ReorderSuggestionsForm();
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi mở Reorder Suggestions: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void advancedAnalyticsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                AdvancedAnalyticsForm form = new AdvancedAnalyticsForm();
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi mở Advanced Analytics: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void supplierAnalysisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SupplierAnalysisForm form = new SupplierAnalysisForm();
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi mở Supplier Analysis: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion
    }
}
