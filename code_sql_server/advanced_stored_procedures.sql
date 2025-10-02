/*
=============================================
ADVANCED STORED PROCEDURES - KHÔNG THAY ĐỔI SCHEMA
=============================================
Mô tả: Stored Procedures phức tạp với XML, Dynamic SQL, Cursor
Kỹ thuật: XML Parsing, Cursor, TRY-CATCH, Dynamic SQL
Ngày: 2025-10-01
=============================================
*/

USE QLNhapHang;
GO

-- =============================================
-- SP #1: Batch Import với XML và Error Handling
-- Kỹ thuật: XML parsing, CURSOR, TRY-CATCH per record
-- =============================================
CREATE OR ALTER PROCEDURE dbo.sp_BatchImportReceipts
    @ReceiptDataXML XML,
    @ProcessedCount INT OUTPUT,
    @ErrorCount INT OUTPUT,
    @Message NVARCHAR(MAX) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT OFF;
    
    DECLARE @ErrorLog TABLE (
        ReceiptNo INT,
        ErrorMessage NVARCHAR(500)
    );
    
    DECLARE @TempReceipts TABLE (
        ReceiptNo INT,
        SupplierID INT,
        UserID INT,
        Notes NVARCHAR(500),
        ProductID INT,
        Quantity INT,
        ImportPrice DECIMAL(10,2)
    );
    
    SET @ProcessedCount = 0;
    SET @ErrorCount = 0;
    
    -- Parse XML vào temp table
    BEGIN TRY
        INSERT INTO @TempReceipts (ReceiptNo, SupplierID, UserID, Notes, ProductID, Quantity, ImportPrice)
        SELECT 
            Receipt.value('(ReceiptNo)[1]', 'INT'),
            Receipt.value('(SupplierID)[1]', 'INT'),
            Receipt.value('(UserID)[1]', 'INT'),
            Receipt.value('(Notes)[1]', 'NVARCHAR(500)'),
            Line.value('(ProductID)[1]', 'INT'),
            Line.value('(Quantity)[1]', 'INT'),
            Line.value('(ImportPrice)[1]', 'DECIMAL(10,2)')
        FROM @ReceiptDataXML.nodes('/Receipts/Receipt') AS R(Receipt)
        CROSS APPLY Receipt.nodes('Lines/Line') AS L(Line);
    END TRY
    BEGIN CATCH
        SET @Message = N'Lỗi parse XML: ' + ERROR_MESSAGE();
        RETURN;
    END CATCH
    
    -- Cursor để xử lý từng receipt
    DECLARE @CurrentReceiptNo INT;
    DECLARE @SupplierID INT;
    DECLARE @UserID INT;
    DECLARE @Notes NVARCHAR(500);
    
    DECLARE receipt_cursor CURSOR LOCAL FAST_FORWARD FOR
    SELECT DISTINCT ReceiptNo, SupplierID, UserID, Notes
    FROM @TempReceipts
    ORDER BY ReceiptNo;
    
    OPEN receipt_cursor;
    FETCH NEXT FROM receipt_cursor INTO @CurrentReceiptNo, @SupplierID, @UserID, @Notes;
    
    WHILE @@FETCH_STATUS = 0
    BEGIN
        BEGIN TRY
            BEGIN TRANSACTION;
            
            -- Validate Supplier
            IF NOT EXISTS (SELECT 1 FROM dbo.Suppliers WHERE SupplierID = @SupplierID)
            BEGIN
                INSERT INTO @ErrorLog VALUES (@CurrentReceiptNo, N'Supplier không tồn tại');
                SET @ErrorCount = @ErrorCount + 1;
                ROLLBACK TRANSACTION;
                GOTO NextReceipt;
            END
            
            -- Validate User
            IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE UserID = @UserID AND IsActive = 1)
            BEGIN
                INSERT INTO @ErrorLog VALUES (@CurrentReceiptNo, N'User không tồn tại hoặc bị khóa');
                SET @ErrorCount = @ErrorCount + 1;
                ROLLBACK TRANSACTION;
                GOTO NextReceipt;
            END
            
            -- Create Receipt Header
            DECLARE @NewReceiptID INT;
            INSERT INTO dbo.GoodsReceipts (SupplierID, UserID, Notes, TotalAmount)
            VALUES (@SupplierID, @UserID, @Notes, 0);
            
            SET @NewReceiptID = SCOPE_IDENTITY();
            
            -- Insert Receipt Lines
            INSERT INTO dbo.GoodsReceiptDetails (ReceiptID, ProductID, Quantity, ImportPrice, LineAmount)
            SELECT 
                @NewReceiptID,
                ProductID,
                Quantity,
                ImportPrice,
                Quantity * ImportPrice
            FROM @TempReceipts
            WHERE ReceiptNo = @CurrentReceiptNo;
            
            COMMIT TRANSACTION;
            SET @ProcessedCount = @ProcessedCount + 1;
            
        END TRY
        BEGIN CATCH
            IF @@TRANCOUNT > 0
                ROLLBACK TRANSACTION;
            
            INSERT INTO @ErrorLog VALUES (@CurrentReceiptNo, ERROR_MESSAGE());
            SET @ErrorCount = @ErrorCount + 1;
        END CATCH
        
        NextReceipt:
        FETCH NEXT FROM receipt_cursor INTO @CurrentReceiptNo, @SupplierID, @UserID, @Notes;
    END
    
    CLOSE receipt_cursor;
    DEALLOCATE receipt_cursor;
    
    -- Build result message
    SET @Message = N'Đã xử lý: ' + CAST(@ProcessedCount AS NVARCHAR(10)) + N' phiếu thành công, ' 
                 + CAST(@ErrorCount AS NVARCHAR(10)) + N' phiếu lỗi.';
    
    IF @ErrorCount > 0
    BEGIN
        SET @Message = @Message + CHAR(13) + N'Chi tiết lỗi:' + CHAR(13);
        SELECT @Message = @Message + N'Receipt #' + CAST(ReceiptNo AS NVARCHAR(10)) 
                        + N': ' + ErrorMessage + CHAR(13)
        FROM @ErrorLog;
    END
END;
GO

-- =============================================
-- SP #2: Dynamic Price Strategy
-- Kỹ thuật: Dynamic SQL, Complex Business Logic
-- =============================================
CREATE OR ALTER PROCEDURE dbo.sp_DynamicPriceStrategy
    @Strategy VARCHAR(50),
    @CategoryName NVARCHAR(100) = NULL,
    @ABCClass CHAR(1) = NULL,
    @AdjustmentPercent DECIMAL(5,2) = NULL,
    @MinDaysInStock INT = NULL,
    @ChangedBy INT,
    @AffectedCount INT OUTPUT,
    @Message NVARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Validate User
        IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE UserID = @ChangedBy AND IsActive = 1)
        BEGIN
            SET @Message = N'User không tồn tại hoặc bị khóa!';
            SET @AffectedCount = 0;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        DECLARE @SQL NVARCHAR(MAX);
        DECLARE @UpdatedProducts TABLE (ProductID INT, OldPrice DECIMAL(10,2), NewPrice DECIMAL(10,2));
        
        IF @Strategy = 'ABC-Based'
        BEGIN
            -- Điều chỉnh giá theo ABC classification
            -- A: +10%, B: +5%, C: -5%
            WITH ABCData AS (
                SELECT * FROM dbo.fn_ProductABCClassification(
                    DATEADD(MONTH, -3, GETDATE()), 
                    GETDATE()
                )
            )
            UPDATE p
            SET p.SellingPrice = CASE 
                WHEN abc.ABCClass = 'A' THEN p.SellingPrice * 1.10
                WHEN abc.ABCClass = 'B' THEN p.SellingPrice * 1.05
                WHEN abc.ABCClass = 'C' THEN p.SellingPrice * 0.95
                ELSE p.SellingPrice
            END
            OUTPUT inserted.ProductID, deleted.SellingPrice, inserted.SellingPrice
            INTO @UpdatedProducts
            FROM dbo.Products p
            INNER JOIN ABCData abc ON abc.ProductID = p.ProductID
            WHERE (@ABCClass IS NULL OR abc.ABCClass = @ABCClass)
              AND (@CategoryName IS NULL OR EXISTS (
                  SELECT 1 FROM dbo.ProductCategories pc
                  JOIN dbo.Categories c ON c.CategoryID = pc.CategoryID
                  WHERE pc.ProductID = p.ProductID AND c.CategoryName = @CategoryName
              ));
        END
        ELSE IF @Strategy = 'Age-Based'
        BEGIN
            -- Giảm giá sản phẩm tồn lâu
            WITH LastImport AS (
                SELECT 
                    d.ProductID,
                    MAX(r.ReceiptDate) AS LastReceiptDate,
                    DATEDIFF(DAY, MAX(r.ReceiptDate), GETDATE()) AS DaysInStock
                FROM dbo.GoodsReceiptDetails d
                JOIN dbo.GoodsReceipts r ON r.ReceiptID = d.ReceiptID
                GROUP BY d.ProductID
            )
            UPDATE p
            SET p.SellingPrice = CASE 
                WHEN li.DaysInStock > 180 THEN p.SellingPrice * 0.80  -- Giảm 20%
                WHEN li.DaysInStock > 90 THEN p.SellingPrice * 0.90   -- Giảm 10%
                WHEN li.DaysInStock > 60 THEN p.SellingPrice * 0.95   -- Giảm 5%
                ELSE p.SellingPrice
            END
            OUTPUT inserted.ProductID, deleted.SellingPrice, inserted.SellingPrice
            INTO @UpdatedProducts
            FROM dbo.Products p
            INNER JOIN LastImport li ON li.ProductID = p.ProductID
            WHERE li.DaysInStock >= ISNULL(@MinDaysInStock, 60)
              AND (@CategoryName IS NULL OR EXISTS (
                  SELECT 1 FROM dbo.ProductCategories pc
                  JOIN dbo.Categories c ON c.CategoryID = pc.CategoryID
                  WHERE pc.ProductID = p.ProductID AND c.CategoryName = @CategoryName
              ));
        END
        ELSE IF @Strategy = 'Manual-Percent'
        BEGIN
            -- Điều chỉnh thủ công theo %
            IF @AdjustmentPercent IS NULL
            BEGIN
                SET @Message = N'Phải cung cấp AdjustmentPercent cho strategy Manual-Percent!';
                SET @AffectedCount = 0;
                ROLLBACK TRANSACTION;
                RETURN;
            END
            
            UPDATE p
            SET p.SellingPrice = p.SellingPrice * (1 + @AdjustmentPercent / 100.0)
            OUTPUT inserted.ProductID, deleted.SellingPrice, inserted.SellingPrice
            INTO @UpdatedProducts
            FROM dbo.Products p
            WHERE (@CategoryName IS NULL OR EXISTS (
                SELECT 1 FROM dbo.ProductCategories pc
                JOIN dbo.Categories c ON c.CategoryID = pc.CategoryID
                WHERE pc.ProductID = p.ProductID AND c.CategoryName = @CategoryName
            ));
        END
        ELSE IF @Strategy = 'Turnover-Based'
        BEGIN
            -- Tăng giá sản phẩm bán chạy, giảm giá sản phẩm ế
            WITH TurnoverData AS (
                SELECT 
                    p.ProductID,
                    t.TurnoverRate
                FROM dbo.Products p
                CROSS APPLY dbo.fn_InventoryTurnoverRate(p.ProductID, 3) t
            )
            UPDATE p
            SET p.SellingPrice = CASE 
                WHEN td.TurnoverRate > 5 THEN p.SellingPrice * 1.10  -- Bán chạy: +10%
                WHEN td.TurnoverRate > 3 THEN p.SellingPrice * 1.05  -- Trung bình: +5%
                WHEN td.TurnoverRate < 1 THEN p.SellingPrice * 0.90  -- Ế: -10%
                ELSE p.SellingPrice
            END
            OUTPUT inserted.ProductID, deleted.SellingPrice, inserted.SellingPrice
            INTO @UpdatedProducts
            FROM dbo.Products p
            INNER JOIN TurnoverData td ON td.ProductID = p.ProductID
            WHERE (@CategoryName IS NULL OR EXISTS (
                SELECT 1 FROM dbo.ProductCategories pc
                JOIN dbo.Categories c ON c.CategoryID = pc.CategoryID
                WHERE pc.ProductID = p.ProductID AND c.CategoryName = @CategoryName
            ));
        END
        ELSE
        BEGIN
            SET @Message = N'Strategy không hợp lệ! Các strategy: ABC-Based, Age-Based, Manual-Percent, Turnover-Based';
            SET @AffectedCount = 0;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        SET @AffectedCount = @@ROWCOUNT;
        
        -- Log price changes vào ProductPriceHistory
        INSERT INTO dbo.ProductPriceHistory (ProductID, OldPrice, NewPrice, ChangedBy, Reason)
        SELECT 
            ProductID, 
            OldPrice, 
            NewPrice, 
            @ChangedBy,
            N'Auto adjust - Strategy: ' + @Strategy
        FROM @UpdatedProducts
        WHERE OldPrice <> NewPrice;
        
        COMMIT TRANSACTION;
        
        SET @Message = N'Đã điều chỉnh giá cho ' + CAST(@AffectedCount AS NVARCHAR(10)) 
                     + N' sản phẩm theo strategy: ' + @Strategy;
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        
        SET @Message = N'Lỗi: ' + ERROR_MESSAGE();
        SET @AffectedCount = 0;
    END CATCH
END;
GO

-- =============================================
-- SP #3: Generate Reorder Suggestions
-- Kỹ thuật: Complex calculations với functions
-- =============================================
CREATE OR ALTER PROCEDURE dbo.sp_GenerateReorderSuggestions
    @DaysToProject INT = 30,
    @ServiceLevel DECIMAL(5,2) = 95.0
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Lấy danh sách sản phẩm cần nhập hàng
    WITH ReorderAnalysis AS (
        SELECT 
            p.ProductID,
            p.SKU,
            p.ProductName,
            p.StockQuantity,
            rp.ReorderPoint,
            rp.SafetyStock,
            rp.AvgDailyUsage,
            rp.StdDevDailyUsage,
            rp.CurrentStock
        FROM dbo.Products p
        CROSS APPLY dbo.fn_CalculateReorderPoint(p.ProductID, 7, @ServiceLevel) rp
        WHERE rp.ShouldReorder = 1
    ),
    TurnoverInfo AS (
        SELECT 
            p.ProductID,
            t.TurnoverRate,
            t.AvgMonthlyImport
        FROM dbo.Products p
        CROSS APPLY dbo.fn_InventoryTurnoverRate(p.ProductID, 3) t
    )
    SELECT 
        ra.ProductID,
        ra.SKU,
        ra.ProductName,
        ra.StockQuantity AS CurrentStock,
        ra.ReorderPoint,
        ra.SafetyStock,
        ra.AvgDailyUsage,
        ti.TurnoverRate,
        ti.AvgMonthlyImport,
        -- Đề xuất số lượng nhập
        SuggestedOrderQty = CAST(
            ra.AvgDailyUsage * @DaysToProject + ra.SafetyStock - ra.StockQuantity
        AS INT),
        -- Mức độ ưu tiên
        Priority = CASE 
            WHEN ra.StockQuantity <= ra.SafetyStock THEN N'Khẩn cấp'
            WHEN ra.StockQuantity <= ra.ReorderPoint * 0.5 THEN N'Cao'
            WHEN ra.StockQuantity <= ra.ReorderPoint THEN N'Trung bình'
            ELSE N'Thấp'
        END,
        -- Ước tính ngày hết hàng
        EstimatedStockoutDays = CASE 
            WHEN ra.AvgDailyUsage > 0 
            THEN CAST(ra.StockQuantity / ra.AvgDailyUsage AS INT)
            ELSE 999
        END
    FROM ReorderAnalysis ra
    LEFT JOIN TurnoverInfo ti ON ti.ProductID = ra.ProductID
    ORDER BY 
        CASE 
            WHEN ra.StockQuantity <= ra.SafetyStock THEN 1
            WHEN ra.StockQuantity <= ra.ReorderPoint * 0.5 THEN 2
            WHEN ra.StockQuantity <= ra.ReorderPoint THEN 3
            ELSE 4
        END,
        ra.AvgDailyUsage DESC;
END;
GO

-- =============================================
-- SP #4: Generate Multiple Report Types
-- Kỹ thuật: Multiple result sets, Dynamic SQL
-- =============================================
CREATE OR ALTER PROCEDURE dbo.sp_GenerateMonthlyReport
    @Month INT,
    @Year INT,
    @ReportType VARCHAR(50) = 'Summary'
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @FromDate DATE = DATEFROMPARTS(@Year, @Month, 1);
    DECLARE @ToDate DATE = EOMONTH(@FromDate);
    
    IF @ReportType = 'Summary' OR @ReportType = 'All'
    BEGIN
        -- Summary Report
        SELECT 
            ReportPeriod = FORMAT(@FromDate, 'MM/yyyy'),
            TotalReceipts = COUNT(DISTINCT r.ReceiptID),
            TotalProducts = COUNT(DISTINCT d.ProductID),
            TotalQuantity = SUM(d.Quantity),
            TotalValue = SUM(r.TotalAmount),
            UniqueSuppliers = COUNT(DISTINCT r.SupplierID),
            AvgReceiptValue = AVG(r.TotalAmount)
        FROM dbo.GoodsReceipts r
        JOIN dbo.GoodsReceiptDetails d ON d.ReceiptID = r.ReceiptID
        WHERE r.ReceiptDate >= @FromDate AND r.ReceiptDate <= @ToDate;
    END
    
    IF @ReportType = 'Detailed' OR @ReportType = 'All'
    BEGIN
        -- Detailed by Product
        SELECT 
            p.SKU,
            p.ProductName,
            TotalImports = SUM(d.Quantity),
            TotalValue = SUM(d.Quantity * d.ImportPrice),
            AvgImportPrice = AVG(d.ImportPrice),
            TimesImported = COUNT(DISTINCT d.ReceiptID),
            CurrentStock = p.StockQuantity
        FROM dbo.GoodsReceiptDetails d
        JOIN dbo.GoodsReceipts r ON r.ReceiptID = d.ReceiptID
        JOIN dbo.Products p ON p.ProductID = d.ProductID
        WHERE r.ReceiptDate >= @FromDate AND r.ReceiptDate <= @ToDate
        GROUP BY p.SKU, p.ProductName, p.StockQuantity
        ORDER BY SUM(d.Quantity * d.ImportPrice) DESC;
    END
    
    IF @ReportType = 'ABC' OR @ReportType = 'All'
    BEGIN
        -- ABC Analysis
        SELECT * FROM dbo.fn_ProductABCClassification(@FromDate, @ToDate)
        ORDER BY ABCClass, CumulativePercent;
    END
    
    IF @ReportType = 'Supplier' OR @ReportType = 'All'
    BEGIN
        -- Supplier Summary
        SELECT * FROM dbo.fn_ImportSummaryBySupplier(@FromDate, @ToDate)
        ORDER BY TotalValue DESC;
    END
END;
GO

-- =============================================
-- SP #5: Update Price cho 1 sản phẩm (MISSING trong cũ!)
-- Kỹ thuật: Simple update với validation
-- =============================================
CREATE OR ALTER PROCEDURE dbo.sp_UpdateSingleProductPrice
    @ProductID INT = NULL,
    @SKU VARCHAR(50) = NULL,
    @NewPrice DECIMAL(10,2),
    @Reason NVARCHAR(200) = NULL,
    @ChangedBy INT,
    @Message NVARCHAR(200) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Validate: Phải có ProductID hoặc SKU
        IF @ProductID IS NULL AND @SKU IS NULL
        BEGIN
            SET @Message = N'Phải cung cấp ProductID hoặc SKU!';
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Nếu có SKU, lấy ProductID
        IF @ProductID IS NULL AND @SKU IS NOT NULL
        BEGIN
            SELECT @ProductID = ProductID 
            FROM dbo.Products 
            WHERE SKU = @SKU;
            
            IF @ProductID IS NULL
            BEGIN
                SET @Message = N'SKU không tồn tại!';
                ROLLBACK TRANSACTION;
                RETURN;
            END
        END
        
        -- Validate product exists
        IF NOT EXISTS (SELECT 1 FROM dbo.Products WHERE ProductID = @ProductID)
        BEGIN
            SET @Message = N'Sản phẩm không tồn tại!';
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Validate user
        IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE UserID = @ChangedBy AND IsActive = 1)
        BEGIN
            SET @Message = N'User không tồn tại hoặc bị khóa!';
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Validate new price
        IF @NewPrice <= 0
        BEGIN
            SET @Message = N'Giá mới phải > 0!';
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Get old price
        DECLARE @OldPrice DECIMAL(10,2);
        SELECT @OldPrice = SellingPrice 
        FROM dbo.Products 
        WHERE ProductID = @ProductID;
        
        -- Check if price changed
        IF @OldPrice = @NewPrice
        BEGIN
            SET @Message = N'Giá mới giống giá cũ, không cần update!';
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Update price
        UPDATE dbo.Products 
        SET SellingPrice = @NewPrice
        WHERE ProductID = @ProductID;
        
        -- Log vào ProductPriceHistory (trigger sẽ tự log, nhưng có thể thêm reason)
        IF @Reason IS NOT NULL
        BEGIN
            UPDATE dbo.ProductPriceHistory
            SET Reason = @Reason
            WHERE ProductID = @ProductID
              AND ChangeDate = (SELECT MAX(ChangeDate) FROM dbo.ProductPriceHistory WHERE ProductID = @ProductID);
        END
        
        COMMIT TRANSACTION;
        
        DECLARE @ProductName NVARCHAR(200);
        SELECT @ProductName = ProductName FROM dbo.Products WHERE ProductID = @ProductID;
        
        SET @Message = N'Đã cập nhật giá cho "' + @ProductName + N'" từ ' 
                     + CAST(@OldPrice AS NVARCHAR(20)) + N' → ' 
                     + CAST(@NewPrice AS NVARCHAR(20));
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        
        SET @Message = N'Lỗi: ' + ERROR_MESSAGE();
    END CATCH
END;
GO

PRINT N'✅ Đã tạo 5 Advanced Stored Procedures thành công!';
PRINT N'   - sp_BatchImportReceipts (XML + Cursor)';
PRINT N'   - sp_DynamicPriceStrategy (Dynamic SQL + Complex Logic)';
PRINT N'   - sp_GenerateReorderSuggestions (Smart recommendations)';
PRINT N'   - sp_GenerateMonthlyReport (Multiple result sets)';
PRINT N'   - sp_UpdateSingleProductPrice (Update giá 1 món - MISSING trong cũ!)';
GO
