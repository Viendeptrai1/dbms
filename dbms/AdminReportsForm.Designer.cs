namespace dbms
{
    partial class AdminReportsForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabInventory = new System.Windows.Forms.TabPage();
            this.tabImportLines = new System.Windows.Forms.TabPage();
            this.tabTopProducts = new System.Windows.Forms.TabPage();
            this.tabSupplierSummary = new System.Windows.Forms.TabPage();
            this.tabProductHistory = new System.Windows.Forms.TabPage();
            this.tabNeverImported = new System.Windows.Forms.TabPage();
            
            // Tab Inventory
            this.dgvInventoryValuation = new System.Windows.Forms.DataGridView();
            this.lblInventoryCount = new System.Windows.Forms.Label();
            
            // Tab Import Lines
            this.dgvImportLines = new System.Windows.Forms.DataGridView();
            this.lblImportLinesCount = new System.Windows.Forms.Label();
            
            // Tab Top Products
            this.dgvTopProducts = new System.Windows.Forms.DataGridView();
            this.lblTopProductsCount = new System.Windows.Forms.Label();
            
            // Tab Supplier Summary
            this.dgvSupplierSummary = new System.Windows.Forms.DataGridView();
            this.lblSupplierSummaryCount = new System.Windows.Forms.Label();
            
            // Tab Product History
            this.dgvProductImportHistory = new System.Windows.Forms.DataGridView();
            this.lblProductImportHistoryCount = new System.Windows.Forms.Label();
            
            // Tab Never Imported
            this.dgvProductsNeverImported = new System.Windows.Forms.DataGridView();
            this.lblProductsNeverImportedCount = new System.Windows.Forms.Label();
            
            // Controls
            this.btnRefreshAll = new System.Windows.Forms.Button();
            this.btnTestAllSQL = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            
            this.tabControl1.SuspendLayout();
            this.tabInventory.SuspendLayout();
            this.tabImportLines.SuspendLayout();
            this.tabTopProducts.SuspendLayout();
            this.tabSupplierSummary.SuspendLayout();
            this.tabProductHistory.SuspendLayout();
            this.tabNeverImported.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInventoryValuation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvImportLines)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTopProducts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSupplierSummary)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductImportHistory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductsNeverImported)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            
            // tabControl1
            this.tabControl1.Controls.Add(this.tabInventory);
            this.tabControl1.Controls.Add(this.tabImportLines);
            this.tabControl1.Controls.Add(this.tabTopProducts);
            this.tabControl1.Controls.Add(this.tabSupplierSummary);
            this.tabControl1.Controls.Add(this.tabProductHistory);
            this.tabControl1.Controls.Add(this.tabNeverImported);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1000, 600);
            this.tabControl1.TabIndex = 0;
            
            // tabInventory
            this.tabInventory.Controls.Add(this.dgvInventoryValuation);
            this.tabInventory.Controls.Add(this.lblInventoryCount);
            this.tabInventory.Location = new System.Drawing.Point(4, 25);
            this.tabInventory.Name = "tabInventory";
            this.tabInventory.Padding = new System.Windows.Forms.Padding(3);
            this.tabInventory.Size = new System.Drawing.Size(992, 571);
            this.tabInventory.TabIndex = 0;
            this.tabInventory.Text = "Định giá Tồn kho";
            this.tabInventory.UseVisualStyleBackColor = true;
            
            // dgvInventoryValuation
            this.dgvInventoryValuation.AllowUserToAddRows = false;
            this.dgvInventoryValuation.AllowUserToDeleteRows = false;
            this.dgvInventoryValuation.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvInventoryValuation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvInventoryValuation.Location = new System.Drawing.Point(3, 3);
            this.dgvInventoryValuation.Name = "dgvInventoryValuation";
            this.dgvInventoryValuation.ReadOnly = true;
            this.dgvInventoryValuation.Size = new System.Drawing.Size(986, 565);
            this.dgvInventoryValuation.TabIndex = 0;
            
            // lblInventoryCount
            this.lblInventoryCount.AutoSize = true;
            this.lblInventoryCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblInventoryCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.lblInventoryCount.Location = new System.Drawing.Point(6, 540);
            this.lblInventoryCount.Name = "lblInventoryCount";
            this.lblInventoryCount.Size = new System.Drawing.Size(100, 20);
            this.lblInventoryCount.TabIndex = 1;
            this.lblInventoryCount.Text = "Tồn kho: 0";
            
            // tabImportLines
            this.tabImportLines.Controls.Add(this.dgvImportLines);
            this.tabImportLines.Controls.Add(this.lblImportLinesCount);
            this.tabImportLines.Location = new System.Drawing.Point(4, 25);
            this.tabImportLines.Name = "tabImportLines";
            this.tabImportLines.Padding = new System.Windows.Forms.Padding(3);
            this.tabImportLines.Size = new System.Drawing.Size(992, 571);
            this.tabImportLines.TabIndex = 1;
            this.tabImportLines.Text = "Dòng Nhập hàng";
            this.tabImportLines.UseVisualStyleBackColor = true;
            
            // dgvImportLines
            this.dgvImportLines.AllowUserToAddRows = false;
            this.dgvImportLines.AllowUserToDeleteRows = false;
            this.dgvImportLines.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvImportLines.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvImportLines.Location = new System.Drawing.Point(3, 3);
            this.dgvImportLines.Name = "dgvImportLines";
            this.dgvImportLines.ReadOnly = true;
            this.dgvImportLines.Size = new System.Drawing.Size(986, 565);
            this.dgvImportLines.TabIndex = 0;
            
            // lblImportLinesCount
            this.lblImportLinesCount.AutoSize = true;
            this.lblImportLinesCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblImportLinesCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.lblImportLinesCount.Location = new System.Drawing.Point(6, 540);
            this.lblImportLinesCount.Name = "lblImportLinesCount";
            this.lblImportLinesCount.Size = new System.Drawing.Size(100, 20);
            this.lblImportLinesCount.TabIndex = 1;
            this.lblImportLinesCount.Text = "Dòng nhập: 0";
            
            // tabTopProducts
            this.tabTopProducts.Controls.Add(this.dgvTopProducts);
            this.tabTopProducts.Controls.Add(this.lblTopProductsCount);
            this.tabTopProducts.Location = new System.Drawing.Point(4, 25);
            this.tabTopProducts.Name = "tabTopProducts";
            this.tabTopProducts.Padding = new System.Windows.Forms.Padding(3);
            this.tabTopProducts.Size = new System.Drawing.Size(992, 571);
            this.tabTopProducts.TabIndex = 2;
            this.tabTopProducts.Text = "Top Sản phẩm";
            this.tabTopProducts.UseVisualStyleBackColor = true;
            
            // dgvTopProducts
            this.dgvTopProducts.AllowUserToAddRows = false;
            this.dgvTopProducts.AllowUserToDeleteRows = false;
            this.dgvTopProducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTopProducts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTopProducts.Location = new System.Drawing.Point(3, 3);
            this.dgvTopProducts.Name = "dgvTopProducts";
            this.dgvTopProducts.ReadOnly = true;
            this.dgvTopProducts.Size = new System.Drawing.Size(986, 565);
            this.dgvTopProducts.TabIndex = 0;
            
            // lblTopProductsCount
            this.lblTopProductsCount.AutoSize = true;
            this.lblTopProductsCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTopProductsCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.lblTopProductsCount.Location = new System.Drawing.Point(6, 540);
            this.lblTopProductsCount.Name = "lblTopProductsCount";
            this.lblTopProductsCount.Size = new System.Drawing.Size(100, 20);
            this.lblTopProductsCount.TabIndex = 1;
            this.lblTopProductsCount.Text = "Top sản phẩm: 0";
            
            // tabSupplierSummary
            this.tabSupplierSummary.Controls.Add(this.dgvSupplierSummary);
            this.tabSupplierSummary.Controls.Add(this.lblSupplierSummaryCount);
            this.tabSupplierSummary.Location = new System.Drawing.Point(4, 25);
            this.tabSupplierSummary.Name = "tabSupplierSummary";
            this.tabSupplierSummary.Padding = new System.Windows.Forms.Padding(3);
            this.tabSupplierSummary.Size = new System.Drawing.Size(992, 571);
            this.tabSupplierSummary.TabIndex = 3;
            this.tabSupplierSummary.Text = "Tổng hợp NCC";
            this.tabSupplierSummary.UseVisualStyleBackColor = true;
            
            // dgvSupplierSummary
            this.dgvSupplierSummary.AllowUserToAddRows = false;
            this.dgvSupplierSummary.AllowUserToDeleteRows = false;
            this.dgvSupplierSummary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSupplierSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSupplierSummary.Location = new System.Drawing.Point(3, 3);
            this.dgvSupplierSummary.Name = "dgvSupplierSummary";
            this.dgvSupplierSummary.ReadOnly = true;
            this.dgvSupplierSummary.Size = new System.Drawing.Size(986, 565);
            this.dgvSupplierSummary.TabIndex = 0;
            
            // lblSupplierSummaryCount
            this.lblSupplierSummaryCount.AutoSize = true;
            this.lblSupplierSummaryCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblSupplierSummaryCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.lblSupplierSummaryCount.Location = new System.Drawing.Point(6, 540);
            this.lblSupplierSummaryCount.Name = "lblSupplierSummaryCount";
            this.lblSupplierSummaryCount.Size = new System.Drawing.Size(100, 20);
            this.lblSupplierSummaryCount.TabIndex = 1;
            this.lblSupplierSummaryCount.Text = "Nhà cung cấp: 0";
            
            // tabProductHistory
            this.tabProductHistory.Controls.Add(this.dgvProductImportHistory);
            this.tabProductHistory.Controls.Add(this.lblProductImportHistoryCount);
            this.tabProductHistory.Location = new System.Drawing.Point(4, 25);
            this.tabProductHistory.Name = "tabProductHistory";
            this.tabProductHistory.Padding = new System.Windows.Forms.Padding(3);
            this.tabProductHistory.Size = new System.Drawing.Size(992, 571);
            this.tabProductHistory.TabIndex = 4;
            this.tabProductHistory.Text = "Lịch sử Nhập";
            this.tabProductHistory.UseVisualStyleBackColor = true;
            
            // dgvProductImportHistory
            this.dgvProductImportHistory.AllowUserToAddRows = false;
            this.dgvProductImportHistory.AllowUserToDeleteRows = false;
            this.dgvProductImportHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProductImportHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProductImportHistory.Location = new System.Drawing.Point(3, 3);
            this.dgvProductImportHistory.Name = "dgvProductImportHistory";
            this.dgvProductImportHistory.ReadOnly = true;
            this.dgvProductImportHistory.Size = new System.Drawing.Size(986, 565);
            this.dgvProductImportHistory.TabIndex = 0;
            
            // lblProductImportHistoryCount
            this.lblProductImportHistoryCount.AutoSize = true;
            this.lblProductImportHistoryCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblProductImportHistoryCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.lblProductImportHistoryCount.Location = new System.Drawing.Point(6, 540);
            this.lblProductImportHistoryCount.Name = "lblProductImportHistoryCount";
            this.lblProductImportHistoryCount.Size = new System.Drawing.Size(100, 20);
            this.lblProductImportHistoryCount.TabIndex = 1;
            this.lblProductImportHistoryCount.Text = "Lịch sử SP0001: 0";
            
            // tabNeverImported
            this.tabNeverImported.Controls.Add(this.dgvProductsNeverImported);
            this.tabNeverImported.Controls.Add(this.lblProductsNeverImportedCount);
            this.tabNeverImported.Location = new System.Drawing.Point(4, 25);
            this.tabNeverImported.Name = "tabNeverImported";
            this.tabNeverImported.Padding = new System.Windows.Forms.Padding(3);
            this.tabNeverImported.Size = new System.Drawing.Size(992, 571);
            this.tabNeverImported.TabIndex = 5;
            this.tabNeverImported.Text = "SP Chưa Nhập";
            this.tabNeverImported.UseVisualStyleBackColor = true;
            
            // dgvProductsNeverImported
            this.dgvProductsNeverImported.AllowUserToAddRows = false;
            this.dgvProductsNeverImported.AllowUserToDeleteRows = false;
            this.dgvProductsNeverImported.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProductsNeverImported.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProductsNeverImported.Location = new System.Drawing.Point(3, 3);
            this.dgvProductsNeverImported.Name = "dgvProductsNeverImported";
            this.dgvProductsNeverImported.ReadOnly = true;
            this.dgvProductsNeverImported.Size = new System.Drawing.Size(986, 565);
            this.dgvProductsNeverImported.TabIndex = 0;
            
            // lblProductsNeverImportedCount
            this.lblProductsNeverImportedCount.AutoSize = true;
            this.lblProductsNeverImportedCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblProductsNeverImportedCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.lblProductsNeverImportedCount.Location = new System.Drawing.Point(6, 540);
            this.lblProductsNeverImportedCount.Name = "lblProductsNeverImportedCount";
            this.lblProductsNeverImportedCount.Size = new System.Drawing.Size(100, 20);
            this.lblProductsNeverImportedCount.TabIndex = 1;
            this.lblProductsNeverImportedCount.Text = "Chưa nhập: 0";
            
            // btnRefreshAll
            this.btnRefreshAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnRefreshAll.FlatAppearance.BorderSize = 0;
            this.btnRefreshAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefreshAll.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.btnRefreshAll.ForeColor = System.Drawing.Color.White;
            this.btnRefreshAll.Location = new System.Drawing.Point(12, 610);
            this.btnRefreshAll.Name = "btnRefreshAll";
            this.btnRefreshAll.Size = new System.Drawing.Size(120, 35);
            this.btnRefreshAll.TabIndex = 1;
            this.btnRefreshAll.Text = "Làm mới tất cả";
            this.btnRefreshAll.UseVisualStyleBackColor = false;
            this.btnRefreshAll.Click += new System.EventHandler(this.btnRefreshAll_Click);
            
            // btnTestAllSQL
            this.btnTestAllSQL.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(102)))), ((int)(((byte)(204)))));
            this.btnTestAllSQL.FlatAppearance.BorderSize = 0;
            this.btnTestAllSQL.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTestAllSQL.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.btnTestAllSQL.ForeColor = System.Drawing.Color.White;
            this.btnTestAllSQL.Location = new System.Drawing.Point(150, 610);
            this.btnTestAllSQL.Name = "btnTestAllSQL";
            this.btnTestAllSQL.Size = new System.Drawing.Size(150, 35);
            this.btnTestAllSQL.TabIndex = 2;
            this.btnTestAllSQL.Text = "Kiểm tra SQL Objects";
            this.btnTestAllSQL.UseVisualStyleBackColor = false;
            this.btnTestAllSQL.Click += new System.EventHandler(this.btnTestAllSQL_Click);
            
            // btnClose
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(320, 610);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 35);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Đóng";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            
            // statusStrip1
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 655);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1000, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            
            // toolStripStatusLabel1
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(983, 17);
            this.toolStripStatusLabel1.Spring = true;
            this.toolStripStatusLabel1.Text = "Sẵn sàng";
            this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            
            // AdminReportsForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1000, 677);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnTestAllSQL);
            this.Controls.Add(this.btnRefreshAll);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AdminReportsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Báo cáo Admin - Tất cả SQL Objects";
            
            this.tabControl1.ResumeLayout(false);
            this.tabInventory.ResumeLayout(false);
            this.tabInventory.PerformLayout();
            this.tabImportLines.ResumeLayout(false);
            this.tabImportLines.PerformLayout();
            this.tabTopProducts.ResumeLayout(false);
            this.tabTopProducts.PerformLayout();
            this.tabSupplierSummary.ResumeLayout(false);
            this.tabSupplierSummary.PerformLayout();
            this.tabProductHistory.ResumeLayout(false);
            this.tabProductHistory.PerformLayout();
            this.tabNeverImported.ResumeLayout(false);
            this.tabNeverImported.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInventoryValuation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvImportLines)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTopProducts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSupplierSummary)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductImportHistory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductsNeverImported)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabInventory;
        private System.Windows.Forms.TabPage tabImportLines;
        private System.Windows.Forms.TabPage tabTopProducts;
        private System.Windows.Forms.TabPage tabSupplierSummary;
        private System.Windows.Forms.TabPage tabProductHistory;
        private System.Windows.Forms.TabPage tabNeverImported;
        
        // DataGridViews
        private System.Windows.Forms.DataGridView dgvInventoryValuation;
        private System.Windows.Forms.DataGridView dgvImportLines;
        private System.Windows.Forms.DataGridView dgvTopProducts;
        private System.Windows.Forms.DataGridView dgvSupplierSummary;
        private System.Windows.Forms.DataGridView dgvProductImportHistory;
        private System.Windows.Forms.DataGridView dgvProductsNeverImported;
        
        // Labels
        private System.Windows.Forms.Label lblInventoryCount;
        private System.Windows.Forms.Label lblImportLinesCount;
        private System.Windows.Forms.Label lblTopProductsCount;
        private System.Windows.Forms.Label lblSupplierSummaryCount;
        private System.Windows.Forms.Label lblProductImportHistoryCount;
        private System.Windows.Forms.Label lblProductsNeverImportedCount;
        
        // Buttons
        private System.Windows.Forms.Button btnRefreshAll;
        private System.Windows.Forms.Button btnTestAllSQL;
        private System.Windows.Forms.Button btnClose;
        
        // Status
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    }
}
