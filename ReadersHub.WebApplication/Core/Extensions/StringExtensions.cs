using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ReadersHub.WebApplication.Core.Extensions
{
    public static class StringExtensions
    {
        public static decimal SmartParseDecimal(this string s)
        {
            //CultureInfo.InvariantCulture uses . as a decimal separator, and , as a thousands separator. 

            var dotCount = s.Count(x => x == '.');
            var commaCount = s.Count(x => x == ',');

            if (commaCount > 1)
            {
                s = s.Replace(",", "");
            }

            if (dotCount > 1)
            {
                s = s.Replace(".", "");
            }

            if (commaCount > 0 && dotCount > 0)
            {
                //find last non-numeric char
                var commaIndex = s.IndexOf(',');
                var dotIndex = s.IndexOf('.');

                if (commaIndex > dotIndex) //comma used for decimal seperator
                {
                    s = s.Replace(".", ""); //remove unnecessary dots
                }
                else //dot used for decimal seperator
                {
                    s = s.Replace(",", ""); //remove unnecessary commas
                }
            }

            s = s.Replace(",", ".");

            return Decimal.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture);

        }

        public static bool SmartTryParseDecimal(this string s, out decimal result)
        {
            //CultureInfo.InvariantCulture uses . as a decimal separator, and , as a thousands separator. 

            var dotCount = s.Count(x => x == '.');
            var commaCount = s.Count(x => x == ',');

            if (commaCount > 1)
            {
                s = s.Replace(",", "");
            }

            if (dotCount > 1)
            {
                s = s.Replace(".", "");
            }

            if (commaCount > 0 && dotCount > 0)
            {
                //find last non-numeric char
                var commaIndex = s.IndexOf(',');
                var dotIndex = s.IndexOf('.');

                if (commaIndex > dotIndex) //comma used for decimal seperator
                {
                    s = s.Replace(".", ""); //remove unnecessary dots
                }
                else //dot used for decimal seperator
                {
                    s = s.Replace(",", ""); //remove unnecessary commas
                }
            }

            s = s.Replace(",", ".");

            return Decimal.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out result);

        }

        public static String SafeSubstring(this string s, int start, int length)
        {
            String returnString = String.Empty;
            int stringLength = !String.IsNullOrEmpty(s) ? s.Length : 0;
            if (!String.IsNullOrEmpty(s) && stringLength > start)
            {
                returnString = s;
                if (stringLength > start + length)
                    returnString = s.Substring(start, length);
                else
                    returnString = s.Substring(start);
            }
            return returnString;
        }

        /// <summary>
        /// checks wheter value is null. if it is, returns string empty, else changes type to string and returns.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <returns></returns>
        public static String SafeToString<T>(this Nullable<T> s) where T : struct
        {
            if (s.HasValue)
                return s.Value.ToString();
            return String.Empty;
        }

        public static bool SmartParseBoolean(this string s)
        {
            string result = string.Empty;
            switch (s.Trim().ToUpper())
            {
                case "YES":
                case "Y":
                case "E":
                case "EVET":
                case "TRUE":
                case "1":
                    result = "true";
                    break;
                case "NO":
                case "N":
                case "H":
                case "HAYIR":
                case "FALSE":
                case "0":
                    result = "false";
                    break;
            }
            return Boolean.Parse(result);
        }

    }
}