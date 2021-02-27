using MaterialSkin.Controls;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Tools_Injector_Mod_Menu.Patch_Manager;

namespace Tools_Injector_Mod_Menu
{
    public static class Utility
    {
        public static void CheckFolder(string folderName)
        {
            var path = FrmMain.AppPath + "\\" + folderName;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public static bool CheckFiles(string fileName)
        {
            return File.Exists(FrmMain.AppPath + "\\" + fileName);
        }

        public static bool CheckFiles(string folderName, string fileName)
        {
            return File.Exists(FrmMain.AppPath + "\\" + folderName + "\\" + fileName);
        }

        public static bool IsEqual(List<string> str1, List<string> str2)
        {
            return !str1.Where((t, i) => !t.Equals(str2[i])).Any();
        }

        public static bool IsEqual(string[] str1, string[] str2)
        {
            return !str1.Where((t, i) => !t.Equals(str2[i])).Any();
        }

        public static bool IsDuplicate(OffsetInfo offsetInfo, List<OffsetInfo> offsetList)
        {
            if (offsetList.Count != 0)
            {
                foreach (var offset in offsetList)
                {
                    if (offsetInfo.Offset == offset.Offset)
                    {
                        MyMessage.MsgShowWarning(@"Found Duplicate " + offsetInfo.Offset + @", Please Check it again before adding a offset!!!");
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool IsDuplicateName(string nameCheat, List<FunctionList> functionList)
        {
            if (functionList.Count != 0)
            {
                foreach (var result in functionList)
                {
                    if (nameCheat == result.CheatName)
                    {
                        MyMessage.MsgShowWarning(@"Found Duplicate " + nameCheat + @", Please Check it again before adding a offset!!!");
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool IsEmpty(MaterialTextBox textBox, bool msg = true)
        {
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                if (!msg) return true;

                MyMessage.MsgShowWarning($@"{textBox.Name} is Empty, Please Check it again!!!");
                return true;
            }

            return false;
        }

        public static bool IsEmpty(string str, bool msg = true)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                if (!msg) return true;

                MyMessage.MsgShowWarning($"{nameof(str)} is Empty, Please Check it again!!!");
                return true;
            }
            return false;
        }

        public static bool IsEmpty(List<OffsetInfo> offsetList)
        {
            return offsetList.Count == 0;
        }

        public static bool IsEmpty(List<FunctionList> functionList)
        {
            return functionList.Count == 0;
        }

        public static bool IsEmpty(List<(string, string)> methodList)
        {
            return methodList.Count == 0;
        }

        public static string InsertSpaces(string str)
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

        public static bool IsEmpty(Dictionary<int, OffsetInfo> offsetDict)
        {
            return offsetDict.Count == 0;
        }

        public static bool IsEmpty(Dictionary<string, Dictionary<int, OffsetInfo>> functionDict)
        {
            return functionDict.Count == 0;
        }

        public static bool IsDuplicate(Dictionary<int, OffsetInfo> offsetDict, OffsetInfo offsetInfo)
        {
            if (offsetDict.Count != 0)
            {
                foreach (var offset in offsetDict.Values)
                {
                    if (offsetInfo.Offset == offset.Offset)
                    {
                        MyMessage.MsgShowWarning(@"Found Duplicate " + offsetInfo.Offset + @", Please Check it again before adding a offset!!!");
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool IsDuplicate(Dictionary<string, Dictionary<int, OffsetInfo>> functionDict, OffsetInfo offsetInfo)
        {
            if (functionDict.Count != 0)
            {
                foreach (var offsetDick in functionDict.Values)
                {
                    foreach (var offset in offsetDick.Values)
                    {
                        if (offsetInfo.Offset == offset.Offset)
                        {
                            MyMessage.MsgShowWarning(@"Found Duplicate " + offsetInfo.Offset + @", Please Check it again before adding a offset!!!");
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static bool IsDuplicateName(Dictionary<string, Dictionary<int, OffsetInfo>> functionDict, string nameCheat)
        {
            if (functionDict.Count != 0)
            {
                foreach (var offsetDict in functionDict.Keys)
                {
                    if (nameCheat == offsetDict)
                    {
                        MyMessage.MsgShowWarning(@"Found Duplicate " + nameCheat + @", Please Check it again before adding a offset!!!");
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool IsSpecialChar(this string input)
        {
            var specialChar = @"\|!#$%&/()=?»«@£§€{}.-;'<>_,";
            foreach (var item in specialChar)
            {
                if (input.Contains(item)) return true;
            }
            return false;
        }

        public static bool IsPathSpecialChar(this string input)
        {
            const string specialChar = @"|!#$%&/()=?»«@£§€{};'<>_, ";
            return specialChar.Any(input.Contains);
        }

        public static string RemoveSpecialCharacters(this string str)
        {
            var sb = new StringBuilder();
            foreach (var c in str.Where(c => c >= '0' && c <= '9' || c >= 'A' && c <= 'Z' || c >= 'a' && c <= 'z' || c == '.' || c == '_'))
            {
                sb.Append(c);
            }
            return sb.ToString();
        }

        public static string RemoveMiniSpecialCharacters(this string str)
        {
            var sb = new StringBuilder();
            foreach (var c in str.Where(c => c >= '0' && c <= '9' || c >= 'A' && c <= 'Z' || c >= 'a' && c <= 'z' || c == '.' || c == ',' || c == '_'))
            {
                sb.Append(c);
            }
            return sb.ToString();
        }

        public static string RemoveSuperSpecialCharacters(this string str)
        {
            var sb = new StringBuilder();
            foreach (var c in str.Where(c => c >= '0' && c <= '9' || c >= 'A' && c <= 'Z' || c >= 'a' && c <= 'z'))
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
    }
}