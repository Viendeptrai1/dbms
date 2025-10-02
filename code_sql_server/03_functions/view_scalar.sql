USE QLNhapHang;
GO

/* 1) Scalar: giá trị tồn kho của 1 sản phẩm */
IF OBJECT_ID('dbo.fn_ProductInventoryValue','FN') IS NOT NULL
    DROP FUNCTION dbo.fn_ProductInventoryValue;
GO
CREATE FUNCTION dbo.fn_ProductInventoryValue(@ProductID INT)
RETURNS DECIMAL(18,2)
AS
BEGIN
    DECLARE @qty INT, @sell DECIMAL(10,2), @last DECIMAL(10,2);
    SELECT @qty = StockQuantity, @sell = SellingPrice
    FROM dbo.Products WHERE ProductID = @ProductID;

    SET @last = dbo.fn_LastImportPrice(@ProductID);  -- từ Pha 1
    RETURN CAST(ISNULL(@qty,0) AS DECIMAL(18,2)) * ISNULL(@last, @sell);
END
GO

/* 2) View: báo cáo tồn kho + đơn giá nhập gần nhất */
IF OBJECT_ID('dbo.vw_InventoryValuation','V') IS NOT NULL
    DROP VIEW dbo.vw_InventoryValuation;
GO
CREATE VIEW dbo.vw_InventoryValuation
AS
SELECT
    p.ProductID,
    p.SKU,
    p.ProductName,
    p.StockQuantity,
    LastImportPrice = dbo.fn_LastImportPrice(p.ProductID),
    InventoryValue  = dbo.fn_ProductInventoryValue(p.ProductID)
FROM dbo.Products p;
GO

-- TEST 4A
SELECT * FROM dbo.vw_InventoryValuation ORDER BY SKU;
GO
S