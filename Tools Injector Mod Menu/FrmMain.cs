using MaterialSkin;
using MaterialSkin.Controls;
using ModernFolderBrowserDialog;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using Tools_Injector_Mod_Menu.Patch_Manager;

namespace Tools_Injector_Mod_Menu
{
    public partial class FrmMain : MaterialForm
    {
        public FrmMain()
        {
            InitializeComponent();
            LoadTheme();
        }

        #region Variable

        public static readonly string AppPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        private string[] _menuFiles;

        private int _offsetCount = 1;

        private readonly string _tempPath = Path.GetTempPath();

        private readonly string _tempPathMenu = Path.GetTempPath() + "TFiveMenu";

        public static string Category, SeekBar;

        private int _compile;

        public enum State
        {
            Idle,
            Running
        }

        #endregion Variable

        #region Load

        private void FrmMain_Load(object sender, EventArgs e)
        {
            CheckFolder();
            LoadFiles();
            LoadSettings();
        }

        private void LoadTheme()
        {
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Indigo500, Primary.Indigo700, Primary.Indigo100, Accent.Blue700, TextShade.WHITE);
        }

        private void CheckFolder()
        {
            if (!Directory.Exists(_tempPathMenu))
            {
                Directory.CreateDirectory(_tempPathMenu);
            }

            Utility.CheckFolder("Theme");
            Utility.CheckFolder("Menu");
            Utility.CheckFolder("Output");
            Utility.CheckFolder("Save");
            Utility.CheckFolder("Logs");
        }

        private void LoadFiles()
        {
            var themeFiles = Directory.GetFiles(AppPath + "\\Theme", "*.zip");
            _menuFiles = Directory.GetFiles(AppPath + "\\Menu", "*.zip");

            if (themeFiles.Length == 0)
            {
                MyMessage.MsgShowError(@"Not found Theme files .zip!!");
                Application.Exit();
            }

            if (_menuFiles.Length == 0)
            {
                MyMessage.MsgShowError(@"Not found Menu files .zip!!");
                Application.Exit();
            }

            if (!Utility.CheckFiles("Theme", "Default.zip"))
            {
                MyMessage.MsgShowError(@"Theme file Default.zip is missing!!");
                Application.Exit();
            }

            if (!Utility.CheckFiles("Menu", "Default.zip"))
            {
                MyMessage.MsgShowError(@"Menu file Default.zip is missing!!");
                Application.Exit();
            }

            var themFile = themeFiles.Select(Path.GetFileName).ToList();
            var menuFile = _menuFiles.Select(Path.GetFileName).ToList();

            if (!Utility.IsEqual(themFile, menuFile))
            {
                MyMessage.MsgShowError("Theme files not equal Menu files");
                Application.Exit();
            }

            foreach (var t in menuFile)
            {
                WriteOutput("[Success] Loaded " + t, Color.Green);
                comboMenu.Items.Add(t.Replace(".zip", ""));
            }
        }

        private void LoadSettings()
        {
            var settings = Properties.Settings.Default;
            txtLibName.Text = settings.txtLibName;
            if (!string.IsNullOrWhiteSpace(settings.txtToast) && settings.txtToast.Contains('|'))
            {
                foreach (var t in settings.txtToast.Split('|'))
                {
                    listToast.Items.Add(t);
                }
            }
            txtName.Text = settings.txtName;
            txtSite.Text = settings.txtSite;
            txtText.Text = settings.txtText;
            txtEndCredit.Text = settings.txtEndCredit;
            txtImg.Text = settings.txtImg;
            txtNDK.Text = settings.txtNDK;

            chkRemoveTemp.Checked = settings.chkRemoveTemp;
            chkTFiveCredit.Checked = settings.chkTFiveCredit;
            chkLogsComplie.Checked = settings.chkLogsComplie;
            chkLogsSuccess.Checked = settings.chkLogsSuccess;
            chkLogsError.Checked = settings.chkLogsError;
            chkSound.Checked = settings.chkSound;

            txtService.Text = settings.txtService;
            txtOnCreate.Text = settings.txtOnCreate;
            txtActionMain.Text = settings.txtActionMain;

            try
            {
                comboMenu.SelectedIndex = settings.menuStyle;
            }
            catch
            {
                comboMenu.SelectedIndex = 0;
            }
        }

        #endregion Load

        #region Main Page

        private void btnAddToast_Click(object sender, EventArgs e)
        {
            try
            {
                if (Utility.IsEmpty(txtToast.Text)) return;
                listToast.Items.Add(txtToast.Text);
                WriteOutput("[Success] Add Toast " + txtToast.Text, Color.Green);
            }
            catch (Exception exception)
            {
                WriteOutput("[Error:001] " + exception.Message, Color.Red);
            }
        }

        private void btnRemoveToast_Click(object sender, EventArgs e)
        {
            try
            {
                if (listToast.SelectedIndex >= 0)
                {
                    WriteOutput("[Success] Remove Toast " + listToast.GetItemText(listToast.SelectedItem), Color.Green);
                    listToast.Items.RemoveAt(listToast.SelectedIndex);
                }
            }
            catch (Exception exception)
            {
                WriteOutput("[Error:002] " + exception.Message, Color.Red);
            }
        }

        private void btnBrowseNDK_Click(object sender, EventArgs e)
        {
            var folderBrowser = new FolderBrowser();
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                if (folderBrowser.SelectedPath.IsPathSpecialChar())
                {
                    MyMessage.MsgShowWarning("Ndk path must without any special character");
                    WriteOutput("[Error:022] Ndk path must without any special character", Color.Red);
                    return;
                }
                txtNDK.Text = folderBrowser.SelectedPath;
                WriteOutput("[Success] Change NDK Path To: " + txtNDK.Text, Color.Green);
            }
        }

        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            try
            {
                if (MyMessage.MsgOkCancel("Save Settings.\n\n" +
                                          "Click \"OK\" to confirm.\n\n" +
                                          "Click \"Cancel\" to cancel."))
                {
                    var settings = Properties.Settings.Default;
                    settings.txtLibName = txtLibName.Text;
                    var toast = listToast.Items.Cast<object>().Aggregate("", (current, t) => current + (t + "|"));
                    if (listToast.Items.Count > 0)
                        toast = toast.Substring(0, toast.Length - 1);
                    settings.txtToast = toast;
                    settings.txtName = txtName.Text;
                    settings.txtSite = txtSite.Text;
                    settings.txtText = txtText.Text;
                    settings.txtEndCredit = txtEndCredit.Text;
                    settings.txtImg = txtImg.Text;
                    settings.txtNDK = txtNDK.Text;
                    settings.menuStyle = comboMenu.SelectedIndex;
                    settings.chkRemoveTemp = chkRemoveTemp.Checked;
                    settings.chkTFiveCredit = chkTFiveCredit.Checked;
                    settings.chkLogsComplie = chkLogsComplie.Checked;
                    settings.chkLogsSuccess = chkLogsSuccess.Checked;
                    settings.chkLogsError = chkLogsError.Checked;
                    settings.chkSound = chkSound.Checked;
                    settings.Save();
                    WriteOutput("[Success] Saved Settings", Color.Green);
                }
            }
            catch (Exception exception)
            {
                WriteOutput("[Error:003] " + exception.Message, Color.Red);
            }
        }

        private void lbImageEncoder_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://codebeautify.org/image-to-base64-converter");
        }

        #endregion Main Page

        #region Menu Page

        #region Offset Group

        private void BtnFunctionManager()
        {
            var functionType = (Enums.FunctionType)comboFunction.SelectedIndex;
            if (functionType == Enums.FunctionType.ToggleSeekBar ||
                functionType == Enums.FunctionType.ButtonOnOffSeekBar)
            {
                EasyEnabled(btnFunction);
                btnFunction.HighEmphasis = true;
                EasyEnabled(chkMultiple);
                EasyEnabled(txtHex, false);
            }
            else if (functionType == Enums.FunctionType.ToggleInputValue ||
                functionType == Enums.FunctionType.ButtonOnOffInputValue)
            {
                EasyEnabled(chkMultiple);
                EasyEnabled(txtHex, false);
            }
            else if (functionType == Enums.FunctionType.Category)
            {
                EasyEnabled(txtOffset, false);
                EasyEnabled(txtHex, false);
            }
            else
            {
                EasyEnabled(txtOffset);
                EasyEnabled(txtHex);

                EasyEnabled(btnFunction, false);
                EasyEnabled(chkMultiple, false);

                btnFunction.HighEmphasis = false;
                chkMultiple.Checked = false;
            }
        }

        private void comboFunction_SelectedIndexChanged(object sender, EventArgs e)
        {
            var functionType = (Enums.FunctionType)comboFunction.SelectedIndex;

            if (functionType == Enums.FunctionType.ButtonOnOffSeekBar ||
                functionType == Enums.FunctionType.ButtonOnOffInputValue)
            {
                //TODO
                comboFunction.SelectedIndex = -1;
                // I don't know why the buttons won't work. But don't worry, Toggle still works.
            }

            BtnFunctionManager();

            if (functionType == Enums.FunctionType.ToggleSeekBar ||
                functionType == Enums.FunctionType.ButtonOnOffSeekBar ||
                functionType == Enums.FunctionType.ToggleInputValue ||
                functionType == Enums.FunctionType.ButtonOnOffInputValue ||
                functionType == Enums.FunctionType.Category)
            {
                if (functionType == Enums.FunctionType.Category)
                {
                    txtOffset.Text = "-";
                }
                txtHex.Text = "-";
            }
            else
            {
                txtOffset.Text = "";
                txtHex.Text = "";
            }
        }

        private void btnAddFunction_Click(object sender, EventArgs e)
        {
            if (Utility.IsEmpty(txtOffset) || Utility.IsEmpty(txtHex)) return;
            if (!txtOffset.Text.StartsWith("0x") && txtOffset.Text != "-")
            {
                MyMessage.MsgShowWarning(@"Offset Does not start with ""0x"" Please Check it again!!!");
                WriteOutput(@"[Warning] Offset Does not start with ""0x""", Color.Orange);
                return;
            }

            var functionType = CheckFunctionValue();
            if (functionType == Enums.FunctionType.Empty) return;

            AddOffset(functionType);
        }

        private Enums.FunctionType CheckFunctionValue()
        {
            var functionType = (Enums.FunctionType)comboFunction.SelectedIndex;
            switch (functionType)
            {
                case Enums.FunctionType.ToggleSeekBar:
                case Enums.FunctionType.ButtonOnOffSeekBar:
                    return Utility.IsEmpty(SeekBar) ? Enums.FunctionType.Empty : functionType;

                case Enums.FunctionType.Category:
                    return Utility.IsEmpty(Category) ? Enums.FunctionType.Empty : Enums.FunctionType.Category;

                default:
                    return functionType;
            }
        }

        private void AddOffset(Enums.FunctionType functionType)
        {
            var hex = Utility.InsertSpaces(txtHex.Text);
            var offset = new OffsetInfo
            {
                OffsetId = _offsetCount,
                Offset = txtOffset.Text,
                Hex = hex
            };

            if (chkDup.Checked && Utility.IsDuplicate(offset, OffsetPatch.OffsetList))
            {
                return;
            }

            if (!MyMessage.MsgOkCancel("Add Patch Offset.\n\n" +
                                       "Click \"OK\" to Continue if Your Offset and Hex Code are Correct!\n\n" +
                                       "Click \"Cancel\" to Fix it if Your Offset and Hex Code not Correct!"))
                return;

            OffsetPatch.AddOffset(offset, OffsetPatch.OffsetList);
            _offsetCount++;

            switch (functionType)
            {
                case Enums.FunctionType.ToggleSeekBar:
                case Enums.FunctionType.ButtonOnOffSeekBar:
                    btnFunction.HighEmphasis = false;
                    EasyEnabled(btnFunction, false);
                    EasyEnabled(chkMultiple, false);
                    break;

                case Enums.FunctionType.Category:
                    EasyEnabled(btnAddOffset, false);
                    EasyEnabled(chkDup, false);
                    break;
            }

            EasyEnabled(comboFunction, false);
            txtOffset.Clear();

            WriteOutput($"[Success] Added Offset - Offset ID: {offset.OffsetId}, Offset: {offset.Offset}, Hex: {offset.Hex}", Color.Green);
        }

        private void btnFunction_Click(object sender, EventArgs e)
        {
            switch ((Enums.FunctionType)comboFunction.SelectedIndex)
            {
                case Enums.FunctionType.ToggleSeekBar:
                case Enums.FunctionType.ButtonOnOffSeekBar:
                    var frmSeekBar = new FrmSeekBar();
                    frmSeekBar.ShowDialog();
                    frmSeekBar.Dispose();
                    break;

                case Enums.FunctionType.Category:
                    var frmCategory = new FrmCategory();
                    frmCategory.ShowDialog();
                    frmCategory.Dispose();
                    break;
            }
        }

        #endregion Offset Group

        #region Function Group

        private void btnAddFunction_Click_1(object sender, EventArgs e)
        {
            AddFunction();
        }

        private void AddFunction()
        {
            var functionType = (Enums.FunctionType)comboFunction.SelectedIndex;
            if (functionType == Enums.FunctionType.Category)
            {
                goto category;
            }

            if (Utility.IsEmpty(txtNameCheat)) return;

            if (Utility.IsEmpty(OffsetPatch.OffsetList)) return;

            if (Utility.IsDuplicateName(txtNameCheat.Text, OffsetPatch.FunctionList))
            {
                return;
            }

        category:

            var functionValue = FunctionValue();

            OffsetPatch.AddFunction(txtNameCheat.Text, OffsetPatch.OffsetList, functionType, functionValue, chkMultiple.Checked);

            _offsetCount = 1;

            BtnFunctionManager();
            EasyEnabled(comboFunction);
            EasyEnabled(btnAddOffset);
            EasyEnabled(chkDup);

            AddListView();
            txtNameCheat.Clear();
        }

        private string FunctionValue()
        {
            switch ((Enums.FunctionType)comboFunction.SelectedIndex)
            {
                case Enums.FunctionType.Category:
                    return Category;

                case Enums.FunctionType.ToggleSeekBar:
                case Enums.FunctionType.ButtonOnOffSeekBar:
                    return SeekBar;

                default:
                    return "";
            }
        }

        #endregion Function Group

        #region ListView Group

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                var index = listView1.SelectedItems[0].Index;
                var frmFunction = new FrmFunction(index);
                Hide();
                frmFunction.ShowDialog();
                Show();
                frmFunction.Dispose();
                AddAllListView(true);
            }
        }

        private void AddListView()
        {
            var offsetList = OffsetPatch.ConvertFunctionList(OffsetPatch.FunctionList.Count - 1);
            var offsetCount = offsetList.Count;

            var function = OffsetPatch.FunctionList[OffsetPatch.FunctionList.Count - 1].FunctionType.ToString();
            var value = OffsetPatch.FunctionList[OffsetPatch.FunctionList.Count - 1].FunctionValue;

            var cheatName = OffsetPatch.ConvertNameList();
            var offset = offsetCount > 1 ? "Multiple Patch Offset" : offsetList[offsetList.Count - 1].Offset;
            var hex = offsetCount > 1 ? "Multiple Patch Hex Code" : offsetList[offsetList.Count - 1].Hex;
            var multiple = OffsetPatch.FunctionList[OffsetPatch.FunctionList.Count - 1].MultipleValue ? "Yes" : "No";

            AddListValues(function, offset, hex, cheatName[cheatName.Count - 1], offsetCount, multiple, value);

            OffsetPatch.OffsetList.Clear();
        }

        private void AddAllListView(bool reValues = false)
        {
            listView1.Items.Clear();
            if (reValues)
            {
                WriteOutput("[Success] Clear Function List");
            }
            for (var i = 0; i < OffsetPatch.FunctionList.Count; i++)
            {
                var offsetList = OffsetPatch.ConvertFunctionList(i);
                var offsetCount = offsetList.Count;

                var function = OffsetPatch.FunctionList[i].FunctionType.ToString();
                var value = OffsetPatch.FunctionList[i].FunctionValue;

                var cheatName = OffsetPatch.ConvertNameList();
                var offset = offsetCount > 1 ? "Multiple Patch Offset" : offsetList[offsetList.Count - 1].Offset;
                var hex = offsetCount > 1 ? "Multiple Patch Hex Code" : offsetList[offsetList.Count - 1].Hex;
                var multiple = OffsetPatch.FunctionList[OffsetPatch.FunctionList.Count - 1].MultipleValue ? "Yes" : "No";

                AddListValues(function, offset, hex, cheatName[i], offsetCount, multiple, value);
                OffsetPatch.OffsetList.Clear();
            }
        }

        private void AddListValues(string function, string offset, string hex, string cheatName, int offsetCount, string multiple,
              string value = "")
        {
            var items = new ListViewItem(cheatName);
            items.SubItems.Add(offset);
            items.SubItems.Add(hex);
            items.SubItems.Add(function);
            items.SubItems.Add(value);
            items.SubItems.Add(offsetCount.ToString());
            items.SubItems.Add(multiple);
            listView1.Items.Add(items);

            WriteOutput("[Success] Added Function " + cheatName, Color.Green);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                var count = listView1.SelectedItems.Count;
                if (listView1.Items.Count != 0 && count != 0)
                {
                    if (MyMessage.MsgOkCancel("You sure you want remove this function in the List?"))
                    {
                        for (var i = 0; i < listView1.Items.Count; i++)
                        {
                            if (listView1.Items[i].Selected)
                            {
                                WriteOutput("[Success] Remove Function" + listView1.SelectedItems[0].Text, Color.Green);
                                listView1.Items[i].Remove();
                                OffsetPatch.FunctionList.RemoveAt(i);
                            }
                        }
                    }
                }
                else
                {
                    MyMessage.MsgShowWarning("There is no Items Functions in the List!!");
                    WriteOutput("[Waring] There is no Items Functions to Remove", Color.Orange);
                }
            }
            catch (Exception ex)
            {
                WriteOutput("[Error:004] " + ex.Message, Color.Red);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                if (MyMessage.MsgOkCancel("You sure you want remove this function in the List?"))
                {
                    listView1.Items.Clear();
                    OffsetPatch.FunctionList.Clear();
                    WriteOutput("[Success] Clear Function", Color.Green);
                }
            }
            catch (Exception ex)
            {
                WriteOutput("[Error:005] " + ex.Message, Color.Red);
            }
        }

        #endregion ListView Group

        #region Load/Save Group

        private void btnSaveCheat_Click(object sender, EventArgs e)
        {
            if (OffsetPatch.FunctionList.Count <= 0)
            {
                MyMessage.MsgShowWarning(@"Function list is Empty, Please Check it again!!!");
                WriteOutput("[Waring] Function list is Empty", Color.Orange);
                return;
            }

            var saveFile = new SaveFileDialog
            {
                Filter = @"XML|*.xml|All files|*.*",
                Title = Text,
                DefaultExt = ".xml",
                FileName = txtNameGame.Text
            };
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                FormState(State.Running);

                OffsetPatch.T5Menu.FunctionList = OffsetPatch.FunctionList;
                OffsetPatch.T5Menu.GameName = txtNameGame.Text;
                OffsetPatch.T5Menu.Target = txtTargetLib.Text;
                OffsetPatch.T5Menu.TypeAbi = (Enums.TypeAbi)comboType.SelectedIndex;

                SaveXml(OffsetPatch.T5Menu, saveFile.FileName);
                FormState(State.Idle);
            }
        }

        private void btnLoadCheat_Click(object sender, EventArgs e)
        {
            var openFile = new OpenFileDialog()
            {
                Filter = @"XML|*.xml|All files|*.*",
                Title = Text,
                DefaultExt = ".xml"
            };
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    FormState(State.Running);
                    OffsetPatch.T5Menu = LoadXml<TFiveMenu>(openFile.FileName);

                    OffsetPatch.FunctionList = OffsetPatch.T5Menu.FunctionList;
                    txtNameGame.Text = OffsetPatch.T5Menu.GameName;
                    txtTargetLib.Text = OffsetPatch.T5Menu.Target;
                    comboType.SelectedIndex = (int)OffsetPatch.T5Menu.TypeAbi;

                    FormState(State.Idle);
                }
                catch (Exception ex)
                {
                    WriteOutput("[Error:006] " + ex.Message, Color.Red);
                    throw;
                }
                finally
                {
                    AddAllListView();
                    txtNameGame.Text = Path.GetFileNameWithoutExtension(openFile.FileName);
                }
            }
        }

        //https://stackoverflow.com/a/6115782/8902883

        private void SaveXml<T>(T serializableObject, string fileName)
        {
            if (serializableObject == null) { return; }

            try
            {
                var xmlDocument = new XmlDocument();
                var serializer = new XmlSerializer(serializableObject.GetType());
                using (var stream = new MemoryStream())
                {
                    serializer.Serialize(stream, serializableObject);
                    stream.Position = 0;
                    xmlDocument.Load(stream);
                    xmlDocument.Save(fileName);
                }
                WriteOutput("[Success] Saved: " + Path.GetFileName(fileName), Color.Green);
            }
            catch (Exception ex)
            {
                WriteOutput("[Error:007] " + ex.Message, Color.Red);
            }
        }

        private T LoadXml<T>(string fileName)
        {
            if (Utility.IsEmpty(fileName)) { return default(T); }

            var objectOut = default(T);

            try
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.Load(fileName);
                var xmlString = xmlDocument.OuterXml;

                using (var read = new StringReader(xmlString))
                {
                    var outType = typeof(T);

                    var serializer = new XmlSerializer(outType);
                    using (XmlReader reader = new XmlTextReader(read))
                    {
                        objectOut = (T)serializer.Deserialize(reader);
                    }
                }
                WriteOutput("[Success] Loaded: " + Path.GetFileName(fileName), Color.Green);
            }
            catch (Exception ex)
            {
                WriteOutput("[Error:008] " + ex.Message, Color.Red);
            }

            return objectOut;
        }

        #endregion Load/Save Group

        #region Compile

        private void btnCompile_Click(object sender, EventArgs e)
        {
            if (Utility.IsEmpty(txtNameGame))
            {
                MyMessage.MsgShowWarning(@"Name Game is Empty, Please Check it again!!!");
                WriteOutput("[Warning] Name Game is Empty", Color.Orange);
                return;
            }
            if (Utility.IsEmpty(txtTargetLib))
            {
                MyMessage.MsgShowWarning(@"Target Library Name is Empty, Please Check it again!!!");
                WriteOutput("[Warning] Name Game is Empty", Color.Orange);
                return;
            }

            if (OffsetPatch.FunctionList.Count <= 0)
            {
                MyMessage.MsgShowWarning(@"Function list is Empty, Please Check it again!!!");
                WriteOutput("[Warning] Name Game is Empty", Color.Orange);
                return;
            }
            FormState(State.Running);
            materialTabControl1.SelectedTab = materialTabControl1.TabPages[2];
            if (!DeleteAll(_tempPathMenu)) return;
            if (!ExtractZip(AppPath + $"\\Menu\\{comboMenu.SelectedItem}.zip", _tempPathMenu)) return;
            if (!ExtractZip(AppPath + $"\\Theme\\{comboMenu.SelectedItem}.zip", _tempPathMenu)) return;
            if (!Replacer())
            {
                MyMessage.MsgShowError("Failed to Replace Something");
                WriteOutput("[Error:018] Failed to Replace Something", Color.Red);
                FormState(State.Idle);
                return;
            }

            if (!MoveDirectory(_tempPathMenu + "\\com", $"{AppPath}\\Output\\{txtNameGame.Text}\\smali\\com")) return;

            CompileNdk();
        }

        #region Modify Files

        private bool Replacer()
        {
            if (!MainActivity()) return false;
            if (!AndroidMk()) return false;
            if (!ApplicationMk()) return false;
            if (!Toast()) return false;
            if (!MenuString()) return false;
            return MainHack();
        }

        private bool MainActivity()
        {
            try
            {
                var text = File.ReadAllText(_tempPathMenu + "\\com\\tfive\\MainActivity.smali");
                text = text.Replace("MyLibName", txtLibName.Text);
                File.WriteAllText(_tempPathMenu + "\\com\\tfive\\MainActivity.smali", text);
                WriteOutput("[Success] Replaced MainActivity.smali", Color.Green);
                return true;
            }
            catch (Exception ex)
            {
                MyMessage.MsgShowError("Error " + ex.Message);
                WriteOutput("[Error:009] " + ex.Message, Color.Red);
                FormState(State.Idle);
                return false;
            }
        }

        private bool AndroidMk()
        {
            try
            {
                var text = File.ReadAllText(_tempPathMenu + "\\jni\\Android.mk");
                text = text.Replace("MyLibName", txtLibName.Text);
                File.WriteAllText(_tempPathMenu + "\\jni\\Android.mk", text);
                WriteOutput("[Success] Replaced Android.mk", Color.Green);
                return true;
            }
            catch (Exception ex)
            {
                MyMessage.MsgShowError("Error " + ex.Message);
                WriteOutput("[Error:010] " + ex.Message, Color.Red);
                FormState(State.Idle);
                return false;
            }
        }

        private bool ApplicationMk()
        {
            try
            {
                var text = File.ReadAllText(_tempPathMenu + "\\jni\\Application.mk");

                var type = "";
                switch (comboType.SelectedIndex)
                {
                    case (int)Enums.TypeAbi.Arm:
                        type = "armeabi-v7a";
                        break;

                    case (int)Enums.TypeAbi.Arm64:
                        type = "arm64-v8a";
                        break;

                    case (int)Enums.TypeAbi.X86:
                        type = "x86";
                        break;

                    case (int)Enums.TypeAbi.All:
                        type = "armeabi-v7a arm64-v8a x86";
                        break;
                }

                text = text.Replace("(ChangeABIHere)", type);
                File.WriteAllText(_tempPathMenu + "\\jni\\Application.mk", text);
                WriteOutput("[Success] Replaced Application.mk", Color.Green);
                return true;
            }
            catch (Exception ex)
            {
                MyMessage.MsgShowError("Error " + ex.Message);
                WriteOutput("[Error:011] " + ex.Message, Color.Red);
                FormState(State.Idle);
                return false;
            }
        }

        private bool Toast()
        {
            try
            {
                var text = File.ReadAllText(_tempPathMenu + "\\jni\\Toast.h");
                var toast = listToast.Items.Cast<object>().Aggregate("", (current, o) => current + @"MakeToast(env, context, OBFUSCATE(""" + o + @"""), Toast::LENGTH_LONG);" + Environment.NewLine);
                text = text.Replace("//ToastHere", toast);
                File.WriteAllText(_tempPathMenu + "\\jni\\Toast.h", text);
                WriteOutput("[Success] Replaced Toast.h", Color.Green);
                return true;
            }
            catch (Exception ex)
            {
                MyMessage.MsgShowError("Error " + ex.Message);
                WriteOutput("[Error:012] " + ex.Message, Color.Red);
                FormState(State.Idle);
                return false;
            }
        }

        private bool MenuString()
        {
            try
            {
                var text = File.ReadAllText(_tempPathMenu + "\\jni\\Menu.h");
                text = text.Replace("(yourName)", txtLibName.Text).
                    Replace("(yourSite)", txtSite.Text).
                    Replace("(yourText)", txtText.Text).
                    Replace("(yourImage)", txtImg.Text);
                text = chkTFiveCredit.Checked ? text.Replace("//(TFiveEndCredit)", @"OBFUSCATE(""0_RichWebView_<html><body><marquee style=\""color: white; font-weight:bold;\"" direction=\""left\"" scrollamount=\""5\"" behavior=\""scroll\"">TFive Tools</marquee></body></html>"")") : text;
                File.WriteAllText(_tempPathMenu + "\\jni\\Menu.h", text);
                WriteOutput("[Success] Replaced Menu.h (Credit)", Color.Green);
                return true;
            }
            catch (Exception ex)
            {
                MyMessage.MsgShowError("Error " + ex.Message);
                WriteOutput("[Error:013] " + ex.Message, Color.Red);
                FormState(State.Idle);
                return false;
            }
        }

        private bool MainHack()
        {
            try
            {
                var text = File.ReadAllText(_tempPathMenu + "\\jni\\Main.cpp");
                var memoryPatch = MemoryPatch();
                var newVariable = NewVariable();
                var newMethod = NewMethod();
                var featuresList = FeaturesList();
                var newFeatures = NewFeatures();
                var hackThread = HackThread();

                text = text.Replace("//VariableHere", memoryPatch)
                    .Replace("//NewVariableHere", newVariable)
                    .Replace("//NewMethodHere", newMethod)
                    .Replace("(yourTargetLibName)", txtTargetLib.Text)
                    .Replace("//(yourFeaturesList)", featuresList)
                    .Replace("(yourEndCredit)", txtEndCredit.Text)
                    .Replace("//(yourFeatures)", newFeatures)
                    .Replace(comboType.SelectedIndex == (int)Enums.TypeAbi.Arm64 ? "//(hackThread64)" : "//(hackThread)", hackThread);

                File.WriteAllText(_tempPathMenu + "\\jni\\Main.cpp", text);
                WriteOutput("[Success] Replaced Main.cpp", Color.Green);
                return true;
            }
            catch (Exception ex)
            {
                MyMessage.MsgShowError("Error " + ex.Message);
                WriteOutput("[Error:015] " + ex.Message, Color.Red);
                FormState(State.Idle);
                return false;
            }
        }

        #region Main.cpp

        private static string FeaturesList()
        {
            var functionList = OffsetPatch.FunctionList;
            var result = "";
            var realCount = 0;
            for (var i = 0; i < OffsetPatch.FunctionList.Count; i++)
            {
                var num = functionList[i].FunctionType == Enums.FunctionType.Category ? "0" : (realCount + 1).ToString();
                var type = functionList[i].FunctionType;
                var cheatName = functionList[i].CheatName;
                var functionExtra = !string.IsNullOrWhiteSpace(functionList[i].FunctionValue)
                    ? "_" + functionList[i].FunctionValue
                    : "";
                switch (type)
                {
                    case Enums.FunctionType.ToggleSeekBar:
                        result += $@"{Environment.NewLine}            OBFUSCATE(""{num}_Toggle_{cheatName}""),";
                        realCount++;
                        num = (realCount + 1).ToString();
                        result += $@"{Environment.NewLine}            OBFUSCATE(""{num}_SeekBar_{cheatName}{functionExtra}""),";
                        break;

                    case Enums.FunctionType.ToggleInputValue:
                        result += $@"{Environment.NewLine}            OBFUSCATE(""{num}_Toggle_{cheatName}""),";
                        realCount++;
                        num = (realCount + 1).ToString();
                        result += $@"{Environment.NewLine}            OBFUSCATE(""{num}_InputValue_{cheatName}{functionExtra}""),";
                        break;

                    case Enums.FunctionType.ButtonOnOffSeekBar:
                        result += $@"{Environment.NewLine}            OBFUSCATE(""{num}_SeekBar_{cheatName}{functionExtra}""),";
                        realCount++;
                        num = (realCount + 1).ToString();
                        result += $@"{Environment.NewLine}            OBFUSCATE(""{num}_ButtonOnOff_{cheatName}""),";
                        break;

                    case Enums.FunctionType.ButtonOnOffInputValue:
                        result += $@"{Environment.NewLine}            OBFUSCATE(""{num}_InputValue_{cheatName}{functionExtra}""),";
                        realCount++;
                        num = (realCount + 1).ToString();
                        result += $@"{Environment.NewLine}            OBFUSCATE(""{num}_ButtonOnOff_{cheatName}""),";
                        break;

                    case Enums.FunctionType.Patch:
                        result += $@"{Environment.NewLine}            OBFUSCATE(""0_RichTextView_{cheatName} - Activated""),";
                        break;

                    default:
                        result += $@"{Environment.NewLine}            OBFUSCATE(""{num}_{type}_{cheatName}{functionExtra}""),";
                        break;
                }

                if (type != Enums.FunctionType.Category)
                {
                    realCount++;
                }
            }
            return result;
        }

        private static string MemoryPatch()
        {
            var functionList = OffsetPatch.FunctionList;
            var result = "MemoryPatch ";
            for (var index = 0; index < functionList.Count; index++)
            {
                var list = OffsetPatch.FunctionList[index];
                result = functionList[index].OffsetList.Where(_ => list.FunctionType != Enums.FunctionType.Category).Aggregate(result, (current, info) => current + $"{list.CheatName.RemoveSuperSpecialCharacters().ReplaceNumCharacters()}_{info.OffsetId}, ");
            }

            return result.Remove(result.Length - 2) + ";";
        }

        private static string NewVariable()
        {
            var result = "";
            foreach (var list in OffsetPatch.FunctionList)
            {
                var nameCheat = list.CheatName.RemoveSuperSpecialCharacters().ReplaceNumCharacters();
                switch (list.FunctionType)
                {
                    case Enums.FunctionType.Toggle:
                    case Enums.FunctionType.ButtonOnOff:
                        result += $"bool _{nameCheat} = false;{Environment.NewLine}";
                        break;

                    case Enums.FunctionType.ToggleSeekBar:
                    case Enums.FunctionType.ToggleInputValue:
                    case Enums.FunctionType.ButtonOnOffSeekBar:
                    case Enums.FunctionType.ButtonOnOffInputValue:
                        result += $"bool _{nameCheat} = false;{Environment.NewLine}";
                        result += $"int _{nameCheat}Value = 1;{Environment.NewLine}";
                        break;
                }
            }

            return result;
        }

        private static string NewMethod()
        {
            var result = "";
            foreach (var list in OffsetPatch.FunctionList)
            {
                var nameCheat = list.CheatName.RemoveSuperSpecialCharacters().ReplaceNumCharacters();
                var multiple = list.MultipleValue ? $@"return _{nameCheat}Value*old_{nameCheat}(instance);" : $@"return _{nameCheat}Value;";
                if (list.FunctionType == Enums.FunctionType.ToggleSeekBar || list.FunctionType == Enums.FunctionType.ToggleInputValue ||
                    list.FunctionType == Enums.FunctionType.ButtonOnOffSeekBar || list.FunctionType == Enums.FunctionType.ButtonOnOffInputValue)
                {
                    result += $@"int (*old_{nameCheat})(void *instance);
int Update{nameCheat}(void *instance) {{
    if (instance != NULL && _{nameCheat} && _{nameCheat}Value > 1) {{
        {multiple}
    }}
    return old_{nameCheat}(instance);
}}
";
                }
            }

            return result;
        }

        private static string NewFeatures()
        {
            var functionList = OffsetPatch.FunctionList;
            var result = "";
            var realCount = 0;
            for (var i = 0; i < OffsetPatch.FunctionList.Count; i++)
            {
                var num = functionList[i].FunctionType == Enums.FunctionType.Category ? "0" : (realCount + 1).ToString();
                var type = functionList[i].FunctionType;
                var cheatName = functionList[i].CheatName.RemoveSuperSpecialCharacters().ReplaceNumCharacters();

                switch (type)
                {
                    case Enums.FunctionType.ButtonOnOff:
                    case Enums.FunctionType.Toggle:
                        var offsetListModify = functionList[i].OffsetList.Aggregate("", (current, info) => current +
                            $@"hexPatches.{cheatName}_{info.OffsetId}.Modify();
                                ");
                        var offsetListRestore = functionList[i].OffsetList.Aggregate("", (current, info) => current +
                            $@"hexPatches.{cheatName}_{info.OffsetId}.Restore();
                                ");

                        result += $@"
                        case {num}:
                            _{cheatName} = boolean;
                            if (_{cheatName}) {{
                                {offsetListModify}LOGI(OBFUSCATE(""On""));
                            }} else {{
                                {offsetListRestore}LOGI(OBFUSCATE(""Off""));
                            }}
                            break;{Environment.NewLine}";
                        break;

                    case Enums.FunctionType.ToggleSeekBar:
                    case Enums.FunctionType.ToggleInputValue:
                    case Enums.FunctionType.ButtonOnOffSeekBar:
                    case Enums.FunctionType.ButtonOnOffInputValue:
                        result += $@"
                        case {num}:
                            _{cheatName} = boolean;
                            break;{Environment.NewLine}";

                        realCount++;
                        num = (realCount + 1).ToString();
                        result += $@"
                        case {num}:
                            if (value >= 1) {{
                                _{cheatName}Value = value;
                            }}
                            break;{Environment.NewLine}";
                        break;
                }

                if (type != Enums.FunctionType.Category)
                {
                    realCount++;
                }
            }
            return result;
        }

        private string HackThread()
        {
            var result = "";
            foreach (var list in OffsetPatch.FunctionList)
            {
                var nameCheat = list.CheatName.RemoveSuperSpecialCharacters().ReplaceNumCharacters();

                if (list.FunctionType == Enums.FunctionType.Toggle ||
                    list.FunctionType == Enums.FunctionType.ButtonOnOff)
                {
                    result = list.OffsetList.Aggregate(result, (current, info) => current + $@"hexPatches.{nameCheat}_{info.OffsetId} = MemoryPatch::createWithHex(""{txtTargetLib.Text}"",
                                        string2Offset(OBFUSCATE_KEY(""{info.Offset}"", 't')),
                                        OBFUSCATE(""{info.Hex}""));{Environment.NewLine}    ");
                    continue;
                }

                if (list.FunctionType == Enums.FunctionType.ToggleSeekBar ||
                    list.FunctionType == Enums.FunctionType.ToggleInputValue ||
                    list.FunctionType == Enums.FunctionType.ButtonOnOffSeekBar ||
                    list.FunctionType == Enums.FunctionType.ButtonOnOffInputValue)
                {
                    result = list.OffsetList.Aggregate(result, (current, info) => current + $@"MSHookFunction((void *) getAbsoluteAddress(targetLibName, string2Offset(OBFUSCATE_KEY(""{info.Offset}"", 't'))),
                            (void *) Update{nameCheat}, (void **) &old_{nameCheat});{Environment.NewLine}    ");
                    continue;
                }

                if (list.FunctionType == Enums.FunctionType.Patch)
                {
                    result = list.OffsetList.Aggregate(result, (current, info) => current + $@"hexPatches.{nameCheat}_{info.OffsetId} = MemoryPatch::createWithHex(""{txtTargetLib.Text}"",
                                        string2Offset(OBFUSCATE_KEY(""{info.Offset}"", 't')),
                                        OBFUSCATE(""{info.Hex}""));
    hexPatches.{nameCheat}_{info.OffsetId}.Modify();{Environment.NewLine}    ");
                }
            }

            return result;
        }

        #endregion Main.cpp

        #endregion Modify Files

        private void CompileNdk()
        {
            compilerWorker.RunWorkerAsync();
        }

        private void compilerWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                _compile = 0;
                var process = new Process
                {
                    StartInfo =
                    {
                        FileName = "cmd.exe",
                        Arguments = $"/c {Properties.Settings.Default.txtNDK}\\build\\ndk-build",
                        WindowStyle = ProcessWindowStyle.Hidden,
                        UseShellExecute = false,
                        RedirectStandardError = true,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true,
                        WorkingDirectory = $"{_tempPath}TFiveMenu\\jni"
                    }
                };

                process.OutputDataReceived += OutputDataReceived;
                process.ErrorDataReceived += ErrorDataReceived;
                process.EnableRaisingEvents = true;
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit(50000);
            }
            catch (Exception exception)
            {
                compilerWorker.CancelAsync();
                WriteOutput("[Error:100] " + exception.Message, Color.Red);
                FormState(State.Idle);
            }
        }

        private void OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            WriteOutput("[Compile] " + e.Data);
        }

        private void ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            _compile++;
            if (_compile <= 2) return;
            WriteOutput("[Compile] " + e.Data, Color.Red);
        }

        private void compilerWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (_compile > 2)
            {
                MyMessage.MsgShowError("Failed to Compile");
                WriteOutput("[Error:020] Failed to Compile", Color.Red);
                FormState(State.Idle);
                return;
            }

            var outputDir = $"{_tempPathMenu}\\libs";
            var desDir = AppPath + "\\Output\\" + txtNameGame.Text + "\\lib";

            var deleteTemp = chkRemoveTemp.Checked;

            if (MoveDirectory(outputDir, desDir, deleteTemp))
            {
                WriteOutput($"[Success] Move {outputDir}{Environment.NewLine}To => {desDir}", Color.Green);
            }
            else
            {
                WriteOutput("[Error:021] Can not Move", Color.Red);
            }

            FormState(State.Idle);
        }

        #endregion Compile

        #region Utility

        private bool DeleteAll(string path)
        {
            try
            {
                var directory = new DirectoryInfo(path);
                foreach (var file in directory.GetFiles())
                {
                    file.Delete();
                }
                foreach (var dir in directory.GetDirectories())
                {
                    dir.Delete(true);
                }
                WriteOutput("[Success] Deleted" + path, Color.Green);
                return true;
            }
            catch (Exception ex)
            {
                MyMessage.MsgShowError("Error " + ex.Message);
                WriteOutput("[Error:016] " + ex.Message, Color.Red);
                FormState(State.Idle);
                return false;
            }
        }

        private bool ExtractZip(string sourceFileName, string destinationPath)
        {
            try
            {
                using (var zipArchive = ZipFile.OpenRead(sourceFileName))
                {
                    zipArchive.ExtractToDirectory(destinationPath, true);
                }
                WriteOutput($"[Success] Extract {Path.GetFileName(sourceFileName)}{Environment.NewLine}To => {destinationPath}", Color.Green);
                return true;
            }
            catch (Exception ex)
            {
                MyMessage.MsgShowError("Error " + ex.Message);
                WriteOutput("[Error:017] " + ex.Message, Color.Red);
                FormState(State.Idle);
                return false;
            }
        }

        private bool MoveDirectory(string sourceDirectory, string destinationPath, bool deleteSource = true)
        {
            try
            {
                if (Directory.Exists(destinationPath) &&
                    MyMessage.MsgOkCancel(destinationPath + " Found.\n\n" +
                                                         "Click \"OK\" to Continue if you want to overwrite!" +
                                                         "\n\nClick \"Cancel\" to cancel it if not!"))
                {
                    Directory.Delete(destinationPath, true);
                }

                Utility.CheckFolder("Output\\" + txtNameGame.Text);

                if (!DirectoryCopy(sourceDirectory, destinationPath, true)) return false;

                if (Directory.Exists(sourceDirectory) && deleteSource)
                {
                    Directory.Delete(sourceDirectory, true);
                }

                if (Directory.Exists(destinationPath))
                {
                    WriteOutput($"[Success] Move {sourceDirectory}{Environment.NewLine}To => {destinationPath}",
                        Color.Green);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                MyMessage.MsgShowError("Error " + ex.Message);
                WriteOutput("[Error:019] " + ex.Message, Color.Red);
                FormState(State.Idle);
                return false;
            }
        }

        private bool DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            try
            {
                var dir = new DirectoryInfo(sourceDirName);

                if (!dir.Exists)
                {
                    return false;
                }

                var dirs = dir.GetDirectories();
                Directory.CreateDirectory(destDirName);
                var files = dir.GetFiles();
                foreach (var file in files)
                {
                    var tempPath = Path.Combine(destDirName, file.Name);
                    file.CopyTo(tempPath, false);
                }
                if (copySubDirs)
                {
                    foreach (var subDir in dirs)
                    {
                        var tempPath = Path.Combine(destDirName, subDir.Name);
                        DirectoryCopy(subDir.FullName, tempPath, true);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MyMessage.MsgShowError("Error " + ex.Message);
                WriteOutput("[Error:022] " + ex.Message, Color.Red);
                FormState(State.Idle);
                return false;
            }
        }

        private void FormState(State state)
        {
            EnableController(this, state != State.Running);
            if (state == State.Idle)
            {
                if (!Properties.Settings.Default.chkSound) return;
                System.Media.SystemSounds.Beep.Play();
            }
        }

        private void EnableController(Form form, bool value)
        {
            foreach (var obj in form.Controls)
            {
                var control = (Control)obj;
                switch (control.GetType().Name)
                {
                    case "Button":
                    case "TextBox":
                    case "RadioButton":
                    case "ListView":
                    case "Label":
                    case "LinkLabel":
                    case "ListBox":
                    case "MaterialButton":
                    case "MaterialCheckBox":
                    case "MaterialComboBox":
                    case "MaterialTabControl":
                    case "MaterialTextBox":
                    case "RichTextBox":
                        control.Enabled = value;
                        break;

                    case "GroupBox":
                    case "MaterialCard":
                    case "MaterialForm":
                    case "Panel":
                    case "TableLayoutPanel":
                    case "TabPage":
                        EnableController(control, value);
                        break;
                }
            }
        }

        private void EnableController(Control control, bool value)
        {
            foreach (var obj in control.Controls)
            {
                var control2 = (Control)obj;
                switch (control.GetType().Name)
                {
                    case "Button":
                    case "TextBox":
                    case "RadioButton":
                    case "ListView":
                    case "Label":
                    case "LinkLabel":
                    case "ListBox":
                    case "MaterialButton":
                    case "MaterialCheckBox":
                    case "MaterialComboBox":
                    case "MaterialTabControl":
                    case "MaterialTextBox":
                    case "RichTextBox":
                        control2.Enabled = value;
                        break;
                }
                switch (control2.GetType().Name)
                {
                    case "GroupBox":
                    case "MaterialCard":
                    case "MaterialForm":
                    case "Panel":
                    case "TableLayoutPanel":
                    case "TabPage":
                        EnableController(control, value);
                        break;
                }
            }
        }

        private void EasyEnabled(Control control, bool status = true)
        {
            control.Enabled = status;
        }

        #endregion Utility

        #endregion Menu Page

        #region Log&About Page

        private void btnOutput_Click(object sender, EventArgs e)
        {
            Process.Start(AppPath + "\\Output");
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            Process.Start(_tempPathMenu);
        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            rbLog.Clear();
        }

        private void btnSaveLog_Click(object sender, EventArgs e)
        {
            var date = DateTime.Now.ToString("yyyy-M-d HH-mm-ss");
            var path = $"{AppPath}\\Log\\{date}.txt";
            File.WriteAllText(path, rbLog.Text);
        }

        private static void AppendText(RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;
            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
            box.ScrollToCaret();
        }

        private void TextToLogs(string str, Color color)
        {
            Invoke(new MethodInvoker(delegate
            {
                AppendText(rbLog, str, color);
            }));
        }

        private void WriteOutput(string str, Color color)
        {
            if (!chkLogsComplie.Checked)
            {
                if (str.Contains("[Compile]")) return;
            }
            if (!chkLogsSuccess.Checked)
            {
                if (str.Contains("[Success]")) return;
            }
            if (!chkLogsError.Checked)
            {
                if (str.Contains("[Error:")) return;
            }
            Invoke(new MethodInvoker(delegate
            {
                TextToLogs(DateTime.Now.ToString("HH:mm:ss tt") + " " + str + Environment.NewLine, color);
            }));
        }

        private void WriteOutput(string str)
        {
            Invoke(new MethodInvoker(delegate
            {
                TextToLogs(DateTime.Now.ToString("HH:mm:ss tt") + " " + str + Environment.NewLine, Color.Black);
            }));
        }

        #endregion Log&About Page

        #region Dev Page

        #region Permission

        private void btnCopyPermission_Click(object sender, EventArgs e)
        {
            CopyText(txtPermission.Text);
        }

        private void btnCopyService_Click(object sender, EventArgs e)
        {
            CopyText(txtService.Text);
        }

        private void btnSavePermission_Click(object sender, EventArgs e)
        {
            if (!MyMessage.MsgOkCancel("Do you want to save?\n\n" +
                                       "Click \"OK\" to confirm.\n\n" +
                                       "Click \"Cancel\" to cancel.")) return;
            Properties.Settings.Default.txtService = txtService.Text;
        }

        #endregion Permission

        #region Method 1

        private void btnOnCreate_Click(object sender, EventArgs e)
        {
            CopyText(txtOnCreate.Text);
        }

        private void btnSaveMethod1_Click(object sender, EventArgs e)
        {
            if (!MyMessage.MsgOkCancel("Do you want to save?\n\n" +
                                       "Click \"OK\" to confirm.\n\n" +
                                       "Click \"Cancel\" to cancel.")) return;
            Properties.Settings.Default.txtOnCreate = txtOnCreate.Text;
        }

        #endregion Method 1

        #region Method 2

        private void btnCopyFind_Click(object sender, EventArgs e)
        {
            CopyText(txtFind.Text);
        }

        private void btnCopyActionMain2_Click(object sender, EventArgs e)
        {
            CopyText(txtActionMain.Text);
        }

        private void btnSaveMethod2_Click(object sender, EventArgs e)
        {
            if (!MyMessage.MsgOkCancel("Do you want to save?\n\n" +
                                       "Click \"OK\" to confirm.\n\n" +
                                       "Click \"Cancel\" to cancel.")) return;
            Properties.Settings.Default.txtActionMain = txtActionMain.Text;
        }

        #endregion Method 2

        private void CopyText(string str)
        {
            Clipboard.SetText(str);
            if (!Properties.Settings.Default.chkSound) return;
            System.Media.SystemSounds.Beep.Play();
        }

        #endregion Dev Page
    }
}