using Microsoft.EntityFrameworkCore;
using QuanLyCauLacBo.Models.Domain;

namespace QuanLyCauLacBo.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Club> Clubs { get; set; }
        public DbSet<ClubMember> ClubMembers { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventRegistration> EventRegistrations { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<UserNotification> UserNotifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(u => u.Username).IsUnique();
                entity.HasIndex(u => u.Email).IsUnique();
                entity.HasIndex(u => u.StudentId);
            });

            // ClubMember - unique constraint (user can only join a club once)
            modelBuilder.Entity<ClubMember>(entity =>
            {
                entity.HasIndex(cm => new { cm.ClubId, cm.UserId }).IsUnique();

                entity.HasOne(cm => cm.Club)
                      .WithMany(c => c.Members)
                      .HasForeignKey(cm => cm.ClubId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(cm => cm.User)
                      .WithMany(u => u.ClubMemberships)
                      .HasForeignKey(cm => cm.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Event
            modelBuilder.Entity<Event>(entity =>
            {
                entity.HasOne(e => e.Club)
                      .WithMany(c => c.Events)
                      .HasForeignKey(e => e.ClubId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.CreatedBy)
                      .WithMany()
                      .HasForeignKey(e => e.CreatedById)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // EventRegistration - unique constraint
            modelBuilder.Entity<EventRegistration>(entity =>
            {
                entity.HasIndex(er => new { er.EventId, er.UserId }).IsUnique();

                entity.HasOne(er => er.Event)
                      .WithMany(e => e.Registrations)
                      .HasForeignKey(er => er.EventId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(er => er.User)
                      .WithMany(u => u.EventRegistrations)
                      .HasForeignKey(er => er.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Activity
            modelBuilder.Entity<Activity>(entity =>
            {
                entity.HasOne(a => a.Club)
                      .WithMany(c => c.Activities)
                      .HasForeignKey(a => a.ClubId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(a => a.CreatedBy)
                      .WithMany()
                      .HasForeignKey(a => a.CreatedById)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Notification
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasOne(n => n.Club)
                      .WithMany(c => c.Notifications)
                      .HasForeignKey(n => n.ClubId)
                      .OnDelete(DeleteBehavior.SetNull)
                      .IsRequired(false);

                entity.HasOne(n => n.CreatedBy)
                      .WithMany(u => u.CreatedNotifications)
                      .HasForeignKey(n => n.CreatedById)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // UserNotification
            modelBuilder.Entity<UserNotification>(entity =>
            {
                entity.HasIndex(un => new { un.NotificationId, un.UserId }).IsUnique();

                entity.HasOne(un => un.Notification)
                      .WithMany(n => n.UserNotifications)
                      .HasForeignKey(un => un.NotificationId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(un => un.User)
                      .WithMany(u => u.UserNotifications)
                      .HasForeignKey(un => un.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
