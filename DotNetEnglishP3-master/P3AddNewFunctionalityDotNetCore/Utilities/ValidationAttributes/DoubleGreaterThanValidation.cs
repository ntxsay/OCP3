using System.ComponentModel.DataAnnotations;

namespace P3AddNewFunctionalityDotNetCore.Utilities.ValidationAttributes
{
    public class DoubleGreaterThanValidation : ValidationAttribute
    {
        public double Value { get; }
        public DoubleGreaterThanValidation(double value) => Value = value;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is double doubleValue && doubleValue <= Value)
            {
                return new ValidationResult(ErrorMessage);
            }
            return ValidationResult.Success;
        }
    }
}
