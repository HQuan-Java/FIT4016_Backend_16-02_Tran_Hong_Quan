using Microsoft.EntityFrameworkCore;
using SchoolManagement.Models;

namespace SchoolManagement.Data
{
    public class SchoolDbContext : DbContext
    {
        public SchoolDbContext(DbContextOptions<SchoolDbContext> options)
            : base(options)
        {
        }

        public DbSet<School> Schools { get; set; }
        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Unique constraint: School.Name
            modelBuilder.Entity<School>()
                .HasIndex(s => s.Name)
                .IsUnique();

            // Unique constraint: Student.StudentId
            modelBuilder.Entity<Student>()
                .HasIndex(s => s.StudentId)
                .IsUnique();

            // Unique constraint: Student.Email
            modelBuilder.Entity<Student>()
                .HasIndex(s => s.Email)
                .IsUnique();

            // Relationship: School 1 - n Students
            modelBuilder.Entity<Student>()
                .HasOne(s => s.School)
                .WithMany(sc => sc.Students)
                .HasForeignKey(s => s.SchoolId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
