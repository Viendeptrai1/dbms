using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace dbms
{
    public partial class AdvancedAnalyticsForm : Form
    {
        private string connectionString;

        public AdvancedAnalyticsForm()
        {
            InitializeComponent();
            connectionString = Properties.Settings.Default.QLNhapHangConnectionString;
        }

        private void AdvancedAnalyticsForm_Load(object sender, EventArgs e)
        {
            dtpFrom.Value = DateTime.Now.AddMonths(-3);
            dtpTo.Value = DateTime.Now;
            
            tabControl1.SelectedIndex = 0;
        }

        #region Tab 1: ABC Analysis
        private void btnLoadABC_Click(object sender, EventArgs e)
        {
            LoadABCAnalysis();
        }

        private void LoadABCAnalysis()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM dbo.fn_ProductABCClassification(@FromDate, @ToDate) ORDER BY CumulativePercent";
                    
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@FromDate", dtpFrom.Value.Date);
                        cmd.Parameters.AddWithValue("@ToDate", dtpTo.Value.Date);

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgvABC.DataSource = dt;

                        // Format columns
                        if (dgvABC.Columns["ProductID"] != null)
                            dgvABC.Columns["ProductID"].Visible = false;
                        
                        if (dgvABC.Columns["TotalImportValue"] != null)
                            dgvABC.Columns["TotalImportValue"].DefaultCellStyle.Format = "N0";
                        
                        if (dgvABC.Columns["PercentOfTotal"] != null)
                            dgvABC.Columns["PercentOfTotal"].DefaultCellStyle.Format = "N2";
                        
                        if (dgvABC.Columns["CumulativePercent"] != null)
                            dgvABC.Columns["CumulativePercent"].DefaultCellStyle.Format = "N2";

                        // Color code by ABC Class
                        foreach (DataGridViewRow row in dgvABC.Rows)
                        {
                            if (row.Cells["ABCClass"].Value != null)
                            {
                                string abcClass = row.Cells["ABCClass"].Value.ToString();
                                if (abcClass == "A")
                                    row.Cells["ABCClass"].Style.BackColor = System.Drawing.Color.LightGreen;
                                else if (abcClass == "B")
                                    row.Cells["ABCClass"].Style.BackColor = System.Drawing.Color.LightYellow;
                                else if (abcClass == "C")
                                    row.Cells["ABCClass"].Style.BackColor = System.Drawing.Color.LightCoral;
                            }
                        }

                        // Count statistics
                        int countA = dt.Select("ABCClass = 'A'").Length;
                        int countB = dt.Select("ABCClass = 'B'").Length;
                        int countC = dt.Select("ABCClass = 'C'").Length;

                        lblABCStats.Text = $"Class A: {countA} sản phẩm | Class B: {countB} sản phẩm | Class C: {countC} sản phẩm";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }
        #endregion

        #region Tab 2: Inventory Turnover
        private void btnLoadTurnover_Click(object sender, EventArgs e)
        {
            LoadInventoryTurnover();
        }

        private void LoadInventoryTurnover()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT 
                            p.ProductID,
                            p.SKU,
                            p.ProductName,
                            p.StockQuantity,
                            t.*
                        FROM dbo.Products p
                        CROSS APPLY dbo.fn_InventoryTurnoverRate(p.ProductID, @Months) t
                        ORDER BY t.TurnoverRate DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Months", (int)numMonths.Value);

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgvTurnover.DataSource = dt;

                        // Hide duplicate columns
                        if (dgvTurnover.Columns["ProductID"] != null)
                            dgvTurnover.Columns["ProductID"].Visible = false;
                        if (dgvTurnover.Columns["ProductID1"] != null)
                            dgvTurnover.Columns["ProductID1"].Visible = false;
                        if (dgvTurnover.Columns["AnalysisPeriodMonths"] != null)
                            dgvTurnover.Columns["AnalysisPeriodMonths"].Visible = false;

                        // Format
                        if (dgvTurnover.Columns["TurnoverRate"] != null)
                            dgvTurnover.Columns["TurnoverRate"].DefaultCellStyle.Format = "N2";
                        if (dgvTurnover.Columns["DaysOfStock"] != null)
                            dgvTurnover.Columns["DaysOfStock"].DefaultCellStyle.Format = "N0";
                        if (dgvTurnover.Columns["AvgMonthlyImport"] != null)
                            dgvTurnover.Columns["AvgMonthlyImport"].DefaultCellStyle.Format = "N2";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }
        #endregion

        #region Tab 3: Price Trends
        private void btnLoadPriceTrend_Click(object sender, EventArgs e)
        {
            if (dgvABC.SelectedRows.Count == 0 && dgvTurnover.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm từ tab ABC hoặc Turnover!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int productID = 0;
            if (tabControl1.SelectedIndex == 0 && dgvABC.SelectedRows.Count > 0)
                productID = Convert.ToInt32(dgvABC.SelectedRows[0].Cells["ProductID"].Value);
            else if (tabControl1.SelectedIndex == 1 && dgvTurnover.SelectedRows.Count > 0)
                productID = Convert.ToInt32(dgvTurnover.SelectedRows[0].Cells["ProductID"].Value);

            if (productID > 0)
                LoadPriceTrend(productID);
        }

        private void LoadPriceTrend(int productID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM dbo.fn_ProductPriceTrend(@ProductID, @Months)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProductID", productID);
                        cmd.Parameters.AddWithValue("@Months", 6);

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            DataRow row = dt.Rows[0];
                            string productName = "Product ID: " + productID;
                            
                            string info = $"=== PRICE TREND ANALYSIS ===\n\n" +
                                $"Product: {productName}\n" +
                                $"Period: 6 months\n\n" +
                                $"First Price: {row["FirstPrice"]:N0}\n" +
                                $"Last Price: {row["LastPrice"]:N0}\n" +
                                $"Current Price: {row["CurrentPrice"]:N0}\n" +
                                $"Min Price: {row["MinPrice"]:N0}\n" +
                                $"Max Price: {row["MaxPrice"]:N0}\n\n" +
                                $"Total Changes: {row["TotalChanges"]}\n" +
                                $"Avg Change: {row["AvgPriceChange"]:N0}\n" +
                                $"Avg % Change: {row["AvgPercentChange"]:N2}%\n" +
                                $"Price Volatility: {row["PriceVolatility"]:N0}\n\n" +
                                $"Increases: {row["IncreaseCount"]}\n" +
                                $"Decreases: {row["DecreaseCount"]}\n" +
                                $"Trend: {row["TrendDirection"]}\n" +
                                $"Overall Change: {row["OverallChangePercent"]:N2}%";

                            txtPriceTrend.Text = info;
                        }
                        else
                        {
                            txtPriceTrend.Text = "No price history available for this product.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "CSV File|*.csv";
                sfd.FileName = $"ABCAnalysis_{DateTime.Now:yyyyMMdd}.csv";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    DataGridView dgv = tabControl1.SelectedIndex == 0 ? dgvABC : dgvTurnover;
                    
                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(sfd.FileName, false, System.Text.Encoding.UTF8))
                    {
                        // Header
                        for (int i = 0; i < dgv.Columns.Count; i++)
                        {
                            if (dgv.Columns[i].Visible)
                            {
                                sw.Write(dgv.Columns[i].HeaderText);
                                if (i < dgv.Columns.Count - 1) sw.Write(",");
                            }
                        }
                        sw.WriteLine();

                        // Rows
                        foreach (DataGridViewRow row in dgv.Rows)
                        {
                            if (!row.IsNewRow)
                            {
                                for (int i = 0; i < dgv.Columns.Count; i++)
                                {
                                    if (dgv.Columns[i].Visible)
                                    {
                                        sw.Write(row.Cells[i].Value?.ToString() ?? "");
                                        if (i < dgv.Columns.Count - 1) sw.Write(",");
                                    }
                                }
                                sw.WriteLine();
                            }
                        }
                    }

                    MessageBox.Show("Export thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi export: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
