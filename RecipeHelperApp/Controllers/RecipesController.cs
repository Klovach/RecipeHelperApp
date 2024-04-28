using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using RecipeHelperApp.Data;
using RecipeHelperApp.Interfaces;
using RecipeHelperApp.Models;
using RecipeHelperApp.ViewModels;

namespace RecipeHelperApp.Controllers
{
    /// <summary>
    /// Controller for managing recipes in the app.
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

            // Include days in the search results
            recipes = recipes.Include(r => r.Day);

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
                          where r.Day.Week.User.Id.Equals(userId) &&
                                !string.IsNullOrEmpty(r.Name) &&
                                !string.IsNullOrEmpty(r.Description)
                          select r;

            if (!string.IsNullOrEmpty(mealType))
            {
                recipes = recipes.Where(r => r.MealType.Contains(mealType));
            }

            // Include days in the search results
            recipes = recipes.Include(r => r.Day);

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
        public async Task<IActionResult> Create([Bind("Id,DayId,Image,MealType,Name,Description,Instructions,Ingredients,Calories,Protein,Fat,Carbohydrates")] RecipeDTO recipeDTO)
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
                Carbohydrates = recipeDTO.Carbohydrates
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
            // Convert Recipe to RecipeDTO
            var recipeDTO = new RecipeDTO
            {
                Id = recipe.Id,
                DayId = recipe.DayId,
                DeleteFlag = false,
                Image = recipe.Image,
                MealType = recipe.MealType,
                Name = recipe.Name,
                Servings = recipe.Servings,
                Description = recipe.Description,
                Instructions = recipe.Instructions,
                Ingredients = recipe.Ingredients,
                Calories = recipe.Calories,
                Protein = recipe.Protein,
                Fat = recipe.Fat,
                Carbohydrates = recipe.Carbohydrates,
                ServingSize = recipe.ServingSize,
                Sodium = recipe.Sodium,
                Potassium = recipe.Potassium,
                Cholesterol = recipe.Cholesterol,
                Fiber = recipe.Fiber,
                Sugar = recipe.Sugar
            };

            ViewData["DayId"] = new SelectList(_context.Days, "Id", "Id", recipe.DayId);
            return View(recipeDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Generate(int id, [Bind("Id, DayId, DeleteFlag, Image, ImageFile, MealType, Name, Description, Instructions, Ingredients, Calories, Protein, Fat, Carbohydrates, Servings, ServingSize, Sodium, Potassium, Cholesterol, Fiber, Sugar")] RecipeDTO recipeDTO)
        {
            var recipe = new Recipe
            {
                Id = recipeDTO.Id,
                DayId = recipeDTO.DayId,
                MealType = recipeDTO.MealType,
                Image = recipeDTO.Image,
                Name = recipeDTO.Name,
                Servings = recipeDTO.Servings,
                ServingSize = recipeDTO.ServingSize,
                Description = recipeDTO.Description,
                Instructions = recipeDTO.Instructions,
                Ingredients = recipeDTO.Ingredients,
                Calories = recipeDTO.Calories,
                Protein = recipeDTO.Protein,
                Fat = recipeDTO.Fat,
                Carbohydrates = recipeDTO.Carbohydrates,
                Sodium = recipeDTO.Sodium,
                Potassium = recipeDTO.Potassium,
                Cholesterol = recipeDTO.Cholesterol,
                Fiber = recipeDTO.Fiber,
                Sugar = recipeDTO.Sugar
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
                    if (genRecipe != null)
                    {
                        _context.Entry(genRecipe).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                    }

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

            return View(recipeDTO);
        }


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
                DeleteFlag = false, 
                Image = recipe.Image,
                MealType = recipe.MealType,
                Name = recipe.Name,
                Servings = recipe.Servings, 
                Description = recipe.Description,
                Instructions = recipe.Instructions,
                Ingredients = recipe.Ingredients,
                Calories = recipe.Calories,
                Protein = recipe.Protein,
                Fat = recipe.Fat,
                Carbohydrates = recipe.Carbohydrates,
                ServingSize = recipe.ServingSize,
                Sodium = recipe.Sodium,
                Potassium = recipe.Potassium,
                Cholesterol = recipe.Cholesterol,
                Fiber = recipe.Fiber,
                Sugar = recipe.Sugar
            };

            ViewData["DayId"] = new SelectList(_context.Days, "Id", "Id", recipe.DayId);
            return View(recipeDTO);
        }


        // POST: Recipes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, DayId, DeleteFlag, Image, ImageFile, MealType, Name, Description, Instructions, Ingredients, Calories, Protein, Fat, Carbohydrates, Servings, ServingSize, Sodium, Potassium, Cholesterol, Fiber, Sugar")] RecipeDTO recipeDTO)
        {
            var recipe = new Recipe
            {
                Id = recipeDTO.Id,
                DayId = recipeDTO.DayId,
                MealType = recipeDTO.MealType,
                Image = recipeDTO.Image,
                Name = recipeDTO.Name,
                Servings = recipeDTO.Servings,
                ServingSize = recipeDTO.ServingSize,
                Description = recipeDTO.Description,
                Instructions = recipeDTO.Instructions,
                Ingredients = recipeDTO.Ingredients,
                Calories = recipeDTO.Calories,
                Protein = recipeDTO.Protein,
                Fat = recipeDTO.Fat,
                Carbohydrates = recipeDTO.Carbohydrates,
                Sodium = recipeDTO.Sodium,
                Potassium = recipeDTO.Potassium,
                Cholesterol = recipeDTO.Cholesterol,
                Fiber = recipeDTO.Fiber,
                Sugar = recipeDTO.Sugar
            };

            Console.WriteLine("Delete flag was: " + recipeDTO.DeleteFlag);
            Console.WriteLine("Imagefile was: " + recipeDTO.ImageFile);
            Console.WriteLine("Image was: " + recipe.Image);
            if ((recipeDTO.ImageFile == null || recipeDTO.ImageFile.Length == 0) && recipe.Image != null && recipeDTO.DeleteFlag == true)
            {
                Console.WriteLine("Emtered delete");
                _ = _photoService.DeletePhotoAsync(recipe.Image);    
                    recipe.Image = null;
             
            }

            if (recipeDTO.ImageFile != null)
            {
                Console.WriteLine("Image file was not null");
                var photoResult = await _photoService.AddPhotoAsync(recipeDTO.ImageFile);

                if (photoResult.Error != null)
                {
                    ModelState.AddModelError("Image", "Photo upload failed: " + photoResult.Error.Message);
                    return View(recipeDTO);
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
                DeleteFlag = false,
                Image = recipe.Image,
                MealType = recipe.MealType,
                Name = recipe.Name,
                Servings = recipe.Servings,
                Description = recipe.Description,
                Instructions = recipe.Instructions,
                Ingredients = recipe.Ingredients,
                Calories = recipe.Calories,
                Protein = recipe.Protein,
                Fat = recipe.Fat,
                Carbohydrates = recipe.Carbohydrates,
                ServingSize = recipe.ServingSize,
                Sodium = recipe.Sodium,
                Potassium = recipe.Potassium,
                Cholesterol = recipe.Cholesterol,
                Fiber = recipe.Fiber,
                Sugar = recipe.Sugar
            };

            ViewData["DayId"] = new SelectList(_context.Days, "Id", "Id", recipe.DayId);
            return View(recipeDTO);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reset(int id, [Bind("Id, DayId, MealType,Name,Description,Instructions,Ingredients,Calories,Protein,Fat,Carbohydrates")] RecipeDTO recipeDTO)
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
                Carbohydrates = 0
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
