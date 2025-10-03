using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Configuration;
using System.Linq;

namespace dbms
{
    public partial class MonthlyReportForm : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["QLNhapHangConnectionString"].ConnectionString;

        public MonthlyReportForm()
        {
            InitializeComponent();
            InitializeForm();
        }

        private void InitializeForm()
        {
            // Set default values
            dtpMonth.Value = DateTime.Now;
            cmbReportType.SelectedIndex = 0;
        }

        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            try
            {
                int month = dtpMonth.Value.Month;
                int year = dtpMonth.Value.Year;
                string reportType = cmbReportType.SelectedItem.ToString();

                GenerateMonthlyReport(month, year, reportType);
            }
            catch (Exception ex)
            {
                ErrorHandler.ShowError("Lỗi tạo báo cáo", ex.Message);
            }
        }

        private void GenerateMonthlyReport(int month, int year, string reportType)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("dbo.sp_GenerateMonthlyReport", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 60;

                        cmd.Parameters.AddWithValue("@Month", month);
                        cmd.Parameters.AddWithValue("@Year", year);
                        cmd.Parameters.AddWithValue("@ReportType", reportType);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataSet dataSet = new DataSet();
                            adapter.Fill(dataSet);

                            DisplayReportResults(dataSet, reportType);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.ShowError("Lỗi tạo báo cáo tháng", ex.Message);
            }
        }

        private void DisplayReportResults(DataSet dataSet, string reportType)
        {
            tabControl1.TabPages.Clear();

            if (dataSet.Tables.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu cho tháng này!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Summary Report
            if (reportType == "Summary" || reportType == "All")
            {
                if (dataSet.Tables.Count > 0)
                {
                    CreateReportTab("Tổng quan", dataSet.Tables[0]);
                }
            }

            // Detailed Report
            if (reportType == "Detailed" || reportType == "All")
            {
                int tableIndex = (reportType == "All") ? 1 : 0;
                if (dataSet.Tables.Count > tableIndex)
                {
                    CreateReportTab("Chi tiết theo sản phẩm", dataSet.Tables[tableIndex]);
                }
            }

            // ABC Analysis
            if (reportType == "ABC" || reportType == "All")
            {
                int tableIndex = (reportType == "All") ? 2 : 0;
                if (dataSet.Tables.Count > tableIndex)
                {
                    CreateReportTab("Phân tích ABC", dataSet.Tables[tableIndex]);
                }
            }

            // Supplier Summary
            if (reportType == "Supplier" || reportType == "All")
            {
                int tableIndex = (reportType == "All") ? 3 : 0;
                if (dataSet.Tables.Count > tableIndex)
                {
                    CreateReportTab("Tổng hợp nhà cung cấp", dataSet.Tables[tableIndex]);
                }
            }
        }

        private void CreateReportTab(string tabName, DataTable dataTable)
        {
            TabPage tabPage = new TabPage(tabName);
            
            DataGridView dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                DataSource = dataTable,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            // Format currency columns
            foreach (DataGridViewColumn column in dgv.Columns)
            {
                if (column.Name.Contains("Amount") || column.Name.Contains("Value") || column.Name.Contains("Price"))
                {
                    column.DefaultCellStyle.Format = "N0";
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            }

            tabPage.Controls.Add(dgv);
            tabControl1.TabPages.Add(tabPage);
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (tabControl1.TabPages.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SaveFileDialog saveDialog = new SaveFileDialog
                {
                    Filter = "Excel Files|*.xlsx",
                    Title = "Xuất báo cáo",
                    FileName = $"BaoCaoThang_{dtpMonth.Value:yyyy_MM}.xlsx"
                };

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    ExportToExcel(saveDialog.FileName);
                    MessageBox.Show("Xuất báo cáo thành công!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.ShowError("Lỗi xuất báo cáo", ex.Message);
            }
        }

        private void ExportToExcel(string fileName)
        {
            // Simple CSV export since Excel libraries might not be available
            using (System.IO.StreamWriter writer = new System.IO.StreamWriter(fileName.Replace(".xlsx", ".csv")))
            {
                foreach (TabPage tab in tabControl1.TabPages)
                {
                    writer.WriteLine($"=== {tab.Text} ===");
                    
                    DataGridView dgv = tab.Controls[0] as DataGridView;
                    if (dgv != null && dgv.DataSource is DataTable dt)
                    {
                        // Write headers
                        var columnNames = new string[dt.Columns.Count];
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            columnNames[i] = dt.Columns[i].ColumnName;
                        }
                        writer.WriteLine(string.Join(",", columnNames));
                        
                        // Write data
                        foreach (DataRow row in dt.Rows)
                        {
                            writer.WriteLine(string.Join(",", row.ItemArray));
                        }
                    }
                    writer.WriteLine();
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
