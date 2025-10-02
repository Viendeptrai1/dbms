USE QLNhapHang;
GO

/* 0) Dọn dẹp */
IF OBJECT_ID('dbo.sp_BulkAdjustPriceByPercent','P') IS NOT NULL
    DROP PROCEDURE dbo.sp_BulkAdjustPriceByPercent;
GO
IF TYPE_ID('dbo.udt_SKUList') IS NOT NULL
    DROP TYPE dbo.udt_SKUList;
GO

/* 1) TVP: danh sách SKU */
CREATE TYPE dbo.udt_SKUList AS TABLE(
    ProductSKU VARCHAR(32) NOT NULL PRIMARY KEY
);
GO

/* 2) Proc: bulk adjust */
CREATE PROCEDURE dbo.sp_BulkAdjustPriceByPercent
    @DeltaPercent DECIMAL(6,2),                -- ví dụ: +10 => tăng 10%, -5 => giảm 5%
    @CategoryName NVARCHAR(100) = NULL,        -- có thể NULL
    @SKUs        dbo.udt_SKUList READONLY      -- có thể rỗng
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        IF ISNULL(@DeltaPercent,0) = 0
            THROW 52000, N'@DeltaPercent phải khác 0.', 1;

        -- Tập sản phẩm mục tiêu
        DECLARE @Targets TABLE(ProductID INT PRIMARY KEY);

        -- Theo Category (nếu có)
        IF @CategoryName IS NOT NULL
        INSERT INTO @Targets(ProductID)
        SELECT DISTINCT p.ProductID
        FROM dbo.Products p
        JOIN dbo.ProductCategories pc ON pc.ProductID = p.ProductID
        JOIN dbo.Categories c ON c.CategoryID = pc.CategoryID
        WHERE c.CategoryName = @CategoryName;

        -- Theo SKU list
        INSERT INTO @Targets(ProductID)
        SELECT p.ProductID
        FROM @SKUs s
        JOIN dbo.Products p ON p.SKU = s.ProductSKU
        WHERE NOT EXISTS (
            SELECT 1 FROM @Targets t WHERE t.ProductID = p.ProductID
        );

        IF NOT EXISTS (SELECT 1 FROM @Targets)
            THROW 52001, N'Không có sản phẩm mục tiêu (Category/ SKU không khớp).', 1;

        BEGIN TRAN;

        -- Cập nhật giá (làm tròn 2 số)
        UPDATE p
           SET p.SellingPrice = ROUND(p.SellingPrice * (1 + (@DeltaPercent/100.0)), 2)
        FROM dbo.Products p
        JOIN @Targets t ON t.ProductID = p.ProductID;

        -- Không cho âm giá
        IF EXISTS (SELECT 1 FROM dbo.Products p JOIN @Targets t ON t.ProductID=p.ProductID WHERE p.SellingPrice < 0)
            THROW 52002, N'Kết quả cập nhật tạo giá âm.', 1;

        COMMIT;

        -- Trả kết quả
        SELECT p.ProductID, p.SKU, p.ProductName, p.SellingPrice
        FROM dbo.Products p
        JOIN @Targets t ON t.ProductID = p.ProductID
        ORDER BY p.SKU;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK;
        THROW;
    END CATCH
END
GO

/* TEST 4B.1: tăng 10% cho Category 'Dairy' */
DECLARE @none dbo.udt_SKUList; -- rỗng
EXEC dbo.sp_BulkAdjustPriceByPercent
    @DeltaPercent = 10,
    @CategoryName = N'Dairy',
    @SKUs = @none;
GO

/* TEST 4B.2: giảm 5% cho 2 SKU cụ thể */
DECLARE @list dbo.udt_SKUList;
INSERT INTO @list(ProductSKU) VALUES ('SP0002'), ('SP0003');
EXEC dbo.sp_BulkAdjustPriceByPercent
    @DeltaPercent = -5,
    @CategoryName = NULL,
    @SKUs = @list;
GO
