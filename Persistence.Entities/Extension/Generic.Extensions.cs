using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace Persistence.Entities.Extension
{
    public static class GenericExtension
    {
        public static object GetDbValue<T>(this Nullable<T> value) where T : struct
        {
            if (value == null || !value.HasValue) { return DBNull.Value; }
            return value.Value;
        }

        public static object GetDbValue(this string value)
        {
            return value == null ? (object)DBNull.Value : value;
        }

        internal static object CheckString(this string s)
        {
            if (String.IsNullOrEmpty(s))
                return DBNull.Value;
            return s;
        }
    }
}
