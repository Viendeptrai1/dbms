namespace dbms
{
    partial class ReorderSuggestionsForm
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
            this.btnHelp = new System.Windows.Forms.Button();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.numServiceLevel = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.numDaysToProject = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.dgvSuggestions = new System.Windows.Forms.DataGridView();
            this.lblResultCount = new System.Windows.Forms.Label();
            this.btnExportToExcel = new System.Windows.Forms.Button();
            this.btnViewDetails = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numServiceLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDaysToProject)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSuggestions)).BeginInit();
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
            this.label1.Text = "📊 Reorder Suggestions - Đề xuất Nhập hàng";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnHelp);
            this.groupBox1.Controls.Add(this.btnGenerate);
            this.groupBox1.Controls.Add(this.numServiceLevel);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.numDaysToProject);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(20, 60);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1040, 90);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tham số";
            // 
            // btnHelp
            // 
            this.btnHelp.Location = new System.Drawing.Point(920, 35);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(100, 35);
            this.btnHelp.TabIndex = 5;
            this.btnHelp.Text = "❓ Hướng dẫn";
            this.btnHelp.UseVisualStyleBackColor = true;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnGenerate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.btnGenerate.ForeColor = System.Drawing.Color.White;
            this.btnGenerate.Location = new System.Drawing.Point(750, 35);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(150, 35);
            this.btnGenerate.TabIndex = 4;
            this.btnGenerate.Text = "🔄 Generate";
            this.btnGenerate.UseVisualStyleBackColor = false;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // numServiceLevel
            // 
            this.numServiceLevel.DecimalPlaces = 1;
            this.numServiceLevel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.numServiceLevel.Location = new System.Drawing.Point(420, 40);
            this.numServiceLevel.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numServiceLevel.Minimum = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.numServiceLevel.Name = "numServiceLevel";
            this.numServiceLevel.Size = new System.Drawing.Size(120, 26);
            this.numServiceLevel.TabIndex = 3;
            this.numServiceLevel.Value = new decimal(new int[] {
            95,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(280, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(132, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "Service Level (%):";
            // 
            // numDaysToProject
            // 
            this.numDaysToProject.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.numDaysToProject.Location = new System.Drawing.Point(150, 40);
            this.numDaysToProject.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.numDaysToProject.Minimum = new decimal(new int[] {
            7,
            0,
            0,
            0});
            this.numDaysToProject.Name = "numDaysToProject";
            this.numDaysToProject.Size = new System.Drawing.Size(100, 26);
            this.numDaysToProject.TabIndex = 1;
            this.numDaysToProject.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(124, 16);
            this.label2.TabIndex = 0;
            this.label2.Text = "Days to Project:";
            // 
            // dgvSuggestions
            // 
            this.dgvSuggestions.AllowUserToAddRows = false;
            this.dgvSuggestions.AllowUserToDeleteRows = false;
            this.dgvSuggestions.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSuggestions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSuggestions.Location = new System.Drawing.Point(20, 200);
            this.dgvSuggestions.MultiSelect = false;
            this.dgvSuggestions.Name = "dgvSuggestions";
            this.dgvSuggestions.ReadOnly = true;
            this.dgvSuggestions.RowHeadersWidth = 51;
            this.dgvSuggestions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSuggestions.Size = new System.Drawing.Size(1040, 380);
            this.dgvSuggestions.TabIndex = 2;
            // 
            // lblResultCount
            // 
            this.lblResultCount.AutoSize = true;
            this.lblResultCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.lblResultCount.ForeColor = System.Drawing.Color.Green;
            this.lblResultCount.Location = new System.Drawing.Point(20, 170);
            this.lblResultCount.Name = "lblResultCount";
            this.lblResultCount.Size = new System.Drawing.Size(160, 18);
            this.lblResultCount.TabIndex = 3;
            this.lblResultCount.Text = "Tìm thấy 0 sản phẩm";
            // 
            // btnExportToExcel
            // 
            this.btnExportToExcel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(125)))), ((int)(((byte)(50)))));
            this.btnExportToExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportToExcel.ForeColor = System.Drawing.Color.White;
            this.btnExportToExcel.Location = new System.Drawing.Point(20, 595);
            this.btnExportToExcel.Name = "btnExportToExcel";
            this.btnExportToExcel.Size = new System.Drawing.Size(150, 35);
            this.btnExportToExcel.TabIndex = 4;
            this.btnExportToExcel.Text = "📊 Export CSV";
            this.btnExportToExcel.UseVisualStyleBackColor = false;
            this.btnExportToExcel.Click += new System.EventHandler(this.btnExportToExcel_Click);
            // 
            // btnViewDetails
            // 
            this.btnViewDetails.Location = new System.Drawing.Point(190, 595);
            this.btnViewDetails.Name = "btnViewDetails";
            this.btnViewDetails.Size = new System.Drawing.Size(150, 35);
            this.btnViewDetails.TabIndex = 5;
            this.btnViewDetails.Text = "🔍 View Details";
            this.btnViewDetails.UseVisualStyleBackColor = true;
            this.btnViewDetails.Click += new System.EventHandler(this.btnViewDetails_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(960, 595);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 35);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Đóng";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label4
            // 
            this.label4.ForeColor = System.Drawing.Color.Gray;
            this.label4.Location = new System.Drawing.Point(500, 165);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(560, 30);
            this.label4.TabIndex = 7;
            this.label4.Text = "🔴 Khẩn cấp  |  🟠 Cao  |  🟡 Trung bình  →  Sản phẩm được sắp xếp theo mức độ ưu tiên";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ReorderSuggestionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1080, 650);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnViewDetails);
            this.Controls.Add(this.btnExportToExcel);
            this.Controls.Add(this.lblResultCount);
            this.Controls.Add(this.dgvSuggestions);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "ReorderSuggestionsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Reorder Suggestions - Đề xuất Nhập hàng Thông minh";
            this.Load += new System.EventHandler(this.ReorderSuggestionsForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numServiceLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDaysToProject)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSuggestions)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown numDaysToProject;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numServiceLevel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.DataGridView dgvSuggestions;
        private System.Windows.Forms.Label lblResultCount;
        private System.Windows.Forms.Button btnExportToExcel;
        private System.Windows.Forms.Button btnViewDetails;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnHelp;
        private System.Windows.Forms.Label label4;
    }
}
