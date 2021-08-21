using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.Script.Serialization;

namespace Tools_Injector_Mod_Menu
{
    //https://stackoverflow.com/a/6541739/8902883
    public class MySettings : AppSettings<MySettings>
    {
        public bool debugMode;
        public string txtLibName;
        public string txtToast;
        public string txtName;
        public string txtSite;
        public string txtText;
        public string txtEndCredit;
        public string ImageCode;
        public bool chkNoMenu;

        public bool chkRemoveTemp = true;
        public bool chkTFiveCredit = true;
        public bool chkSound = true;
        public bool chkCheckUpdate = true;
        public bool chkAlwaysOverwrite;
        public bool chkMergeApk;
        public bool chkOpenOutput;
        public bool chkRemoveOther;

        public string txtNDK;
        public int menuStyle;
        public int apkTools;

        public string txtService = @"<service android:enabled=""true"" android:exported=""false"" android:name=""com.tfive.modmenu.FloatingModMenuService"" android:stopWithTask=""true""/>";
        public string txtOnCreate = "invoke-static {p0}, Lcom/tfive/MainActivity;->Start(Landroid/content/Context;)V";

        public string txtActionMain = @"<activity android:configChanges=""keyboardHidden|orientation|screenSize"" android:name=""com.tfivel.MainActivity"">
     <intent-filter>
         <action android:name=""android.intent.action.MAIN""/>
         <category android:name=""android.intent.category.LAUNCHER""/>
     </intent-filter>
</activity>
";
    }

    public class AppSettings<T> where T : new()
    {
        private const string DefaultFilename = "settings.json";

        public void Save(string fileName = DefaultFilename)
        {
            var pretty = Process(new JavaScriptSerializer().Serialize(this));
            File.WriteAllText(fileName, pretty);
        }

        public static void Save(T pSettings, string fileName = DefaultFilename)
        {
            File.WriteAllText(fileName, new JavaScriptSerializer().Serialize(pSettings));
        }

        public static T Load(string fileName = DefaultFilename)
        {
            var t = new T();
            if (File.Exists(fileName))
                t = new JavaScriptSerializer().Deserialize<T>(File.ReadAllText(fileName));
            return t;
        }

        //https://stackoverflow.com/a/23828858/8902883
        private static string Process(string inputText)
        {
            var escaped = false;
            var inQuotes = false;
            var column = 0;
            var indentation = 0;
            var indentations = new Stack<int>();
            const int tabbing = 8;
            var sb = new StringBuilder();
            inputText = "{\n " + inputText.Remove(0, 1);
            inputText = inputText.Remove(inputText.Length - 1, 1) + "\n}";
            foreach (var x in inputText)
            {
                sb.Append(x);
                column++;
                if (escaped)
                {
                    escaped = false;
                }
                else
                {
                    switch (x)
                    {
                        case '\\':
                            escaped = true;
                            break;

                        case '\"':
                            inQuotes = !inQuotes;
                            break;

                        default:
                            {
                                if (!inQuotes)
                                {
                                    if (x == ',')
                                    {
                                        // if we see a comma, go to next line, and indent to the same depth
                                        sb.Append("\r\n");
                                        column = 0;
                                        for (var i = 0; i < indentation; i++)
                                        {
                                            sb.Append(" ");
                                            column++;
                                        }
                                    }
                                    else if (x == '[' || x == '{')
                                    {
                                        // if we open a bracket or brace, indent further (push on stack)
                                        indentations.Push(indentation);
                                        indentation = column;
                                    }
                                    else if (x == ']' || x == '}')
                                    {
                                        // if we close a bracket or brace, undo one level of indent (pop)
                                        indentation = indentations.Pop();
                                    }
                                    else if (x == ':')
                                    {
                                        // if we see a colon, add spaces until we get to the next
                                        // tab stop, but without using tab characters!
                                        while (column % tabbing != 0)
                                        {
                                            sb.Append(' ');
                                            column++;
                                        }
                                    }
                                }

                                break;
                            }
                    }
                }
            }
            return sb.ToString();
        }
    }
}