using Microsoft.EntityFrameworkCore;

namespace Enrollment.Infrastructure.Data
{
    public class EnrollmentDbContext : DbContext
    {
        public EnrollmentDbContext(DbContextOptions<EnrollmentDbContext> options) : base(options) { }

        public DbSet<Enrollment.Domain.Entities.Enrollment> Enrollments
            => Set<Enrollment.Domain.Entities.Enrollment>();

        public DbSet<Enrollment.Domain.Entities.AuditLog> AuditLogs
            => Set<Enrollment.Domain.Entities.AuditLog>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Enrollment.Domain.Entities.Enrollment>()
                .HasIndex(x => new { x.StudentId, x.CourseId })
                .IsUnique();
        }
    }
}
