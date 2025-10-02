# Hệ thống Quản lý Nhập hàng - QLNhapHang

## 🎯 Tổng quan

Đây là một hệ thống quản lý nhập hàng hoàn chỉnh được phát triển trên nền tảng **Windows Forms (.NET Framework 4.7.2)** với cơ sở dữ liệu **SQL Server**. Hệ thống được thiết kế theo mô hình phân quyền 3 lớp với các vai trò khác nhau.

## 🏗️ Kiến trúc hệ thống

### **Lớp Cơ sở dữ liệu (Database Layer)**
- **Database**: QLNhapHang
- **Tables**: Users, Roles, UsersRoles, Categories, Products, ProductCategories, Suppliers, GoodsReceipts, GoodsReceiptDetails, ProductPriceHistory
- **Views**: vw_ProductsWithCategories, vw_InventoryValuation, vw_ImportLines
- **Stored Procedures**: sp_AddProductWithCategories, sp_BulkAdjustPriceByPercent, sp_CreateGoodsReceipt, sp_DeleteGoodsReceipt
- **Functions**: fn_ProductsByCategory, fn_TopProductsByImportValue, fn_ImportSummaryBySupplier
- **User-Defined Types**: udt_GoodsReceiptLine, udt_SKUList, udt_CategoryNameList
- **Triggers**: TR_GRD_StockAndTotal, TR_Products_Normalize, TR_Product_LogPriceChange, TR_Categories_PreventDeleteIfInUse, TR_DDL_PreventDrop

### **Lớp Ứng dụng (Application Layer)**
- **Authentication**: Hệ thống đăng nhập với mã hóa SHA2-256
- **Authorization**: Phân quyền 3 cấp độ (Admin, Seller, User)
- **Business Logic**: Xử lý nghiệp vụ nhập hàng, quản lý sản phẩm, báo cáo

### **Lớp Giao diện (Presentation Layer)**
- **LoginForm**: Form đăng nhập
- **AdminMainForm**: Giao diện quản trị viên
- **SellerMainForm**: Giao diện nhân viên nghiệp vụ
- **ProductSearchForm**: Giao diện tra cứu (cho User)

## 👥 Hệ thống phân quyền

### **1. Admin (Quản trị viên)**
- **Quyền hạn**: Toàn quyền trên hệ thống
- **Chức năng**:
  - Quản lý người dùng (tạo, sửa, khóa/mở khóa tài khoản)
  - Phân quyền vai trò cho người dùng
  - Xem báo cáo hệ thống

### **2. Seller (Nhân viên nghiệp vụ)**
- **Quyền hạn**: Quản lý nghiệp vụ hằng ngày
- **Chức năng**:
  - Quản lý sản phẩm (thêm, điều chỉnh giá)
  - Tạo và quản lý phiếu nhập hàng
  - Xem báo cáo tồn kho và nhập hàng
  - Quản lý danh mục sản phẩm

### **3. User (Người dùng cơ bản)**
- **Quyền hạn**: Chỉ đọc thông tin
- **Chức năng**:
  - Tra cứu sản phẩm theo tên/SKU
  - Lọc sản phẩm theo danh mục
  - Xem thông tin tồn kho và giá bán

## 🚀 Hướng dẫn sử dụng

### **Bước 1: Đăng nhập**
1. Chạy ứng dụng
2. Nhập thông tin đăng nhập:
   - **Username**: admin (mặc định)
   - **Password**: 123456 (mặc định)
3. Hệ thống sẽ tự động xác định vai trò và mở giao diện tương ứng

### **Bước 2: Sử dụng theo vai trò**

#### **🔧 Admin**
- **Quản lý người dùng**: Chọn người dùng → "Quản lý vai trò" để phân quyền
- **Khóa/Mở khóa tài khoản**: Chọn người dùng → "Khóa/Mở khóa"
- **Làm mới dữ liệu**: Click "Làm mới" để cập nhật thông tin

#### **💼 Seller**
- **Thêm sản phẩm**: Tab "Quản lý Sản phẩm" → "Thêm sản phẩm"
- **Điều chỉnh giá**: Tab "Quản lý Sản phẩm" → "Điều chỉnh giá"
- **Tạo phiếu nhập**: Tab "Nhập hàng" → "Tạo phiếu nhập"
- **Xem báo cáo**: Tab "Báo cáo" → Click các nút tải báo cáo

#### **👤 User**
- **Tra cứu sản phẩm**: Nhập từ khóa vào ô tìm kiếm
- **Lọc theo danh mục**: Chọn danh mục từ dropdown
- **Làm mới**: Click "Làm mới" để xóa bộ lọc

## 🔧 Cấu hình kỹ thuật

### **Connection String**
```xml
<connectionStrings>
    <add name="dbms.Properties.Settings.QLNhapHangConnectionString"
        connectionString="Data Source=.;Initial Catalog=QLNhapHang;Integrated Security=True;TrustServerCertificate=True"
        providerName="System.Data.SqlClient" />
</connectionStrings>
```

### **Dependencies**
- .NET Framework 4.7.2
- System.Data.SqlClient
- System.Windows.Forms
- SQL Server (LocalDB hoặc Full SQL Server)

### **Security Features**
- Mã hóa mật khẩu bằng SHA2-256
- Phân quyền cấp database
- Validation dữ liệu đầu vào
- Transaction management
- Error handling

## 📊 Tính năng nổi bật

### **1. Quản lý sản phẩm thông minh**
- Thêm sản phẩm với nhiều danh mục
- Điều chỉnh giá hàng loạt theo danh mục
- Tự động tạo danh mục mới nếu chưa tồn tại

### **2. Nghiệp vụ nhập hàng**
- Tạo phiếu nhập với nhiều sản phẩm
- Tự động cập nhật tồn kho
- Quản lý hạn sử dụng
- Rollback khi tồn kho âm

### **3. Báo cáo đa dạng**
- Định giá tồn kho theo giá nhập cuối
- Top sản phẩm theo giá trị nhập
- Tổng hợp theo nhà cung cấp
- Lịch sử thay đổi giá

### **4. Bảo mật cao**
- Phân quyền cấp database
- Mã hóa mật khẩu
- Audit trail cho thay đổi dữ liệu
- DDL triggers bảo vệ cấu trúc

## 🛠️ Troubleshooting

### **Lỗi kết nối database**
- Kiểm tra SQL Server đang chạy
- Verify connection string trong App.config
- Đảm bảo database QLNhapHang đã được tạo

### **Lỗi đăng nhập**
- Kiểm tra username/password
- Đảm bảo tài khoản đang active
- Verify user có vai trò được gán

### **Lỗi nghiệp vụ**
- Kiểm tra dữ liệu đầu vào
- Xem log trong database
- Verify stored procedures đã được tạo

## 📝 Ghi chú phát triển

- Tất cả SQL objects được tạo từ các file trong thư mục `code_sql_server`
- Context database được lưu trong `database_context.json`
- Forms được thiết kế responsive và user-friendly
- Error handling được implement đầy đủ
- Code được comment chi tiết

## 🎉 Kết luận

Hệ thống QLNhapHang là một ứng dụng quản lý nhập hàng hoàn chỉnh với:
- ✅ Kiến trúc 3 lớp rõ ràng
- ✅ Phân quyền bảo mật cao
- ✅ Giao diện thân thiện
- ✅ Nghiệp vụ đầy đủ
- ✅ Báo cáo đa dạng
- ✅ Performance tối ưu

Sẵn sàng để deploy và sử dụng trong môi trường production!
