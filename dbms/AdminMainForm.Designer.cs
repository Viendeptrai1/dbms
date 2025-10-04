namespace dbms
{
    partial class AdminMainForm
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabUserManagement = new System.Windows.Forms.TabPage();
            
            // Tab User Management
            this.dgvUsers = new System.Windows.Forms.DataGridView();
            this.dgvRoles = new System.Windows.Forms.DataGridView();
            this.dgvUsersRoles = new System.Windows.Forms.DataGridView();
            this.btnRefreshUsers = new System.Windows.Forms.Button();
            this.btnManageUserRoles = new System.Windows.Forms.Button();
            this.btnToggleUserStatus = new System.Windows.Forms.Button();
            this.btnOpenProductSearch = new System.Windows.Forms.Button();
            this.btnOpenAddProduct = new System.Windows.Forms.Button();
            this.btnOpenPriceAdjust = new System.Windows.Forms.Button();
            this.btnOpenCreateReceipt = new System.Windows.Forms.Button();
            this.btnBatchImport = new System.Windows.Forms.Button();
            this.btnOpenReports = new System.Windows.Forms.Button();
            this.btnUserManagement = new System.Windows.Forms.Button();
            
            // Status and Info
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblConnectionStatus = new System.Windows.Forms.Label();
            this.lblUserInfo = new System.Windows.Forms.Label();
            
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabUserManagement.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRoles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsersRoles)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            
            // menuStrip1
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hệThốngToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1200, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            
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
            this.tabControl1.Controls.Add(this.tabUserManagement);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 24);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1200, 676);
            this.tabControl1.TabIndex = 1;
            
            // tabUserManagement
            this.tabUserManagement.Controls.Add(this.dgvUsers);
            this.tabUserManagement.Controls.Add(this.dgvRoles);
            this.tabUserManagement.Controls.Add(this.dgvUsersRoles);
            this.tabUserManagement.Controls.Add(this.btnRefreshUsers);
            this.tabUserManagement.Controls.Add(this.btnManageUserRoles);
            this.tabUserManagement.Controls.Add(this.btnToggleUserStatus);
            this.tabUserManagement.Controls.Add(this.btnOpenProductSearch);
            this.tabUserManagement.Controls.Add(this.btnOpenAddProduct);
            this.tabUserManagement.Controls.Add(this.btnOpenPriceAdjust);
            this.tabUserManagement.Controls.Add(this.btnOpenCreateReceipt);
            this.tabUserManagement.Controls.Add(this.btnBatchImport);
            this.tabUserManagement.Controls.Add(this.btnOpenReports);
            this.tabUserManagement.Controls.Add(this.btnUserManagement);
            this.tabUserManagement.Location = new System.Drawing.Point(4, 25);
            this.tabUserManagement.Name = "tabUserManagement";
            this.tabUserManagement.Padding = new System.Windows.Forms.Padding(3);
            this.tabUserManagement.Size = new System.Drawing.Size(1192, 647);
            this.tabUserManagement.TabIndex = 0;
            this.tabUserManagement.Text = "Quản lý Người dùng";
            this.tabUserManagement.UseVisualStyleBackColor = true;
            
            // dgvUsers
            this.dgvUsers.AllowUserToAddRows = false;
            this.dgvUsers.AllowUserToDeleteRows = false;
            this.dgvUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUsers.Location = new System.Drawing.Point(6, 6);
            this.dgvUsers.Name = "dgvUsers";
            this.dgvUsers.ReadOnly = true;
            this.dgvUsers.Size = new System.Drawing.Size(580, 400);
            this.dgvUsers.TabIndex = 0;
            
            // dgvRoles
            this.dgvRoles.AllowUserToAddRows = false;
            this.dgvRoles.AllowUserToDeleteRows = false;
            this.dgvRoles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRoles.Location = new System.Drawing.Point(600, 6);
            this.dgvRoles.Name = "dgvRoles";
            this.dgvRoles.ReadOnly = true;
            this.dgvRoles.Size = new System.Drawing.Size(280, 200);
            this.dgvRoles.TabIndex = 1;
            
            // dgvUsersRoles
            this.dgvUsersRoles.AllowUserToAddRows = false;
            this.dgvUsersRoles.AllowUserToDeleteRows = false;
            this.dgvUsersRoles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUsersRoles.Location = new System.Drawing.Point(600, 220);
            this.dgvUsersRoles.Name = "dgvUsersRoles";
            this.dgvUsersRoles.ReadOnly = true;
            this.dgvUsersRoles.Size = new System.Drawing.Size(280, 186);
            this.dgvUsersRoles.TabIndex = 2;
            
            // btnRefreshUsers
            this.btnRefreshUsers.Location = new System.Drawing.Point(6, 420);
            this.btnRefreshUsers.Name = "btnRefreshUsers";
            this.btnRefreshUsers.Size = new System.Drawing.Size(120, 30);
            this.btnRefreshUsers.TabIndex = 3;
            this.btnRefreshUsers.Text = "Làm mới";
            this.btnRefreshUsers.UseVisualStyleBackColor = true;
            this.btnRefreshUsers.Click += new System.EventHandler(this.btnRefreshUsers_Click);
            
            // btnManageUserRoles
            this.btnManageUserRoles.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(102)))), ((int)(((byte)(204)))));
            this.btnManageUserRoles.FlatAppearance.BorderSize = 0;
            this.btnManageUserRoles.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnManageUserRoles.ForeColor = System.Drawing.Color.White;
            this.btnManageUserRoles.Location = new System.Drawing.Point(140, 420);
            this.btnManageUserRoles.Name = "btnManageUserRoles";
            this.btnManageUserRoles.Size = new System.Drawing.Size(120, 30);
            this.btnManageUserRoles.TabIndex = 4;
            this.btnManageUserRoles.Text = "Quản lý vai trò";
            this.btnManageUserRoles.UseVisualStyleBackColor = false;
            this.btnManageUserRoles.Click += new System.EventHandler(this.btnManageUserRoles_Click);
            
            // btnToggleUserStatus
            this.btnToggleUserStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnToggleUserStatus.FlatAppearance.BorderSize = 0;
            this.btnToggleUserStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToggleUserStatus.ForeColor = System.Drawing.Color.White;
            this.btnToggleUserStatus.Location = new System.Drawing.Point(274, 420);
            this.btnToggleUserStatus.Name = "btnToggleUserStatus";
            this.btnToggleUserStatus.Size = new System.Drawing.Size(120, 30);
            this.btnToggleUserStatus.TabIndex = 5;
            this.btnToggleUserStatus.Text = "Khóa/Mở khóa";
            this.btnToggleUserStatus.UseVisualStyleBackColor = false;
            this.btnToggleUserStatus.Click += new System.EventHandler(this.btnToggleUserStatus_Click);
            
            // btnOpenProductSearch
            this.btnOpenProductSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(162)))), ((int)(((byte)(184)))));
            this.btnOpenProductSearch.FlatAppearance.BorderSize = 0;
            this.btnOpenProductSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenProductSearch.ForeColor = System.Drawing.Color.White;
            this.btnOpenProductSearch.Location = new System.Drawing.Point(420, 420);
            this.btnOpenProductSearch.Name = "btnOpenProductSearch";
            this.btnOpenProductSearch.Size = new System.Drawing.Size(120, 30);
            this.btnOpenProductSearch.TabIndex = 6;
            this.btnOpenProductSearch.Text = "Tra cứu SP";
            this.btnOpenProductSearch.UseVisualStyleBackColor = false;
            this.btnOpenProductSearch.Click += new System.EventHandler(this.btnOpenProductSearch_Click);
            
            // btnOpenAddProduct
            this.btnOpenAddProduct.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnOpenAddProduct.FlatAppearance.BorderSize = 0;
            this.btnOpenAddProduct.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenAddProduct.ForeColor = System.Drawing.Color.White;
            this.btnOpenAddProduct.Location = new System.Drawing.Point(550, 420);
            this.btnOpenAddProduct.Name = "btnOpenAddProduct";
            this.btnOpenAddProduct.Size = new System.Drawing.Size(120, 30);
            this.btnOpenAddProduct.TabIndex = 7;
            this.btnOpenAddProduct.Text = "Thêm SP";
            this.btnOpenAddProduct.UseVisualStyleBackColor = false;
            this.btnOpenAddProduct.Click += new System.EventHandler(this.btnOpenAddProduct_Click);
            
            // btnOpenPriceAdjust
            this.btnOpenPriceAdjust.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(7)))));
            this.btnOpenPriceAdjust.FlatAppearance.BorderSize = 0;
            this.btnOpenPriceAdjust.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenPriceAdjust.ForeColor = System.Drawing.Color.White;
            this.btnOpenPriceAdjust.Location = new System.Drawing.Point(680, 420);
            this.btnOpenPriceAdjust.Name = "btnOpenPriceAdjust";
            this.btnOpenPriceAdjust.Size = new System.Drawing.Size(120, 30);
            this.btnOpenPriceAdjust.TabIndex = 8;
            this.btnOpenPriceAdjust.Text = "Điều chỉnh giá";
            this.btnOpenPriceAdjust.UseVisualStyleBackColor = false;
            this.btnOpenPriceAdjust.Click += new System.EventHandler(this.btnOpenPriceAdjust_Click);
            
            // btnOpenCreateReceipt
            this.btnOpenCreateReceipt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(102)))), ((int)(((byte)(204)))));
            this.btnOpenCreateReceipt.FlatAppearance.BorderSize = 0;
            this.btnOpenCreateReceipt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenCreateReceipt.ForeColor = System.Drawing.Color.White;
            this.btnOpenCreateReceipt.Location = new System.Drawing.Point(420, 460);
            this.btnOpenCreateReceipt.Name = "btnOpenCreateReceipt";
            this.btnOpenCreateReceipt.Size = new System.Drawing.Size(120, 30);
            this.btnOpenCreateReceipt.TabIndex = 9;
            this.btnOpenCreateReceipt.Text = "Tạo phiếu nhập";
            this.btnOpenCreateReceipt.UseVisualStyleBackColor = false;
            this.btnOpenCreateReceipt.Click += new System.EventHandler(this.btnOpenCreateReceipt_Click);
            
            // btnBatchImport
            this.btnBatchImport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnBatchImport.FlatAppearance.BorderSize = 0;
            this.btnBatchImport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBatchImport.ForeColor = System.Drawing.Color.White;
            this.btnBatchImport.Location = new System.Drawing.Point(420, 500);
            this.btnBatchImport.Name = "btnBatchImport";
            this.btnBatchImport.Size = new System.Drawing.Size(120, 30);
            this.btnBatchImport.TabIndex = 12;
            this.btnBatchImport.Text = "Batch Import";
            this.btnBatchImport.UseVisualStyleBackColor = false;
            this.btnBatchImport.Click += new System.EventHandler(this.btnBatchImport_Click);
            
            // btnOpenReports
            this.btnOpenReports.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(66)))), ((int)(((byte)(193)))));
            this.btnOpenReports.FlatAppearance.BorderSize = 0;
            this.btnOpenReports.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenReports.ForeColor = System.Drawing.Color.White;
            this.btnOpenReports.Location = new System.Drawing.Point(550, 460);
            this.btnOpenReports.Name = "btnOpenReports";
            this.btnOpenReports.Size = new System.Drawing.Size(120, 30);
            this.btnOpenReports.TabIndex = 10;
            this.btnOpenReports.Text = "Báo cáo";
            this.btnOpenReports.UseVisualStyleBackColor = false;
            this.btnOpenReports.Click += new System.EventHandler(this.btnOpenReports_Click);
            
            // btnUserManagement
            this.btnUserManagement.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))));
            this.btnUserManagement.FlatAppearance.BorderSize = 0;
            this.btnUserManagement.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUserManagement.ForeColor = System.Drawing.Color.White;
            this.btnUserManagement.Location = new System.Drawing.Point(680, 460);
            this.btnUserManagement.Name = "btnUserManagement";
            this.btnUserManagement.Size = new System.Drawing.Size(120, 30);
            this.btnUserManagement.TabIndex = 11;
            this.btnUserManagement.Text = "Quản lý User";
            this.btnUserManagement.UseVisualStyleBackColor = false;
            this.btnUserManagement.Click += new System.EventHandler(this.btnUserManagement_Click);
            
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
            
            // AdminMainForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 700);
            this.Controls.Add(this.lblUserInfo);
            this.Controls.Add(this.lblConnectionStatus);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "AdminMainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hệ thống Quản lý Nhập hàng - Admin";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabUserManagement.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRoles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsersRoles)).EndInit();
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
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabUserManagement;
        
        // Tab User Management
        private System.Windows.Forms.DataGridView dgvUsers;
        private System.Windows.Forms.DataGridView dgvRoles;
        private System.Windows.Forms.DataGridView dgvUsersRoles;
        private System.Windows.Forms.Button btnRefreshUsers;
        private System.Windows.Forms.Button btnManageUserRoles;
        private System.Windows.Forms.Button btnToggleUserStatus;
        private System.Windows.Forms.Button btnOpenProductSearch;
        private System.Windows.Forms.Button btnOpenAddProduct;
        private System.Windows.Forms.Button btnOpenPriceAdjust;
        private System.Windows.Forms.Button btnOpenCreateReceipt;
        private System.Windows.Forms.Button btnBatchImport;
        private System.Windows.Forms.Button btnOpenReports;
        private System.Windows.Forms.Button btnUserManagement;
        
        // Status and Info
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Label lblConnectionStatus;
        private System.Windows.Forms.Label lblUserInfo;
    }
}
