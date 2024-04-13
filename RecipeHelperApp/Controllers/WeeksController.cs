using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RecipeHelperApp.Data;
using RecipeHelperApp.Models;
using RecipeHelperApp.ViewModels;

namespace RecipeHelperApp
{
    [Authorize]
    public class WeeksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<WeeksController> _logger;

        public WeeksController(ApplicationDbContext context, ILogger<WeeksController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Weeks
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var weeks = await _context.Weeks
                          .Include(d => d.User)
                          .Where(d => d.User.Id == userId)
                          .ToListAsync();


            return View(weeks);
        }

        // GET: Weeks/Details/5
         public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var week = await _context.Weeks.FirstOrDefaultAsync(m => m.Id == id);

            if (week == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "Days", new { weekId = week.Id });
        }

        // GET: Weeks/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Weeks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,WeekName,Description")] WeekDTO weekDTO)
        {
            var week = new Week
            {
                UserId = weekDTO.UserId,
                WeekName = weekDTO.WeekName,
                Description = weekDTO.Description
            };

            if (ModelState.IsValid)
            {
                // Retrieve the user based on UserId.
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == week.UserId);

                // Check if the user exists
                if (user != null)
                {
                    // Set the User property in the Week object
                    week.User = user;

                    week.InitializeDays();

                    foreach (var day in week.Days)
                    {
                        Console.WriteLine(day.ToString());
                        _context.Add(day);
                    }


                    // Add the week to the context and save changes
                    _context.Add(week);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }

                else
                {
                    // Handle case where user does not exist
                    ModelState.AddModelError(string.Empty, "Invalid user selected.");
                }
            }

            // If model state is not valid or user not found, return to the view with the model
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", week.UserId);
            return View(week);
        }

        // GET: Weeks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var week = await _context.Weeks.FindAsync(id);
            if (week == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", week.UserId);
            return View(week);
        }

        // POST: Weeks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,WeekName,Description")] WeekDTO weekDTO)
        {

            var week = new Week
            {
                Id = weekDTO.Id,
                UserId = weekDTO.UserId,
                WeekName = weekDTO.WeekName,
                Description = weekDTO.Description
            };


            if (id != week.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(week);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WeekExists(week.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", week.UserId);
            return View(week);
        }

        // GET: Weeks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var week = await _context.Weeks
                .Include(w => w.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (week == null)
            {
                return NotFound();
            }

            return View(week);
        }

        // POST: Weeks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var week = await _context.Weeks.FindAsync(id);
            if (week != null)
            {
                _context.Weeks.Remove(week);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WeekExists(int id)
        {
            return _context.Weeks.Any(e => e.Id == id);
        }
    }
}
