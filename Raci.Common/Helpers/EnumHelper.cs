using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace Raci.Common.Helpers
{
    public static class EnumHelper
    {
        public static List<(string Text, string Value)> GetDisplayList<T>() where T : struct, IConvertible
        {
            var list = Enum.GetValues(typeof(T)).Cast<T>().Select(v =>
                (CommonHelper.Replace_AndGetPrettyText(v.ToString()),
                (Convert.ToInt32(Enum.Parse(typeof(T), v.ToString()) as Enum)).ToString()))
            .ToList();

            return list;
        }

        [Obsolete("Use GetDisplayValue instead")]
        public static string GetName<T>(int? value)
        {
            if (value.HasValue)
            {
                return CommonHelper.Replace_AndGetPrettyText(Enum.GetName(typeof(T), value));
            }
            return "";
        }

        public static string GetNameFromEnum<T>(T value) where T : struct, IConvertible
        {
            return CommonHelper.Replace_AndGetPrettyText(Enum.GetName(typeof(T), value));
        }

        /// <summary>
        /// Display Attribute
        /// </summary>
        public static string GetDisplayValue<T>(this T type)
        {
            try
            {

                var enumType = typeof(T);
                var name = Enum.GetName(enumType, type);
                var enumMemberAttribute = ((DisplayAttribute[])enumType
                    .GetField(name)
                    .GetCustomAttributes(typeof(DisplayAttribute), true))
                    .SingleOrDefault();
                var result = enumMemberAttribute?.Name;

                if (string.IsNullOrEmpty(result))
                {
                    result = CommonHelper.Replace_AndGetPrettyText(Enum.GetName(typeof(T), type));
                }

                return result;
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public static bool IsUndefinedOrNotSet<T>(this T type)
            where T : struct, IComparable, IConvertible, IFormattable
        {
            var enumName = Enum.GetName(typeof(T), type);

            return enumName == "Undefined"
                   || enumName == "NotSet"
                   || enumName == "Not_Set";
        }

        #region For EnumMemberAttribute

        public static string ToEnumString<T>(this T type)
        {
            var enumType = typeof(T);
            var name = Enum.GetName(enumType, type);
            var enumMemberAttribute = ((EnumMemberAttribute[])enumType
                .GetField(name)
                .GetCustomAttributes(typeof(EnumMemberAttribute), true))
                .SingleOrDefault();
            var result = enumMemberAttribute?.Value;

            if (string.IsNullOrEmpty(result))
            {
                result = CommonHelper.Replace_AndGetPrettyText(Enum.GetName(typeof(T), type));
            }

            return result;
        }

        public static T ToEnum<T>(this string str)
        {
            var enumType = typeof(T);
            foreach (var name in Enum.GetNames(enumType))
            {
                var enumMemberAttribute = ((EnumMemberAttribute[])enumType
                    .GetField(name)
                    .GetCustomAttributes(typeof(EnumMemberAttribute), true))
                    .Single();

                if (enumMemberAttribute.Value == str)
                {
                    return (T)Enum.Parse(enumType, name);
                }
            }

            //throw exception or whatever handling you want or
            return default(T);
        }

        #endregion

        public static List<T> ConvertStringToListEnum<T>(string stringInput) where T : struct, IConvertible
        {
            stringInput = Regex.Replace(stringInput, @"\s", "");
            string[] arrayEnum = stringInput.Split(new Char[] { ',' });
            var listEnum = new List<T>();
            foreach (string item in arrayEnum)
            {
                if (int.TryParse(item, out int n) == true)
                {
                    int enumId = Convert.ToInt32(item);
                    if (Enum.IsDefined(typeof(T), enumId))
                    {
                        if (!listEnum.Contains((T)Enum.ToObject(typeof(T), enumId)))
                            listEnum.Add((T)Enum.ToObject(typeof(T), enumId));
                    }
                }
                if (Enum.IsDefined(typeof(T), item))
                {
                    if (!listEnum.Contains((T)Enum.Parse(typeof(T), item)))
                        listEnum.Add((T)Enum.Parse(typeof(T), item));
                }
            }

            return listEnum;
        }
    }
}
