/*
    TEMPORARY FIX: Disable triggers để test
    Tạm thời tắt triggers để tạo phiếu nhập được
*/

USE QLNhapHang;
GO

-- Disable triggers tạm thời
PRINT 'Disabling triggers...';

DISABLE TRIGGER TR_GoodsReceipts_BusinessRules ON dbo.GoodsReceipts;
PRINT 'Disabled: TR_GoodsReceipts_BusinessRules';

DISABLE TRIGGER TR_GoodsReceipts_UpdateSupplierMetrics ON dbo.GoodsReceipts;
PRINT 'Disabled: TR_GoodsReceipts_UpdateSupplierMetrics';

PRINT '';
PRINT 'Done! Triggers have been DISABLED.';
PRINT 'Now you can test creating receipt in the app.';
PRINT '';
PRINT 'To RE-ENABLE later, run:';
PRINT '  ENABLE TRIGGER TR_GoodsReceipts_BusinessRules ON dbo.GoodsReceipts;';
PRINT '  ENABLE TRIGGER TR_GoodsReceipts_UpdateSupplierMetrics ON dbo.GoodsReceipts;';
GO
