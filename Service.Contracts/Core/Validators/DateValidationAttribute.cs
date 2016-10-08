using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Service.Contracts;

namespace Service.Contracts.Core.Validators
{
    public class DateValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            DateTime dt;
            bool parsed = DateTime.TryParseExact((string)value, ServiceConstants.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
            return parsed;
        }
    }
}