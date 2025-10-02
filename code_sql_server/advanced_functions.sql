/*
=============================================
ADVANCED FUNCTIONS - KHÔNG THAY ĐỔI SCHEMA
=============================================
Mô tả: Functions nâng cao cho phân tích và báo cáo
Kỹ thuật: Window Functions, Statistics, Trend Analysis
Ngày: 2025-10-01
=============================================
*/

USE QLNhapHang;
GO

-- =============================================
-- Function #1: ABC Analysis - Phân loại Pareto
-- Kỹ thuật: Window Functions, PERCENT_RANK, SUM OVER
-- =============================================
CREATE OR ALTER FUNCTION dbo.fn_ProductABCClassification(
    @FromDate DATETIME2(3),
    @ToDate DATETIME2(3)
)
RETURNS TABLE
AS
RETURN
(
    WITH ProductValue AS (
        SELECT 
            p.ProductID,
            p.SKU,
            p.ProductName,
            TotalImportQty = SUM(d.Quantity),
            TotalImportValue = SUM(CAST(d.Quantity AS DECIMAL(18,2)) * d.ImportPrice)
        FROM dbo.Products p
        LEFT JOIN dbo.GoodsReceiptDetails d ON d.ProductID = p.ProductID
        LEFT JOIN dbo.GoodsReceipts r ON r.ReceiptID = d.ReceiptID
        WHERE r.ReceiptDate >= @FromDate AND r.ReceiptDate <= @ToDate
        GROUP BY p.ProductID, p.SKU, p.ProductName
    ),
    RankedProducts AS (
        SELECT 
            *,
            RunningTotal = SUM(TotalImportValue) OVER (ORDER BY TotalImportValue DESC),
            GrandTotal = SUM(TotalImportValue) OVER ()
        FROM ProductValue
    )
    SELECT 
        ProductID,
        SKU,
        ProductName,
        TotalImportQty,
        TotalImportValue,
        PercentOfTotal = CAST((TotalImportValue * 100.0 / NULLIF(GrandTotal, 0)) AS DECIMAL(5,2)),
        CumulativePercent = CAST((RunningTotal * 100.0 / NULLIF(GrandTotal, 0)) AS DECIMAL(5,2)),
        ABCClass = CASE 
            WHEN (RunningTotal * 100.0 / NULLIF(GrandTotal, 0)) <= 80 THEN 'A'
            WHEN (RunningTotal * 100.0 / NULLIF(GrandTotal, 0)) <= 95 THEN 'B'
            ELSE 'C'
        END
    FROM RankedProducts
);
GO

-- =============================================
-- Function #2: Inventory Turnover Rate
-- Kỹ thuật: Window Functions, Rolling Calculations
-- =============================================
CREATE OR ALTER FUNCTION dbo.fn_InventoryTurnoverRate(
    @ProductID INT,
    @Months INT = 3
)
RETURNS TABLE
AS
RETURN
(
    WITH ImportHistory AS (
        SELECT 
            d.Quantity,
            MonthOffset = DATEDIFF(MONTH, r.ReceiptDate, GETDATE())
        FROM dbo.GoodsReceiptDetails d
        JOIN dbo.GoodsReceipts r ON r.ReceiptID = d.ReceiptID
        WHERE d.ProductID = @ProductID
          AND r.ReceiptDate >= DATEADD(MONTH, -@Months, GETDATE())
    )
    SELECT 
        ProductID = @ProductID,
        AnalysisPeriodMonths = @Months,
        TotalImported = SUM(Quantity),
        AvgMonthlyImport = AVG(CAST(Quantity AS DECIMAL(18,2))),
        CurrentStock = (SELECT StockQuantity FROM dbo.Products WHERE ProductID = @ProductID),
        TurnoverRate = CASE 
            WHEN (SELECT StockQuantity FROM dbo.Products WHERE ProductID = @ProductID) > 0
            THEN CAST(SUM(Quantity) AS DECIMAL(18,2)) / 
                 CAST((SELECT StockQuantity FROM dbo.Products WHERE ProductID = @ProductID) AS DECIMAL(18,2))
            ELSE 0
        END,
        DaysOfStock = CASE 
            WHEN SUM(Quantity) > 0
            THEN CAST((SELECT StockQuantity FROM dbo.Products WHERE ProductID = @ProductID) AS DECIMAL(18,2)) 
                 / (SUM(Quantity) / CAST(@Months * 30.0 AS DECIMAL(18,2)))
            ELSE 999
        END
    FROM ImportHistory
);
GO

-- =============================================
-- Function #3: Reorder Point Calculator
-- Kỹ thuật: Statistical Functions (STDEV, AVG)
-- Công thức: Reorder Point = (Avg Daily Usage × Lead Time) + Safety Stock
-- =============================================
CREATE OR ALTER FUNCTION dbo.fn_CalculateReorderPoint(
    @ProductID INT,
    @LeadTimeDays INT = 7,
    @ServiceLevel DECIMAL(5,2) = 95.0
)
RETURNS TABLE
AS
RETURN
(
    WITH DailyUsage AS (
        SELECT 
            r.ReceiptDate,
            DailyQty = SUM(d.Quantity)
        FROM dbo.GoodsReceiptDetails d
        JOIN dbo.GoodsReceipts r ON r.ReceiptID = d.ReceiptID
        WHERE d.ProductID = @ProductID
          AND r.ReceiptDate >= DATEADD(MONTH, -3, GETDATE())
        GROUP BY r.ReceiptDate
    ),
    StatsData AS (
        SELECT 
            AvgDailyUsage = AVG(CAST(DailyQty AS DECIMAL(18,2))),
            StdDevDailyUsage = STDEV(CAST(DailyQty AS DECIMAL(18,2)))
        FROM DailyUsage
    )
    SELECT 
        ProductID = @ProductID,
        LeadTimeDays = @LeadTimeDays,
        ServiceLevel = @ServiceLevel,
        AvgDailyUsage = ISNULL(AvgDailyUsage, 0),
        StdDevDailyUsage = ISNULL(StdDevDailyUsage, 0),
        ZScore = CASE 
            WHEN @ServiceLevel >= 99 THEN 2.326
            WHEN @ServiceLevel >= 95 THEN 1.645
            WHEN @ServiceLevel >= 90 THEN 1.282
            ELSE 1.0
        END,
        SafetyStock = ISNULL(
            CASE 
                WHEN @ServiceLevel >= 99 THEN 2.326
                WHEN @ServiceLevel >= 95 THEN 1.645
                WHEN @ServiceLevel >= 90 THEN 1.282
                ELSE 1.0
            END * StdDevDailyUsage * SQRT(CAST(@LeadTimeDays AS FLOAT)), 0),
        ReorderPoint = CAST(
            ISNULL(AvgDailyUsage * @LeadTimeDays, 0) + 
            ISNULL(CASE 
                WHEN @ServiceLevel >= 99 THEN 2.326
                WHEN @ServiceLevel >= 95 THEN 1.645
                WHEN @ServiceLevel >= 90 THEN 1.282
                ELSE 1.0
            END * StdDevDailyUsage * SQRT(CAST(@LeadTimeDays AS FLOAT)), 0)
        AS INT),
        CurrentStock = (SELECT StockQuantity FROM dbo.Products WHERE ProductID = @ProductID),
        ShouldReorder = CASE 
            WHEN (SELECT StockQuantity FROM dbo.Products WHERE ProductID = @ProductID) <= 
                 CAST(ISNULL(AvgDailyUsage * @LeadTimeDays, 0) + 
                      ISNULL(CASE 
                          WHEN @ServiceLevel >= 99 THEN 2.326
                          WHEN @ServiceLevel >= 95 THEN 1.645
                          WHEN @ServiceLevel >= 90 THEN 1.282
                          ELSE 1.0
                      END * StdDevDailyUsage * SQRT(CAST(@LeadTimeDays AS FLOAT)), 0) AS INT)
            THEN 1 
            ELSE 0 
        END
    FROM StatsData
);
GO

-- =============================================
-- Function #4: Supplier Performance Score
-- Kỹ thuật: STDDEV, Complex Scoring Algorithm
-- =============================================
CREATE OR ALTER FUNCTION dbo.fn_SupplierPerformanceScore(
    @SupplierID INT,
    @Months INT = 6
)
RETURNS TABLE
AS
RETURN
(
    WITH ReceiptDates AS (
        SELECT DISTINCT
            r.ReceiptDate,
            PrevReceiptDate = LAG(r.ReceiptDate) OVER (ORDER BY r.ReceiptDate)
        FROM dbo.GoodsReceipts r
        WHERE r.SupplierID = @SupplierID
          AND r.ReceiptDate >= DATEADD(MONTH, -@Months, GETDATE())
    ),
    SupplierStats AS (
        SELECT 
            TotalReceipts = COUNT(DISTINCT r.ReceiptID),
            TotalValue = SUM(r.TotalAmount),
            AvgReceiptValue = AVG(r.TotalAmount),
            TotalProducts = COUNT(DISTINCT d.ProductID),
            PriceVolatility = STDEV(d.ImportPrice),
            AvgImportPrice = AVG(d.ImportPrice),
            DaysBetweenReceipts = (
                SELECT AVG(CAST(DATEDIFF(DAY, PrevReceiptDate, ReceiptDate) AS FLOAT))
                FROM ReceiptDates
                WHERE PrevReceiptDate IS NOT NULL
            )
        FROM dbo.GoodsReceipts r
        JOIN dbo.GoodsReceiptDetails d ON d.ReceiptID = r.ReceiptID
        WHERE r.SupplierID = @SupplierID
          AND r.ReceiptDate >= DATEADD(MONTH, -@Months, GETDATE())
    )
    SELECT 
        SupplierID = @SupplierID,
        TotalReceipts,
        TotalValue,
        AvgReceiptValue,
        TotalProducts,
        PriceVolatility = ISNULL(PriceVolatility, 0),
        VolumeScore = CASE 
            WHEN TotalReceipts >= 20 THEN 30
            WHEN TotalReceipts >= 10 THEN 20
            WHEN TotalReceipts >= 5 THEN 10
            ELSE 5
        END,
        ValueScore = CASE 
            WHEN TotalValue >= 100000000 THEN 30
            WHEN TotalValue >= 50000000 THEN 20
            WHEN TotalValue >= 10000000 THEN 10
            ELSE 5
        END,
        ConsistencyScore = CASE 
            WHEN ISNULL(PriceVolatility, 0) = 0 THEN 20
            WHEN (PriceVolatility / NULLIF(AvgImportPrice, 0)) < 0.1 THEN 20
            WHEN (PriceVolatility / NULLIF(AvgImportPrice, 0)) < 0.2 THEN 15
            WHEN (PriceVolatility / NULLIF(AvgImportPrice, 0)) < 0.3 THEN 10
            ELSE 5
        END,
        FrequencyScore = CASE 
            WHEN ISNULL(DaysBetweenReceipts, 999) <= 7 THEN 20
            WHEN DaysBetweenReceipts <= 15 THEN 15
            WHEN DaysBetweenReceipts <= 30 THEN 10
            ELSE 5
        END,
        TotalScore = (
            CASE WHEN TotalReceipts >= 20 THEN 30 WHEN TotalReceipts >= 10 THEN 20 WHEN TotalReceipts >= 5 THEN 10 ELSE 5 END +
            CASE WHEN TotalValue >= 100000000 THEN 30 WHEN TotalValue >= 50000000 THEN 20 WHEN TotalValue >= 10000000 THEN 10 ELSE 5 END +
            CASE WHEN ISNULL(PriceVolatility, 0) = 0 THEN 20 WHEN (PriceVolatility / NULLIF(AvgImportPrice, 0)) < 0.1 THEN 20 
                 WHEN (PriceVolatility / NULLIF(AvgImportPrice, 0)) < 0.2 THEN 15 WHEN (PriceVolatility / NULLIF(AvgImportPrice, 0)) < 0.3 THEN 10 ELSE 5 END +
            CASE WHEN ISNULL(DaysBetweenReceipts, 999) <= 7 THEN 20 WHEN DaysBetweenReceipts <= 15 THEN 15 WHEN DaysBetweenReceipts <= 30 THEN 10 ELSE 5 END
        ),
        Rating = CASE 
            WHEN (CASE WHEN TotalReceipts >= 20 THEN 30 WHEN TotalReceipts >= 10 THEN 20 WHEN TotalReceipts >= 5 THEN 10 ELSE 5 END +
                  CASE WHEN TotalValue >= 100000000 THEN 30 WHEN TotalValue >= 50000000 THEN 20 WHEN TotalValue >= 10000000 THEN 10 ELSE 5 END +
                  CASE WHEN ISNULL(PriceVolatility, 0) = 0 THEN 20 WHEN (PriceVolatility / NULLIF(AvgImportPrice, 0)) < 0.1 THEN 20 
                       WHEN (PriceVolatility / NULLIF(AvgImportPrice, 0)) < 0.2 THEN 15 WHEN (PriceVolatility / NULLIF(AvgImportPrice, 0)) < 0.3 THEN 10 ELSE 5 END +
                  CASE WHEN ISNULL(DaysBetweenReceipts, 999) <= 7 THEN 20 WHEN DaysBetweenReceipts <= 15 THEN 15 WHEN DaysBetweenReceipts <= 30 THEN 10 ELSE 5 END
                 ) >= 80 THEN 'Excellent'
            WHEN (CASE WHEN TotalReceipts >= 20 THEN 30 WHEN TotalReceipts >= 10 THEN 20 WHEN TotalReceipts >= 5 THEN 10 ELSE 5 END +
                  CASE WHEN TotalValue >= 100000000 THEN 30 WHEN TotalValue >= 50000000 THEN 20 WHEN TotalValue >= 10000000 THEN 10 ELSE 5 END +
                  CASE WHEN ISNULL(PriceVolatility, 0) = 0 THEN 20 WHEN (PriceVolatility / NULLIF(AvgImportPrice, 0)) < 0.1 THEN 20 
                       WHEN (PriceVolatility / NULLIF(AvgImportPrice, 0)) < 0.2 THEN 15 WHEN (PriceVolatility / NULLIF(AvgImportPrice, 0)) < 0.3 THEN 10 ELSE 5 END +
                  CASE WHEN ISNULL(DaysBetweenReceipts, 999) <= 7 THEN 20 WHEN DaysBetweenReceipts <= 15 THEN 15 WHEN DaysBetweenReceipts <= 30 THEN 10 ELSE 5 END
                 ) >= 60 THEN 'Good'
            WHEN (CASE WHEN TotalReceipts >= 20 THEN 30 WHEN TotalReceipts >= 10 THEN 20 WHEN TotalReceipts >= 5 THEN 10 ELSE 5 END +
                  CASE WHEN TotalValue >= 100000000 THEN 30 WHEN TotalValue >= 50000000 THEN 20 WHEN TotalValue >= 10000000 THEN 10 ELSE 5 END +
                  CASE WHEN ISNULL(PriceVolatility, 0) = 0 THEN 20 WHEN (PriceVolatility / NULLIF(AvgImportPrice, 0)) < 0.1 THEN 20 
                       WHEN (PriceVolatility / NULLIF(AvgImportPrice, 0)) < 0.2 THEN 15 WHEN (PriceVolatility / NULLIF(AvgImportPrice, 0)) < 0.3 THEN 10 ELSE 5 END +
                  CASE WHEN ISNULL(DaysBetweenReceipts, 999) <= 7 THEN 20 WHEN DaysBetweenReceipts <= 15 THEN 15 WHEN DaysBetweenReceipts <= 30 THEN 10 ELSE 5 END
                 ) >= 40 THEN 'Average'
            ELSE 'Poor'
        END
    FROM SupplierStats
);
GO

-- =============================================
-- Function #5: Price Trend Analysis
-- Kỹ thuật: LAG, LEAD, Trend calculation
-- =============================================
CREATE OR ALTER FUNCTION dbo.fn_ProductPriceTrend(
    @ProductID INT,
    @Months INT = 6
)
RETURNS TABLE
AS
RETURN
(
    WITH PriceHistory AS (
        SELECT 
            ChangeDate,
            OldPrice,
            NewPrice,
            PriceChange = NewPrice - OldPrice,
            PriceChangePercent = ((NewPrice - OldPrice) * 100.0 / NULLIF(OldPrice, 0)),
            RowNum = ROW_NUMBER() OVER (ORDER BY ChangeDate)
        FROM dbo.ProductPriceHistory
        WHERE ProductID = @ProductID
          AND ChangeDate >= DATEADD(MONTH, -@Months, GETDATE())
    ),
    TrendAnalysis AS (
        SELECT 
            TotalChanges = COUNT(*),
            FirstPrice = MIN(CASE WHEN RowNum = 1 THEN OldPrice END),
            LastPrice = MAX(NewPrice),
            MinPrice = MIN(OldPrice),
            MaxPrice = MAX(NewPrice),
            TotalPriceChange = SUM(PriceChange),
            AvgPercentChange = AVG(PriceChangePercent),
            StdDevPrice = STDEV(NewPrice),
            IncreaseCount = SUM(CASE WHEN PriceChange > 0 THEN 1 ELSE 0 END),
            DecreaseCount = SUM(CASE WHEN PriceChange < 0 THEN 1 ELSE 0 END)
        FROM PriceHistory
    )
    SELECT 
        ProductID = @ProductID,
        AnalysisPeriodMonths = @Months,
        TotalChanges = ISNULL(TotalChanges, 0),
        FirstPrice = ISNULL(FirstPrice, (SELECT SellingPrice FROM dbo.Products WHERE ProductID = @ProductID)),
        LastPrice = ISNULL(LastPrice, (SELECT SellingPrice FROM dbo.Products WHERE ProductID = @ProductID)),
        CurrentPrice = (SELECT SellingPrice FROM dbo.Products WHERE ProductID = @ProductID),
        MinPrice = ISNULL(MinPrice, 0),
        MaxPrice = ISNULL(MaxPrice, 0),
        TotalPriceChange = ISNULL(TotalPriceChange, 0),
        AvgPercentChange = ISNULL(AvgPercentChange, 0),
        PriceVolatility = ISNULL(StdDevPrice, 0),
        IncreaseCount = ISNULL(IncreaseCount, 0),
        DecreaseCount = ISNULL(DecreaseCount, 0),
        TrendDirection = CASE 
            WHEN ISNULL(IncreaseCount, 0) > ISNULL(DecreaseCount, 0) THEN N'Tăng'
            WHEN ISNULL(IncreaseCount, 0) < ISNULL(DecreaseCount, 0) THEN N'Giảm'
            ELSE N'Ổn định'
        END,
        OverallChangePercent = CASE 
            WHEN ISNULL(FirstPrice, 0) > 0 
            THEN ((ISNULL(LastPrice, (SELECT SellingPrice FROM dbo.Products WHERE ProductID = @ProductID)) 
                   - ISNULL(FirstPrice, (SELECT SellingPrice FROM dbo.Products WHERE ProductID = @ProductID))) 
                  * 100.0 / FirstPrice)
            ELSE 0
        END
    FROM TrendAnalysis
);
GO

-- Test functions
PRINT N'✅ Đã tạo 5 Advanced Functions thành công!';
PRINT N'   - fn_ProductABCClassification';
PRINT N'   - fn_InventoryTurnoverRate';
PRINT N'   - fn_CalculateReorderPoint';
PRINT N'   - fn_SupplierPerformanceScore';
PRINT N'   - fn_ProductPriceTrend';
GO
