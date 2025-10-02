namespace dbms
{
    partial class SupplierAnalysisForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnAnalyze = new System.Windows.Forms.Button();
            this.numMonths = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbSupplier = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblRating = new System.Windows.Forms.Label();
            this.lblScoreGauge = new System.Windows.Forms.Label();
            this.txtDetails = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dgvComparison = new System.Windows.Forms.DataGridView();
            this.btnCompareAll = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMonths)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvComparison)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.label1.Location = new System.Drawing.Point(15, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(438, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "🏭 Supplier Analysis - Đánh giá Nhà cung cấp";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnAnalyze);
            this.groupBox1.Controls.Add(this.numMonths);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cmbSupplier);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(20, 60);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(500, 120);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Chọn Nhà cung cấp";
            // 
            // btnAnalyze
            // 
            this.btnAnalyze.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnAnalyze.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAnalyze.ForeColor = System.Drawing.Color.White;
            this.btnAnalyze.Location = new System.Drawing.Point(350, 35);
            this.btnAnalyze.Name = "btnAnalyze";
            this.btnAnalyze.Size = new System.Drawing.Size(130, 60);
            this.btnAnalyze.TabIndex = 4;
            this.btnAnalyze.Text = "📊 Analyze";
            this.btnAnalyze.UseVisualStyleBackColor = false;
            this.btnAnalyze.Click += new System.EventHandler(this.btnAnalyze_Click);
            // 
            // numMonths
            // 
            this.numMonths.Location = new System.Drawing.Point(150, 70);
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
            this.numMonths.Size = new System.Drawing.Size(150, 22);
            this.numMonths.TabIndex = 3;
            this.numMonths.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(125, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "Period (Months):";
            // 
            // cmbSupplier
            // 
            this.cmbSupplier.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSupplier.FormattingEnabled = true;
            this.cmbSupplier.Location = new System.Drawing.Point(150, 35);
            this.cmbSupplier.Name = "cmbSupplier";
            this.cmbSupplier.Size = new System.Drawing.Size(180, 24);
            this.cmbSupplier.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(126, 16);
            this.label2.TabIndex = 0;
            this.label2.Text = "Nhà cung cấp:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblRating);
            this.groupBox2.Controls.Add(this.lblScoreGauge);
            this.groupBox2.Controls.Add(this.txtDetails);
            this.groupBox2.Location = new System.Drawing.Point(20, 190);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(500, 440);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Performance Score";
            // 
            // lblRating
            // 
            this.lblRating.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.lblRating.Location = new System.Drawing.Point(20, 90);
            this.lblRating.Name = "lblRating";
            this.lblRating.Size = new System.Drawing.Size(460, 30);
            this.lblRating.TabIndex = 2;
            this.lblRating.Text = "-";
            this.lblRating.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblScoreGauge
            // 
            this.lblScoreGauge.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold);
            this.lblScoreGauge.Location = new System.Drawing.Point(20, 30);
            this.lblScoreGauge.Name = "lblScoreGauge";
            this.lblScoreGauge.Size = new System.Drawing.Size(460, 50);
            this.lblScoreGauge.TabIndex = 1;
            this.lblScoreGauge.Text = "0/100\n░░░░░░░░░░░░░░░░░░░░";
            this.lblScoreGauge.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtDetails
            // 
            this.txtDetails.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtDetails.Location = new System.Drawing.Point(20, 130);
            this.txtDetails.Multiline = true;
            this.txtDetails.Name = "txtDetails";
            this.txtDetails.ReadOnly = true;
            this.txtDetails.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDetails.Size = new System.Drawing.Size(460, 290);
            this.txtDetails.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dgvComparison);
            this.groupBox3.Controls.Add(this.btnCompareAll);
            this.groupBox3.Location = new System.Drawing.Point(540, 60);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(520, 570);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Compare All Suppliers";
            // 
            // dgvComparison
            // 
            this.dgvComparison.AllowUserToAddRows = false;
            this.dgvComparison.AllowUserToDeleteRows = false;
            this.dgvComparison.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvComparison.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvComparison.Location = new System.Drawing.Point(20, 80);
            this.dgvComparison.Name = "dgvComparison";
            this.dgvComparison.ReadOnly = true;
            this.dgvComparison.RowHeadersWidth = 51;
            this.dgvComparison.Size = new System.Drawing.Size(480, 470);
            this.dgvComparison.TabIndex = 1;
            // 
            // btnCompareAll
            // 
            this.btnCompareAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnCompareAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCompareAll.ForeColor = System.Drawing.Color.White;
            this.btnCompareAll.Location = new System.Drawing.Point(20, 30);
            this.btnCompareAll.Name = "btnCompareAll";
            this.btnCompareAll.Size = new System.Drawing.Size(180, 35);
            this.btnCompareAll.TabIndex = 0;
            this.btnCompareAll.Text = "📊 Compare All";
            this.btnCompareAll.UseVisualStyleBackColor = false;
            this.btnCompareAll.Click += new System.EventHandler(this.btnCompareAll_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(960, 645);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 35);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Đóng";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // SupplierAnalysisForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1080, 700);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "SupplierAnalysisForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Supplier Analysis - Đánh giá Nhà cung cấp";
            this.Load += new System.EventHandler(this.SupplierAnalysisForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMonths)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvComparison)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cmbSupplier;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numMonths;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnAnalyze;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtDetails;
        private System.Windows.Forms.Label lblScoreGauge;
        private System.Windows.Forms.Label lblRating;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView dgvComparison;
        private System.Windows.Forms.Button btnCompareAll;
        private System.Windows.Forms.Button btnClose;
    }
}
