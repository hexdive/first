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
    public class StaffsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StaffsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Staffs
        public async Task<IActionResult> Index()
        {
            return View(await _context.Staff.ToListAsync());
        }

        // GET: Staffs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staff = await _context.Staff
                .FirstOrDefaultAsync(m => m.ID == id);
            if (staff == null)
            {
                return NotFound();
            }

            return View(staff);
        }
        //get : staff
        public IActionResult Create()
        {
            return View();
        }

        /// POST: Staffs/Create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Staff staff)
        {
            if (staff == null) return Json(new { success = false, message = "Invalid staff data" });

            if (ModelState.IsValid)
            {
                _context.Add(staff);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Staff created successfully" });
            }
            return BadRequest(ModelState);
        }














        // GET: Staffs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staff = await _context.Staff.FindAsync(id);
            if (staff == null)
            {
                return NotFound();
            }
            return View(staff);
        }

        

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [FromBody] Staff staff)
        {
            if (staff == null)
            {
                return BadRequest(new { success = false, message = "Invalid staff data" });
            }


            var existingStaff = await _context.Staff.FindAsync(id);
            if (existingStaff == null)
            {
                return NotFound(new { success = false, message = "Staff not found" });
            }


            existingStaff.Name = staff.Name;
            existingStaff.Email = staff.Email;

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return Json(new { success = true, message = "Staff updated successfully" });
                }
                catch (DbUpdateConcurrencyException)
                {
                    return StatusCode(500, new { success = false, message = "Concurrency error occurred" });
                }
            }

            return BadRequest(new { success = false, message = "Invalid data" });
        }

        // GET: Staffs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staff = await _context.Staff
                .FirstOrDefaultAsync(m => m.ID == id);
            if (staff == null)
            {
                return NotFound();
            }

            return View(staff);
        }

        // POST: Staffs/Delete/5
        [HttpPost, ActionName("Delete")]
        
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var staff = await _context.Staff.FindAsync(id);
            if (staff != null)
            {
                _context.Staff.Remove(staff);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StaffExists(int id)
        {
            return _context.Staff.Any(e => e.ID == id);
        }

        //All staff Get
        [HttpGet]
        public async Task<IActionResult> GetAllStaffs(string name, string email)
        {
            var query = _context.Staff.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(s => s.Name.Contains(name));
            }

            if (!string.IsNullOrEmpty(email))
            {
                query = query.Where(s => s.Email.Contains(email));
            }

            var staffList = await query.ToListAsync();
            return Json(staffList);
        }




        public IActionResult AllStaffs()
        {
            return View();
        }
    }



    }
