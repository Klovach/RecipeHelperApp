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

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
        public bool IncludeNutrition { get; set; }
        public bool IncludeIngredients { get; set; }
        public bool ExcludeIngredients { get; set; }
        public string? IncludedIngredients { get; set; }
        public string? ExcludedIngredients { get; set; }
        public string? NutrientsJson
        {
            get => JsonConvert.SerializeObject(Nutrients);
            set => Nutrients = JsonConvert.DeserializeObject<Nutrients>(value);
        }

        public Nutrients Nutrients { get; set; }

        NutritionalFormService nutritionalFormService = new NutritionalFormService();

        public NutritionForm()
        {
        }

        public NutritionForm(DateTime birthDate, string sex, decimal weight, decimal height, decimal targetWeight, DateTime targetWeightDate, string activityLevel, string fitnessGoal)
        {
            Nutrients newNutrients = nutritionalFormService.calculateNeeds(birthDate, sex, weight, height, targetWeight, targetWeightDate, activityLevel, fitnessGoal);
            UpdateNutritionForm(newNutrients);
        }

        private void UpdateNutritionForm(Nutrients newNutrients)
        {
            Nutrients = newNutrients;
        }
    }
}