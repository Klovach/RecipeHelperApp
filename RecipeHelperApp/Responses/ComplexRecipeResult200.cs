using Microsoft.DotNet.MSIdentity.Shared;
using Newtonsoft.Json;

namespace RecipeHelperApp.Responses
{
    public class ComplexRecipeResult200
    {
        public List<Result> results { get; set; }
        public int offset { get; set; }
        public int number { get; set; }
        public int totalResults { get; set; }

        public class AnalyzedInstruction
        {
            public string name { get; set; }
            public List<Step> steps { get; set; }
        }

        public class Equipment
        {
            public int id { get; set; }
            public string name { get; set; }
            public string localizedName { get; set; }
            public string image { get; set; }
        }

        public class Ingredient
        {
            public int id { get; set; }
            public string name { get; set; }
            public string localizedName { get; set; }
            public string image { get; set; }
        }

        public class Length
        {
            public int number { get; set; }
            public string unit { get; set; }
        }

        public class Nutrient
        {
            public string name { get; set; }
            public double amount { get; set; }
            public string unit { get; set; }
        }

        public class Nutrition
        {
            public List<Nutrient> nutrients { get; set; }
        }

        public class Result
        {
            public bool vegetarian { get; set; }
            public bool vegan { get; set; }
            public bool glutenFree { get; set; }
            public bool dairyFree { get; set; }
            public bool veryHealthy { get; set; }
            public bool cheap { get; set; }
            public bool veryPopular { get; set; }
            public bool sustainable { get; set; }
            public bool lowFodmap { get; set; }
            public int weightWatcherSmartPoints { get; set; }
            public string gaps { get; set; }
            public int preparationMinutes { get; set; }
            public int cookingMinutes { get; set; }
            public int aggregateLikes { get; set; }
            public int healthScore { get; set; }
            public string creditsText { get; set; }
            public string sourceName { get; set; }
            public double pricePerServing { get; set; }
            public int id { get; set; }
            public string title { get; set; }
            public int readyInMinutes { get; set; }
            public int servings { get; set; }
            public string sourceUrl { get; set; }
            public string image { get; set; }
            public string imageType { get; set; }
            public string summary { get; set; }
            public List<object> cuisines { get; set; }
            public List<string> dishTypes { get; set; }
            public List<string> diets { get; set; }
            public List<object> occasions { get; set; }
            public List<AnalyzedInstruction> analyzedInstructions { get; set; }
            public double spoonacularScore { get; set; }
            public string spoonacularSourceUrl { get; set; }
            public Nutrition nutrition { get; set; }
        }

        public class Step
        {
            public int number { get; set; }
            public string step { get; set; }
            public List<Ingredient> ingredients { get; set; }
            public List<Equipment> equipment { get; set; }
            public Length length { get; set; }
        }
    }
}
