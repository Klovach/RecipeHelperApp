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

        [DisplayName("Include Ingredients")]
        public bool IncludeIngredients { get; set; }

        [DisplayName("Exclude Ingredients")]
        public bool ExcludeIngredients { get; set; }
        public string? IncludedIngredients { get; set; }
        public string? ExcludedIngredients { get; set; }
        public Nutrients Nutrients { get; set; }

        [Required]
        [Range(65, 1000, ErrorMessage = "Weight must be between 65 and 1000 lbs.")]
        public double Weight { get; set; }

        [Required]
        [Display(Name = "Target Weight")]
        public double TargetWeight { get; set; }

        [Required]
        [Display(Name = "Target Weight Date")]
        [ValidateDates]
        [ValidateTargetDate(ErrorMessage = "The provided target date is too close to be safe. Pick another date or re-avaluate how much weight you want to gain or lose.")]
        public DateTime TargetWeightDate { get; set; }

        [Required]
        [Display(Name = "Activity Level")]
        public string ActivityLevel { get; set; }

        [Required]
        [Display(Name = "Fitness Goal")]
        [ValidateFitnessGoal]
        public string FitnessGoal { get; set; }


    }
}
