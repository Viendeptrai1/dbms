
/* ============================
   CREATE DATABASE
============================ */
CREATE DATABASE QLNhapHang;
GO
USE QLNhapHang;
GO

/* ============================
   ROLES & USERS (M-N: UsersRoles)
============================ */
CREATE TABLE dbo.Roles (
    RoleID   INT IDENTITY(1,1) PRIMARY KEY,
    RoleName NVARCHAR(50) NOT NULL UNIQUE -- ví dụ: Admin, Seller, User
);

CREATE TABLE dbo.Users (
    UserID      INT IDENTITY(1,1) PRIMARY KEY,
    Username    VARCHAR(50)  NOT NULL UNIQUE,
    PasswordHash VARBINARY(32) NOT NULL,        -- HASHBYTES('SHA2_256', '...')
    FullName    NVARCHAR(100) NOT NULL,
    IsActive    BIT           NOT NULL CONSTRAINT DF_Users_IsActive DEFAULT 1,
    CreatedAt   DATETIME2(3)  NOT NULL CONSTRAINT DF_Users_CreatedAt DEFAULT SYSDATETIME()
);

-- Bảng trung gian M-N giữa Users và Roles
CREATE TABLE dbo.UsersRoles (
    UserID INT NOT NULL,
    RoleID INT NOT NULL,
    CONSTRAINT PK_UsersRoles PRIMARY KEY (UserID, RoleID),
    CONSTRAINT FK_UsersRoles_User  FOREIGN KEY (UserID) REFERENCES dbo.Users(UserID) ON DELETE CASCADE,
    CONSTRAINT FK_UsersRoles_Role  FOREIGN KEY (RoleID) REFERENCES dbo.Roles(RoleID) ON DELETE CASCADE
);

/* ============================
   CATEGORIES & PRODUCTS (M-N: ProductCategories)
============================ */
CREATE TABLE dbo.Categories (
    CategoryID   INT IDENTITY(1,1) PRIMARY KEY,
    CategoryName NVARCHAR(100) NOT NULL UNIQUE
);

CREATE TABLE dbo.Products (
    ProductID     INT IDENTITY(1,1) PRIMARY KEY,
    SKU           VARCHAR(32)    NOT NULL UNIQUE,
    ProductName   NVARCHAR(200)  NOT NULL,
    SellingPrice  DECIMAL(10,2)  NOT NULL CONSTRAINT DF_Products_SellingPrice DEFAULT 0,
    StockQuantity INT            NOT NULL CONSTRAINT DF_Products_Stock DEFAULT 0,
    CONSTRAINT CK_Products_Stock_NonNegative CHECK (StockQuantity >= 0),
    CONSTRAINT CK_Products_Price_NonNegative CHECK (SellingPrice  >= 0)
);

-- Bảng trung gian M-N giữa Products và Categories
CREATE TABLE dbo.ProductCategories (
    ProductID  INT NOT NULL,
    CategoryID INT NOT NULL,
    CONSTRAINT PK_ProductCategories PRIMARY KEY (ProductID, CategoryID),
    CONSTRAINT FK_PC_Product  FOREIGN KEY (ProductID)  REFERENCES dbo.Products(ProductID)  ON DELETE CASCADE,
    CONSTRAINT FK_PC_Category FOREIGN KEY (CategoryID) REFERENCES dbo.Categories(CategoryID) ON DELETE CASCADE
);

/* ============================
   SUPPLIERS & GOODS RECEIPTS
============================ */
CREATE TABLE dbo.Suppliers (
    SupplierID    INT IDENTITY(1,1) PRIMARY KEY,
    SupplierName  NVARCHAR(200) NOT NULL,
    Phone         VARCHAR(20)   NULL
    -- có thể thêm UNIQUE(Phone) nếu cần
);

CREATE TABLE dbo.GoodsReceipts (
    ReceiptID   BIGINT IDENTITY(1,1) PRIMARY KEY,
    SupplierID  INT NOT NULL,
    UserID      INT NOT NULL,                    -- nhân viên lập phiếu
    ReceiptDate DATETIME2(3) NOT NULL CONSTRAINT DF_GR_Date DEFAULT SYSDATETIME(),
    TotalAmount DECIMAL(12,2) NOT NULL CONSTRAINT DF_GR_Total DEFAULT 0,
    CONSTRAINT FK_GR_Supplier FOREIGN KEY (SupplierID) REFERENCES dbo.Suppliers(SupplierID),
    CONSTRAINT FK_GR_User     FOREIGN KEY (UserID)     REFERENCES dbo.Users(UserID)
);

-- Mỗi sản phẩm xuất hiện tối đa 1 dòng/phiếu (đơn giản hóa cho đồ án)
CREATE TABLE dbo.GoodsReceiptDetails (
    ReceiptID   BIGINT       NOT NULL,
    ProductID   INT          NOT NULL,
    Quantity    INT          NOT NULL,
    ImportPrice DECIMAL(10,2) NOT NULL,
    ExpiryDate  DATE         NULL,
    CONSTRAINT PK_GRD PRIMARY KEY (ReceiptID, ProductID),
    CONSTRAINT FK_GRD_Receipt FOREIGN KEY (ReceiptID) REFERENCES dbo.GoodsReceipts(ReceiptID) ON DELETE CASCADE,
    CONSTRAINT FK_GRD_Product FOREIGN KEY (ProductID)  REFERENCES dbo.Products(ProductID),
    CONSTRAINT CK_GRD_Qty_Positive CHECK (Quantity > 0),
    CONSTRAINT CK_GRD_Price_Positive CHECK (ImportPrice > 0)
);

/* ============================
   INDEXES nhẹ cho FK hay join
============================ */
CREATE INDEX IX_UsersRoles_RoleID   ON dbo.UsersRoles(RoleID);
CREATE INDEX IX_PC_CategoryID       ON dbo.ProductCategories(CategoryID);
CREATE INDEX IX_GR_Supplier         ON dbo.GoodsReceipts(SupplierID);
CREATE INDEX IX_GR_User             ON dbo.GoodsReceipts(UserID);
CREATE INDEX IX_GRD_Product         ON dbo.GoodsReceiptDetails(ProductID);

/* ============================
   SAMPLE DATA (ít – đủ test)
============================ */
-- Roles
INSERT INTO dbo.Roles(RoleName) VALUES (N'Admin'), (N'Seller'), (N'User');

-- Users (mật khẩu demo: '123456', 'seller123', 'user123')
INSERT INTO dbo.Users(Username, PasswordHash, FullName)
VALUES
('admin',  HASHBYTES('SHA2_256', CONVERT(NVARCHAR(200),'123456')),  N'Quản trị viên'),
('seller01',HASHBYTES('SHA2_256', CONVERT(NVARCHAR(200),'seller123')),N'Nhân viên bán hàng 01'),
('user01', HASHBYTES('SHA2_256', CONVERT(NVARCHAR(200),'user123')),  N'Khách hàng 01');

-- Gán vai trò (M-N)
INSERT INTO dbo.UsersRoles(UserID, RoleID)
SELECT u.UserID, r.RoleID
FROM dbo.Users u
JOIN dbo.Roles r
  ON (u.Username = 'admin'    AND r.RoleName = N'Admin')
  OR (u.Username = 'seller01' AND r.RoleName = N'Seller')
  OR (u.Username = 'user01'   AND r.RoleName = N'User');

-- Categories
INSERT INTO dbo.Categories(CategoryName)
VALUES (N'Beverages'), (N'Snacks'), (N'Dairy');

-- Products
INSERT INTO dbo.Products(SKU, ProductName, SellingPrice, StockQuantity)
VALUES
('SP0001', N'Milk 1L',     25.00, 20),
('SP0002', N'Chips 100g',   8.00, 30),
('SP0003', N'Green Tea',   12.00, 15);

-- ProductCategories (M-N)
INSERT INTO dbo.ProductCategories(ProductID, CategoryID)
SELECT p.ProductID, c.CategoryID
FROM dbo.Products p
JOIN dbo.Categories c
  ON (p.SKU='SP0001' AND c.CategoryName=N'Dairy')
  OR (p.SKU='SP0002' AND c.CategoryName=N'Snacks')
  OR (p.SKU='SP0003' AND c.CategoryName=N'Beverages');

-- Suppliers
INSERT INTO dbo.Suppliers(SupplierName, Phone)
VALUES (N'ABC Supply', '090000001'), (N'FreshFoods Co', '090000002');

-- 2 phiếu nhập mẫu
INSERT INTO dbo.GoodsReceipts(SupplierID, UserID, ReceiptDate, TotalAmount)
VALUES
((SELECT SupplierID FROM dbo.Suppliers WHERE SupplierName=N'ABC Supply'),
 (SELECT UserID FROM dbo.Users WHERE Username='seller01'),
 SYSDATETIME(), 990.00),
((SELECT SupplierID FROM dbo.Suppliers WHERE SupplierName=N'FreshFoods Co'),
 (SELECT UserID FROM dbo.Users WHERE Username='seller01'),
 DATEADD(DAY,-2,SYSDATETIME()), 500.00);

-- Chi tiết phiếu nhập
INSERT INTO dbo.GoodsReceiptDetails(ReceiptID, ProductID, Quantity, ImportPrice, ExpiryDate)
VALUES
(1, (SELECT ProductID FROM dbo.Products WHERE SKU='SP0001'), 50, 15.00, DATEADD(MONTH, 6, CAST(GETDATE() AS DATE))), -- Milk
(1, (SELECT ProductID FROM dbo.Products WHERE SKU='SP0003'), 30,  8.00, DATEADD(MONTH,12, CAST(GETDATE() AS DATE))), -- Green Tea
(2, (SELECT ProductID FROM dbo.Products WHERE SKU='SP0002'),100,  5.00, NULL);                                        -- Chips
GO

/* ============================
   QUICK TEST QUERIES (tùy chọn)
============================ */
-- Sản phẩm và các nhóm (M-N)
SELECT p.SKU, p.ProductName, STRING_AGG(c.CategoryName, ', ') WITHIN GROUP (ORDER BY c.CategoryName) AS Categories
FROM dbo.Products p
LEFT JOIN dbo.ProductCategories pc ON pc.ProductID = p.ProductID
LEFT JOIN dbo.Categories c ON c.CategoryID = pc.CategoryID
GROUP BY p.SKU, p.ProductName;

-- Người dùng và vai trò (M-N)
SELECT u.Username, STRING_AGG(r.RoleName, ', ') WITHIN GROUP (ORDER BY r.RoleName) AS Roles
FROM dbo.Users u
JOIN dbo.UsersRoles ur ON ur.UserID = u.UserID
JOIN dbo.Roles r ON r.RoleID = ur.RoleID
GROUP BY u.Username;
