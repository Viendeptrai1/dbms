# 👥 08_user_management - User Management

## 📁 **Nội dung thư mục**
- `sp_user_management_fixed.sql` - Stored procedures quản lý user và role (FIXED - KHUYẾN NGHỊ)
- `sp_user_management.sql` - Stored procedures quản lý user và role (ORIGINAL)
- `create_default_users.sql` - Script tạo users mặc định

## 🚀 **Cách sử dụng**

### **1. sp_user_management_fixed.sql (KHUYẾN NGHỊ)**
Chứa 7 stored procedures đã được fix:

#### **Stored Procedures:**
- **`sp_CreateUserWithRole`** - Tạo user mới và gán role
- **`sp_ChangeUserRole`** - Thay đổi role của user
- **`sp_ToggleUserStatus`** - Khóa/mở khóa user
- **`sp_DeleteUser`** - Xóa user (soft delete)
- **`sp_GetUsersWithRoles`** - Lấy danh sách users với roles
- **`sp_CheckUserPermission`** - Kiểm tra quyền user
- **`sp_RevokeAllUserPermissions`** - Revoke tất cả quyền của user

#### **Parameters:**
```sql
-- Tạo user mới
EXEC sp_CreateUserWithRole 
    @Username = 'newuser',
    @Password = 'password123',        -- Plain text password
    @FullName = N'Tên đầy đủ',
    @RoleName = 'Seller',
    @NewUserID = @NewUserID OUTPUT,
    @Message = @Message OUTPUT;
```

### **2. create_default_users.sql**
Tạo 3 users mặc định:
- **admin** / admin123 (Role: Admin)
- **seller** / seller123 (Role: Seller)
- **user** / user123 (Role: User)

## ⚠️ **Lưu ý quan trọng**
- **Dùng `sp_user_management_fixed.sql`** (đã fix lỗi)
- **Password được hash bằng SQL HASHBYTES('SHA2_256', password)**
- **Tất cả users được tạo với IsActive = 1**
- **Chạy `create_default_users.sql` sau khi setup schema**
