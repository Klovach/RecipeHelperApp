using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using RecipeHelperApp.Data;
using RecipeHelperApp.Models;
using RecipeHelperApp.Services;
using RecipeHelperApp.ViewModels;

namespace RecipeHelperApp.Controllers
{
    /// <summary>
    /// Controller for managing recipes.
    /// </summary>

    [Authorize]
    public class RecipesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPhotoService _photoService;
        private readonly IRecipeGenerator _recipeService; 
        private readonly ILogger<RecipesController> _logger;


        /// <summary>
        /// Constructor for RecipesController.
        /// </summary>
        public RecipesController(ApplicationDbContext context, IPhotoService photoService, IRecipeGenerator recipeService, ILogger<RecipesController> logger)
        {
            _context = context;
            _photoService = photoService;
            _recipeService = recipeService;
            _logger = logger;
        }

        // GET: Recipes
        /// <summary>
        /// Displays recipes for a specific day.
        /// </summary>
        /// <param name="dayId">ID of the day for which recipes are displayed.</param>
        public async Task<IActionResult> Index(int dayId)
        {
            var recipes = await _context.Recipes
                             .Include(d => d.Day)
                             .Where(d => d.Day.Id == dayId)
                             .ToListAsync();

            return View(recipes);
        }

        // GET: Recipes/Search/ By Name
        /// <summary>
        /// Searches for recipes by name.
        /// </summary>
        /// <param name="searchString">The string to search for in recipe names.</param>
        public async Task<IActionResult> Search(string searchString)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


            var recipes = from r in _context.Recipes
                          where r.Day.Week.User.Id.Equals(userId)
                          select r;

            if (!string.IsNullOrEmpty(searchString))
            {
                recipes = recipes.Where(r => r.Name.Contains(searchString));
            }

            return View(await recipes.ToListAsync());
        }

        // GET: Recipes/Filter/ By Category
        /// <summary>
        /// Filters recipes by meal type.
        /// </summary>
        /// <param name="mealType">The type of meal to filter by.</param>
        public async Task<IActionResult> Filter(string mealType)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var recipes = from r in _context.Recipes
                          where r.Day.Week.User.Id.Equals(userId)
                          select r;

            if (!string.IsNullOrEmpty(mealType))
            {
                recipes = recipes.Where(r => r.MealType.Contains(mealType));
            }

            return View(await recipes.ToListAsync());
        }

        // Sort by Nutritional Values (ex. Calories). 


        // GET: Recipes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes
                .Include(r => r.Day)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        // GET: Recipes/Create
        public IActionResult Create()
        {
            ViewData["DayId"] = new SelectList(_context.Days, "Id", "Id");
            return View();
        }

        // POST: Recipes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DayId,Image,MealType,Name,Description,Instructions,Ingredients,Calories,Protein,Fat,Carbs")] RecipeDTO recipeDTO)
        {
            var recipe = new Recipe
            {
                DayId = recipeDTO.DayId,
                MealType = recipeDTO.MealType,
                Name = recipeDTO.Name,
                Description = recipeDTO.Description,
                Instructions = recipeDTO.Instructions,
                Ingredients = recipeDTO.Ingredients,
                Calories = recipeDTO.Calories,
                Protein = recipeDTO.Protein,
                Fat = recipeDTO.Fat,
                Carbs = recipeDTO.Carbs
            };

            if (recipeDTO.ImageFile != null && recipeDTO.Image != null)
            {
                var result = await _photoService.AddPhotoAsync(recipeDTO.ImageFile);
                recipe.Image = result.Url.ToString();
            }
            else
            {
                Console.WriteLine("image was null");
            }


            if (ModelState.IsValid)
            {
                _context.Add(recipe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                Console.WriteLine("Upload fail");
            }

            ViewData["DayId"] = new SelectList(_context.Days, "Id", "Id", recipe.DayId);
            return View(recipeDTO);
        }



        public async Task<IActionResult> Generate(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }

            // Convert Recipe to RecipeDTO
            var recipeDTO = new RecipeDTO
            {
                Id = recipe.Id,
                DayId = recipe.DayId,
                MealType = recipe.MealType,
                Name = recipe.Name,
                Description = recipe.Description,
                Instructions = recipe.Instructions,
                Ingredients = recipe.Ingredients,
                Calories = recipe.Calories,
                Protein = recipe.Protein,
                Fat = recipe.Fat,
                Carbs = recipe.Carbs
            };

            ViewData["DayId"] = new SelectList(_context.Days, "Id", "Id", recipe.DayId);
            return View(recipeDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Generate(int id, [Bind("Id, DayId, MealType,Name,Description,Instructions,Ingredients,Calories,Protein,Fat,Carbs")] RecipeDTO recipeDTO)
        {
            var recipe = new Recipe
            {
                Id = recipeDTO.Id,
                DayId = recipeDTO.DayId,
                MealType = recipeDTO.MealType,
                Name = recipeDTO.Name,
                Description = recipeDTO.Description,
                Instructions = recipeDTO.Instructions,
                Ingredients = recipeDTO.Ingredients,
                Calories = recipeDTO.Calories,
                Protein = recipeDTO.Protein,
                Fat = recipeDTO.Fat,
                Carbs = recipeDTO.Carbs
            };

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


            if (id != recipe.Id)
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


            if (ModelState.IsValid)
            {

                Console.WriteLine("Model State is valid");
                try
                {

                    // Call GenerateRecipe asynchronously and await the result
                    var genRecipe = await _recipeService.GenerateRecipe(nutritionForm, recipe);

                    // Update the generated recipe in the database context
                    _context.Entry(genRecipe).State = EntityState.Modified;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipeExists(recipe.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction("Index", "Recipes", new { dayId = recipe.DayId });
            }
            // Write a return view to redirect to details page.

            return View(recipe);
        }



        /*
        public async Task<IActionResult> GenerateBackup(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null)
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

            // Call the CalculateNewValues method
            await recipe.GenerateRecipe(nutritionForm);

            var day = await _context.Days.FindAsync(recipe.DayId);
            if (day == null)
            {
                return NotFound(); // Return NotFound result if day is not found
            }

            // Assign the retrieved Day to the recipe result
            recipe.Day = day;

            // Save changes to the database
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = recipe.Id });
        } */


        // GET: Recipes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }

            // Convert Recipe to RecipeDTO
            var recipeDTO = new RecipeDTO
            {
                Id = recipe.Id,
                DayId = recipe.DayId,
                Image = recipe.Image,
                MealType = recipe.MealType,
                Name = recipe.Name,
                Description = recipe.Description,
                Instructions = recipe.Instructions,
                Ingredients = recipe.Ingredients,
                Calories = recipe.Calories,
                Protein = recipe.Protein,
                Fat = recipe.Fat,
                Carbs = recipe.Carbs
            };

            ViewData["DayId"] = new SelectList(_context.Days, "Id", "Id", recipe.DayId);
            return View(recipeDTO);
        }


        // POST: Recipes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, DayId, Image, ImageFile, MealType,Name,Description,Instructions,Ingredients,Calories,Protein,Fat,Carbs")] RecipeDTO recipeDTO)
        {
            var recipe = new Recipe
            {
                Id = recipeDTO.Id,
                DayId = recipeDTO.DayId,
                MealType = recipeDTO.MealType,
                Name = recipeDTO.Name,
                Description = recipeDTO.Description,
                Instructions = recipeDTO.Instructions,
                Ingredients = recipeDTO.Ingredients,
                Calories = recipeDTO.Calories,
                Protein = recipeDTO.Protein,
                Fat = recipeDTO.Fat,
                Carbs = recipeDTO.Carbs
            };

            if (recipeDTO.ImageFile == null)
            {
                recipe.Image = recipeDTO.Image;
            }

            if (recipeDTO.ImageFile != null)
            {
                var photoResult = await _photoService.AddPhotoAsync(recipeDTO.ImageFile);

                if (photoResult.Error != null)
                {
                    ModelState.AddModelError("Image", "Photo upload failed: " + photoResult.Error.Message);
                    return View(recipeDTO);
                }
                if (!string.IsNullOrEmpty(recipe.Image))
                {
                    _ = _photoService.DeletePhotoAsync(recipe.Image);
                }
                else
                {
                    recipe.Image = photoResult.Url.ToString();
                }
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


            if (id != recipe.Id)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {

                Console.WriteLine("Model State is valid");
                try
                {
                    _context.Update(recipe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipeExists(recipe.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction("Index", "Recipes", new { dayId = recipe.DayId });
            }
            // Write a return view to redirect to details page.

            return View(recipe);
        }



        // GET: Recipes/Edit/5
        public async Task<IActionResult> Reset(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }

            // Convert Recipe to RecipeDTO
            var recipeDTO = new RecipeDTO
            {
                Id = recipe.Id,
                DayId = recipe.DayId,
                MealType = recipe.MealType,
                Name = recipe.Name,
                Description = recipe.Description,
                Instructions = recipe.Instructions,
                Ingredients = recipe.Ingredients,
                Calories = recipe.Calories,
                Protein = recipe.Protein,
                Fat = recipe.Fat,
                Carbs = recipe.Carbs
            };

            ViewData["DayId"] = new SelectList(_context.Days, "Id", "Id", recipe.DayId);
            return View(recipeDTO);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reset(int id, [Bind("Id, DayId, MealType,Name,Description,Instructions,Ingredients,Calories,Protein,Fat,Carbs")] RecipeDTO recipeDTO)
        {
            var recipe = new Recipe
            {
                Id = recipeDTO.Id,
                DayId = recipeDTO.DayId,
                MealType = recipeDTO.MealType,
                Name = "",
                Description = "",
                Instructions = "",
                Ingredients = "",
                Calories = 0,
                Protein = 0,
                Fat = 0,
                Carbs = 0
            };

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


            if (id != recipe.Id)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {

                Console.WriteLine("Model State is valid");
                try
                {
                    _context.Update(recipe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipeExists(recipe.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction("Index", "Recipes", new { dayId = recipe.DayId });
            }
            // Write a return view to redirect to details page.

            return View(recipe);
        }


        // GET: Recipes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes
                .Include(r => r.Day)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        // POST: Recipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe != null)
            {
                _context.Recipes.Remove(recipe);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecipeExists(int id)
        {
            return _context.Recipes.Any(e => e.Id == id);
        }
    }
}
