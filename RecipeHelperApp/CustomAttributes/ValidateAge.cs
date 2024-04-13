using System.ComponentModel.DataAnnotations;

namespace RecipeHelperApp.CustomAttributes
{
    // Age validator 
    public class ValidateAge : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                DateTime dateOfBirth = (DateTime)value;
                var today = DateTime.Today;
                var age = today.Year - dateOfBirth.Year;
                if (age >= 13)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult($"You must be at least 13 years old.");
                }
            }
            return new ValidationResult("Invalid date of birth.");
        }
    }
}
