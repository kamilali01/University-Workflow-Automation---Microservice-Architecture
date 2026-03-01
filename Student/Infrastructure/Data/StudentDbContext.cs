using Microsoft.EntityFrameworkCore;

namespace Student.Infrastructure.Data
{
    public class StudentDbContext : DbContext
    {
        public StudentDbContext(DbContextOptions<StudentDbContext> options) : base(options) { }

        public DbSet<Student.Domain.Entities.Student> Students => Set<Student.Domain.Entities.Student>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student.Domain.Entities.Student>()
                .HasIndex(x => x.Email)
                .IsUnique();
        }
    }
}
