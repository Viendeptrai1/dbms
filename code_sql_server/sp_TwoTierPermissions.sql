/*
=============================================
PHÂN QUYỀN 2 TẦNG - HOÀN CHỈNH
=============================================
Tích hợp Application Users + SQL Server Security
Tự động GRANT/REVOKE quyền khi thay đổi role
=============================================
*/

USE QLNhapHang;
GO

-- =============================================
-- 1. SP TẠO USER VÀ SQL LOGIN ĐỒNG THỜI
-- =============================================
IF OBJECT_ID('dbo.sp_CreateUserWithSQLLogin', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_CreateUserWithSQLLogin;
GO

CREATE PROCEDURE sp_CreateUserWithSQLLogin
    @Username VARCHAR(50),
    @Password NVARCHAR(100),  -- Plain text password
    @FullName NVARCHAR(100),
    @RoleName NVARCHAR(20),   -- 'Admin', 'Seller', 'User'
    @IsActive BIT = 1,
    @NewUserID INT OUTPUT,
    @Message NVARCHAR(MAX) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT OFF;
    
    DECLARE @SQL NVARCHAR(MAX);
    DECLARE @DBRole VARCHAR(50);
    DECLARE @RoleID INT;
    DECLARE @Steps NVARCHAR(MAX) = '';
    
    BEGIN TRY
        -- ========================================
        -- VALIDATE
        -- ========================================
        IF EXISTS (SELECT 1 FROM dbo.Users WHERE Username = @Username)
        BEGIN
            SET @Message = N'❌ Username [' + @Username + N'] đã tồn tại!';
            SET @NewUserID = -1;
            RETURN;
        END
        
        IF NOT EXISTS (SELECT 1 FROM dbo.Roles WHERE RoleName = @RoleName)
        BEGIN
            SET @Message = N'❌ Role [' + @RoleName + N'] không hợp lệ! Chỉ chấp nhận: Admin, Seller, User';
            SET @NewUserID = -1;
            RETURN;
        END
        
        SELECT @RoleID = RoleID FROM dbo.Roles WHERE RoleName = @RoleName;
        SET @DBRole = 'dbrole_' + @RoleName;
        
        -- ========================================
        -- TẠO APPLICATION USER (TẦNG 1)
        -- ========================================
        BEGIN TRANSACTION AppUserTxn;
        
        INSERT INTO dbo.Users (Username, Password, FullName, IsActive)
        VALUES (@Username, @Password, @FullName, @IsActive);
        
        SET @NewUserID = SCOPE_IDENTITY();
        
        INSERT INTO dbo.UsersRoles (UserID, RoleID)
        VALUES (@NewUserID, @RoleID);
        
        COMMIT TRANSACTION AppUserTxn;
        SET @Steps += N'✅ Application User đã tạo' + CHAR(13) + CHAR(10);
        
        -- ========================================
        -- TẠO SQL SERVER LOGIN (TẦNG 2)
        -- ========================================
        BEGIN TRY
            IF NOT EXISTS (SELECT 1 FROM sys.server_principals WHERE name = @Username AND type = 'S')
            BEGIN
                SET @SQL = N'CREATE LOGIN [' + @Username + N'] WITH PASSWORD = ''' + 
                           @Password + N''', DEFAULT_DATABASE = [QLNhapHang], CHECK_POLICY = OFF;';
                EXEC sp_executesql @SQL;
                SET @Steps += N'✅ SQL Login đã tạo' + CHAR(13) + CHAR(10);
            END
            ELSE
                SET @Steps += N'⚠️ SQL Login đã tồn tại, reuse' + CHAR(13) + CHAR(10);
        END TRY
        BEGIN CATCH
            SET @Steps += N'⚠️ Không tạo được SQL Login: ' + ERROR_MESSAGE() + CHAR(13) + CHAR(10);
        END CATCH
        
        -- ========================================
        -- TẠO DATABASE USER
        -- ========================================
        BEGIN TRY
            IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = @Username AND type = 'S')
            BEGIN
                SET @SQL = N'CREATE USER [' + @Username + N'] FOR LOGIN [' + @Username + N'];';
                EXEC sp_executesql @SQL;
                SET @Steps += N'✅ Database User đã tạo' + CHAR(13) + CHAR(10);
            END
            ELSE
                SET @Steps += N'⚠️ Database User đã tồn tại' + CHAR(13) + CHAR(10);
        END TRY
        BEGIN CATCH
            SET @Steps += N'⚠️ Không tạo được Database User: ' + ERROR_MESSAGE() + CHAR(13) + CHAR(10);
        END CATCH
        
        -- ========================================
        -- GÁN DATABASE ROLE → TỰ ĐỘNG GRANT QUYỀN
        -- ========================================
        BEGIN TRY
            IF EXISTS (SELECT 1 FROM sys.database_principals WHERE name = @DBRole AND type = 'R')
            BEGIN
                IF NOT EXISTS (
                    SELECT 1 FROM sys.database_role_members drm
                    JOIN sys.database_principals r ON drm.role_principal_id = r.principal_id
                    JOIN sys.database_principals u ON drm.member_principal_id = u.principal_id
                    WHERE r.name = @DBRole AND u.name = @Username
                )
                BEGIN
                    SET @SQL = N'ALTER ROLE [' + @DBRole + N'] ADD MEMBER [' + @Username + N'];';
                    EXEC sp_executesql @SQL;
                    SET @Steps += N'✅ Đã gán Database Role [' + @DBRole + N']' + CHAR(13) + CHAR(10);
                    SET @Steps += N'   👉 Quyền đã được cấp tự động' + CHAR(13) + CHAR(10);
                END
            END
        END TRY
        BEGIN CATCH
            SET @Steps += N'⚠️ Không gán được Database Role: ' + ERROR_MESSAGE() + CHAR(13) + CHAR(10);
        END CATCH
        
        -- ========================================
        -- TỔNG KẾT
        -- ========================================
        SET @Message = N'🎉 TẠO USER THÀNH CÔNG!' + CHAR(13) + CHAR(10) +
                       N'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━' + CHAR(13) + CHAR(10) +
                       @Steps + 
                       CHAR(13) + CHAR(10) +
                       N'📋 THÔNG TIN:' + CHAR(13) + CHAR(10) +
                       N'  👤 Username: ' + @Username + CHAR(13) + CHAR(10) +
                       N'  📝 Full Name: ' + @FullName + CHAR(13) + CHAR(10) +
                       N'  🎭 App Role: ' + @RoleName + CHAR(13) + CHAR(10) +
                       N'  🛡️ DB Role: ' + @DBRole + CHAR(13) + CHAR(10);
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION AppUserTxn;
            
        SET @Message = N'❌ LỖI: ' + ERROR_MESSAGE();
        SET @NewUserID = -1;
    END CATCH
END
GO

-- =============================================
-- 2. SP XÓA USER (HARD/SOFT DELETE)
-- =============================================
IF OBJECT_ID('dbo.sp_DeleteUserComplete', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_DeleteUserComplete;
GO

CREATE PROCEDURE sp_DeleteUserComplete
    @Username VARCHAR(50),
    @CurrentUsername VARCHAR(50),  -- Username của admin đang thực hiện
    @DeleteType VARCHAR(10),       -- 'HARD' hoặc 'SOFT'
    @Message NVARCHAR(MAX) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT OFF;
    
    DECLARE @SQL NVARCHAR(MAX);
    DECLARE @UserID INT;
    DECLARE @DBRole VARCHAR(50);
    DECLARE @RoleName NVARCHAR(20);
    DECLARE @Steps NVARCHAR(MAX) = '';
    
    BEGIN TRY
        -- ========================================
        -- VALIDATE
        -- ========================================
        IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Username = @Username)
        BEGIN
            SET @Message = N'❌ User [' + @Username + N'] không tồn tại!';
            RETURN;
        END
        
        -- KHÔNG CHO ADMIN TỰ XÓA CHÍNH MÌNH
        IF @Username = @CurrentUsername
        BEGIN
            SET @Message = N'❌ BẠN KHÔNG THỂ XÓA CHÍNH MÌNH!' + CHAR(13) + CHAR(10) +
                           N'Vui lòng dùng tài khoản admin khác để thực hiện.';
            RETURN;
        END
        
        SELECT @UserID = UserID FROM dbo.Users WHERE Username = @Username;
        
        SELECT @RoleName = r.RoleName
        FROM dbo.UsersRoles ur
        JOIN dbo.Roles r ON ur.RoleID = r.RoleID
        WHERE ur.UserID = @UserID;
        
        SET @DBRole = 'dbrole_' + @RoleName;
        
        -- ========================================
        -- SOFT DELETE
        -- ========================================
        IF @DeleteType = 'SOFT'
        BEGIN
            BEGIN TRANSACTION SoftDeleteTxn;
            
            -- Disable SQL Login (nếu có)
            BEGIN TRY
                IF EXISTS (SELECT 1 FROM sys.server_principals WHERE name = @Username AND type = 'S')
                BEGIN
                    SET @SQL = N'ALTER LOGIN [' + @Username + N'] DISABLE;';
                    EXEC sp_executesql @SQL;
                    SET @Steps += N'✅ SQL Login đã DISABLE' + CHAR(13) + CHAR(10);
                END
            END TRY
            BEGIN CATCH
                SET @Steps += N'⚠️ Không DISABLE được Login: ' + ERROR_MESSAGE() + CHAR(13) + CHAR(10);
            END CATCH
            
            -- Đánh dấu inactive trong app
            UPDATE dbo.Users 
            SET IsActive = 0,
                Username = Username + '_DELETED_' + CONVERT(VARCHAR(20), GETDATE(), 112) + '_' + CONVERT(VARCHAR(20), @UserID)
            WHERE UserID = @UserID;
            
            COMMIT TRANSACTION SoftDeleteTxn;
            SET @Steps += N'✅ User đã đánh dấu INACTIVE' + CHAR(13) + CHAR(10);
            
            SET @Message = N'✅ SOFT DELETE THÀNH CÔNG!' + CHAR(13) + CHAR(10) +
                           N'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━' + CHAR(13) + CHAR(10) +
                           @Steps +
                           CHAR(13) + CHAR(10) +
                           N'📌 Lưu ý:' + CHAR(13) + CHAR(10) +
                           N'  • User không thể đăng nhập' + CHAR(13) + CHAR(10) +
                           N'  • Dữ liệu vẫn được giữ lại' + CHAR(13) + CHAR(10) +
                           N'  • SQL Login bị DISABLE' + CHAR(13) + CHAR(10) +
                           N'  • Có thể khôi phục sau';
            RETURN;
        END
        
        -- ========================================
        -- HARD DELETE
        -- ========================================
        IF @DeleteType = 'HARD'
        BEGIN
            -- Xóa SQL Server objects trước (bottom-up)
            
            -- 1. DROP MEMBER khỏi Database Role
            BEGIN TRY
                IF EXISTS (
                    SELECT 1 FROM sys.database_role_members drm
                    JOIN sys.database_principals r ON drm.role_principal_id = r.principal_id
                    JOIN sys.database_principals u ON drm.member_principal_id = u.principal_id
                    WHERE r.name = @DBRole AND u.name = @Username
                )
                BEGIN
                    SET @SQL = N'ALTER ROLE [' + @DBRole + N'] DROP MEMBER [' + @Username + N'];';
                    EXEC sp_executesql @SQL;
                    SET @Steps += N'✅ Đã rời Database Role' + CHAR(13) + CHAR(10);
                    SET @Steps += N'   👉 Quyền đã bị thu hồi tự động' + CHAR(13) + CHAR(10);
                END
            END TRY
            BEGIN CATCH
                SET @Steps += N'⚠️ Không DROP MEMBER được: ' + ERROR_MESSAGE() + CHAR(13) + CHAR(10);
            END CATCH
            
            -- 2. DROP DATABASE USER
            BEGIN TRY
                IF EXISTS (SELECT 1 FROM sys.database_principals WHERE name = @Username AND type = 'S')
                BEGIN
                    SET @SQL = N'DROP USER [' + @Username + N'];';
                    EXEC sp_executesql @SQL;
                    SET @Steps += N'✅ Database User đã xóa' + CHAR(13) + CHAR(10);
                END
            END TRY
            BEGIN CATCH
                SET @Steps += N'⚠️ Không DROP USER được: ' + ERROR_MESSAGE() + CHAR(13) + CHAR(10);
            END CATCH
            
            -- 3. DROP SQL LOGIN
            BEGIN TRY
                IF EXISTS (SELECT 1 FROM sys.server_principals WHERE name = @Username AND type = 'S')
                BEGIN
                    SET @SQL = N'DROP LOGIN [' + @Username + N'];';
                    EXEC sp_executesql @SQL;
                    SET @Steps += N'✅ SQL Login đã xóa' + CHAR(13) + CHAR(10);
                END
            END TRY
            BEGIN CATCH
                SET @Steps += N'⚠️ Không DROP LOGIN được: ' + ERROR_MESSAGE() + CHAR(13) + CHAR(10);
            END CATCH
            
            -- 4. Xóa Application data
            BEGIN TRANSACTION HardDeleteTxn;
            
            DELETE FROM dbo.UsersRoles WHERE UserID = @UserID;
            SET @Steps += N'✅ UsersRoles đã xóa' + CHAR(13) + CHAR(10);
            
            DELETE FROM dbo.Users WHERE UserID = @UserID;
            SET @Steps += N'✅ Users đã xóa' + CHAR(13) + CHAR(10);
            
            COMMIT TRANSACTION HardDeleteTxn;
            
            SET @Message = N'✅ HARD DELETE HOÀN TẤT!' + CHAR(13) + CHAR(10) +
                           N'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━' + CHAR(13) + CHAR(10) +
                           @Steps +
                           CHAR(13) + CHAR(10) +
                           N'⚠️ Lưu ý:' + CHAR(13) + CHAR(10) +
                           N'  • User đã bị xóa vĩnh viễn' + CHAR(13) + CHAR(10) +
                           N'  • SQL Login/User đã xóa' + CHAR(13) + CHAR(10) +
                           N'  • KHÔNG THỂ HOÀN TÁC!';
            RETURN;
        END
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK;
            
        SET @Message = N'❌ LỖI: ' + ERROR_MESSAGE();
    END CATCH
END
GO

-- =============================================
-- 3. SP THAY ĐỔI ROLE HOÀN CHỈNH
-- =============================================
IF OBJECT_ID('dbo.sp_ChangeUserRoleComplete', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_ChangeUserRoleComplete;
GO

CREATE PROCEDURE sp_ChangeUserRoleComplete
    @Username NVARCHAR(50),
    @NewRoleName NVARCHAR(20),
    @CurrentUsername VARCHAR(50),  -- Username của admin đang thực hiện
    @Message NVARCHAR(MAX) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @SQL NVARCHAR(MAX);
    DECLARE @UserID INT;
    DECLARE @OldRoleName NVARCHAR(20);
    DECLARE @OldDBRole VARCHAR(50);
    DECLARE @NewDBRole VARCHAR(50);
    DECLARE @NewRoleID INT;
    DECLARE @Steps NVARCHAR(MAX) = '';
    
    BEGIN TRY
        -- ========================================
        -- VALIDATE
        -- ========================================
        IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Username = @Username)
        BEGIN
            SET @Message = N'❌ User không tồn tại!';
            RETURN;
        END
        
        -- KHÔNG CHO ADMIN TỰ THAY ĐỔI ROLE CHÍNH MÌNH
        IF @Username = @CurrentUsername
        BEGIN
            SET @Message = N'❌ BẠN KHÔNG THỂ THAY ĐỔI ROLE CHÍNH MÌNH!' + CHAR(13) + CHAR(10) +
                           N'Vui lòng dùng tài khoản admin khác để thực hiện.';
            RETURN;
        END
        
        IF NOT EXISTS (SELECT 1 FROM dbo.Roles WHERE RoleName = @NewRoleName)
        BEGIN
            SET @Message = N'❌ Role mới không hợp lệ!';
            RETURN;
        END
        
        SELECT @UserID = UserID FROM dbo.Users WHERE Username = @Username;
        
        SELECT @OldRoleName = r.RoleName
        FROM dbo.UsersRoles ur
        JOIN dbo.Roles r ON ur.RoleID = r.RoleID
        WHERE ur.UserID = @UserID;
        
        IF @OldRoleName = @NewRoleName
        BEGIN
            SET @Message = N'ℹ️ User đã có role [' + @NewRoleName + N'] rồi!';
            RETURN;
        END
        
        SET @OldDBRole = 'dbrole_' + @OldRoleName;
        SET @NewDBRole = 'dbrole_' + @NewRoleName;
        
        SELECT @NewRoleID = RoleID FROM dbo.Roles WHERE RoleName = @NewRoleName;
        
        -- ========================================
        -- THAY ĐỔI SQL DATABASE ROLE (TẦNG 2)
        -- ========================================
        
        -- Rời role cũ
        BEGIN TRY
            IF EXISTS (
                SELECT 1 FROM sys.database_role_members drm
                JOIN sys.database_principals r ON drm.role_principal_id = r.principal_id
                JOIN sys.database_principals u ON drm.member_principal_id = u.principal_id
                WHERE r.name = @OldDBRole AND u.name = @Username
            )
            BEGIN
                SET @SQL = N'ALTER ROLE [' + @OldDBRole + N'] DROP MEMBER [' + @Username + N'];';
                EXEC sp_executesql @SQL;
                SET @Steps += N'✅ Đã rời role cũ [' + @OldDBRole + N']' + CHAR(13) + CHAR(10);
                SET @Steps += N'   👉 Quyền ' + @OldRoleName + N' đã bị thu hồi' + CHAR(13) + CHAR(10);
            END
        END TRY
        BEGIN CATCH
            SET @Steps += N'⚠️ Không rời được role cũ: ' + ERROR_MESSAGE() + CHAR(13) + CHAR(10);
        END CATCH
        
        -- Vào role mới
        BEGIN TRY
            IF EXISTS (SELECT 1 FROM sys.database_principals WHERE name = @NewDBRole AND type = 'R')
            BEGIN
                IF NOT EXISTS (
                    SELECT 1 FROM sys.database_role_members drm
                    JOIN sys.database_principals r ON drm.role_principal_id = r.principal_id
                    JOIN sys.database_principals u ON drm.member_principal_id = u.principal_id
                    WHERE r.name = @NewDBRole AND u.name = @Username
                )
                BEGIN
                    SET @SQL = N'ALTER ROLE [' + @NewDBRole + N'] ADD MEMBER [' + @Username + N'];';
                    EXEC sp_executesql @SQL;
                    SET @Steps += N'✅ Đã vào role mới [' + @NewDBRole + N']' + CHAR(13) + CHAR(10);
                    SET @Steps += N'   👉 Quyền ' + @NewRoleName + N' đã được cấp' + CHAR(13) + CHAR(10);
                END
            END
        END TRY
        BEGIN CATCH
            SET @Steps += N'⚠️ Không vào được role mới: ' + ERROR_MESSAGE() + CHAR(13) + CHAR(10);
        END CATCH
        
        -- ========================================
        -- CẬP NHẬT APPLICATION ROLE (TẦNG 1)
        -- ========================================
        BEGIN TRANSACTION ChangeRoleTxn;
        
        DELETE FROM dbo.UsersRoles WHERE UserID = @UserID;
        
        INSERT INTO dbo.UsersRoles (UserID, RoleID)
        VALUES (@UserID, @NewRoleID);
        
        COMMIT TRANSACTION ChangeRoleTxn;
        SET @Steps += N'✅ Application role đã cập nhật' + CHAR(13) + CHAR(10);
        
        -- ========================================
        -- TỔNG KẾT
        -- ========================================
        SET @Message = N'🎉 THAY ĐỔI ROLE THÀNH CÔNG!' + CHAR(13) + CHAR(10) +
                       N'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━' + CHAR(13) + CHAR(10) +
                       @Steps +
                       CHAR(13) + CHAR(10) +
                       N'📋 THÔNG TIN:' + CHAR(13) + CHAR(10) +
                       N'  👤 Username: ' + @Username + CHAR(13) + CHAR(10) +
                       N'  🔄 Role: ' + @OldRoleName + N' → ' + @NewRoleName + CHAR(13) + CHAR(10) +
                       N'  🛡️ DB Role: ' + @OldDBRole + N' → ' + @NewDBRole;
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION ChangeRoleTxn;
            
        SET @Message = N'❌ LỖI: ' + ERROR_MESSAGE();
    END CATCH
END
GO

-- =============================================
-- 4. SP TẠO SQL LOGIN CHO USER ĐÃ TỒN TẠI
-- =============================================
IF OBJECT_ID('dbo.sp_CreateSQLLoginForExistingUser', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_CreateSQLLoginForExistingUser;
GO

CREATE PROCEDURE sp_CreateSQLLoginForExistingUser
    @Username VARCHAR(50),
    @Password NVARCHAR(100),  -- Plain text password cho SQL login
    @Message NVARCHAR(MAX) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @SQL NVARCHAR(MAX);
    DECLARE @RoleName NVARCHAR(20);
    DECLARE @DBRole VARCHAR(50);
    DECLARE @Steps NVARCHAR(MAX) = '';
    
    BEGIN TRY
        -- ========================================
        -- VALIDATE
        -- ========================================
        IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Username = @Username)
        BEGIN
            SET @Message = N'❌ User [' + @Username + N'] không tồn tại trong bảng Users!';
            RETURN;
        END
        
        -- Lấy role của user
        SELECT @RoleName = r.RoleName
        FROM dbo.Users u
        JOIN dbo.UsersRoles ur ON u.UserID = ur.UserID
        JOIN dbo.Roles r ON ur.RoleID = r.RoleID
        WHERE u.Username = @Username;
        
        SET @DBRole = 'dbrole_' + @RoleName;
        
        -- ========================================
        -- TẠO SQL SERVER LOGIN
        -- ========================================
        IF EXISTS (SELECT 1 FROM sys.server_principals WHERE name = @Username AND type = 'S')
        BEGIN
            SET @Steps += N'⚠️ SQL Login [' + @Username + N'] đã tồn tại' + CHAR(13) + CHAR(10);
        END
        ELSE
        BEGIN
            BEGIN TRY
                SET @SQL = N'CREATE LOGIN [' + @Username + N'] WITH PASSWORD = ''' + 
                           @Password + N''', DEFAULT_DATABASE = [QLNhapHang], CHECK_POLICY = OFF;';
                EXEC sp_executesql @SQL;
                SET @Steps += N'✅ SQL Login đã tạo' + CHAR(13) + CHAR(10);
            END TRY
            BEGIN CATCH
                SET @Steps += N'❌ Không tạo được SQL Login: ' + ERROR_MESSAGE() + CHAR(13) + CHAR(10);
                SET @Message = @Steps;
                RETURN;
            END CATCH
        END
        
        -- ========================================
        -- TẠO DATABASE USER
        -- ========================================
        IF EXISTS (SELECT 1 FROM sys.database_principals WHERE name = @Username AND type = 'S')
        BEGIN
            SET @Steps += N'⚠️ Database User [' + @Username + N'] đã tồn tại' + CHAR(13) + CHAR(10);
        END
        ELSE
        BEGIN
            BEGIN TRY
                SET @SQL = N'CREATE USER [' + @Username + N'] FOR LOGIN [' + @Username + N'];';
                EXEC sp_executesql @SQL;
                SET @Steps += N'✅ Database User đã tạo' + CHAR(13) + CHAR(10);
            END TRY
            BEGIN CATCH
                SET @Steps += N'❌ Không tạo được Database User: ' + ERROR_MESSAGE() + CHAR(13) + CHAR(10);
                SET @Message = @Steps;
                RETURN;
            END CATCH
        END
        
        -- ========================================
        -- GÁN DATABASE ROLE
        -- ========================================
        IF @DBRole IS NOT NULL AND EXISTS (SELECT 1 FROM sys.database_principals WHERE name = @DBRole AND type = 'R')
        BEGIN
            BEGIN TRY
                IF NOT EXISTS (
                    SELECT 1 FROM sys.database_role_members drm
                    JOIN sys.database_principals r ON drm.role_principal_id = r.principal_id
                    JOIN sys.database_principals u ON drm.member_principal_id = u.principal_id
                    WHERE r.name = @DBRole AND u.name = @Username
                )
                BEGIN
                    SET @SQL = N'ALTER ROLE [' + @DBRole + N'] ADD MEMBER [' + @Username + N'];';
                    EXEC sp_executesql @SQL;
                    SET @Steps += N'✅ Đã gán Database Role [' + @DBRole + N']' + CHAR(13) + CHAR(10);
                    SET @Steps += N'   👉 Quyền đã được cấp tự động' + CHAR(13) + CHAR(10);
                END
                ELSE
                BEGIN
                    SET @Steps += N'ℹ️ User đã là member của [' + @DBRole + N']' + CHAR(13) + CHAR(10);
                END
            END TRY
            BEGIN CATCH
                SET @Steps += N'❌ Không gán được Database Role: ' + ERROR_MESSAGE() + CHAR(13) + CHAR(10);
            END CATCH
        END
        ELSE
        BEGIN
            SET @Steps += N'⚠️ Database Role [' + @DBRole + N'] không tồn tại!' + CHAR(13) + CHAR(10);
        END
        
        -- ========================================
        -- TỔNG KẾT
        -- ========================================
        SET @Message = N'✅ HOÀN THÀNH!' + CHAR(13) + CHAR(10) +
                       N'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━' + CHAR(13) + CHAR(10) +
                       @Steps +
                       CHAR(13) + CHAR(10) +
                       N'📋 THÔNG TIN:' + CHAR(13) + CHAR(10) +
                       N'  👤 Username: ' + @Username + CHAR(13) + CHAR(10) +
                       N'  🎭 App Role: ' + @RoleName + CHAR(13) + CHAR(10) +
                       N'  🛡️ DB Role: ' + @DBRole;
        
    END TRY
    BEGIN CATCH
        SET @Message = N'❌ LỖI: ' + ERROR_MESSAGE();
    END CATCH
END
GO

-- =============================================
-- 5. SP XEM THÔNG TIN ĐẦY ĐỦ
-- =============================================
IF OBJECT_ID('dbo.sp_GetFullUserInfo', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_GetFullUserInfo;
GO

CREATE PROCEDURE sp_GetFullUserInfo
    @Username VARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        u.UserID,
        u.Username,
        u.FullName,
        CASE WHEN u.IsActive = 1 THEN N'✅ Active' ELSE N'❌ Inactive' END AS Status,
        r.RoleName AS AppRole,
        CASE WHEN sl.name IS NOT NULL THEN N'✅' ELSE N'❌' END AS HasSQLLogin,
        CASE WHEN du.name IS NOT NULL THEN N'✅' ELSE N'❌' END AS HasDBUser,
        STUFF((
            SELECT ', ' + dp.name
            FROM sys.database_role_members drm
            JOIN sys.database_principals dp ON drm.role_principal_id = dp.principal_id
            WHERE drm.member_principal_id = du.principal_id
            FOR XML PATH('')
        ), 1, 2, '') AS DBRoles
    FROM dbo.Users u
    LEFT JOIN dbo.UsersRoles ur ON u.UserID = ur.UserID
    LEFT JOIN dbo.Roles r ON ur.RoleID = r.RoleID
    LEFT JOIN sys.server_principals sl ON sl.name = u.Username AND sl.type = 'S'
    LEFT JOIN sys.database_principals du ON du.name = u.Username AND du.type = 'S'
    WHERE (@Username IS NULL OR u.Username = @Username)
        AND u.Username NOT LIKE '%_DELETED_%'  -- Không hiển thị user đã soft delete
    ORDER BY u.Username;
END
GO

-- =============================================
-- GRANT QUYỀN
-- =============================================
GRANT EXECUTE ON OBJECT::dbo.sp_CreateUserWithSQLLogin TO dbrole_Admin;
GRANT EXECUTE ON OBJECT::dbo.sp_DeleteUserComplete TO dbrole_Admin;
GRANT EXECUTE ON OBJECT::dbo.sp_ChangeUserRoleComplete TO dbrole_Admin;
GRANT EXECUTE ON OBJECT::dbo.sp_CreateSQLLoginForExistingUser TO dbrole_Admin;
GRANT EXECUTE ON OBJECT::dbo.sp_GetFullUserInfo TO dbrole_Admin;
GO

PRINT N'';
PRINT N'✅✅✅ HOÀN TẤT SETUP STORED PROCEDURES! ✅✅✅';
PRINT N'';
PRINT N'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━';
PRINT N'📋 ĐÃ TẠO CÁC STORED PROCEDURES:';
PRINT N'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━';
PRINT N'  ✅ sp_CreateUserWithSQLLogin';
PRINT N'  ✅ sp_DeleteUserComplete (Hard/Soft)';
PRINT N'  ✅ sp_ChangeUserRoleComplete';
PRINT N'  ✅ sp_CreateSQLLoginForExistingUser';
PRINT N'  ✅ sp_GetFullUserInfo';
PRINT N'';
PRINT N'🔐 TÍNH NĂNG:';
PRINT N'  • Tự động GRANT quyền khi ADD vào role';
PRINT N'  • Tự động REVOKE quyền khi DROP khỏi role';
PRINT N'  • Admin không thể tự xóa/sửa chính mình';
PRINT N'  • Hỗ trợ Hard/Soft Delete';
GO
