# ✅ lab 01: 8bd3319 (seed commit)
## đối với bài lab 01, để xem bài làm tại thời điểm đó, chỉ cần
- clone về như bình thường
- sau đó gõ lệnh `git checkout 8bd3319` để chuyển về commit này
- hoặc nhấn vào đây để download source code của bài lab01 : https://github.com/Quytaki/SachOnline_Lab1/archive/8bd33191cad40c09e662f988a35ca194b7595dfe.zip


# ✅ Lab 02:
## đối với bài lab 02, để xem bài làm tại thời điểm đó, chỉ cần
- clone về như bình thường
- sau đó gõ lệnh `git checkout 7192c7c` để chuyển về commit này hoặc checkout nhánh `lab02`
- hoặc nhấn vào đây để download source code của bài lab02 : https://github.com/Quytaki/SachOnline_Lab1/archive/7192c7cd645a9578c110feacf80d6c4bba302b90.zip

- # ✅ Lab 03:
- ## Với yêu cầu đầu tiên, do nó giống với lab02 nên chỉ cần lấy lab02 về là có đủ, đã update csdl nên cần nạp csdl vào database

## 🎯 Mục tiêu

- Phát triển tiếp từ Lab 01 với cơ sở dữ liệu và layout có sẵn.
- Kết nối CSDL sử dụng mô hình Entity Framework.
- Hiển thị:
  - ✅ 6 sản phẩm **mới nhất** lên trang chủ.
  - ✅ 6 sản phẩm **bán chạy** lên trang chủ.
- Tạo menu trái hiển thị:
  - ✅ Danh sách **chủ đề**, liên kết đến sách theo chủ đề.
  - ✅ Danh mục **nhà xuất bản**, liên kết đến sách theo nhà xuất bản.
- ✅ Hiển thị chi tiết sách khi người dùng nhấp vào tên sách.

---

## ✅ Các phần đã hoàn thành

### 1. Kết nối CSDL bằng Entity Framework

- Tạo file `Model1.edmx` kết nối tới database mẫu.
- Đã import toàn bộ các bảng cần thiết (`SACH`, `CHUDE`, `NHAXUATBAN`,...).

### 2. Hiển thị 6 sách mới nhất trên `Index.cshtml`

- Viết hàm `LaySachMoi(int count)` trong `SachOnlineController`.
- Gọi hàm trong action `Index` để truyền danh sách sách mới về view.
- Hiển thị đẹp 6 sách mới theo bố cục 3 cột x 2 hàng, responsive.
- Mỗi sách có:
  - Ảnh bìa.
  - Tên sách có liên kết đến trang Chi tiết sản phẩm.
  - Nút "Add to Cart".

### 3. Hiển thị 6 sách bán chạy (`SachBanNhieuPartial`)

- Tạo Partial View `SachBanNhieuPartial`.
- Hiển thị tương tự như sách mới.
- Sắp xếp theo trường `SoLuongBan` giảm dần.

✅ **Quan trọng**: `SachBanNhieuPartial` **chỉ hiển thị trong Index**, không xuất hiện ở các view khác.

### 4. Hiển thị chủ đề sách (Partial View)

- Tạo action `ChuDePartial` trong `SachOnlineController`.
- Tạo Partial View hiển thị danh sách chủ đề (dạng danh sách `ul > li`).
- Mỗi chủ đề liên kết đến `SachTheoChuDe`.

### 5. Hiển thị nhà xuất bản (Partial View)

- Tương tự như chủ đề.
- Mỗi nhà xuất bản liên kết đến `SachTheoNXB`.

### 6. Trang Chi tiết sách

- Đã tạo action `ChiTietSach(int id)` trong `SachOnlineController`.
- Hiển thị chi tiết sách bao gồm:
  - Tên sách.
  - Ảnh bìa.
  - Mô tả, giá, nhà xuất bản, chủ đề,...
- Liên kết từ Index và các danh sách đều trỏ đến trang chi tiết chính xác.

---



## 📂 Cấu trúc thư mục chính

```plaintext
/Models
    Model1.edmx

/Controllers
    SachOnlineController.cs

/Views
    /SachOnline
        Index.cshtml
        BookDetail.cshtml
        SachTheoChuDe.cshtml
        SachTheoNXB.cshtml
        SachBanNhieuPartial.cshtml
        ChuDePartial.cshtml
        NhaXuatBanPartial.cshtml

/Images
    (chứa ảnh sách mẫu)
```

---

## ⚙️ Hướng dẫn cài đặt và kết nối trên máy khác

### Yêu cầu:

- Visual Studio (phiên bản hỗ trợ ASP.NET MVC, ví dụ VS 2017 trở lên)
- SQL Server (phiên bản phù hợp, có thể là SQL Server Express)
- File Database `.mdf` hoặc database đã được tạo sẵn (có đính kèm script db trong prj) (cùng tên và cấu trúc như Lab01)
- Project source code đầy đủ (bao gồm thư mục `Images` chứa ảnh bìa sách)

### Các bước:

1. **Mở project trong Visual Studio**

   - Mở file solution `.sln` hoặc project ASP.NET MVC trong Visual Studio.

2. **Thiết lập chuỗi kết nối trong `Web.config`**

   - Mở file `Web.config` ở thư mục gốc.
   - Tìm tới phần `<connectionStrings>` và sửa chuỗi kết nối cho phù hợp với máy bạn, ví dụ:

   ```xml
   <connectionStrings>
       <add name="SachOnlineEntities" 
            connectionString="Data Source=.\SQLEXPRESS;Initial Catalog=SachOnlineDB;Integrated Security=True" 
            providerName="System.Data.SqlClient" />
   </connectionStrings>

   - sau đó, liên kết lại entity framework với chuỗi kết nối này.
   ```

   - `Data Source` có thể là `localhost`, `.\SQLEXPRESS` hoặc tên server SQL của bạn.
   - `Initial Catalog` là tên database của bạn.
   - Nếu dùng SQL Server Authentication, cần thêm `User ID` và `Password`.

3. **Tạo hoặc import database**

   - Nếu bạn có file `.bak` hoặc `.mdf` của database:
     - Restore hoặc attach database vào SQL Server.
   - Nếu không, chạy script SQL tạo bảng và dữ liệu mẫu đã cung cấp trong Lab01.

4. **Build và chạy project**

5. **Kiểm tra hiển thị và điều hướng**

   - Kiểm tra:
     - Trang chủ hiển thị đúng sách.
     - Nhấn vào tên sách để vào trang chi tiết.
     - Menu trái có danh sách chủ đề và nhà xuất bản.
     - Các liên kết đều hoạt động đúng.

---

## ✅ Kết luận

