namespace RecipeHelperApp.Responses
{
    public class CombinedRecipeResult
    {
        public int id { get; set; }
        public string? image { get; set; }
        public string? title { get; set; }
        public string? description { get; set; }
        public int preparationMinutes { get; set; }
        public int cookingMinutes { get; set; }
        public int servings { get; set; }
        public Nutrition nutrition { get; set; }
        public string? instructions { get; set; }
    }
    }

