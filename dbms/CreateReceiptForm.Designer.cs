namespace dbms
{
    partial class CreateReceiptForm
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblUserInfo = new System.Windows.Forms.Label();
            this.lblSupplier = new System.Windows.Forms.Label();
            this.lblReceiptDate = new System.Windows.Forms.Label();
            this.cmbSupplier = new System.Windows.Forms.ComboBox();
            this.dtpReceiptDate = new System.Windows.Forms.DateTimePicker();
            this.grpProductInfo = new System.Windows.Forms.GroupBox();
            this.lblSKU = new System.Windows.Forms.Label();
            this.lblQuantity = new System.Windows.Forms.Label();
            this.lblImportPrice = new System.Windows.Forms.Label();
            this.txtSKU = new System.Windows.Forms.TextBox();
            this.txtQuantity = new System.Windows.Forms.TextBox();
            this.txtImportPrice = new System.Windows.Forms.TextBox();
            this.btnAddLine = new System.Windows.Forms.Button();
            this.btnRemoveLine = new System.Windows.Forms.Button();
            this.lstLines = new System.Windows.Forms.ListBox();
            this.lblTotalAmount = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.grpProductInfo.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            
            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(102)))), ((int)(((byte)(204)))));
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(180, 26);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Tạo Phiếu Nhập";
            
            // lblUserInfo
            this.lblUserInfo.AutoSize = true;
            this.lblUserInfo.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblUserInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.lblUserInfo.Location = new System.Drawing.Point(20, 60);
            this.lblUserInfo.Name = "lblUserInfo";
            this.lblUserInfo.Size = new System.Drawing.Size(110, 20);
            this.lblUserInfo.TabIndex = 1;
            this.lblUserInfo.Text = "Nhân viên: User";
            
            // lblSupplier
            this.lblSupplier.AutoSize = true;
            this.lblSupplier.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblSupplier.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.lblSupplier.Location = new System.Drawing.Point(20, 100);
            this.lblSupplier.Name = "lblSupplier";
            this.lblSupplier.Size = new System.Drawing.Size(120, 20);
            this.lblSupplier.TabIndex = 2;
            this.lblSupplier.Text = "Nhà cung cấp:";
            
            // lblReceiptDate
            this.lblReceiptDate.AutoSize = true;
            this.lblReceiptDate.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblReceiptDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.lblReceiptDate.Location = new System.Drawing.Point(300, 100);
            this.lblReceiptDate.Name = "lblReceiptDate";
            this.lblReceiptDate.Size = new System.Drawing.Size(80, 20);
            this.lblReceiptDate.TabIndex = 3;
            this.lblReceiptDate.Text = "Ngày nhập:";
            
            // cmbSupplier
            this.cmbSupplier.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSupplier.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.cmbSupplier.FormattingEnabled = true;
            this.cmbSupplier.Location = new System.Drawing.Point(150, 97);
            this.cmbSupplier.Name = "cmbSupplier";
            this.cmbSupplier.Size = new System.Drawing.Size(140, 27);
            this.cmbSupplier.TabIndex = 4;
            
            // dtpReceiptDate
            this.dtpReceiptDate.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.dtpReceiptDate.Location = new System.Drawing.Point(390, 97);
            this.dtpReceiptDate.Name = "dtpReceiptDate";
            this.dtpReceiptDate.Size = new System.Drawing.Size(150, 25);
            this.dtpReceiptDate.TabIndex = 5;
            
            // grpProductInfo
            this.grpProductInfo.Controls.Add(this.lblSKU);
            this.grpProductInfo.Controls.Add(this.lblQuantity);
            this.grpProductInfo.Controls.Add(this.lblImportPrice);
            this.grpProductInfo.Controls.Add(this.txtSKU);
            this.grpProductInfo.Controls.Add(this.txtQuantity);
            this.grpProductInfo.Controls.Add(this.txtImportPrice);
            this.grpProductInfo.Controls.Add(this.btnAddLine);
            this.grpProductInfo.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.grpProductInfo.Location = new System.Drawing.Point(20, 140);
            this.grpProductInfo.Name = "grpProductInfo";
            this.grpProductInfo.Size = new System.Drawing.Size(510, 180);
            this.grpProductInfo.TabIndex = 6;
            this.grpProductInfo.TabStop = false;
            this.grpProductInfo.Text = "Thông tin sản phẩm";
            
            // lblSKU
            this.lblSKU.AutoSize = true;
            this.lblSKU.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblSKU.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.lblSKU.Location = new System.Drawing.Point(20, 30);
            this.lblSKU.Name = "lblSKU";
            this.lblSKU.Size = new System.Drawing.Size(50, 20);
            this.lblSKU.TabIndex = 0;
            this.lblSKU.Text = "SKU:";
            
            // lblQuantity
            this.lblQuantity.AutoSize = true;
            this.lblQuantity.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblQuantity.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.lblQuantity.Location = new System.Drawing.Point(20, 70);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Size = new System.Drawing.Size(80, 20);
            this.lblQuantity.TabIndex = 1;
            this.lblQuantity.Text = "Số lượng:";
            
            // lblImportPrice
            this.lblImportPrice.AutoSize = true;
            this.lblImportPrice.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblImportPrice.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.lblImportPrice.Location = new System.Drawing.Point(270, 30);
            this.lblImportPrice.Name = "lblImportPrice";
            this.lblImportPrice.Size = new System.Drawing.Size(90, 20);
            this.lblImportPrice.TabIndex = 2;
            this.lblImportPrice.Text = "Giá nhập:";
            
            // txtSKU
            this.txtSKU.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.txtSKU.Location = new System.Drawing.Point(80, 27);
            this.txtSKU.Name = "txtSKU";
            this.txtSKU.Size = new System.Drawing.Size(150, 25);
            this.txtSKU.TabIndex = 4;
            
            // txtQuantity
            this.txtQuantity.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.txtQuantity.Location = new System.Drawing.Point(110, 67);
            this.txtQuantity.Name = "txtQuantity";
            this.txtQuantity.Size = new System.Drawing.Size(100, 25);
            this.txtQuantity.TabIndex = 5;
            
            // txtImportPrice
            this.txtImportPrice.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.txtImportPrice.Location = new System.Drawing.Point(370, 27);
            this.txtImportPrice.Name = "txtImportPrice";
            this.txtImportPrice.Size = new System.Drawing.Size(120, 25);
            this.txtImportPrice.TabIndex = 6;
            
            // btnAddLine
            this.btnAddLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(102)))), ((int)(((byte)(204)))));
            this.btnAddLine.FlatAppearance.BorderSize = 0;
            this.btnAddLine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddLine.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.btnAddLine.ForeColor = System.Drawing.Color.White;
            this.btnAddLine.Location = new System.Drawing.Point(20, 120);
            this.btnAddLine.Name = "btnAddLine";
            this.btnAddLine.Size = new System.Drawing.Size(120, 40);
            this.btnAddLine.TabIndex = 8;
            this.btnAddLine.Text = "➕ Thêm dòng";
            this.btnAddLine.UseVisualStyleBackColor = false;
            this.btnAddLine.Click += new System.EventHandler(this.btnAddLine_Click);
            
            // btnRemoveLine
            this.btnRemoveLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnRemoveLine.FlatAppearance.BorderSize = 0;
            this.btnRemoveLine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveLine.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.btnRemoveLine.ForeColor = System.Drawing.Color.White;
            this.btnRemoveLine.Location = new System.Drawing.Point(20, 340);
            this.btnRemoveLine.Name = "btnRemoveLine";
            this.btnRemoveLine.Size = new System.Drawing.Size(120, 40);
            this.btnRemoveLine.TabIndex = 7;
            this.btnRemoveLine.Text = "➖ Xóa dòng";
            this.btnRemoveLine.UseVisualStyleBackColor = false;
            this.btnRemoveLine.Click += new System.EventHandler(this.btnRemoveLine_Click);
            
            // lstLines
            this.lstLines.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.lstLines.FormattingEnabled = true;
            this.lstLines.ItemHeight = 17;
            this.lstLines.Location = new System.Drawing.Point(140, 340);
            this.lstLines.Name = "lstLines";
            this.lstLines.Size = new System.Drawing.Size(390, 140);
            this.lstLines.TabIndex = 8;
            
            // lblTotalAmount
            this.lblTotalAmount.AutoSize = true;
            this.lblTotalAmount.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTotalAmount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.lblTotalAmount.Location = new System.Drawing.Point(20, 500);
            this.lblTotalAmount.Name = "lblTotalAmount";
            this.lblTotalAmount.Size = new System.Drawing.Size(120, 22);
            this.lblTotalAmount.TabIndex = 9;
            this.lblTotalAmount.Text = "Tổng tiền: 0 VNĐ";
            
            // btnSave
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(350, 530);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(130, 45);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "💾 Lưu phiếu";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            
            // btnCancel
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(500, 530);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(130, 45);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "❌ Hủy";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            
            // panel1
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Padding = new System.Windows.Forms.Padding(15);
            this.panel1.Controls.Add(this.lblTitle);
            this.panel1.Controls.Add(this.lblUserInfo);
            this.panel1.Controls.Add(this.lblSupplier);
            this.panel1.Controls.Add(this.lblReceiptDate);
            this.panel1.Controls.Add(this.cmbSupplier);
            this.panel1.Controls.Add(this.dtpReceiptDate);
            this.panel1.Controls.Add(this.grpProductInfo);
            this.panel1.Controls.Add(this.btnRemoveLine);
            this.panel1.Controls.Add(this.lstLines);
            this.panel1.Controls.Add(this.lblTotalAmount);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(650, 590);
            this.panel1.TabIndex = 12;
            
            // CreateReceiptForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(674, 614);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CreateReceiptForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Tạo Phiếu Nhập Hàng";
            
            this.grpProductInfo.ResumeLayout(false);
            this.grpProductInfo.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblUserInfo;
        private System.Windows.Forms.Label lblSupplier;
        private System.Windows.Forms.Label lblReceiptDate;
        private System.Windows.Forms.ComboBox cmbSupplier;
        private System.Windows.Forms.DateTimePicker dtpReceiptDate;
        private System.Windows.Forms.GroupBox grpProductInfo;
        private System.Windows.Forms.Label lblSKU;
        private System.Windows.Forms.Label lblQuantity;
        private System.Windows.Forms.Label lblImportPrice;
        private System.Windows.Forms.TextBox txtSKU;
        private System.Windows.Forms.TextBox txtQuantity;
        private System.Windows.Forms.TextBox txtImportPrice;
        private System.Windows.Forms.Button btnAddLine;
        private System.Windows.Forms.Button btnRemoveLine;
        private System.Windows.Forms.ListBox lstLines;
        private System.Windows.Forms.Label lblTotalAmount;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel panel1;
    }
}
