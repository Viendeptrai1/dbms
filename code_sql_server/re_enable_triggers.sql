/*
    RE-ENABLE triggers sau khi fix xong
*/

USE QLNhapHang;
GO

PRINT 'Re-enabling triggers...';

ENABLE TRIGGER TR_GoodsReceipts_BusinessRules ON dbo.GoodsReceipts;
PRINT 'Enabled: TR_GoodsReceipts_BusinessRules';

ENABLE TRIGGER TR_GoodsReceipts_UpdateSupplierMetrics ON dbo.GoodsReceipts;
PRINT 'Enabled: TR_GoodsReceipts_UpdateSupplierMetrics';

PRINT '';
PRINT 'Done! Triggers have been RE-ENABLED.';
GO
