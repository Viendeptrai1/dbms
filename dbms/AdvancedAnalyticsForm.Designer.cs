namespace dbms
{
    partial class AdvancedAnalyticsForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabABC = new System.Windows.Forms.TabPage();
            this.lblABCStats = new System.Windows.Forms.Label();
            this.dgvABC = new System.Windows.Forms.DataGridView();
            this.btnLoadABC = new System.Windows.Forms.Button();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tabTurnover = new System.Windows.Forms.TabPage();
            this.dgvTurnover = new System.Windows.Forms.DataGridView();
            this.btnLoadTurnover = new System.Windows.Forms.Button();
            this.numMonths = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.tabPriceTrend = new System.Windows.Forms.TabPage();
            this.txtPriceTrend = new System.Windows.Forms.TextBox();
            this.btnLoadPriceTrend = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabABC.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvABC)).BeginInit();
            this.tabTurnover.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTurnover)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMonths)).BeginInit();
            this.tabPriceTrend.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.label1.Location = new System.Drawing.Point(15, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(398, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "📈 Advanced Analytics - Phân tích Nâng cao";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabABC);
            this.tabControl1.Controls.Add(this.tabTurnover);
            this.tabControl1.Controls.Add(this.tabPriceTrend);
            this.tabControl1.Location = new System.Drawing.Point(20, 60);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1040, 520);
            this.tabControl1.TabIndex = 1;
            // 
            // tabABC
            // 
            this.tabABC.Controls.Add(this.lblABCStats);
            this.tabABC.Controls.Add(this.dgvABC);
            this.tabABC.Controls.Add(this.btnLoadABC);
            this.tabABC.Controls.Add(this.dtpTo);
            this.tabABC.Controls.Add(this.dtpFrom);
            this.tabABC.Controls.Add(this.label3);
            this.tabABC.Controls.Add(this.label2);
            this.tabABC.Location = new System.Drawing.Point(4, 25);
            this.tabABC.Name = "tabABC";
            this.tabABC.Padding = new System.Windows.Forms.Padding(3);
            this.tabABC.Size = new System.Drawing.Size(1032, 491);
            this.tabABC.TabIndex = 0;
            this.tabABC.Text = "ABC Analysis";
            this.tabABC.UseVisualStyleBackColor = true;
            // 
            // lblABCStats
            // 
            this.lblABCStats.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.lblABCStats.Location = new System.Drawing.Point(20, 450);
            this.lblABCStats.Name = "lblABCStats";
            this.lblABCStats.Size = new System.Drawing.Size(980, 30);
            this.lblABCStats.TabIndex = 6;
            this.lblABCStats.Text = "Class A: 0 | Class B: 0 | Class C: 0";
            this.lblABCStats.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dgvABC
            // 
            this.dgvABC.AllowUserToAddRows = false;
            this.dgvABC.AllowUserToDeleteRows = false;
            this.dgvABC.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvABC.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvABC.Location = new System.Drawing.Point(20, 80);
            this.dgvABC.Name = "dgvABC";
            this.dgvABC.ReadOnly = true;
            this.dgvABC.RowHeadersWidth = 51;
            this.dgvABC.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvABC.Size = new System.Drawing.Size(990, 360);
            this.dgvABC.TabIndex = 5;
            // 
            // btnLoadABC
            // 
            this.btnLoadABC.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnLoadABC.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadABC.ForeColor = System.Drawing.Color.White;
            this.btnLoadABC.Location = new System.Drawing.Point(540, 25);
            this.btnLoadABC.Name = "btnLoadABC";
            this.btnLoadABC.Size = new System.Drawing.Size(120, 35);
            this.btnLoadABC.TabIndex = 4;
            this.btnLoadABC.Text = "📊 Load Data";
            this.btnLoadABC.UseVisualStyleBackColor = false;
            this.btnLoadABC.Click += new System.EventHandler(this.btnLoadABC_Click);
            // 
            // dtpTo
            // 
            this.dtpTo.Location = new System.Drawing.Point(350, 32);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(150, 22);
            this.dtpTo.TabIndex = 3;
            // 
            // dtpFrom
            // 
            this.dtpFrom.Location = new System.Drawing.Point(100, 32);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(150, 22);
            this.dtpFrom.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(280, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 16);
            this.label3.TabIndex = 1;
            this.label3.Text = "To Date:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 16);
            this.label2.TabIndex = 0;
            this.label2.Text = "From Date:";
            // 
            // tabTurnover
            // 
            this.tabTurnover.Controls.Add(this.dgvTurnover);
            this.tabTurnover.Controls.Add(this.btnLoadTurnover);
            this.tabTurnover.Controls.Add(this.numMonths);
            this.tabTurnover.Controls.Add(this.label4);
            this.tabTurnover.Location = new System.Drawing.Point(4, 25);
            this.tabTurnover.Name = "tabTurnover";
            this.tabTurnover.Size = new System.Drawing.Size(1032, 491);
            this.tabTurnover.TabIndex = 1;
            this.tabTurnover.Text = "Inventory Turnover";
            this.tabTurnover.UseVisualStyleBackColor = true;
            // 
            // dgvTurnover
            // 
            this.dgvTurnover.AllowUserToAddRows = false;
            this.dgvTurnover.AllowUserToDeleteRows = false;
            this.dgvTurnover.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTurnover.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTurnover.Location = new System.Drawing.Point(20, 80);
            this.dgvTurnover.Name = "dgvTurnover";
            this.dgvTurnover.ReadOnly = true;
            this.dgvTurnover.RowHeadersWidth = 51;
            this.dgvTurnover.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTurnover.Size = new System.Drawing.Size(990, 390);
            this.dgvTurnover.TabIndex = 3;
            // 
            // btnLoadTurnover
            // 
            this.btnLoadTurnover.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnLoadTurnover.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadTurnover.ForeColor = System.Drawing.Color.White;
            this.btnLoadTurnover.Location = new System.Drawing.Point(250, 25);
            this.btnLoadTurnover.Name = "btnLoadTurnover";
            this.btnLoadTurnover.Size = new System.Drawing.Size(120, 35);
            this.btnLoadTurnover.TabIndex = 2;
            this.btnLoadTurnover.Text = "📊 Load Data";
            this.btnLoadTurnover.UseVisualStyleBackColor = false;
            this.btnLoadTurnover.Click += new System.EventHandler(this.btnLoadTurnover_Click);
            // 
            // numMonths
            // 
            this.numMonths.Location = new System.Drawing.Point(120, 32);
            this.numMonths.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.numMonths.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMonths.Name = "numMonths";
            this.numMonths.Size = new System.Drawing.Size(100, 22);
            this.numMonths.TabIndex = 1;
            this.numMonths.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 16);
            this.label4.TabIndex = 0;
            this.label4.Text = "Period (Months):";
            // 
            // tabPriceTrend
            // 
            this.tabPriceTrend.Controls.Add(this.txtPriceTrend);
            this.tabPriceTrend.Controls.Add(this.btnLoadPriceTrend);
            this.tabPriceTrend.Controls.Add(this.label5);
            this.tabPriceTrend.Location = new System.Drawing.Point(4, 25);
            this.tabPriceTrend.Name = "tabPriceTrend";
            this.tabPriceTrend.Size = new System.Drawing.Size(1032, 491);
            this.tabPriceTrend.TabIndex = 2;
            this.tabPriceTrend.Text = "Price Trend";
            this.tabPriceTrend.UseVisualStyleBackColor = true;
            // 
            // txtPriceTrend
            // 
            this.txtPriceTrend.Font = new System.Drawing.Font("Consolas", 10F);
            this.txtPriceTrend.Location = new System.Drawing.Point(20, 80);
            this.txtPriceTrend.Multiline = true;
            this.txtPriceTrend.Name = "txtPriceTrend";
            this.txtPriceTrend.ReadOnly = true;
            this.txtPriceTrend.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtPriceTrend.Size = new System.Drawing.Size(990, 390);
            this.txtPriceTrend.TabIndex = 2;
            // 
            // btnLoadPriceTrend
            // 
            this.btnLoadPriceTrend.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnLoadPriceTrend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadPriceTrend.ForeColor = System.Drawing.Color.White;
            this.btnLoadPriceTrend.Location = new System.Drawing.Point(20, 25);
            this.btnLoadPriceTrend.Name = "btnLoadPriceTrend";
            this.btnLoadPriceTrend.Size = new System.Drawing.Size(200, 35);
            this.btnLoadPriceTrend.TabIndex = 1;
            this.btnLoadPriceTrend.Text = "📈 Load Selected Product";
            this.btnLoadPriceTrend.UseVisualStyleBackColor = false;
            this.btnLoadPriceTrend.Click += new System.EventHandler(this.btnLoadPriceTrend_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Gray;
            this.label5.Location = new System.Drawing.Point(240, 35);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(350, 16);
            this.label5.TabIndex = 0;
            this.label5.Text = "Select a product from ABC or Turnover tab first";
            // 
            // btnExport
            // 
            this.btnExport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(125)))), ((int)(((byte)(50)))));
            this.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExport.ForeColor = System.Drawing.Color.White;
            this.btnExport.Location = new System.Drawing.Point(20, 595);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(120, 35);
            this.btnExport.TabIndex = 2;
            this.btnExport.Text = "📊 Export CSV";
            this.btnExport.UseVisualStyleBackColor = false;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(960, 595);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 35);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Đóng";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // AdvancedAnalyticsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1080, 650);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "AdvancedAnalyticsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Advanced Analytics - Phân tích Nâng cao";
            this.Load += new System.EventHandler(this.AdvancedAnalyticsForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabABC.ResumeLayout(false);
            this.tabABC.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvABC)).EndInit();
            this.tabTurnover.ResumeLayout(false);
            this.tabTurnover.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTurnover)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMonths)).EndInit();
            this.tabPriceTrend.ResumeLayout(false);
            this.tabPriceTrend.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabABC;
        private System.Windows.Forms.TabPage tabTurnover;
        private System.Windows.Forms.TabPage tabPriceTrend;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnLoadABC;
        private System.Windows.Forms.DataGridView dgvABC;
        private System.Windows.Forms.Label lblABCStats;
        private System.Windows.Forms.NumericUpDown numMonths;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnLoadTurnover;
        private System.Windows.Forms.DataGridView dgvTurnover;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnLoadPriceTrend;
        private System.Windows.Forms.TextBox txtPriceTrend;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnClose;
    }
}
