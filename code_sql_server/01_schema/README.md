# 📊 01_schema - Database Schema

## 📁 **Nội dung thư mục**
- `create_db.sql` - Tạo database và các bảng cơ bản
- `phanquyen.sql` - Tạo roles và phân quyền

## 🚀 **Cách sử dụng**

### **1. create_db.sql**
Tạo database `QLNhapHang` với các bảng:
- **Users** - Bảng người dùng
- **Roles** - Bảng vai trò (Admin, Seller, User)
- **UsersRoles** - Bảng trung gian M-N
- **Categories** - Bảng danh mục sản phẩm
- **Products** - Bảng sản phẩm
- **ProductCategories** - Bảng trung gian M-N
- **Suppliers** - Bảng nhà cung cấp
- **GoodsReceipts** - Bảng phiếu nhập hàng
- **GoodsReceiptDetails** - Bảng chi tiết phiếu nhập

### **2. phanquyen.sql**
Tạo database roles và phân quyền:
- **dbrole_Admin** - Quyền Admin (toàn quyền)
- **dbrole_Seller** - Quyền Seller (nghiệp vụ)
- **dbrole_User** - Quyền User (chỉ đọc)

## ⚠️ **Lưu ý quan trọng**
- **Luôn chạy `create_db.sql` trước**
- **Sau đó chạy `phanquyen.sql`**
- **Không được đảo thứ tự**
