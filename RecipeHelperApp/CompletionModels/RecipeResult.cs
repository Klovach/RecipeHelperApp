namespace RecipeHelperApp.CompletionModels
{
    public class RecipeResult
    {
        public string Name { get; set; }

        // Description
        public string Description { get; set; }
        // Instructions
        public string Instructions { get; set; }

        //Ingredients
        public string Ingredients { get; set; }

        // Servings 
        public int Servings { get; set; }

        // Nutritional Facts
        public double Calories { get; set; }
        public double ServingSize { get; set; }
        public double Fat { get; set; }
        public double Protein { get; set; }
        public double Sodium { get; set; }
        public double Potassium { get; set; }
        public double Cholesterol { get; set; }
        public double Carbohydrates { get; set; }
        public double Fiber { get; set; }
        public double Sugar { get; set; }
    }
}
