using QuanLyCauLacBo.Models.Domain;

namespace QuanLyCauLacBo.Data
{
    public static class SeedData
    {
        public static void Initialize(ApplicationDbContext context)
        {
            if (context.Users.Any()) return; // DB đã có dữ liệu

            // === USERS ===
            var users = new List<User>
            {
                new User
                {
                    Username = "admin",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                    Email = "admin@ktcn.edu.vn",
                    FullName = "Quản trị viên hệ thống",
                    Role = "Admin",
                    IsActive = true,
                    DateOfBirth = new DateTime(1985, 1, 1),
                    CreatedAt = DateTime.Now.AddMonths(-12)
                },
                new User
                {
                    Username = "nguyen.van.a",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Student@123"),
                    Email = "nguyen.van.a@sv.ktcn.edu.vn",
                    FullName = "Nguyễn Văn A",
                    StudentId = "DK24TT001",
                    PhoneNumber = "0901234501",
                    Role = "Student",
                    IsActive = true,
                    DateOfBirth = new DateTime(2003, 5, 15),
                    CreatedAt = DateTime.Now.AddMonths(-8)
                },
                new User
                {
                    Username = "tran.thi.b",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Student@123"),
                    Email = "tran.thi.b@sv.ktcn.edu.vn",
                    FullName = "Trần Thị B",
                    StudentId = "DK24TT002",
                    PhoneNumber = "0901234502",
                    Role = "Student",
                    IsActive = true,
                    DateOfBirth = new DateTime(2003, 8, 20),
                    CreatedAt = DateTime.Now.AddMonths(-8)
                },
                new User
                {
                    Username = "le.van.c",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Student@123"),
                    Email = "le.van.c@sv.ktcn.edu.vn",
                    FullName = "Lê Văn C",
                    StudentId = "DK24TT003",
                    PhoneNumber = "0901234503",
                    Role = "ClubManager",
                    IsActive = true,
                    DateOfBirth = new DateTime(2002, 3, 10),
                    CreatedAt = DateTime.Now.AddMonths(-10)
                },
                new User
                {
                    Username = "pham.thi.d",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Student@123"),
                    Email = "pham.thi.d@sv.ktcn.edu.vn",
                    FullName = "Phạm Thị D",
                    StudentId = "DK24TT004",
                    PhoneNumber = "0901234504",
                    Role = "ClubManager",
                    IsActive = true,
                    DateOfBirth = new DateTime(2002, 11, 25),
                    CreatedAt = DateTime.Now.AddMonths(-9)
                },
                new User
                {
                    Username = "hoang.van.e",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Student@123"),
                    Email = "hoang.van.e@sv.ktcn.edu.vn",
                    FullName = "Hoàng Văn E",
                    StudentId = "DK24TT005",
                    PhoneNumber = "0901234505",
                    Role = "Student",
                    IsActive = true,
                    DateOfBirth = new DateTime(2003, 7, 3),
                    CreatedAt = DateTime.Now.AddMonths(-6)
                },
                new User
                {
                    Username = "vo.thi.f",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Student@123"),
                    Email = "vo.thi.f@sv.ktcn.edu.vn",
                    FullName = "Võ Thị F",
                    StudentId = "DK24TT006",
                    PhoneNumber = "0901234506",
                    Role = "Student",
                    IsActive = true,
                    DateOfBirth = new DateTime(2004, 2, 14),
                    CreatedAt = DateTime.Now.AddMonths(-5)
                },
                new User
                {
                    Username = "dang.van.g",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Student@123"),
                    Email = "dang.van.g@sv.ktcn.edu.vn",
                    FullName = "Đặng Văn G",
                    StudentId = "DK24TT007",
                    PhoneNumber = "0901234507",
                    Role = "Student",
                    IsActive = true,
                    DateOfBirth = new DateTime(2003, 9, 30),
                    CreatedAt = DateTime.Now.AddMonths(-4)
                }
            };

            context.Users.AddRange(users);
            context.SaveChanges();

            // === CLUBS ===
            var clubs = new List<Club>
            {
                new Club
                {
                    Name = "CLB Công nghệ thông tin",
                    Description = "Câu lạc bộ dành cho sinh viên yêu thích lập trình, công nghệ và khám phá thế giới số. Tổ chức các buổi workshop, hackathon và chia sẻ kiến thức công nghệ.",
                    FoundedDate = new DateTime(2020, 9, 1),
                    ContactEmail = "clb.cntt@ktcn.edu.vn",
                    ContactPhone = "0901100001",
                    Faculty = "Khoa Công nghệ thông tin",
                    Status = "Active",
                    CreatedAt = DateTime.Now.AddMonths(-24)
                },
                new Club
                {
                    Name = "CLB Kỹ thuật Robot",
                    Description = "Câu lạc bộ nghiên cứu và chế tạo robot, tự động hóa. Tham gia các cuộc thi robot trong và ngoài trường.",
                    FoundedDate = new DateTime(2019, 3, 15),
                    ContactEmail = "clb.robot@ktcn.edu.vn",
                    ContactPhone = "0901100002",
                    Faculty = "Khoa Cơ điện tử",
                    Status = "Active",
                    CreatedAt = DateTime.Now.AddMonths(-30)
                },
                new Club
                {
                    Name = "CLB Khởi nghiệp và Đổi mới sáng tạo",
                    Description = "Nơi ươm mầm ý tưởng khởi nghiệp, kết nối sinh viên với doanh nghiệp và nhà đầu tư. Tổ chức pitch competition, mentoring session.",
                    FoundedDate = new DateTime(2021, 5, 10),
                    ContactEmail = "clb.startup@ktcn.edu.vn",
                    ContactPhone = "0901100003",
                    Faculty = "Phòng Công tác sinh viên",
                    Status = "Active",
                    CreatedAt = DateTime.Now.AddMonths(-18)
                },
                new Club
                {
                    Name = "CLB Tiếng Anh Kỹ thuật",
                    Description = "Rèn luyện kỹ năng tiếng Anh chuyên ngành kỹ thuật, luyện thi IELTS, TOEIC và giao lưu quốc tế.",
                    FoundedDate = new DateTime(2018, 9, 1),
                    ContactEmail = "clb.english@ktcn.edu.vn",
                    ContactPhone = "0901100004",
                    Faculty = "Khoa Ngoại ngữ",
                    Status = "Active",
                    CreatedAt = DateTime.Now.AddMonths(-36)
                },
                new Club
                {
                    Name = "CLB Thiết kế Đồ họa",
                    Description = "Sáng tạo và học hỏi về thiết kế đồ họa, UI/UX, multimedia. Workshop Photoshop, Illustrator, Figma.",
                    FoundedDate = new DateTime(2022, 1, 20),
                    ContactEmail = "clb.design@ktcn.edu.vn",
                    ContactPhone = "0901100005",
                    Faculty = "Khoa Truyền thông",
                    Status = "Active",
                    CreatedAt = DateTime.Now.AddMonths(-12)
                }
            };

            context.Clubs.AddRange(clubs);
            context.SaveChanges();

            // === CLUB MEMBERS ===
            var members = new List<ClubMember>
            {
                // CLB CNTT
                new ClubMember { ClubId = clubs[0].Id, UserId = users[3].Id, Role = "President", Status = "Active", JoinedDate = DateTime.Now.AddMonths(-18) },
                new ClubMember { ClubId = clubs[0].Id, UserId = users[1].Id, Role = "Vice President", Status = "Active", JoinedDate = DateTime.Now.AddMonths(-8) },
                new ClubMember { ClubId = clubs[0].Id, UserId = users[2].Id, Role = "Secretary", Status = "Active", JoinedDate = DateTime.Now.AddMonths(-8) },
                new ClubMember { ClubId = clubs[0].Id, UserId = users[5].Id, Role = "Member", Status = "Active", JoinedDate = DateTime.Now.AddMonths(-4) },
                new ClubMember { ClubId = clubs[0].Id, UserId = users[6].Id, Role = "Member", Status = "Pending", JoinedDate = DateTime.Now.AddDays(-3) },

                // CLB Robot
                new ClubMember { ClubId = clubs[1].Id, UserId = users[4].Id, Role = "President", Status = "Active", JoinedDate = DateTime.Now.AddMonths(-20) },
                new ClubMember { ClubId = clubs[1].Id, UserId = users[6].Id, Role = "Member", Status = "Active", JoinedDate = DateTime.Now.AddMonths(-3) },
                new ClubMember { ClubId = clubs[1].Id, UserId = users[7].Id, Role = "Member", Status = "Pending", JoinedDate = DateTime.Now.AddDays(-1) },

                // CLB Startup
                new ClubMember { ClubId = clubs[2].Id, UserId = users[1].Id, Role = "Member", Status = "Active", JoinedDate = DateTime.Now.AddMonths(-5) },
                new ClubMember { ClubId = clubs[2].Id, UserId = users[2].Id, Role = "Member", Status = "Active", JoinedDate = DateTime.Now.AddMonths(-4) },

                // CLB English
                new ClubMember { ClubId = clubs[3].Id, UserId = users[5].Id, Role = "Member", Status = "Active", JoinedDate = DateTime.Now.AddMonths(-6) },
            };

            context.ClubMembers.AddRange(members);
            context.SaveChanges();

            // === EVENTS ===
            var events = new List<Event>
            {
                new Event
                {
                    ClubId = clubs[0].Id,
                    Title = "Hackathon Công nghệ 2024 - Giải pháp cho thành phố thông minh",
                    Description = "Cuộc thi lập trình 48 giờ liên tục với chủ đề xây dựng giải pháp công nghệ cho thành phố thông minh. Giải thưởng lên đến 20 triệu đồng.",
                    StartDate = DateTime.Now.AddDays(15),
                    EndDate = DateTime.Now.AddDays(17),
                    Location = "Hội trường A - Cơ sở 1",
                    MaxParticipants = 100,
                    Status = "Upcoming",
                    CreatedById = users[3].Id,
                    CreatedAt = DateTime.Now.AddDays(-5)
                },
                new Event
                {
                    ClubId = clubs[0].Id,
                    Title = "Workshop: Lập trình Web với React & Node.js",
                    Description = "Buổi workshop thực hành về phát triển ứng dụng web full-stack sử dụng React.js và Node.js. Dành cho sinh viên đã có kiến thức cơ bản về JavaScript.",
                    StartDate = DateTime.Now.AddDays(5),
                    EndDate = DateTime.Now.AddDays(5).AddHours(4),
                    Location = "Phòng máy tính B302",
                    MaxParticipants = 40,
                    Status = "Upcoming",
                    CreatedById = users[3].Id,
                    CreatedAt = DateTime.Now.AddDays(-3)
                },
                new Event
                {
                    ClubId = clubs[1].Id,
                    Title = "Cuộc thi Robot Line Following 2024",
                    Description = "Thi đấu robot dò đường với các vòng loại và chung kết. Mỗi đội tối đa 3 thành viên. Đăng ký trước ngày thi 1 tuần.",
                    StartDate = DateTime.Now.AddDays(20),
                    EndDate = DateTime.Now.AddDays(21),
                    Location = "Xưởng thực hành C2",
                    MaxParticipants = 60,
                    Status = "Upcoming",
                    CreatedById = users[4].Id,
                    CreatedAt = DateTime.Now.AddDays(-7)
                },
                new Event
                {
                    ClubId = clubs[2].Id,
                    Title = "Startup Pitch Competition - Mùa 3",
                    Description = "Cuộc thi thuyết trình ý tưởng khởi nghiệp với ban giám khảo là các doanh nhân và nhà đầu tư. Giải nhất nhận được gói hỗ trợ 50 triệu đồng.",
                    StartDate = DateTime.Now.AddDays(30),
                    EndDate = DateTime.Now.AddDays(30).AddHours(6),
                    Location = "Hội trường Trung tâm",
                    MaxParticipants = 80,
                    Status = "Upcoming",
                    CreatedById = users[3].Id,
                    CreatedAt = DateTime.Now.AddDays(-2)
                },
                new Event
                {
                    ClubId = clubs[3].Id,
                    Title = "IELTS Speaking Club - Buổi 15",
                    Description = "Luyện tập kỹ năng nói tiếng Anh theo định dạng IELTS Speaking. Có native speaker hỗ trợ.",
                    StartDate = DateTime.Now.AddDays(3),
                    EndDate = DateTime.Now.AddDays(3).AddHours(2),
                    Location = "Phòng D105",
                    MaxParticipants = 30,
                    Status = "Upcoming",
                    CreatedById = users[4].Id,
                    CreatedAt = DateTime.Now.AddDays(-1)
                },
                new Event
                {
                    ClubId = clubs[0].Id,
                    Title = "Seminar AI & Machine Learning trong Kỹ thuật",
                    Description = "Buổi seminar về ứng dụng AI và Machine Learning trong lĩnh vực kỹ thuật. Diễn giả là kỹ sư senior từ các công ty công nghệ hàng đầu.",
                    StartDate = DateTime.Now.AddDays(-10),
                    EndDate = DateTime.Now.AddDays(-10).AddHours(3),
                    Location = "Hội trường B",
                    MaxParticipants = 150,
                    Status = "Completed",
                    CreatedById = users[3].Id,
                    CreatedAt = DateTime.Now.AddDays(-20)
                }
            };

            context.Events.AddRange(events);
            context.SaveChanges();

            // === EVENT REGISTRATIONS ===
            var registrations = new List<EventRegistration>
            {
                new EventRegistration { EventId = events[0].Id, UserId = users[1].Id, Status = "Registered", RegisteredAt = DateTime.Now.AddDays(-3) },
                new EventRegistration { EventId = events[0].Id, UserId = users[2].Id, Status = "Registered", RegisteredAt = DateTime.Now.AddDays(-2) },
                new EventRegistration { EventId = events[0].Id, UserId = users[5].Id, Status = "Registered", RegisteredAt = DateTime.Now.AddDays(-1) },
                new EventRegistration { EventId = events[1].Id, UserId = users[1].Id, Status = "Registered", RegisteredAt = DateTime.Now.AddDays(-2) },
                new EventRegistration { EventId = events[1].Id, UserId = users[6].Id, Status = "Registered", RegisteredAt = DateTime.Now.AddDays(-1) },
                new EventRegistration { EventId = events[4].Id, UserId = users[5].Id, Status = "Registered", RegisteredAt = DateTime.Now.AddDays(-1) },
                new EventRegistration { EventId = events[5].Id, UserId = users[1].Id, Status = "Attended", RegisteredAt = DateTime.Now.AddDays(-15) },
                new EventRegistration { EventId = events[5].Id, UserId = users[2].Id, Status = "Attended", RegisteredAt = DateTime.Now.AddDays(-15) },
                new EventRegistration { EventId = events[5].Id, UserId = users[5].Id, Status = "Absent", RegisteredAt = DateTime.Now.AddDays(-14) },
            };

            context.EventRegistrations.AddRange(registrations);
            context.SaveChanges();

            // === ACTIVITIES ===
            var activities = new List<Activity>
            {
                new Activity
                {
                    ClubId = clubs[0].Id,
                    Title = "Xây dựng Website nội bộ CLB",
                    Description = "Phát triển website quản lý nội bộ và giới thiệu câu lạc bộ sử dụng React và ASP.NET Core.",
                    StartDate = DateTime.Now.AddMonths(-2),
                    EndDate = DateTime.Now.AddMonths(1),
                    Location = "Phòng Lab CNTT",
                    Status = "InProgress",
                    Progress = 65,
                    CreatedById = users[3].Id,
                    CreatedAt = DateTime.Now.AddMonths(-2)
                },
                new Activity
                {
                    ClubId = clubs[0].Id,
                    Title = "Khóa học lập trình Python cho người mới bắt đầu",
                    Description = "Tổ chức khóa học Python miễn phí 10 buổi cho sinh viên chưa có kiến thức lập trình.",
                    StartDate = DateTime.Now.AddMonths(-3),
                    EndDate = DateTime.Now.AddMonths(-1),
                    Status = "Completed",
                    Progress = 100,
                    Result = "Đã hoàn thành 10 buổi học với 35 học viên tham gia. 28 học viên hoàn thành khóa học và nhận chứng chỉ.",
                    CreatedById = users[3].Id,
                    CreatedAt = DateTime.Now.AddMonths(-4)
                },
                new Activity
                {
                    ClubId = clubs[1].Id,
                    Title = "Thiết kế và chế tạo Robot phục vụ tại canteen",
                    Description = "Nghiên cứu và chế tạo robot tự động phục vụ tại canteen trường theo đơn đặt hàng của Ban Giám hiệu.",
                    StartDate = DateTime.Now.AddMonths(-1),
                    EndDate = DateTime.Now.AddMonths(3),
                    Location = "Xưởng thực hành C2",
                    Status = "InProgress",
                    Progress = 30,
                    CreatedById = users[4].Id,
                    CreatedAt = DateTime.Now.AddMonths(-1)
                },
                new Activity
                {
                    ClubId = clubs[2].Id,
                    Title = "Kết nối mentor cho sinh viên khởi nghiệp",
                    Description = "Xây dựng mạng lưới mentor gồm các doanh nhân, chuyên gia tư vấn cho sinh viên có ý tưởng khởi nghiệp.",
                    StartDate = DateTime.Now.AddDays(-15),
                    EndDate = DateTime.Now.AddMonths(2),
                    Status = "InProgress",
                    Progress = 20,
                    CreatedById = users[3].Id,
                    CreatedAt = DateTime.Now.AddDays(-15)
                },
                new Activity
                {
                    ClubId = clubs[3].Id,
                    Title = "Biên soạn tài liệu IELTS kỹ thuật",
                    Description = "Biên soạn bộ tài liệu từ vựng và bài tập IELTS chuyên ngành kỹ thuật dành riêng cho sinh viên trường.",
                    StartDate = DateTime.Now.AddDays(5),
                    Status = "Planning",
                    Progress = 0,
                    CreatedById = users[4].Id,
                    CreatedAt = DateTime.Now.AddDays(-2)
                }
            };

            context.Activities.AddRange(activities);
            context.SaveChanges();

            // === NOTIFICATIONS ===
            var notifications = new List<Notification>
            {
                new Notification
                {
                    Title = "Khai mạc năm học hoạt động CLB 2024-2025",
                    Content = "Phòng Công tác sinh viên thông báo: Tất cả các câu lạc bộ cần nộp kế hoạch hoạt động năm học 2024-2025 trước ngày 15/10/2024. Liên hệ phòng CTSV để được hỗ trợ.",
                    IsGlobal = true,
                    Type = "Info",
                    CreatedById = users[0].Id,
                    CreatedAt = DateTime.Now.AddDays(-30),
                    ExpiresAt = DateTime.Now.AddMonths(2)
                },
                new Notification
                {
                    Title = "Thông báo: Hackathon CNTT 2024 mở đăng ký",
                    Content = "CLB Công nghệ thông tin trân trọng thông báo: Cuộc thi Hackathon CNTT 2024 đã chính thức mở đăng ký. Sinh viên toàn trường có thể đăng ký theo nhóm 2-5 người. Hạn đăng ký: 5 ngày trước khi diễn ra sự kiện.",
                    ClubId = clubs[0].Id,
                    IsGlobal = false,
                    Type = "Event",
                    CreatedById = users[3].Id,
                    CreatedAt = DateTime.Now.AddDays(-5)
                },
                new Notification
                {
                    Title = "Cập nhật lịch sinh hoạt CLB Robot tháng 11",
                    Content = "CLB Kỹ thuật Robot thông báo lịch sinh hoạt tháng 11: Mỗi thứ 4 hàng tuần từ 18:00-20:00 tại xưởng thực hành C2. Thành viên vắng mặt quá 3 buổi liên tiếp sẽ bị xem xét tư cách thành viên.",
                    ClubId = clubs[1].Id,
                    IsGlobal = false,
                    Type = "Info",
                    CreatedById = users[4].Id,
                    CreatedAt = DateTime.Now.AddDays(-7)
                },
                new Notification
                {
                    Title = "KHẨN: Nộp báo cáo hoạt động học kỳ 1",
                    Content = "Tất cả các câu lạc bộ CẦN NỘP báo cáo hoạt động học kỳ 1 năm 2024-2025 trước 17:00 ngày 31/12/2024. Báo cáo gồm: danh sách thành viên, kết quả hoạt động, kế hoạch học kỳ 2. Nộp về phòng CTSV phòng A101.",
                    IsGlobal = true,
                    Type = "Urgent",
                    CreatedById = users[0].Id,
                    CreatedAt = DateTime.Now.AddDays(-2)
                },
                new Notification
                {
                    Title = "Workshop React & Node.js - Còn 5 chỗ trống",
                    Content = "Workshop Lập trình Web với React & Node.js còn rất ít chỗ. Đăng ký ngay tại website để không bỏ lỡ cơ hội học hỏi từ các chuyên gia đầu ngành!",
                    ClubId = clubs[0].Id,
                    IsGlobal = false,
                    Type = "Warning",
                    CreatedById = users[3].Id,
                    CreatedAt = DateTime.Now.AddDays(-1)
                }
            };

            context.Notifications.AddRange(notifications);
            context.SaveChanges();

            // === USER NOTIFICATIONS ===
            // Gán thông báo toàn cục cho tất cả users
            var userNotifications = new List<UserNotification>();
            foreach (var user in users)
            {
                foreach (var notif in notifications.Where(n => n.IsGlobal))
                {
                    userNotifications.Add(new UserNotification
                    {
                        NotificationId = notif.Id,
                        UserId = user.Id,
                        IsRead = false
                    });
                }
            }
            // Thông báo CLB CNTT cho thành viên CLB CNTT
            var cnttMembers = members.Where(m => m.ClubId == clubs[0].Id).ToList();
            foreach (var m in cnttMembers)
            {
                foreach (var notif in notifications.Where(n => n.ClubId == clubs[0].Id))
                {
                    if (!userNotifications.Any(un => un.NotificationId == notif.Id && un.UserId == m.UserId))
                    {
                        userNotifications.Add(new UserNotification
                        {
                            NotificationId = notif.Id,
                            UserId = m.UserId,
                            IsRead = false
                        });
                    }
                }
            }

            context.UserNotifications.AddRange(userNotifications);
            context.SaveChanges();
        }
    }
}
