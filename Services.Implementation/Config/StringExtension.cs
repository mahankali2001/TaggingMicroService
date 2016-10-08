using System;
using System.Globalization;

namespace Services.Implementation.Config
{
    public static class StringExtension
    {
        public static DateTime? ToDateTime(this string value, string dateTimePattern)
        {
            DateTime dt;
            if (String.IsNullOrEmpty(value))
                return null;

            bool canParse = DateTime.TryParseExact(value, dateTimePattern, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
            if (canParse)
                return dt;
            else
            {
                return null;
            }
        }
    }
}