USE QLNhapHang;
GO

/* 0) Dọn dẹp đúng thứ tự */
IF OBJECT_ID('dbo.sp_AddProductWithCategories','P') IS NOT NULL
    DROP PROCEDURE dbo.sp_AddProductWithCategories;
GO
IF TYPE_ID('dbo.udt_CategoryNameList') IS NOT NULL
    DROP TYPE dbo.udt_CategoryNameList;
GO

/* 1) TVP: danh sách tên category */
CREATE TYPE dbo.udt_CategoryNameList AS TABLE(
    CategoryName NVARCHAR(100) NOT NULL PRIMARY KEY
);
GO

/* 2) Proc: thêm Product + gán Category (tạo nếu thiếu), set tồn khởi tạo */
CREATE PROCEDURE dbo.sp_AddProductWithCategories
    @SKU           VARCHAR(32),
    @ProductName   NVARCHAR(200),
    @SellingPrice  DECIMAL(10,2),
    @InitialStock  INT = 0,
    @CategoryNames dbo.udt_CategoryNameList READONLY,
    @NewProductID  INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        IF EXISTS (SELECT 1 FROM dbo.Products WHERE SKU = @SKU)
            THROW 53000, N'SKU đã tồn tại.', 1;

        IF @SellingPrice < 0 OR @InitialStock < 0
            THROW 53001, N'Giá/ tồn khởi tạo không hợp lệ.', 1;

        IF NOT EXISTS (SELECT 1 FROM @CategoryNames)
            THROW 53002, N'Phải cung cấp ít nhất 1 Category.', 1;

        BEGIN TRAN;

        -- Tạo product
        INSERT INTO dbo.Products(SKU, ProductName, SellingPrice, StockQuantity)
        VALUES (@SKU, @ProductName, @SellingPrice, @InitialStock);

        SET @NewProductID = SCOPE_IDENTITY();

        -- Bổ sung category nếu thiếu
        INSERT INTO dbo.Categories(CategoryName)
        SELECT cn.CategoryName
        FROM @CategoryNames cn
        WHERE NOT EXISTS (SELECT 1 FROM dbo.Categories c WHERE c.CategoryName = cn.CategoryName);

        -- Gán product-category (M-N)
        INSERT INTO dbo.ProductCategories(ProductID, CategoryID)
        SELECT @NewProductID, c.CategoryID
        FROM dbo.Categories c
        JOIN @CategoryNames cn ON cn.CategoryName = c.CategoryName;

        COMMIT;

        -- Kết quả
        SELECT p.ProductID, p.SKU, p.ProductName, p.SellingPrice, p.StockQuantity
        FROM dbo.Products p WHERE p.ProductID = @NewProductID;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK;
        THROW;
    END CATCH
END
GO

/* TEST 4C: tạo sản phẩm mới + gán nhiều category */
DECLARE @cats dbo.udt_CategoryNameList;
INSERT INTO @cats(CategoryName) VALUES (N'Dairy'), (N'Snacks'); -- có, không sao; trùng sẽ bỏ qua
DECLARE @newid INT;
EXEC dbo.sp_AddProductWithCategories
    @SKU = 'SP0004',
    @ProductName  = N'Yogurt Cup',
    @SellingPrice = 10.50,
    @InitialStock = 5,
    @CategoryNames= @cats,
    @NewProductID = @newid OUTPUT;

SELECT NewProductID=@newid;

-- Kiểm tra M-N
SELECT p.SKU, c.CategoryName
FROM dbo.Products p
JOIN dbo.ProductCategories pc ON pc.ProductID=p.ProductID
JOIN dbo.Categories c ON c.CategoryID=pc.CategoryID
WHERE p.ProductID=@newid
ORDER BY c.CategoryName;
GO
