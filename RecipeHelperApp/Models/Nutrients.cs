using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeHelperApp.Models
{
  
    [NotMapped]
    public class Nutrients
    {
        public Nutrients()
        {

        }

        public Nutrients(double dailyCalories, double maxCarbohydrates, double maxProtein, double maxFat)
        {
            Calories = dailyCalories;
            Carbohydrates = maxCarbohydrates;
            Protein = maxProtein;
            Fat = maxFat;
        }


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
