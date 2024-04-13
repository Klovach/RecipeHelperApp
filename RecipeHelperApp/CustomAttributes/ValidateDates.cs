using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using RecipeHelperApp.Data;
using System.ComponentModel.DataAnnotations;

namespace RecipeHelperApp.CustomAttributes
{
    public class ValidateDates : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                DateTime dateToCheck = (DateTime)value;
                if (dateToCheck < DateTime.Now)
                {
                    return new ValidationResult("Your date cannot be in the past, it  must be in the future!");
                }
            }
            return ValidationResult.Success;
        }
    }
}
