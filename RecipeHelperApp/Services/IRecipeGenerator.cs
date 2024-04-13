using RecipeHelperApp.Models;

namespace RecipeHelperApp.Services
{
    public interface IRecipeGenerator
    {
        string CompilePrompt(NutritionForm nutritionForm);

        Task<string> GenerateImage(string prompt);

        Task<Recipe> GenerateRecipeAsync(NutritionForm nutritionForm, Recipe recipe);

        void ParseGeneratedRecipe(Recipe recipe, string generatedRecipe);

        Task<Recipe> GenerateRecipe(NutritionForm nutritionForm, Recipe recipe);
        public Task GenerateRecipes(NutritionForm nutritionForm, Day day);

        public void ResetRecipes(Day day);
        void ResetValues(Recipe recipe);

    }

}
