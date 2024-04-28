using RecipeHelperApp.CompletionModels;

namespace RecipeHelperApp.Interfaces
{
    public interface INutritionService
    {
        Task<NutritionResult> GenerateNutritionalFacts(string query, int servings);

    }
}
