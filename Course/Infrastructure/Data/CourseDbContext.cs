using Microsoft.EntityFrameworkCore;

namespace Course.Infrastructure.Data
{
    public class CourseDbContext : DbContext
    {
        public CourseDbContext(DbContextOptions<CourseDbContext> options) : base(options) { }

        public DbSet<Course.Domain.Entities.Course> Courses => Set<Course.Domain.Entities.Course>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course.Domain.Entities.Course>()
                .HasIndex(x => x.Code)
                .IsUnique();
        }
    }
}
