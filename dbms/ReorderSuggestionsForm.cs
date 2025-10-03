using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace dbms
{
    public partial class ReorderSuggestionsForm : Form
    {
        private string connectionString;

        public ReorderSuggestionsForm()
        {
            InitializeComponent();
            connectionString = Properties.Settings.Default.QLNhapHangConnectionString;
        }

        private void ReorderSuggestionsForm_Load(object sender, EventArgs e)
        {
            // Set default values
            numDaysToProject.Value = 30;
            numServiceLevel.Value = 95;
            
            LoadReorderSuggestions();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            LoadReorderSuggestions();
        }

        private void LoadReorderSuggestions()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("dbo.sp_GenerateReorderSuggestions", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 60;

                        cmd.Parameters.AddWithValue("@DaysToProject", (int)numDaysToProject.Value);
                        cmd.Parameters.AddWithValue("@ServiceLevel", (decimal)numServiceLevel.Value);

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgvSuggestions.DataSource = dt;
                        
                        // Format columns
                        if (dgvSuggestions.Columns["ProductID"] != null)
                            dgvSuggestions.Columns["ProductID"].Visible = false;
                        
                        if (dgvSuggestions.Columns["CurrentStock"] != null)
                            dgvSuggestions.Columns["CurrentStock"].DefaultCellStyle.Format = "N0";
                        
                        if (dgvSuggestions.Columns["ReorderPoint"] != null)
                            dgvSuggestions.Columns["ReorderPoint"].DefaultCellStyle.Format = "N0";
                        
                        if (dgvSuggestions.Columns["SafetyStock"] != null)
                            dgvSuggestions.Columns["SafetyStock"].DefaultCellStyle.Format = "N0";
                        
                        if (dgvSuggestions.Columns["SuggestedOrderQty"] != null)
                        {
                            dgvSuggestions.Columns["SuggestedOrderQty"].DefaultCellStyle.Format = "N0";
                            dgvSuggestions.Columns["SuggestedOrderQty"].DefaultCellStyle.BackColor = System.Drawing.Color.LightYellow;
                        }
                        
                        if (dgvSuggestions.Columns["AvgDailyUsage"] != null)
                            dgvSuggestions.Columns["AvgDailyUsage"].DefaultCellStyle.Format = "N2";
                        
                        if (dgvSuggestions.Columns["TurnoverRate"] != null)
                            dgvSuggestions.Columns["TurnoverRate"].DefaultCellStyle.Format = "N2";
                        
                        if (dgvSuggestions.Columns["AvgMonthlyImport"] != null)
                            dgvSuggestions.Columns["AvgMonthlyImport"].DefaultCellStyle.Format = "N2";
                        
                        if (dgvSuggestions.Columns["EstimatedStockoutDays"] != null)
                            dgvSuggestions.Columns["EstimatedStockoutDays"].DefaultCellStyle.Format = "N0";
                        
                        // Color code by Priority
                        if (dgvSuggestions.Columns["Priority"] != null)
                        {
                            foreach (DataGridViewRow row in dgvSuggestions.Rows)
                            {
                                if (row.Cells["Priority"].Value != null)
                                {
                                    string priority = row.Cells["Priority"].Value.ToString();
                                    if (priority.Contains("Khẩn cấp"))
                                        row.Cells["Priority"].Style.BackColor = System.Drawing.Color.Red;
                                    else if (priority.Contains("Cao"))
                                        row.Cells["Priority"].Style.BackColor = System.Drawing.Color.Orange;
                                    else if (priority.Contains("Trung bình"))
                                        row.Cells["Priority"].Style.BackColor = System.Drawing.Color.Yellow;
                                }
                            }
                        }

                        lblResultCount.Text = $"Tìm thấy {dt.Rows.Count} sản phẩm cần nhập hàng";
                        
                        if (dt.Rows.Count == 0)
                        {
                            ErrorHandler.ShowSuccess("Không có sản phẩm nào cần nhập hàng!", "Thông báo");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralError(ex, "tải gợi ý nhập hàng");
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void btnExportToExcel_Click(object sender, EventArgs e)
        {
            if (dgvSuggestions.Rows.Count == 0)
            {
                ErrorHandler.ShowWarning("Không có dữ liệu để export!", "⚠️ Không có dữ liệu");
                return;
            }

            try
            {
                // Simple CSV export
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "CSV File|*.csv";
                sfd.FileName = $"ReorderSuggestions_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(sfd.FileName, false, System.Text.Encoding.UTF8))
                    {
                        // Header
                        for (int i = 0; i < dgvSuggestions.Columns.Count; i++)
                        {
                            if (dgvSuggestions.Columns[i].Visible)
                            {
                                sw.Write(dgvSuggestions.Columns[i].HeaderText);
                                if (i < dgvSuggestions.Columns.Count - 1)
                                    sw.Write(",");
                            }
                        }
                        sw.WriteLine();

                        // Rows
                        foreach (DataGridViewRow row in dgvSuggestions.Rows)
                        {
                            if (!row.IsNewRow)
                            {
                                for (int i = 0; i < dgvSuggestions.Columns.Count; i++)
                                {
                                    if (dgvSuggestions.Columns[i].Visible)
                                    {
                                        sw.Write(row.Cells[i].Value?.ToString() ?? "");
                                        if (i < dgvSuggestions.Columns.Count - 1)
                                            sw.Write(",");
                                    }
                                }
                                sw.WriteLine();
                            }
                        }
                    }

                    ErrorHandler.ShowSuccess($"Export thành công!\nFile: {sfd.FileName}");
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralError(ex, "export dữ liệu");
            }
        }

        private void btnViewDetails_Click(object sender, EventArgs e)
        {
            if (dgvSuggestions.SelectedRows.Count == 0)
            {
                ErrorHandler.ShowWarning("Vui lòng chọn sản phẩm!", "⚠️ Chưa chọn sản phẩm");
                return;
            }

            try
            {
                int productID = Convert.ToInt32(dgvSuggestions.SelectedRows[0].Cells["ProductID"].Value);
                string productName = dgvSuggestions.SelectedRows[0].Cells["ProductName"].Value.ToString();
                int currentStock = Convert.ToInt32(dgvSuggestions.SelectedRows[0].Cells["CurrentStock"].Value);
                int reorderPoint = Convert.ToInt32(dgvSuggestions.SelectedRows[0].Cells["ReorderPoint"].Value);
                int suggestedQty = Convert.ToInt32(dgvSuggestions.SelectedRows[0].Cells["SuggestedOrderQty"].Value);
                string priority = dgvSuggestions.SelectedRows[0].Cells["Priority"].Value.ToString();

                string details = $"=== CHI TIẾT SẢN PHẨM ===\n\n" +
                    $"Sản phẩm: {productName}\n" +
                    $"Tồn kho hiện tại: {currentStock:N0}\n" +
                    $"Reorder Point: {reorderPoint:N0}\n" +
                    $"Số lượng đề xuất nhập: {suggestedQty:N0}\n" +
                    $"Mức độ ưu tiên: {priority}\n\n" +
                    $"=== PHÂN TÍCH ===\n" +
                    $"• Sản phẩm đã dưới điểm đặt hàng\n" +
                    $"• Cần nhập hàng ngay để tránh hết hàng\n" +
                    $"• Dự trữ cho {numDaysToProject.Value} ngày tới";

                ErrorHandler.ShowSuccess(details, "Chi tiết Reorder Suggestion");
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralError(ex, "tải gợi ý nhập hàng");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            string helpText = @"=== HƯỚNG DẪN SỬ DỤNG ===

1. THAM SỐ:
   • Days to Project: Số ngày dự trữ (mặc định 30)
   • Service Level: Mức độ phục vụ 90-99% (mặc định 95%)

2. MỨC ĐỘ ƯU TIÊN:
   🔴 Khẩn cấp: Tồn kho <= Safety Stock
   🟠 Cao: Tồn kho <= 50% Reorder Point
   🟡 Trung bình: Tồn kho <= Reorder Point

3. SUGGESTED ORDER QTY:
   Số lượng cần nhập = (Avg Daily Usage × Days) + Safety Stock - Current Stock

4. ACTIONS:
   • Generate: Tạo đề xuất mới
   • Export: Xuất ra file CSV
   • View Details: Xem chi tiết sản phẩm";

            ErrorHandler.ShowSuccess(helpText, "Hướng dẫn");
        }

        // New method to calculate reorder point for individual products
        private DataTable CalculateReorderPointForProduct(int productId, int leadTimeDays = 7, decimal serviceLevel = 95.0m)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM dbo.fn_CalculateReorderPoint(@ProductID, @LeadTimeDays, @ServiceLevel)";
                    
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProductID", productId);
                        cmd.Parameters.AddWithValue("@LeadTimeDays", leadTimeDays);
                        cmd.Parameters.AddWithValue("@ServiceLevel", serviceLevel);

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralError(ex, "tính toán reorder point");
                return new DataTable();
            }
        }
    }
}
