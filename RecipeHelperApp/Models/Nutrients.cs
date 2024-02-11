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

        public Nutrients(decimal calories, decimal carbs, decimal protein, decimal fat)
        {
            Calories = calories;
            Carbs = carbs;
            Protein = protein;
            Fat = fat;
        }

        public decimal Calories { get; set; }
        public decimal Carbs { get; set; }
        public decimal Protein { get; set; }
        public decimal Fat {  get; set; }
    }
}
