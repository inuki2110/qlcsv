using Microsoft.EntityFrameworkCore;
using QLCSV.Models;

namespace QLCSV.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        // ===================== DbSet =====================
        public DbSet<User> Users { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Major> Majors { get; set; }
        public DbSet<AlumniProfile> AlumniProfiles { get; set; }
        public DbSet<Batch> Batches { get; set; }
        public DbSet<AlumniBatch> AlumniBatches { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventRegistration> EventRegistrations { get; set; }

        // ===================== SaveChanges =====================
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries()
                .Where(e =>
                    e.State == EntityState.Added ||
                    e.State == EntityState.Modified
                );

            foreach (var entry in entries)
            {
                if (entry.Entity is User user)
                {
                    user.UpdatedAt = DateTime.UtcNow;
                    if (entry.State == EntityState.Added)
                        user.CreatedAt = DateTime.UtcNow;
                }

                if (entry.Entity is AlumniProfile profile)
                {
                    profile.UpdatedAt = DateTime.UtcNow;
                    if (entry.State == EntityState.Added)
                        profile.CreatedAt = DateTime.UtcNow;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        // ===================== Model Config =====================
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ===== USER =====
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.HasKey(u => u.Id);
                entity.Property(u => u.Username).IsRequired();
                entity.Property(u => u.Email).HasMaxLength(255).IsRequired();
                entity.Property(u => u.PasswordHash).HasMaxLength(255).IsRequired();
                entity.Property(u => u.FullName).HasMaxLength(255).IsRequired();

                entity.HasIndex(u => u.Email).IsUnique();
                entity.HasIndex(u => u.EmailVerificationToken);

                entity.Property(u => u.Role)
                      .HasMaxLength(20)
                      .HasDefaultValue("pending");

                entity.Property(u => u.IsActive).HasDefaultValue(true);
                base.OnModelCreating(modelBuilder);

                modelBuilder.Entity<AlumniBatch>()
                    .HasKey(ab => new { ab.AlumniId, ab.BatchId });

                modelBuilder.Entity<AlumniBatch>()
                    .HasOne(ab => ab.Alumni)
                    .WithMany(a => a.AlumniBatches)
                    .HasForeignKey(ab => ab.AlumniId);

                modelBuilder.Entity<AlumniBatch>()
                    .HasOne(ab => ab.Batch)
                    .WithMany(b => b.AlumniBatches)
                    .HasForeignKey(ab => ab.BatchId);
            });

            // ===== FACULTY =====
            modelBuilder.Entity<Faculty>(entity =>
            {
                entity.ToTable("faculties");

                entity.HasIndex(f => f.Name).IsUnique();
                entity.Property(f => f.Name).HasMaxLength(255).IsRequired();
                entity.Property(f => f.ShortName).HasMaxLength(50);
            });

            // ===== MAJOR =====
            modelBuilder.Entity<Major>(entity =>
            {
                entity.ToTable("majors");

                entity.HasOne(m => m.Faculty)
                      .WithMany(f => f.Majors)
                      .HasForeignKey(m => m.FacultyId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(m => new { m.FacultyId, m.Code })
                      .IsUnique()
                      .HasFilter("\"Code\" IS NOT NULL");
            });

            // ===== ALUMNI PROFILE =====
            modelBuilder.Entity<AlumniProfile>(entity =>
            {
                entity.ToTable("alumni_profiles");

                entity.HasIndex(a => a.StudentId).IsUnique();
                entity.Property(a => a.StudentId).HasMaxLength(20);

                entity.Property(a => a.Country)
                      .HasMaxLength(100)
                      .HasDefaultValue("Việt Nam");

                entity.HasOne(a => a.User)
                      .WithOne(u => u.AlumniProfile)
                      .HasForeignKey<AlumniProfile>(a => a.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(a => a.Faculty)
                      .WithMany(f => f.AlumniProfiles)
                      .HasForeignKey(a => a.FacultyId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.Major)
                      .WithMany(m => m.AlumniProfiles)
                      .HasForeignKey(a => a.MajorId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ===== EVENT =====
            modelBuilder.Entity<Event>(entity =>
            {
                entity.ToTable("events");
                entity.Property(e => e.Title).HasMaxLength(255).IsRequired();

                entity.HasOne(e => e.CreatedByUser)
                      .WithMany(u => u.EventsCreated)
                      .HasForeignKey(e => e.CreatedBy)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ===== EVENT REGISTRATION =====
            modelBuilder.Entity<EventRegistration>(entity =>
            {
                entity.ToTable("event_registrations");

                entity.HasIndex(r => new { r.EventId, r.UserId }).IsUnique();

                entity.Property(r => r.Status)
                      .HasMaxLength(20)
                      .HasDefaultValue("registered");
            });
        }
    }
}
