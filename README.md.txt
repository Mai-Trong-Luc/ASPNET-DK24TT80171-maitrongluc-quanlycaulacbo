# Hệ thống Quản lý Câu lạc bộ và Hoạt động Sinh viên

> **Môn học:** Phát triển ứng dụng Web
> **Sinh viên:** Mái Trọng Lực — MSSV: DK24TT80171
> **Trường:** Trường Kỹ thuật và Công nghệ

---

## Mục lục

1. [Tổng quan](#1-tổng-quan)
2. [Stack công nghệ](#2-stack-công-nghệ)
3. [Cấu trúc thư mục](#3-cấu-trúc-thư-mục)
4. [Cách chạy lần đầu](#4-cách-chạy-lần-đầu)
5. [Tài khoản demo](#5-tài-khoản-demo)
6. [Lộ trình test end-to-end](#6-lộ-trình-test-end-to-end)
7. [Reset / Seed lại dữ liệu](#7-reset--seed-lại-dữ-liệu)
8. [Sơ đồ dữ liệu](#8-sơ-đồ-dữ-liệu)

---

## 1. Tổng quan

Website quản lý câu lạc bộ sinh viên trong trường Kỹ thuật và Công nghệ, giúp:

- **Sinh viên** đăng ký tài khoản, tìm kiếm và tham gia câu lạc bộ, đăng ký các sự kiện.
- **Quản lý CLB** tạo & cập nhật thông tin CLB, tạo sự kiện, theo dõi tiến độ hoạt động, duyệt thành viên, gửi thông báo.
- **Admin** có toàn quyền quản lý người dùng, câu lạc bộ, sự kiện, hoạt động và thông báo toàn trường.

### Chức năng chính

| Module | Mô tả |
|--------|-------|
| **Xác thực** | Đăng ký, đăng nhập, đổi mật khẩu (Cookie Auth + BCrypt) |
| **Phân quyền** | 3 vai trò: `Admin` / `ClubManager` / `Student` |
| **Câu lạc bộ** | CRUD, tham gia/rời CLB, duyệt thành viên, quản lý vai trò |
| **Sự kiện** | CRUD, đăng ký/hủy đăng ký, giới hạn số chỗ, theo dõi người tham dự |
| **Hoạt động** | CRUD, progress bar %, cập nhật tiến độ & kết quả |
| **Thông báo** | Gửi theo CLB hoặc toàn trường, loại: Info / Event / Warning / Urgent |
| **Admin Dashboard** | Thống kê tổng quan, quản lý user (khóa/mở khóa, đổi vai trò) |

---

## 2. Stack công nghệ

| Lớp | Công nghệ |
|-----|-----------|
| **Backend** | ASP.NET Core 8 — Mô hình MVC |
| **ORM** | Entity Framework Core 8 (Code-First) |
| **Database** | SQL Server / LocalDB |
| **Auth** | Cookie Authentication (Microsoft.AspNetCore.Authentication.Cookies) |
| **Hash mật khẩu** | BCrypt.Net-Next 4 |
| **Frontend** | Bootstrap 5.3, Font Awesome 6.4 (CDN) |
| **Ngôn ngữ** | C# 12 |

---

## 3. Cấu trúc thư mục

```
ASPNET-DK24TT80171-maitrongluc-quanlycaulacbo/
├── README.md
└── QuanLyCauLacBo/                         ← Project chính
    ├── Controllers/
    │   ├── HomeController.cs               ← Trang chủ + Dashboard sinh viên
    │   ├── AccountController.cs            ← Đăng nhập, đăng ký, hồ sơ
    │   ├── ClubController.cs               ← CRUD CLB + quản lý thành viên
    │   ├── EventController.cs              ← CRUD sự kiện + đăng ký
    │   ├── ActivityController.cs           ← CRUD hoạt động + tiến độ
    │   ├── NotificationController.cs       ← Gửi & xem thông báo
    │   └── AdminController.cs              ← Dashboard + quản lý users
    │
    ├── Models/
    │   ├── Domain/                         ← Entity (bảng trong DB)
    │   │   ├── User.cs
    │   │   ├── Club.cs
    │   │   ├── ClubMember.cs
    │   │   ├── Event.cs
    │   │   ├── EventRegistration.cs
    │   │   ├── Activity.cs
    │   │   ├── Notification.cs
    │   │   └── UserNotification.cs
    │   └── ViewModels/                     ← DTO cho form / view
    │       ├── AccountViewModels.cs
    │       ├── ClubViewModels.cs
    │       ├── EventViewModels.cs
    │       ├── ActivityViewModels.cs
    │       └── DashboardViewModels.cs
    │
    ├── Data/
    │   ├── ApplicationDbContext.cs         ← EF Core DbContext + cấu hình quan hệ
    │   └── SeedData.cs                     ← Dữ liệu mock (8 users, 5 CLB, 6 sự kiện...)
    │
    ├── Services/
    │   ├── Interfaces/                     ← Contract (IUserService, IClubService...)
    │   │   ├── IUserService.cs
    │   │   ├── IClubService.cs
    │   │   ├── IEventService.cs
    │   │   ├── IActivityService.cs
    │   │   └── INotificationService.cs
    │   ├── UserService.cs
    │   ├── ClubService.cs
    │   ├── EventService.cs
    │   ├── ActivityService.cs
    │   └── NotificationService.cs
    │
    ├── Views/
    │   ├── Shared/
    │   │   └── _Layout.cshtml              ← Layout chung (navbar, footer)
    │   ├── Home/
    │   │   ├── Index.cshtml                ← Trang chủ khách
    │   │   └── StudentDashboard.cshtml     ← Dashboard sinh viên
    │   ├── Account/
    │   │   ├── Login.cshtml
    │   │   ├── Register.cshtml
    │   │   ├── Profile.cshtml
    │   │   ├── ChangePassword.cshtml
    │   │   └── AccessDenied.cshtml
    │   ├── Club/
    │   │   ├── Index.cshtml
    │   │   ├── Details.cshtml
    │   │   ├── Create.cshtml
    │   │   ├── Edit.cshtml
    │   │   └── ManageMembers.cshtml
    │   ├── Event/
    │   │   ├── Index.cshtml
    │   │   ├── Details.cshtml
    │   │   ├── Create.cshtml
    │   │   └── Edit.cshtml
    │   ├── Activity/
    │   │   ├── Index.cshtml
    │   │   ├── Details.cshtml
    │   │   ├── Create.cshtml
    │   │   └── Edit.cshtml
    │   ├── Notification/
    │   │   ├── Index.cshtml
    │   │   └── Create.cshtml
    │   └── Admin/
    │       ├── Dashboard.cshtml
    │       ├── Users.cshtml
    │       └── Notifications.cshtml
    │
    ├── wwwroot/
    │   ├── css/site.css
    │   └── js/site.js
    │
    ├── Program.cs                          ← DI, Auth, EF, SeedData
    ├── appsettings.json                    ← Connection string
    └── QuanLyCauLacBo.csproj
```

---

## 4. Cách chạy lần đầu

### 4.1 Yêu cầu môi trường

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server hoặc LocalDB (đi kèm Visual Studio)

### 4.2 Cấu hình Connection String

Mở file `QuanLyCauLacBo/appsettings.json`, sửa chuỗi kết nối nếu cần:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=QuanLyCauLacBoDB;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

> **Dùng SQL Server Express:** thay bằng
> `"Server=.\\SQLEXPRESS;Database=QuanLyCauLacBoDB;Trusted_Connection=True;"`
> **Dùng SQL Server có mật khẩu:** thay bằng
> `"Server=localhost;Database=QuanLyCauLacBoDB;User Id=sa;Password=YourPassword;TrustServerCertificate=True;"`

### 4.3 Chạy ứng dụng

```bash
cd QuanLyCauLacBo
dotnet run
```

Khi khởi động lần đầu, ứng dụng sẽ **tự động**:
1. Tạo database `QuanLyCauLacBoDB`
2. Tạo toàn bộ bảng (EnsureCreated)
3. Chèn dữ liệu mock (SeedData)

Truy cập: `https://localhost:5001` hoặc `http://localhost:5000`

### 4.4 Chạy từ Visual Studio

1. Mở file `QuanLyCauLacBo.csproj` hoặc mở thư mục gốc bằng Visual Studio
2. Nhấn **F5** hoặc chọn **Debug → Start Debugging**

---

## 5. Tài khoản demo

| Vai trò | Tên đăng nhập | Mật khẩu | Họ tên |
|---------|--------------|----------|--------|
| **Admin** | `admin` | `Admin@123` | Quản trị viên hệ thống |
| **Quản lý CLB** | `le.van.c` | `Student@123` | Lê Văn C — Chủ nhiệm CLB CNTT |
| **Quản lý CLB** | `pham.thi.d` | `Student@123` | Phạm Thị D — Chủ nhiệm CLB Robot |
| **Sinh viên** | `nguyen.van.a` | `Student@123` | Nguyễn Văn A — DK24TT001 |
| **Sinh viên** | `tran.thi.b` | `Student@123` | Trần Thị B — DK24TT002 |
| **Sinh viên** | `hoang.van.e` | `Student@123` | Hoàng Văn E — DK24TT005 |

---

## 6. Lộ trình test end-to-end

### Luồng 1 — Quản trị viên

```
Đăng nhập (admin / Admin@123)
  → Admin Dashboard          (xem thống kê tổng quan)
  → Quản lý → Tạo CLB mới   (thêm CLB mới nếu muốn)
  → Admin → Quản lý Users    (đổi vai trò / khóa tài khoản)
  → Gửi thông báo toàn trường
```

### Luồng 2 — Sinh viên tham gia CLB

```
Đăng ký tài khoản mới  (/Account/Register)
  → Đăng nhập
  → Câu lạc bộ          (xem danh sách)
  → Chọn một CLB → Xem chi tiết
  → Nhấn "Tham gia CLB" → trạng thái "Chờ duyệt"
  → Sự kiện             (xem danh sách → Đăng ký tham gia sự kiện)
  → Thông báo           (xem thông báo của CLB)
```

### Luồng 3 — Quản lý CLB duyệt thành viên

```
Đăng nhập (le.van.c / Student@123)
  → Câu lạc bộ → CLB Công nghệ thông tin → Xem chi tiết
  → Nhấn "Quản lý thành viên"
  → Duyệt / Từ chối sinh viên vừa đăng ký
  → Tạo sự kiện mới      (/Event/Create)
  → Tạo hoạt động mới    (/Activity/Create)
  → Cập nhật tiến độ hoạt động (trang Details → form cập nhật %)
  → Gửi thông báo cho thành viên CLB
```

### Luồng 4 — Kiểm tra phân quyền

```
Đăng nhập bằng tài khoản Student
  → Thử truy cập /Admin/Dashboard     → Redirect → AccessDenied
  → Thử truy cập /Club/Create         → Redirect → Login (403)
  → Đúng: chỉ xem được CLB / Sự kiện / Hoạt động
```

---

## 7. Reset / Seed lại dữ liệu

Khi cần **xóa toàn bộ dữ liệu và tạo lại từ đầu**:

**Cách 1 — Xóa database qua SQL Server Management Studio (SSMS):**
```sql
DROP DATABASE QuanLyCauLacBoDB;
```
Sau đó chạy lại `dotnet run` — database và seed data sẽ tự tạo lại.

**Cách 2 — Dùng Package Manager Console (Visual Studio):**
```powershell
Drop-Database
Update-Database
```

**Cách 3 — Xóa file LocalDB (nếu dùng LocalDB):**
```powershell
# Mở PowerShell, chạy lệnh
sqllocaldb stop MSSQLLocalDB
sqllocaldb delete MSSQLLocalDB
sqllocaldb create MSSQLLocalDB
```
Sau đó chạy lại ứng dụng.

> **Lưu ý:** `SeedData.cs` có kiểm tra `if (context.Users.Any()) return;`
> — nghĩa là seed chỉ chạy một lần duy nhất khi database rỗng.

---

## 8. Sơ đồ dữ liệu

```
┌──────────────┐        ┌──────────────────┐        ┌──────────────┐
│    Users     │        │   ClubMembers    │        │    Clubs     │
│──────────────│        │──────────────────│        │──────────────│
│ Id (PK)      │──1──<──│ UserId (FK)      │──>──1──│ Id (PK)      │
│ Username     │        │ ClubId (FK)      │        │ Name         │
│ PasswordHash │        │ Role             │        │ Description  │
│ Email        │        │ Status           │        │ Faculty      │
│ FullName     │        │   Pending        │        │ FoundedDate  │
│ StudentId    │        │   Active         │        │ Status       │
│ Role         │        │   Rejected       │        │ ContactEmail │
│   Admin      │        │ JoinedDate       │        │ CreatedAt    │
│   ClubManager│        └──────────────────┘        └──────┬───────┘
│   Student    │                                           │
│ IsActive     │        ┌──────────────────┐               │
│ CreatedAt    │        │     Events       │───────────────┘
└──────┬───────┘        │──────────────────│        1:N Club có nhiều Event
       │                │ Id (PK)          │
       │                │ ClubId (FK)      │        ┌──────────────────┐
       │                │ Title            │        │ EventRegistration │
       │                │ StartDate        │──1──<──│──────────────────│
       │                │ EndDate          │        │ EventId (FK)     │
       │                │ Location         │        │ UserId (FK)──────┼──> Users
       │                │ MaxParticipants  │        │ Status           │
       │                │ Status           │        │   Registered     │
       │                │ CreatedById (FK)─┼──>─────│   Attended       │
       │                └──────────────────┘  Users │   Cancelled      │
       │                                            └──────────────────┘
       │                ┌──────────────────┐
       │                │   Activities     │        ┌──────────────────┐
       │                │──────────────────│        │  Notifications   │
       │                │ Id (PK)          │        │──────────────────│
       │                │ ClubId (FK)──────┼──>Club │ Id (PK)          │
       │                │ Title            │        │ ClubId (FK, null)│
       │                │ Status           │        │ Title            │
       │                │   Planning       │        │ Content          │
       │                │   InProgress     │        │ Type             │
       │                │   Completed      │        │ IsGlobal         │
       │                │ Progress (0-100) │        │ CreatedById (FK) │
       │                │ Result           │        │ ExpiresAt        │
       │                │ CreatedById (FK)─┼──>─────└──────┬───────────┘
       │                └──────────────────┘  Users        │
       │                                                    │
       │                ┌──────────────────────────────┐   │
       └────────────────│      UserNotifications       │<──┘
                        │──────────────────────────────│
                        │ NotificationId (FK)           │
                        │ UserId (FK)                   │
                        │ IsRead                        │
                        │ ReadAt                        │
                        └──────────────────────────────┘
```

### Quan hệ tóm tắt

| Quan hệ | Kiểu |
|---------|------|
| User ↔ Club | N:N qua `ClubMembers` |
| User ↔ Event | N:N qua `EventRegistrations` |
| Club → Events | 1:N |
| Club → Activities | 1:N |
| Club → Notifications | 1:N (nullable, null = global) |
| Notification ↔ User | N:N qua `UserNotifications` |

---

*Dự án tốt nghiệp — Trường Kỹ thuật và Công nghệ — 2024*
