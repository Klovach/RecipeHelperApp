namespace RecipeHelperApp.Services
{
    using Microsoft.AspNetCore.Http.HttpResults;
    using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Images;
using RecipeHelperApp.Models;
using System.Security.Claims;


    public static class RecipeGenerator
    {
        const string openAiApiKey = "";

        static APIAuthentication apiAuthentication;
        static OpenAIAPI openAiApi;

        static string CompilePrompt(NutritionForm nutritionForm)
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


            if (nutritionForm.IncludeIngredients == true)
            {
                prompt += $"Include these ingredients Ingredients:{nutritionForm.IncludedIngredients}";
               
            }
            if (nutritionForm.ExcludeIngredients == true)    
            {
                prompt += $" and exclude these ingredients ExcludedIngrdients:{nutritionForm.ExcludedIngredients}"; 
            }
            if (nutritionForm.IncludeNutrition == true)
            {
              prompt += $"\nBased on the following maximum values: " +
              $"\nCalories: {nutritionForm.Nutrients.Calories / 4}, " +
              $"\nProtein: {nutritionForm.Nutrients.Protein / 4}, " +
              $"\nFat: {nutritionForm.Nutrients.Fat / 4}, " +
              $"\nCarbohydrates: {nutritionForm.Nutrients.Carbs / 4}.";
            }

            Console.WriteLine(prompt);

            return prompt;

        }

        public static async Task<Recipe> GenerateRecipeAsync(NutritionForm nutritionForm, Recipe recipe)
        {
            apiAuthentication = new APIAuthentication(openAiApiKey);
            openAiApi = new OpenAIAPI(apiAuthentication);

            string prompt = CompilePrompt(nutritionForm);

            try
            {
                string model = "gpt-3.5-turbo";
                int maxTokens = 1000;

                var completionRequest = new ChatRequest
                {
                    Model = model,
                    Messages = [new ChatMessage(ChatMessageRole.User, prompt)],
                    MaxTokens = maxTokens
                };

                var completionResult = await openAiApi.Chat.CreateChatCompletionAsync(completionRequest);

                // Extract generated recipe from the completion result
                string generatedRecipe = completionResult.ToString();

                if (generatedRecipe != null)
                {
                    recipe.ParseGeneratedRecipe(generatedRecipe);
                    if (recipe != null)
                        return recipe;
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
    }
}

