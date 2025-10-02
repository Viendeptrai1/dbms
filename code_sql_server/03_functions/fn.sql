USE QLNhapHang;
GO

CREATE FUNCTION dbo.fn_TotalStock(@ProductID INT)
RETURNS INT
AS
BEGIN
    DECLARE @qty INT;
    SELECT @qty = p.StockQuantity FROM dbo.Products p WHERE p.ProductID = @ProductID;
    RETURN @qty;
END
GO

-- TEST 1
DECLARE @pid INT = (SELECT TOP(1) ProductID FROM dbo.Products WHERE SKU='SP0001');
SELECT SKU=(SELECT SKU FROM dbo.Products WHERE ProductID=@pid),
       TotalStock=dbo.fn_TotalStock(@pid);
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
    RETURN @price; -- có thể là NULL nếu chưa từng nhập
END
GO

-- TEST 2
SELECT SKU, LastImportPrice = dbo.fn_LastImportPrice(ProductID)
FROM dbo.Products;
GO

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
    JOIN dbo.Suppliers s     ON s.SupplierID = r.SupplierID
    WHERE d.ProductID = @ProductID
);
GO

-- TEST 3
SELECT * 
FROM dbo.fn_ProductImportHistory((SELECT ProductID FROM dbo.Products WHERE SKU='SP0001'))
ORDER BY ReceiptDate DESC;
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

-- TEST 4
SELECT * FROM dbo.fn_ProductsByCategory(N'Dairy');
GO

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

-- TEST 5
SELECT * FROM dbo.vw_ProductsWithCategories ORDER BY SKU;
GO
