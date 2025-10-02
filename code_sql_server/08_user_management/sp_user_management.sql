/*
=============================================
STORED PROCEDURES QUẢN LÝ USER VÀ ROLE
=============================================
Tác giả: Database Admin
Ngày tạo: 2024
Mô tả: Các stored procedures để quản lý tài khoản người dùng và phân quyền
*/

USE QLNhapHang;
GO

-- =============================================
-- 1. SP TẠO USER MỚI VÀ GÁN ROLE
-- =============================================
CREATE OR ALTER PROCEDURE sp_CreateUserWithRole
    @Username NVARCHAR(50),
    @PasswordHash NVARCHAR(64),  -- SHA2-256 hash
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
        
        -- Tạo user mới
        INSERT INTO dbo.Users (Username, PasswordHash, IsActive)
        VALUES (@Username, @PasswordHash, @IsActive);
        
        SET @NewUserID = SCOPE_IDENTITY();
        
        -- Lấy RoleID
        DECLARE @RoleID INT;
        SELECT @RoleID = RoleID FROM dbo.Roles WHERE RoleName = @RoleName;
        
        -- Gán role cho user
        INSERT INTO dbo.UsersRoles (UserID, RoleID)
        VALUES (@NewUserID, @RoleID);
        
        -- Tạo database user (không có login - chỉ để test)
        DECLARE @DBUser NVARCHAR(50) = @Username + '_db';
        DECLARE @SQL NVARCHAR(MAX);
        
        -- Tạo user database
        SET @SQL = N'IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = ''' + @DBUser + ''')
                     CREATE USER [' + @DBUser + '] WITHOUT LOGIN;';
        EXEC sp_executesql @SQL;
        
        -- Gán role database
        DECLARE @DBRole NVARCHAR(50) = 'dbrole_' + @RoleName;
        SET @SQL = N'ALTER ROLE [' + @DBRole + '] ADD MEMBER [' + @DBUser + '];';
        
        -- Kiểm tra role database tồn tại trước khi gán
        IF EXISTS (SELECT 1 FROM sys.database_principals WHERE name = @DBRole AND type = 'R')
        BEGIN
            EXEC sp_executesql @SQL;
        END
        
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
CREATE OR ALTER PROCEDURE sp_ChangeUserRole
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
        
        DECLARE @UserID INT, @NewRoleID INT, @OldRoleID INT;
        
        -- Lấy UserID
        SELECT @UserID = UserID FROM dbo.Users WHERE Username = @Username;
        
        -- Lấy RoleID mới
        SELECT @NewRoleID = RoleID FROM dbo.Roles WHERE RoleName = @NewRoleName;
        
        -- Lấy RoleID cũ
        SELECT @OldRoleID = RoleID FROM dbo.UsersRoles WHERE UserID = @UserID;
        
        -- Xóa role cũ
        DELETE FROM dbo.UsersRoles WHERE UserID = @UserID;
        
        -- Gán role mới
        INSERT INTO dbo.UsersRoles (UserID, RoleID)
        VALUES (@UserID, @NewRoleID);
        
        -- Cập nhật database roles
        DECLARE @DBUser NVARCHAR(50) = @Username + '_db';
        DECLARE @OldDBRole NVARCHAR(50) = 'dbrole_' + 
            (SELECT RoleName FROM dbo.Roles WHERE RoleID = @OldRoleID);
        DECLARE @NewDBRole NVARCHAR(50) = 'dbrole_' + @NewRoleName;
        DECLARE @SQL NVARCHAR(MAX);
        
        -- Xóa khỏi role cũ
        SET @SQL = N'ALTER ROLE [' + @OldDBRole + '] DROP MEMBER [' + @DBUser + '];';
        EXEC sp_executesql @SQL;
        
        -- Thêm vào role mới
        SET @SQL = N'ALTER ROLE [' + @NewDBRole + '] ADD MEMBER [' + @DBUser + '];';
        EXEC sp_executesql @SQL;
        
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
CREATE OR ALTER PROCEDURE sp_ToggleUserStatus
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
CREATE OR ALTER PROCEDURE sp_DeleteUser
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
            Username = Username + '_DELETED_' + CAST(GETDATE() AS NVARCHAR(20))
        WHERE UserID = @UserID;
        
        -- Xóa database user
        DECLARE @DBUser NVARCHAR(50) = @Username + '_db';
        DECLARE @SQL NVARCHAR(MAX);
        
        SET @SQL = N'IF EXISTS (SELECT 1 FROM sys.database_principals WHERE name = ''' + @DBUser + ''')
                     DROP USER [' + @DBUser + '];';
        EXEC sp_executesql @SQL;
        
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
CREATE OR ALTER PROCEDURE sp_GetUsersWithRoles
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
CREATE OR ALTER PROCEDURE sp_CheckUserPermission
    @Username NVARCHAR(50),
    @Permission NVARCHAR(50)  -- 'Admin', 'Seller', 'User'
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
CREATE OR ALTER PROCEDURE sp_RevokeAllUserPermissions
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
        
        -- Revoke tất cả database permissions
        DECLARE @DBUser NVARCHAR(50) = @Username + '_db';
        DECLARE @SQL NVARCHAR(MAX);
        
        -- Xóa khỏi tất cả roles
        SET @SQL = N'
        DECLARE @RoleName NVARCHAR(50);
        DECLARE role_cursor CURSOR FOR
        SELECT name FROM sys.database_role_members rm
        JOIN sys.database_principals r ON r.principal_id = rm.role_principal_id
        JOIN sys.database_principals u ON u.principal_id = rm.member_principal_id
        WHERE u.name = ''' + @DBUser + ''';
        
        OPEN role_cursor;
        FETCH NEXT FROM role_cursor INTO @RoleName;
        
        WHILE @@FETCH_STATUS = 0
        BEGIN
            EXEC(''ALTER ROLE ['' + @RoleName + ''] DROP MEMBER [' + @DBUser + '];'');
            FETCH NEXT FROM role_cursor INTO @RoleName;
        END
        
        CLOSE role_cursor;
        DEALLOCATE role_cursor;';
        
        EXEC sp_executesql @SQL;
        
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
    @Username = N'testuser01',
    @PasswordHash = N'123456',  -- Trong thực tế phải hash SHA2-256
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
