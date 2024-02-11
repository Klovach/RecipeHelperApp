using Microsoft.AspNetCore.Mvc;

namespace RecipeHelperApp.Responses
{
 

    public class GetRecipeByIdResult200
    {
        public GetRecipeByIdResult200()
        {
        }

        // JSON to C# Class Converter: https://json2csharp.com/
        // RecipeResultModel : use the JSON to C# class converter to convert a JSON from a GET result
        // and alter this page to change the recipe model retrieved. If in the future the API should need to be
        // changed for another API, use this converter. 

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

    public class Nutrition
    {
        public List<Nutrient>? nutrients { get; set; }
        public CaloricBreakdown? caloricBreakdown { get; set; }
        public WeightPerServing? weightPerServing { get; set; }
    }

    public class Nutrient
    {
        public string? name { get; set; }
        public double amount { get; set; }
        public string? unit { get; set; }
        public double percentOfDailyNeeds { get; set; }
    }

    public class CaloricBreakdown
    {
        public double percentProtein { get; set; }
        public double percentFat { get; set; }
        public double percentCarbs { get; set; }
    }

    public class WeightPerServing
    {
        public int amount { get; set; }
        public string? unit { get; set; }
    }

}

