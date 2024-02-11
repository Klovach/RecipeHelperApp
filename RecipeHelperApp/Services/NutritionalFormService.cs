using Microsoft.EntityFrameworkCore;
using RecipeHelperApp.Data;
using RecipeHelperApp.Models;
using static RecipeHelperApp.Models.NutritionForm;

namespace RecipeHelperApp.Services
{
    [Keyless]
    public class NutritionalFormService
    {
        public NutritionalFormService()
        {
        }

        public Nutrients calculateNeeds(DateTime birthDate, string sex, decimal weight, decimal height, decimal
            targetWeight, DateTime targetWeightDate, string activityLevel, string fitnessGoal)
        {
            int userAge = CalculateAge(birthDate);  
            decimal BMR = GetBaseMetabolicRate(sex, weight, height, userAge);
            decimal TDEE = GetTotalDailyExpenditure(activityLevel, BMR);
            decimal poundsPerWeek = GetPoundsPerWeek(weight,targetWeight,targetWeightDate);
            decimal dailyCalories = GetDailyCalories(fitnessGoal, weight, poundsPerWeek, TDEE);
            Nutrients nutrients = GetMacroRatios(fitnessGoal, dailyCalories);
       
            return nutrients; 
        }


        // Calculate a user's age accurately from birth date. 
        public int CalculateAge(DateTime birthDate)
        {
            DateTime currentDate = DateTime.Now;
            int age = currentDate.Year - birthDate.Year;

            
            if (birthDate > currentDate.AddYears(-age))
            {
                age--;
            }

            return age;
        }

        // Calculate Base Metabolic Rate Using Miffin-St Jeor Equation:
        public decimal GetBaseMetabolicRate(string sex, decimal weight, decimal height, int age)
        {
            decimal BMR;

            // If Male:
            if (sex == "Male")
            {
                BMR = 10 * weight + 6.25m * height - 5 * age + 5;
            }
            // If Female:
            else
            {
                BMR = 10 * weight + 6.25m * height - 5 * age - 161;
            }

            return BMR;
        }

        public decimal GetTotalDailyExpenditure(string activityLevel, decimal BMR)
        {
            decimal activityMultiplier;
            decimal TDEE = 0;

            switch (activityLevel)
            {
                case "Sedentary":
                    activityMultiplier = 1.2m;
                    TDEE = BMR * activityMultiplier;
                    break;
                case "Minimally Active":
                    activityMultiplier = 3.755m;
                    TDEE = BMR * activityMultiplier;
                    break;
                case "Moderately Active":
                    activityMultiplier = 1.55m; 
                    TDEE = BMR * activityMultiplier;
                    break;
                case "Very Active":
                    activityMultiplier = 1.725m;
                    TDEE = BMR * activityMultiplier;
                    break;
                case "Extremely Active":
                    activityMultiplier = 1.9m;
                    TDEE = BMR * activityMultiplier;
                    break;
                default:
                    break;
            }

            return TDEE;
        }

        public decimal GetPoundsPerWeek(decimal userWeight, decimal userGoalWeight, DateTime goalDate)
        {
            TimeSpan timeRemaining = goalDate - DateTime.Now;
            int weeksRemaining = (int)Math.Ceiling(timeRemaining.TotalDays / 7);
            decimal poundsPerWeek = (userWeight - userGoalWeight) / weeksRemaining;
            return poundsPerWeek;
        }


        public decimal GetDailyCalories(string nutritionalGoal, decimal userWeight, decimal poundsPerWeek, decimal TDEE)
        {
            if (string.IsNullOrEmpty(nutritionalGoal)) return 0;

            decimal dailyCalories = 0;
            decimal caloricDifference = poundsPerWeek * (3500 / userWeight) / 7;

            switch (nutritionalGoal)
            {
               
                case "Lose Weight":
                    dailyCalories = TDEE - caloricDifference;
                    break;
                    
                case "Mantain Weight":
                    dailyCalories = TDEE;
                    break;

                case "Gain Weight":
                    dailyCalories = TDEE + caloricDifference;
                    break; 

            }

            return dailyCalories;
        }

        // Change to minMax 
        public Nutrients GetMacroRatios(string nutritionalGoal, decimal dailyCalories)
        {

            var carbohydratesRange = 0..0;
            var proteinRange = 0..0;
            var fatRange = 0..0;

            switch (nutritionalGoal)
            {
                case "Lose Weight":
                    carbohydratesRange = 10..20;
                    proteinRange = 40..50;
                    fatRange = 30..40;
                    break;
                case "Mantain Weight":
                    carbohydratesRange = 30..40;
                    proteinRange = 25..35;
                    fatRange = 25..35;
                    break;
                case "Gain Weight":
                    carbohydratesRange = 50..60;
                    proteinRange = 25..35;
                    fatRange = 20..35;
                    break;
            }
            // GET VALUE FROM STRUCT 
            var startCarbs = carbohydratesRange.Start.Value;
            var endCarbs = carbohydratesRange.End.Value;

            var startProtein = proteinRange.Start.Value;
            var endProtein = proteinRange.End.Value;

            var startFat = fatRange.Start.Value;
            var endFat = fatRange.End.Value;

            dailyCalories = Math.Round(dailyCalories, 0); 
            decimal minProtein = Math.Round(((startProtein * dailyCalories) / 4), 0);
            decimal maxProtein = Math.Round(((endProtein * dailyCalories) / 4), 0);

            decimal minCarbohydrates = Math.Round(((startCarbs * dailyCalories) / 4), 0);
            decimal maxCarbohydrates = Math.Round(((endCarbs * dailyCalories) / 4), 0);

            decimal minFat = Math.Round(((startFat * dailyCalories) / 9), 0);
            decimal maxFat = Math.Round(((endFat * dailyCalories) / 9), 0);

            Nutrients macroRatios = new Nutrients(dailyCalories, maxCarbohydrates, maxProtein, maxFat);

            return macroRatios;
        }
    }
}
