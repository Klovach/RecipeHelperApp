using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeHelperApp.Models
{
    public class Recipe
    {
        public int Id { get; set; }

        [ForeignKey("DayId")]
        public virtual Day Day { get; set; }
        public string? MealType { get; set; }

        // Name
        public string? Name { get; set; }

        // Description
        public string? Description {  get; set; }
        // Instructions
        public string? Instructions { get; set; }

        //Ingredients
        public string? Ingredients {  get; set; }

        // Nutritional Facts
        public double Calories { get; set; }
        public double Protein { get; set; }
        public double Fat { get; set; }
        public double Carbs { get; set; }

        public Recipe(string MealType)
        {
            this.MealType = MealType;
        }

        public Recipe()
        {
        }


    }
}
