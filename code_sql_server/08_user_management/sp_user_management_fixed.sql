/*
=============================================
STORED PROCEDURES QUẢN LÝ USER VÀ ROLE (FIXED)
=============================================
Tác giả: Database Admin
Ngày tạo: 2024
Mô tả: Các stored procedures để quản lý tài khoản người dùng và phân quyền
*/

USE QLNhapHang;
GO

-- Xóa các stored procedures cũ nếu tồn tại
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_CreateUserWithRole]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_CreateUserWithRole];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_ChangeUserRole]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_ChangeUserRole];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_ToggleUserStatus]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_ToggleUserStatus];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_DeleteUser]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_DeleteUser];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetUsersWithRoles]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_GetUsersWithRoles];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_CheckUserPermission]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_CheckUserPermission];
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_RevokeAllUserPermissions]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_RevokeAllUserPermissions];
GO

-- =============================================
-- 1. SP TẠO USER MỚI VÀ GÁN ROLE
-- =============================================
CREATE PROCEDURE sp_CreateUserWithRole
    @Username VARCHAR(50),
    @Password NVARCHAR(100),  -- Nhận password plain text
    @FullName NVARCHAR(100),
    @RoleName NVARCHAR(20),
    @IsActive BIT = 1,
    @NewUserID INT OUTPUT,
    @Message NVARCHAR(200) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Kiểm tra username đã tồn tại chưa
        IF EXISTS (SELECT 1 FROM dbo.Users WHERE Username = @Username)
        BEGIN
            SET @Message = N'Username đã tồn tại!';
            SET @NewUserID = -1;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Kiểm tra role hợp lệ
        IF NOT EXISTS (SELECT 1 FROM dbo.Roles WHERE RoleName = @RoleName)
        BEGIN
            SET @Message = N'Role không hợp lệ!';
            SET @NewUserID = -1;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Tạo user mới với password hash bằng SQL
        INSERT INTO dbo.Users (Username, PasswordHash, FullName, IsActive)
        VALUES (@Username, HASHBYTES('SHA2_256', @Password), @FullName, @IsActive);
        
        SET @NewUserID = SCOPE_IDENTITY();
        
        -- Lấy RoleID
        DECLARE @RoleID INT;
        SELECT @RoleID = RoleID FROM dbo.Roles WHERE RoleName = @RoleName;
        
        -- Gán role cho user
        INSERT INTO dbo.UsersRoles (UserID, RoleID)
        VALUES (@NewUserID, @RoleID);
        
        COMMIT TRANSACTION;
        SET @Message = N'Tạo user thành công!';
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
            
        SET @Message = N'Lỗi: ' + ERROR_MESSAGE();
        SET @NewUserID = -1;
    END CATCH
END
GO

-- =============================================
-- 2. SP THAY ĐỔI ROLE CỦA USER
-- =============================================
CREATE PROCEDURE sp_ChangeUserRole
    @Username NVARCHAR(50),
    @NewRoleName NVARCHAR(20),
    @Message NVARCHAR(200) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Kiểm tra user tồn tại
        IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Username = @Username)
        BEGIN
            SET @Message = N'User không tồn tại!';
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Kiểm tra role hợp lệ
        IF NOT EXISTS (SELECT 1 FROM dbo.Roles WHERE RoleName = @NewRoleName)
        BEGIN
            SET @Message = N'Role không hợp lệ!';
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        DECLARE @UserID INT, @NewRoleID INT;
        
        -- Lấy UserID
        SELECT @UserID = UserID FROM dbo.Users WHERE Username = @Username;
        
        -- Lấy RoleID mới
        SELECT @NewRoleID = RoleID FROM dbo.Roles WHERE RoleName = @NewRoleName;
        
        -- Xóa role cũ
        DELETE FROM dbo.UsersRoles WHERE UserID = @UserID;
        
        -- Gán role mới
        INSERT INTO dbo.UsersRoles (UserID, RoleID)
        VALUES (@UserID, @NewRoleID);
        
        COMMIT TRANSACTION;
        SET @Message = N'Thay đổi role thành công!';
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
            
        SET @Message = N'Lỗi: ' + ERROR_MESSAGE();
    END CATCH
END
GO

-- =============================================
-- 3. SP KHÓA/MỞ KHÓA USER
-- =============================================
CREATE PROCEDURE sp_ToggleUserStatus
    @Username NVARCHAR(50),
    @Message NVARCHAR(200) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        -- Kiểm tra user tồn tại
        IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Username = @Username)
        BEGIN
            SET @Message = N'User không tồn tại!';
            RETURN;
        END
        
        -- Toggle status
        UPDATE dbo.Users 
        SET IsActive = CASE WHEN IsActive = 1 THEN 0 ELSE 1 END
        WHERE Username = @Username;
        
        DECLARE @NewStatus BIT;
        SELECT @NewStatus = IsActive FROM dbo.Users WHERE Username = @Username;
        
        SET @Message = CASE WHEN @NewStatus = 1 THEN N'Đã mở khóa user!' ELSE N'Đã khóa user!' END;
        
    END TRY
    BEGIN CATCH
        SET @Message = N'Lỗi: ' + ERROR_MESSAGE();
    END CATCH
END
GO

-- =============================================
-- 4. SP XÓA USER (SOFT DELETE)
-- =============================================
CREATE PROCEDURE sp_DeleteUser
    @Username NVARCHAR(50),
    @Message NVARCHAR(200) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Kiểm tra user tồn tại
        IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Username = @Username)
        BEGIN
            SET @Message = N'User không tồn tại!';
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        DECLARE @UserID INT;
        SELECT @UserID = UserID FROM dbo.Users WHERE Username = @Username;
        
        -- Xóa user roles
        DELETE FROM dbo.UsersRoles WHERE UserID = @UserID;
        
        -- Soft delete user (không xóa thật để giữ lịch sử)
        UPDATE dbo.Users 
        SET IsActive = 0, 
            Username = Username + '_DELETED_' + CONVERT(NVARCHAR(20), GETDATE(), 112)
        WHERE UserID = @UserID;
        
        COMMIT TRANSACTION;
        SET @Message = N'Xóa user thành công!';
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
            
        SET @Message = N'Lỗi: ' + ERROR_MESSAGE();
    END CATCH
END
GO

-- =============================================
-- 5. SP LẤY DANH SÁCH USER VÀ ROLE
-- =============================================
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
    GROUP BY u.UserID, u.Username, u.IsActive
    ORDER BY u.Username;
END
GO

-- =============================================
-- 6. SP KIỂM TRA QUYỀN USER
-- =============================================
CREATE PROCEDURE sp_CheckUserPermission
    @Username NVARCHAR(50),
    @Permission NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        CASE 
            WHEN EXISTS (
                SELECT 1 FROM dbo.Users u
                JOIN dbo.UsersRoles ur ON u.UserID = ur.UserID
                JOIN dbo.Roles r ON ur.RoleID = r.RoleID
                WHERE u.Username = @Username 
                AND u.IsActive = 1
                AND r.RoleName = @Permission
            ) THEN 1
            ELSE 0
        END AS HasPermission;
END
GO

-- =============================================
-- 7. SP REVOKE TẤT CẢ QUYỀN CỦA USER
-- =============================================
CREATE PROCEDURE sp_RevokeAllUserPermissions
    @Username NVARCHAR(50),
    @Message NVARCHAR(200) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Kiểm tra user tồn tại
        IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Username = @Username)
        BEGIN
            SET @Message = N'User không tồn tại!';
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        DECLARE @UserID INT;
        SELECT @UserID = UserID FROM dbo.Users WHERE Username = @Username;
        
        -- Xóa tất cả roles của user
        DELETE FROM dbo.UsersRoles WHERE UserID = @UserID;
        
        COMMIT TRANSACTION;
        SET @Message = N'Đã revoke tất cả quyền của user!';
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
            
        SET @Message = N'Lỗi: ' + ERROR_MESSAGE();
    END CATCH
END
GO

-- =============================================
-- GRANT QUYỀN EXECUTE CHO ADMIN
-- =============================================
GRANT EXECUTE ON OBJECT::dbo.sp_CreateUserWithRole TO dbrole_Admin;
GRANT EXECUTE ON OBJECT::dbo.sp_ChangeUserRole TO dbrole_Admin;
GRANT EXECUTE ON OBJECT::dbo.sp_ToggleUserStatus TO dbrole_Admin;
GRANT EXECUTE ON OBJECT::dbo.sp_DeleteUser TO dbrole_Admin;
GRANT EXECUTE ON OBJECT::dbo.sp_GetUsersWithRoles TO dbrole_Admin;
GRANT EXECUTE ON OBJECT::dbo.sp_CheckUserPermission TO dbrole_Admin;
GRANT EXECUTE ON OBJECT::dbo.sp_RevokeAllUserPermissions TO dbrole_Admin;

-- =============================================
-- TEST CÁC STORED PROCEDURES
-- =============================================

-- Test 1: Tạo user mới
/*
DECLARE @NewUserID INT, @Message NVARCHAR(200);
EXEC sp_CreateUserWithRole 
    @Username = 'testuser01',
    @Password = '123456',
    @FullName = N'Test User 01',
    @RoleName = N'Seller',
    @NewUserID = @NewUserID OUTPUT,
    @Message = @Message OUTPUT;
    
SELECT @NewUserID, @Message;
*/

-- Test 2: Lấy danh sách users
/*
EXEC sp_GetUsersWithRoles;
*/

-- Test 3: Kiểm tra quyền
/*
EXEC sp_CheckUserPermission @Username = N'admin', @Permission = N'Admin';
*/

PRINT N'Đã tạo xong tất cả stored procedures quản lý user và role!';
GO

