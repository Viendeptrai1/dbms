using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace dbms
{
    public partial class SupplierAnalysisForm : Form
    {
        private string connectionString;

        public SupplierAnalysisForm()
        {
            InitializeComponent();
            connectionString = Properties.Settings.Default.QLNhapHangConnectionString;
        }

        private void SupplierAnalysisForm_Load(object sender, EventArgs e)
        {
            numMonths.Value = 6;
            LoadSupplierList();
        }

        private void LoadSupplierList()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT SupplierID, SupplierName FROM dbo.Suppliers ORDER BY SupplierName";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    cmbSupplier.DataSource = dt;
                    cmbSupplier.DisplayMember = "SupplierName";
                    cmbSupplier.ValueMember = "SupplierID";
                    cmbSupplier.SelectedIndex = -1;
                }
            }
            catch (SqlException sqlEx)
            {
                ErrorHandler.HandleSqlError(sqlEx, "tải danh sách nhà cung cấp");
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralError(ex, "tải danh sách nhà cung cấp");
            }
        }

        private void btnAnalyze_Click(object sender, EventArgs e)
        {
            if (cmbSupplier.SelectedIndex < 0)
            {
                ErrorHandler.ShowWarning("Vui lòng chọn nhà cung cấp!", "⚠️ Thiếu thông tin");
                return;
            }

            int supplierID = Convert.ToInt32(cmbSupplier.SelectedValue);
            LoadSupplierAnalysis(supplierID);
        }

        private void LoadSupplierAnalysis(int supplierID)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    
                    // Get performance score
                    string query = "SELECT * FROM dbo.fn_SupplierPerformanceScore(@SupplierID, @Months)";
                    
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@SupplierID", supplierID);
                        cmd.Parameters.AddWithValue("@Months", (int)numMonths.Value);

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            DataRow row = dt.Rows[0];
                            
                            DisplayScoreDetails(row);
                            DisplayScoreGauge(Convert.ToInt32(row["TotalScore"]));
                        }
                        else
                        {
                            txtDetails.Text = "No data available for this supplier in the selected period.";
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                ErrorHandler.HandleSqlError(sqlEx, "phân tích nhà cung cấp");
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralError(ex, "phân tích nhà cung cấp");
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void DisplayScoreDetails(DataRow row)
        {
            string supplierName = cmbSupplier.Text;
            
            string details = $"=== SUPPLIER PERFORMANCE ANALYSIS ===\n\n" +
                $"Supplier: {supplierName}\n" +
                $"Analysis Period: {numMonths.Value} months\n\n" +
                $"--- METRICS ---\n" +
                $"Total Receipts: {row["TotalReceipts"]}\n" +
                $"Total Value: {Convert.ToDecimal(row["TotalValue"]):N0} VNĐ\n" +
                $"Avg Receipt Value: {Convert.ToDecimal(row["AvgReceiptValue"]):N0} VNĐ\n" +
                $"Total Products: {row["TotalProducts"]}\n" +
                $"Price Volatility: {Convert.ToDecimal(row["PriceVolatility"]):N0}\n\n" +
                $"--- SCORING (0-100) ---\n" +
                $"Volume Score: {row["VolumeScore"]}/30\n" +
                $"  → Based on number of receipts\n\n" +
                $"Value Score: {row["ValueScore"]}/30\n" +
                $"  → Based on total import value\n\n" +
                $"Consistency Score: {row["ConsistencyScore"]}/20\n" +
                $"  → Based on price stability\n\n" +
                $"Frequency Score: {row["FrequencyScore"]}/20\n" +
                $"  → Based on receipt frequency\n\n" +
                $"═══════════════════════════════\n" +
                $"TOTAL SCORE: {row["TotalScore"]}/100\n" +
                $"RATING: {row["Rating"]}\n" +
                $"═══════════════════════════════\n\n" +
                $"--- INTERPRETATION ---\n" +
                $"• 80-100: Excellent supplier ⭐⭐⭐\n" +
                $"• 60-79:  Good supplier ⭐⭐\n" +
                $"• 40-59:  Average supplier ⭐\n" +
                $"• <40:    Poor performance";

            txtDetails.Text = details;
        }

        private void DisplayScoreGauge(int totalScore)
        {
            // Simple text-based gauge
            string gauge = "";
            int bars = totalScore / 5; // 20 bars = 100 points
            
            for (int i = 0; i < 20; i++)
            {
                if (i < bars)
                    gauge += "█";
                else
                    gauge += "░";
            }

            lblScoreGauge.Text = $"{totalScore}/100\n{gauge}";
            
            // Color based on score
            if (totalScore >= 80)
            {
                lblScoreGauge.ForeColor = System.Drawing.Color.Green;
                lblRating.Text = "⭐⭐⭐ EXCELLENT";
                lblRating.ForeColor = System.Drawing.Color.Green;
            }
            else if (totalScore >= 60)
            {
                lblScoreGauge.ForeColor = System.Drawing.Color.Orange;
                lblRating.Text = "⭐⭐ GOOD";
                lblRating.ForeColor = System.Drawing.Color.Orange;
            }
            else if (totalScore >= 40)
            {
                lblScoreGauge.ForeColor = System.Drawing.Color.DarkGoldenrod;
                lblRating.Text = "⭐ AVERAGE";
                lblRating.ForeColor = System.Drawing.Color.DarkGoldenrod;
            }
            else
            {
                lblScoreGauge.ForeColor = System.Drawing.Color.Red;
                lblRating.Text = "POOR";
                lblRating.ForeColor = System.Drawing.Color.Red;
            }
        }

        private void btnCompareAll_Click(object sender, EventArgs e)
        {
            LoadAllSuppliersComparison();
        }

        private void LoadAllSuppliersComparison()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    
                    string query = @"
                        SELECT 
                            s.SupplierName,
                            sp.*
                        FROM dbo.Suppliers s
                        CROSS APPLY dbo.fn_SupplierPerformanceScore(s.SupplierID, @Months) sp
                        ORDER BY sp.TotalScore DESC";
                    
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Months", (int)numMonths.Value);

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgvComparison.DataSource = dt;

                        // Hide ID
                        if (dgvComparison.Columns["SupplierID"] != null)
                            dgvComparison.Columns["SupplierID"].Visible = false;

                        // Format
                        if (dgvComparison.Columns["TotalValue"] != null)
                            dgvComparison.Columns["TotalValue"].DefaultCellStyle.Format = "N0";
                        if (dgvComparison.Columns["AvgReceiptValue"] != null)
                            dgvComparison.Columns["AvgReceiptValue"].DefaultCellStyle.Format = "N0";
                        if (dgvComparison.Columns["PriceVolatility"] != null)
                            dgvComparison.Columns["PriceVolatility"].DefaultCellStyle.Format = "N0";

                        // Color code by rating
                        foreach (DataGridViewRow row in dgvComparison.Rows)
                        {
                            if (row.Cells["Rating"].Value != null)
                            {
                                string rating = row.Cells["Rating"].Value.ToString();
                                if (rating == "Excellent")
                                    row.Cells["Rating"].Style.BackColor = System.Drawing.Color.LightGreen;
                                else if (rating == "Good")
                                    row.Cells["Rating"].Style.BackColor = System.Drawing.Color.LightYellow;
                                else if (rating == "Average")
                                    row.Cells["Rating"].Style.BackColor = System.Drawing.Color.LightGray;
                                else
                                    row.Cells["Rating"].Style.BackColor = System.Drawing.Color.LightCoral;
                            }
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                ErrorHandler.HandleSqlError(sqlEx, "phân tích nhà cung cấp");
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralError(ex, "phân tích nhà cung cấp");
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // New method to load supplier performance summary from view
        private void LoadSupplierPerformanceSummary()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM dbo.vw_SupplierPerformanceSummary ORDER BY TotalValue DESC";
                    
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            
                            // This could be used to populate a new tab or replace existing data
                            // For now, we'll add it as a method that can be called
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralError(ex, "tải tổng hợp hiệu suất nhà cung cấp");
            }
        }

        // New method to load monthly import trends
        private void LoadMonthlyImportTrends()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT TOP 12 * FROM dbo.vw_MonthlyImportTrends ORDER BY Year DESC, Month DESC";
                    
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            
                            // This could be used to show trends in a chart or grid
                            // For now, we'll add it as a method that can be called
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleGeneralError(ex, "tải xu hướng nhập hàng theo tháng");
            }
        }
    }
}
