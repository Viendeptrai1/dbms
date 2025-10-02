namespace dbms
{
    partial class SmartPricingForm
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
            this.tabSingle = new System.Windows.Forms.TabPage();
            this.lblCurrentPrice = new System.Windows.Forms.Label();
            this.btnUpdateSinglePrice = new System.Windows.Forms.Button();
            this.txtReason = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtNewPrice = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dgvProducts = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.tabStrategy = new System.Windows.Forms.TabPage();
            this.lblPercentHint = new System.Windows.Forms.Label();
            this.txtAdjustPercent = new System.Windows.Forms.TextBox();
            this.lblAdjustPercent = new System.Windows.Forms.Label();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnApplyStrategy = new System.Windows.Forms.Button();
            this.cmbStrategy = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tabHistory = new System.Windows.Forms.TabPage();
            this.dgvPriceHistory = new System.Windows.Forms.DataGridView();
            this.btnLoadPriceHistory = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabSingle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).BeginInit();
            this.tabStrategy.SuspendLayout();
            this.tabHistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPriceHistory)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabSingle);
            this.tabControl1.Controls.Add(this.tabStrategy);
            this.tabControl1.Controls.Add(this.tabHistory);
            this.tabControl1.Location = new System.Drawing.Point(12, 60);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(960, 500);
            this.tabControl1.TabIndex = 0;
            // 
            // tabSingle
            // 
            this.tabSingle.Controls.Add(this.lblCurrentPrice);
            this.tabSingle.Controls.Add(this.btnUpdateSinglePrice);
            this.tabSingle.Controls.Add(this.txtReason);
            this.tabSingle.Controls.Add(this.label4);
            this.tabSingle.Controls.Add(this.txtNewPrice);
            this.tabSingle.Controls.Add(this.label3);
            this.tabSingle.Controls.Add(this.dgvProducts);
            this.tabSingle.Controls.Add(this.label2);
            this.tabSingle.Location = new System.Drawing.Point(4, 25);
            this.tabSingle.Name = "tabSingle";
            this.tabSingle.Padding = new System.Windows.Forms.Padding(3);
            this.tabSingle.Size = new System.Drawing.Size(952, 471);
            this.tabSingle.TabIndex = 0;
            this.tabSingle.Text = "Cập nhật Giá 1 Món";
            this.tabSingle.UseVisualStyleBackColor = true;
            // 
            // lblCurrentPrice
            // 
            this.lblCurrentPrice.AutoSize = true;
            this.lblCurrentPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.lblCurrentPrice.ForeColor = System.Drawing.Color.Blue;
            this.lblCurrentPrice.Location = new System.Drawing.Point(20, 320);
            this.lblCurrentPrice.Name = "lblCurrentPrice";
            this.lblCurrentPrice.Size = new System.Drawing.Size(139, 18);
            this.lblCurrentPrice.TabIndex = 7;
            this.lblCurrentPrice.Text = "Giá hiện tại: 0 đ";
            // 
            // btnUpdateSinglePrice
            // 
            this.btnUpdateSinglePrice.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnUpdateSinglePrice.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdateSinglePrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.btnUpdateSinglePrice.ForeColor = System.Drawing.Color.White;
            this.btnUpdateSinglePrice.Location = new System.Drawing.Point(23, 420);
            this.btnUpdateSinglePrice.Name = "btnUpdateSinglePrice";
            this.btnUpdateSinglePrice.Size = new System.Drawing.Size(200, 35);
            this.btnUpdateSinglePrice.TabIndex = 6;
            this.btnUpdateSinglePrice.Text = "✔ Cập nhật Giá";
            this.btnUpdateSinglePrice.UseVisualStyleBackColor = false;
            this.btnUpdateSinglePrice.Click += new System.EventHandler(this.btnUpdateSinglePrice_Click);
            // 
            // txtReason
            // 
            this.txtReason.Location = new System.Drawing.Point(23, 385);
            this.txtReason.Name = "txtReason";
            this.txtReason.Size = new System.Drawing.Size(400, 22);
            this.txtReason.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 365);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(134, 16);
            this.label4.TabIndex = 4;
            this.label4.Text = "Lý do (tùy chọn):";
            // 
            // txtNewPrice
            // 
            this.txtNewPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtNewPrice.Location = new System.Drawing.Point(23, 355);
            this.txtNewPrice.Name = "txtNewPrice";
            this.txtNewPrice.Size = new System.Drawing.Size(200, 26);
            this.txtNewPrice.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 335);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "Giá mới:";
            // 
            // dgvProducts
            // 
            this.dgvProducts.AllowUserToAddRows = false;
            this.dgvProducts.AllowUserToDeleteRows = false;
            this.dgvProducts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvProducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProducts.Location = new System.Drawing.Point(23, 45);
            this.dgvProducts.MultiSelect = false;
            this.dgvProducts.Name = "dgvProducts";
            this.dgvProducts.ReadOnly = true;
            this.dgvProducts.RowHeadersWidth = 51;
            this.dgvProducts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProducts.Size = new System.Drawing.Size(900, 260);
            this.dgvProducts.TabIndex = 1;
            this.dgvProducts.SelectionChanged += new System.EventHandler(this.dgvProducts_SelectionChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(20, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(180, 18);
            this.label2.TabIndex = 0;
            this.label2.Text = "Chọn sản phẩm cần cập nhật:";
            // 
            // tabStrategy
            // 
            this.tabStrategy.Controls.Add(this.lblPercentHint);
            this.tabStrategy.Controls.Add(this.txtAdjustPercent);
            this.tabStrategy.Controls.Add(this.lblAdjustPercent);
            this.tabStrategy.Controls.Add(this.cmbCategory);
            this.tabStrategy.Controls.Add(this.label6);
            this.tabStrategy.Controls.Add(this.btnApplyStrategy);
            this.tabStrategy.Controls.Add(this.cmbStrategy);
            this.tabStrategy.Controls.Add(this.label5);
            this.tabStrategy.Controls.Add(this.label7);
            this.tabStrategy.Location = new System.Drawing.Point(4, 25);
            this.tabStrategy.Name = "tabStrategy";
            this.tabStrategy.Padding = new System.Windows.Forms.Padding(3);
            this.tabStrategy.Size = new System.Drawing.Size(952, 471);
            this.tabStrategy.TabIndex = 1;
            this.tabStrategy.Text = "Strategy Điều chỉnh Giá";
            this.tabStrategy.UseVisualStyleBackColor = true;
            // 
            // lblPercentHint
            // 
            this.lblPercentHint.AutoSize = true;
            this.lblPercentHint.ForeColor = System.Drawing.Color.Gray;
            this.lblPercentHint.Location = new System.Drawing.Point(250, 175);
            this.lblPercentHint.Name = "lblPercentHint";
            this.lblPercentHint.Size = new System.Drawing.Size(200, 16);
            this.lblPercentHint.TabIndex = 8;
            this.lblPercentHint.Text = "(VD: 10 = tăng 10%, -5 = giảm 5%)";
            this.lblPercentHint.Visible = false;
            // 
            // txtAdjustPercent
            // 
            this.txtAdjustPercent.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtAdjustPercent.Location = new System.Drawing.Point(30, 170);
            this.txtAdjustPercent.Name = "txtAdjustPercent";
            this.txtAdjustPercent.Size = new System.Drawing.Size(200, 26);
            this.txtAdjustPercent.TabIndex = 7;
            this.txtAdjustPercent.Visible = false;
            // 
            // lblAdjustPercent
            // 
            this.lblAdjustPercent.AutoSize = true;
            this.lblAdjustPercent.Location = new System.Drawing.Point(27, 145);
            this.lblAdjustPercent.Name = "lblAdjustPercent";
            this.lblAdjustPercent.Size = new System.Drawing.Size(150, 16);
            this.lblAdjustPercent.TabIndex = 6;
            this.lblAdjustPercent.Text = "Phần trăm điều chỉnh (%):";
            this.lblAdjustPercent.Visible = false;
            // 
            // cmbCategory
            // 
            this.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategory.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.cmbCategory.FormattingEnabled = true;
            this.cmbCategory.Location = new System.Drawing.Point(30, 245);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(300, 28);
            this.cmbCategory.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(27, 220);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(147, 16);
            this.label6.TabIndex = 4;
            this.label6.Text = "Danh mục (tùy chọn):";
            // 
            // btnApplyStrategy
            // 
            this.btnApplyStrategy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnApplyStrategy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApplyStrategy.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.btnApplyStrategy.ForeColor = System.Drawing.Color.White;
            this.btnApplyStrategy.Location = new System.Drawing.Point(30, 300);
            this.btnApplyStrategy.Name = "btnApplyStrategy";
            this.btnApplyStrategy.Size = new System.Drawing.Size(250, 40);
            this.btnApplyStrategy.TabIndex = 3;
            this.btnApplyStrategy.Text = "⚡ Áp dụng Strategy";
            this.btnApplyStrategy.UseVisualStyleBackColor = false;
            this.btnApplyStrategy.Click += new System.EventHandler(this.btnApplyStrategy_Click);
            // 
            // cmbStrategy
            // 
            this.cmbStrategy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStrategy.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.cmbStrategy.FormattingEnabled = true;
            this.cmbStrategy.Items.AddRange(new object[] {
            "ABC-Based",
            "Age-Based",
            "Manual-Percent",
            "Turnover-Based"});
            this.cmbStrategy.Location = new System.Drawing.Point(30, 95);
            this.cmbStrategy.Name = "cmbStrategy";
            this.cmbStrategy.Size = new System.Drawing.Size(300, 28);
            this.cmbStrategy.TabIndex = 2;
            this.cmbStrategy.SelectedIndexChanged += new System.EventHandler(this.cmbStrategy_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(27, 70);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(126, 16);
            this.label5.TabIndex = 1;
            this.label5.Text = "Chọn Strategy:";
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label7.Location = new System.Drawing.Point(27, 15);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(900, 45);
            this.label7.TabIndex = 0;
            this.label7.Text = "Điều chỉnh giá hàng loạt theo các strategy thông minh:\r\n• ABC-Based: A +10%, B +5%, C -5%  • Age-Based: Giảm giá hàng tồn lâu  • Manual-Percent: Tùy chỉnh %  • Turnover-Based: Tăng giá hàng chạy";
            // 
            // tabHistory
            // 
            this.tabHistory.Controls.Add(this.dgvPriceHistory);
            this.tabHistory.Controls.Add(this.btnLoadPriceHistory);
            this.tabHistory.Controls.Add(this.label8);
            this.tabHistory.Location = new System.Drawing.Point(4, 25);
            this.tabHistory.Name = "tabHistory";
            this.tabHistory.Size = new System.Drawing.Size(952, 471);
            this.tabHistory.TabIndex = 2;
            this.tabHistory.Text = "Lịch sử Thay đổi Giá";
            this.tabHistory.UseVisualStyleBackColor = true;
            // 
            // dgvPriceHistory
            // 
            this.dgvPriceHistory.AllowUserToAddRows = false;
            this.dgvPriceHistory.AllowUserToDeleteRows = false;
            this.dgvPriceHistory.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPriceHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPriceHistory.Location = new System.Drawing.Point(20, 80);
            this.dgvPriceHistory.Name = "dgvPriceHistory";
            this.dgvPriceHistory.ReadOnly = true;
            this.dgvPriceHistory.RowHeadersWidth = 51;
            this.dgvPriceHistory.Size = new System.Drawing.Size(910, 370);
            this.dgvPriceHistory.TabIndex = 2;
            // 
            // btnLoadPriceHistory
            // 
            this.btnLoadPriceHistory.Location = new System.Drawing.Point(20, 40);
            this.btnLoadPriceHistory.Name = "btnLoadPriceHistory";
            this.btnLoadPriceHistory.Size = new System.Drawing.Size(200, 30);
            this.btnLoadPriceHistory.TabIndex = 1;
            this.btnLoadPriceHistory.Text = "🔍 Xem Lịch sử Giá";
            this.btnLoadPriceHistory.UseVisualStyleBackColor = true;
            this.btnLoadPriceHistory.Click += new System.EventHandler(this.btnLoadPriceHistory_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.label8.Location = new System.Drawing.Point(17, 15);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(350, 18);
            this.label8.TabIndex = 0;
            this.label8.Text = "Chọn sản phẩm trong tab 1, sau đó click nút bên dưới";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.label1.Location = new System.Drawing.Point(15, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(398, 29);
            this.label1.TabIndex = 1;
            this.label1.Text = "⚡ Smart Pricing - Quản lý Giá";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(872, 570);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 30);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Đóng";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // SmartPricingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 611);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "SmartPricingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Smart Pricing - Quản lý Giá Thông minh";
            this.Load += new System.EventHandler(this.SmartPricingForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabSingle.ResumeLayout(false);
            this.tabSingle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).EndInit();
            this.tabStrategy.ResumeLayout(false);
            this.tabStrategy.PerformLayout();
            this.tabHistory.ResumeLayout(false);
            this.tabHistory.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPriceHistory)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabSingle;
        private System.Windows.Forms.TabPage tabStrategy;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dgvProducts;
        private System.Windows.Forms.TextBox txtNewPrice;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtReason;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnUpdateSinglePrice;
        private System.Windows.Forms.Label lblCurrentPrice;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbStrategy;
        private System.Windows.Forms.Button btnApplyStrategy;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtAdjustPercent;
        private System.Windows.Forms.Label lblAdjustPercent;
        private System.Windows.Forms.Label lblPercentHint;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TabPage tabHistory;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnLoadPriceHistory;
        private System.Windows.Forms.DataGridView dgvPriceHistory;
        private System.Windows.Forms.Button btnClose;
    }
}
