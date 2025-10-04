/*
    FIX: sp_CreateGoodsReceipt
    Đảm bảo SP đúng như mong đợi
*/

USE QLNhapHang;
GO

-- Drop nếu tồn tại
IF OBJECT_ID('dbo.sp_CreateGoodsReceipt', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_CreateGoodsReceipt;
GO

CREATE PROCEDURE dbo.sp_CreateGoodsReceipt
    @SupplierID INT,
    @UserID INT,
    @Notes NVARCHAR(500) = NULL,
    @ReceiptLines dbo.udt_GoodsReceiptLine READONLY,
    @ReceiptID INT OUTPUT,
    @Message NVARCHAR(200) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Validate Supplier exists
        IF NOT EXISTS (SELECT 1 FROM dbo.Suppliers WHERE SupplierID = @SupplierID)
        BEGIN
            SET @Message = N'Nhà cung cấp không tồn tại!';
            SET @ReceiptID = -1;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Validate User exists and is active
        IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE UserID = @UserID AND IsActive = 1)
        BEGIN
            SET @Message = N'User không tồn tại hoặc đã bị khóa!';
            SET @ReceiptID = -1;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Insert GoodsReceipt
        INSERT INTO dbo.GoodsReceipts (SupplierID, UserID, Notes, TotalAmount)
        VALUES (@SupplierID, @UserID, @Notes, 0);
        
        SET @ReceiptID = SCOPE_IDENTITY();
        
        -- Insert GoodsReceiptDetails
        INSERT INTO dbo.GoodsReceiptDetails (ReceiptID, ProductID, Quantity, ImportPrice, LineAmount)
        SELECT @ReceiptID, ProductID, Quantity, ImportPrice, Quantity * ImportPrice
        FROM @ReceiptLines;
        
        -- Update Products stock
        UPDATE p
        SET p.StockQuantity = p.StockQuantity + rl.Quantity
        FROM dbo.Products p
        INNER JOIN @ReceiptLines rl ON p.ProductID = rl.ProductID;
        
        -- Update total amount
        UPDATE dbo.GoodsReceipts 
        SET TotalAmount = (SELECT SUM(LineAmount) FROM dbo.GoodsReceiptDetails WHERE ReceiptID = @ReceiptID)
        WHERE ReceiptID = @ReceiptID;
        
        COMMIT TRANSACTION;
        SET @Message = N'Tạo phiếu nhập thành công!';
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
            
        SET @Message = N'Lỗi: ' + ERROR_MESSAGE();
        SET @ReceiptID = -1;
    END CATCH
END;
GO

-- Test SP
PRINT 'Stored procedure sp_CreateGoodsReceipt đã được tạo lại thành công!';
GO
