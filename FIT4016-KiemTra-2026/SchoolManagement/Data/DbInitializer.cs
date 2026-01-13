using SchoolManagement.Models;

namespace SchoolManagement.Data
{
    public static class DbInitializer
    {
        public static void Seed(SchoolDbContext context)
        {
            if (context.Schools.Any()) return;

            var schools = Enumerable.Range(1, 10)
                .Select(i => new School
                {
                    Name = $"School {i}",
                    Principal = $"Principal {i}",
                    Address = $"Address {i}"
                }).ToList();

            context.Schools.AddRange(schools);
            context.SaveChanges();

            var students = Enumerable.Range(1, 20)
                .Select(i => new Student
                {
                    FullName = $"Student {i}",
                    StudentId = $"ST{i:000}",
                    Email = $"student{i}@example.com",
                    Phone = "0123456789",
                    SchoolId = schools[i % 10].Id
                }).ToList();

            context.Students.AddRange(students);
            context.SaveChanges();
        }
    }
}
