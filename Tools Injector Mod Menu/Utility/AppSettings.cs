using System.IO;
using System.Web.Script.Serialization;

namespace Tools_Injector_Mod_Menu
{
    //https://stackoverflow.com/a/6541739/8902883
    public class MySettings : AppSettings<MySettings>
    {
        public string txtLibName;
        public string txtToast;
        public string txtName;
        public string txtSite;
        public string txtText;
        public string txtEndCredit;
        public string ImageCode;
        public string txtNDK;

        public int menuStyle;

        public bool chkRemoveTemp = true;
        public bool chkTFiveCredit = true;
        public bool chkLogsComplie = true;
        public bool chkLogsSuccess = true;
        public bool chkLogsError = true;
        public bool chkSound = true;

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
            File.WriteAllText(fileName, new JavaScriptSerializer().Serialize(this));
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
    }
}