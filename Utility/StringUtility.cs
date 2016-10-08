using System;
using System.Collections.Generic;
using System.Linq;

namespace Utility
{
    public static class StringUtility
    {
        public static int DefaultNullableInt = 0;
        public static char DEFAULT_STRING_DELIMITER_CHAR = ',';
        public static DateTime DateTimeMinimum = DateTime.MinValue;
        public static int IntNull(int? i)
        {
            if (i == null)
                return DefaultNullableInt;
            else
                try
                {
                    return Convert.ToInt32(i);
                }
                catch (Exception)
                {
                    return DefaultNullableInt;                                       
                }
        }

        public static DateTime DateTimeNull(DateTime? i)
        {
            if (i == null)
                return DateTimeMinimum;
            else
                try
                {
                    return i.Value;
                }
                catch (Exception)
                {
                    return DateTimeMinimum;
                }
        }

        public static int[] ConvertStringToArrayInt(string input)
        {
            var ret = new int[] {};
            if (String.IsNullOrEmpty(input))
                return ret;
            else
                ret = input.Split(DEFAULT_STRING_DELIMITER_CHAR).Select(n => Convert.ToInt32((string)n)).ToArray();
            return ret;
        }

        public static List<int> ConvertArryToList(int[] a)
        {
            var l = new List<int>();
            if ((a == null) || (!a.Any())) return l;
            l.AddRange(a);
            return l;
        }
    }
}
