/*
    SHOW: Trigger definitions
    Xem chi tiết 2 triggers đang gây lỗi
*/

USE QLNhapHang;
GO

-- Trigger 1: TR_GoodsReceipts_BusinessRules
PRINT '========================================';
PRINT 'TRIGGER 1: TR_GoodsReceipts_BusinessRules';
PRINT '========================================';
DECLARE @def1 NVARCHAR(MAX) = OBJECT_DEFINITION(OBJECT_ID('dbo.TR_GoodsReceipts_BusinessRules'));
IF @def1 IS NOT NULL
BEGIN
    -- Print từng 4000 ký tự (giới hạn của PRINT)
    DECLARE @pos1 INT = 1;
    WHILE @pos1 <= LEN(@def1)
    BEGIN
        PRINT SUBSTRING(@def1, @pos1, 4000);
        SET @pos1 = @pos1 + 4000;
    END
END
ELSE
    PRINT 'Trigger not found!';
GO

PRINT '';
PRINT '';

-- Trigger 2: TR_GoodsReceipts_UpdateSupplierMetrics
PRINT '========================================';
PRINT 'TRIGGER 2: TR_GoodsReceipts_UpdateSupplierMetrics';
PRINT '========================================';
DECLARE @def2 NVARCHAR(MAX) = OBJECT_DEFINITION(OBJECT_ID('dbo.TR_GoodsReceipts_UpdateSupplierMetrics'));
IF @def2 IS NOT NULL
BEGIN
    DECLARE @pos2 INT = 1;
    WHILE @pos2 <= LEN(@def2)
    BEGIN
        PRINT SUBSTRING(@def2, @pos2, 4000);
        SET @pos2 = @pos2 + 4000;
    END
END
ELSE
    PRINT 'Trigger not found!';
GO

PRINT '';
PRINT '======== DONE ========';

