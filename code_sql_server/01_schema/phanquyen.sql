USE QLNhapHang;
GO

/* =========================================================
   PHA 7 — PHÂN QUYỀN Ở MỨC DB
   - Tạo 3 database roles: Admin, Seller, User
   - Cấp quyền đối tượng (tables, views, TVF, scalar FN, stored procs)
   - Cấp quyền trên TYPE (TVP) để gọi proc có tham số bảng
   - Tạo 3 user không login để TEST và add vào roles
========================================================= */

-------------------------
-- 0) TẠO DATABASE ROLES
-------------------------
IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name='dbrole_Admin')
    CREATE ROLE dbrole_Admin;
IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name='dbrole_Seller')
    CREATE ROLE dbrole_Seller;
IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name='dbrole_User')
    CREATE ROLE dbrole_User;
GO

-----------------------------------------
-- 1) GRANT CHO ADMIN (full trên schema)
-----------------------------------------
GRANT CONTROL ON SCHEMA::dbo TO dbrole_Admin;
GO

/* ----------------------------------------
   2) GRANT CHO SELLER (đủ nghiệp vụ)
   - Đọc dữ liệu chính
   - UPDATE một số cột của Products
   - SELECT các VIEW/TVF báo cáo
   - EXEC scalar FN, stored procedures
   - QUAN TRỌNG: cấp quyền trên TYPE (TVP)
---------------------------------------- */
-- Bảng chính
GRANT SELECT ON OBJECT::dbo.Products             TO dbrole_Seller;
GRANT SELECT ON OBJECT::dbo.Categories           TO dbrole_Seller;
GRANT SELECT ON OBJECT::dbo.ProductCategories    TO dbrole_Seller;
GRANT SELECT ON OBJECT::dbo.Suppliers            TO dbrole_Seller;
GRANT SELECT ON OBJECT::dbo.GoodsReceipts        TO dbrole_Seller;
GRANT SELECT ON OBJECT::dbo.GoodsReceiptDetails  TO dbrole_Seller;

-- UPDATE giới hạn cột
GRANT UPDATE (StockQuantity, SellingPrice) ON OBJECT::dbo.Products TO dbrole_Seller;

-- Views
GRANT SELECT ON OBJECT::dbo.vw_ProductsWithCategories TO dbrole_Seller;
GRANT SELECT ON OBJECT::dbo.vw_InventoryValuation     TO dbrole_Seller;
GRANT SELECT ON OBJECT::dbo.vw_ImportLines            TO dbrole_Seller;

-- TVF (bảng)
GRANT SELECT ON OBJECT::dbo.fn_ProductsByCategory       TO dbrole_Seller;
GRANT SELECT ON OBJECT::dbo.fn_ProductImportHistory     TO dbrole_Seller;
GRANT SELECT ON OBJECT::dbo.fn_TopProductsByImportValue TO dbrole_Seller;
GRANT SELECT ON OBJECT::dbo.fn_ImportSummaryBySupplier  TO dbrole_Seller;

-- Scalar functions
GRANT EXECUTE ON OBJECT::dbo.fn_TotalStock            TO dbrole_Seller;
GRANT EXECUTE ON OBJECT::dbo.fn_LastImportPrice       TO dbrole_Seller;
GRANT EXECUTE ON OBJECT::dbo.fn_ProductInventoryValue TO dbrole_Seller;

-- Stored procedures
GRANT EXECUTE ON OBJECT::dbo.sp_CreateGoodsReceipt         TO dbrole_Seller;
GRANT EXECUTE ON OBJECT::dbo.sp_BulkAdjustPriceByPercent   TO dbrole_Seller;
GRANT EXECUTE ON OBJECT::dbo.sp_AddProductWithCategories   TO dbrole_Seller;
GRANT EXECUTE ON OBJECT::dbo.sp_DeleteGoodsReceipt         TO dbrole_Seller;

-- QUYỀN TRÊN TYPE (TVP) — bắt buộc để dùng tham số bảng
GRANT EXECUTE   ON TYPE::dbo.udt_GoodsReceiptLine   TO dbrole_Seller;
GRANT REFERENCES ON TYPE::dbo.udt_GoodsReceiptLine  TO dbrole_Seller;

GRANT EXECUTE   ON TYPE::dbo.udt_SKUList            TO dbrole_Seller;
GRANT REFERENCES ON TYPE::dbo.udt_SKUList           TO dbrole_Seller;

GRANT EXECUTE   ON TYPE::dbo.udt_CategoryNameList   TO dbrole_Seller;
GRANT REFERENCES ON TYPE::dbo.udt_CategoryNameList  TO dbrole_Seller;
GO

/* ----------------------------------------
   3) GRANT CHO USER (chỉ đọc)
---------------------------------------- */
GRANT SELECT ON OBJECT::dbo.Products                  TO dbrole_User;
GRANT SELECT ON OBJECT::dbo.Categories                TO dbrole_User;
GRANT SELECT ON OBJECT::dbo.vw_ProductsWithCategories TO dbrole_User;
GRANT SELECT ON OBJECT::dbo.fn_ProductsByCategory     TO dbrole_User;   -- TVF (bảng)
-- (có thể thêm view/TVF khác nếu muốn)
GO

/* ----------------------------------------
   4) TẠO USER TEST (không login) & ADD ROLE
---------------------------------------- */
IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name='admin_test')
    CREATE USER admin_test WITHOUT LOGIN;
IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name='seller_test')
    CREATE USER seller_test WITHOUT LOGIN;
IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name='user_test')
    CREATE USER user_test WITHOUT LOGIN;

-- Gán vào roles
IF NOT EXISTS (
    SELECT 1 FROM sys.database_role_members rm
    JOIN sys.database_principals r ON r.principal_id = rm.role_principal_id AND r.name='dbrole_Admin'
    JOIN sys.database_principals u ON u.principal_id = rm.member_principal_id AND u.name='admin_test'
)
    ALTER ROLE dbrole_Admin ADD MEMBER admin_test;

IF NOT EXISTS (
    SELECT 1 FROM sys.database_role_members rm
    JOIN sys.database_principals r ON r.principal_id = rm.role_principal_id AND r.name='dbrole_Seller'
    JOIN sys.database_principals u ON u.principal_id = rm.member_principal_id AND u.name='seller_test'
)
    ALTER ROLE dbrole_Seller ADD MEMBER seller_test;

IF NOT EXISTS (
    SELECT 1 FROM sys.database_role_members rm
    JOIN sys.database_principals r ON r.principal_id = rm.role_principal_id AND r.name='dbrole_User'
    JOIN sys.database_principals u ON u.principal_id = rm.member_principal_id AND u.name='user_test'
)
    ALTER ROLE dbrole_User ADD MEMBER user_test;
GO

/* =========================================================
   TEST QUYỀN
========================================================= */

-- TEST A — Admin: có thể UPDATE tự do
PRINT '--- TEST A: Admin ---';
EXECUTE AS USER = 'admin_test';
-- Thử update tên sản phẩm (Admin được quyền CONTROL schema)
UPDATE dbo.Products SET ProductName = ProductName WHERE SKU='SP0001';
SELECT TOP 1 'OK_Admin_Update_ProductName' AS Result FROM dbo.Products WHERE SKU='SP0001';
REVERT;
GO

-- TEST B — Seller:
--  - gọi proc tạo phiếu (OK)
--  - update SellingPrice (OK)
--  - update ProductName (BỊ CHẶN)
PRINT '--- TEST B: Seller ---';
EXECUTE AS USER = 'seller_test';

DECLARE @lines dbo.udt_GoodsReceiptLine;
INSERT INTO @lines(ProductSKU, Quantity, ImportPrice) VALUES ('SP0002', 2, 4.00);
DECLARE @rid BIGINT;
EXEC dbo.sp_CreateGoodsReceipt N'ABC Supply','seller01',NULL,@lines,@rid OUTPUT;
SELECT CreatedReceiptID=@rid;

-- Được phép cập nhật giá bán
UPDATE dbo.Products SET SellingPrice = SellingPrice + 0.01 WHERE SKU='SP0002';
SELECT 'OK_Seller_Update_Price' AS Result, SKU, SellingPrice FROM dbo.Products WHERE SKU='SP0002';

-- Không được phép sửa tên sản phẩm
BEGIN TRY
    UPDATE dbo.Products SET ProductName = N'Thu Nghiem' WHERE SKU='SP0002';
END TRY
BEGIN CATCH
    SELECT
        'DENIED_Seller_Update_ProductName' AS Result,
        ERROR_NUMBER()   AS ErrNo,
        ERROR_SEVERITY() AS ErrSev,
        ERROR_STATE()    AS ErrState,
        ERROR_LINE()     AS ErrLine,
        ERROR_MESSAGE()  AS ErrMsg;
END CATCH;

REVERT;
GO

-- TEST C — User:
--  - SELECT (OK)
--  - INSERT (BỊ CHẶN)
PRINT '--- TEST C: User ---';
EXECUTE AS USER = 'user_test';
SELECT TOP 3 SKU, ProductName FROM dbo.Products ORDER BY SKU;

BEGIN TRY
    INSERT INTO dbo.Products(SKU, ProductName, SellingPrice, StockQuantity)
    VALUES ('TEST_U1', N'User Try', 1.00, 1);
END TRY
BEGIN CATCH
    SELECT
        'DENIED_User_Insert_Product' AS Result,
        ERROR_NUMBER()   AS ErrNo,
        ERROR_SEVERITY() AS ErrSev,
        ERROR_STATE()    AS ErrState,
        ERROR_LINE()     AS ErrLine,
        ERROR_MESSAGE()  AS ErrMsg;
END CATCH;

REVERT;
GO
