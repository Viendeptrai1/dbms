using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace dbms
{
    public partial class SmartPricingForm : Form
    {
        private int currentUserID;
        private string connectionString;

        public SmartPricingForm(int userID)
        {
            InitializeComponent();
            currentUserID = userID;
            connectionString = Properties.Settings.Default.QLNhapHangConnectionString;
        }

        private void SmartPricingForm_Load(object sender, EventArgs e)
        {
            LoadProducts();
            LoadCategories();
            
            // Set default strategy
            cmbStrategy.SelectedIndex = 0;
            
            // Set default tab
            tabControl1.SelectedIndex = 0;
        }

        #region Load Data
        private void LoadProducts()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                        SELECT 
                            ProductID, 
                            SKU, 
                            ProductName, 
                            SellingPrice, 
                            StockQuantity
                        FROM dbo.Products
                        ORDER BY ProductName";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvProducts.DataSource = dt;
                    dgvProducts.Columns["ProductID"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load sản phẩm: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadCategories()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT CategoryName FROM dbo.Categories ORDER BY CategoryName";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    cmbCategory.Items.Clear();
                    cmbCategory.Items.Add("-- Tất cả --");
                    foreach (DataRow row in dt.Rows)
                    {
                        cmbCategory.Items.Add(row["CategoryName"].ToString());
                    }
                    cmbCategory.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load categories: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Tab 1: Update Single Product
        private void btnUpdateSinglePrice_Click(object sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int productID = Convert.ToInt32(dgvProducts.SelectedRows[0].Cells["ProductID"].Value);
            string productName = dgvProducts.SelectedRows[0].Cells["ProductName"].Value.ToString();
            decimal oldPrice = Convert.ToDecimal(dgvProducts.SelectedRows[0].Cells["SellingPrice"].Value);

            if (!decimal.TryParse(txtNewPrice.Text, out decimal newPrice))
            {
                MessageBox.Show("Giá mới không hợp lệ!", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (newPrice <= 0)
            {
                MessageBox.Show("Giá mới phải > 0!", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string reason = txtReason.Text.Trim();

            DialogResult result = MessageBox.Show(
                $"Cập nhật giá cho \"{productName}\"?\n" +
                $"Giá cũ: {oldPrice:N0} đ\n" +
                $"Giá mới: {newPrice:N0} đ\n" +
                $"Lý do: {(string.IsNullOrEmpty(reason) ? "(Không có)" : reason)}",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                UpdateSinglePrice(productID, newPrice, reason);
            }
        }

        private void UpdateSinglePrice(int productID, decimal newPrice, string reason)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("dbo.sp_UpdateSingleProductPrice", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@ProductID", productID);
                        cmd.Parameters.AddWithValue("@NewPrice", newPrice);
                        cmd.Parameters.AddWithValue("@Reason", (object)reason ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@ChangedBy", currentUserID);

                        SqlParameter outputParam = new SqlParameter("@Message", SqlDbType.NVarChar, 200);
                        outputParam.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(outputParam);

                        cmd.ExecuteNonQuery();

                        string message = outputParam.Value.ToString();
                        MessageBox.Show(message, "Thành công", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LoadProducts();
                        txtNewPrice.Clear();
                        txtReason.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvProducts_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count > 0)
            {
                decimal currentPrice = Convert.ToDecimal(
                    dgvProducts.SelectedRows[0].Cells["SellingPrice"].Value);
                lblCurrentPrice.Text = $"Giá hiện tại: {currentPrice:N0} đ";
            }
        }
        #endregion

        #region Tab 2: Dynamic Strategy
        private void btnApplyStrategy_Click(object sender, EventArgs e)
        {
            string strategy = cmbStrategy.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(strategy))
            {
                MessageBox.Show("Vui lòng chọn strategy!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string category = cmbCategory.SelectedIndex > 0 ? cmbCategory.SelectedItem.ToString() : null;
            decimal? adjustPercent = null;

            if (strategy == "Manual-Percent")
            {
                if (!decimal.TryParse(txtAdjustPercent.Text, out decimal percent))
                {
                    MessageBox.Show("Phần trăm điều chỉnh không hợp lệ!", "Lỗi", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                adjustPercent = percent;
            }

            DialogResult result = MessageBox.Show(
                $"Áp dụng strategy: {strategy}\n" +
                $"Category: {(category ?? "Tất cả")}\n" +
                $"Adjust %: {(adjustPercent.HasValue ? adjustPercent.Value.ToString() : "N/A")}\n\n" +
                $"Điều chỉnh giá hàng loạt?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                ApplyDynamicStrategy(strategy, category, adjustPercent);
            }
        }

        private void ApplyDynamicStrategy(string strategy, string category, decimal? adjustPercent)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("dbo.sp_DynamicPriceStrategy", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 60; // Strategy có thể mất thời gian

                        cmd.Parameters.AddWithValue("@Strategy", strategy);
                        cmd.Parameters.AddWithValue("@CategoryName", (object)category ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@AdjustmentPercent", (object)adjustPercent ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@ChangedBy", currentUserID);

                        SqlParameter affectedParam = new SqlParameter("@AffectedCount", SqlDbType.Int);
                        affectedParam.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(affectedParam);

                        SqlParameter messageParam = new SqlParameter("@Message", SqlDbType.NVarChar, 500);
                        messageParam.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(messageParam);

                        cmd.ExecuteNonQuery();

                        int affected = Convert.ToInt32(affectedParam.Value);
                        string message = messageParam.Value.ToString();

                        MessageBox.Show(message, "Kết quả", 
                            MessageBoxButtons.OK, 
                            affected > 0 ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

                        LoadProducts();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbStrategy_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool showPercent = cmbStrategy.SelectedItem?.ToString() == "Manual-Percent";
            lblAdjustPercent.Visible = showPercent;
            txtAdjustPercent.Visible = showPercent;
            lblPercentHint.Visible = showPercent;
        }
        #endregion

        #region Tab 3: Price History
        private void btnLoadPriceHistory_Click(object sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int productID = Convert.ToInt32(dgvProducts.SelectedRows[0].Cells["ProductID"].Value);
            LoadPriceHistory(productID);
        }

        private void LoadPriceHistory(int productID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                        SELECT 
                            ChangeDate,
                            OldPrice,
                            NewPrice,
                            (NewPrice - OldPrice) AS PriceChange,
                            ((NewPrice - OldPrice) * 100.0 / OldPrice) AS PercentChange,
                            Reason
                        FROM dbo.ProductPriceHistory
                        WHERE ProductID = @ProductID
                        ORDER BY ChangeDate DESC";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    adapter.SelectCommand.Parameters.AddWithValue("@ProductID", productID);
                    
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvPriceHistory.DataSource = dt;

                    if (dgvPriceHistory.Columns["ChangeDate"] != null)
                        dgvPriceHistory.Columns["ChangeDate"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                    if (dgvPriceHistory.Columns["OldPrice"] != null)
                        dgvPriceHistory.Columns["OldPrice"].DefaultCellStyle.Format = "N0";
                    if (dgvPriceHistory.Columns["NewPrice"] != null)
                        dgvPriceHistory.Columns["NewPrice"].DefaultCellStyle.Format = "N0";
                    if (dgvPriceHistory.Columns["PriceChange"] != null)
                        dgvPriceHistory.Columns["PriceChange"].DefaultCellStyle.Format = "N0";
                    if (dgvPriceHistory.Columns["PercentChange"] != null)
                        dgvPriceHistory.Columns["PercentChange"].DefaultCellStyle.Format = "N2";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load lịch sử: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
