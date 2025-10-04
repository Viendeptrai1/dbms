# 🚀 HƯỚNG DẪN SỬ DỤNG CÁC TÍNH NĂNG MỚI

## 1️⃣ THAY ĐỔI ROLE HOÀN CHỈNH (sp_ChangeUserRoleComplete)

### Cách truy cập:
1. Đăng nhập với tài khoản Admin
2. Mở **User Management** (button "Quản lý User")
3. Chọn user cần thay đổi role
4. Click button **"Đổi Role"**
5. Trong form hiện ra, chọn **1 role duy nhất**
6. Click **"Save"**

### Lưu ý quan trọng:
- ⚠️ Chỉ được chọn **1 role** (hệ thống Two-Tier Permission)
- ⚠️ **Admin không thể tự thay đổi role chính mình**
- ✅ System tự động đồng bộ cả Application Role và Database Role
- ✅ Thông báo chi tiết về các thay đổi

### Ví dụ thông báo thành công:
```
🎉 THAY ĐỔI ROLE THÀNH CÔNG!
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
✅ Đã rời role cũ [dbrole_Seller]
   👉 Quyền Seller đã bị thu hồi
✅ Đã vào role mới [dbrole_Admin]
   👉 Quyền Admin đã được cấp
✅ Application role đã cập nhật

📋 THÔNG TIN:
  👤 Username: nguyen_van_a
  🔄 Role: Seller → Admin
  🛡️ DB Role: dbrole_Seller → dbrole_Admin
```

---

## 2️⃣ TẠO SQL LOGIN CHO USER (sp_CreateSQLLoginForExistingUser)

### Mục đích:
Tạo SQL Server Login để user có thể connect trực tiếp vào database (không qua application)

### Cách truy cập:
1. Đăng nhập với tài khoản Admin
2. Mở **User Management**
3. Tab **"Quản lý Users"**
4. Chọn user chưa có SQL Login (cột **HasSQLLogin** = No/False)
5. Click button **"Tạo SQL Login"** (màu tím)
6. Nhập password (tối thiểu 6 ký tự)
7. Xác nhận password
8. Click **"Tạo Login"**

### Khi nào cần dùng:
- ✅ User cần connect trực tiếp vào SQL Server Management Studio
- ✅ Testing và troubleshooting
- ✅ User được tạo qua application nhưng chưa có SQL credentials
- ✅ Cấp quyền database cho external tools

### Thông tin quan trọng:
- 🔐 Password tối thiểu **6 ký tự**
- 🔐 Database Role được gán tự động theo Application Role hiện tại
- ⚠️ Nếu user đã có SQL Login, sẽ thông báo và không tạo mới

### Ví dụ thông báo thành công:
```
✅ SQL Login đã tạo
✅ Database User đã tạo
✅ Đã gán Database Role [dbrole_Seller]
   👉 Quyền đã được cấp tự động
```

---

## 3️⃣ BATCH IMPORT PHIẾU NHẬP (sp_BatchImportReceipts)

### Mục đích:
Import nhiều phiếu nhập hàng cùng lúc thông qua XML data

### Cách truy cập:

#### Từ AdminMainForm:
1. Đăng nhập Admin
2. Tab **"Quản lý Người dùng"**
3. Click button **"Batch Import"** (màu đỏ, phía dưới)

#### Từ SellerMainForm:
1. Đăng nhập Seller
2. Tab **"Nhập hàng"**
3. Click button **"Batch Import"** (màu đỏ)

### Hướng dẫn sử dụng:

#### Bước 1: Chuẩn bị XML Data

**Cách 1**: Chỉnh sửa template có sẵn
- Form tự động load template mẫu với UserID của bạn
- Chỉnh sửa SupplierID, ProductID, Quantity, ImportPrice

**Cách 2**: Load từ file
- Click **"📂 Tải từ File"**
- Chọn file XML đã chuẩn bị sẵn

**Cách 3**: Tạo từ Excel
```
1. Tạo data trong Excel
2. Export/convert sang XML với cấu trúc đúng
3. Load vào form
```

#### Bước 2: Validate XML
- Form tự động validate XML khi bạn click **"▶️ IMPORT"**
- Nếu XML không hợp lệ, sẽ báo lỗi ngay

#### Bước 3: Thực hiện Import
1. Click **"▶️ IMPORT"**
2. Xác nhận trong dialog
3. Chờ xử lý (tối đa 2 phút cho lượng lớn)
4. Xem kết quả trong phần **"Kết quả Import"**

### Cấu trúc XML:

```xml
<Receipts>
  <!-- Phiếu nhập #1 -->
  <Receipt>
    <ReceiptNo>1</ReceiptNo>
    <SupplierID>1</SupplierID>
    <UserID>5</UserID>
    <Notes>Nhập hàng tháng 10/2024</Notes>
    <Lines>
      <Line>
        <ProductID>101</ProductID>
        <Quantity>100</Quantity>
        <ImportPrice>50000</ImportPrice>
      </Line>
      <Line>
        <ProductID>102</ProductID>
        <Quantity>50</Quantity>
        <ImportPrice>75000</ImportPrice>
      </Line>
    </Lines>
  </Receipt>
  
  <!-- Phiếu nhập #2 -->
  <Receipt>
    <ReceiptNo>2</ReceiptNo>
    <SupplierID>2</SupplierID>
    <UserID>5</UserID>
    <Notes>Nhập hàng khẩn cấp</Notes>
    <Lines>
      <Line>
        <ProductID>201</ProductID>
        <Quantity>200</Quantity>
        <ImportPrice>30000</ImportPrice>
      </Line>
    </Lines>
  </Receipt>
</Receipts>
```

### Quy tắc quan trọng:

#### ✅ Bắt buộc:
- XML phải **well-formed** (mở/đóng tag đúng)
- **SupplierID** phải tồn tại trong bảng Suppliers
- **ProductID** phải tồn tại trong bảng Products
- **UserID** phải active (IsActive = 1)
- **Quantity** và **ImportPrice** phải > 0

#### ⚠️ Xử lý lỗi:
- Nếu 1 phiếu bị lỗi → phiếu đó **skip**, các phiếu khác vẫn được xử lý
- Log chi tiết sẽ hiển thị phiếu nào lỗi và lý do

### Các tính năng hỗ trợ:

#### 💾 Lưu Template
- Click **"💾 Lưu Template"**
- Chọn vị trí lưu file .xml
- Dùng làm template cho lần sau

#### 🗑️ Clear
- Click **"🗑️ Clear"** để xóa toàn bộ dữ liệu và bắt đầu lại

#### ❓ Hướng dẫn
- Click **"❓ Hướng dẫn"** để xem help chi tiết

### Ví dụ kết quả:

```
=== KẾT QUẢ BATCH IMPORT ===

✅ Số phiếu nhập thành công: 8
❌ Số phiếu nhập lỗi: 2

Chi tiết:
✅ Receipt #1: Đã tạo phiếu ReceiptID=1024
✅ Receipt #2: Đã tạo phiếu ReceiptID=1025
❌ Receipt #3: Supplier không tồn tại
✅ Receipt #4: Đã tạo phiếu ReceiptID=1026
...
```

---

## 🎯 TIPS & TRICKS

### Thay đổi Role:
- 💡 Luôn có 1 admin khác để backup (tránh admin tự khóa mình)
- 💡 Test với user không quan trọng trước khi apply cho production users
- 💡 Document lại các thay đổi role để audit

### Tạo SQL Login:
- 💡 Chỉ tạo khi thực sự cần (security best practice)
- 💡 Password nên mạnh hơn 6 ký tự (8+ ký tự, có số và ký tự đặc biệt)
- 💡 Thông báo user về credentials ngay sau khi tạo

### Batch Import:
- 💡 Test với XML nhỏ (2-3 phiếu) trước khi import lớn
- 💡 Backup database trước khi import số lượng lớn
- 💡 Validate SupplierID và ProductID trước trong database
- 💡 Dùng Excel + macro để generate XML nhanh từ spreadsheet
- 💡 Lưu template thành công để tái sử dụng

### Tạo XML từ Excel:

**Method 1**: Dùng công thức Excel
```excel
="<Receipt><ReceiptNo>"&A2&"</ReceiptNo><SupplierID>"&B2&"</SupplierID>..."
```

**Method 2**: VBA Macro trong Excel
```vba
' Tạo macro để export sheet thành XML theo format
```

**Method 3**: Python/PowerShell script
```python
import pandas as pd
df = pd.read_excel('receipts.xlsx')
# Convert to XML format
```

---

## 🐛 TROUBLESHOOTING

### Lỗi khi thay đổi role:
**"Bạn không thể thay đổi role chính mình"**
- ➡️ Dùng tài khoản admin khác để thực hiện

**"Role mới không hợp lệ"**
- ➡️ Check trong bảng Roles xem role có tồn tại không

### Lỗi khi tạo SQL Login:
**"SQL Login đã tồn tại"**
- ➡️ User đã có login rồi, không cần tạo mới
- ➡️ Có thể ALTER PASSWORD nếu cần

**"Không tạo được Database User"**
- ➡️ Check SQL Server permissions
- ➡️ Ensure database is online và accessible

### Lỗi Batch Import:
**"XML không hợp lệ"**
- ➡️ Check open/close tags
- ➡️ Dùng XML validator online để test
- ➡️ Ensure no special characters chưa được escape

**"Supplier không tồn tại"**
- ➡️ Query: `SELECT SupplierID FROM Suppliers` để lấy danh sách valid IDs
- ➡️ Tạo Supplier trước nếu chưa có

**"Product không tồn tại"**
- ➡️ Query: `SELECT ProductID FROM Products` để lấy danh sách valid IDs
- ➡️ Tạo Product trước nếu chưa có

**"Timeout"**
- ➡️ Giảm số lượng receipts trong 1 lần import
- ➡️ Chia nhỏ thành nhiều batches

---

## 📞 HỖ TRỢ

Nếu gặp vấn đề:
1. ✅ Đọc lại hướng dẫn này
2. ✅ Check error message chi tiết
3. ✅ Test với dữ liệu nhỏ trước
4. ✅ Verify database data (SupplierID, ProductID, UserID)
5. ✅ Check SQL Server permissions

---

## 📊 SUMMARY

| Tính năng | Form | Button | Vai trò cần thiết |
|-----------|------|--------|-------------------|
| Thay đổi Role | UserManagementForm | "Đổi Role" | Admin |
| Tạo SQL Login | UserManagementForm | "Tạo SQL Login" | Admin |
| Batch Import | BatchImportReceiptsForm | "Batch Import" | Admin hoặc Seller |

**Chúc bạn sử dụng thành công! 🎉**
