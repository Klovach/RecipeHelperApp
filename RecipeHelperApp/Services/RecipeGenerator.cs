using RecipeHelperApp.Models;
using RecipeHelperApp.Services;
using System.Text.RegularExpressions;
using System.Text;

namespace RecipeHelperApp.Services
{
    using Microsoft.AspNetCore.Http.HttpResults;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using NuGet.ContentModel;
    using OpenAI_API;
    using OpenAI_API.Chat;
    using OpenAI_API.Images;
    using RecipeHelperApp.Models;
    using System.Net.Http.Headers;
    using System.Security.Claims;
    using System.Text;

    public class RecipeGenerator : IRecipeGenerator
    {
        private readonly APIAuthentication _apiAuthentication;
        private readonly OpenAIAPI _openAiApi;
        private readonly IPhotoService _photoService;
        public RecipeGenerator(IOptions<OpenAISettings> optionsAccessor, IPhotoService photoService)
        {
            Options = optionsAccessor.Value;
            _apiAuthentication = new APIAuthentication(Options.OpenAIKey);
            _openAiApi = new OpenAIAPI(_apiAuthentication);
            _photoService = photoService;
        }
        OpenAISettings Options { get; }

        public string CompilePrompt(NutritionForm nutritionForm)
        {
            string prompt = "Generate a recipe in the following format:" +
                "\nName: " +
                "\nDescription: " +
                "\nInstructions: " +
                "\nIngredients: " +
                "\nCalories: " +
                "\nFat: " +
                "\nProtein: " +
                "\nCarbohydrates: ";


            if (nutritionForm.IncludeIngredients)
            {
                prompt += $"You can include these ingredients from the user's pantry Ingredients:{nutritionForm.IncludedIngredients}";

            }
            if (nutritionForm.ExcludeIngredients)
            {
                prompt += $" and exclude these ingredients ExcludedIngredients:{nutritionForm.ExcludedIngredients}";
            }
            if (nutritionForm.IncludeNutrition)
            {
                prompt += $"\nThe user's recipe needs at least {nutritionForm.Nutrients.Calories / 4} calories, {nutritionForm.Nutrients.Protein} grams of protein: ," +
                    $"{nutritionForm.Nutrients.Fat} grams of fat, and {nutritionForm.Nutrients.Carbs} grams of carbohydrates.";
                //  $"\nCalories: {nutritionForm.Nutrients.Calories / 4} ";
                //   $"\nProtein: {nutritionForm.Nutrients.Protein}, " +
                //  $"\nFat: {nutritionForm.Nutrients.Fat}, " +
                //   $"\nCarbohydrates: {nutritionForm.Nutrients.Carbs}.";
            }

            Console.WriteLine(prompt);

            return prompt;

        }


        // LINK:
        // CITE

        [HttpPost]
        public async Task<string> GenerateImage(string prompt)
        {
            string apiUrl = "https://api.openai.com/v1/images/generations";

            var data = new
            {
                prompt = prompt,
                size = "1024x1024",
                model = "dall-e-3"
            };

            // Serialize the data object to JSON
            var jsonData = JsonConvert.SerializeObject(data);


            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Options.OpenAIKey);

                var Message = await client.PostAsync(apiUrl, new StringContent(jsonData, null, "application/json"));

                if (Message.IsSuccessStatusCode)
                {
                    var content = await Message.Content.ReadAsStringAsync();
                    dynamic response = JObject.Parse(content);
                    var resultUrl = response.data[0].url;
                    Console.WriteLine(resultUrl);
                    return resultUrl;
                }
                else
                {
                    if (Message.StatusCode == System.Net.HttpStatusCode.BadRequest || Message.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        if (Message.Content.Headers.ContentLength > 0 && Message.Content.Headers.ContentType?.ToString() == "application/json")
                        {
                            jsonData = await Message.Content.ReadAsStringAsync();
                            dynamic error = JObject.Parse(jsonData);
                            Console.WriteLine(jsonData);
                            string? msg = error.error?.message;
                            Console.WriteLine(msg);
                            return null;
                        }
                    }
                }
            }

            // Default return statement
            return null;
        }



        public async Task<Recipe> GenerateRecipeAsync(NutritionForm nutritionForm, Recipe recipe)
        {
            Console.WriteLine("Entered generateRecipeasync");
            Console.WriteLine("Recipe is: " + recipe);
            //  apiAuthentication = new APIAuthentication(openAiApiKey);
            //  openAiApi = new OpenAIAPI(apiAuthentication);

            string prompt = CompilePrompt(nutritionForm);

            try
            {
                string model = "gpt-3.5-turbo";
                int maxTokens = 1000;

                var completionRequest = new ChatRequest
                {
                    Model = model,
                    Messages = new List<ChatMessage> { new ChatMessage(ChatMessageRole.User, prompt) },
                    MaxTokens = maxTokens
                };

                var completionResult = await _openAiApi.Chat.CreateChatCompletionAsync(completionRequest);

                // Extract generated recipe from the completion result
                string generatedRecipe = completionResult.ToString();

                if (generatedRecipe != null)
                {
                    recipe.ParseGeneratedRecipe(generatedRecipe);
                    if (recipe != null)
                    {
                        string recipeImgUrl = await GenerateImage(recipe.Name);
                        var uploadResult = await _photoService.DownloadImageFromUrlAsync(recipeImgUrl);
                        recipe.Image = uploadResult.Url.ToString();
                        return recipe;
                    }
                    else
                    {
                        Console.WriteLine("Generated recipe came back null");
                        return null; // Return null in this case
                    }
                }
                else
                {
                    Console.WriteLine("Error: Generated recipe text is null.");
                    return null; // Return null when generatedRecipe is null
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");

                throw; // Rethrow the exception or handle it as necessary
            }
        }



        public async Task<Recipe> GenerateRecipe(NutritionForm nutritionForm, Recipe recipe)
        {
            Recipe generatedRecipe = new Recipe();
            generatedRecipe.Id = recipe.Id;
            generatedRecipe.DayId = recipe.DayId;
            generatedRecipe.MealType = recipe.MealType;

            if (generatedRecipe != null)
            {
                // The recipe was generated successfully, do something with it
                Console.WriteLine("Generated recipe:");
                Console.WriteLine($"Id: {generatedRecipe.Id}");
                Console.WriteLine($"Day: {generatedRecipe.Day}");
                Console.WriteLine($"DayId: {generatedRecipe.DayId}");
                Console.WriteLine($"MealType: {generatedRecipe.MealType}");
                Console.WriteLine($"Name: {generatedRecipe.Name}");
                Console.WriteLine($"Description: {generatedRecipe.Description}");
                Console.WriteLine($"Instructions: {generatedRecipe.Instructions}");
                Console.WriteLine($"Ingredients: {generatedRecipe.Ingredients}");
                Console.WriteLine($"Calories: {generatedRecipe.Calories}");
                Console.WriteLine($"Protein: {generatedRecipe.Protein}");
                Console.WriteLine($"Fat: {generatedRecipe.Fat}");
                Console.WriteLine($"Carbs: {generatedRecipe.Carbs}");
            }
            else
            {
                // Handle the case where the recipe could not be generated
                Console.WriteLine("Error: Failed to generate recipe.");
            }

            return await GenerateRecipeAsync(nutritionForm, generatedRecipe);
        }


        public void ResetValues(Recipe recipe)
        {
            recipe.Name = "";
            recipe.Description = "";
            recipe.Instructions = "";
            recipe.Ingredients = "";
            recipe.Calories = 0;
            recipe.Protein = 0;
            recipe.Fat = 0;
            recipe.Carbs = 0;
        }

        public async Task GenerateRecipes(NutritionForm nutritionForm, Day day)
        {
            string[] recipeList = { "Breakfast", "Lunch", "Dinner", "Snack" };

            // Clear existing recipes.
            day.Recipes.Clear();

            // Initialize and add new recipes.
            foreach (var meal in recipeList)
            {
                Recipe recipeTemplate = new Recipe(meal);
                await GenerateRecipe(nutritionForm, recipeTemplate);
                day.Recipes.Add(recipeTemplate);
            }
        }


        public void ResetRecipes(Day day)
        {
            Console.WriteLine("Called reset Recipes");
            string[] recipeList = { "Breakfast", "Lunch", "Dinner", "Snack" };

            day.Recipes.Clear();

            // Initialize and add new recipes
            foreach (var meal in recipeList)
            {
                Recipe recipeTemplate = new Recipe(meal);
                day.Recipes.Add(recipeTemplate);
            }
        }

        public void ParseGeneratedRecipe(Recipe recipe, string generatedRecipe)
        {

            Console.WriteLine(generatedRecipe);

            recipe.Name = "";
            recipe.Description = "";
            recipe.Instructions = "";
            recipe.Ingredients = "";
            recipe.Calories = 0.0;
            recipe.Protein = 0.0;
            recipe.Fat = 0.0;
            recipe.Carbs = 0.0;

            // Split the generated recipe string into lines using \n.
            var lines = generatedRecipe.Split("\n");

            // Define regex patterns for numerical values
            Regex numericRegex = new Regex(@"\d+(\.\d+)?");

            // Iterate through each line to identify and extract relevant information,
            foreach (var line in lines)
            {
                // Identify and extract recipe name.
                if (line.StartsWith("Name:"))
                {
                    recipe.Name = line.Substring("Name:".Length).Trim();
                }
                else if (line.StartsWith("Description:"))
                {
                    var descriptionBuilder = new StringBuilder();
                    int index = Array.IndexOf(lines, line);
                    while (index < lines.Length && !lines[index].StartsWith("Instructions:"))
                    {
                        descriptionBuilder.AppendLine(lines[index].Trim());
                        index++;
                    }
                    recipe.Description = descriptionBuilder.ToString().Trim();
                }
                // Identify and extract recipe instructions.
                else if (line.StartsWith("Instructions:"))
                {
                    var instructionsBuilder = new StringBuilder();
                    int index = Array.IndexOf(lines, line);
                    while (index < lines.Length && !lines[index].StartsWith("Ingredients:"))
                    {
                        instructionsBuilder.AppendLine(lines[index].Trim());
                        index++;
                    }
                    recipe.Instructions = instructionsBuilder.ToString().Trim();
                }
                // Identify and extract recipe ingredients.
                else if (line.StartsWith("Ingredients:"))
                {
                    var ingredientsBuilder = new StringBuilder();
                    int index = Array.IndexOf(lines, line);
                    while (index < lines.Length && !lines[index].StartsWith("Calories:"))
                    {
                        ingredientsBuilder.AppendLine(lines[index].Trim());
                        index++;
                    }
                    recipe.Ingredients = ingredientsBuilder.ToString().Trim();
                }

                // Identify and extract the nutritional facts using regex.
                else if (line.StartsWith("Calories:"))
                {
                    string valueString = numericRegex.Match(line).Value;
                    if (double.TryParse(valueString, out double value))
                    {
                        recipe.Calories = value;
                    }
                    else
                    {
                        Console.WriteLine("Error: Failed to parse calories value: " + valueString);
                    }
                }
                else if (line.StartsWith("Protein:"))
                {
                    string valueString = numericRegex.Match(line).Value;
                    if (double.TryParse(valueString, out double value))
                    {
                        recipe.Protein = value;
                    }
                    else
                    {
                        Console.WriteLine("Error: Failed to parse protein value: " + valueString);
                    }
                }
                else if (line.StartsWith("Fat:"))
                {
                    string valueString = numericRegex.Match(line).Value;
                    if (double.TryParse(valueString, out double value))
                    {
                        recipe.Fat = value;
                    }
                    else
                    {
                        Console.WriteLine("Error: Failed to parse fat value: " + valueString);
                    }
                }
                else if (line.StartsWith("Carbohydrates:"))
                {
                    string valueString = numericRegex.Match(line).Value;
                    if (double.TryParse(valueString, out double value))
                    {
                        recipe.Carbs = value;
                    }
                    else
                    {
                        Console.WriteLine("Error: Failed to parse carbohydrates value: " + valueString);
                    }
                }
            }
        }
    }
}
