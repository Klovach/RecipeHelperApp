using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RecipeHelperApp.Data;
using RecipeHelperApp.Models;

namespace RecipeHelperApp
{
    public class WeeksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WeeksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Weeks
        public async Task<IActionResult> Index()
        {
         //   return View(await _context.Week.ToListAsync());
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var weeks = await _context.Week
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

            var week = await _context.Week
                .FirstOrDefaultAsync(m => m.Id == id);
            if (week == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "Days", new { weekId = week.Id });
        }

        // GET: Weeks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Weeks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create([Bind("Id,WeekName,Description,Days")] Week week, string userId)
{
    Console.WriteLine("Called create");

    if (!ModelState.IsValid)
    {
        Console.WriteLine("Model State is not valid");
        foreach (var modelStateEntry in ModelState.Values)
        {
            foreach (var error in modelStateEntry.Errors)
            {
                Console.WriteLine(error.ErrorMessage);
            }
        }
    }

    week.InitializeDays();

    if (ModelState.IsValid)
    {
        Console.WriteLine("Model State is valid");
        // Retrieve user based on userId
        var user = await _context.Users.FindAsync(userId);
        if (user != null)
        {
            week.User = user;
            _context.Add(week);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        else
        {
            ModelState.AddModelError(string.Empty, "User not found.");
        }
    }
    return View(week);
}

        // GET: Weeks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var week = await _context.Week.FindAsync(id);
            if (week == null)
            {
                return NotFound();
            }
            return View(week);
        }

        // POST: Weeks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,WeekName,Description,Days")] Week week)
        {
            if (id != week.Id)
            {
                return NotFound();
            }


            if (!ModelState.IsValid)
            {
                Console.WriteLine("Model State is not valid");
                foreach (var modelStateEntry in ModelState.Values)
                {
                    foreach (var error in modelStateEntry.Errors)
                    {
                        Console.WriteLine(error.ErrorMessage);
                    }
                }
            }

            if (ModelState.IsValid)
            {
                Console.WriteLine("Model State is alid");
                try
                {
                    Console.WriteLine("Updating week.");
                    _context.Update(week);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WeekExists(week.Id))
                    {
                        Console.WriteLine("Week doesn't exist.");
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(week);
        }

        // GET: Weeks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var week = await _context.Week
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
            var week = await _context.Week.FindAsync(id);
            if (week != null)
            {
                _context.Week.Remove(week);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WeekExists(int id)
        {
            return _context.Week.Any(e => e.Id == id);
        }
    }
}
