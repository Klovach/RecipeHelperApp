using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RecipeHelperApp.Data;
using RecipeHelperApp.Data.Migrations;
using RecipeHelperApp.Interfaces;
using RecipeHelperApp.Models;
using RecipeHelperApp.Services;

namespace RecipeHelperApp.Controllers
{
    public class DaysController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IRecipeGenerator _recipeService;
        private readonly ILogger<DaysController> _logger;
        private readonly IPhotoService _photoService;

        public DaysController(ApplicationDbContext context, IPhotoService photoService, IRecipeGenerator recipeService, ILogger<DaysController> logger)
        {
            _photoService = photoService; 
            _context = context;
            _recipeService = recipeService;
            _logger = logger;
        }


        public async Task<IActionResult> Index(int? weekId, int? dayId)
        {
            if (weekId != null)
            {
                var days = await _context.Days
                                         .Include(d => d.Week)
                                         .Include(d => d.Recipes)
                                         .Where(d => d.Week.Id == weekId)
                                         .ToListAsync();

                foreach (var day in days)
                {
                    await Task.Run(() => day.CalculateRecipeTotals());
                }

                return View("Index", days);
            }
            else if (dayId != null)
            {
                var dayModel = await _context.Days
                             .Include(d => d.Week)
                             .Include(d => d.Recipes)
                             .FirstOrDefaultAsync(d => d.Id == dayId);

                if (dayModel == null)
                {
                    return NotFound();
                }

                var weekFromDayId = dayModel.WeekId;

                var days = await _context.Days
                                        .Include(d => d.Week)
                                        .Include(d => d.Recipes)
                                        .Where(d => d.Week.Id == weekFromDayId)
                                        .ToListAsync();
                if (days == null)
                {
                    return NotFound();
                }

                // Calculate recipe totals for the retrieved day
                foreach (var day in days)
                {
                    await Task.Run(() => day.CalculateRecipeTotals());
                }

               
                return View("Index", days); 
            }
            else
            {
                return BadRequest("Please provide either weekId or dayId.");
            }
        }

            public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var day = await _context.Days
                .FirstOrDefaultAsync(m => m.Id == id);
            if (day == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "Recipes", new { dayId = day.Id });
        }


        public IActionResult Create()
        {
            ViewData["WeekId"] = new SelectList(_context.Weeks, "Id", "UserId");
            return View();
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,WeekId,WeekDay,TotalCalories,TotalProtein,TotalFat,TotalCarbs")] Day day)
        {
            if (ModelState.IsValid)
            {
                _context.Add(day);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["WeekId"] = new SelectList(_context.Weeks, "Id", "UserId", day.WeekId);
            return View(day);
        }

        // GET: Days/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var day = await _context.Days.FindAsync(id);
            if (day == null)
            {
                return NotFound();
            }
            ViewData["WeekId"] = new SelectList(_context.Weeks, "Id", "UserId", day.WeekId);
            return View(day);
        }

        // POST: Days/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,WeekId,WeekDay,TotalCalories,TotalProtein,TotalFat,TotalCarbs")] Day day)
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
            ViewData["WeekId"] = new SelectList(_context.Weeks, "Id", "UserId", day.WeekId);
            return View(day);
        }


        public async Task<IActionResult> Generate(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var day = await _context.Days
                .Include(d => d.Week)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (day == null)
            {
                return NotFound();
            }

            return View(day);
        }

        public async Task<IActionResult> GenerateConfirmed(int id)
        {
           
            var day = await _context.Days.FindAsync(id);
            if (day == null)
            {
                return NotFound();
            }


            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _context.Users.FindAsync(userId);


            if (user == null)
            {
                return NotFound();
            }

            var nutritionForm = await _context.NutritionForms.FirstOrDefaultAsync(nf => nf.UserId == userId);
            if (nutritionForm == null)
            {
                return NotFound();
            }

            var recipes = _context.Recipes.Where(r => r.DayId == day.Id);

            foreach (var recipe in recipes)
            {
                //  var genRecipe = await recipe.GenerateRecipe(nutritionForm);
                var genRecipe = await _recipeService.GenerateRecipe(nutritionForm, recipe);


                // Detach the existing entity from the context
                _context.Entry(recipe).State = EntityState.Detached;

                _context.Update(genRecipe);
            }



            // Save changes to the database
            await _context.SaveChangesAsync();


            return RedirectToAction("Index", new { weekId = day.WeekId });
        }


        // GET: Days/Delete/5
        public async Task<IActionResult> Reset(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var day = await _context.Days
                .Include(d => d.Week)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (day == null)
            {
                return NotFound();
            }

            return View(day);
        }

        public async Task<IActionResult> ResetConfirmed(int id)
        {
            Console.WriteLine("Id was " + id);

            var day = await _context.Days.FindAsync(id);
            if (day == null)
            {
                return NotFound();
            }

            var week = await _context.Weeks.FindAsync(day.WeekId);
            if (week == null)
            {
                return NotFound();
            }

            var recipes = _context.Recipes.Where(r => r.DayId == day.Id);

            foreach (var recipe in recipes)
            {
                if (recipe.Image != null)
                {
                    _ = _photoService.DeletePhotoAsync(recipe.Image);
                }
                recipe.ResetValues();
            }


            await _context.SaveChangesAsync();


            return RedirectToAction("Index", new { weekId = day.WeekId });
        }




        // GET: Days/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var day = await _context.Days
                .Include(d => d.Week)
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
            var day = await _context.Days.FindAsync(id);
            if (day != null)
            {
                _context.Days.Remove(day);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DayExists(int id)
        {
            return _context.Days.Any(e => e.Id == id);
        }
    }
}
