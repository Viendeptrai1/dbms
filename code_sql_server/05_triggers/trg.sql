USE QLNhapHang;
GO

IF OBJECT_ID('dbo.TR_GRD_StockAndTotal','TR') IS NOT NULL
    DROP TRIGGER dbo.TR_GRD_StockAndTotal;
GO

CREATE TRIGGER dbo.TR_GRD_StockAndTotal
ON dbo.GoodsReceiptDetails
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    /* ===== 1) Tính delta tồn kho theo ProductID ===== */
    DECLARE @prodDelta TABLE(
        ProductID INT PRIMARY KEY,
        Qty       INT NOT NULL
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
        ReceiptID BIGINT PRIMARY KEY
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
END
GO


-- Lấy 1 product
DECLARE @pid INT = (SELECT ProductID FROM dbo.Products WHERE SKU='SP0001'); -- Milk

PRINT 'Before:';
SELECT StockBefore = StockQuantity FROM dbo.Products WHERE ProductID=@pid;

-- Tạo phiếu trống
INSERT INTO dbo.GoodsReceipts(SupplierID, UserID)
VALUES (
  (SELECT TOP(1) SupplierID FROM dbo.Suppliers ORDER BY SupplierID),
  (SELECT TOP(1) UserID FROM dbo.Users WHERE Username='seller01')
);
DECLARE @rid BIGINT = SCOPE_IDENTITY();

-- INSERT detail -> tăng tồn & tổng tiền
INSERT INTO dbo.GoodsReceiptDetails(ReceiptID, ProductID, Quantity, ImportPrice, ExpiryDate)
VALUES (@rid, @pid, 10, 16.00, NULL);

PRINT 'After INSERT:';
SELECT StockAfterInsert = StockQuantity FROM dbo.Products WHERE ProductID=@pid;
SELECT TotalAfterInsert = TotalAmount FROM dbo.GoodsReceipts WHERE ReceiptID=@rid;

-- UPDATE detail (10 -> 12) -> tăng tồn thêm 2, tổng tiền cộng thêm 32
UPDATE dbo.GoodsReceiptDetails
SET Quantity = 12
WHERE ReceiptID=@rid AND ProductID=@pid;

PRINT 'After UPDATE:';
SELECT StockAfterUpdate = StockQuantity FROM dbo.Products WHERE ProductID=@pid;
SELECT TotalAfterUpdate = TotalAmount FROM dbo.GoodsReceipts WHERE ReceiptID=@rid;

-- DELETE detail -> trả tồn, tổng về 0
DELETE dbo.GoodsReceiptDetails WHERE ReceiptID=@rid AND ProductID=@pid;

PRINT 'After DELETE:';
SELECT StockAfterDelete = StockQuantity FROM dbo.Products WHERE ProductID=@pid;
SELECT TotalAfterDelete = TotalAmount FROM dbo.GoodsReceipts WHERE ReceiptID=@rid;

-- Dọn phiếu
DELETE dbo.GoodsReceipts WHERE ReceiptID=@rid;
