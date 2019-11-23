using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

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
        private static readonly Regex AlphaNumeralAndHyphenExpression = new Regex("^[A-Za-z0-9\\-]+$");
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
        public static string CleanAlienText(this string target)
        {
            if (IsNullOrEmpty(target)) return string.Empty;

            if ((target.Contains("#")))
            {
                target = target.Replace("#", "HASH");
            }
            if ((target.Contains("$")))
            {
                target = target.Replace("$", "DOLLAR");
            }
            if ((target.Contains("^")))
            {
                target = target.Replace("^", "CARET");
            }
            if ((target.Contains("~")))
            {
                target = target.Replace("~", "TILDE");
            }

            if ((target.Contains(";")))
            {
                target = target.Replace(";", "；");
            }
            if ((target.Contains(",")))
            {
                target = target.Replace(",", "，");
            }
            if ((target.Contains("`")))
            {
                target = target.Replace("`", "BACKQUOTE");
            }
            return target;
        }

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

            if (strList.Count() > 0)
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

        public static String[] SplitThenTrim(this string target, char separator)
        {
            if (IsNullOrEmpty(target)) return null;
            var arry = target.Split(separator);
            var result = new string[arry.Length];
            var i = 0;
            foreach (var str in arry)
            {
                var str1 = str.Trim();
                result[i] = str1;
                i++;
            }
            return result;
        }

        public static String[] SplitByLastSeparator(this string target, char separator)
        {
            if (IsNullOrEmpty(target)) return null;
            var arry = target.Split(separator);
            var result = new string[2];
            for (int i = 0; i < arry.Length - 1; i++)
            {
                if (i == 0)
                {
                    result[0] = arry[i];
                }
                else
                {
                    result[0] = result[0] + separator.ToString() + arry[i];
                }
            }
            result[1] = arry[arry.Length - 1];
            return result;
        }

        //#join
        public static string JoinByStringArray(String[] strArry, char separator)
        {
            string str = "";
            if (strArry == null) return string.Empty;
            for (int i = 0; i < strArry.Length; i++)
            {
                if (i == 0)
                {
                    str = strArry[i];
                }
                else
                {
                    str = str + separator + strArry[i];
                }
            }
            return str;
        }

        public static string AddOrDelToSeparatedStrings(this string target, string str, bool add, char separator)
        {
            if (add)
            {
                if (target.IsNullOrEmpty()) return str;
                if (str.IsNullOrEmpty()) return target;
                else
                {
                    var strArray = target.Split(separator);
                    if (strArray.Any(x => x.Equals(str)))
                    {
                        return target;
                    }
                    else
                    {
                        return (target + separator + str);
                    }
                }

            }
            else
            {
                if (target.IsNullOrEmpty()) return string.Empty;
                if (str.IsNullOrEmpty()) return target;
                else
                {
                    var strArray = target.Split(separator);
                    if (strArray.Any(x => x.Equals(str)))
                    {
                        var retStr = "";
                        var n = 0;
                        for (int i = 0; i < strArray.Length; i++)
                        {
                            if (strArray[i] == str) continue;
                            if (n == 0)
                            {
                                retStr = strArray[i];
                            }
                            else
                            {
                                retStr = retStr + separator + strArray[i];
                            }
                            n++;
                        }
                        return retStr;
                    }
                    else
                    {
                        return target;
                    }
                }
            }

        }

        //#to
        public static String ToUniqueStringByNow(this string target)
        {
            var str = DateTime.Now.ToString("yyMMddHHmmssfff");
            if (IsNullOrEmpty(target))
            {
                return str;
            }
            return target + "-" + str;
        }

        public static String ToUniqueStringByNow(this string target, string seperator)
        {
            if (IsNullOrEmpty(target)) return string.Empty;
            var str = DateTime.Now.ToString("yyMMddHHmmssfff");
            return target + seperator + str;
        }

        public static string ToLegalUrl(this string target)
        {
            if (string.IsNullOrEmpty(target))
            {
                return target;
            }

            target = target.Trim();
            target = target.Replace(".", "-");
            if (target.IndexOfAny(IllegalUrlCharacters) > -1)
            {
                foreach (char character in IllegalUrlCharacters)
                {
                    target = target.Replace(character.ToString(CultureInfo.CurrentCulture), string.Empty);
                }
            }

            target = target.Replace(" ", "-");

            while (target.Contains("--"))
            {
                target = target.Replace("--", "-");
            }

            return target;
        }

        public static string ToLegalFileName(this string target)
        {
            if (string.IsNullOrEmpty(target))
            {
                return target;
            }

            target = target.Trim();
            target = target.Replace(" ", "-");
            if (target.IndexOfAny(IllegalFileNameCharacters) > -1)
            {
                foreach (char character in IllegalFileNameCharacters)
                {
                    target = target.Replace(character.ToString(CultureInfo.CurrentCulture), "-");
                }
            }
            return target;
        }


        //#get
        public static string GetShortGuidStr()
        {
            var guid = Guid.NewGuid();
            return guid.ToBase64String();
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


        public static string GetLastSeparatedString(this string target, char separator)
        {
            if (IsNullOrEmpty(target)) return null;
            var arry = target.Split(separator);
            if (arry.Length == 1) return null;
            return arry[arry.Length - 1];
        }

        public static string GetStyleValue(this string target, string styleStr)
        {
            if (string.IsNullOrEmpty(target))
                return string.Empty;
            var val = target.ToLower().SplitByTwoDifferentStrings(styleStr.ToLower() + ":", ";", true);
            if (val != null) return val[0].Trim();
            return string.Empty;
        }

        public static char GetParamSeparator(this string target)
        {
            var separator = ';';
            if (target.Contains("^")) separator = '^';
            else if (target.Contains("~")) separator = '~';
            return separator;
        }

        public static char GetSubParamSeparator(this string target)
        {
            var separator = ',';
            if (target.Contains("`")) separator = '`';
            return separator;
        }

        //#judge
        public static bool IsBeContainedInStringArray(this string target, string[] strArray)
        {
            if (string.IsNullOrEmpty(target))
            {
                return false;
            }

            if (strArray == null)
            {
                return false;
            }

            return strArray.Any(x => x == target);
        }

        public static bool IsNullOrEmpty(this string target)
        {
            if (string.IsNullOrEmpty(target))
            {
                return true;
            }
            return false;
        }

        public static bool IsWebUrl(this string target)
        {
            return !string.IsNullOrEmpty(target) && WebUrlExpression.IsMatch(target);
            //throw new ArgumentException("\"{0}\" is not a valid web url.".FormatWith(argumentName), argumentName);
        }


        public static bool IsEmailAddress(this string target)
        {
            return !string.IsNullOrEmpty(target) && EmailExpression.IsMatch(target);
        }

        public static bool IsPassword(this string target)
        {
            if (target.Length < 7) return false;
            return !string.IsNullOrEmpty(target) && PasswordExpression.IsMatch(target);
        }

        public static bool IsPlusIntegerOrZero(this string target)
        {
            return !string.IsNullOrEmpty(target) && (PlusIntegerExpression.IsMatch(target) | target == "0");
        }

        public static bool IsPlusInteger(this string target)
        {
            return !string.IsNullOrEmpty(target) && PlusIntegerExpression.IsMatch(target);
        }

        public static bool IsNumber(this string target)
        {
            for (int i = 0; i < target.Length; i++)
            {
                if (!Char.IsNumber(target, i))
                    return false;
            }
            return true;
        }

        public static bool IsAlphaNumeralAndHyphenOrEmpty(this string target)
        {
            if (target.IsNullOrEmpty()) return true;
            return AlphaNumeralAndHyphenExpression.IsMatch(target);
        }


        public static bool ContainSubParamSeparator(this string target)
        {
            if (target.Contains("`") | target.Contains(",")) return true;
            return false;
        }


    }
}