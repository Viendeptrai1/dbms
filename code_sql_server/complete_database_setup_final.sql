/*
=============================================
COMPLETE DATABASE SETUP - QLNHAPHANG (FINAL)
=============================================
Mô tả: File SQL hoàn chỉnh để setup toàn bộ database (FINAL VERSION)
Thứ tự: Schema → Stored Procedures → Functions → Views → Triggers → Indexes → User Management
Ngày tạo: 2025-01-01
*/

USE master;
GO

-- Tạo database nếu chưa có
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'QLNhapHang')
BEGIN
    CREATE DATABASE QLNhapHang;
END
GO

USE QLNhapHang;
GO

/*
=============================================
01. SCHEMA - CREATE TABLES
=============================================
*/

-- Bảng vai trò người dùng
CREATE TABLE dbo.Roles (
    RoleID      INT IDENTITY(1,1) PRIMARY KEY,
    RoleName    NVARCHAR(50)  NOT NULL UNIQUE,
    Description NVARCHAR(200) NULL
);
GO

-- Bảng người dùng
CREATE TABLE dbo.Users (
    UserID      INT IDENTITY(1,1) PRIMARY KEY,
    Username    VARCHAR(50)  NOT NULL UNIQUE,
    Password    NVARCHAR(100) NOT NULL,  -- Lưu plain text, không băm
    FullName    NVARCHAR(100) NOT NULL,
    IsActive    BIT           NOT NULL CONSTRAINT DF_Users_IsActive DEFAULT 1,
    CreatedAt   DATETIME2(3)  NOT NULL CONSTRAINT DF_Users_CreatedAt DEFAULT SYSDATETIME()
);
GO

-- Bảng trung gian M-N giữa Users và Roles
CREATE TABLE dbo.UsersRoles (
    UserID INT NOT NULL,
    RoleID INT NOT NULL,
    CONSTRAINT PK_UsersRoles PRIMARY KEY (UserID, RoleID),
    CONSTRAINT FK_UsersRoles_User  FOREIGN KEY (UserID) REFERENCES dbo.Users(UserID) ON DELETE CASCADE,
    CONSTRAINT FK_UsersRoles_Role  FOREIGN KEY (RoleID) REFERENCES dbo.Roles(RoleID) ON DELETE CASCADE
);
GO

-- Bảng danh mục sản phẩm
CREATE TABLE dbo.Categories (
    CategoryID   INT IDENTITY(1,1) PRIMARY KEY,
    CategoryName NVARCHAR(100) NOT NULL UNIQUE,
    Description  NVARCHAR(200) NULL
);
GO

-- Bảng sản phẩm
CREATE TABLE dbo.Products (
    ProductID     INT IDENTITY(1,1) PRIMARY KEY,
    SKU           VARCHAR(50)   NOT NULL UNIQUE,
    ProductName   NVARCHAR(200) NOT NULL,
    SellingPrice  DECIMAL(10,2) NOT NULL,
    StockQuantity INT           NOT NULL CONSTRAINT DF_Products_Stock DEFAULT 0,
    CreatedAt     DATETIME2(3)  NOT NULL CONSTRAINT DF_Products_CreatedAt DEFAULT SYSDATETIME(),
    CONSTRAINT CK_Products_Stock_NonNegative CHECK (StockQuantity >= 0),
    CONSTRAINT CK_Products_Price_NonNegative CHECK (SellingPrice  >= 0)
);
GO

-- Bảng trung gian M-N giữa Products và Categories
CREATE TABLE dbo.ProductCategories (
    ProductID  INT NOT NULL,
    CategoryID INT NOT NULL,
    CONSTRAINT PK_ProductCategories PRIMARY KEY (ProductID, CategoryID),
    CONSTRAINT FK_PC_Product  FOREIGN KEY (ProductID)  REFERENCES dbo.Products(ProductID)  ON DELETE CASCADE,
    CONSTRAINT FK_PC_Category FOREIGN KEY (CategoryID) REFERENCES dbo.Categories(CategoryID) ON DELETE CASCADE
);
GO

-- Bảng nhà cung cấp
CREATE TABLE dbo.Suppliers (
    SupplierID    INT IDENTITY(1,1) PRIMARY KEY,
    SupplierName  NVARCHAR(200) NOT NULL,
    Phone         VARCHAR(20)   NULL
);
GO

-- Bảng phiếu nhập hàng
CREATE TABLE dbo.GoodsReceipts (
    ReceiptID     INT IDENTITY(1,1) PRIMARY KEY,
    ReceiptDate   DATETIME2(3)  NOT NULL CONSTRAINT DF_GR_ReceiptDate DEFAULT SYSDATETIME(),
    SupplierID    INT           NOT NULL,
    UserID        INT           NOT NULL,
    TotalAmount   DECIMAL(15,2) NOT NULL CONSTRAINT DF_GR_TotalAmount DEFAULT 0,
    Notes         NVARCHAR(500) NULL,
    CONSTRAINT FK_GR_Supplier FOREIGN KEY (SupplierID) REFERENCES dbo.Suppliers(SupplierID),
    CONSTRAINT FK_GR_User     FOREIGN KEY (UserID)     REFERENCES dbo.Users(UserID)
);
GO

-- Bảng chi tiết phiếu nhập
CREATE TABLE dbo.GoodsReceiptDetails (
    ReceiptDetailID INT IDENTITY(1,1) PRIMARY KEY,
    ReceiptID       INT           NOT NULL,
    ProductID       INT           NOT NULL,
    Quantity        INT           NOT NULL,
    ImportPrice     DECIMAL(10,2) NOT NULL,
    LineAmount      DECIMAL(15,2) NOT NULL,
    CONSTRAINT FK_GRD_Receipt FOREIGN KEY (ReceiptID) REFERENCES dbo.GoodsReceipts(ReceiptID) ON DELETE CASCADE,
    CONSTRAINT FK_GRD_Product FOREIGN KEY (ProductID) REFERENCES dbo.Products(ProductID),
    CONSTRAINT CK_GRD_Quantity_Positive CHECK (Quantity > 0),
    CONSTRAINT CK_GRD_Price_Positive    CHECK (ImportPrice > 0)
);
GO

-- Bảng lịch sử giá
CREATE TABLE dbo.ProductPriceHistory (
    PriceHistoryID INT IDENTITY(1,1) PRIMARY KEY,
    ProductID      INT           NOT NULL,
    OldPrice       DECIMAL(10,2) NOT NULL,
    NewPrice       DECIMAL(10,2) NOT NULL,
    ChangeDate     DATETIME2(3)  NOT NULL CONSTRAINT DF_PPH_ChangeDate DEFAULT SYSDATETIME(),
    ChangedBy      INT           NOT NULL,
    Reason         NVARCHAR(200) NULL,
    CONSTRAINT FK_PPH_Product FOREIGN KEY (ProductID) REFERENCES dbo.Products(ProductID),
    CONSTRAINT FK_PPH_User    FOREIGN KEY (ChangedBy) REFERENCES dbo.Users(UserID)
);
GO

/*
=============================================
02. USER-DEFINED TYPES (TVP)
=============================================
*/

-- TVP cho GoodsReceiptLine
CREATE TYPE dbo.udt_GoodsReceiptLine AS TABLE (
    ProductID   INT           NOT NULL,
    Quantity    INT           NOT NULL,
    ImportPrice DECIMAL(10,2) NOT NULL
);
GO

-- TVP cho SKUList
CREATE TYPE dbo.udt_SKUList AS TABLE (
    SKU NVARCHAR(50) NOT NULL
);
GO

-- TVP cho CategoryNameList
CREATE TYPE dbo.udt_CategoryNameList AS TABLE (
    CategoryName NVARCHAR(100) NOT NULL
);
GO

/*
=============================================
03. SAMPLE DATA - INSERT INITIAL DATA
=============================================
*/

-- Thêm roles
INSERT INTO dbo.Roles (RoleName, Description) VALUES
(N'Admin', N'Quản trị viên hệ thống'),
(N'Seller', N'Nhân viên bán hàng'),
(N'User', N'Người dùng cơ bản');
GO

-- Thêm categories
INSERT INTO dbo.Categories (CategoryName, Description) VALUES
(N'Dairy', N'Sản phẩm từ sữa'),
(N'Beverages', N'Đồ uống'),
(N'Snacks', N'Đồ ăn vặt'),
(N'Frozen', N'Đông lạnh');
GO

-- Thêm suppliers
INSERT INTO dbo.Suppliers (SupplierName, Phone) VALUES
(N'Công ty TNHH ABC', N'0123456789'),
(N'Công ty XYZ', N'0987654321'),
(N'Nhà cung cấp DEF', N'0369258147');
GO

-- Thêm products
INSERT INTO dbo.Products (SKU, ProductName, SellingPrice, StockQuantity) VALUES
('MILK001', N'Sữa tươi Vinamilk 1L', 25000, 100),
('COKE001', N'Coca Cola 330ml', 15000, 200),
('CHIP001', N'Khoai tây chiên Lays', 20000, 150),
('ICE001', N'Kem tươi Wall''s', 18000, 80);
GO

-- Thêm product categories
INSERT INTO dbo.ProductCategories (ProductID, CategoryID) VALUES
(1, 1), (2, 2), (3, 3), (4, 4);
GO

/*
=============================================
04. STORED PROCEDURES
=============================================
*/

CREATE PROCEDURE sp_CreateGoodsReceipt
    @SupplierID INT,
    @UserID INT,
    @Notes NVARCHAR(500) = NULL,
    @ReceiptLines dbo.udt_GoodsReceiptLine READONLY,
    @ReceiptID INT OUTPUT,
    @Message NVARCHAR(200) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        IF NOT EXISTS (SELECT 1 FROM dbo.Suppliers WHERE SupplierID = @SupplierID)
        BEGIN
            SET @Message = N'Nhà cung cấp không tồn tại!';
            SET @ReceiptID = -1;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE UserID = @UserID AND IsActive = 1)
        BEGIN
            SET @Message = N'User không tồn tại hoặc đã bị khóa!';
            SET @ReceiptID = -1;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        INSERT INTO dbo.GoodsReceipts (SupplierID, UserID, Notes, TotalAmount)
        VALUES (@SupplierID, @UserID, @Notes, 0);
        
        SET @ReceiptID = SCOPE_IDENTITY();
        
        INSERT INTO dbo.GoodsReceiptDetails (ReceiptID, ProductID, Quantity, ImportPrice, LineAmount)
        SELECT @ReceiptID, ProductID, Quantity, ImportPrice, Quantity * ImportPrice
        FROM @ReceiptLines;
        
        UPDATE dbo.GoodsReceipts 
        SET TotalAmount = (SELECT SUM(LineAmount) FROM dbo.GoodsReceiptDetails WHERE ReceiptID = @ReceiptID)
        WHERE ReceiptID = @ReceiptID;
        
        COMMIT TRANSACTION;
        SET @Message = N'Tạo phiếu nhập thành công!';
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
            
        SET @Message = N'Lỗi: ' + ERROR_MESSAGE();
        SET @ReceiptID = -1;
    END CATCH
END;
GO

CREATE PROCEDURE sp_DeleteGoodsReceipt
    @ReceiptID INT,
    @Message NVARCHAR(200) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        IF NOT EXISTS (SELECT 1 FROM dbo.GoodsReceipts WHERE ReceiptID = @ReceiptID)
        BEGIN
            SET @Message = N'Phiếu nhập không tồn tại!';
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        DECLARE @CanDelete BIT = 1;
        
        SELECT @CanDelete = CASE 
            WHEN SUM(p.StockQuantity - grd.Quantity) < 0 THEN 0
            ELSE 1
        END
        FROM dbo.GoodsReceiptDetails grd
        JOIN dbo.Products p ON grd.ProductID = p.ProductID
        WHERE grd.ReceiptID = @ReceiptID;
        
        IF @CanDelete = 0
        BEGIN
            SET @Message = N'Không thể xóa! Tồn kho sẽ bị âm.';
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        DELETE FROM dbo.GoodsReceipts WHERE ReceiptID = @ReceiptID;
        
        COMMIT TRANSACTION;
        SET @Message = N'Xóa phiếu nhập thành công!';
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
            
        SET @Message = N'Lỗi: ' + ERROR_MESSAGE();
    END CATCH
END;
GO

CREATE PROCEDURE sp_BulkAdjustPriceByPercent
    @SKUList dbo.udt_SKUList READONLY,
    @PercentAdjustment DECIMAL(5,2),
    @Reason NVARCHAR(200) = NULL,
    @ChangedBy INT,
    @Message NVARCHAR(200) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE UserID = @ChangedBy AND IsActive = 1)
        BEGIN
            SET @Message = N'User không tồn tại hoặc đã bị khóa!';
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        IF @PercentAdjustment < -50 OR @PercentAdjustment > 100
        BEGIN
            SET @Message = N'Phần trăm điều chỉnh phải trong khoảng -50% đến +100%!';
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        UPDATE p
        SET SellingPrice = p.SellingPrice * (1 + @PercentAdjustment / 100)
        FROM dbo.Products p
        INNER JOIN @SKUList sku ON p.SKU = sku.SKU;
        
        DECLARE @AffectedRows INT = @@ROWCOUNT;
        
        IF @AffectedRows = 0
        BEGIN
            SET @Message = N'Không có sản phẩm nào được cập nhật!';
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        COMMIT TRANSACTION;
        SET @Message = N'Điều chỉnh giá thành công cho ' + CAST(@AffectedRows AS NVARCHAR(10)) + ' sản phẩm!';
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
            
        SET @Message = N'Lỗi: ' + ERROR_MESSAGE();
    END CATCH
END;
GO

CREATE PROCEDURE sp_AddProductWithCategories
    @SKU VARCHAR(50),
    @ProductName NVARCHAR(200),
    @SellingPrice DECIMAL(10,2),
    @CategoryNames dbo.udt_CategoryNameList READONLY,
    @ProductID INT OUTPUT,
    @Message NVARCHAR(200) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        IF EXISTS (SELECT 1 FROM dbo.Products WHERE SKU = @SKU)
        BEGIN
            SET @Message = N'SKU đã tồn tại!';
            SET @ProductID = -1;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        IF NOT EXISTS (SELECT 1 FROM @CategoryNames cn 
                      INNER JOIN dbo.Categories c ON cn.CategoryName = c.CategoryName)
        BEGIN
            SET @Message = N'Một hoặc nhiều danh mục không tồn tại!';
            SET @ProductID = -1;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        INSERT INTO dbo.Products (SKU, ProductName, SellingPrice, StockQuantity)
        VALUES (@SKU, @ProductName, @SellingPrice, 0);
        
        SET @ProductID = SCOPE_IDENTITY();
        
        INSERT INTO dbo.ProductCategories (ProductID, CategoryID)
        SELECT @ProductID, c.CategoryID
        FROM @CategoryNames cn
        INNER JOIN dbo.Categories c ON cn.CategoryName = c.CategoryName;
        
        COMMIT TRANSACTION;
        SET @Message = N'Tạo sản phẩm thành công!';
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
            
        SET @Message = N'Lỗi: ' + ERROR_MESSAGE();
        SET @ProductID = -1;
    END CATCH
END;
GO

/*
=============================================
05. FUNCTIONS
=============================================
*/

-- Scalar Functions
CREATE FUNCTION dbo.fn_TotalStock(@ProductID INT)
RETURNS INT
AS
BEGIN
    DECLARE @qty INT;
    SELECT @qty = p.StockQuantity FROM dbo.Products p WHERE p.ProductID = @ProductID;
    RETURN @qty;
END;
GO

CREATE FUNCTION dbo.fn_LastImportPrice(@ProductID INT)
RETURNS DECIMAL(10,2)
AS
BEGIN
    DECLARE @price DECIMAL(10,2);
    SELECT TOP(1) @price = CAST(d.ImportPrice AS DECIMAL(10,2))
    FROM dbo.GoodsReceiptDetails d
    JOIN dbo.GoodsReceipts r ON r.ReceiptID = d.ReceiptID
    WHERE d.ProductID = @ProductID
    ORDER BY r.ReceiptDate DESC, d.ReceiptID DESC;
    RETURN @price;
END;
GO

CREATE FUNCTION dbo.fn_ProductInventoryValue(@ProductID INT)
RETURNS DECIMAL(18,2)
AS
BEGIN
    DECLARE @qty INT, @sell DECIMAL(10,2), @last DECIMAL(10,2);
    SELECT @qty = StockQuantity, @sell = SellingPrice
    FROM dbo.Products WHERE ProductID = @ProductID;

    SET @last = dbo.fn_LastImportPrice(@ProductID);
    RETURN CAST(ISNULL(@qty,0) AS DECIMAL(18,2)) * ISNULL(@last, @sell);
END;
GO

CREATE FUNCTION dbo.fn_ProductCount()
RETURNS INT
AS
BEGIN
    DECLARE @c INT = (SELECT COUNT(*) FROM dbo.Products);
    RETURN @c;
END;
GO

CREATE FUNCTION dbo.fn_TotalInventoryValue()
RETURNS DECIMAL(18,2)
AS
BEGIN
    DECLARE @v DECIMAL(18,2);
    SELECT @v = SUM(dbo.fn_ProductInventoryValue(p.ProductID))
    FROM dbo.Products p;
    RETURN ISNULL(@v, 0);
END;
GO

-- Table-Valued Functions
CREATE FUNCTION dbo.fn_ProductImportHistory(@ProductID INT)
RETURNS TABLE
AS
RETURN
(
    SELECT
        r.ReceiptID,
        r.ReceiptDate,
        s.SupplierName,
        d.Quantity,
        d.ImportPrice,
        LineAmount = CAST(d.Quantity AS DECIMAL(18,2)) * d.ImportPrice
    FROM dbo.GoodsReceiptDetails d
    JOIN dbo.GoodsReceipts r ON r.ReceiptID = d.ReceiptID
    JOIN dbo.Suppliers s ON s.SupplierID = r.SupplierID
    WHERE d.ProductID = @ProductID
);
GO

CREATE FUNCTION dbo.fn_ProductsByCategory(@CategoryName NVARCHAR(100))
RETURNS TABLE
AS
RETURN
(
    SELECT p.ProductID, p.SKU, p.ProductName, p.SellingPrice, p.StockQuantity
    FROM dbo.Products p
    JOIN dbo.ProductCategories pc ON pc.ProductID = p.ProductID
    JOIN dbo.Categories c ON c.CategoryID = pc.CategoryID
    WHERE c.CategoryName = @CategoryName
);
GO

CREATE FUNCTION dbo.fn_TopProductsByImportValue(
    @From DATETIME2(3),
    @To DATETIME2(3) = NULL,
    @TopN INT = 10,
    @SupplierName NVARCHAR(200) = NULL,
    @CategoryName NVARCHAR(100) = NULL
)
RETURNS TABLE
AS
RETURN
(
    WITH base AS (
        SELECT
            d.ProductID,
            TotalQty = SUM(d.Quantity),
            ImportValue = SUM(CAST(d.Quantity AS DECIMAL(18,2)) * d.ImportPrice)
        FROM dbo.GoodsReceiptDetails d
        JOIN dbo.GoodsReceipts r ON r.ReceiptID = d.ReceiptID
        WHERE r.ReceiptDate >= @From
          AND (@To IS NULL OR r.ReceiptDate < @To)
          AND (
                @SupplierName IS NULL
                OR EXISTS (SELECT 1
                           FROM dbo.Suppliers s
                           WHERE s.SupplierID = r.SupplierID
                             AND s.SupplierName = @SupplierName)
              )
          AND (
                @CategoryName IS NULL
                OR EXISTS (SELECT 1
                           FROM dbo.ProductCategories pc
                           JOIN dbo.Categories c ON c.CategoryID = pc.CategoryID
                           WHERE pc.ProductID = d.ProductID
                             AND c.CategoryName = @CategoryName)
              )
        GROUP BY d.ProductID
    )
    SELECT TOP (@TopN)
        p.ProductID,
        p.SKU,
        p.ProductName,
        b.TotalQty,
        b.ImportValue
    FROM base b
    JOIN dbo.Products p ON p.ProductID = b.ProductID
    ORDER BY b.ImportValue DESC, p.SKU
);
GO

CREATE FUNCTION dbo.fn_ImportSummaryBySupplier(
    @From DATETIME2(3),
    @To DATETIME2(3) = NULL
)
RETURNS TABLE
AS
RETURN
(
    SELECT
        s.SupplierName,
        TotalReceipts = COUNT(DISTINCT r.ReceiptID),
        TotalQty = SUM(d.Quantity),
        TotalValue = SUM(CAST(d.Quantity AS DECIMAL(18,2)) * d.ImportPrice)
    FROM dbo.GoodsReceiptDetails d
    JOIN dbo.GoodsReceipts r ON r.ReceiptID = d.ReceiptID
    JOIN dbo.Suppliers s ON s.SupplierID = r.SupplierID
    WHERE r.ReceiptDate >= @From
      AND (@To IS NULL OR r.ReceiptDate < @To)
    GROUP BY s.SupplierName
);
GO

CREATE FUNCTION dbo.fn_ProductsNeverImported()
RETURNS TABLE
AS
RETURN
(
    SELECT p.ProductID, p.SKU, p.ProductName, p.SellingPrice, p.StockQuantity
    FROM dbo.Products p
    WHERE NOT EXISTS (
        SELECT 1 FROM dbo.GoodsReceiptDetails d WHERE d.ProductID = p.ProductID
    )
);
GO

/*
=============================================
06. VIEWS
=============================================
*/

CREATE VIEW dbo.vw_ProductsWithCategories
AS
SELECT
    p.ProductID, p.SKU, p.ProductName, p.SellingPrice, p.StockQuantity,
    Categories = STRING_AGG(c.CategoryName, ', ') WITHIN GROUP (ORDER BY c.CategoryName)
FROM dbo.Products p
LEFT JOIN dbo.ProductCategories pc ON pc.ProductID = p.ProductID
LEFT JOIN dbo.Categories c ON c.CategoryID = pc.CategoryID
GROUP BY p.ProductID, p.SKU, p.ProductName, p.SellingPrice, p.StockQuantity;
GO

CREATE VIEW dbo.vw_InventoryValuation
AS
SELECT
    p.ProductID,
    p.SKU,
    p.ProductName,
    p.StockQuantity,
    LastImportPrice = dbo.fn_LastImportPrice(p.ProductID),
    InventoryValue = dbo.fn_ProductInventoryValue(p.ProductID)
FROM dbo.Products p;
GO

CREATE VIEW dbo.vw_ImportLines
AS
SELECT
    r.ReceiptID,
    r.ReceiptDate,
    s.SupplierName,
    d.ProductID,
    p.SKU,
    p.ProductName,
    d.Quantity,
    d.ImportPrice,
    LineAmount = CAST(d.Quantity AS DECIMAL(18,2)) * d.ImportPrice
FROM dbo.GoodsReceiptDetails d
JOIN dbo.GoodsReceipts r ON r.ReceiptID = d.ReceiptID
JOIN dbo.Suppliers s ON s.SupplierID = r.SupplierID
JOIN dbo.Products p ON p.ProductID = d.ProductID;
GO

/*
=============================================
07. TRIGGERS
=============================================
*/

CREATE TRIGGER TR_GRD_StockAndTotal
ON dbo.GoodsReceiptDetails
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    /* ===== 1) Tính delta tồn kho theo ProductID ===== */
    DECLARE @prodDelta TABLE(
        ProductID INT PRIMARY KEY,
        Qty INT NOT NULL
    );

    INSERT INTO @prodDelta(ProductID, Qty)
    SELECT ProductID, SUM(Qty) AS Qty
    FROM (
        SELECT i.ProductID, i.Quantity AS Qty FROM inserted i
        UNION ALL
        SELECT d.ProductID, -d.Quantity FROM deleted d
    ) X
    GROUP BY ProductID;

    /* Update tồn kho (nếu có ảnh hưởng) */
    UPDATE p
        SET p.StockQuantity = p.StockQuantity + pd.Qty
    FROM dbo.Products p
    JOIN @prodDelta pd ON pd.ProductID = p.ProductID;

    /* Nếu âm tồn -> rollback toàn bộ */
    IF EXISTS (
        SELECT 1
        FROM dbo.Products p
        JOIN @prodDelta pd ON pd.ProductID = p.ProductID
        WHERE p.StockQuantity < 0
    )
    BEGIN
        RAISERROR(N'Tồn kho âm - thao tác bị hủy.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END

    /* ===== 2) Re-calc TotalAmount cho các phiếu bị ảnh hưởng ===== */
    DECLARE @affectedReceipts TABLE(
        ReceiptID INT PRIMARY KEY
    );

    INSERT INTO @affectedReceipts(ReceiptID)
    SELECT DISTINCT ReceiptID FROM inserted
    UNION
    SELECT DISTINCT ReceiptID FROM deleted;

    /* Ghi đè TotalAmount = SUM(Quantity*ImportPrice) (hoặc 0 nếu không còn chi tiết) */
    UPDATE r
        SET r.TotalAmount = ISNULL(agg.SumAmount, 0)
    FROM dbo.GoodsReceipts r
    JOIN @affectedReceipts ar ON ar.ReceiptID = r.ReceiptID
    OUTER APPLY (
        SELECT SUM(CAST(d.Quantity AS DECIMAL(18,2)) * d.ImportPrice) AS SumAmount
        FROM dbo.GoodsReceiptDetails d
        WHERE d.ReceiptID = r.ReceiptID
    ) agg;
END;
GO

CREATE TRIGGER TR_Products_Normalize
ON dbo.Products
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    -- Chuẩn hóa SKU: trim + upper; Tên: trim
    UPDATE p
       SET p.SKU = UPPER(LTRIM(RTRIM(p.SKU))),
           p.ProductName= LTRIM(RTRIM(p.ProductName))
    FROM dbo.Products p
    JOIN inserted i ON i.ProductID = p.ProductID;
END;
GO

CREATE TRIGGER TR_Product_LogPriceChange
ON dbo.Products
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.ProductPriceHistory(ProductID, OldPrice, NewPrice, ChangedBy)
    SELECT i.ProductID, d.SellingPrice, i.SellingPrice, SUSER_SNAME()
    FROM inserted i
    JOIN deleted d ON d.ProductID = i.ProductID
    WHERE ISNULL(i.SellingPrice,0) <> ISNULL(d.SellingPrice,0);
END;
GO

CREATE TRIGGER TR_Categories_PreventDeleteIfInUse
ON dbo.Categories
INSTEAD OF DELETE
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (
        SELECT 1 FROM deleted d
        JOIN dbo.ProductCategories pc ON pc.CategoryID = d.CategoryID
    )
    BEGIN
        RAISERROR(N'Không thể xóa Category vì đang được gán cho sản phẩm.',16,1);
        ROLLBACK TRANSACTION;
        RETURN;
    END
    -- Nếu không còn dùng -> cho xóa bình thường
    DELETE c FROM dbo.Categories c JOIN deleted d ON d.CategoryID = c.CategoryID;
END;
GO

CREATE TRIGGER TR_DDL_PreventDrop
ON DATABASE
FOR DROP_TABLE, ALTER_TABLE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @obj SYSNAME = EVENTDATA().value('(/EVENT_INSTANCE/ObjectName)[1]','SYSNAME');
    DECLARE @schema SYSNAME = EVENTDATA().value('(/EVENT_INSTANCE/SchemaName)[1]','SYSNAME');

    IF @schema = 'dbo' AND @obj IN
       ('Products','Categories','ProductCategories','Suppliers','GoodsReceipts','GoodsReceiptDetails','Users','Roles','UsersRoles')
    BEGIN
        RAISERROR(N'DDL bị chặn: không được DROP/ALTER bảng lõi %s.%s.', 16, 1, @schema, @obj);
        ROLLBACK;
    END
END;
GO

/*
=============================================
08. INDEXES
=============================================
*/

CREATE NONCLUSTERED INDEX IX_Products_SKU ON dbo.Products (SKU);
CREATE NONCLUSTERED INDEX IX_Products_ProductName ON dbo.Products (ProductName);
CREATE NONCLUSTERED INDEX IX_GoodsReceipts_ReceiptDate ON dbo.GoodsReceipts (ReceiptDate);
CREATE NONCLUSTERED INDEX IX_GoodsReceipts_SupplierID ON dbo.GoodsReceipts (SupplierID);
CREATE NONCLUSTERED INDEX IX_GoodsReceiptDetails_ProductID ON dbo.GoodsReceiptDetails (ProductID);
CREATE NONCLUSTERED INDEX IX_GoodsReceiptDetails_ReceiptID ON dbo.GoodsReceiptDetails (ReceiptID);
CREATE NONCLUSTERED INDEX IX_Users_Username ON dbo.Users (Username);

-- Additional indexes for performance
CREATE INDEX IX_GR_ReceiptDate
ON dbo.GoodsReceipts(ReceiptDate)
INCLUDE (SupplierID, UserID, TotalAmount);
GO

CREATE INDEX IX_GRD_ReceiptID
ON dbo.GoodsReceiptDetails(ReceiptID)
INCLUDE (ProductID, Quantity, ImportPrice);
GO

/*
=============================================
09. USER MANAGEMENT STORED PROCEDURES
=============================================
*/

CREATE PROCEDURE sp_CreateUserWithRole
    @Username VARCHAR(50),
    @Password NVARCHAR(100),
    @FullName NVARCHAR(100),
    @RoleName NVARCHAR(20),
    @IsActive BIT = 1,
    @NewUserID INT OUTPUT,
    @Message NVARCHAR(200) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        IF EXISTS (SELECT 1 FROM dbo.Users WHERE Username = @Username)
        BEGIN
            SET @Message = N'Username đã tồn tại!';
            SET @NewUserID = -1;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        IF NOT EXISTS (SELECT 1 FROM dbo.Roles WHERE RoleName = @RoleName)
        BEGIN
            SET @Message = N'Role không hợp lệ!';
            SET @NewUserID = -1;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        INSERT INTO dbo.Users (Username, Password, FullName, IsActive)
        VALUES (@Username, @Password, @FullName, @IsActive);
        
        SET @NewUserID = SCOPE_IDENTITY();
        
        DECLARE @RoleID INT;
        SELECT @RoleID = RoleID FROM dbo.Roles WHERE RoleName = @RoleName;
        
        INSERT INTO dbo.UsersRoles (UserID, RoleID)
        VALUES (@NewUserID, @RoleID);
        
        COMMIT TRANSACTION;
        SET @Message = N'Tạo user thành công!';
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
            
        SET @Message = N'Lỗi: ' + ERROR_MESSAGE();
        SET @NewUserID = -1;
    END CATCH
END;
GO

CREATE PROCEDURE sp_ChangeUserRole
    @Username NVARCHAR(50),
    @NewRoleName NVARCHAR(20),
    @Message NVARCHAR(200) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Username = @Username)
        BEGIN
            SET @Message = N'User không tồn tại!';
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        IF NOT EXISTS (SELECT 1 FROM dbo.Roles WHERE RoleName = @NewRoleName)
        BEGIN
            SET @Message = N'Role không hợp lệ!';
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        DECLARE @UserID INT, @NewRoleID INT;
        
        SELECT @UserID = UserID FROM dbo.Users WHERE Username = @Username;
        SELECT @NewRoleID = RoleID FROM dbo.Roles WHERE RoleName = @NewRoleName;
        
        DELETE FROM dbo.UsersRoles WHERE UserID = @UserID;
        
        INSERT INTO dbo.UsersRoles (UserID, RoleID)
        VALUES (@UserID, @NewRoleID);
        
        COMMIT TRANSACTION;
        SET @Message = N'Thay đổi role thành công!';
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
            
        SET @Message = N'Lỗi: ' + ERROR_MESSAGE();
    END CATCH
END;
GO

CREATE PROCEDURE sp_ToggleUserStatus
    @Username NVARCHAR(50),
    @Message NVARCHAR(200) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Username = @Username)
        BEGIN
            SET @Message = N'User không tồn tại!';
            RETURN;
        END
        
        UPDATE dbo.Users 
        SET IsActive = CASE WHEN IsActive = 1 THEN 0 ELSE 1 END
        WHERE Username = @Username;
        
        DECLARE @NewStatus BIT;
        SELECT @NewStatus = IsActive FROM dbo.Users WHERE Username = @Username;
        
        SET @Message = CASE WHEN @NewStatus = 1 THEN N'Đã mở khóa user!' ELSE N'Đã khóa user!' END;
        
    END TRY
    BEGIN CATCH
        SET @Message = N'Lỗi: ' + ERROR_MESSAGE();
    END CATCH
END;
GO

CREATE PROCEDURE sp_DeleteUser
    @Username NVARCHAR(50),
    @Message NVARCHAR(200) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Username = @Username)
        BEGIN
            SET @Message = N'User không tồn tại!';
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        DECLARE @UserID INT;
        SELECT @UserID = UserID FROM dbo.Users WHERE Username = @Username;
        
        DELETE FROM dbo.UsersRoles WHERE UserID = @UserID;
        
        UPDATE dbo.Users 
        SET IsActive = 0, 
            Username = Username + '_DELETED_' + CONVERT(NVARCHAR(20), GETDATE(), 112)
        WHERE UserID = @UserID;
        
        COMMIT TRANSACTION;
        SET @Message = N'Xóa user thành công!';
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
            
        SET @Message = N'Lỗi: ' + ERROR_MESSAGE();
    END CATCH
END;
GO

CREATE PROCEDURE sp_GetUsersWithRoles
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        u.UserID,
        u.Username,
        u.IsActive,
        STRING_AGG(r.RoleName, ', ') WITHIN GROUP (ORDER BY r.RoleName) AS Roles,
        COUNT(ur.RoleID) AS RoleCount
    FROM dbo.Users u
    LEFT JOIN dbo.UsersRoles ur ON u.UserID = ur.UserID
    LEFT JOIN dbo.Roles r ON ur.RoleID = r.RoleID
    GROUP BY u.UserID, u.Username, u.IsActive
    ORDER BY u.Username;
END;
GO

CREATE PROCEDURE sp_CheckUserPermission
    @Username NVARCHAR(50),
    @Permission NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        CASE 
            WHEN EXISTS (
                SELECT 1 FROM dbo.Users u
                JOIN dbo.UsersRoles ur ON u.UserID = ur.UserID
                JOIN dbo.Roles r ON ur.RoleID = r.RoleID
                WHERE u.Username = @Username 
                AND u.IsActive = 1
                AND r.RoleName = @Permission
            ) THEN 1
            ELSE 0
        END AS HasPermission;
END;
GO

CREATE PROCEDURE sp_RevokeAllUserPermissions
    @Username NVARCHAR(50),
    @Message NVARCHAR(200) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Username = @Username)
        BEGIN
            SET @Message = N'User không tồn tại!';
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        DECLARE @UserID INT;
        SELECT @UserID = UserID FROM dbo.Users WHERE Username = @Username;
        
        DELETE FROM dbo.UsersRoles WHERE UserID = @UserID;
        
        COMMIT TRANSACTION;
        SET @Message = N'Đã revoke tất cả quyền của user!';
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
            
        SET @Message = N'Lỗi: ' + ERROR_MESSAGE();
    END CATCH
END;
GO

/*
=============================================
10. CREATE DEFAULT USERS
=============================================
*/

INSERT INTO dbo.Users (Username, Password, FullName, IsActive)
VALUES ('admin', 'admin123', N'Administrator', 1);

DECLARE @AdminUserID INT = SCOPE_IDENTITY();

INSERT INTO dbo.Users (Username, Password, FullName, IsActive)
VALUES ('seller', 'seller123', N'Seller User', 1);

DECLARE @SellerUserID INT = SCOPE_IDENTITY();

INSERT INTO dbo.Users (Username, Password, FullName, IsActive)
VALUES ('user', 'user123', N'Basic User', 1);

DECLARE @UserUserID INT = SCOPE_IDENTITY();

INSERT INTO dbo.UsersRoles (UserID, RoleID)
SELECT @AdminUserID, RoleID FROM dbo.Roles WHERE RoleName = 'Admin';

INSERT INTO dbo.UsersRoles (UserID, RoleID)
SELECT @SellerUserID, RoleID FROM dbo.Roles WHERE RoleName = 'Seller';

INSERT INTO dbo.UsersRoles (UserID, RoleID)
SELECT @UserUserID, RoleID FROM dbo.Roles WHERE RoleName = 'User';
GO

/*
=============================================
11. GRANT PERMISSIONS
=============================================
*/

IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = 'dbrole_Admin')
    CREATE ROLE dbrole_Admin;

IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = 'dbrole_Seller')
    CREATE ROLE dbrole_Seller;

IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = 'dbrole_User')
    CREATE ROLE dbrole_User;

GRANT SELECT, INSERT, UPDATE, DELETE ON SCHEMA::dbo TO dbrole_Admin;
GRANT EXECUTE ON SCHEMA::dbo TO dbrole_Admin;

GRANT SELECT, INSERT, UPDATE ON dbo.Products TO dbrole_Seller;
GRANT SELECT, INSERT, UPDATE ON dbo.Categories TO dbrole_Seller;
GRANT SELECT, INSERT, UPDATE ON dbo.ProductCategories TO dbrole_Seller;
GRANT SELECT, INSERT, UPDATE ON dbo.Suppliers TO dbrole_Seller;
GRANT SELECT, INSERT, UPDATE ON dbo.GoodsReceipts TO dbrole_Seller;
GRANT SELECT, INSERT, UPDATE ON dbo.GoodsReceiptDetails TO dbrole_Seller;
GRANT SELECT ON dbo.Users TO dbrole_Seller;
GRANT SELECT ON dbo.Roles TO dbrole_Seller;
GRANT SELECT ON dbo.UsersRoles TO dbrole_Seller;
GRANT EXECUTE ON OBJECT::dbo.sp_CreateGoodsReceipt TO dbrole_Seller;
GRANT EXECUTE ON OBJECT::dbo.sp_BulkAdjustPriceByPercent TO dbrole_Seller;
GRANT EXECUTE ON OBJECT::dbo.sp_AddProductWithCategories TO dbrole_Seller;
GRANT SELECT ON dbo.vw_ProductsWithCategories TO dbrole_Seller;
GRANT SELECT ON dbo.vw_InventoryValuation TO dbrole_Seller;
GRANT SELECT ON dbo.vw_ImportLines TO dbrole_Seller;

-- Grant permissions for new functions
GRANT SELECT ON OBJECT::dbo.fn_ProductsByCategory TO dbrole_Seller;
GRANT SELECT ON OBJECT::dbo.fn_ProductImportHistory TO dbrole_Seller;
GRANT SELECT ON OBJECT::dbo.fn_TopProductsByImportValue TO dbrole_Seller;
GRANT SELECT ON OBJECT::dbo.fn_ImportSummaryBySupplier TO dbrole_Seller;
GRANT SELECT ON OBJECT::dbo.fn_ProductsNeverImported TO dbrole_Seller;
GRANT EXECUTE ON OBJECT::dbo.fn_TotalStock TO dbrole_Seller;
GRANT EXECUTE ON OBJECT::dbo.fn_LastImportPrice TO dbrole_Seller;
GRANT EXECUTE ON OBJECT::dbo.fn_ProductInventoryValue TO dbrole_Seller;
GRANT EXECUTE ON OBJECT::dbo.fn_ProductCount TO dbrole_Seller;
GRANT EXECUTE ON OBJECT::dbo.fn_TotalInventoryValue TO dbrole_Seller;

GRANT SELECT ON dbo.Products TO dbrole_User;
GRANT SELECT ON dbo.Categories TO dbrole_User;
GRANT SELECT ON dbo.ProductCategories TO dbrole_User;
GRANT SELECT ON dbo.vw_ProductsWithCategories TO dbrole_User;
GRANT SELECT ON OBJECT::dbo.fn_ProductsByCategory TO dbrole_User;
GO

/*
=============================================
12. FINAL VERIFICATION
=============================================
*/

-- Check all users with roles
SELECT 
    u.Username,
    u.FullName,
    r.RoleName,
    u.IsActive
FROM dbo.Users u
JOIN dbo.UsersRoles ur ON u.UserID = ur.UserID
JOIN dbo.Roles r ON ur.RoleID = r.RoleID
ORDER BY u.Username;

-- Count functions and triggers
SELECT
  TVF = SUM(CASE WHEN type IN ('IF','TF') THEN 1 ELSE 0 END),
  Scalar = SUM(CASE WHEN type = 'FN' THEN 1 ELSE 0 END),
  Views = SUM(CASE WHEN type = 'V' THEN 1 ELSE 0 END),
  Triggers = SUM(CASE WHEN type = 'TR' THEN 1 ELSE 0 END)
FROM sys.objects
WHERE type IN ('IF','TF','FN','V','TR');

-- Test admin login
DECLARE @TestResult INT;
SELECT @TestResult = COUNT(*)
FROM dbo.Users u
JOIN dbo.UsersRoles ur ON u.UserID = ur.UserID
JOIN dbo.Roles r ON ur.RoleID = r.RoleID
WHERE u.Username = 'admin' 
AND u.Password = 'admin123'
AND u.IsActive = 1;

IF @TestResult > 0
    SELECT 'SUCCESS' AS Status, 'Admin login works' AS Message;
ELSE
    SELECT 'FAILED' AS Status, 'Admin login failed' AS Message;
