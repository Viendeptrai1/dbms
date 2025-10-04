/*
=============================================
FIX: sp_GetUsersWithRoles - Loại bỏ users đã soft delete
=============================================
Mô tả: Fix stored procedure để không hiển thị users đã bị soft delete 
       (username có chứa '_DELETED_')
Vấn đề: Hiện tại SP đang hiển thị cả users đã soft delete
Fix: Thêm WHERE clause để filter users có '_DELETED_' trong username
=============================================
*/

USE QLNhapHang;
GO

-- Drop existing procedure
IF OBJECT_ID('dbo.sp_GetUsersWithRoles', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_GetUsersWithRoles;
GO

-- Recreate với filter mới
CREATE PROCEDURE sp_GetUsersWithRoles
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        u.UserID,
        u.Username,
        u.IsActive,
        STRING_AGG(r.RoleName, ', ') WITHIN GROUP (ORDER BY r.RoleName) AS Roles,
        COUNT(ur.RoleID) AS RoleCount
    FROM dbo.Users u
    LEFT JOIN dbo.UsersRoles ur ON u.UserID = ur.UserID
    LEFT JOIN dbo.Roles r ON ur.RoleID = r.RoleID
    WHERE u.Username NOT LIKE '%_DELETED_%'  -- ✅ FIX: Loại bỏ users đã soft delete
    GROUP BY u.UserID, u.Username, u.IsActive
    ORDER BY u.Username;
END;
GO

PRINT N'';
PRINT N'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━';
PRINT N'✅ sp_GetUsersWithRoles đã được FIX thành công!';
PRINT N'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━';
PRINT N'';
PRINT N'🔧 THAY ĐỔI:';
PRINT N'  • Thêm filter: WHERE u.Username NOT LIKE ''%_DELETED_%''';
PRINT N'  • Users đã soft delete sẽ KHÔNG còn hiển thị trong danh sách';
PRINT N'';
PRINT N'📋 TEST:';
PRINT N'  EXEC sp_GetUsersWithRoles;';
PRINT N'';
