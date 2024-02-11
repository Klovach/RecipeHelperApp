using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RecipeHelperApp.Data;
using RecipeHelperApp.Data.Migrations;
using RecipeHelperApp.Models;

namespace RecipeHelperApp.Controllers
{
    public class DaysController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DaysController(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET: Days
        public async Task<IActionResult> Index(int? weekId)
        {
            // Retrieve days associated with the weeks of the current user
            var days = await _context.Day
                                    // Filter day by week id & include recipes. 
                                    .Include(d => d.Week)
                                    .Include(d => d.Recipes)
                                    .Where(d => d.Week.Id == weekId)
                                    .ToListAsync();

            foreach (var day in days)
            {
               await Task.Run(() => day.CalculateRecipeTotals()); // Calculate totals for each day
            }
            return View(days);
        }

        // GET: Days/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var day = await _context.Day
                .FirstOrDefaultAsync(m => m.Id == id);
            if (day == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "Recipes", new { dayId = day.Id });
        }

        // GET: Days/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Days/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,WeekDay,TotalCalories,TotalProtein,TotalFat,TotalCarbs")] Day day)
        {
            if (ModelState.IsValid)
            {
                _context.Add(day);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(day);
        }

        // GET: Days/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var day = await _context.Day.FindAsync(id);
            if (day == null)
            {
                return NotFound();
            }
            return View(day);
        }

        // POST: Days/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,WeekDay,TotalCalories,TotalProtein,TotalFat,TotalCarbs")] Day day)
        {
            if (id != day.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(day);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DayExists(day.Id))
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
            return View(day);
        }

        // GET: Days/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var day = await _context.Day
                .FirstOrDefaultAsync(m => m.Id == id);
            if (day == null)
            {
                return NotFound();
            }

            return View(day);
        }

        // POST: Days/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var day = await _context.Day.FindAsync(id);
            if (day != null)
            {
                _context.Day.Remove(day);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DayExists(int id)
        {
            return _context.Day.Any(e => e.Id == id);
        }
    }
}
