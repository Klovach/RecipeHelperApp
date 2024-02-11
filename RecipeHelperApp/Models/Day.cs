using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeHelperApp.Models
{
    public class Day
    {
        public int Id { get; set; }

        [ForeignKey("WeekId")]
        public virtual Week Week { get; set; }
        public string? WeekDay { get; set; }
        public double TotalCalories { get; set; }
        public double TotalProtein { get; set; }
        public double TotalFat { get; set; }
        public double TotalCarbs { get; set; }
        public List<Recipe> Recipes { get; set; }

        // Defaut Constructor
        public Day()
        {
         Recipes = new List<Recipe>();
         //  InitializeRecipes();
        }

        public Day(string weekDay, int totalCalories, double totalProtein, double totalFat, int totalCarbs)
        {
            this.WeekDay = weekDay; ;
            this.TotalCalories = totalCalories;
            this.TotalProtein = totalProtein;
            this.TotalFat = totalFat;
            this.TotalCarbs = totalCarbs;
            InitializeRecipes();
        }

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

        private void InitializeRecipes()
        {
            Recipes = new List<Recipe>();
            string[] recipeList = { "Breakfast", "Lunch", "Dinner", "Snack" };

            foreach (var meal in recipeList)
            {
                Recipe recipeTemplate = new Recipe(meal);
                Recipes.Add(recipeTemplate);
            }
        }
    }
}
    
