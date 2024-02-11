using Microsoft.AspNetCore.Identity;
using Microsoft.SqlServer.Server;
using RecipeHelperApp.CustomAttributes;
using RecipeHelperApp.Models;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Intrinsics.X86;

namespace RecipeHelperApp.Data
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime BirthDate { get; set; }
        public string Sex { get; set; }

        [RegularExpression(@"^(4'0""|[4-7]'(?:1[0-2]|[0-9])""|8'0"")$")]
        public decimal Height { get; set; }
        public decimal Weight { get; set; }

        [ValidateTargetWeight(ErrorMessage="Your target weight doesn't match")]
        public decimal TargetWeight { get; set; }
        public DateTime TargetWeightDate { get; set; }

        [Required(ErrorMessage = "Please select an activity level.")]
        [Display(Name = "Activity Level")]
        public string ActivityLevel { get; set; }
        public string FitnessGoal { get; set; }
        public virtual List<Week> WeekModel { get; set; }

        public virtual NutritionForm NutritionFormModel { get; set;}

    }
}
