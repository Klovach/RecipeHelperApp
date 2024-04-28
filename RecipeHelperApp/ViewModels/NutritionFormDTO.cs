using RecipeHelperApp.CustomAttributes;
using RecipeHelperApp.Data;
using RecipeHelperApp.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RecipeHelperApp.ViewModels
{
    public class NutritionFormDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        [DisplayName("Include Nutrition")]
        public bool IncludeNutrition { get; set; }

        [DisplayName("Include Servings")]
        public bool IncludeServings { get; set; }

        [DisplayName("Include Ingredients")]
        public bool IncludeIngredients { get; set; }

        [DisplayName("Exclude Ingredients")]
        public bool ExcludeIngredients { get; set; }
        public int Servings { get; set;  }
        public string? IncludedIngredients { get; set; }
        public string? ExcludedIngredients { get; set; }
        public Nutrients Nutrients { get; set; }
        public string Sex {  get; set; }
        public int Age { get; set; }
        public double Height {  get; set; }

        [Required]
        [Range(65, 1000, ErrorMessage = "Weight must be between 65 and 1000 lbs.")]
        public double Weight { get; set; }

        [Required]
        [Display(Name = "Pounds Per Week")]
        public double PoundsPerWeek { get; set; }


        [Required]
        [Display(Name = "Activity Level")]
        public string ActivityLevel { get; set; }

        [Required]
        [Display(Name = "Fitness Goal")]
        public string FitnessGoal { get; set; }


    }
}
