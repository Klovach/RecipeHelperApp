using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using RecipeHelperApp.Data;
using System.ComponentModel.DataAnnotations;

namespace RecipeHelperApp.CustomAttributes
{
    public class ValidateFitnessGoal : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Get the values of weight and targetWeight from the object instance.
            var nutritionalGoalProperty = validationContext.ObjectType.GetProperty("FitnessGoal");
            var weightProperty = validationContext.ObjectType.GetProperty("Weight");
            var targetWeightProperty = validationContext.ObjectType.GetProperty("TargetWeight");

            if (nutritionalGoalProperty == null || weightProperty == null || targetWeightProperty == null)
            {
                return new ValidationResult("Invalid property names.");
            }

            var nutritionalGoalValue = (string)nutritionalGoalProperty.GetValue(validationContext.ObjectInstance);
            var weightValue = (double)weightProperty.GetValue(validationContext.ObjectInstance);
            var targetWeightValue = (double)targetWeightProperty.GetValue(validationContext.ObjectInstance);

            bool isMatch = true;
            string errorMessage = "Your provided weight does not match your fitness goal."; 

            // Set to true by default. This way we can set a custom error message for each case.
            switch (nutritionalGoalValue)
            {
                case "Lose Weight":
                    if (!(weightValue > targetWeightValue))
                        isMatch = false;
                    errorMessage = "The provided weight does not match the fitness goal. You're trying to lose weight, but your current weight is not greater than your target weight.";
                    break;
                case "Maintain Weight":
                    if (weightValue != targetWeightValue)
                        isMatch = false;
                        errorMessage = "The provided weight does not match the fitness goal. You're trying to mantain weight, but your current weight is not the same value as your target weight.";
                    break; 
                case "Gain Weight":
                    if (!(weightValue < targetWeightValue))
                        isMatch = false;
                    errorMessage = "The provided weight does not match the fitness goal. You're trying to gain weight, but your current weight is not less than your target weight.";
                    break; 
            }

            if (isMatch)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(ErrorMessage ?? errorMessage);
            }
        }
    }
}

