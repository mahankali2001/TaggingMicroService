using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Service.Contracts.Core.Validators
{
    public class ValidationResultCollection : ValidationResult
    {
        private readonly List<ValidationResult> _results = new List<ValidationResult>();

        public IEnumerable<ValidationResult> Results
        {
            get
            {
                return _results;
            }
        }

        public ValidationResultCollection(string errorMessage) : base(errorMessage) { }
        public ValidationResultCollection(string errorMessage, IEnumerable<string> memberNames) : base(errorMessage, memberNames) { }
        protected ValidationResultCollection(ValidationResult validationResult) : base(validationResult) { }

        public void AddResult(ValidationResult validationResult)
        {
            _results.Add(validationResult);
        }
    }
}
