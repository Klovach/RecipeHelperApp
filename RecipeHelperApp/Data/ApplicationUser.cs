﻿using Microsoft.AspNetCore.Identity;
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
        public double Height { get; set; }
        public double Weight { get; set; }
        public double TargetWeight { get; set; }
        public DateTime TargetWeightDate { get; set; }
        public required string ActivityLevel { get; set; }
        public required string FitnessGoal { get; set; }
        public ICollection<Week> Weeks { get; set; }

        public NutritionForm NutritionForm { get; set;}

    }
}
