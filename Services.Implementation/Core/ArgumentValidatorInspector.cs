using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.Dispatcher;
using System.Text;
using Service.Contracts.Core.Validators;

namespace Services.Implementation.Core
{
    public class ArgumentValidatorInspector : IParameterInspector
    {
        public void AfterCall(string operationName, object[] outputs, object returnValue, object correlationState)
        {
        }

        public object BeforeCall(string operationName, object[] inputs)
        {
            var results = new List<ValidationResult>();
            foreach (var input in inputs)
            {
                if (input != null)
                {
                    var context = new ValidationContext(input, null, null);
                    System.ComponentModel.DataAnnotations.Validator.TryValidateObject(input, context, results, true);

                    if (results != null && results.Count != 0)
                    {
                        var sb = new StringBuilder();
                        FormatValidationResultsToValidationExceptions(results,sb , true);
                    }
                }
            }
            return null;
        }

        private static void FormatValidationResultsToValidationExceptions(IEnumerable<ValidationResult> results, StringBuilder sb, bool isRootCall)
        {
            bool errorFound = false;
            foreach (var validationResult in results)
            {
                errorFound = true;
                sb.Append("[");

                sb.Append(validationResult.ErrorMessage);
                if (validationResult is ValidationResultCollection)
                {
                    FormatValidationResultsToValidationExceptions(((ValidationResultCollection)validationResult).Results,sb, false);
                }
                sb.Append("]");
            }

            if (isRootCall && errorFound)
            {
                throw new ValidationException(sb.ToString());
            }
        }
    }
}