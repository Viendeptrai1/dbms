/*
    CHECK: GoodsReceipts table schema
    Kiểm tra cấu trúc bảng thực tế trong database
*/

USE QLNhapHang;
GO

-- 1. Check columns
PRINT '===== COLUMNS =====';
SELECT 
    COLUMN_NAME,
    DATA_TYPE,
    CHARACTER_MAXIMUM_LENGTH,
    IS_NULLABLE,
    COLUMN_DEFAULT
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'GoodsReceipts'
ORDER BY ORDINAL_POSITION;
GO

-- 2. Check triggers
PRINT '===== TRIGGERS =====';
SELECT 
    t.name AS TriggerName,
    t.type_desc,
    t.is_disabled,
    OBJECT_DEFINITION(t.object_id) AS TriggerDefinition
FROM sys.triggers t
INNER JOIN sys.tables tb ON t.parent_id = tb.object_id
WHERE tb.name = 'GoodsReceipts';
GO

-- 3. Check constraints
PRINT '===== CONSTRAINTS =====';
SELECT 
    con.name AS ConstraintName,
    con.type_desc,
    con.definition
FROM sys.check_constraints con
INNER JOIN sys.tables t ON con.parent_object_id = t.object_id
WHERE t.name = 'GoodsReceipts';
GO

-- 4. Check computed columns
PRINT '===== COMPUTED COLUMNS =====';
SELECT 
    c.name AS ColumnName,
    c.is_computed,
    cc.definition
FROM sys.columns c
LEFT JOIN sys.computed_columns cc ON c.object_id = cc.object_id AND c.column_id = cc.column_id
WHERE c.object_id = OBJECT_ID('dbo.GoodsReceipts')
AND c.is_computed = 1;
GO

PRINT 'Done checking schema!';
