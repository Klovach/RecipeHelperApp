using System.ComponentModel.DataAnnotations;

namespace RecipeHelperApp.CustomAttributes
{
    // Custom validation properties can do a world of good.
    public class ValidateTargetDate : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Check if the value is a valid DateTime. Thn use the borrowed logic from the controller. 
            if (value is DateTime)
            {
                // Get Properties.. Searches for public properties with the specified name. 
                var targetWeightDateProperty = validationContext.ObjectType.GetProperty("TargetWeightDate");
                var weightProperty = validationContext.ObjectType.GetProperty("Weight");
                var targetWeightProperty = validationContext.ObjectType.GetProperty("TargetWeight");

                // Get the values from the properties we acquired. For this to work, bear in mind the property names MUST
                // be the same as they appear in the class we are attempting to validate. 
                var targetWeightDateValue = (DateTime)targetWeightDateProperty.GetValue(validationContext.ObjectInstance);
                var weightValue = (double)weightProperty.GetValue(validationContext.ObjectInstance);
                var targetWeightValue = (double)targetWeightProperty.GetValue(validationContext.ObjectInstance);

                // Calculate the weight loss/gain per week. It is advisable to use a formula here. 
                var weeksDifference = (targetWeightDateValue - DateTime.Now).TotalDays / 7;
                var weightDifference = targetWeightValue - weightValue;
                var weightChangePerWeek = weightDifference / weeksDifference;

                // Check if the weight loss/gain per week is within the allowed range. The pounds per week should not be above
                // a certain threshold. In this instance, we have set a limit of five. 

                // We use Math.Absolute to get the absolute value of our double. 
                if (Math.Abs(weightChangePerWeek) > 5)
                {
                    // Return a validation result.
                    return new ValidationResult("The target weight change exceeds the allowed limit of 5 pounds per week. Check your target date and ensure you are setting a realistic goal!");
                }
            }

            // The default validation result is success. If it fails, the above result will be returned instead. 
            return ValidationResult.Success;
        }
    }
}
