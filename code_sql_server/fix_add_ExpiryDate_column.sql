/*
=============================================
FIX: Thêm cột ExpiryDate vào GoodsReceiptDetails
=============================================
Mô tả: DataSet có cột ExpiryDate nhưng database table không có
Vấn đề: Gây lỗi "Invalid column name 'ExpiryDate'" khi load data
Fix: Thêm cột ExpiryDate (DATE NULL) vào bảng GoodsReceiptDetails
=============================================
*/

USE QLNhapHang;
GO

-- Kiểm tra nếu cột chưa tồn tại thì thêm
IF NOT EXISTS (
    SELECT 1 
    FROM sys.columns 
    WHERE object_id = OBJECT_ID('dbo.GoodsReceiptDetails') 
    AND name = 'ExpiryDate'
)
BEGIN
    PRINT N'🔧 Đang thêm cột ExpiryDate vào GoodsReceiptDetails...';
    
    ALTER TABLE dbo.GoodsReceiptDetails
    ADD ExpiryDate DATE NULL;  -- NULL vì không phải tất cả sản phẩm đều có hạn sử dụng
    
    PRINT N'✅ Đã thêm cột ExpiryDate thành công!';
END
ELSE
BEGIN
    PRINT N'ℹ️ Cột ExpiryDate đã tồn tại rồi.';
END
GO

PRINT N'';
PRINT N'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━';
PRINT N'✅ FIX HOÀN TẤT!';
PRINT N'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━';
PRINT N'';
PRINT N'📋 CẤU TRÚC MỚI:';
PRINT N'  • ExpiryDate DATE NULL';
PRINT N'  • Dùng để lưu ngày hết hạn sản phẩm (nếu có)';
PRINT N'  • NULL = không có hạn sử dụng hoặc chưa cập nhật';
PRINT N'';
PRINT N'🔍 KIỂM TRA:';
SELECT 
    c.name AS ColumnName,
    t.name AS DataType,
    c.max_length AS MaxLength,
    c.is_nullable AS IsNullable
FROM sys.columns c
JOIN sys.types t ON c.user_type_id = t.user_type_id
WHERE c.object_id = OBJECT_ID('dbo.GoodsReceiptDetails')
ORDER BY c.column_id;
GO

PRINT N'';
PRINT N'✅ Lỗi "Invalid column name ExpiryDate" sẽ được fix!';
PRINT N'';
