USE QLNhapHang;
GO

/* 0) Làm sạch theo đúng thứ tự: proc -> type */
IF OBJECT_ID('dbo.sp_CreateGoodsReceipt','P') IS NOT NULL
    DROP PROCEDURE dbo.sp_CreateGoodsReceipt;
GO
IF TYPE_ID('dbo.udt_GoodsReceiptLine') IS NOT NULL
    DROP TYPE dbo.udt_GoodsReceiptLine;
GO

/* 1) TVP cho dòng phiếu */
CREATE TYPE dbo.udt_GoodsReceiptLine AS TABLE(
    ProductSKU  VARCHAR(32)   NOT NULL,
    Quantity    INT           NOT NULL CHECK (Quantity > 0),
    ImportPrice DECIMAL(10,2) NOT NULL CHECK (ImportPrice > 0),
    ExpiryDate  DATE          NULL
);
GO

/* 2) Proc tạo phiếu nhập an toàn */
CREATE PROCEDURE dbo.sp_CreateGoodsReceipt
    @SupplierName NVARCHAR(200),
    @Username     VARCHAR(50),
    @ReceiptDate  DATETIME2(3) = NULL,
    @Lines        dbo.udt_GoodsReceiptLine READONLY,
    @NewReceiptID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        /* --- Validate inputs --- */
        IF NOT EXISTS (SELECT 1 FROM @Lines)
            THROW 51000, N'Danh sách dòng nhập trống.', 1;

        -- SKU trùng trong TVP
        IF EXISTS (
            SELECT 1
            FROM @Lines
            GROUP BY ProductSKU
            HAVING COUNT(*) > 1
        )
            THROW 51001, N'TVP Lines có SKU trùng lặp. Hãy gộp trước khi gọi.', 1;

        DECLARE @SupplierID INT = (SELECT SupplierID FROM dbo.Suppliers WHERE SupplierName=@SupplierName);
        IF @SupplierID IS NULL
            THROW 51002, N'Không tìm thấy Supplier theo tên cung cấp.', 1;

        DECLARE @UserID INT = (SELECT UserID FROM dbo.Users WHERE Username=@Username);
        IF @UserID IS NULL
            THROW 51003, N'Không tìm thấy User theo Username.', 1;

        -- SKU không tồn tại trong Products
        IF EXISTS (
            SELECT l.ProductSKU
            FROM @Lines l
            LEFT JOIN dbo.Products p ON p.SKU = l.ProductSKU
            WHERE p.ProductID IS NULL
        )
        BEGIN
            DECLARE @BadSKU NVARCHAR(MAX) =
            (
                SELECT STRING_AGG(l.ProductSKU, ', ')
                FROM @Lines l
                LEFT JOIN dbo.Products p ON p.SKU = l.ProductSKU
                WHERE p.ProductID IS NULL
            );
            THROW 51004, N'Một số SKU không tồn tại trong Products.', 1;
        END

        IF @ReceiptDate IS NULL SET @ReceiptDate = SYSDATETIME();

        /* --- Transactional insert --- */
        BEGIN TRAN;

        INSERT INTO dbo.GoodsReceipts(SupplierID, UserID, ReceiptDate, TotalAmount)
        VALUES (@SupplierID, @UserID, @ReceiptDate, 0);

        SET @NewReceiptID = SCOPE_IDENTITY();

        -- Map SKU -> ProductID vào bảng tạm để insert detail
        DECLARE @Map TABLE(
            ProductID   INT PRIMARY KEY,
            Quantity    INT,
            ImportPrice DECIMAL(10,2),
            ExpiryDate  DATE
        );

        INSERT INTO @Map(ProductID, Quantity, ImportPrice, ExpiryDate)
        SELECT p.ProductID, l.Quantity, l.ImportPrice, l.ExpiryDate
        FROM @Lines l
        JOIN dbo.Products p ON p.SKU = l.ProductSKU;

        -- Chèn chi tiết (trigger từ Pha 2 sẽ tự cập nhật tồn kho & TotalAmount)
        INSERT INTO dbo.GoodsReceiptDetails(ReceiptID, ProductID, Quantity, ImportPrice, ExpiryDate)
        SELECT @NewReceiptID, ProductID, Quantity, ImportPrice, ExpiryDate
        FROM @Map;

        COMMIT;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK;
        -- Ném lại lỗi gốc để xem số/line dễ debug
        THROW;
    END CATCH
END
GO
/* CASE 1: Thành công */
DECLARE @lines dbo.udt_GoodsReceiptLine;
INSERT INTO @lines(ProductSKU, Quantity, ImportPrice, ExpiryDate)
VALUES ('SP0001', 5, 14.50, DATEADD(MONTH, 3, CAST(GETDATE() AS DATE))),
       ('SP0002', 20, 4.50,  NULL);

DECLARE @rid BIGINT;
EXEC dbo.sp_CreateGoodsReceipt
     @SupplierName = N'ABC Supply',
     @Username     = 'seller01',
     @ReceiptDate  = NULL,
     @Lines        = @lines,
     @NewReceiptID = @rid OUTPUT;

SELECT NewReceiptID=@rid;
SELECT * FROM dbo.GoodsReceipts WHERE ReceiptID=@rid;
SELECT * FROM dbo.GoodsReceiptDetails WHERE ReceiptID=@rid ORDER BY ProductID;
SELECT SKU, StockQuantity FROM dbo.Products WHERE SKU IN ('SP0001','SP0002');


/* CASE 2: SKU không tồn tại -> báo lỗi 51004 */
DECLARE @bad dbo.udt_GoodsReceiptLine;
INSERT INTO @bad(ProductSKU, Quantity, ImportPrice, ExpiryDate)
VALUES ('NO_SUCH', 3, 1.00, NULL);

DECLARE @rid2 BIGINT;
BEGIN TRY
  EXEC dbo.sp_CreateGoodsReceipt N'ABC Supply','seller01',NULL,@bad,@rid2 OUTPUT;
END TRY
BEGIN CATCH
  SELECT
    ErrNo   = ERROR_NUMBER(),
    ErrSev  = ERROR_SEVERITY(),
    ErrState= ERROR_STATE(),
    ErrLine = ERROR_LINE(),
    ErrProc = ERROR_PROCEDURE(),
    ErrMsg  = ERROR_MESSAGE();
END CATCH;
GO

/* CASE 3: SKU trùng trong TVP -> báo lỗi 51001 */
DECLARE @dup dbo.udt_GoodsReceiptLine;
INSERT INTO @dup(ProductSKU, Quantity, ImportPrice)
VALUES ('SP0001', 3, 14.0),
       ('SP0001', 2, 14.0);

DECLARE @rid3 BIGINT;
BEGIN TRY
  EXEC dbo.sp_CreateGoodsReceipt N'ABC Supply','seller01',NULL,@dup,@rid3 OUTPUT;
END TRY
BEGIN CATCH
  SELECT
    ErrNo   = ERROR_NUMBER(),
    ErrSev  = ERROR_SEVERITY(),
    ErrState= ERROR_STATE(),
    ErrLine = ERROR_LINE(),
    ErrProc = ERROR_PROCEDURE(),
    ErrMsg  = ERROR_MESSAGE();
END CATCH;
GO
