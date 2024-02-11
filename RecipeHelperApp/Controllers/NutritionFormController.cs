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

namespace RecipeHelperApp.Controllers
{
    public class NutritionFormController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NutritionFormController(ApplicationDbContext context)
        {
            _context = context;
        }

        public string GetCurrentUserId()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return userId;
        }

        // GET: NutritionForm
        public async Task<IActionResult> Index()
        {
            var userId = GetCurrentUserId();

            var items = await _context.NutritionForm
                               .Where(i => i.User.Id == userId)
                               .ToListAsync();
           
            return View(items);
        }

        // GET: NutritionForm/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nutritionForm = await _context.NutritionForm
                .FirstOrDefaultAsync(m => m.Id == id);
            if (nutritionForm == null)
            {
                return NotFound();
            }

            return View(nutritionForm);
        }

        // GET: NutritionForm/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NutritionForm/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IncludeNutrition,IncludeIngredients,ExcludeIngredients,IncludedIngredients,ExcludedIngredients,NutrientsJson")] NutritionForm nutritionForm)
        {
            if (ModelState.IsValid)
            {
                _context.Add(nutritionForm);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(nutritionForm);
        }

        // GET: NutritionForm/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nutritionForm = await _context.NutritionForm.FindAsync(id);
            if (nutritionForm == null)
            {
                return NotFound();
            }
            return View(nutritionForm);
        }

        // POST: NutritionForm/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IncludeNutrition,IncludeIngredients,ExcludeIngredients,IncludedIngredients,ExcludedIngredients,NutrientsJson, Nutrients")] NutritionForm nutritionForm)
        {
            if (id != nutritionForm.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nutritionForm);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NutritionFormExists(nutritionForm.Id))
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
            return View(nutritionForm);
        }

        // GET: NutritionForm/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nutritionForm = await _context.NutritionForm
                .FirstOrDefaultAsync(m => m.Id == id);
            if (nutritionForm == null)
            {
                return NotFound();
            }

            return View(nutritionForm);
        }

        // POST: NutritionForm/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nutritionForm = await _context.NutritionForm.FindAsync(id);
            if (nutritionForm != null)
            {
                _context.NutritionForm.Remove(nutritionForm);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NutritionFormExists(int id)
        {
            return _context.NutritionForm.Any(e => e.Id == id);
        }
    }
}
