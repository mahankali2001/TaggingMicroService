using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Service.Contracts.Core.Validators
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ValidateCollectionAttribute : ValidationAttribute
    {
        public Type ValidationType { get; set; }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var collectionResults = new ValidationResultCollection(String.Format(Service.Contracts.Resources.ValidationMessages.ObjectValidationFailed, validationContext.DisplayName));
            var enumerable = value as IEnumerable;
            if (enumerable != null)
            {
                var index = 0;
                foreach (var val in enumerable)
                {
                    if (val == null)
                    {
                        continue;
                    }

                    var results = new List<ValidationResult>();
                    var context = new ValidationContext(val, validationContext.ServiceContainer, null);
                    Validator.TryValidateObject(val, context, results, true);
                    if (results != null && results.Count != 0)
                    {
                        var elementName = string.Format("{0}[{1}]", validationContext.DisplayName, index);
                        var compositeResults = new ValidationResultCollection(String.Format(Resources.ValidationMessages.ObjectValidationFailed, elementName));
                        results.ForEach(compositeResults.AddResult);
                        collectionResults.AddResult(compositeResults);
                    }
                    index++;
                }
            }

            if (collectionResults.Results.Any())
            {
                return collectionResults;
            }
            return ValidationResult.Success;
        }

        private IEnumerable<ValidationAttribute> GetValidators()
        {
            if (ValidationType == null) yield break;

            yield return (ValidationAttribute)Activator.CreateInstance(ValidationType);
        }
    }
}