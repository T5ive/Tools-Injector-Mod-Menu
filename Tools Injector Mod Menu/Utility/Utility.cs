using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
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
            if (str == null || string.IsNullOrWhiteSpace(str))
            {
                if (!msg) return true;

                MyMessage.MsgShowWarning($"{nameof(str)} is Empty, Please Check it again!!!");
                return true;
            }
            return false;
        }

        public static bool IsEmpty(object str, int line, string name = "", bool msg = true)
        {
            if (str == null || string.IsNullOrWhiteSpace(str.ToString()))
            {
                if (!msg) return true;
                MyMessage.MsgShowWarning($"{name} At {line} is Empty, Please Check it again!!!");
                return true;
            }
            return false;
        }

        public static bool IsEmpty(object str)
        {
            if (str == null || string.IsNullOrWhiteSpace(str.ToString()))
            {
                return true;
            }
            return false;
        }

        public static bool IsEmpty(DataGridView dataGrid, bool msg = true)
        {
            if (dataGrid.Rows.Count == 0)
            {
                if (!msg) return true;

                MyMessage.MsgShowWarning($"{nameof(dataGrid)} is Empty, Please Check it again!!!");
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
            foreach (var c in str.Where(c => c is >= '0' and <= '9' || c is >= 'A' and <= 'Z' || c is >= 'a' and <= 'z' || c == '.' || c == '_'))
            {
                sb.Append(c);
            }
            return sb.ToString();
        }

        public static string RemoveMiniSpecialCharacters(this string str)
        {
            var sb = new StringBuilder();
            foreach (var c in str.Where(c => c is >= '0' and <= '9' || c is >= 'A' and <= 'Z' || c is >= 'a' and <= 'z' || c == '.' || c == ',' || c == '_'))
            {
                sb.Append(c);
            }
            return sb.ToString();
        }

        public static string RemoveSuperSpecialCharacters(this string str)
        {
            var sb = new StringBuilder();
            foreach (var c in str.Where(c => c is >= '0' and <= '9' || c is >= 'A' and <= 'Z' || c is >= 'a' and <= 'z'))
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

        public static string GetBetween(string strSource, string strStart, string strEnd)
        {
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                var start = strSource.IndexOf(strStart, 0, StringComparison.Ordinal) + strStart.Length;
                var end = strSource.IndexOf(strEnd, start, StringComparison.Ordinal);
                return strSource.Substring(start, end - start);
            }
            return null;
        }

        public static string SmaliCountToName(int count, bool move = false)
        {
            if (move)
            {
                return $"smali_classes{count + 1}";
            }

            return count == 1 ? "smali" : $"smali_classes{count}";
        }

        public static string GetApkName(string apk)
        {
            try
            {
                var split = apk.Split('\\');
                return split[split.Length - 1];
            }
            catch
            {
                return null;
            }
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

        public static string FunctionTypeToString(Enums.FunctionType type)
        {
            return type switch
            {
                Enums.FunctionType.Category => "Category",
                Enums.FunctionType.HookButton => "Hook: Button",
                Enums.FunctionType.HookButtonOnOf => "Hook: Button On/Of",
                Enums.FunctionType.HookInputButton => "Hook: Input Button",
                Enums.FunctionType.HookInputOnOff => "Hook: Input OnOff",
                Enums.FunctionType.HookInputValue => "Hook: Input Value",
                Enums.FunctionType.HookSeekBar => "Hook: SeekBar",
                Enums.FunctionType.HookSeekBarToggle => "Hook: SeekBar Toggle",
                Enums.FunctionType.HookToggle => "Hook: Toggle",
                Enums.FunctionType.PatchButtonOnOff => "Patch: Button On/Off",
                Enums.FunctionType.PatchLabel => "Patch: Label",
                Enums.FunctionType.PatchToggle => "Patch: Toggle",
                _ => "Empty"
            };
        }

        public static Enums.FunctionType StringToFunctionType(string str)
        {
            return str switch
            {
                "Category" => Enums.FunctionType.Category,
                "Hook: Button" => Enums.FunctionType.HookButton,
                "Hook: Button On/Of" => Enums.FunctionType.HookButtonOnOf,
                "Hook: Input Button" => Enums.FunctionType.HookInputButton,
                "Hook: Input OnOff" => Enums.FunctionType.HookInputOnOff,
                "Hook: Input Value" => Enums.FunctionType.HookInputValue,
                "Hook: SeekBar" => Enums.FunctionType.HookSeekBar,
                "Hook: SeekBar Toggle" => Enums.FunctionType.HookSeekBarToggle,
                "Hook: Toggle" => Enums.FunctionType.HookToggle,
                "Patch: Button On/Off" => Enums.FunctionType.PatchButtonOnOff,
                "Patch: Label" => Enums.FunctionType.PatchLabel,
                "Patch: Toggle" => Enums.FunctionType.PatchToggle,
                _ => Enums.FunctionType.Empty
            };
        }

        public static string FunctionTypeToStringFeatures(Enums.FunctionType type)
        {
            return type switch
            {
                Enums.FunctionType.Category => "Category",
                Enums.FunctionType.HookButton => "Button",
                Enums.FunctionType.HookButtonOnOf => "ButtonOnOff",
                Enums.FunctionType.HookInputButton => "InputButton",
                Enums.FunctionType.HookInputOnOff => "InputOnOff",
                Enums.FunctionType.HookInputValue => "InputValue",
                Enums.FunctionType.HookSeekBar => "SeekBar",
                Enums.FunctionType.HookSeekBarToggle => "SeekBarSwitch",
                Enums.FunctionType.HookToggle => "Toggle",
                Enums.FunctionType.PatchButtonOnOff => "ButtonOnOff",
                Enums.FunctionType.PatchLabel => "RichTextView",
                Enums.FunctionType.PatchToggle => "Toggle",
                _ => "Empty"
            };
        }

        public static Enums.Type StringToType(string str)
        {
            return str switch
            {
                "bool" => Enums.Type.Bool,
                "double" => Enums.Type.Double,
                "float" => Enums.Type.Float,
                "int" => Enums.Type.Int,
                "long" => Enums.Type.Long,
                "void" => Enums.Type.Void,
                "links" => Enums.Type.Links,
                _ => Enums.Type.Empty
            };
        }

        public static string TypeToString(Enums.Type type)
        {
            return type switch
            {
                Enums.Type.Bool => "bool",
                Enums.Type.Double => "double",
                Enums.Type.Float => "float",
                Enums.Type.Int => "int",
                Enums.Type.Long => "long",
                Enums.Type.Void => "void",
                Enums.Type.Links => "links",
                _ => null
            };
        }

        public static string TypeToStringEnd(Enums.Type type)
        {
            return type switch
            {
                Enums.Type.Bool => ";",
                Enums.Type.Double => "= 1;",
                Enums.Type.Float => "= 1;",
                Enums.Type.Int => "= 1;",
                Enums.Type.Long => "= 1;",
                Enums.Type.Void => ";",
                Enums.Type.Links => "",
                _ => null
            };
        }
    }
}