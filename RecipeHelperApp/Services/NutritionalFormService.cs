using Microsoft.EntityFrameworkCore;
using RecipeHelperApp.Data;
using RecipeHelperApp.Models;
using static RecipeHelperApp.Models.NutritionForm;

namespace RecipeHelperApp.Services
{
    public class NutritionalFormService
    {
        public NutritionalFormService()
        {
        }

        // NutritionFormService 
        public Nutrients CalculateNeeds(ApplicationUser user)
        {
            int userAge = user.GetAge(); 
            double BMR = GetBaseMetabolicRate(user.Sex, user.Weight, user.Height, userAge);
            double TDEE = GetTotalDailyExpenditure(user.ActivityLevel, BMR);
            double dailyCalories = GetDailyCalories(user.FitnessGoal, user.PoundsPerWeek, TDEE);
            Nutrients nutrients = GetMacroRatios(user.FitnessGoal, dailyCalories);

            return nutrients;
        }

        // Calculate Base Metabolic Rate Using Miffin-St Jeor Equation:
        public double GetBaseMetabolicRate(string sex, double weight, double height, int age)
        {
            Console.WriteLine("Weight was:" + weight);
            Console.WriteLine("Height was:" + height);
            Console.WriteLine("Sex was: " + sex);

            double BMR;
            // Convert weight to KG
            weight = weight / 2.20462;


            // If Male:
            if (sex == "Male")
            {
                BMR = (10 * weight) + (6.25 * height) - (5 * age) + 5;
            }
            // If Female:
            else
            {
                BMR = (10 * weight) + (6.25 * height) - (5 * age) - 161;
            }

            Console.WriteLine("BMR was: " + BMR);
            return BMR;
        }

        public double GetTotalDailyExpenditure(string activityLevel, double BMR)
        {
            double activityMultiplier;
            double TDEE = 0;

            switch (activityLevel)
            {
                case "Sedentary":
                    activityMultiplier = 1.2;
                    TDEE = BMR * activityMultiplier;
                    break;
                case "Minimally Active":
                    activityMultiplier = 1.375;
                    TDEE = BMR * activityMultiplier;
                    break;
                case "Moderately Active":
                    activityMultiplier = 1.55;
                    TDEE = BMR * activityMultiplier;
                    break;
                case "Very Active":
                    activityMultiplier = 1.725;
                    TDEE = BMR * activityMultiplier;
                    break;
                case "Extremely Active":
                    activityMultiplier = 1.9;
                    TDEE = BMR * activityMultiplier;
                    break;
                default:
                    break;
            }

            Console.WriteLine("TDEE was: " + TDEE);

            return TDEE;
        }

        public double GetDailyCalories(string nutritionalGoal,double poundsPerWeek, double TDEE)
        {
            if (string.IsNullOrEmpty(nutritionalGoal)) return 0;

            double dailyCalories = 0;
            double caloricDifference = (poundsPerWeek * 3500) / 7;

            switch (nutritionalGoal)
            {
                case "Lose Weight":
                    dailyCalories = TDEE - caloricDifference;
                    break;

                case "Maintain Weight":
                    dailyCalories = TDEE;
                    break;

                case "Gain Weight":
                    dailyCalories = TDEE + caloricDifference;
                    break;
                default:
                    Console.Write("Something went wrong");
                    break; 
            }

            Console.WriteLine("daily Calories" + dailyCalories);
            return dailyCalories;
        }

        // Change to minMax 
        public Nutrients GetMacroRatios(string nutritionalGoal, double dailyCalories)
        {
            double carbPercentage = 0, proteinPercentage = 0, fatPercentage = 0;

            switch (nutritionalGoal)
            {
                case "Lose Weight":
                    carbPercentage = 0.35;
                    proteinPercentage = 0.30;
                    fatPercentage = 0.35;
                    break;
                case "Maintain Weight":
                    carbPercentage = 0.35;
                    proteinPercentage = 0.30;
                    fatPercentage = 0.35;
                    break;
                case "Gain Weight":
                    carbPercentage = 0.35;
                    proteinPercentage = 0.30;
                    fatPercentage = 0.35;
                    break;
                default:
                    break;
            }

            double carbCalories = dailyCalories * carbPercentage;
            double proteinCalories = dailyCalories * proteinPercentage;
            double fatCalories = dailyCalories * fatPercentage;

            double carbGrams = carbCalories / 4;
            double proteinGrams = proteinCalories / 4;
            double fatGrams = fatCalories / 9;


            Nutrients macroRatios = new Nutrients(Math.Round(dailyCalories, 1), Math.Round(carbGrams, 1), Math.Round(proteinGrams, 1), Math.Round(fatGrams, 1));

            return macroRatios;
        }
    }
}