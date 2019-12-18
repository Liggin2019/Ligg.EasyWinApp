using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Ligg.Base.Extension
{
    public static class StringExtension
    {
        //#regex
        private static readonly Regex WebUrlExpression = new Regex(@"(http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.Singleline | RegexOptions.Compiled);
        private static readonly Regex EmailExpression = new Regex(@"^([0-9a-zA-Z]+[-._+&])*[0-9a-zA-Z]+@([-0-9a-zA-Z]+[.])+[a-zA-Z]{2,6}$", RegexOptions.Singleline | RegexOptions.Compiled);
        //必须字母、数字，特殊字符三者具备
        private static readonly Regex PasswordExpression = new Regex(@"^(?:(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])|(?=.*[A-Z])(?=.*[a-z])(?=.*[^A-Za-z0-9])|(?=.*[A-Z])(?=.*[0-9])(?=.*[^A-Za-z0-9])|(?=.*[a-z])(?=.*[0-9])(?=.*[^A-Za-z0-9])).+", RegexOptions.Singleline | RegexOptions.Compiled);
        //必须字母、数字，特殊字符二者具备 has issue
        //private static readonly Regex PasswordExpression = new Regex(@"(?!^\\d+$)(?!^[a-zA-Z]+$)(?!^[_#@]+$).{7,}", RegexOptions.Singleline | RegexOptions.Compiled);
        private static readonly Regex StripHtmlExpression = new Regex("<\\S[^><]*>", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.CultureInvariant | RegexOptions.Compiled);
        private static readonly Regex PlusIntegerExpression = new Regex("^[0-9]*[1-9][0-9]*$");
        private static readonly Regex AlphaAndNumeralExpressiown = new Regex("^[A-Za-z0-9]+$");
        private static readonly Regex AlphaNumeralUnderscoreHyphenAndChineseExpression = new Regex("^[A-Za-z0-9_\\-\u4e00-\u9fa5]+$");
        private static readonly Regex AlphaNumeralAndHyphenExpression = new Regex("^[A-Za-z0-9\\-]+$");
        private static readonly Regex ChineseExpression = new Regex("^[\u4e00-\u9fa5]+$ ");
        private static readonly char[] IllegalUrlCharacters = new[] { ';', '/', '\\', '?', ':', '@', '&', '=', '+', '$', ',', '<', '>', '#', '%', '.', '!', '*', '\'', '"', '(', ')', '[', ']', '{', '}', '|', '^', '`', '~', '–', '‘', '’', '“', '”', '»', '«' };
        private static readonly char[] IllegalFileNameCharacters = new[] { '<', '>', '/', '\\', '?', ':', '|', '*', '"' };

        //#common
        public static string FormatWith(this string target, params object[] args)
        {
            if (IsNullOrEmpty(target)) return string.Empty;
            return string.Format(CultureInfo.CurrentCulture, target, args);
        }


        public static string AddCharTillLength(this string target, int len, char addedChar)
        {
            if (target.Length < len)
            {
                target = addedChar + target;
            }
            if (target.Length == len)
            {
                return target;
            }
            target = AddCharTillLength(target, len, addedChar);
            return target;
        }

        //#clean

        //#split
        public static String[] SplitByTwoDifferentStrings(this string target, string seperator1, string seperator2, bool ignoreLastSeperator)
        {
            if (IsNullOrEmpty(target)) return null;
            if (ignoreLastSeperator)
            {
                if (!target.Contains(seperator1)) return null;

            }
            else
            {
                if (!target.Contains(seperator1) || !target.Contains(seperator2)) return null;
            }

            var strArry = target.SplitByString(seperator2);
            var strList = new List<string>();
            for (int i = 0; i < strArry.Length; i++)
            {
                if (!ignoreLastSeperator)
                {
                    if (i == strArry.Length - 1) break;
                }

                var tmpStr = strArry[i];
                if (tmpStr.Contains(seperator1))
                {
                    var qty = GetQtyOfIncludedString(tmpStr, seperator1);
                    var n = tmpStr.IndexOf(seperator1, qty - 1);
                    var subStr = tmpStr.Substring(n + seperator1.Length, tmpStr.Length - seperator1.Length - n);
                    if (!string.IsNullOrEmpty(subStr))
                    {
                        //strList.Add(subStr);
                    }
                    strList.Add(subStr);
                }
            }

            if (strList.Count > 0)
            {
                var result = new String[strList.Count()];
                for (int i = 0; i < strList.Count; i++)
                {
                    result[i] = strList[i];
                }
                return result;
            }
            return null;
        }

        public static String[] SplitByString(this string target, string seperator)
        {
            if (IsNullOrEmpty(target)) return null;
            //if (!target.Contains(seperator)) return null;
            var qty = GetQtyOfIncludedString(target, seperator);
            var result = new String[qty + 1];
            var str = target;
            for (int i = 1; i <= qty + 1; i++)
            {
                if (i != (qty + 1))
                {
                    var n = str.IndexOf(seperator, 0);
                    var subStr = str.Substring(0, n);
                    result[i - 1] = !string.IsNullOrEmpty(subStr) ? subStr : "";
                    str = str.Substring(n + seperator.Length, str.Length - n - seperator.Length);
                }
                else
                {
                    result[i - 1] = !string.IsNullOrEmpty(str) ? str : "";

                }
            }
            return result;
        }


        //#to
        public static List<T> ConvertIdsStringToIntegerList<T>(this string target, char separator)
        {
            var IntegerList = new List<T>();
            var idStrArray = target.Split(separator);
            if (!(idStrArray.Length == 1 && idStrArray[0] == ""))
            {
                if (idStrArray.Length > 0)
                {
                    for (var i = 0; i < idStrArray.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(idStrArray[i]))
                        {
                            var integer = (T)Convert.ChangeType(idStrArray[i], typeof(T));
                            IntegerList.Add(integer);
                        }
                    }
                }
            }
            return IntegerList;
        }



        public static Object ConvertToAnyType(this String str, Type type, char spacingChar, char lineBreakChar)
        {
            if (String.IsNullOrEmpty(str))
                return null;
            if (type == null)
                return str;
            if (type.IsArray)
            {
                Type elementType = type.GetElementType();
                String[] strs = str.Split(new char[] { lineBreakChar });
                Array array = Array.CreateInstance(elementType, strs.Length);
                for (int i = 0, c = strs.Length; i < c; ++i)
                {
                    var tempStr = strs[i].Replace(spacingChar.ToString(), " ");
                    array.SetValue(ConvertToAnySimpleType(tempStr, elementType), i);
                }
                return array;
            }
            return ConvertToAnySimpleType(str, type);
        }

        private static object ConvertToAnySimpleType(object value, Type type)
        {
            object returnValue;
            if ((value == null) || type.IsInstanceOfType(value))
            {
                return value;
            }
            var str = value as string;
            if ((str != null) && (str.Length == 0))
            {
                return null;
            }

            TypeConverter converter = TypeDescriptor.GetConverter(type);
            bool flag = converter.CanConvertFrom(value.GetType());
            if (!flag)
            {
                converter = TypeDescriptor.GetConverter(value.GetType());
            }
            if (!flag && !converter.CanConvertTo(type))
            {
                throw new InvalidOperationException("Can't Convert Type：" + value.ToString() + "==>" + type);
            }
            try
            {
                returnValue = flag ? converter.ConvertFrom(null, null, value) : converter.ConvertTo(null, null, value, type);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Convert Type：" + value.ToString() + "==>" + type, e);
            }
            return returnValue;
        }


        public static int GetQtyOfIncludedChar(this string target, char incChar)
        {
            int count = 0;
            for (int i = 0; i < target.Length; i++)
            {
                if (target[i] == incChar)
                {
                    count++;
                }
            }
            return count;
        }

        public static int GetQtyOfIncludedString(this string target, string incStr)
        {
            var schStr = incStr.Trim();
            if (string.IsNullOrEmpty(schStr)) return 0;
            if (string.IsNullOrEmpty(target)) return 0;
            int schStrLen = schStr.Length;
            var targetLen = target.Length;
            int index = 0;
            int pos = 0;
            int count = 0;
            do
            {
                index = target.IndexOf(schStr, pos);
                if (index != -1)
                {
                    count++;
                }
                pos = schStrLen + index;
            } while (index != -1 && pos + schStrLen < targetLen + 1);
            return count;
        }

        public static char GetSubParamSeparator(this string target)
        {
            var separator = ',';
            if (target.Contains("`")) separator = '`';
            return separator;
        }

        //#judge
        public static bool IsNullOrEmpty(this string target)
        {
            if (string.IsNullOrEmpty(target))
            {
                return true;
            }
            return false;
        }

        public static bool IsIntersectingAfterSplitWithSeparator(this string target, string str, char separator)
        {
            if (string.IsNullOrEmpty(target))
            {
                return false;
            }
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            var idStrArray1 = target.Split(separator);
            var idStrArray2 = str.Split(separator);
            return idStrArray2.Any(t2 => idStrArray1.Any(t1 => t2 == t1));
        }


        public static bool IsPassword(this string target)
        {
            if (target.Length < 7) return false;
            return !string.IsNullOrEmpty(target) && PasswordExpression.IsMatch(target);
        }


        public static bool IsPlusInteger(this string target)
        {
            return !string.IsNullOrEmpty(target) && PlusIntegerExpression.IsMatch(target);
        }


    }
}