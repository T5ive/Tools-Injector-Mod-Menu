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
        //TODO Edit offset list -> Frm Function
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

        public enum State
        {
            Idle,
            Running
        }

        #endregion Variable

        #region Load

        private void FrmMain_Load(object sender, EventArgs e)
        {
            LoadFiles();
            LoadSettings();
            CheckFolder();
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

            if (!Utility.CheckFiles("Theme", "Default.zip"))
            {
                MyMessage.MsgShow(@"Theme file Default.zip is missing!!", MessageBoxIcon.Error);
                Application.Exit();
            }

            if (!Utility.CheckFiles("Menu", "Default.zip"))
            {
                MyMessage.MsgShow(@"Menu file Default.zip is missing!!", MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        private void LoadFiles()
        {
            var themeFiles = Directory.GetFiles(AppPath + "\\Theme", "*.zip");
            _menuFiles = Directory.GetFiles(AppPath + "\\Menu", "*.zip");

            if (themeFiles.Length == 0)
            {
                MyMessage.MsgShow(@"Not found Theme files .zip!!", MessageBoxIcon.Error);
                Application.Exit();
            }

            if (_menuFiles.Length == 0)
            {
                MyMessage.MsgShow(@"Not found Menu files .zip!!", MessageBoxIcon.Error);
                Application.Exit();
            }

            var themFile = themeFiles.Select(Path.GetFileName).ToList();
            var menuFile = _menuFiles.Select(Path.GetFileName).ToList();

            if (!Utility.IsEqual(themFile, menuFile))
            {
                MyMessage.MsgShow("Theme files not equal Menu files", MessageBoxIcon.Error);
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
            if (!string.IsNullOrWhiteSpace(settings.txtToast))
            {
                if (settings.txtToast.Contains('|'))
                {
                    var split = settings.txtToast.Split('|');
                    foreach (var t in split)
                    {
                        listToast.Items.Add(t);
                    }
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
                    MyMessage.MsgShow("Ndk path must without any special character", MessageBoxIcon.Warning);
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

        private void comboFunction_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboFunction.SelectedIndex == 3 || comboFunction.SelectedIndex == 2)
            {
                btnFunction.Enabled = true;
                btnFunction.HighEmphasis = true;

                if (comboFunction.SelectedIndex == 3)
                {
                    txtOffset.Text = @"-";
                    txtOffset.Enabled = false;
                    txtHex.Text = @"-";
                    txtHex.Enabled = false;
                    return;
                }

                txtOffset.Text = @"";
                txtHex.Text = @"";
                txtOffset.Enabled = true;
                txtHex.Enabled = true;
            }
            else
            {
                txtOffset.Enabled = true;
                txtHex.Enabled = true;
                btnFunction.Enabled = false;
                btnFunction.HighEmphasis = false;
            }
            txtHex.Hint = comboFunction.SelectedIndex == 2 ? "TT TT A0 E3 1E FF 2F E1" : "7F 06 A0 E3 1E FF 2F E1";
        }

        private void btnAddFunction_Click(object sender, EventArgs e)
        {
            if (Utility.IsEmpty(txtOffset) || Utility.IsEmpty(txtHex)) return;
            var functionType = Enums.FunctionType.Toggle;
            if (!txtOffset.Text.StartsWith("0x"))
            {
                if (txtOffset.Text != @"-")
                {
                    MyMessage.MsgShow(@"Offset Does not start with ""0x"" Please Check it again!!!", MessageBoxIcon.Asterisk);
                    WriteOutput(@"[Warning] Offset Does not start with ""0x""", Color.Orange);
                    return;
                }
            }

            if (comboFunction.SelectedIndex == 2)
            {
                if (Utility.IsEmpty(SeekBar)) return;
                functionType = Enums.FunctionType.SeekBar;
            }

            if (comboFunction.SelectedIndex == 3)
            {
                if (Utility.IsEmpty(Category)) return;
                functionType = Enums.FunctionType.Category;
            }

            AddOffset(functionType);
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

            if (chkDup.Checked)
            {
                if (Utility.IsDuplicate(offset, OffsetPatch.OffsetList)) return;
            }

            if (MyMessage.MsgOkCancel("Add Patch Offset.\n\n" +
                                      "Click \"OK\" to Continue if Your Offset and Hex Code are Correct!" +
                                      "\n\nClick \"Cancel\" to Fix it if Your Offset and Hex Code not Correct!"))
            {
                OffsetPatch.AddOffset(offset, OffsetPatch.OffsetList);
                _offsetCount++;
                if (functionType == Enums.FunctionType.SeekBar)
                {
                    btnFunction.HighEmphasis = false;
                    EasyEnabled(btnFunction, false);
                }
                if (functionType == Enums.FunctionType.Category)
                {
                    EasyEnabled(btnAddOffset, false);
                    EasyEnabled(chkDup, false);
                }
                EasyEnabled(comboFunction, false);
                txtOffset.Clear();

                WriteOutput($"[Success] Added Offset - Offset ID: {offset.OffsetId}, Offset: {offset.Offset}, Hex: {offset.Hex}", Color.Green);
            }
        }

        private void btnFunction_Click(object sender, EventArgs e)
        {
            if (comboFunction.SelectedIndex == 2)
            {
                var frmSeekBar = new FrmSeekBar();
                frmSeekBar.ShowDialog();
                frmSeekBar.Dispose();
            }

            if (comboFunction.SelectedIndex == 3)
            {
                var frmCategory = new FrmCategory();
                frmCategory.ShowDialog();
                frmCategory.Dispose();
            }
        }

        #endregion Offset Group

        #region Function Group

        private void btnAddFunction_Click_1(object sender, EventArgs e)
        {
            if (comboFunction.SelectedIndex == 3)
            {
                AddFunction(true);
            }
            else
            {
                AddFunction();
            }
        }

        private void AddFunction(bool premium = false)
        {
            if (premium)
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

            var functionType = (Enums.FunctionType)comboFunction.SelectedIndex;
            var functionValue = "";

            if (functionType == Enums.FunctionType.Category)
            {
                functionValue = Category;
            }
            if (functionType == Enums.FunctionType.SeekBar)
            {
                functionValue = SeekBar;
            }

            OffsetPatch.AddFunction(txtNameCheat.Text, OffsetPatch.OffsetList, functionType, functionValue);

            _offsetCount = 1;

            btnFunction.HighEmphasis = true;
            EasyEnabled(btnFunction, true);
            EasyEnabled(comboFunction, true);
            EasyEnabled(btnAddOffset, true);
            EasyEnabled(chkDup, true);

            AddListView();
            txtNameCheat.Clear();
        }

        #endregion Function Group

        #region ListView Group

        private void AddListView()
        {
            var offsetList = OffsetPatch.ConvertFunctionList(OffsetPatch.FunctionList.Count - 1);
            var offsetCount = offsetList.Count;

            var function = OffsetPatch.FunctionList[OffsetPatch.FunctionList.Count - 1].FunctionType.ToString();
            var value = OffsetPatch.FunctionList[OffsetPatch.FunctionList.Count - 1].FunctionValue;

            var cheatName = OffsetPatch.ConvertNameList();
            var offset = offsetCount > 1 ? "Multiple Patch Offset" : offsetList[offsetList.Count - 1].Offset;
            var hex = offsetCount > 1 ? "Multiple Patch Hex Code" : offsetList[offsetList.Count - 1].Hex;

            AddListValues(function, offset, hex, cheatName[cheatName.Count - 1], offsetCount, value);

            OffsetPatch.OffsetList.Clear();
        }

        private void AddAllListView()
        {
            for (var i = 0; i < OffsetPatch.FunctionList.Count; i++)
            {
                var offsetList = OffsetPatch.ConvertFunctionList(i);
                var offsetCount = offsetList.Count;

                var function = OffsetPatch.FunctionList[i].FunctionType.ToString();
                var value = OffsetPatch.FunctionList[i].FunctionValue;

                var cheatName = OffsetPatch.ConvertNameList();
                var offset = offsetCount > 1 ? "Multiple Patch Offset" : offsetList[offsetList.Count - 1].Offset;
                var hex = offsetCount > 1 ? "Multiple Patch Hex Code" : offsetList[offsetList.Count - 1].Hex;

                AddListValues(function, offset, hex, cheatName[i], offsetCount, value);
                OffsetPatch.OffsetList.Clear();
            }
        }

        private void AddListValues(string function, string offset, string hex, string cheatName, int offsetCount,
              string value = "")
        {
            var items = new ListViewItem(cheatName);
            items.SubItems.Add(offset);
            items.SubItems.Add(hex);
            items.SubItems.Add(function);
            items.SubItems.Add(value);
            items.SubItems.Add(offsetCount.ToString());
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
                    MyMessage.MsgShow("There is no Items Functions in the List!!", MessageBoxIcon.Error);
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
                MyMessage.MsgShow(@"Function list is Empty, Please Check it again!!!", MessageBoxIcon.Asterisk);
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
                MyMessage.MsgShow(@"Name Game is Empty, Please Check it again!!!", MessageBoxIcon.Asterisk);
                WriteOutput("[Warning] Name Game is Empty", Color.Orange);
                return;
            }
            if (Utility.IsEmpty(txtTargetLib))
            {
                MyMessage.MsgShow(@"Target Library Name is Empty, Please Check it again!!!", MessageBoxIcon.Asterisk);
                WriteOutput("[Warning] Name Game is Empty", Color.Orange);
                return;
            }

            if (OffsetPatch.FunctionList.Count <= 0)
            {
                MyMessage.MsgShow(@"Function list is Empty, Please Check it again!!!", MessageBoxIcon.Asterisk);
                WriteOutput("[Warning] Name Game is Empty", Color.Orange);
                return;
            }
            FormState(State.Running);
            if (!DeleteAll(_tempPathMenu)) return;
            if (!ExtractZip(AppPath + $"\\Menu\\{comboMenu.SelectedItem}.zip", _tempPathMenu)) return;
            if (!ExtractZip(AppPath + $"\\Theme\\{comboMenu.SelectedItem}.zip", _tempPathMenu)) return;
            if (!Replacer())
            {
                MyMessage.MsgShow("Failed to Replace Something", MessageBoxIcon.Error);
                WriteOutput("[Error:018] Failed to Replace Something", Color.Red);
                FormState(State.Idle);
                return;
            }

            if (!MoveDirectory(_tempPathMenu + "\\com", $"{AppPath}\\Output\\{txtNameGame.Text}\\com")) return;

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
            if (!MenuFeatures()) return false;
            if (!MainHack()) return false;
            return true;
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
                MyMessage.MsgShow("Error " + ex.Message, MessageBoxIcon.Error);
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
                MyMessage.MsgShow("Error " + ex.Message, MessageBoxIcon.Error);
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
                }

                text = text.Replace("(ChangeABIHere)", type);
                File.WriteAllText(_tempPathMenu + "\\jni\\Application.mk", text);
                WriteOutput("[Success] Replaced Application.mk", Color.Green);
                return true;
            }
            catch (Exception ex)
            {
                MyMessage.MsgShow("Error " + ex.Message, MessageBoxIcon.Error);
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
                MyMessage.MsgShow("Error " + ex.Message, MessageBoxIcon.Error);
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

                if (chkTFiveCredit.Checked)
                {
                    text = text.Replace("//(TFiveEndCredit)", @"OBFUSCATE(""0_RichWebView_<html><body><marquee style=\""color: white; font-weight:bold;\"" direction=\""left\"" scrollamount=\""5\"" behavior=\""scroll\"">TFive Tools</marquee></body></html>"")");
                }

                File.WriteAllText(_tempPathMenu + "\\jni\\Menu.h", text);
                WriteOutput("[Success] Replaced Menu.h (Credit)", Color.Green);
                return true;
            }
            catch (Exception ex)
            {
                MyMessage.MsgShow("Error " + ex.Message, MessageBoxIcon.Error);
                WriteOutput("[Error:013] " + ex.Message, Color.Red);
                FormState(State.Idle);
                return false;
            }
        }

        private bool MenuFeatures()
        {
            try
            {
                var text = File.ReadAllText(_tempPathMenu + "\\jni\\Menu.h");
                var features = "";
                var functionList = OffsetPatch.FunctionList;

                for (var i = 0; i < OffsetPatch.FunctionList.Count; i++)
                {
                    var category = functionList[i].FunctionType == Enums.FunctionType.Category ? "0" : (i + 1).ToString();
                    var functionExtra = !string.IsNullOrWhiteSpace(functionList[i].FunctionValue)
                        ? "_" + functionList[i].FunctionValue
                        : "";
                    features += $@"OBFUSCATE(""{category}_{functionList[i].FunctionType}_{functionList[i].CheatName}{functionExtra}""),{Environment.NewLine}";
                }
                text = text.Replace("//(yourFeatures)", features).Replace("(yourEndCredit)", txtEndCredit.Text);
                File.WriteAllText(_tempPathMenu + "\\jni\\Menu.h", text);
                WriteOutput("[Success] Replaced Menu.h (Features)", Color.Green);
                return true;
            }
            catch (Exception ex)
            {
                MyMessage.MsgShow("Error " + ex.Message, MessageBoxIcon.Error);
                WriteOutput("[Error:014] " + ex.Message, Color.Red);
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
                var newFeatures = NewFeatures();
                var hackThread = HackThread();
                var hackThread64 = HackThread64();

                text = text.Replace("//VariableHere", memoryPatch)
                    .Replace("//NewVariableHere", newVariable)
                    .Replace("(yourTargetLibName)", txtTargetLib.Text)
                    .Replace("//(yourFeatures)", newFeatures)
                    .Replace("//(hackThread64)", hackThread64)
                    .Replace("//(hackThread)", hackThread);
                File.WriteAllText(_tempPathMenu + "\\jni\\Main.cpp", text);
                WriteOutput("[Success] Replaced Main.cpp", Color.Green);
                return true;
            }
            catch (Exception ex)
            {
                MyMessage.MsgShow("Error " + ex.Message, MessageBoxIcon.Error);
                WriteOutput("[Error:015] " + ex.Message, Color.Red);
                FormState(State.Idle);
                return false;
            }
        }

        #region Main.cpp

        private string MemoryPatch()
        {
            var functionList = OffsetPatch.FunctionList;
            var result = "MemoryPatch ";
            for (var index = 0; index < functionList.Count; index++)
            {
                var list = OffsetPatch.FunctionList[index];
                foreach (var info in functionList[index].OffsetList)
                {
                    if (list.FunctionType != Enums.FunctionType.Category)
                    {
                        result += $"{list.CheatName.RemoveSuperSpecialCharacters()}_{info.OffsetId}, ";
                    }
                }
            }

            return result.Remove(result.Length - 2) + ";";
        }

        private string NewVariable()
        {
            var functionList = OffsetPatch.FunctionList;
            var result = "";
            foreach (var t in functionList)
            {
                if (t.FunctionType == Enums.FunctionType.Toggle || t.FunctionType == Enums.FunctionType.ButtonOnOff)
                {
                    result += $"bool _{t.CheatName.RemoveSuperSpecialCharacters()} = false;{Environment.NewLine}";
                }
                if (t.FunctionType == Enums.FunctionType.SeekBar)
                {
                    result += $"int _{t.CheatName.RemoveSuperSpecialCharacters()} = 1;{Environment.NewLine}";
                }
            }

            return result;
        }

        private string NewFeatures()
        {
            var functionList = OffsetPatch.FunctionList;
            var result = "";
            for (var i = 0; i < OffsetPatch.FunctionList.Count; i++)
            {
                var category = functionList[i].FunctionType == Enums.FunctionType.Category ? "0" : (i + 1).ToString();

                if (functionList[i].FunctionType == Enums.FunctionType.ButtonOnOff || functionList[i].FunctionType == Enums.FunctionType.Toggle)
                {
                    var offsetListModify = functionList[i].OffsetList.Aggregate("", (current, t) => current + $@"hexPatches.{functionList[i].CheatName.RemoveSuperSpecialCharacters()}_{t.OffsetId}.Modify();
                                ");
                    var offsetListRestore = functionList[i].OffsetList.Aggregate("", (current, t) => current + $@"hexPatches.{functionList[i].CheatName.RemoveSuperSpecialCharacters()}_{t.OffsetId}.Restore();
                                ");

                    result += $@"
                        case {category}:
                            _{functionList[i].CheatName.RemoveSuperSpecialCharacters()} = boolean;
                            if (_{functionList[i].CheatName.RemoveSuperSpecialCharacters()}) {{
                                {offsetListModify}LOGI(OBFUSCATE(""On""));
                            }} else {{
                                {offsetListRestore}LOGI(OBFUSCATE(""Off""));
                            }}
                            break;{Environment.NewLine}";
                }

                //TODO
                if (functionList[i].FunctionType == Enums.FunctionType.SeekBar)
                {
                    //result += $@"case {category}:{Environment.NewLine}
                    //                        if (_
                    //                         break; ";
                }
            }
            return result;
        }

        private string HackThread()
        {
            return OffsetPatch.FunctionList.Where(t => comboType.SelectedIndex == (int)Enums.TypeAbi.Arm | comboType.SelectedIndex == (int)Enums.TypeAbi.X86 && t.FunctionType != Enums.FunctionType.Category && t.FunctionType != Enums.FunctionType.SeekBar)
                .Aggregate("", (current1, t) => t.OffsetList.Aggregate(current1, (current, t1) => current + $@"    hexPatches.{t.CheatName.RemoveSuperSpecialCharacters()}_{t1.OffsetId} = MemoryPatch::createWithHex(""{txtTargetLib.Text}"",
                        string2Offset(OBFUSCATE_KEY(""{t1.Offset}"", 't')),
                        OBFUSCATE(""{t1.Hex}""));{Environment.NewLine}"));
        }

        private string HackThread64()
        {
            return OffsetPatch.FunctionList.Where(t => comboType.SelectedIndex == (int)Enums.TypeAbi.Arm64 && t.FunctionType != Enums.FunctionType.Category && t.FunctionType != Enums.FunctionType.SeekBar)
                .Aggregate("", (current1, t) => t.OffsetList.Aggregate(current1, (current, t1) => current + $@"    hexPatches.{t.CheatName.RemoveSuperSpecialCharacters()}_{t1.OffsetId} = MemoryPatch::createWithHex(""{txtTargetLib.Text}"",
                        string2Offset(OBFUSCATE_KEY(""{t1.Offset}"", 't')),
                        OBFUSCATE(""{t1.Hex}"")); "));
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
                process.EnableRaisingEvents = true;
                process.Start();
                process.BeginOutputReadLine();
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
            WriteOutput("[Compile] " + e.Data, Color.Black);
        }

        private void compilerWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MyMessage.MsgShow("Failed to Compile", MessageBoxIcon.Error);
                WriteOutput("[Error:020] ", Color.Red);
                FormState(State.Idle);
            }

            var outputDir = $"{_tempPathMenu}\\libs";
            var desDir = AppPath + "\\Output\\" + txtNameGame.Text + "\\Lib";

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
                MyMessage.MsgShow("Error " + ex.Message, MessageBoxIcon.Error);
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
                MyMessage.MsgShow("Error " + ex.Message, MessageBoxIcon.Error);
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
                MyMessage.MsgShow("Error " + ex.Message, MessageBoxIcon.Error);
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
                MyMessage.MsgShow("Error " + ex.Message, MessageBoxIcon.Error);
                WriteOutput("[Error:022] " + ex.Message, Color.Red);
                FormState(State.Idle);
                return false;
            }
        }

        private void FormState(State state)
        {
            EnableController(this, state != State.Running);
        }

        private void EnableController(Form form, bool value)
        {
            foreach (var obj in form.Controls)
            {
                var control = (Control)obj;
                if (control.GetType().Name == "Button" ||
                    control.GetType().Name == "TextBox" ||
                    control.GetType().Name == "RadioButton" ||
                    control.GetType().Name == "ListView" ||
                    control.GetType().Name == "Label" ||
                    control.GetType().Name == "LinkLabel" ||
                    control.GetType().Name == "ListBox" ||
                    control.GetType().Name == "MaterialButton" ||
                    control.GetType().Name == "MaterialCheckBox" ||
                    control.GetType().Name == "MaterialComboBox" ||
                    control.GetType().Name == "MaterialTabControl" ||
                    control.GetType().Name == "MaterialTextBox" ||
                    control.GetType().Name == "RichTextBox")
                {
                    control.Enabled = value;
                }
                if (control.GetType().Name == "GroupBox" ||
                    control.GetType().Name == "MaterialCard" ||
                    control.GetType().Name == "MaterialForm" ||
                    control.GetType().Name == "Panel" ||
                    control.GetType().Name == "TableLayoutPanel" ||
                    control.GetType().Name == "TabPage")
                {
                    EnableController(control, value);
                }
            }
        }

        private void EnableController(Control control, bool value)
        {
            foreach (var obj in control.Controls)
            {
                var control2 = (Control)obj;
                if (control.GetType().Name == "Button" ||
                    control.GetType().Name == "TextBox" ||
                    control.GetType().Name == "RadioButton" ||
                    control.GetType().Name == "ListView" ||
                    control.GetType().Name == "Label" ||
                    control.GetType().Name == "LinkLabel" ||
                    control.GetType().Name == "ListBox" ||
                    control.GetType().Name == "MaterialButton" ||
                    control.GetType().Name == "MaterialCheckBox" ||
                    control.GetType().Name == "MaterialComboBox" ||
                    control.GetType().Name == "MaterialTabControl" ||
                    control.GetType().Name == "MaterialTextBox" ||
                    control.GetType().Name == "RichTextBox")
                {
                    control2.Enabled = value;
                }
                if (control2.GetType().Name == "GroupBox" ||
                    control.GetType().Name == "MaterialCard" ||
                    control.GetType().Name == "MaterialForm" ||
                    control2.GetType().Name == "Panel" ||
                    control2.GetType().Name == "TableLayoutPanel" ||
                    control2.GetType().Name == "TabPage")
                {
                    EnableController(control, value);
                }
            }
        }

        private void EasyEnabled(Control control, bool status)
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
            using (File.Create(path))
            {
                rbLog.SaveFile(path, RichTextBoxStreamType.RichText);
            }
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
    }
}