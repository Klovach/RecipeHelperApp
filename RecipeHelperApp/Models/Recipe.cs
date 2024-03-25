using RecipeHelperApp.Data.Migrations;
using RecipeHelperApp.Services;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.RegularExpressions;

namespace RecipeHelperApp.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public int DayId { get; set; }
        public Day Day { get; set; }
        public string? Image { get; set; }
        public string MealType { get; set; }

        // Name
        public string? Name { get; set; }

        // Description
        public string? Description { get; set; }
        // Instructions
        public string? Instructions { get; set; }

        //Ingredients
        public string? Ingredients { get; set; }

        // Nutritional Facts
        public double Calories { get; set; }
        public double Protein { get; set; }
        public double Fat { get; set; }
        public double Carbs { get; set; }

        public Recipe(string MealType)
        {
            this.MealType = MealType;
            Name = ""; 
            Description = "";
            Instructions = "";
            Ingredients = "";
        }

        public Recipe()
        {

        }



        public string GetNutrientAsString(double nutrient)
        {
            string value;
            value = nutrient.ToString() + "g";
            return value;
        }

        public async Task<Recipe> GenerateRecipe(NutritionForm nutritionForm)
        {
            Recipe generatedRecipe = new Recipe();
            generatedRecipe.Id = this.Id;
            generatedRecipe.DayId = this.DayId;
            generatedRecipe.MealType = this.MealType;

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

            return await RecipeGenerator.GenerateRecipeAsync(nutritionForm, generatedRecipe);
        }

        public void ResetValues()
        {
            Name = "";
            Description = "";
            Instructions = "";
            Ingredients = "";
            Calories = 0;
            Protein = 0;
            Fat = 0;
            Carbs = 0;

        }

        public void ParseGeneratedRecipe(string generatedRecipe)
        {

            Console.WriteLine(generatedRecipe);

            Name = "";
            Description = "";
            Instructions = "";
            Ingredients = "";
            Calories = 0.0;
            Protein = 0.0;
            Fat = 0.0;
            Carbs = 0.0;

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
                    Name = line.Substring("Name:".Length).Trim();
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
                    Description = descriptionBuilder.ToString().Trim();
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
                    Instructions = instructionsBuilder.ToString().Trim();
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
                    Ingredients = ingredientsBuilder.ToString().Trim();
                }

                // Identify and extract the nutritional facts using regex.
                else if (line.StartsWith("Calories:"))
                {
                    string valueString = numericRegex.Match(line).Value;
                    if (double.TryParse(valueString, out double value))
                    {
                        Calories = value;
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
                        Protein = value;
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
                        Fat = value;
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
                        Carbs = value;
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
