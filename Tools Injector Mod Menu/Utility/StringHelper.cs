using System;
using System.Linq;
using System.Text;

namespace Tools_Injector_Mod_Menu
{
    public static class StringHelper
    {
        public static string InsertSpaces(this string str)
        {
            if (!str.Contains(' '))
            {
                for (var i = 2; i <= str.Length; i += 2)
                {
                    str = str.Insert(i, " ");
                    str = str.TrimEnd(' ');
                    i++;
                }
            }
            return str;
        }

        public static string RemoveSpecialCharacters(this string str)
        {
            var sb = new StringBuilder();
            foreach (var c in str.Where(c => c is >= '0' and <= '9' or >= 'A' and <= 'Z' or >= 'a' and <= 'z' or '.' or '_' or ' '))
            {
                sb.Append(c);
            }
            return sb.ToString();
        }

        public static string RemoveMiniSpecialCharacters(this string str)
        {
            var sb = new StringBuilder();
            foreach (var c in str.Where(c => c is >= '0' and <= '9' or >= 'A' and <= 'Z' or >= 'a' and <= 'z' or '.' or ',' or '_'))
            {
                sb.Append(c);
            }
            return sb.ToString();
        }

        public static string RemoveSuperSpecialCharacters(this string str)
        {
            var sb = new StringBuilder();
            foreach (var c in str.Where(c => c is >= '0' and <= '9' or >= 'A' and <= 'Z' or >= 'a' and <= 'z'))
            {
                sb.Append(c);
            }
            return sb.ToString();
        }

        public static string ReplaceNumCharacters(this string str)
        {
            return str.Replace("0", "Zero").Replace("1", "One").Replace("2", "Two").Replace("3", "Three")
                .Replace("4", "Four").Replace("5", "Five").Replace("6", "Six").Replace("7", "Seven")
                .Replace("8", "Eight").Replace("9", "Nine").Replace("-", "Dash").Replace(".", "Dot").Replace(",", "Comma");
        }

        public static string GetBetween(this string strSource, string strStart, string strEnd)
        {
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                var start = strSource.IndexOf(strStart, 0, StringComparison.Ordinal) + strStart.Length;
                var end = strSource.IndexOf(strEnd, start, StringComparison.Ordinal);
                return strSource.Substring(start, end - start);
            }
            return null;
        }

        public static string ReplaceFirst(this string text, string oldValue, string newValue)
        {
            var pos = text.IndexOf(oldValue, StringComparison.Ordinal);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + newValue + text.Substring(pos + oldValue.Length);
        }
    }
}