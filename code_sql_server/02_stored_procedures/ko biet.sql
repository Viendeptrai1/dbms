USE QLNhapHang;
GO

-- Dọn cũ (nếu có)
IF OBJECT_ID('dbo.sp_DeleteGoodsReceipt','P') IS NOT NULL
    DROP PROCEDURE dbo.sp_DeleteGoodsReceipt;
GO

CREATE PROCEDURE dbo.sp_DeleteGoodsReceipt
    @ReceiptID BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM dbo.GoodsReceipts WHERE ReceiptID = @ReceiptID)
            THROW 54000, N'Không tìm thấy phiếu nhập.', 1;

        BEGIN TRAN;

        -- ON DELETE CASCADE sẽ xóa chi tiết; trigger Pha 2 sẽ tự trừ tồn
        DELETE dbo.GoodsReceipts WHERE ReceiptID = @ReceiptID;

        COMMIT;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK;
        THROW;  -- trả lại lỗi gốc (ví dụ: 'Tồn kho âm - thao tác bị hủy.' từ trigger)
    END CATCH
END
GO

-- Tạo 1 phiếu nhỏ cho SP0003 rồi xóa
DECLARE @pid INT = (SELECT ProductID FROM dbo.Products WHERE SKU='SP0003'); -- Green Tea
DECLARE @stock_before INT = (SELECT StockQuantity FROM dbo.Products WHERE ProductID=@pid);

-- Tạo phiếu
DECLARE @lines dbo.udt_GoodsReceiptLine;
INSERT INTO @lines(ProductSKU, Quantity, ImportPrice, ExpiryDate)
VALUES ('SP0003', 3, 8.50, NULL);

DECLARE @rid BIGINT;
EXEC dbo.sp_CreateGoodsReceipt
     @SupplierName=N'ABC Supply', @Username='seller01',
     @ReceiptDate=NULL, @Lines=@lines, @NewReceiptID=@rid OUTPUT;

SELECT CreatedReceiptID=@rid,
       StockAfterCreate = (SELECT StockQuantity FROM dbo.Products WHERE ProductID=@pid);

-- Xóa phiếu -> tồn quay về như cũ
EXEC dbo.sp_DeleteGoodsReceipt @ReceiptID=@rid;

SELECT StockAfterDelete = (SELECT StockQuantity FROM dbo.Products WHERE ProductID=@pid),
       ExpectedOriginal = @stock_before;

-- Tạo phiếu rồi cố xóa khi tồn đang về 0 -> delete sẽ khiến tồn âm => bị chặn
DECLARE @pid2 INT = (SELECT ProductID FROM dbo.Products WHERE SKU='SP0003');

-- Tạo phiếu 5 đơn vị
DECLARE @lines2 dbo.udt_GoodsReceiptLine;
INSERT INTO @lines2(ProductSKU, Quantity, ImportPrice) VALUES ('SP0003', 5, 8.30);

DECLARE @rid2 BIGINT;
EXEC dbo.sp_CreateGoodsReceipt N'ABC Supply','seller01',NULL,@lines2,@rid2 OUTPUT;

-- Hạ tồn về 0 để giả lập đã bán hết
UPDATE dbo.Products SET StockQuantity = 0 WHERE ProductID=@pid2;

-- Thử xóa -> trigger Pha 2 sẽ RAISERROR 'Tồn kho âm - thao tác bị hủy.'
BEGIN TRY
    EXEC dbo.sp_DeleteGoodsReceipt @ReceiptID=@rid2;
END TRY
BEGIN CATCH
    SELECT
        ErrNo    = ERROR_NUMBER(),
        ErrLine  = ERROR_LINE(),
        ErrMsg   = ERROR_MESSAGE();
END CATCH;

-- Phiếu vẫn còn, tồn giữ nguyên 0
SELECT StillThere = COUNT(*) FROM dbo.GoodsReceipts WHERE ReceiptID=@rid2;
SELECT SKU='SP0003', StockNow = StockQuantity FROM dbo.Products WHERE ProductID=@pid2;
