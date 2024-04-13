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

        public Nutrients CalculateNeeds(ApplicationUser user)
        {
            int userAge = CalculateAge(user.BirthDate);
            double BMR = GetBaseMetabolicRate(user.Sex, user.Weight, user.Height, userAge);
            double TDEE = GetTotalDailyExpenditure(user.ActivityLevel, BMR);
            double poundsPerWeek = GetPoundsPerWeek(user.Weight, user.TargetWeight, user.TargetWeightDate);
            double dailyCalories = GetDailyCalories(user.FitnessGoal, user.Weight, poundsPerWeek, TDEE);
            Nutrients nutrients = GetMacroRatios(user.FitnessGoal, dailyCalories);

            return nutrients;
        }

        // Calculate a user's age from birth date.
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
        public double GetBaseMetabolicRate(string sex, double weight, double height, int age)
        {
            double BMR;

            // If Male:
            if (sex == "Male")
            {
                BMR = 10 * weight + 6.25 * height - 5 * age + 5;
            }
            // If Female:
            else
            {
                BMR = 10 * weight + 6.25 * height - 5 * age - 161;
            }

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
                    activityMultiplier = 3.755;
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

            return TDEE;
        }

        public double GetPoundsPerWeek(double userWeight, double userGoalWeight, DateTime goalDate)
        {
            TimeSpan timeRemaining = goalDate - DateTime.Now;
            int weeksRemaining = (int)Math.Ceiling(timeRemaining.TotalDays / 7);
            double poundsPerWeek = (userWeight - userGoalWeight) / weeksRemaining;
            return poundsPerWeek;
        }

        public double GetDailyCalories(string nutritionalGoal, double userWeight, double poundsPerWeek, double TDEE)
        {
            if (string.IsNullOrEmpty(nutritionalGoal)) return 0;

            double dailyCalories = 0;
            double caloricDifference = poundsPerWeek * (3500 / userWeight) / 7;

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
        public Nutrients GetMacroRatios(string nutritionalGoal, double dailyCalories)
        {
            double carbPercentage = 0, proteinPercentage = 0, fatPercentage = 0;

            switch (nutritionalGoal)
            {
                case "Lose Weight":
                    carbPercentage = 0.30;
                    proteinPercentage = 0.50;
                    fatPercentage = 0.20;
                    break;
                case "Maintain Weight":
                    carbPercentage = 0.55;
                    proteinPercentage = 0.25;
                    fatPercentage = 0.25;
                    break;
                case "Gain Weight":
                    carbPercentage = 0.45;
                    proteinPercentage = 0.35;
                    fatPercentage = 0.20;
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


            Nutrients macroRatios = new Nutrients(Math.Round(dailyCalories), Math.Round(carbGrams), Math.Round(proteinGrams), Math.Round(fatGrams));

            return macroRatios;
        }
    }
}