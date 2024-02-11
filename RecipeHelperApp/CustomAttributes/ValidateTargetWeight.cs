using RecipeHelperApp.Data;
using System.ComponentModel.DataAnnotations;

namespace RecipeHelperApp.CustomAttributes
{
    public class ValidateTargetWeight : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is ApplicationUser user)
            {
                // Access the properties directly from the Car instance
                var weight = user.Weight;
                var targetWeight = user.TargetWeight;
                var nutritionalGoal = user.FitnessGoal;



                if (!DoesGoalMatch(nutritionalGoal, weight, targetWeight))
                {
                    return new ValidationResult("Your target weight doesn't match your intended goal!");
                }
            }


            return ValidationResult.Success;
            }

            public bool DoesGoalMatch(string nutritionalGoal, decimal weight, decimal targetWeight)
            {
                bool isMatch = false;
                switch (nutritionalGoal)
                {
                    case "Lose Weight":
                        if (weight > targetWeight)
                            isMatch = true;
                        break;
                    case "Maintain Weight":
                        if (weight == targetWeight)
                            isMatch = true;
                        break;
                    case "Gain Weight":
                        if (weight < targetWeight)
                            isMatch = true;
                        break;
                }
                return isMatch;
            }
        }
    }

