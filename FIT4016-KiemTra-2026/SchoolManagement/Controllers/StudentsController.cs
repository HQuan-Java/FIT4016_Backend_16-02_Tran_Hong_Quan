using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;

namespace SchoolManagement.Controllers
{
    public class StudentsController : Controller
    {
        private readonly SchoolDbContext _context;

        public StudentsController(SchoolDbContext context)
        {
            _context = context;
        }

        // READ: List students with pagination
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 10;

            var studentsQuery = _context.Students
                .Include(s => s.School)
                .OrderBy(s => s.Id);

            int totalStudents = await studentsQuery.CountAsync();

            var students = await studentsQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalStudents / (double)pageSize);

            return View(students);
        }
    }
}
