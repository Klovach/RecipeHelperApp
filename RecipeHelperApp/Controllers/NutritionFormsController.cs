using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;
using RecipeHelperApp.Data;
using RecipeHelperApp.Data.Migrations;
using RecipeHelperApp.Models;
using RecipeHelperApp.ViewModels;

namespace RecipeHelperApp.Controllers
{
    [Authorize]
    public class NutritionFormsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<NutritionFormsController> _logger;

        public NutritionFormsController(ApplicationDbContext context, ILogger<NutritionFormsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: NutritionForms
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var nutritionForm = await _context.NutritionForms
              .Include(n => n.User)
              .FirstOrDefaultAsync(m => m.UserId == userId);


            if (nutritionForm == null)
            {
                return NotFound();
            }

            var nutritionFormDTO = new NutritionFormDTO
            {
                Id = nutritionForm.Id,
                UserId = nutritionForm.UserId,
                IncludeNutrition = nutritionForm.IncludeNutrition,
                IncludeIngredients = nutritionForm.IncludeIngredients,
                ExcludeIngredients = nutritionForm.ExcludeIngredients,
                IncludedIngredients = nutritionForm.IncludedIngredients,
                ExcludedIngredients = nutritionForm.ExcludedIngredients,
                Nutrients = nutritionForm.Nutrients,
                Weight = user.Weight,
                ActivityLevel = user.ActivityLevel,
                FitnessGoal = user.FitnessGoal,
                TargetWeight = user.TargetWeight,
                TargetWeightDate = user.TargetWeightDate
            };

            return View(nutritionFormDTO);
        }

        // GET: NutritionForms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nutritionForm = await _context.NutritionForms
                .Include(n => n.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (nutritionForm == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(nutritionForm.UserId);

            if (user == null)
            {
                return NotFound();
            }

            var nutritionFormDTO = new NutritionFormDTO
            {
                Id = nutritionForm.Id,
                UserId = nutritionForm.UserId,
                IncludeNutrition = nutritionForm.IncludeNutrition,
                IncludeIngredients = nutritionForm.IncludeIngredients,
                ExcludeIngredients = nutritionForm.ExcludeIngredients,
                IncludedIngredients = nutritionForm.IncludedIngredients,
                ExcludedIngredients = nutritionForm.ExcludedIngredients,
                Nutrients = nutritionForm.Nutrients,
                Weight = user.Weight,
                ActivityLevel = user.ActivityLevel,
                FitnessGoal = user.FitnessGoal,
                TargetWeight = user.TargetWeight,
                TargetWeightDate = user.TargetWeightDate
            };

            return View(nutritionFormDTO);
        }

        // GET: NutritionForms/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: NutritionForms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,IncludeNutrition,IncludeIngredients,ExcludeIngredients,IncludedIngredients,ExcludedIngredients,NutrientsJson")] Models.NutritionForm nutritionForm)
        {
            if (ModelState.IsValid)
            {
                _context.Add(nutritionForm);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", nutritionForm.UserId);
            return View(nutritionForm);
        }

        // GET: NutritionForms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nutritionForm = await _context.NutritionForms.FindAsync(id);
            if (nutritionForm == null)
            {
                return NotFound();
            }

            // Get user
            var user = await _context.Users.FindAsync(nutritionForm.UserId);

            if (user == null)
            {
                return NotFound();
            }

            var nutritionFormDTO = new NutritionFormDTO
            {
                Id = nutritionForm.Id,
                UserId = nutritionForm.UserId,
                IncludeNutrition = nutritionForm.IncludeNutrition,
                IncludeIngredients = nutritionForm.IncludeIngredients,
                ExcludeIngredients = nutritionForm.ExcludeIngredients,
                IncludedIngredients = nutritionForm.IncludedIngredients,
                ExcludedIngredients = nutritionForm.ExcludedIngredients,
                Nutrients = nutritionForm.Nutrients,
                Weight = user.Weight,
                ActivityLevel = user.ActivityLevel,
                FitnessGoal = user.FitnessGoal,
                TargetWeight = user.TargetWeight,
                TargetWeightDate = user.TargetWeightDate
            };

            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", nutritionForm.UserId);
            return View(nutritionFormDTO);
        }

        // POST: NutritionForms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,IncludeNutrition,IncludeIngredients,ExcludeIngredients,IncludedIngredients,ExcludedIngredients,Nutrients, Weight, TargetWeight, TargetWeightDate, ActivityLevel, FitnessGoal")] NutritionFormDTO nutritionFormDTO)
        {
            if (!ModelState.IsValid)
            {
                // If ModelState is not valid, return the view with the validation errors
                return View(nutritionFormDTO);
            }

            try
            {
                var user = await _context.Users.FindAsync(nutritionFormDTO.UserId);
                if (user == null)
                {
                    return NotFound("User not found");
                }

                // Update user properties
                user.Weight = nutritionFormDTO.Weight;
                user.ActivityLevel = nutritionFormDTO.ActivityLevel;
                user.FitnessGoal = nutritionFormDTO.FitnessGoal;
                user.TargetWeight = nutritionFormDTO.TargetWeight;
                user.TargetWeightDate = nutritionFormDTO.TargetWeightDate;

                // Update NutritionForm properties
                var nutritionForm = new Models.NutritionForm
                {
                    Id = nutritionFormDTO.Id,
                    UserId = nutritionFormDTO.UserId,
                    IncludeNutrition = nutritionFormDTO.IncludeNutrition,
                    IncludeIngredients = nutritionFormDTO.IncludeIngredients,
                    ExcludeIngredients = nutritionFormDTO.ExcludeIngredients,
                    IncludedIngredients = nutritionFormDTO.IncludedIngredients,
                    ExcludedIngredients = nutritionFormDTO.ExcludedIngredients,
                    Nutrients = nutritionFormDTO.Nutrients
                };

                // Update the entity state
                _context.Update(user);
                _context.Update(nutritionForm);

                await _context.SaveChangesAsync();


                return RedirectToAction("Index");
                //  return RedirectToAction("Edit", new { id = nutritionForm.Id });

            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle concurrency exception
                ModelState.AddModelError("", "Concurrency error occurred while saving changes.");
                return View(nutritionFormDTO);
            }
        }



        public async Task<IActionResult> Recalculate(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nutritionForm = await _context.NutritionForms.FindAsync(id);
            if (nutritionForm == null)
            {
                return NotFound();
            }

            // Retrieve the user from the database
            var user = await _context.Users.FindAsync(nutritionForm.UserId);
            if (user == null)
            {
                return NotFound();
            }

            // Call the CalculateNewValues method
            nutritionForm.CalculateNewValues(user);

            // Save changes to the database
            await _context.SaveChangesAsync();


            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Restart(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nutritionForm = await _context.NutritionForms.FindAsync(id);
            if (nutritionForm == null)
            {
                return NotFound();
            }

            // Retrieve the user from the database
            var user = await _context.Users.FindAsync(nutritionForm.UserId);
            if (user == null)
            {
                return NotFound();
            }

            // Call the CalculateNewValues method
            nutritionForm.ResetValues();

            // Save changes to the database
            await _context.SaveChangesAsync();


            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Reset(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nutritionForm = await _context.NutritionForms.FindAsync(id);
            if (nutritionForm == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", nutritionForm.UserId);
            return View(nutritionForm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reset(int id, [Bind("Id,UserId,IncludeNutrition,IncludeIngredients,ExcludeIngredients,IncludedIngredients,ExcludedIngredients,Nutrients")] NutritionFormDTO nutritionFormDTO)
        {
            var nutritionForm = new Models.NutritionForm
            {
                Id = nutritionFormDTO.Id,
                UserId = nutritionFormDTO.UserId,
                IncludeNutrition = true,
                IncludeIngredients = false,
                ExcludeIngredients = false,
                IncludedIngredients = null,
                ExcludedIngredients = null,
                Nutrients = new Nutrients()
            };

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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", nutritionForm.UserId);
            return View(nutritionForm);
        }


        // GET: NutritionForms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nutritionForm = await _context.NutritionForms
                .Include(n => n.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (nutritionForm == null)
            {
                return NotFound();
            }

            return View(nutritionForm);
        }

        // POST: NutritionForms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nutritionForm = await _context.NutritionForms.FindAsync(id);
            if (nutritionForm != null)
            {
                _context.NutritionForms.Remove(nutritionForm);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NutritionFormExists(int id)
        {
            return _context.NutritionForms.Any(e => e.Id == id);
        }
    }
}
