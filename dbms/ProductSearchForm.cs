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
    public partial class ProductSearchForm : Form
    {
        private QLNhapHangDataSet dataSet;
        private bool columnsFormatted = false;

        public ProductSearchForm()
        {
            InitializeComponent();
            
            // Đăng ký event để format columns sau khi data binding hoàn tất
            dgvProducts.DataBindingComplete += DgvProducts_DataBindingComplete;
            
            InitializeData();
            
            // Cải thiện styling cho DataGridView
            StyleDataGridView();
        }

        // Event handler để format columns sau khi data binding hoàn tất
        private void DgvProducts_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                FormatDataGridViewColumns();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Lỗi trong DataBindingComplete: " + ex.Message);
            }
        }

        // Helper method để đảm bảo DataGridView an toàn
        private bool EnsureDataGridViewReady()
        {
            if (dgvProducts == null)
            {
                MessageBox.Show("DataGridView chưa được khởi tạo!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        // Method để cải thiện styling cho DataGridView
        private void StyleDataGridView()
        {
            try
            {
                // Thiết lập màu sắc cho header
                dgvProducts.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 102, 204);
                dgvProducts.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgvProducts.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
                dgvProducts.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // Thiết lập màu sắc cho alternating rows
                dgvProducts.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);

                // Thiết lập màu sắc cho selection
                dgvProducts.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 102, 204);
                dgvProducts.DefaultCellStyle.SelectionForeColor = Color.White;

                // Thiết lập font cho cells
                dgvProducts.DefaultCellStyle.Font = new Font("Microsoft YaHei UI", 9F);
                dgvProducts.DefaultCellStyle.Padding = new Padding(5);

                // Thiết lập border cho cells
                dgvProducts.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                dgvProducts.GridColor = Color.FromArgb(224, 224, 224);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thiết lập styling cho DataGridView: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Method riêng để định dạng cột DataGridView một cách an toàn
        private void FormatDataGridViewColumns()
        {
            try
            {
                // Tránh format nhiều lần
                if (columnsFormatted)
                    return;

                // Kiểm tra DataGridView và columns
                if (dgvProducts == null || dgvProducts.Columns == null || dgvProducts.Columns.Count == 0)
                    return;

                // Định dạng từng cột với kiểm tra an toàn
                if (dgvProducts.Columns.Contains("ProductID") && dgvProducts.Columns["ProductID"] != null)
                    dgvProducts.Columns["ProductID"].Visible = false;

                if (dgvProducts.Columns.Contains("SKU") && dgvProducts.Columns["SKU"] != null)
                {
                    var skuColumn = dgvProducts.Columns["SKU"];
                    if (skuColumn != null)
                        skuColumn.Width = 100;
                }

                if (dgvProducts.Columns.Contains("ProductName") && dgvProducts.Columns["ProductName"] != null)
                {
                    var nameColumn = dgvProducts.Columns["ProductName"];
                    if (nameColumn != null)
                        nameColumn.Width = 250;
                }

                if (dgvProducts.Columns.Contains("SellingPrice") && dgvProducts.Columns["SellingPrice"] != null)
                {
                    var priceColumn = dgvProducts.Columns["SellingPrice"];
                    if (priceColumn != null)
                    {
                        priceColumn.DefaultCellStyle.Format = "N2";
                        priceColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                }

                if (dgvProducts.Columns.Contains("StockQuantity") && dgvProducts.Columns["StockQuantity"] != null)
                {
                    var stockColumn = dgvProducts.Columns["StockQuantity"];
                    if (stockColumn != null)
                        stockColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

                if (dgvProducts.Columns.Contains("Categories") && dgvProducts.Columns["Categories"] != null)
                {
                    var categoryColumn = dgvProducts.Columns["Categories"];
                    if (categoryColumn != null)
                        categoryColumn.Width = 200;
                }

                // Đánh dấu đã format xong
                columnsFormatted = true;
            }
            catch (Exception ex)
            {
                // Log lỗi nhưng không làm crash ứng dụng
                System.Diagnostics.Debug.WriteLine("Lỗi định dạng cột DataGridView: " + ex.Message);
            }
        }

        private void InitializeData()
        {
            try
            {
                dataSet = new QLNhapHangDataSet();
                LoadProducts();
                LoadCategories();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khởi tạo dữ liệu: " + ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadProducts()
        {
            try
            {
                // Đảm bảo DataGridView đã được khởi tạo
                if (!EnsureDataGridViewReady())
                    return;

                // Sử dụng view vw_ProductsWithCategories để hiển thị sản phẩm với danh mục
                using (var adapter = new QLNhapHangDataSetTableAdapters.vw_ProductsWithCategoriesTableAdapter())
                {
                    var data = adapter.GetData();
                    
                    // Đảm bảo data không null trước khi gán
                    if (data != null)
                    {
                        dgvProducts.DataSource = data;
                        
                        // Format columns sẽ được gọi tự động qua DataBindingComplete event
                        lblProductCount.Text = $"Tổng số sản phẩm: {dgvProducts.Rows.Count}";
                    }
                    else
                    {
                        MessageBox.Show("Không có dữ liệu sản phẩm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách sản phẩm: " + ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadCategories()
        {
            try
            {
                using (var adapter = new QLNhapHangDataSetTableAdapters.CategoriesTableAdapter())
                {
                    var data = adapter.GetData();
                    cmbCategory.DataSource = data;
                    cmbCategory.DisplayMember = "CategoryName";
                    cmbCategory.ValueMember = "CategoryID";
                    
                    // Thêm item "Tất cả"
                    var allCategories = new DataTable();
                    allCategories.Columns.Add("CategoryID", typeof(int));
                    allCategories.Columns.Add("CategoryName", typeof(string));
                    allCategories.Rows.Add(-1, "-- Tất cả danh mục --");
                    
                    foreach (DataRow row in data.Rows)
                    {
                        allCategories.Rows.Add(row["CategoryID"], row["CategoryName"]);
                    }
                    
                    cmbCategory.DataSource = allCategories;
                    cmbCategory.DisplayMember = "CategoryName";
                    cmbCategory.ValueMember = "CategoryID";
                    cmbCategory.SelectedValue = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh mục: " + ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                // Đảm bảo DataGridView đã được khởi tạo
                if (!EnsureDataGridViewReady())
                    return;

                string searchText = txtSearch.Text.Trim();
                int selectedCategoryId = (int)cmbCategory.SelectedValue;

                if (selectedCategoryId == -1 && string.IsNullOrEmpty(searchText))
                {
                    // Hiển thị tất cả sản phẩm
                    LoadProducts();
                    return;
                }

                if (selectedCategoryId != -1)
                {
                    // Lọc theo danh mục sử dụng fn_ProductsByCategory
                    LoadProductsByCategory(selectedCategoryId, searchText);
                }
                else
                {
                    // Chỉ tìm kiếm theo tên/SKU
                    SearchProductsByName(searchText);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadProductsByCategory(int categoryId, string searchText)
        {
            try
            {
                // Đảm bảo DataGridView đã được khởi tạo
                if (!EnsureDataGridViewReady())
                    return;

                // Lấy tên danh mục
                string categoryName = cmbCategory.Text;
                
                using (var connection = new SqlConnection(Properties.Settings.Default.QLNhapHangConnectionString))
                {
                    connection.Open();
                    
                    // Sử dụng fn_ProductsByCategory để lấy sản phẩm theo danh mục
                    string query = @"
                        SELECT p.ProductID, p.SKU, p.ProductName, p.SellingPrice, p.StockQuantity,
                               STRING_AGG(c.CategoryName, ', ') WITHIN GROUP (ORDER BY c.CategoryName) AS Categories
                        FROM dbo.fn_ProductsByCategory(@CategoryName) p
                        LEFT JOIN dbo.ProductCategories pc ON pc.ProductID = p.ProductID
                        LEFT JOIN dbo.Categories c ON c.CategoryID = pc.CategoryID
                        WHERE (@SearchText = '' OR p.ProductName LIKE @SearchPattern OR p.SKU LIKE @SearchPattern)
                        GROUP BY p.ProductID, p.SKU, p.ProductName, p.SellingPrice, p.StockQuantity";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CategoryName", categoryName);
                        command.Parameters.AddWithValue("@SearchText", searchText);
                        command.Parameters.AddWithValue("@SearchPattern", "%" + searchText + "%");

                        var adapter = new SqlDataAdapter(command);
                        var dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        
                        dgvProducts.DataSource = dataTable;
                        
                        if (dataTable.Rows.Count > 0)
                        {
                            // Format columns sẽ được gọi tự động qua DataBindingComplete event
                        }
                    }
                }
                
                lblProductCount.Text = $"Số sản phẩm tìm thấy: {dgvProducts.Rows.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lọc theo danh mục: " + ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SearchProductsByName(string searchText)
        {
            try
            {
                // Đảm bảo DataGridView đã được khởi tạo
                if (!EnsureDataGridViewReady())
                    return;

                // Tìm kiếm trong view vw_ProductsWithCategories
                using (var connection = new SqlConnection(Properties.Settings.Default.QLNhapHangConnectionString))
                {
                    connection.Open();
                    
                    string query = @"
                        SELECT * FROM dbo.vw_ProductsWithCategories 
                        WHERE ProductName LIKE @SearchPattern OR SKU LIKE @SearchPattern";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@SearchPattern", "%" + searchText + "%");

                        var adapter = new SqlDataAdapter(command);
                        var dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        
                        dgvProducts.DataSource = dataTable;
                        
                        if (dataTable.Rows.Count > 0)
                        {
                            // Format columns sẽ được gọi tự động qua DataBindingComplete event
                        }
                    }
                }
                
                lblProductCount.Text = $"Số sản phẩm tìm thấy: {dgvProducts.Rows.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            cmbCategory.SelectedValue = -1;
            LoadProducts();
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnSearch_Click(sender, e);
            }
        }

        private void cmbCategory_SelectionChangeCommitted(object sender, EventArgs e)
        {
            btnSearch_Click(sender, e);
        }

        private void dgvProducts_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Định dạng giá tiền
            if (dgvProducts.Columns[e.ColumnIndex].Name == "SellingPrice" && e.Value != null)
            {
                if (decimal.TryParse(e.Value.ToString(), out decimal price))
                {
                    e.Value = price.ToString("N2") + " VNĐ";
                    e.FormattingApplied = true;
                }
            }
            
            // Định dạng màu cho cột tồn kho
            if (dgvProducts.Columns[e.ColumnIndex].Name == "StockQuantity" && e.Value != null)
            {
                if (int.TryParse(e.Value.ToString(), out int stock))
                {
                    if (stock == 0)
                    {
                        e.CellStyle.ForeColor = Color.Red;
                        e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
                    }
                    else if (stock < 10)
                    {
                        e.CellStyle.ForeColor = Color.Orange;
                        e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
                    }
                    else
                    {
                        e.CellStyle.ForeColor = Color.Green;
                    }
                }
            }
        }
    }
}
