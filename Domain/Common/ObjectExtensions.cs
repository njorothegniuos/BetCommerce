﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Domain.Common
{
    public static class ObjectExtensions
    {
        public static bool IsNull(this object obj)
        {
            return obj == null;
        }

        public static bool IsNotNull(this object obj)
        {
            return obj != null; //is{}
        }

        /// <summary>
        /// Uppercase the first letter in the string.
        /// </summary>
        /// <param name="value">your string to cast</param>
        /// <returns></returns>
        public static string UppercaseFirstLetter(this string value)
        {
            value ??= string.Empty;
            char[] array = value.Trim().ToCharArray();
            array[0] = char.ToUpper(array[0]);
            return new string(array);
        }

        /// <summary>
        /// Uppercase the first letter of each word in the string.
        /// </summary>
        /// <param name="value">string to cast</param>
        /// <returns></returns>
        //public static string ToTitleCase(this string value)
        //{
        //    value ??= string.Empty;
        //    char[] array = value.Trim().ToCharArray();
        //    bool newWord = true;
        //    for (int i = 0; i < array.Length; i++)
        //    {
        //        if (newWord) { array[i] = char.ToUpper(array[i]); newWord = false; } else { array[i] = char.ToLower(array[i]); }
        //        if (array[i] == ' ') newWord = true;
        //    }
        //    return new string(array);
        //}

        /// <summary>
        /// Return the int's ordinal extension.
        /// </summary>
        /// <param name="value">number to evaluate</param>
        /// <returns></returns>
        public static string ToOrdinal(this int value)
        {
            if (value <= 0) return value.ToString();

            // Start with the most common extension.
            string extension = "th";

            // Examine the last 2 digits.
            int lastDigits = value % 100;

            if (lastDigits < 11 || lastDigits > 13)
            {
                // Check the last digit.
                switch (lastDigits % 10)
                {
                    case 1:
                        extension = "st";
                        break;
                    case 2:
                        extension = "nd";
                        break;
                    case 3:
                        extension = "rd";
                        break;
                }
            }
            return value + extension;
        }

        public static string ToDigits(this string value)
        {
            value ??= string.Empty;
            return new string(value.Where(char.IsDigit).ToArray());
        }

        /// <summary>
        /// Returns new value after truncating incoming string of a specified length
        /// </summary>
        /// <param name="value"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static string TruncateString(this string value, int maxLength)
        {
            value ??= string.Empty;
            return value.Substring(0, Math.Min(value.Length, maxLength));
        }

        public static long ToEpoch(this DateTime value)
        {
            return new DateTimeOffset(value).ToUnixTimeSeconds();
        }

        public static DateTime FromEpoch(this long value)
        {
            return DateTimeOffset.FromUnixTimeSeconds(value).DateTime.ToLocalTime();
        }

        public static IEnumerable<string> Validate(object value)
        {
            if (value.IsNull())
            {
                yield return "Please provide a valid object for this request";
            }

            Type type = value.GetType();
            PropertyInfo[] properties = type.GetProperties();
            Type attrType = typeof(ValidationAttribute);

            foreach (PropertyInfo propertyInfo in properties)
            {
                object[] customAttributes = propertyInfo.GetCustomAttributes(attrType, inherit: true);

                foreach (object customAttribute in customAttributes)
                {
                    ValidationAttribute validationAttribute = (ValidationAttribute)customAttribute;

                    bool isValid = validationAttribute.IsValid(propertyInfo.GetValue(value, BindingFlags.GetProperty, null, null, null));

                    if (!isValid)
                    {
                        yield return validationAttribute.ErrorMessage;
                    }
                }
            }
        }

        public static string ValidateModel(object value)
        {
            if (value.IsNull())
            {
                return "Please provide a valid object for this request";
            }

            return string.Join(" >> ", TypeDescriptor.GetProperties(value.GetType()).Cast<PropertyDescriptor>()
                .SelectMany(pd => pd.Attributes.OfType<ValidationAttribute>().Where(va => !va.IsValid(pd.GetValue(value))))
                .Select(xx => xx.ErrorMessage));
        }

        public static string ToParsedDate(this DateTime value, string dateFormat)
        {
            TimeZoneInfo currentTimeZone = TimeZoneInfo.FindSystemTimeZoneById("E. Africa Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(value, currentTimeZone).ToString(dateFormat);
        }

        public static string ToFormattedDate(this DateTime value)
        {
            return $"{value:d/M/yyyy}";
        }

        public static string ToFormattedTime(this DateTime value)
        {
            return $"{value:h:mm:ss tt}";
        }

        public static double ToShillings(this long value)
        {
            return value * 0.01;
        }

        public static long ToShareCents(this decimal value)
        {
            if (value % 1 == 0) return Convert.ToInt64(value);
            string stringfiedValue = value.ToString();
            double tenthValue = char.GetNumericValue(stringfiedValue[stringfiedValue.IndexOf('.') + 1]);
            value = Math.Truncate(value);
            if (tenthValue > 5) value++;
            return Convert.ToInt64(value);
        }
    }
}
