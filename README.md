# QUẢN LÝ CÂU LẠC BỘ SINH VIÊN

## Giới thiệu

Đây là dự án xây dựng hệ thống quản lý câu lạc bộ sinh viên sử dụng ASP.NET MVC. Hệ thống hỗ trợ quản lý thông tin câu lạc bộ, thành viên, sự kiện và các hoạt động liên quan nhằm giúp việc quản lý trở nên dễ dàng và hiệu quả hơn.

## Chức năng chính

### Quản lý thành viên

* Thêm mới thành viên.
* Cập nhật thông tin thành viên.
* Xóa thành viên.
* Tìm kiếm thành viên.

### Quản lý câu lạc bộ

* Thêm câu lạc bộ mới.
* Chỉnh sửa thông tin câu lạc bộ.
* Xóa câu lạc bộ.
* Xem danh sách câu lạc bộ.

### Quản lý sự kiện

* Tạo sự kiện mới.
* Cập nhật thông tin sự kiện.
* Xóa sự kiện.
* Theo dõi số lượng người tham gia.

### Quản lý tài khoản

* Đăng nhập hệ thống.
* Phân quyền người dùng.
* Đổi mật khẩu.

## Công nghệ sử dụng

* ASP.NET MVC
* C#
* SQL Server
* Entity Framework
* HTML, CSS, JavaScript
* Bootstrap

## Yêu cầu hệ thống

* Visual Studio 2022 trở lên
* .NET Framework hoặc .NET 8
* SQL Server 2019 trở lên

## Cài đặt

1. Clone dự án:

```bash
git clone https://github.com/Mai-Trong-Luc/ASPNET-DK24TT80171-maitrongluc-quanlycaulacbo.git
```

2. Mở file `.sln` bằng Visual Studio.

3. Khôi phục các gói NuGet:

```bash
Update-Package -Reinstall
```

4. Cấu hình chuỗi kết nối trong file `appsettings.json` hoặc `Web.config`.

5. Chạy Migration và cập nhật cơ sở dữ liệu.

6. Nhấn F5 để chạy chương trình.

## Cấu trúc dự án

```text
Controllers/
Models/
Views/
Data/
wwwroot/
Migrations/
```

## Tác giả

* Họ và tên: Mai Trọng Lực
* Lớp: DK24TT80171
* Ngành: Công nghệ Thông tin

## Hướng phát triển

* Tích hợp gửi email thông báo.
* Quản lý điểm danh thành viên.
* Thống kê và báo cáo trực quan.
* Xây dựng API cho ứng dụng di động.

## Giấy phép

Dự án được phát triển phục vụ mục đích học tập và nghiên cứu.
