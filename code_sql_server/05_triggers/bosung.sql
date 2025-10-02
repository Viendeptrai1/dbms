USE QLNhapHang;
GO
/* =========================
   (A) THÊM HÀM CÒN THIẾU
========================= */

-- 1) TVF: sản phẩm chưa từng nhập (NO PARAMS)
IF OBJECT_ID('dbo.fn_ProductsNeverImported','IF') IS NOT NULL DROP FUNCTION dbo.fn_ProductsNeverImported;
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
-- TEST TVF
SELECT * FROM dbo.fn_ProductsNeverImported();

-- 2) Scalar: tổng số sản phẩm (NO PARAMS)
IF OBJECT_ID('dbo.fn_ProductCount','FN') IS NOT NULL DROP FUNCTION dbo.fn_ProductCount;
GO
CREATE FUNCTION dbo.fn_ProductCount()
RETURNS INT
AS
BEGIN
    DECLARE @c INT = (SELECT COUNT(*) FROM dbo.Products);
    RETURN @c;
END
GO
-- TEST scalar
SELECT TotalProducts = dbo.fn_ProductCount();

-- 3) Scalar: tổng giá trị tồn kho hiện tại (NO PARAMS)
IF OBJECT_ID('dbo.fn_TotalInventoryValue','FN') IS NOT NULL DROP FUNCTION dbo.fn_TotalInventoryValue;
GO
CREATE FUNCTION dbo.fn_TotalInventoryValue()
RETURNS DECIMAL(18,2)
AS
BEGIN
    DECLARE @v DECIMAL(18,2);
    SELECT @v = SUM(dbo.fn_ProductInventoryValue(p.ProductID))
    FROM dbo.Products p;
    RETURN ISNULL(@v, 0);
END
GO
-- TEST scalar
SELECT TotalInventoryValue = dbo.fn_TotalInventoryValue();
GO

/* (TÙY CHỌN) CẤP QUYỀN CHO SELLER/USER CHO 3 HÀM MỚI */
IF EXISTS (SELECT 1 FROM sys.database_principals WHERE name='dbrole_Seller')
BEGIN
    GRANT SELECT  ON OBJECT::dbo.fn_ProductsNeverImported TO dbrole_Seller;
    GRANT EXECUTE ON OBJECT::dbo.fn_ProductCount         TO dbrole_Seller;
    GRANT EXECUTE ON OBJECT::dbo.fn_TotalInventoryValue  TO dbrole_Seller;
END
IF EXISTS (SELECT 1 FROM sys.database_principals WHERE name='dbrole_User')
BEGIN
    GRANT SELECT  ON OBJECT::dbo.fn_ProductsNeverImported TO dbrole_User;
END
GO

/* =========================
   (B) THÊM 4 TRIGGER KHÁC
   (để đủ 5; đã có TR_GRD_StockAndTotal ở Pha 2)
========================= */

-- Bảng log đổi giá
IF OBJECT_ID('dbo.ProductPriceHistory','U') IS NULL
CREATE TABLE dbo.ProductPriceHistory(
    HistoryID   BIGINT IDENTITY(1,1) PRIMARY KEY,
    ProductID   INT NOT NULL,
    OldPrice    DECIMAL(10,2) NOT NULL,
    NewPrice    DECIMAL(10,2) NOT NULL,
    ChangedAt   DATETIME2(3)  NOT NULL CONSTRAINT DF_PPH_ChangedAt DEFAULT SYSDATETIME(),
    ChangedBy   SYSNAME       NULL
);
GO

-- T1) Chuẩn hóa SKU & ProductName
IF OBJECT_ID('dbo.TR_Products_Normalize','TR') IS NOT NULL DROP TRIGGER dbo.TR_Products_Normalize;
GO
CREATE TRIGGER dbo.TR_Products_Normalize
ON dbo.Products
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    -- Chuẩn hóa SKU: trim + upper; Tên: trim
    UPDATE p
       SET p.SKU        = UPPER(LTRIM(RTRIM(p.SKU))),
           p.ProductName= LTRIM(RTRIM(p.ProductName))
    FROM dbo.Products p
    JOIN inserted i ON i.ProductID = p.ProductID;
END
GO

-- T2) Log lịch sử đổi giá bán
IF OBJECT_ID('dbo.TR_Product_LogPriceChange','TR') IS NOT NULL DROP TRIGGER dbo.TR_Product_LogPriceChange;
GO
CREATE TRIGGER dbo.TR_Product_LogPriceChange
ON dbo.Products
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.ProductPriceHistory(ProductID, OldPrice, NewPrice, ChangedBy)
    SELECT i.ProductID, d.SellingPrice, i.SellingPrice, SUSER_SNAME()
    FROM inserted i
    JOIN deleted  d ON d.ProductID = i.ProductID
    WHERE ISNULL(i.SellingPrice,0) <> ISNULL(d.SellingPrice,0);
END
GO

-- T3) Không cho xóa Category nếu đang được dùng (override CASCADE)
IF OBJECT_ID('dbo.TR_Categories_PreventDeleteIfInUse','TR') IS NOT NULL DROP TRIGGER dbo.TR_Categories_PreventDeleteIfInUse;
GO
CREATE TRIGGER dbo.TR_Categories_PreventDeleteIfInUse
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
END
GO

/* =========================
   (C) TEST TRIGGERS NHANH
========================= */

-- Test T1: Normalize
INSERT INTO dbo.Products(SKU, ProductName, SellingPrice, StockQuantity)
VALUES ('  sp_x01  ', N'  Bánh Mới  ', 9.99, 1);
SELECT SKU, ProductName FROM dbo.Products WHERE SKU LIKE '%X01%';

-- Test T2: Price history
UPDATE dbo.Products SET SellingPrice = SellingPrice + 0.11 WHERE SKU='SP0001';
SELECT TOP 5 * FROM dbo.ProductPriceHistory ORDER BY HistoryID DESC;

-- Test T3: Prevent delete category đang dùng
BEGIN TRY
    DELETE FROM dbo.Categories WHERE CategoryName = N'Dairy';
END TRY
BEGIN CATCH
    SELECT ERROR_NUMBER() AS ErrNo, ERROR_MESSAGE() AS ErrMsg; -- mong đợi bị chặn
END CATCH;
-- Thử xóa category không dùng
IF NOT EXISTS(SELECT 1 FROM dbo.Categories WHERE CategoryName=N'TempCat') 
    INSERT INTO dbo.Categories(CategoryName) VALUES (N'TempCat');
DELETE FROM dbo.Categories WHERE CategoryName=N'TempCat';
SELECT CategoryName FROM dbo.Categories WHERE CategoryName=N'TempCat'; -- không còn

-- Test T4: DDL chặn DROP bảng lõi (sẽ bị chặn)
BEGIN TRY
    DROP TABLE dbo.Products;
END TRY
BEGIN CATCH
    SELECT ERROR_NUMBER() AS ErrNo, ERROR_MESSAGE() AS ErrMsg; -- mong đợi bị chặn
END CATCH;
GO
USE QLNhapHang;
GO

-- Drop đúng cách cho DDL trigger (không prefix schema)
IF EXISTS (
    SELECT 1
    FROM sys.triggers
    WHERE name = 'TR_DDL_PreventDrop' AND parent_class_desc = 'DATABASE'
)
    DROP TRIGGER TR_DDL_PreventDrop ON DATABASE;
GO

-- Tạo lại DDL trigger (không prefix schema)
CREATE TRIGGER TR_DDL_PreventDrop
ON DATABASE
FOR DROP_TABLE, ALTER_TABLE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @obj    SYSNAME = EVENTDATA().value('(/EVENT_INSTANCE/ObjectName)[1]','SYSNAME');
    DECLARE @schema SYSNAME = EVENTDATA().value('(/EVENT_INSTANCE/SchemaName)[1]','SYSNAME');

    IF @schema = 'dbo' AND @obj IN
       ('Products','Categories','ProductCategories','Suppliers','GoodsReceipts','GoodsReceiptDetails','Users','Roles','UsersRoles')
    BEGIN
        RAISERROR(N'DDL bị chặn: không được DROP/ALTER bảng lõi %s.%s.', 16, 1, @schema, @obj);
        ROLLBACK;
    END
END
GO
BEGIN TRY
    DROP TABLE dbo.Products;   -- kỳ vọng: bị chặn
END TRY
BEGIN CATCH
    SELECT ERROR_NUMBER() AS ErrNo, ERROR_MESSAGE() AS ErrMsg;
END CATCH;
-- 1) Liệt kê & đếm trigger (cả table-level và database-level)
SELECT name, parent_class_desc, type_desc
FROM sys.triggers
WHERE name LIKE 'TR_%';

-- 2) Đếm hàm: 5 TVF + 5 scalar
SELECT
  TVF    = SUM(CASE WHEN type IN ('IF','TF') THEN 1 ELSE 0 END),
  Scalar = SUM(CASE WHEN type = 'FN' THEN 1 ELSE 0 END)
FROM sys.objects
WHERE type IN ('IF','TF','FN');

-- 3) Xem danh sách TVF & Scalar hiện có
SELECT type_desc, name
FROM sys.objects
WHERE type IN ('IF','TF','FN')
ORDER BY type_desc, name;
