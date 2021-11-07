using System.Collections.Generic;
using System.Linq;

namespace Raci.Common.Helpers
{
    public static class CommonHelper
    {
        public static bool IsDifference<T>(List<T> newValues, List<T> originalValues)
        {
            newValues = _newListIfNull(newValues);
            originalValues = _newListIfNull(originalValues);
            return originalValues.Any(d => !newValues.Contains(d)) || newValues.Any(d => !originalValues.Contains(d));
        }

        private static List<T> _newListIfNull<T>(List<T> list)
        {
            if (list == null)
            {
                list = new List<T>();
            }

            return list;
        }

        public static string GetPrettyText(this string text)
        {
            //SplitCamelCase
            return System.Text.RegularExpressions.Regex.Replace(text ?? "", "([A-Z])", " $1", System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
        }

        public static string Replace_AndGetPrettyText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return "";
            }

            // don't splict usd and vnd
            if (text == "USD" || text == "VND")
            {
                return text;
            }

            return GetPrettyText(text.Replace("_", ""));
        }

        public static string Replace_ToSpaceAndGetPrettyText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return "";
            }
            if (text == "USD" || text == "VND")// don't splict usd and vnd
            { return text; }

            return GetPrettyText(text.Replace("_", " "));
        }


        #region Format 
        public static string FormatPriceToString(double? value)
        {
            if (value.HasValue && value.Value > 0)
            {
                if (value.Value > 999999999)
                {
                    string bil = (value.Value / 1000000000).ToString();

                    return (bil.Length > 5 ? bil.Substring(0, 5) : bil) + " bil";
                }
                else
                {
                    return string.Format("{0:0,0}", value.Value);
                }
            }
            return "0";
        }

        public static string FormatPriceToString(decimal? value)
        {
            if (value == null)
                return FormatPriceToString((double?)null);

            return FormatPriceToString((double)value);
        }

        public static string FormatPriceToString(long? value)
        {
            if (value == null)
                return FormatPriceToString((double?)null);

            return FormatPriceToString((double)value);
        }
        #endregion


        public static bool IsNullOrEmpty(this List<int> lst)
        {
            return lst == null || lst.Count == 0;
        }

        public static bool IsNullOrEmpty(this List<int?> lst)
        {
            return lst == null || lst.Count == 0;
        }
    }
}
