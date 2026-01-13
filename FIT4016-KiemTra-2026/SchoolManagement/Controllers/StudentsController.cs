using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using SchoolManagement.Models;

namespace SchoolManagement.Controllers
{
    public class StudentsController : Controller
    {
        private readonly SchoolDbContext _context;

        public StudentsController(SchoolDbContext context)
        {
            _context = context;
        }

        // =========================
        // READ - LIST STUDENTS
        // =========================
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 10;

            var students = _context.Students
                .Include(s => s.School)
                .OrderBy(s => s.Id);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(await students.CountAsync() / (double)pageSize);

            return View(await students
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync());
        }

        // =========================
        // CREATE - GET
        // =========================
        public IActionResult Create()
        {
            ViewBag.Schools = new SelectList(_context.Schools, "Id", "Name");
            return View();
        }

        // =========================
        // CREATE - POST
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            ViewBag.Schools = new SelectList(_context.Schools, "Id", "Name", student.SchoolId);

            // Validate duplicate Student ID
            if (_context.Students.Any(s => s.StudentId == student.StudentId))
            {
                ModelState.AddModelError("StudentId", "Student ID already exists");
            }

            // Validate duplicate Email
            if (_context.Students.Any(s => s.Email == student.Email))
            {
                ModelState.AddModelError("Email", "Email already exists");
            }

            if (!ModelState.IsValid)
            {
                return View(student);
            }

            try
            {
                _context.Students.Add(student);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Student created successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An error occurred while creating the student");
                return View(student);
            }
        }

        // =========================
        // UPDATE - GET
        // =========================
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            ViewBag.Schools = new SelectList(_context.Schools, "Id", "Name", student.SchoolId);
            return View(student);
        }

        // =========================
        // UPDATE - POST
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            ViewBag.Schools = new SelectList(_context.Schools, "Id", "Name", student.SchoolId);

            // Check Email duplicate (except itself)
            if (_context.Students.Any(s => s.Email == student.Email && s.Id != student.Id))
            {
                ModelState.AddModelError("Email", "Email already exists");
            }

            if (!ModelState.IsValid)
            {
                return View(student);
            }

            try
            {
                student.UpdatedAt = DateTime.Now;
                _context.Update(student);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Student updated successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An error occurred while updating the student");
                return View(student);
            }
        }

        // =========================
        // DELETE - GET (CONFIRM)
        // =========================
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.School)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // =========================
        // DELETE - POST
        // =========================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var student = await _context.Students.FindAsync(id);
                if (student != null)
                {
                    _context.Students.Remove(student);
                    await _context.SaveChangesAsync();
                }

                TempData["SuccessMessage"] = "Student deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An error occurred while deleting the student");
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
