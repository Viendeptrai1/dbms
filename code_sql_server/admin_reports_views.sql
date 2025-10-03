/*
=============================================
ADMIN REPORTS - VIEWS & STORED PROCEDURES
=============================================
Báo cáo hệ thống cho AdminReportsForm
1. Users theo Role
2. User Activity (GoodsReceipts by User)
3. Price History Audit Trail
=============================================
*/

USE QLNhapHang;
GO

-- =============================================
-- 1. VIEW: USERS THEO ROLE
-- =============================================
IF OBJECT_ID('dbo.vw_UsersRoleSummary', 'V') IS NOT NULL
    DROP VIEW dbo.vw_UsersRoleSummary;
GO

CREATE VIEW dbo.vw_UsersRoleSummary
AS
SELECT 
    r.RoleName,
    r.Description AS RoleDescription,
    TotalUsers = COUNT(DISTINCT ur.UserID),
    ActiveUsers = SUM(CASE WHEN u.IsActive = 1 THEN 1 ELSE 0 END),
    InactiveUsers = SUM(CASE WHEN u.IsActive = 0 THEN 1 ELSE 0 END),
    UserList = STRING_AGG(u.Username, ', ') WITHIN GROUP (ORDER BY u.Username)
FROM dbo.Roles r
LEFT JOIN dbo.UsersRoles ur ON r.RoleID = ur.RoleID
LEFT JOIN dbo.Users u ON ur.UserID = u.UserID
WHERE u.Username NOT LIKE '%_DELETED_%' OR u.Username IS NULL
GROUP BY r.RoleID, r.RoleName, r.Description;
GO

-- =============================================
-- 2. VIEW: USER ACTIVITY - GOODSRECEIPTS BY USER
-- =============================================
IF OBJECT_ID('dbo.vw_UserActivity', 'V') IS NOT NULL
    DROP VIEW dbo.vw_UserActivity;
GO

CREATE VIEW dbo.vw_UserActivity
AS
SELECT 
    u.UserID,
    u.Username,
    u.FullName,
    r.RoleName,
    TotalReceipts = COUNT(DISTINCT gr.ReceiptID),
    TotalAmount = ISNULL(SUM(gr.TotalAmount), 0),
    FirstReceipt = MIN(gr.ReceiptDate),
    LastReceipt = MAX(gr.ReceiptDate),
    IsActive = u.IsActive
FROM dbo.Users u
LEFT JOIN dbo.UsersRoles ur ON u.UserID = ur.UserID
LEFT JOIN dbo.Roles r ON ur.RoleID = r.RoleID
LEFT JOIN dbo.GoodsReceipts gr ON u.UserID = gr.UserID
WHERE u.Username NOT LIKE '%_DELETED_%'
GROUP BY u.UserID, u.Username, u.FullName, r.RoleName, u.IsActive;
GO

-- =============================================
-- 3. VIEW: PRICE HISTORY AUDIT TRAIL
-- =============================================
IF OBJECT_ID('dbo.vw_ProductPriceHistory', 'V') IS NOT NULL
    DROP VIEW dbo.vw_ProductPriceHistory;
GO

CREATE VIEW dbo.vw_ProductPriceHistory
AS
SELECT 
    pph.PriceHistoryID,
    pph.ProductID,
    p.SKU,
    p.ProductName,
    pph.OldPrice,
    pph.NewPrice,
    PriceChange = pph.NewPrice - pph.OldPrice,
    PercentChange = CASE 
        WHEN pph.OldPrice = 0 THEN NULL
        ELSE CAST((pph.NewPrice - pph.OldPrice) * 100.0 / pph.OldPrice AS DECIMAL(10,2))
    END,
    pph.ChangeDate,
    ChangedByUserID = pph.ChangedBy,
    ChangedByUsername = u.Username
FROM dbo.ProductPriceHistory pph
JOIN dbo.Products p ON pph.ProductID = p.ProductID
LEFT JOIN dbo.Users u ON pph.ChangedBy = u.UserID;
GO

-- =============================================
-- 4. STORED PROCEDURE: GET USER ACTIVITY BY DATE RANGE
-- =============================================
IF OBJECT_ID('dbo.sp_GetUserActivityByDateRange', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_GetUserActivityByDateRange;
GO

CREATE PROCEDURE sp_GetUserActivityByDateRange
    @FromDate DATETIME2(3) = NULL,
    @ToDate DATETIME2(3) = NULL,
    @Username VARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Default date range: last 30 days
    IF @FromDate IS NULL
        SET @FromDate = DATEADD(DAY, -30, GETDATE());
    
    IF @ToDate IS NULL
        SET @ToDate = GETDATE();
    
    SELECT 
        u.UserID,
        u.Username,
        u.FullName,
        r.RoleName,
        TotalReceipts = COUNT(DISTINCT gr.ReceiptID),
        TotalAmount = ISNULL(SUM(gr.TotalAmount), 0),
        AvgAmount = ISNULL(AVG(gr.TotalAmount), 0),
        FirstReceipt = MIN(gr.ReceiptDate),
        LastReceipt = MAX(gr.ReceiptDate)
    FROM dbo.Users u
    LEFT JOIN dbo.UsersRoles ur ON u.UserID = ur.UserID
    LEFT JOIN dbo.Roles r ON ur.RoleID = r.RoleID
    LEFT JOIN dbo.GoodsReceipts gr ON u.UserID = gr.UserID 
        AND gr.ReceiptDate >= @FromDate 
        AND gr.ReceiptDate <= @ToDate
    WHERE u.Username NOT LIKE '%_DELETED_%'
        AND (@Username IS NULL OR u.Username = @Username)
    GROUP BY u.UserID, u.Username, u.FullName, r.RoleName
    ORDER BY TotalAmount DESC, TotalReceipts DESC;
END;
GO

-- =============================================
-- 5. STORED PROCEDURE: GET PRICE HISTORY BY DATE RANGE
-- =============================================
IF OBJECT_ID('dbo.sp_GetPriceHistoryByDateRange', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_GetPriceHistoryByDateRange;
GO

CREATE PROCEDURE sp_GetPriceHistoryByDateRange
    @FromDate DATETIME2(3) = NULL,
    @ToDate DATETIME2(3) = NULL,
    @ProductID INT = NULL,
    @SKU VARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Default date range: last 30 days
    IF @FromDate IS NULL
        SET @FromDate = DATEADD(DAY, -30, GETDATE());
    
    IF @ToDate IS NULL
        SET @ToDate = GETDATE();
    
    SELECT 
        pph.PriceHistoryID,
        pph.ProductID,
        p.SKU,
        p.ProductName,
        pph.OldPrice,
        pph.NewPrice,
        PriceChange = pph.NewPrice - pph.OldPrice,
        PercentChange = CASE 
            WHEN pph.OldPrice = 0 THEN NULL
            ELSE CAST((pph.NewPrice - pph.OldPrice) * 100.0 / pph.OldPrice AS DECIMAL(10,2))
        END,
        pph.ChangeDate,
        ChangedByUserID = pph.ChangedBy,
        ChangedByUsername = u.Username
    FROM dbo.ProductPriceHistory pph
    JOIN dbo.Products p ON pph.ProductID = p.ProductID
    LEFT JOIN dbo.Users u ON pph.ChangedBy = u.UserID
    WHERE pph.ChangeDate >= @FromDate 
        AND pph.ChangeDate <= @ToDate
        AND (@ProductID IS NULL OR pph.ProductID = @ProductID)
        AND (@SKU IS NULL OR p.SKU = @SKU)
    ORDER BY pph.ChangeDate DESC;
END;
GO

-- =============================================
-- GRANT PERMISSIONS
-- =============================================
GRANT SELECT ON dbo.vw_UsersRoleSummary TO dbrole_Admin;
GRANT SELECT ON dbo.vw_UserActivity TO dbrole_Admin;
GRANT SELECT ON dbo.vw_ProductPriceHistory TO dbrole_Admin;
GRANT EXECUTE ON dbo.sp_GetUserActivityByDateRange TO dbrole_Admin;
GRANT EXECUTE ON dbo.sp_GetPriceHistoryByDateRange TO dbrole_Admin;
GO

PRINT N'';
PRINT N'✅✅✅ HOÀN TẤT SETUP ADMIN REPORTS! ✅✅✅';
PRINT N'';
PRINT N'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━';
PRINT N'📋 ĐÃ TẠO CÁC VIEWS:';
PRINT N'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━';
PRINT N'  ✅ vw_UsersRoleSummary - Tổng quan users theo role';
PRINT N'  ✅ vw_UserActivity - Hoạt động người dùng';
PRINT N'  ✅ vw_ProductPriceHistory - Lịch sử thay đổi giá';
PRINT N'';
PRINT N'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━';
PRINT N'📋 ĐÃ TẠO CÁC STORED PROCEDURES:';
PRINT N'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━';
PRINT N'  ✅ sp_GetUserActivityByDateRange';
PRINT N'  ✅ sp_GetPriceHistoryByDateRange';
GO
