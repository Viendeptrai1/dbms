namespace dbms
{
    partial class SellerMainForm
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.hệThốngToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.đăngXuấtToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.thoátToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.advancedFeaturesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.smartPricingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reorderSuggestionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.advancedAnalyticsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.supplierAnalysisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabProducts = new System.Windows.Forms.TabPage();
            this.tabGoodsReceipt = new System.Windows.Forms.TabPage();
            this.tabReports = new System.Windows.Forms.TabPage();
            
            // Tab Products
            this.dgvProducts = new System.Windows.Forms.DataGridView();
            this.dgvCategories = new System.Windows.Forms.DataGridView();
            this.dgvSuppliers = new System.Windows.Forms.DataGridView();
            this.btnAddProduct = new System.Windows.Forms.Button();
            this.btnUpdatePrice = new System.Windows.Forms.Button();
            this.btnRefreshProducts = new System.Windows.Forms.Button();
            
            // Tab Goods Receipt
            this.dgvGoodsReceipts = new System.Windows.Forms.DataGridView();
            this.dgvGoodsReceiptDetails = new System.Windows.Forms.DataGridView();
            this.btnCreateReceipt = new System.Windows.Forms.Button();
            this.btnDeleteReceipt = new System.Windows.Forms.Button();
            this.btnRefreshReceipts = new System.Windows.Forms.Button();
            
            // Tab Reports
            this.dgvInventoryValuation = new System.Windows.Forms.DataGridView();
            this.dgvImportLines = new System.Windows.Forms.DataGridView();
            this.dgvTopProducts = new System.Windows.Forms.DataGridView();
            this.btnLoadInventory = new System.Windows.Forms.Button();
            this.btnLoadImportLines = new System.Windows.Forms.Button();
            this.btnLoadTopProducts = new System.Windows.Forms.Button();
            this.btnLoadSupplierSummary = new System.Windows.Forms.Button();
            
            // Status and Info
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblConnectionStatus = new System.Windows.Forms.Label();
            this.lblUserInfo = new System.Windows.Forms.Label();
            
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabProducts.SuspendLayout();
            this.tabGoodsReceipt.SuspendLayout();
            this.tabReports.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCategories)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSuppliers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGoodsReceipts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGoodsReceiptDetails)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInventoryValuation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvImportLines)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTopProducts)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            
            // menuStrip1
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hệThốngToolStripMenuItem,
            this.advancedFeaturesToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1200, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            
            // advancedFeaturesToolStripMenuItem
            this.advancedFeaturesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.smartPricingToolStripMenuItem,
            this.reorderSuggestionsToolStripMenuItem,
            this.advancedAnalyticsToolStripMenuItem,
            this.supplierAnalysisToolStripMenuItem});
            this.advancedFeaturesToolStripMenuItem.Name = "advancedFeaturesToolStripMenuItem";
            this.advancedFeaturesToolStripMenuItem.Size = new System.Drawing.Size(130, 20);
            this.advancedFeaturesToolStripMenuItem.Text = "⚡ Advanced Features";
            
            // smartPricingToolStripMenuItem
            this.smartPricingToolStripMenuItem.Name = "smartPricingToolStripMenuItem";
            this.smartPricingToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.smartPricingToolStripMenuItem.Text = "💰 Smart Pricing";
            this.smartPricingToolStripMenuItem.Click += new System.EventHandler(this.smartPricingToolStripMenuItem_Click);
            
            // reorderSuggestionsToolStripMenuItem
            this.reorderSuggestionsToolStripMenuItem.Name = "reorderSuggestionsToolStripMenuItem";
            this.reorderSuggestionsToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.reorderSuggestionsToolStripMenuItem.Text = "📊 Reorder Suggestions";
            this.reorderSuggestionsToolStripMenuItem.Click += new System.EventHandler(this.reorderSuggestionsToolStripMenuItem_Click);
            
            // advancedAnalyticsToolStripMenuItem
            this.advancedAnalyticsToolStripMenuItem.Name = "advancedAnalyticsToolStripMenuItem";
            this.advancedAnalyticsToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.advancedAnalyticsToolStripMenuItem.Text = "📈 Advanced Analytics";
            this.advancedAnalyticsToolStripMenuItem.Click += new System.EventHandler(this.advancedAnalyticsToolStripMenuItem_Click);
            
            // supplierAnalysisToolStripMenuItem
            this.supplierAnalysisToolStripMenuItem.Name = "supplierAnalysisToolStripMenuItem";
            this.supplierAnalysisToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.supplierAnalysisToolStripMenuItem.Text = "🏭 Supplier Analysis";
            this.supplierAnalysisToolStripMenuItem.Click += new System.EventHandler(this.supplierAnalysisToolStripMenuItem_Click);
            
            // hệThốngToolStripMenuItem
            this.hệThốngToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.đăngXuấtToolStripMenuItem,
            this.toolStripSeparator1,
            this.thoátToolStripMenuItem});
            this.hệThốngToolStripMenuItem.Name = "hệThốngToolStripMenuItem";
            this.hệThốngToolStripMenuItem.Size = new System.Drawing.Size(69, 20);
            this.hệThốngToolStripMenuItem.Text = "Hệ thống";
            
            // đăngXuấtToolStripMenuItem
            this.đăngXuấtToolStripMenuItem.Name = "đăngXuấtToolStripMenuItem";
            this.đăngXuấtToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.đăngXuấtToolStripMenuItem.Text = "Đăng xuất";
            this.đăngXuấtToolStripMenuItem.Click += new System.EventHandler(this.đăngXuấtToolStripMenuItem_Click);
            
            // toolStripSeparator1
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(121, 6);
            
            // thoátToolStripMenuItem
            this.thoátToolStripMenuItem.Name = "thoátToolStripMenuItem";
            this.thoátToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.thoátToolStripMenuItem.Text = "Thoát";
            this.thoátToolStripMenuItem.Click += new System.EventHandler(this.thoátToolStripMenuItem_Click);
            
            // tabControl1
            this.tabControl1.Controls.Add(this.tabProducts);
            this.tabControl1.Controls.Add(this.tabGoodsReceipt);
            this.tabControl1.Controls.Add(this.tabReports);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 24);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1200, 676);
            this.tabControl1.TabIndex = 1;
            
            // tabProducts
            this.tabProducts.Controls.Add(this.dgvProducts);
            this.tabProducts.Controls.Add(this.dgvCategories);
            this.tabProducts.Controls.Add(this.dgvSuppliers);
            this.tabProducts.Controls.Add(this.btnAddProduct);
            this.tabProducts.Controls.Add(this.btnUpdatePrice);
            this.tabProducts.Controls.Add(this.btnRefreshProducts);
            this.tabProducts.Location = new System.Drawing.Point(4, 25);
            this.tabProducts.Name = "tabProducts";
            this.tabProducts.Padding = new System.Windows.Forms.Padding(3);
            this.tabProducts.Size = new System.Drawing.Size(1192, 647);
            this.tabProducts.TabIndex = 0;
            this.tabProducts.Text = "Quản lý Sản phẩm";
            this.tabProducts.UseVisualStyleBackColor = true;
            
            // dgvProducts
            this.dgvProducts.AllowUserToAddRows = false;
            this.dgvProducts.AllowUserToDeleteRows = false;
            this.dgvProducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProducts.Location = new System.Drawing.Point(6, 6);
            this.dgvProducts.Name = "dgvProducts";
            this.dgvProducts.ReadOnly = true;
            this.dgvProducts.Size = new System.Drawing.Size(580, 400);
            this.dgvProducts.TabIndex = 0;
            
            // dgvCategories
            this.dgvCategories.AllowUserToAddRows = false;
            this.dgvCategories.AllowUserToDeleteRows = false;
            this.dgvCategories.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCategories.Location = new System.Drawing.Point(600, 6);
            this.dgvCategories.Name = "dgvCategories";
            this.dgvCategories.ReadOnly = true;
            this.dgvCategories.Size = new System.Drawing.Size(580, 180);
            this.dgvCategories.TabIndex = 1;
            
            // dgvSuppliers
            this.dgvSuppliers.AllowUserToAddRows = false;
            this.dgvSuppliers.AllowUserToDeleteRows = false;
            this.dgvSuppliers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSuppliers.Location = new System.Drawing.Point(600, 200);
            this.dgvSuppliers.Name = "dgvSuppliers";
            this.dgvSuppliers.ReadOnly = true;
            this.dgvSuppliers.Size = new System.Drawing.Size(580, 206);
            this.dgvSuppliers.TabIndex = 2;
            
            // btnAddProduct
            this.btnAddProduct.Location = new System.Drawing.Point(6, 420);
            this.btnAddProduct.Name = "btnAddProduct";
            this.btnAddProduct.Size = new System.Drawing.Size(120, 30);
            this.btnAddProduct.TabIndex = 3;
            this.btnAddProduct.Text = "Thêm sản phẩm";
            this.btnAddProduct.UseVisualStyleBackColor = true;
            this.btnAddProduct.Click += new System.EventHandler(this.btnAddProduct_Click);
            
            // btnUpdatePrice
            this.btnUpdatePrice.Location = new System.Drawing.Point(140, 420);
            this.btnUpdatePrice.Name = "btnUpdatePrice";
            this.btnUpdatePrice.Size = new System.Drawing.Size(120, 30);
            this.btnUpdatePrice.TabIndex = 4;
            this.btnUpdatePrice.Text = "Điều chỉnh giá";
            this.btnUpdatePrice.UseVisualStyleBackColor = true;
            this.btnUpdatePrice.Click += new System.EventHandler(this.btnUpdatePrice_Click);
            
            // btnRefreshProducts
            this.btnRefreshProducts.Location = new System.Drawing.Point(274, 420);
            this.btnRefreshProducts.Name = "btnRefreshProducts";
            this.btnRefreshProducts.Size = new System.Drawing.Size(120, 30);
            this.btnRefreshProducts.TabIndex = 5;
            this.btnRefreshProducts.Text = "Làm mới";
            this.btnRefreshProducts.UseVisualStyleBackColor = true;
            this.btnRefreshProducts.Click += new System.EventHandler(this.btnRefreshProducts_Click);
            
            // tabGoodsReceipt
            this.tabGoodsReceipt.Controls.Add(this.dgvGoodsReceipts);
            this.tabGoodsReceipt.Controls.Add(this.dgvGoodsReceiptDetails);
            this.tabGoodsReceipt.Controls.Add(this.btnCreateReceipt);
            this.tabGoodsReceipt.Controls.Add(this.btnDeleteReceipt);
            this.tabGoodsReceipt.Controls.Add(this.btnRefreshReceipts);
            this.tabGoodsReceipt.Location = new System.Drawing.Point(4, 25);
            this.tabGoodsReceipt.Name = "tabGoodsReceipt";
            this.tabGoodsReceipt.Padding = new System.Windows.Forms.Padding(3);
            this.tabGoodsReceipt.Size = new System.Drawing.Size(1192, 647);
            this.tabGoodsReceipt.TabIndex = 1;
            this.tabGoodsReceipt.Text = "Nhập hàng";
            this.tabGoodsReceipt.UseVisualStyleBackColor = true;
            
            // dgvGoodsReceipts
            this.dgvGoodsReceipts.AllowUserToAddRows = false;
            this.dgvGoodsReceipts.AllowUserToDeleteRows = false;
            this.dgvGoodsReceipts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvGoodsReceipts.Location = new System.Drawing.Point(6, 6);
            this.dgvGoodsReceipts.Name = "dgvGoodsReceipts";
            this.dgvGoodsReceipts.ReadOnly = true;
            this.dgvGoodsReceipts.Size = new System.Drawing.Size(580, 400);
            this.dgvGoodsReceipts.TabIndex = 0;
            
            // dgvGoodsReceiptDetails
            this.dgvGoodsReceiptDetails.AllowUserToAddRows = false;
            this.dgvGoodsReceiptDetails.AllowUserToDeleteRows = false;
            this.dgvGoodsReceiptDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvGoodsReceiptDetails.Location = new System.Drawing.Point(600, 6);
            this.dgvGoodsReceiptDetails.Name = "dgvGoodsReceiptDetails";
            this.dgvGoodsReceiptDetails.ReadOnly = true;
            this.dgvGoodsReceiptDetails.Size = new System.Drawing.Size(580, 400);
            this.dgvGoodsReceiptDetails.TabIndex = 1;
            
            // btnCreateReceipt
            this.btnCreateReceipt.Location = new System.Drawing.Point(6, 420);
            this.btnCreateReceipt.Name = "btnCreateReceipt";
            this.btnCreateReceipt.Size = new System.Drawing.Size(120, 30);
            this.btnCreateReceipt.TabIndex = 2;
            this.btnCreateReceipt.Text = "Tạo phiếu nhập";
            this.btnCreateReceipt.UseVisualStyleBackColor = true;
            this.btnCreateReceipt.Click += new System.EventHandler(this.btnCreateReceipt_Click);
            
            // btnDeleteReceipt
            this.btnDeleteReceipt.Location = new System.Drawing.Point(140, 420);
            this.btnDeleteReceipt.Name = "btnDeleteReceipt";
            this.btnDeleteReceipt.Size = new System.Drawing.Size(120, 30);
            this.btnDeleteReceipt.TabIndex = 3;
            this.btnDeleteReceipt.Text = "Xóa phiếu";
            this.btnDeleteReceipt.UseVisualStyleBackColor = true;
            this.btnDeleteReceipt.Click += new System.EventHandler(this.btnDeleteReceipt_Click);
            
            // btnRefreshReceipts
            this.btnRefreshReceipts.Location = new System.Drawing.Point(274, 420);
            this.btnRefreshReceipts.Name = "btnRefreshReceipts";
            this.btnRefreshReceipts.Size = new System.Drawing.Size(120, 30);
            this.btnRefreshReceipts.TabIndex = 4;
            this.btnRefreshReceipts.Text = "Làm mới";
            this.btnRefreshReceipts.UseVisualStyleBackColor = true;
            this.btnRefreshReceipts.Click += new System.EventHandler(this.btnRefreshReceipts_Click);
            
            // tabReports
            this.tabReports.Controls.Add(this.dgvInventoryValuation);
            this.tabReports.Controls.Add(this.dgvImportLines);
            this.tabReports.Controls.Add(this.dgvTopProducts);
            this.tabReports.Controls.Add(this.btnLoadInventory);
            this.tabReports.Controls.Add(this.btnLoadImportLines);
            this.tabReports.Controls.Add(this.btnLoadTopProducts);
            this.tabReports.Controls.Add(this.btnLoadSupplierSummary);
            this.tabReports.Location = new System.Drawing.Point(4, 25);
            this.tabReports.Name = "tabReports";
            this.tabReports.Padding = new System.Windows.Forms.Padding(3);
            this.tabReports.Size = new System.Drawing.Size(1192, 647);
            this.tabReports.TabIndex = 2;
            this.tabReports.Text = "Báo cáo";
            this.tabReports.UseVisualStyleBackColor = true;
            
            // dgvInventoryValuation
            this.dgvInventoryValuation.AllowUserToAddRows = false;
            this.dgvInventoryValuation.AllowUserToDeleteRows = false;
            this.dgvInventoryValuation.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvInventoryValuation.Location = new System.Drawing.Point(6, 6);
            this.dgvInventoryValuation.Name = "dgvInventoryValuation";
            this.dgvInventoryValuation.ReadOnly = true;
            this.dgvInventoryValuation.Size = new System.Drawing.Size(580, 200);
            this.dgvInventoryValuation.TabIndex = 0;
            
            // dgvImportLines
            this.dgvImportLines.AllowUserToAddRows = false;
            this.dgvImportLines.AllowUserToDeleteRows = false;
            this.dgvImportLines.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvImportLines.Location = new System.Drawing.Point(6, 220);
            this.dgvImportLines.Name = "dgvImportLines";
            this.dgvImportLines.ReadOnly = true;
            this.dgvImportLines.Size = new System.Drawing.Size(580, 200);
            this.dgvImportLines.TabIndex = 1;
            
            // dgvTopProducts
            this.dgvTopProducts.AllowUserToAddRows = false;
            this.dgvTopProducts.AllowUserToDeleteRows = false;
            this.dgvTopProducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTopProducts.Location = new System.Drawing.Point(600, 6);
            this.dgvTopProducts.Name = "dgvTopProducts";
            this.dgvTopProducts.ReadOnly = true;
            this.dgvTopProducts.Size = new System.Drawing.Size(580, 200);
            this.dgvTopProducts.TabIndex = 2;
            
            // btnLoadInventory
            this.btnLoadInventory.Location = new System.Drawing.Point(6, 440);
            this.btnLoadInventory.Name = "btnLoadInventory";
            this.btnLoadInventory.Size = new System.Drawing.Size(140, 30);
            this.btnLoadInventory.TabIndex = 3;
            this.btnLoadInventory.Text = "Tải định giá tồn kho";
            this.btnLoadInventory.UseVisualStyleBackColor = true;
            this.btnLoadInventory.Click += new System.EventHandler(this.btnLoadInventory_Click);
            
            // btnLoadImportLines
            this.btnLoadImportLines.Location = new System.Drawing.Point(160, 440);
            this.btnLoadImportLines.Name = "btnLoadImportLines";
            this.btnLoadImportLines.Size = new System.Drawing.Size(140, 30);
            this.btnLoadImportLines.TabIndex = 4;
            this.btnLoadImportLines.Text = "Tải dòng nhập hàng";
            this.btnLoadImportLines.UseVisualStyleBackColor = true;
            this.btnLoadImportLines.Click += new System.EventHandler(this.btnLoadImportLines_Click);
            
            // btnLoadTopProducts
            this.btnLoadTopProducts.Location = new System.Drawing.Point(314, 440);
            this.btnLoadTopProducts.Name = "btnLoadTopProducts";
            this.btnLoadTopProducts.Size = new System.Drawing.Size(140, 30);
            this.btnLoadTopProducts.TabIndex = 5;
            this.btnLoadTopProducts.Text = "Tải top sản phẩm";
            this.btnLoadTopProducts.UseVisualStyleBackColor = true;
            this.btnLoadTopProducts.Click += new System.EventHandler(this.btnLoadTopProducts_Click);
            
            // btnLoadSupplierSummary
            this.btnLoadSupplierSummary.Location = new System.Drawing.Point(468, 440);
            this.btnLoadSupplierSummary.Name = "btnLoadSupplierSummary";
            this.btnLoadSupplierSummary.Size = new System.Drawing.Size(140, 30);
            this.btnLoadSupplierSummary.TabIndex = 6;
            this.btnLoadSupplierSummary.Text = "Tổng hợp nhà cung cấp";
            this.btnLoadSupplierSummary.UseVisualStyleBackColor = true;
            this.btnLoadSupplierSummary.Click += new System.EventHandler(this.btnLoadSupplierSummary_Click);
            
            // statusStrip1
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 678);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1200, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            
            // toolStripStatusLabel1
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(1183, 17);
            this.toolStripStatusLabel1.Spring = true;
            this.toolStripStatusLabel1.Text = "Sẵn sàng";
            this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            
            // lblConnectionStatus
            this.lblConnectionStatus.AutoSize = true;
            this.lblConnectionStatus.Location = new System.Drawing.Point(12, 655);
            this.lblConnectionStatus.Name = "lblConnectionStatus";
            this.lblConnectionStatus.Size = new System.Drawing.Size(100, 15);
            this.lblConnectionStatus.TabIndex = 3;
            this.lblConnectionStatus.Text = "Kết nối: Đang kiểm tra...";
            
            // lblUserInfo
            this.lblUserInfo.AutoSize = true;
            this.lblUserInfo.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblUserInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(102)))), ((int)(((byte)(204)))));
            this.lblUserInfo.Location = new System.Drawing.Point(200, 655);
            this.lblUserInfo.Name = "lblUserInfo";
            this.lblUserInfo.Size = new System.Drawing.Size(150, 20);
            this.lblUserInfo.TabIndex = 4;
            this.lblUserInfo.Text = "Xin chào: User (Role)";
            
            // SellerMainForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 700);
            this.Controls.Add(this.lblUserInfo);
            this.Controls.Add(this.lblConnectionStatus);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "SellerMainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hệ thống Quản lý Nhập hàng - Seller";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabProducts.ResumeLayout(false);
            this.tabGoodsReceipt.ResumeLayout(false);
            this.tabReports.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCategories)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSuppliers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGoodsReceipts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGoodsReceiptDetails)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInventoryValuation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvImportLines)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTopProducts)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem hệThốngToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem đăngXuấtToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem thoátToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem advancedFeaturesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem smartPricingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reorderSuggestionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem advancedAnalyticsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem supplierAnalysisToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabProducts;
        private System.Windows.Forms.TabPage tabGoodsReceipt;
        private System.Windows.Forms.TabPage tabReports;
        
        // Tab Products
        private System.Windows.Forms.DataGridView dgvProducts;
        private System.Windows.Forms.DataGridView dgvCategories;
        private System.Windows.Forms.DataGridView dgvSuppliers;
        private System.Windows.Forms.Button btnAddProduct;
        private System.Windows.Forms.Button btnUpdatePrice;
        private System.Windows.Forms.Button btnRefreshProducts;
        
        // Tab Goods Receipt
        private System.Windows.Forms.DataGridView dgvGoodsReceipts;
        private System.Windows.Forms.DataGridView dgvGoodsReceiptDetails;
        private System.Windows.Forms.Button btnCreateReceipt;
        private System.Windows.Forms.Button btnDeleteReceipt;
        private System.Windows.Forms.Button btnRefreshReceipts;
        
        // Tab Reports
        private System.Windows.Forms.DataGridView dgvInventoryValuation;
        private System.Windows.Forms.DataGridView dgvImportLines;
        private System.Windows.Forms.DataGridView dgvTopProducts;
        private System.Windows.Forms.Button btnLoadInventory;
        private System.Windows.Forms.Button btnLoadImportLines;
        private System.Windows.Forms.Button btnLoadTopProducts;
        private System.Windows.Forms.Button btnLoadSupplierSummary;
        
        // Status and Info
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Label lblConnectionStatus;
        private System.Windows.Forms.Label lblUserInfo;
    }
}
