using Microsoft.DotNet.Scaffolding.Shared.Project;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RecipeHelperApp.Data;
using RecipeHelperApp.Services;
using System.ComponentModel.DataAnnotations.Schema;


namespace RecipeHelperApp.Models
{
    public class NutritionForm
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public bool IncludeServings {  get; set; }
        public bool IncludeNutrition { get; set; }
        public bool IncludeIngredients { get; set; }
        public bool ExcludeIngredients { get; set; }
        public int Servings { get; set; }
        public string? IncludedIngredients { get; set; }
        public string? ExcludedIngredients { get; set; }
        public string NutrientsJson
        {
            get => JsonConvert.SerializeObject(Nutrients);
            set => Nutrients = JsonConvert.DeserializeObject<Nutrients>(value);
        }

        public Nutrients Nutrients { get; set; }

        NutritionalFormService nutritionalFormService = new NutritionalFormService();


        public NutritionForm()
        {
           
        }


        public NutritionForm(ApplicationUser user)
        {
            CalculateNewValues(user);
        }

        public void CalculateNewValues(ApplicationUser user)
        {
            Nutrients newNutrients = nutritionalFormService.CalculateNeeds(user);
            Nutrients = newNutrients;
        }


        public void ResetValues()
        {
            IncludeNutrition = true;
            IncludeIngredients = false;
            ExcludeIngredients = false;
            IncludedIngredients = null;
            ExcludedIngredients = null;
            Nutrients.Carbohydrates = 0;
            Nutrients.Protein = 0;
            Nutrients.Calories = 0;
            Nutrients.Fat = 0;
        }

    }
}