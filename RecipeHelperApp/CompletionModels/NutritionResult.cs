namespace RecipeHelperApp.CompletionModels
{

    // https://calorieninjas.com/profile

    // STEPS:
    // 1. Create a IngredientsResult Model
    // 2. Assemble a method to call the API.
    // 3. Alter the recipes controller to call the ingedientsresult method.
    // 4. If the values exceed the user's minimum or maximum values, regenerate the recipe.
    // 5. Only generate the image after an image has been decided upon.
    // 

    // tffxTQVmQCe0hCuPxAz0rg==lTytTxSQDhoZbp8h
   public class NutritionResult
    {
        public string name { get; set; }
        public double calories { get; set; }
        public double serving_size_g { get; set; }
        public double fat_total_g { get; set; }
        public double fat_saturated_g { get; set; }
        public double protein_g { get; set; }
        public double sodium_mg { get; set; }
        public double potassium_mg { get; set; }
        public double cholesterol_mg { get; set; }
        public double carbohydrates_total_g { get; set; }
        public double fiber_g { get; set; }
        public double sugar_g { get; set; }
    }

    public class NutritionData
    {
       public List<NutritionResult> items {  get; set; }
    }

}