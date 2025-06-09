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
- ## đối với bài lab 03, để xem bài làm tại thời điểm đó, chỉ cần
- clone về như bình thường
- sau đó gõ lệnh `git checkout fb9a4de` để chuyển về commit này
- hoặc nhấn vào đây để download source code của bài lab03 : https://github.com/Quytaki/SachOnline_Lab1/archive/fb9a4de1c66a8e325b367f242a877e2801424979.zip

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

