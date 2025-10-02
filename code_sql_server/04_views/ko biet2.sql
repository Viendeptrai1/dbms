USE QLNhapHang;
GO

/* =========================================================
   0) INDEX hỗ trợ lọc theo ngày & join ReceiptID
========================================================= */
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_GR_ReceiptDate' AND object_id=OBJECT_ID('dbo.GoodsReceipts'))
    DROP INDEX IX_GR_ReceiptDate ON dbo.GoodsReceipts;
GO
CREATE INDEX IX_GR_ReceiptDate
ON dbo.GoodsReceipts(ReceiptDate)
INCLUDE (SupplierID, UserID, TotalAmount);
GO

IF EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_GRD_ReceiptID' AND object_id=OBJECT_ID('dbo.GoodsReceiptDetails'))
    DROP INDEX IX_GRD_ReceiptID ON dbo.GoodsReceiptDetails;
GO
CREATE INDEX IX_GRD_ReceiptID
ON dbo.GoodsReceiptDetails(ReceiptID)
INCLUDE (ProductID, Quantity, ImportPrice);
GO

/* =========================================================
   1) VIEW: Dòng nhập (Import Lines) — tiện tra cứu/báo cáo
========================================================= */
IF OBJECT_ID('dbo.vw_ImportLines','V') IS NOT NULL
    DROP VIEW dbo.vw_ImportLines;
GO
CREATE VIEW dbo.vw_ImportLines
AS
SELECT
    r.ReceiptID,
    r.ReceiptDate,
    s.SupplierName,
    d.ProductID,
    p.SKU,
    p.ProductName,
    d.Quantity,
    d.ImportPrice,
    LineAmount = CAST(d.Quantity AS DECIMAL(18,2)) * d.ImportPrice
FROM dbo.GoodsReceiptDetails d
JOIN dbo.GoodsReceipts r ON r.ReceiptID = d.ReceiptID
JOIN dbo.Suppliers     s ON s.SupplierID = r.SupplierID
JOIN dbo.Products      p ON p.ProductID = d.ProductID;
GO

-- TEST VIEW
SELECT TOP (10) * FROM dbo.vw_ImportLines ORDER BY ReceiptDate DESC, ReceiptID DESC;
GO

/* =========================================================
   2) TVF: Top-N sản phẩm theo giá trị nhập trong khoảng thời gian
      - @From: inclusive
      - @To  : exclusive (NULL => không giới hạn trên)
      - @SupplierName, @CategoryName: có thể NULL để bỏ lọc
========================================================= */
IF OBJECT_ID('dbo.fn_TopProductsByImportValue','IF') IS NOT NULL
    DROP FUNCTION dbo.fn_TopProductsByImportValue;
GO
CREATE FUNCTION dbo.fn_TopProductsByImportValue
(
    @From          DATETIME2(3),
    @To            DATETIME2(3) = NULL,
    @TopN          INT          = 10,
    @SupplierName  NVARCHAR(200) = NULL,
    @CategoryName  NVARCHAR(100) = NULL
)
RETURNS TABLE
AS
RETURN
(
    WITH base AS (
        SELECT
            d.ProductID,
            TotalQty    = SUM(d.Quantity),
            ImportValue = SUM(CAST(d.Quantity AS DECIMAL(18,2)) * d.ImportPrice)
        FROM dbo.GoodsReceiptDetails d
        JOIN dbo.GoodsReceipts r ON r.ReceiptID = d.ReceiptID
        WHERE r.ReceiptDate >= @From
          AND (@To IS NULL OR r.ReceiptDate < @To)
          AND (
                @SupplierName IS NULL
                OR EXISTS (SELECT 1
                           FROM dbo.Suppliers s
                           WHERE s.SupplierID = r.SupplierID
                             AND s.SupplierName = @SupplierName)
              )
          AND (
                @CategoryName IS NULL
                OR EXISTS (SELECT 1
                           FROM dbo.ProductCategories pc
                           JOIN dbo.Categories c ON c.CategoryID = pc.CategoryID
                           WHERE pc.ProductID = d.ProductID
                             AND c.CategoryName = @CategoryName)
              )
        GROUP BY d.ProductID
    )
    SELECT TOP (@TopN)
        p.ProductID,
        p.SKU,
        p.ProductName,
        b.TotalQty,
        b.ImportValue
    FROM base b
    JOIN dbo.Products p ON p.ProductID = b.ProductID
    ORDER BY b.ImportValue DESC, p.SKU
);
GO

/* TEST 2A: Top-5 trong 7 ngày gần đây */
DECLARE @from DATETIME2(3) = DATEADD(DAY, -7, SYSDATETIME());
DECLARE @to   DATETIME2(3) = SYSDATETIME();
SELECT * FROM dbo.fn_TopProductsByImportValue(@from, @to, 5, NULL, NULL);

/* TEST 2B: Lọc theo Category = 'Dairy' */
SELECT * FROM dbo.fn_TopProductsByImportValue(@from, @to, 5, NULL, N'Dairy');

/* TEST 2C: Lọc theo Supplier = 'ABC Supply' */
SELECT * FROM dbo.fn_TopProductsByImportValue(@from, @to, 5, N'ABC Supply', NULL);
GO

/* =========================================================
   3) TVF: Tổng hợp theo Nhà cung cấp trong khoảng thời gian
========================================================= */
IF OBJECT_ID('dbo.fn_ImportSummaryBySupplier','IF') IS NOT NULL
    DROP FUNCTION dbo.fn_ImportSummaryBySupplier;
GO
CREATE FUNCTION dbo.fn_ImportSummaryBySupplier
(
    @From DATETIME2(3),
    @To   DATETIME2(3) = NULL
)
RETURNS TABLE
AS
RETURN
(
    SELECT
        s.SupplierName,
        TotalReceipts = COUNT(DISTINCT r.ReceiptID),
        TotalQty      = SUM(d.Quantity),
        TotalValue    = SUM(CAST(d.Quantity AS DECIMAL(18,2)) * d.ImportPrice)
    FROM dbo.GoodsReceiptDetails d
    JOIN dbo.GoodsReceipts r ON r.ReceiptID = d.ReceiptID
    JOIN dbo.Suppliers     s ON s.SupplierID = r.SupplierID
    WHERE r.ReceiptDate >= @From
      AND (@To IS NULL OR r.ReceiptDate < @To)
    GROUP BY s.SupplierName
);
GO

-- TEST 3: Tổng hợp theo nhà cung cấp (7 ngày gần đây)
DECLARE @from2 DATETIME2(3) = DATEADD(DAY, -7, SYSDATETIME());
DECLARE @to2   DATETIME2(3) = SYSDATETIME();
SELECT * FROM dbo.fn_ImportSummaryBySupplier(@from2, @to2) ORDER BY TotalValue DESC;
GO
