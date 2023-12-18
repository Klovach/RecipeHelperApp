using Microsoft.AspNetCore.Identity;
using Microsoft.SqlServer.Server;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Intrinsics.X86;

namespace RecipeHelperApp.Data
{
    public class ApplicationUser : IdentityUser
    {
        public DateOnly BirthDate { get; set; }
        public string Sex { get; set; }

        [RegularExpression(@"^(4'0""|[4-7]'(?:1[0-2]|[0-9])""|8'0"")$")]
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public decimal TargetWeight { get; set; }
        public DateOnly TargetWeightDate { get; set; }

        [Required(ErrorMessage = "Please select an activity level.")]
        [Display(Name = "Activity Level")]
        public string ActivityLevel { get; set; }
        public string FitnessGoal { get; set; }

    }
}
