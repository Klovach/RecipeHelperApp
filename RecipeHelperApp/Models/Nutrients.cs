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
            Carbs = maxCarbohydrates;
            Protein = maxProtein;
            Fat = maxFat;
        }


        public double Calories { get; set; }
        public double Carbs { get; set; }
        public double Protein { get; set; }
        public double Fat {  get; set; }

        
    }
}
