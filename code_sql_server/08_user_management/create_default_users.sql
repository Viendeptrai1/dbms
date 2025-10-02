/*
=============================================
CREATE DEFAULT USERS FROM SCRATCH
=============================================
Mô tả: Tạo lại tất cả user mặc định từ đầu
*/

USE QLNhapHang;
GO

PRINT '=== XÓA TẤT CẢ USER CŨ ===';

-- Xóa tất cả user roles trước
DELETE FROM dbo.UsersRoles;

-- Xóa tất cả users
DELETE FROM dbo.Users;

PRINT '=== TẠO LẠI TẤT CẢ USERS ===';

-- Tạo Admin user
INSERT INTO dbo.Users (Username, PasswordHash, FullName, IsActive)
VALUES ('admin', HASHBYTES('SHA2_256', 'admin123'), N'Administrator', 1);

DECLARE @AdminUserID INT = SCOPE_IDENTITY();

-- Tạo Seller user
INSERT INTO dbo.Users (Username, PasswordHash, FullName, IsActive)
VALUES ('seller', HASHBYTES('SHA2_256', 'seller123'), N'Seller User', 1);

DECLARE @SellerUserID INT = SCOPE_IDENTITY();

-- Tạo User user
INSERT INTO dbo.Users (Username, PasswordHash, FullName, IsActive)
VALUES ('user', HASHBYTES('SHA2_256', 'user123'), N'Basic User', 1);

DECLARE @UserUserID INT = SCOPE_IDENTITY();

PRINT '=== GÁN ROLES ===';

-- Gán role Admin cho admin
INSERT INTO dbo.UsersRoles (UserID, RoleID)
SELECT @AdminUserID, RoleID FROM dbo.Roles WHERE RoleName = 'Admin';

-- Gán role Seller cho seller
INSERT INTO dbo.UsersRoles (UserID, RoleID)
SELECT @SellerUserID, RoleID FROM dbo.Roles WHERE RoleName = 'Seller';

-- Gán role User cho user
INSERT INTO dbo.UsersRoles (UserID, RoleID)
SELECT @UserUserID, RoleID FROM dbo.Roles WHERE RoleName = 'User';

PRINT '=== KIỂM TRA KẾT QUẢ ===';

SELECT 
    u.Username,
    u.FullName,
    r.RoleName,
    u.IsActive,
    LEN(u.PasswordHash) AS HashLength
FROM dbo.Users u
JOIN dbo.UsersRoles ur ON u.UserID = ur.UserID
JOIN dbo.Roles r ON ur.RoleID = r.RoleID
ORDER BY u.Username;

PRINT '=== TEST LOGIN CHO TỪNG USER ===';

-- Test admin login
SELECT 'Testing admin login...' AS Test;
SELECT 
    u.Username,
    r.RoleName,
    'Admin login OK' AS Status
FROM dbo.Users u
JOIN dbo.UsersRoles ur ON u.UserID = ur.UserID
JOIN dbo.Roles r ON ur.RoleID = r.RoleID
WHERE u.Username = 'admin' 
AND u.PasswordHash = HASHBYTES('SHA2_256', 'admin123')
AND u.IsActive = 1;

-- Test seller login
SELECT 'Testing seller login...' AS Test;
SELECT 
    u.Username,
    r.RoleName,
    'Seller login OK' AS Status
FROM dbo.Users u
JOIN dbo.UsersRoles ur ON u.UserID = ur.UserID
JOIN dbo.Roles r ON ur.RoleID = r.RoleID
WHERE u.Username = 'seller' 
AND u.PasswordHash = HASHBYTES('SHA2_256', 'seller123')
AND u.IsActive = 1;

-- Test user login
SELECT 'Testing user login...' AS Test;
SELECT 
    u.Username,
    r.RoleName,
    'User login OK' AS Status
FROM dbo.Users u
JOIN dbo.UsersRoles ur ON u.UserID = ur.UserID
JOIN dbo.Roles r ON ur.RoleID = r.RoleID
WHERE u.Username = 'user' 
AND u.PasswordHash = HASHBYTES('SHA2_256', 'user123')
AND u.IsActive = 1;

PRINT '=== HOÀN THÀNH ===';
PRINT 'Tất cả user đã được tạo lại. Thử đăng nhập:';
PRINT 'Admin: admin / admin123';
PRINT 'Seller: seller / seller123';
PRINT 'User: user / user123';
