using Microsoft.AspNetCore.Mvc.ModelBinding;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using System;
using System.ComponentModel.DataAnnotations;

namespace P3AddNewFunctionalityDotNetCore.Utilities.ValidationAttributes
{
    public class DoubleParserValidationAttribute: ValidationAttribute
    {

        protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
        {
            if (!double.TryParse(value?.ToString(), out _))
            {
                return new ValidationResult(ErrorMessage);
            }


            return ValidationResult.Success;
        }
    }
}
