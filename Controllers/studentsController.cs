using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using first.Data;
using first.Models;

namespace first.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            return View(await _context.Student.ToListAsync());
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var student = await _context.Student.FirstOrDefaultAsync(m => m.ID == id);
            if (student == null) return NotFound();

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create (Handles File Upload)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student, IFormFile imageFile, IFormFile thumbnailFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await imageFile.CopyToAsync(memoryStream);
                        student.ImageData = memoryStream.ToArray(); // Store as byte[]
                    }
                }

                if (thumbnailFile != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await thumbnailFile.CopyToAsync(memoryStream);
                        student.ThumbnailData = memoryStream.ToArray(); // Store as byte[]
                    }
                }

                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Student student, IFormFile? imageFile, IFormFile? thumbnailFile)
        {
            if (id != student.ID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var existingStudent = await _context.Student.FindAsync(id);
                    if (existingStudent == null) return NotFound();

                    // ✅ Update basic details
                    existingStudent.Name = student.Name;
                    existingStudent.Email = student.Email;
                    existingStudent.PhoneNumber = student.PhoneNumber;

                    // ✅ Keep old image if no new file is uploaded
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await imageFile.CopyToAsync(memoryStream);
                            existingStudent.ImageData = memoryStream.ToArray();
                        }
                    }

                    // ✅ Keep old thumbnail if no new file is uploaded
                    if (thumbnailFile != null && thumbnailFile.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await thumbnailFile.CopyToAsync(memoryStream);
                            existingStudent.ThumbnailData = memoryStream.ToArray();
                        }
                    }

                    _context.Update(existingStudent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.ID)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            // ✅ Ensure the old image data is returned so it remains visible after validation errors
            var studentWithImages = await _context.Student.FindAsync(id);
            return View(studentWithImages);
        }



        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var student = await _context.Student.FirstOrDefaultAsync(m => m.ID == id);
            if (student == null) return NotFound();

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Student.FindAsync(id);
            if (student != null) _context.Student.Remove(student);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Student.Any(e => e.ID == id);
        }
    }
}
