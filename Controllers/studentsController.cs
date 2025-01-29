using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using first.Data;
using first.Models;

namespace first.Controllers
{
    public class studentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public studentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: students
        public async Task<IActionResult> Index()
        {
            return View(await _context.Student.ToListAsync());
        }

        // GET: students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .FirstOrDefaultAsync(m => m.ID == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("ID,Name,Email")] student student)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(student);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(student);
        //}
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] student student)
        {
            if (student == null) return BadRequest("Invalid data received.");

            if (ModelState.IsValid)
            {

                _context.Add(student);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "student created successfully" });
            }
            return BadRequest(ModelState);
        }
        // GET: students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Email")] student student)
        //{
        //    if (id != student.ID)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(student);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!studentExists(student.ID))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(student);
        //}

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [FromBody] student student)
        {
            if (student == null)
            {
                return BadRequest(new { success = false, message = "Invalid student data" });
            }


            var existingStudent = await _context.Student.FindAsync(id);
            if (existingStudent == null)
            {
                return NotFound(new { success = false, message = "student not found" });
            }


            existingStudent.Name = student.Name;
            existingStudent.Email = student.Email;

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return Json(new { success = true, message = "Student updated successfully" });
                }
                catch (DbUpdateConcurrencyException)
                {
                    return StatusCode(500, new { success = false, message = "Concurrency error occurred" });
                }
            }

            return BadRequest(new { success = false, message = "Invalid data" });
        }

        // GET: students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .FirstOrDefaultAsync(m => m.ID == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Student.FindAsync(id);
            if (student != null)
            {
                _context.Student.Remove(student);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Student.Any(e => e.ID == id);
        }


        //All student Get
        [HttpGet]
        public async Task<IActionResult> GetAllStudents(string name, string email)
        {
            var query = _context.Student.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(s => s.Name.Contains(name));
            }

            if (!string.IsNullOrEmpty(email))
            {
                query = query.Where(s => s.Email.Contains(email));
            }

            var studentList = await query.ToListAsync();
            return Json(studentList);
        }




        public IActionResult AllStudents()
        {
            return View();
        }
    }
}

