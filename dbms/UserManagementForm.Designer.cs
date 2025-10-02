namespace dbms
{
    partial class UserManagementForm
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
            this.tabCreateUser = new System.Windows.Forms.TabPage();
            this.tabManageUsers = new System.Windows.Forms.TabPage();
            this.tabUserRoles = new System.Windows.Forms.TabPage();
            
            // Tab Create User
            this.lblUsername = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblFullName = new System.Windows.Forms.Label();
            this.txtFullName = new System.Windows.Forms.TextBox();
            this.lblRole = new System.Windows.Forms.Label();
            this.cmbRoles = new System.Windows.Forms.ComboBox();
            this.btnCreateUser = new System.Windows.Forms.Button();
            this.lblCreateUserInfo = new System.Windows.Forms.Label();
            
            // Tab Manage Users
            this.dgvUsers = new System.Windows.Forms.DataGridView();
            this.lblNewRole = new System.Windows.Forms.Label();
            this.cmbNewRole = new System.Windows.Forms.ComboBox();
            this.btnChangeRole = new System.Windows.Forms.Button();
            this.btnToggleStatus = new System.Windows.Forms.Button();
            this.btnRevokeAll = new System.Windows.Forms.Button();
            this.btnDeleteUser = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            
            // Tab User Roles
            this.dgvUserRoles = new System.Windows.Forms.DataGridView();
            this.lblUserRolesInfo = new System.Windows.Forms.Label();
            
            // Common controls
            this.btnClose = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            
            this.tabControl1.SuspendLayout();
            this.tabCreateUser.SuspendLayout();
            this.tabManageUsers.SuspendLayout();
            this.tabUserRoles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUserRoles)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            
            // tabControl1
            this.tabControl1.Controls.Add(this.tabCreateUser);
            this.tabControl1.Controls.Add(this.tabManageUsers);
            this.tabControl1.Controls.Add(this.tabUserRoles);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1000, 600);
            this.tabControl1.TabIndex = 0;
            
            // tabCreateUser
            this.tabCreateUser.Controls.Add(this.lblCreateUserInfo);
            this.tabCreateUser.Controls.Add(this.btnCreateUser);
            this.tabCreateUser.Controls.Add(this.cmbRoles);
            this.tabCreateUser.Controls.Add(this.lblRole);
            this.tabCreateUser.Controls.Add(this.txtFullName);
            this.tabCreateUser.Controls.Add(this.lblFullName);
            this.tabCreateUser.Controls.Add(this.txtPassword);
            this.tabCreateUser.Controls.Add(this.lblPassword);
            this.tabCreateUser.Controls.Add(this.txtUsername);
            this.tabCreateUser.Controls.Add(this.lblUsername);
            this.tabCreateUser.Location = new System.Drawing.Point(4, 25);
            this.tabCreateUser.Name = "tabCreateUser";
            this.tabCreateUser.Padding = new System.Windows.Forms.Padding(3);
            this.tabCreateUser.Size = new System.Drawing.Size(992, 571);
            this.tabCreateUser.TabIndex = 0;
            this.tabCreateUser.Text = "Tạo User Mới";
            this.tabCreateUser.UseVisualStyleBackColor = true;
            
            // lblCreateUserInfo
            this.lblCreateUserInfo.AutoSize = true;
            this.lblCreateUserInfo.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblCreateUserInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.lblCreateUserInfo.Location = new System.Drawing.Point(20, 20);
            this.lblCreateUserInfo.Name = "lblCreateUserInfo";
            this.lblCreateUserInfo.Size = new System.Drawing.Size(200, 21);
            this.lblCreateUserInfo.TabIndex = 0;
            this.lblCreateUserInfo.Text = "Tạo tài khoản người dùng mới";
            
            // lblUsername
            this.lblUsername.AutoSize = true;
            this.lblUsername.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.lblUsername.Location = new System.Drawing.Point(50, 80);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(100, 20);
            this.lblUsername.TabIndex = 1;
            this.lblUsername.Text = "Username:";
            
            // txtUsername
            this.txtUsername.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.txtUsername.Location = new System.Drawing.Point(200, 77);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(250, 25);
            this.txtUsername.TabIndex = 2;
            
            // lblPassword
            this.lblPassword.AutoSize = true;
            this.lblPassword.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.lblPassword.Location = new System.Drawing.Point(50, 130);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(100, 20);
            this.lblPassword.TabIndex = 3;
            this.lblPassword.Text = "Password:";
            
            // txtPassword
            this.txtPassword.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.txtPassword.Location = new System.Drawing.Point(200, 127);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(250, 25);
            this.txtPassword.TabIndex = 4;
            
            // lblFullName
            this.lblFullName.AutoSize = true;
            this.lblFullName.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.lblFullName.Location = new System.Drawing.Point(50, 180);
            this.lblFullName.Name = "lblFullName";
            this.lblFullName.Size = new System.Drawing.Size(100, 20);
            this.lblFullName.TabIndex = 5;
            this.lblFullName.Text = "Họ tên:";
            
            // txtFullName
            this.txtFullName.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.txtFullName.Location = new System.Drawing.Point(200, 177);
            this.txtFullName.Name = "txtFullName";
            this.txtFullName.Size = new System.Drawing.Size(250, 25);
            this.txtFullName.TabIndex = 6;
            
            // lblRole
            this.lblRole.AutoSize = true;
            this.lblRole.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.lblRole.Location = new System.Drawing.Point(50, 230);
            this.lblRole.Name = "lblRole";
            this.lblRole.Size = new System.Drawing.Size(100, 20);
            this.lblRole.TabIndex = 7;
            this.lblRole.Text = "Role:";
            
            // cmbRoles
            this.cmbRoles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRoles.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.cmbRoles.FormattingEnabled = true;
            this.cmbRoles.Location = new System.Drawing.Point(200, 227);
            this.cmbRoles.Name = "cmbRoles";
            this.cmbRoles.Size = new System.Drawing.Size(250, 27);
            this.cmbRoles.TabIndex = 8;
            
            // btnCreateUser
            this.btnCreateUser.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnCreateUser.FlatAppearance.BorderSize = 0;
            this.btnCreateUser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCreateUser.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.btnCreateUser.ForeColor = System.Drawing.Color.White;
            this.btnCreateUser.Location = new System.Drawing.Point(200, 280);
            this.btnCreateUser.Name = "btnCreateUser";
            this.btnCreateUser.Size = new System.Drawing.Size(120, 35);
            this.btnCreateUser.TabIndex = 9;
            this.btnCreateUser.Text = "Tạo User";
            this.btnCreateUser.UseVisualStyleBackColor = false;
            this.btnCreateUser.Click += new System.EventHandler(this.btnCreateUser_Click);
            
            // tabManageUsers
            this.tabManageUsers.Controls.Add(this.btnRefresh);
            this.tabManageUsers.Controls.Add(this.btnDeleteUser);
            this.tabManageUsers.Controls.Add(this.btnRevokeAll);
            this.tabManageUsers.Controls.Add(this.btnToggleStatus);
            this.tabManageUsers.Controls.Add(this.btnChangeRole);
            this.tabManageUsers.Controls.Add(this.cmbNewRole);
            this.tabManageUsers.Controls.Add(this.lblNewRole);
            this.tabManageUsers.Controls.Add(this.dgvUsers);
            this.tabManageUsers.Location = new System.Drawing.Point(4, 25);
            this.tabManageUsers.Name = "tabManageUsers";
            this.tabManageUsers.Padding = new System.Windows.Forms.Padding(3);
            this.tabManageUsers.Size = new System.Drawing.Size(992, 571);
            this.tabManageUsers.TabIndex = 1;
            this.tabManageUsers.Text = "Quản lý Users";
            this.tabManageUsers.UseVisualStyleBackColor = true;
            
            // dgvUsers
            this.dgvUsers.AllowUserToAddRows = false;
            this.dgvUsers.AllowUserToDeleteRows = false;
            this.dgvUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUsers.Location = new System.Drawing.Point(20, 20);
            this.dgvUsers.Name = "dgvUsers";
            this.dgvUsers.ReadOnly = true;
            this.dgvUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUsers.Size = new System.Drawing.Size(950, 350);
            this.dgvUsers.TabIndex = 0;
            
            // lblNewRole
            this.lblNewRole.AutoSize = true;
            this.lblNewRole.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.lblNewRole.Location = new System.Drawing.Point(20, 400);
            this.lblNewRole.Name = "lblNewRole";
            this.lblNewRole.Size = new System.Drawing.Size(100, 20);
            this.lblNewRole.TabIndex = 1;
            this.lblNewRole.Text = "Role mới:";
            
            // cmbNewRole
            this.cmbNewRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNewRole.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.cmbNewRole.FormattingEnabled = true;
            this.cmbNewRole.Location = new System.Drawing.Point(130, 397);
            this.cmbNewRole.Name = "cmbNewRole";
            this.cmbNewRole.Size = new System.Drawing.Size(150, 27);
            this.cmbNewRole.TabIndex = 2;
            
            // btnChangeRole
            this.btnChangeRole.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(102)))), ((int)(((byte)(204)))));
            this.btnChangeRole.FlatAppearance.BorderSize = 0;
            this.btnChangeRole.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChangeRole.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.btnChangeRole.ForeColor = System.Drawing.Color.White;
            this.btnChangeRole.Location = new System.Drawing.Point(300, 395);
            this.btnChangeRole.Name = "btnChangeRole";
            this.btnChangeRole.Size = new System.Drawing.Size(120, 30);
            this.btnChangeRole.TabIndex = 3;
            this.btnChangeRole.Text = "Đổi Role";
            this.btnChangeRole.UseVisualStyleBackColor = false;
            this.btnChangeRole.Click += new System.EventHandler(this.btnChangeRole_Click);
            
            // btnToggleStatus
            this.btnToggleStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(7)))));
            this.btnToggleStatus.FlatAppearance.BorderSize = 0;
            this.btnToggleStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToggleStatus.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.btnToggleStatus.ForeColor = System.Drawing.Color.White;
            this.btnToggleStatus.Location = new System.Drawing.Point(440, 395);
            this.btnToggleStatus.Name = "btnToggleStatus";
            this.btnToggleStatus.Size = new System.Drawing.Size(120, 30);
            this.btnToggleStatus.TabIndex = 4;
            this.btnToggleStatus.Text = "Khóa/Mở";
            this.btnToggleStatus.UseVisualStyleBackColor = false;
            this.btnToggleStatus.Click += new System.EventHandler(this.btnToggleStatus_Click);
            
            // btnRevokeAll
            this.btnRevokeAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnRevokeAll.FlatAppearance.BorderSize = 0;
            this.btnRevokeAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRevokeAll.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.btnRevokeAll.ForeColor = System.Drawing.Color.White;
            this.btnRevokeAll.Location = new System.Drawing.Point(580, 395);
            this.btnRevokeAll.Name = "btnRevokeAll";
            this.btnRevokeAll.Size = new System.Drawing.Size(120, 30);
            this.btnRevokeAll.TabIndex = 5;
            this.btnRevokeAll.Text = "Revoke All";
            this.btnRevokeAll.UseVisualStyleBackColor = false;
            this.btnRevokeAll.Click += new System.EventHandler(this.btnRevokeAll_Click);
            
            // btnDeleteUser
            this.btnDeleteUser.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnDeleteUser.FlatAppearance.BorderSize = 0;
            this.btnDeleteUser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteUser.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.btnDeleteUser.ForeColor = System.Drawing.Color.White;
            this.btnDeleteUser.Location = new System.Drawing.Point(720, 395);
            this.btnDeleteUser.Name = "btnDeleteUser";
            this.btnDeleteUser.Size = new System.Drawing.Size(120, 30);
            this.btnDeleteUser.TabIndex = 6;
            this.btnDeleteUser.Text = "Xóa User";
            this.btnDeleteUser.UseVisualStyleBackColor = false;
            this.btnDeleteUser.Click += new System.EventHandler(this.btnDeleteUser_Click);
            
            // btnRefresh
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(162)))), ((int)(((byte)(184)))));
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(20, 450);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 30);
            this.btnRefresh.TabIndex = 7;
            this.btnRefresh.Text = "Làm mới";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            
            // tabUserRoles
            this.tabUserRoles.Controls.Add(this.lblUserRolesInfo);
            this.tabUserRoles.Controls.Add(this.dgvUserRoles);
            this.tabUserRoles.Location = new System.Drawing.Point(4, 25);
            this.tabUserRoles.Name = "tabUserRoles";
            this.tabUserRoles.Padding = new System.Windows.Forms.Padding(3);
            this.tabUserRoles.Size = new System.Drawing.Size(992, 571);
            this.tabUserRoles.TabIndex = 2;
            this.tabUserRoles.Text = "User Roles";
            this.tabUserRoles.UseVisualStyleBackColor = true;
            
            // lblUserRolesInfo
            this.lblUserRolesInfo.AutoSize = true;
            this.lblUserRolesInfo.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblUserRolesInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.lblUserRolesInfo.Location = new System.Drawing.Point(20, 20);
            this.lblUserRolesInfo.Name = "lblUserRolesInfo";
            this.lblUserRolesInfo.Size = new System.Drawing.Size(250, 21);
            this.lblUserRolesInfo.TabIndex = 1;
            this.lblUserRolesInfo.Text = "Danh sách User và Role assignments";
            
            // dgvUserRoles
            this.dgvUserRoles.AllowUserToAddRows = false;
            this.dgvUserRoles.AllowUserToDeleteRows = false;
            this.dgvUserRoles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUserRoles.Location = new System.Drawing.Point(20, 60);
            this.dgvUserRoles.Name = "dgvUserRoles";
            this.dgvUserRoles.ReadOnly = true;
            this.dgvUserRoles.Size = new System.Drawing.Size(950, 480);
            this.dgvUserRoles.TabIndex = 0;
            
            // btnClose
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(900, 610);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 35);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Đóng";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            
            // statusStrip1
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 655);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1000, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            
            // toolStripStatusLabel1
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(983, 17);
            this.toolStripStatusLabel1.Spring = true;
            this.toolStripStatusLabel1.Text = "Sẵn sàng";
            this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            
            // UserManagementForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1000, 677);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UserManagementForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Quản lý User và Role";
            
            this.tabControl1.ResumeLayout(false);
            this.tabCreateUser.ResumeLayout(false);
            this.tabCreateUser.PerformLayout();
            this.tabManageUsers.ResumeLayout(false);
            this.tabManageUsers.PerformLayout();
            this.tabUserRoles.ResumeLayout(false);
            this.tabUserRoles.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUserRoles)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabCreateUser;
        private System.Windows.Forms.TabPage tabManageUsers;
        private System.Windows.Forms.TabPage tabUserRoles;
        
        // Tab Create User
        private System.Windows.Forms.Label lblCreateUserInfo;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblFullName;
        private System.Windows.Forms.TextBox txtFullName;
        private System.Windows.Forms.Label lblRole;
        private System.Windows.Forms.ComboBox cmbRoles;
        private System.Windows.Forms.Button btnCreateUser;
        
        // Tab Manage Users
        private System.Windows.Forms.DataGridView dgvUsers;
        private System.Windows.Forms.Label lblNewRole;
        private System.Windows.Forms.ComboBox cmbNewRole;
        private System.Windows.Forms.Button btnChangeRole;
        private System.Windows.Forms.Button btnToggleStatus;
        private System.Windows.Forms.Button btnRevokeAll;
        private System.Windows.Forms.Button btnDeleteUser;
        private System.Windows.Forms.Button btnRefresh;
        
        // Tab User Roles
        private System.Windows.Forms.DataGridView dgvUserRoles;
        private System.Windows.Forms.Label lblUserRolesInfo;
        
        // Common controls
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    }
}
