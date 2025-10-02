USE QLNhapHang;
GO

/* 0) Tạo database roles */
IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name='dbrole_Admin')
    CREATE ROLE dbrole_Admin;
IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name='dbrole_Seller')
    CREATE ROLE dbrole_Seller;
IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name='dbrole_User')
    CREATE ROLE dbrole_User;
GO

/* 1) GRANT cho Admin: full quyền trên schema dbo (gọn, dễ demo) */
GRANT CONTROL ON SCHEMA::dbo TO dbrole_Admin;
GO

/* 2) GRANT cho Seller: đọc dữ liệu chính, chạy các proc nghiệp vụ, 
      chỉ được UPDATE một số cột của Products (StockQuantity, SellingPrice) */
GRANT SELECT ON OBJECT::dbo.Products           TO dbrole_Seller;
GRANT SELECT ON OBJECT::dbo.Categories         TO dbrole_Seller;
GRANT SELECT ON OBJECT::dbo.ProductCategories  TO dbrole_Seller;
GRANT SELECT ON OBJECT::dbo.Suppliers          TO dbrole_Seller;
GRANT SELECT ON OBJECT::dbo.GoodsReceipts      TO dbrole_Seller;
GRANT SELECT ON OBJECT::dbo.GoodsReceiptDetails TO dbrole_Seller;

-- UPDATE hạn chế cột
GRANT UPDATE (StockQuantity, SellingPrice) ON OBJECT::dbo.Products TO dbrole_Seller;

-- Views/Functions phục vụ nghiệp vụ
GRANT SELECT  ON OBJECT::dbo.vw_ProductsWithCategories TO dbrole_Seller;
GRANT SELECT  ON OBJECT::dbo.vw_InventoryValuation     TO dbrole_Seller;
GRANT SELECT  ON OBJECT::dbo.vw_ImportLines            TO dbrole_Seller;

GRANT SELECT  ON OBJECT::dbo.fn_ProductsByCategory     TO dbrole_Seller;  -- TVF
GRANT SELECT  ON OBJECT::dbo.fn_ProductImportHistory   TO dbrole_Seller;  -- TVF
GRANT SELECT  ON OBJECT::dbo.fn_TopProductsByImportValue TO dbrole_Seller; -- TVF
GRANT SELECT  ON OBJECT::dbo.fn_ImportSummaryBySupplier  TO dbrole_Seller; -- TVF
GRANT EXECUTE ON OBJECT::dbo.fn_TotalStock             TO dbrole_Seller;  -- scalar
GRANT EXECUTE ON OBJECT::dbo.fn_LastImportPrice        TO dbrole_Seller;  -- scalar
GRANT EXECUTE ON OBJECT::dbo.fn_ProductInventoryValue  TO dbrole_Seller;  -- scalar

-- Stored procedures
GRANT EXECUTE ON OBJECT::dbo.sp_CreateGoodsReceipt       TO dbrole_Seller;
GRANT EXECUTE ON OBJECT::dbo.sp_BulkAdjustPriceByPercent TO dbrole_Seller;
GRANT EXECUTE ON OBJECT::dbo.sp_AddProductWithCategories TO dbrole_Seller;
GRANT EXECUTE ON OBJECT::dbo.sp_DeleteGoodsReceipt       TO dbrole_Seller;
GO

/* 3) GRANT cho User: chỉ đọc danh mục & sản phẩm + view gộp */
GRANT SELECT ON OBJECT::dbo.Products                   TO dbrole_User;
GRANT SELECT ON OBJECT::dbo.Categories                 TO dbrole_User;
GRANT SELECT ON OBJECT::dbo.vw_ProductsWithCategories  TO dbrole_User;
GRANT SELECT ON OBJECT::dbo.fn_ProductsByCategory      TO dbrole_User;   -- TVF
-- (có thể thêm các view/TVF khác nếu muốn cho User xem báo cáo)
GO

/* 4) Tạo 3 user DB (không cần login) để test, rồi add vào roles */
IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name='admin_test')
    CREATE USER admin_test WITHOUT LOGIN;
IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name='seller_test')
    CREATE USER seller_test WITHOUT LOGIN;
IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name='user_test')
    CREATE USER user_test WITHOUT LOGIN;

ALTER ROLE dbrole_Admin  ADD MEMBER admin_test;
ALTER ROLE dbrole_Seller ADD MEMBER seller_test;
ALTER ROLE dbrole_User   ADD MEMBER user_test;
GO

/* =========================
   TEST A — Admin: có thể UPDATE tự do
========================= */
PRINT '--- TEST A: Admin ---';
EXECUTE AS USER = 'admin_test';
-- Thử update tên sản phẩm (Admin được)
UPDATE dbo.Products SET ProductName = ProductName WHERE SKU='SP0001';
SELECT TOP 1 'OK_Admin_Update_ProductName' AS Result FROM dbo.Products WHERE SKU='SP0001';
REVERT;
GO

/* =========================
   TEST B — Seller: 
   - chạy proc tạo phiếu (được)
   - update SellingPrice (được)
   - update ProductName (bị chặn)
========================= */
PRINT '--- TEST B: Seller ---';
EXECUTE AS USER = 'seller_test';
-- Gọi proc tạo phiếu nhập (được)
DECLARE @lines dbo.udt_GoodsReceiptLine;
INSERT INTO @lines(ProductSKU, Quantity, ImportPrice) VALUES ('SP0002', 2, 4.00);
DECLARE @rid BIGINT;
EXEC dbo.sp_CreateGoodsReceipt N'ABC Supply','seller01',NULL,@lines,@rid OUTPUT;
SELECT CreatedReceiptID=@rid;

-- Được phép update giá bán
UPDATE dbo.Products SET SellingPrice = SellingPrice + 0.01 WHERE SKU='SP0002';
SELECT 'OK_Seller_Update_Price' AS Result, SKU, SellingPrice FROM dbo.Products WHERE SKU='SP0002';

-- Không được phép sửa tên sản phẩm
BEGIN TRY
    UPDATE dbo.Products SET ProductName = N'Thu Nghiem' WHERE SKU='SP0002';
END TRY
BEGIN CATCH
    SELECT 'DENIED_Seller_Update_ProductName' AS Result, ERROR_NUMBER() AS ErrNo, ERROR_MESSAGE() AS ErrMsg;
END CATCH;

REVERT;
GO

/* =========================
   TEST C — User:
   - SELECT (được)
   - INSERT (bị chặn)
========================= */
PRINT '--- TEST C: User ---';
EXECUTE AS USER = 'user_test';
-- SELECT được
SELECT TOP 3 SKU, ProductName FROM dbo.Products ORDER BY SKU;

-- INSERT bị chặn
BEGIN TRY
    INSERT INTO dbo.Products(SKU, ProductName, SellingPrice, StockQuantity)
    VALUES ('TEST_U1', N'User Try', 1.00, 1);
END TRY
BEGIN CATCH
    SELECT 'DENIED_User_Insert_Product' AS Result, ERROR_NUMBER() AS ErrNo, ERROR_MESSAGE() AS ErrMsg;
END CATCH;

REVERT;
GO
