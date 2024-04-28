using RecipeHelperApp.Data.Migrations;
using RecipeHelperApp.Services;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.RegularExpressions;

namespace RecipeHelperApp.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public int DayId { get; set; }
        public Day Day { get; set; }
        public string? Image { get; set; }
        public string MealType { get; set; }

        // Name
        public string? Name { get; set; }

        // Description
        public string? Description { get; set; }
        // Instructions
        public string? Instructions { get; set; }

        //Ingredients
        public string? Ingredients { get; set; }
        
        // Servings 
        public int Servings { get; set; }

        public double ServingSize { get; set; }
        // Nutritional Facts
        public double Calories { get; set; }
        public double Protein { get; set; }
        public double Fat { get; set; }
        public double Carbohydrates { get; set; }
        public double Sodium { get; set; }
        public double Potassium { get; set; }
        public double Cholesterol { get; set; }
        public double Fiber { get; set; }
        public double Sugar { get; set; }

        public Recipe(string MealType)
        {
            this.MealType = MealType;
            Name = ""; 
            Description = "";
            Instructions = "";
            Ingredients = "";
        }

        public Recipe()
        {

        }

        public string GetNutrientAsString(string nutrient, double count)
        {
            string value;

            if (nutrient.Equals("Sodium") || nutrient.Equals("Potassium") || nutrient.Equals("Cholesterol"))
            {
                if (count <= 0)
                    value = count.ToString("0") + "mg"; 
                else
                    value = count.ToString("#.#") + "mg";
            }
            else
            {
                if (count <= 0)
                    value = count.ToString("0") + "g";
                else
                    value = count.ToString("#.#") + "g";
            }

            return value;
        }


        public void ResetValues()
        {
            Image = null;
            Name = "";
            Description = "";
            Instructions = "";
            Ingredients = "";
            Calories = 0;
            Protein = 0;
            Fat = 0;
            Carbohydrates = 0;

        }

      
    }
}
