using System.ComponentModel.DataAnnotations;
using LM.Api.Helpers;

namespace LM.Api.Filters
{
    public class ISBNValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return IsbnHelper.TryValidate(value.ToString())
                ? ValidationResult.Success
                : new ValidationResult("Please enter a valid Isbn");
        }
    }
}