using MaterialSkin.Controls;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Tools_Injector_Mod_Menu.Patch_Manager;

namespace Tools_Injector_Mod_Menu
{
    public static class Checker
    {
        #region Check Directory & File

        public static void CheckFolder(string folderName, bool appPath = true)
        {
            var path = appPath ? FrmMain.AppPath + "\\" + folderName : folderName;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public static bool CheckFiles(string folderName, string fileName) => File.Exists(FrmMain.AppPath + "\\" + folderName + "\\" + fileName);

        #endregion Check Directory & File

        #region Check SpecialChar

        public static bool IsSpecialChar(this string input) => @"\|!#$%&/()=?»«@£§€{}.-;'<>_,".Any(input.Contains);

        public static bool IsPathSpecialChar(this string input) => "|!#$%&/()=?»«@£§€{};'<>_, ".Any(input.Contains);

        #endregion Check SpecialChar

        #region Check Equal & Duplicate

        public static bool IsEqual(this List<string> str1, List<string> str2) => !str1.Where((t, i) => !t.Equals(str2[i])).Any();

        public static bool IsEqual(this string[] str1, string[] str2) => !str1.Where((t, i) => !t.Equals(str2[i])).Any();

        public static bool IsDuplicate(this OffsetInfo offsetInfo, List<OffsetInfo> offsetList)
        {
            if (offsetList.Count != 0 && offsetList.Any(offset => offsetInfo.Offset == offset.Offset))
            {
                MyMessage.MsgShowWarning("Found Duplicate " + offsetInfo.Offset + @", Please Check it again before adding a offset!!!");
                return true;
            }
            return false;
        }

        public static bool IsDuplicate(this Dictionary<int, OffsetInfo> offsetDict, OffsetInfo offsetInfo)
        {
            if (offsetDict.Count != 0 && offsetDict.Values.Any(offset => offsetInfo.Offset == offset.Offset))
            {
                MyMessage.MsgShowWarning("Found Duplicate " + offsetInfo.Offset + ", Please Check it again before adding a offset!!!");
                return true;
            }
            return false;
        }

        public static bool IsDuplicate(this Dictionary<string, Dictionary<int, OffsetInfo>> functionDict, OffsetInfo offsetInfo)
        {
            if (functionDict.Count != 0 && functionDict.Values.Any(offsetDick => offsetDick.Values.Any(offset => offsetInfo.Offset == offset.Offset)))
            {
                MyMessage.MsgShowWarning("Found Duplicate " + offsetInfo.Offset + ", Please Check it again before adding a offset!!!");
                return true;
            }
            return false;
        }

        public static bool IsDuplicateName(this string nameCheat, List<FunctionList> functionList)
        {
            if (functionList.Count != 0 && functionList.Any(result => nameCheat == result.CheatName))
            {
                MyMessage.MsgShowWarning("Found Duplicate " + nameCheat + ", Please Check it again before adding a offset!!!");
                return true;
            }
            return false;
        }

        public static bool IsDuplicateName(this Dictionary<string, Dictionary<int, OffsetInfo>> functionDict, string nameCheat)
        {
            if (functionDict.Count != 0 && functionDict.Keys.Any(offsetDict => nameCheat == offsetDict))
            {
                MyMessage.MsgShowWarning("Found Duplicate " + nameCheat + ", Please Check it again before adding a offset!!!");
                return true;
            }
            return false;
        }

        #endregion Check Equal & Duplicate

        #region Check Empty

        public static bool IsEmpty(this object str)
        {
            return str == null || string.IsNullOrWhiteSpace(str.ToString());
        }

        public static bool IsEmpty(this object str, string message)
        {
            if (str == null || string.IsNullOrWhiteSpace(str.ToString()))
            {
                MyMessage.MsgShowWarning($"{message} is Empty, Please Check it again!!!");
                return true;
            }
            return false;
        }

        public static bool IsEmpty(this MaterialTextBox textBox)
        {
            return string.IsNullOrWhiteSpace(textBox.Text);
        }

        public static bool IsEmpty(this MaterialTextBox textBox, string message)
        {
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                MyMessage.MsgShowWarning($"{message} is Empty, Please Check it again!!!");
                return true;
            }

            return false;
        }

        public static bool IsEmpty(this object str, int line, string name = "", bool msg = true)
        {
            if (str == null || string.IsNullOrWhiteSpace(str.ToString()))
            {
                if (!msg) return true;
                MyMessage.MsgShowWarning($"{name} At {line} is Empty, Please Check it again!!!");
                return true;
            }
            return false;
        }

        public static bool IsEmpty(this DataGridView dataGrid, bool msg = true)
        {
            if (dataGrid.Rows.Count == 0)
            {
                if (!msg) return true;

                MyMessage.MsgShowWarning($"{dataGrid.Name} is Empty, Please Check it again!!!");
                return true;
            }
            return false;
        }

        public static bool IsEmpty(List<OffsetInfo> offsetList) => offsetList.Count == 0;

        public static bool IsEmpty(List<FunctionList> functionList) => functionList.Count == 0;

        public static bool IsEmpty(List<(string, string)> methodList) => methodList.Count == 0;

        public static bool IsEmpty(this Dictionary<int, OffsetInfo> offsetDict) => offsetDict.Count == 0;

        public static bool IsEmpty(this Dictionary<string, Dictionary<int, OffsetInfo>> functionDict) => functionDict.Count == 0;

        #endregion Check Empty
    }
}