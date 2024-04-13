using RecipeHelperApp.Services;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeHelperApp.Models
{
    // This class will serve as the example class for explaining this application. One-to-many relationships in ASP.NET can be complicated.
    // Using an ORM such as EntityFramework can further complicate things if one doesn't understand what is going on behind the scenes.
    // ASP.NET makes use of Navigation Properties and foreign keys. The distinction is important.

    // A navigation property is an optional property on an entity type that allows for navigation from one end of an association to the other end.
    // Unlike other properties, navigation properties DO NOT carry data.

    // It is advisable to initialize a foreign key, which should possess a name that is similar to the navigation property. 
    public class Day
    {
        public int Id { get; set; }
        public int WeekId { get; set; }
        public Week Week { get; set; }
        public string? WeekDay { get; set; }
        public double TotalCalories { get; set; }
        public double TotalProtein { get; set; }
        public double TotalFat { get; set; }
        public double TotalCarbs { get; set; }
        public List<Recipe> Recipes { get; set; }

        // It is wise to avoid using too much business logic in a model class. However, for demonstrative purposes and for time's sake,
        // such logic is included here to carry out operations. Each time a day is created, a series of recipes are initialized by calling
        // a method in its constructor.
        // The same logic is utilized in the Week model class to create a series of days corresponding to the week. Is this efficient?
        // I still question if there isn't a better way to achieve this, but it will suffice for now. 

        public Day()
        {
            Recipes = new List<Recipe>();
            InitializeRecipes();
        }

        public Day(string weekDay, int totalCalories, double totalProtein, double totalFat, int totalCarbs)
        {
            Recipes = new List<Recipe>();
            this.WeekDay = weekDay; ;
            this.TotalCalories = totalCalories;
            this.TotalProtein = totalProtein;
            this.TotalFat = totalFat;
            this.TotalCarbs = totalCarbs;
            InitializeRecipes();
        }

        // Here we calculate recipe totals using a foreach loop to iterate through each recipe object. This method
        // is called anytime we must return to the index. These values will be utilized for the pie chart.
        public void CalculateRecipeTotals()
        {
            TotalCalories = 0;
            TotalProtein = 0;
            TotalFat = 0;
            TotalCarbs = 0;

            foreach (var recipe in Recipes)
            {
                TotalCalories += recipe.Calories;
                TotalProtein += recipe.Protein;
                TotalFat += recipe.Fat;
                TotalCarbs += recipe.Carbs;
            }
        }

        // Initialize a series of recipes. For each meal type name in the array ...
        private void InitializeRecipes()
        {
            string[] recipeList = { "Breakfast", "Lunch", "Dinner", "Snack" };

            foreach (var meal in recipeList)
            {
                Recipe recipeTemplate = new Recipe(meal);
                Recipes.Add(recipeTemplate);
            }
        }

        // To alter existing elements in the list, it is wise here to remove them and then add 
        // the newly generated recipes. 

        /* public async Task GenerateRecipes(NutritionForm nutritionForm)
        {
            string[] recipeList = { "Breakfast", "Lunch", "Dinner", "Snack" };

            // Clear existing recipes.
            Recipes.Clear();

            // Initialize and add new recipes.
            foreach (var meal in recipeList)
            {
                Recipe recipeTemplate = new Recipe(meal);
                await recipeTemplate.GenerateRecipe(nutritionForm);   
                Recipes.Add(recipeTemplate);
            }
        }


        public void ResetRecipes()
        {
            Console.WriteLine("Called reset Recipes");
            string[] recipeList = { "Breakfast", "Lunch", "Dinner", "Snack" };

            Recipes.Clear();

            // Initialize and add new recipes
            foreach (var meal in recipeList)
            {
                Recipe recipeTemplate = new Recipe(meal);
                Recipes.Add(recipeTemplate);
            }
        } 
    } */
    }
}
    