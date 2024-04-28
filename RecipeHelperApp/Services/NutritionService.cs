using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OpenAI_API;
using RecipeHelperApp.CompletionModels;
using RecipeHelperApp.Interfaces;
using RecipeHelperApp.Settings;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace RecipeHelperApp.Services
{
    public class NutritionService : INutritionService
    {
        //  https://json2csharp.com/
        private readonly string _nutritionAPIKey; 
        public NutritionService(IOptions<NutritionServiceSettings> optionsAccessor)
        {
            Options = optionsAccessor.Value;
            _nutritionAPIKey = Options.NutritionAPIKey;
        }

        public NutritionServiceSettings Options { get; }


        /*
        public double Round (double value, int precision)
        {
            int scale = (int)Math.Pow(10, precision);
            return (double)Math.Round(value * scale) / scale;
        } */

        public async Task<NutritionResult> GenerateNutritionalFacts(string query, int servings)
        {
            string apiUrl = "https://api.calorieninjas.com/v1/nutrition?query=";

            var result = new NutritionResult();

            // Split the original string by newlines so that we get one of each item. 
            string[] items = query.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            // Remove the leading hyphen and space from each item using trim. 
            for (int i = 0; i < items.Length; i++)
            {
                items[i] = items[i].TrimStart('-', ' ');
            }

            // Join the modified items into a single string separated by commas to ensure this query works. 
            query = string.Join(", ", items);

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-Api-Key", _nutritionAPIKey);

                HttpResponseMessage response = await client.GetAsync(apiUrl + query);

                // Getting a list of nutrients and adding all of their values. 
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(json);
                    NutritionData nutritionData = JsonConvert.DeserializeObject<NutritionData>(json);
                    List<NutritionResult> itemList = nutritionData.items;

                    foreach (var nutritionInfo in itemList)
                    {
                        // Check if servings greater than 1:
                        double numServings = servings;
                        if (servings > 1)
                        {
                            // Divide by the number of servings:
                            double factor = 1.0 / numServings;
                            result.calories += Math.Round(nutritionInfo.calories * factor, 1);
                            result.serving_size_g += Math.Round(nutritionInfo.serving_size_g * factor, 1);
                            result.fat_total_g += Math.Round(nutritionInfo.fat_total_g * factor, 1);
                            result.fat_saturated_g += Math.Round(nutritionInfo.fat_saturated_g * factor, 1);
                            result.protein_g += Math.Round(nutritionInfo.protein_g * factor, 1);
                            result.sodium_mg += Math.Round(nutritionInfo.sodium_mg * factor, 1);
                            result.potassium_mg += Math.Round(nutritionInfo.potassium_mg * factor, 1);
                            result.cholesterol_mg += Math.Round(nutritionInfo.cholesterol_mg * factor, 1);
                            result.carbohydrates_total_g += Math.Round(nutritionInfo.carbohydrates_total_g * factor, 1);
                            result.fiber_g += Math.Round(nutritionInfo.fiber_g * factor, 1);
                            result.sugar_g += Math.Round(nutritionInfo.sugar_g * factor, 1);
                        }
                        else
                        {
                            // If servings is 1 or less, the values must be added as they are: 
                            result.calories += Math.Round(nutritionInfo.calories, 1);
                            result.serving_size_g += Math.Round(nutritionInfo.serving_size_g, 1);
                            result.fat_total_g += Math.Round(nutritionInfo.fat_total_g, 1);
                            result.fat_saturated_g += Math.Round(nutritionInfo.fat_saturated_g, 1);
                            result.protein_g += Math.Round(nutritionInfo.protein_g, 1);
                            result.sodium_mg += Math.Round(nutritionInfo.sodium_mg, 1);
                            result.potassium_mg += Math.Round(nutritionInfo.potassium_mg, 1);
                            result.cholesterol_mg += Math.Round(nutritionInfo.cholesterol_mg, 1);
                            result.carbohydrates_total_g += Math.Round(nutritionInfo.carbohydrates_total_g, 1);
                            result.fiber_g += Math.Round(nutritionInfo.fiber_g, 1);
                            result.sugar_g += Math.Round(nutritionInfo.sugar_g, 1);
                        }
                    }
                    return result;
                }

                Console.WriteLine($"Error: {response.StatusCode} {await response.Content.ReadAsStringAsync()}");

                // If something went wrong, return null. 
                return null;
            }
       
        }
    }
}
