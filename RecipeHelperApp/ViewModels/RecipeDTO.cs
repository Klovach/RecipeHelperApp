using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecipeHelperApp.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace RecipeHelperApp.ViewModels
{
    public class RecipeDTO
    {
        public int Id { get; set; }
        public int DayId { get; set; }
        public IFormFile? ImageFile { get; set; }
        public bool DeleteFlag { get; set; }

        public string? Image { get; set; }
        public string MealType { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Instructions { get; set; }
        public string? Ingredients { get; set; }

        // Nutritional Facts
        public int Servings { get; set; }
        public double Calories { get; set; }
        public double Protein { get; set; }
        public double Fat { get; set; }
        public double Carbohydrates { get; set; }

        public double ServingSize { get; set; }
        public double Sodium { get; set; }
        public double Potassium { get; set; }
        public double Cholesterol { get; set; }
        public double Fiber { get; set; }
        public double Sugar { get; set; }

    }
}
