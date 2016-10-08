using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Service.Contracts.Core.Validators
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ValidateObjectAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;

            var results = new List<ValidationResult>();
            var context = new ValidationContext(value, null, null);

            Validator.TryValidateObject(value, context, results, true);
            if (results != null && results.Count != 0)
            {
                var compositeResults = new ValidationResultCollection(String.Format(Service.Contracts.Resources.ValidationMessages.ObjectValidationFailed, validationContext.DisplayName));
                results.ForEach(compositeResults.AddResult);
                return compositeResults;
            }

            return ValidationResult.Success;
        }
    }
}