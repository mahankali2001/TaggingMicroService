// -----------------------------------------------------------------------
// <copyright file="Helper.cs" company="IBM">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Globalization;

namespace Business.Implementation.Utility
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static partial class Helper
    {
        public static void IffFalse(bool condition, Action falseAction)
        {
            if (!condition)
            {
                falseAction();
            }
        }

        public static void IffTrue(bool condition, Action trueAction)
        {
            if (condition)
            {
                trueAction();
            }
        }

        public static void Iff(bool condition, Action trueAction, Action falseAction)
        {
            if (condition)
            {
                trueAction();
            }
            else
            {
                falseAction();
            }
        }

        /// <summary>
        /// Checks if the two lists are same
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        internal static bool AreSame(List<int> a, List<int> b)
        {
            if (a == null && b == null)
            {
                return true;
            }

            if (a == null || b == null || a.Count != b.Count)
            {
                return false;
            }

            a.Sort();
            b.Sort();

            for (int index = 0; index < a.Count; index++)
            {
                if (a[index] != b[index])
                {
                    return false;
                }
            }

            return true;
        }

        public static int ToEpoch(DateTime d)
        {
            TimeSpan t = d - new DateTime(1970, 1, 1);
            int secondsSinceEpoch = (int) t.TotalSeconds;
            return secondsSinceEpoch;
        }

        public static int ConvertToInt(string s)
        {
            int r;
            if (int.TryParse(s, out r))
            {
                return r;
            }
            else
            {
                throw new ArgumentException(string.Format("Parameter [{0}] passed is not a valid integer ", s));
            }
        }

        public static string FormatTimeSpan(TimeSpan timeSpan, bool wholeSeconds = true)
        {
            string txt = "";
            if (timeSpan.Days > 0)
            {
                txt += ", " + timeSpan.Days.ToString(CultureInfo.InvariantCulture) + " days";
                timeSpan = timeSpan.Subtract(new TimeSpan(timeSpan.Days, 0, 0, 0));
            }
            if (timeSpan.Hours > 0)
            {
                txt += ", " + timeSpan.Hours.ToString(CultureInfo.InvariantCulture) + " hours";
                timeSpan = timeSpan.Subtract(new TimeSpan(0, timeSpan.Hours, 0, 0));
            }
            if (timeSpan.Minutes > 0)
            {
                txt += ", " + timeSpan.Minutes.ToString(CultureInfo.InvariantCulture) + " " + "minutes";
                timeSpan = timeSpan.Subtract(new TimeSpan(0, 0, timeSpan.Minutes, 0));
            }

            if (wholeSeconds)
            {
                // Display only whole seconds.
                if (timeSpan.Seconds > 0)
                {
                    txt += ", " + timeSpan.Seconds.ToString(CultureInfo.InvariantCulture) + " " + "seconds";
                }
            }
            else
            {
                // Display fractional seconds.
                txt += ", " + timeSpan.TotalSeconds.ToString(CultureInfo.InvariantCulture) + " " + "seconds";
            }

            // Remove the leading ", ".
            if (txt.Length > 0)
                txt = txt.Substring(2);
            // Return the result.
            return txt;
        }
    }
}
