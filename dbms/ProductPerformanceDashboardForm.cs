using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Configuration;
using System.Drawing;
using System.Linq;

namespace dbms
{
    public partial class ProductPerformanceDashboardForm : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["QLNhapHangConnectionString"].ConnectionString;

        public ProductPerformanceDashboardForm()
        {
            InitializeComponent();
            InitializeForm();
        }

        private void InitializeForm()
        {
            LoadProductPerformanceDashboard();
            LoadLowStockAlerts();
        }

        private void LoadProductPerformanceDashboard()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM dbo.vw_ProductPerformanceDashboard ORDER BY InventoryValue DESC";
                    
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            dgvPerformance.DataSource = dt;
                            
                            FormatPerformanceGrid();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.ShowError("Lỗi tải dashboard hiệu suất", ex.Message);
            }
        }

        private void LoadLowStockAlerts()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM dbo.vw_LowStockAlerts ORDER BY Priority, CurrentStock";
                    
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            dgvLowStock.DataSource = dt;
                            
                            FormatLowStockGrid();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.ShowError("Lỗi tải cảnh báo tồn kho", ex.Message);
            }
        }

        private void FormatPerformanceGrid()
        {
            if (dgvPerformance.Columns.Count == 0) return;

            // Hide ProductID
            if (dgvPerformance.Columns["ProductID"] != null)
                dgvPerformance.Columns["ProductID"].Visible = false;

            // Format headers
            if (dgvPerformance.Columns["SKU"] != null)
                dgvPerformance.Columns["SKU"].HeaderText = "Mã SP";
            if (dgvPerformance.Columns["ProductName"] != null)
                dgvPerformance.Columns["ProductName"].HeaderText = "Tên sản phẩm";
            if (dgvPerformance.Columns["SellingPrice"] != null)
                dgvPerformance.Columns["SellingPrice"].HeaderText = "Giá bán";
            if (dgvPerformance.Columns["StockQuantity"] != null)
                dgvPerformance.Columns["StockQuantity"].HeaderText = "Tồn kho";
            if (dgvPerformance.Columns["LastImportPrice"] != null)
                dgvPerformance.Columns["LastImportPrice"].HeaderText = "Giá nhập cuối";
            if (dgvPerformance.Columns["ProfitMargin"] != null)
                dgvPerformance.Columns["ProfitMargin"].HeaderText = "Lợi nhuận (%)";
            if (dgvPerformance.Columns["InventoryValue"] != null)
                dgvPerformance.Columns["InventoryValue"].HeaderText = "Giá trị tồn kho";
            if (dgvPerformance.Columns["TotalImports"] != null)
                dgvPerformance.Columns["TotalImports"].HeaderText = "Số lần nhập";
            if (dgvPerformance.Columns["LastImportDate"] != null)
                dgvPerformance.Columns["LastImportDate"].HeaderText = "Nhập cuối";
            if (dgvPerformance.Columns["DaysSinceLastImport"] != null)
                dgvPerformance.Columns["DaysSinceLastImport"].HeaderText = "Ngày từ nhập cuối";
            if (dgvPerformance.Columns["StockStatus"] != null)
                dgvPerformance.Columns["StockStatus"].HeaderText = "Trạng thái tồn kho";
            if (dgvPerformance.Columns["AgingStatus"] != null)
                dgvPerformance.Columns["AgingStatus"].HeaderText = "Trạng thái tuổi";

            // Format currency columns
            string[] currencyColumns = { "SellingPrice", "LastImportPrice", "InventoryValue" };
            foreach (string colName in currencyColumns)
            {
                if (dgvPerformance.Columns[colName] != null)
                {
                    dgvPerformance.Columns[colName].DefaultCellStyle.Format = "N0";
                    dgvPerformance.Columns[colName].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            }

            // Format percentage columns
            if (dgvPerformance.Columns["ProfitMargin"] != null)
            {
                dgvPerformance.Columns["ProfitMargin"].DefaultCellStyle.Format = "N2";
                dgvPerformance.Columns["ProfitMargin"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            // Color code status columns
            foreach (DataGridViewRow row in dgvPerformance.Rows)
            {
                if (row.Cells["StockStatus"] != null && row.Cells["StockStatus"].Value != null)
                {
                    string status = row.Cells["StockStatus"].Value.ToString();
                    if (status == "Hết hàng")
                        row.Cells["StockStatus"].Style.BackColor = Color.Red;
                    else if (status == "Sắp hết")
                        row.Cells["StockStatus"].Style.BackColor = Color.Orange;
                    else if (status == "Thấp")
                        row.Cells["StockStatus"].Style.BackColor = Color.Yellow;
                }

                if (row.Cells["AgingStatus"] != null && row.Cells["AgingStatus"].Value != null)
                {
                    string aging = row.Cells["AgingStatus"].Value.ToString();
                    if (aging == "Tồn lâu")
                        row.Cells["AgingStatus"].Style.BackColor = Color.LightCoral;
                    else if (aging == "Cảnh báo")
                        row.Cells["AgingStatus"].Style.BackColor = Color.LightYellow;
                }
            }
        }

        private void FormatLowStockGrid()
        {
            if (dgvLowStock.Columns.Count == 0) return;

            // Hide ProductID and Priority columns
            if (dgvLowStock.Columns["ProductID"] != null)
                dgvLowStock.Columns["ProductID"].Visible = false;
            if (dgvLowStock.Columns["Priority"] != null)
                dgvLowStock.Columns["Priority"].Visible = false;

            // Format headers
            if (dgvLowStock.Columns["SKU"] != null)
                dgvLowStock.Columns["SKU"].HeaderText = "Mã SP";
            if (dgvLowStock.Columns["ProductName"] != null)
                dgvLowStock.Columns["ProductName"].HeaderText = "Tên sản phẩm";
            if (dgvLowStock.Columns["CurrentStock"] != null)
                dgvLowStock.Columns["CurrentStock"].HeaderText = "Tồn kho";
            if (dgvLowStock.Columns["SellingPrice"] != null)
                dgvLowStock.Columns["SellingPrice"].HeaderText = "Giá bán";
            if (dgvLowStock.Columns["Categories"] != null)
                dgvLowStock.Columns["Categories"].HeaderText = "Danh mục";
            if (dgvLowStock.Columns["LastImportDate"] != null)
                dgvLowStock.Columns["LastImportDate"].HeaderText = "Nhập cuối";
            if (dgvLowStock.Columns["DaysSinceLastImport"] != null)
                dgvLowStock.Columns["DaysSinceLastImport"].HeaderText = "Ngày từ nhập cuối";
            if (dgvLowStock.Columns["AlertLevel"] != null)
                dgvLowStock.Columns["AlertLevel"].HeaderText = "Mức cảnh báo";

            // Format currency columns
            if (dgvLowStock.Columns["SellingPrice"] != null)
            {
                dgvLowStock.Columns["SellingPrice"].DefaultCellStyle.Format = "N0";
                dgvLowStock.Columns["SellingPrice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            // Color code alert levels
            foreach (DataGridViewRow row in dgvLowStock.Rows)
            {
                if (row.Cells["AlertLevel"] != null && row.Cells["AlertLevel"].Value != null)
                {
                    string alert = row.Cells["AlertLevel"].Value.ToString();
                    if (alert.Contains("Khẩn cấp"))
                        row.DefaultCellStyle.BackColor = Color.MistyRose;
                    else if (alert.Contains("Cao"))
                        row.DefaultCellStyle.BackColor = Color.PeachPuff;
                    else if (alert.Contains("Trung bình"))
                        row.DefaultCellStyle.BackColor = Color.LightYellow;
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadProductPerformanceDashboard();
            LoadLowStockAlerts();
        }

        private void btnExportPerformance_Click(object sender, EventArgs e)
        {
            ExportToCSV(dgvPerformance, "ProductPerformance");
        }

        private void btnExportLowStock_Click(object sender, EventArgs e)
        {
            ExportToCSV(dgvLowStock, "LowStockAlerts");
        }

        private void ExportToCSV(DataGridView dgv, string fileName)
        {
            try
            {
                SaveFileDialog saveDialog = new SaveFileDialog
                {
                    Filter = "CSV Files|*.csv",
                    Title = "Xuất dữ liệu",
                    FileName = $"{fileName}_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
                };

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    using (System.IO.StreamWriter writer = new System.IO.StreamWriter(saveDialog.FileName, false, System.Text.Encoding.UTF8))
                    {
                        // Write headers
                        for (int i = 0; i < dgv.Columns.Count; i++)
                        {
                            if (dgv.Columns[i].Visible)
                            {
                                writer.Write(dgv.Columns[i].HeaderText);
                                if (i < dgv.Columns.Count - 1) writer.Write(",");
                            }
                        }
                        writer.WriteLine();

                        // Write data
                        foreach (DataGridViewRow row in dgv.Rows)
                        {
                            if (!row.IsNewRow)
                            {
                                for (int i = 0; i < dgv.Columns.Count; i++)
                                {
                                    if (dgv.Columns[i].Visible)
                                    {
                                        writer.Write(row.Cells[i].Value?.ToString() ?? "");
                                        if (i < dgv.Columns.Count - 1) writer.Write(",");
                                    }
                                }
                                writer.WriteLine();
                            }
                        }
                    }

                    MessageBox.Show("Xuất dữ liệu thành công!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.ShowError("Lỗi xuất dữ liệu", ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
