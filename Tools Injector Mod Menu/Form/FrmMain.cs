using MaterialSkin;
using MaterialSkin.Controls;
using ModernFolderBrowserDialog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using Tools_Injector_Mod_Menu.Patch_Manager;

namespace Tools_Injector_Mod_Menu
{
    public partial class FrmMain : MaterialForm
    {
        private const bool Debug = false;

        public FrmMain()
        {
            InitializeComponent();
            LoadTheme();
        }

        #region Variable

        public static readonly string AppPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        private MySettings _mySettings = new MySettings();

        private string[] _menuFiles;

        private int _offsetCount = 1;

        private readonly string _tempPath = Path.GetTempPath();

        private readonly string _tempPathMenu = Path.GetTempPath() + "TFiveMenu";

        public static string ImageCode;

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
            _mySettings = MySettings.Load();
            txtLibName.Text = _mySettings.txtLibName;
            if (!Utility.IsEmpty(_mySettings.txtToast, false) && _mySettings.txtToast.Contains('|'))
            {
                foreach (var t in _mySettings.txtToast.Split('|'))
                {
                    listToast.Items.Add(t);
                }
            }

            if (!Utility.IsEmpty(_mySettings.txtToast, false) && !_mySettings.txtToast.Contains('|'))
            {
                listToast.Items.Add(_mySettings.txtToast);
            }

            txtName.Text = _mySettings.txtName;
            txtSite.Text = _mySettings.txtSite;
            txtText.Text = _mySettings.txtText;
            txtEndCredit.Text = _mySettings.txtEndCredit;
            ImageCode = _mySettings.ImageCode;
            txtNDK.Text = _mySettings.txtNDK;

            chkRemoveTemp.Checked = _mySettings.chkRemoveTemp;
            chkTFiveCredit.Checked = _mySettings.chkTFiveCredit;
            chkLogsComplie.Checked = _mySettings.chkLogsComplie;
            chkLogsSuccess.Checked = _mySettings.chkLogsSuccess;
            chkLogsError.Checked = _mySettings.chkLogsError;
            chkSound.Checked = _mySettings.chkSound;

            txtService.Text = _mySettings.txtService;
            txtOnCreate.Text = _mySettings.txtOnCreate;
            txtActionMain.Text = _mySettings.txtActionMain;
            LoadImg();

            try
            {
                comboMenu.SelectedIndex = _mySettings.menuStyle;
            }
            catch
            {
                comboMenu.SelectedIndex = 0;
            }
        }

        private void LoadImg()
        {
            try
            {
                if (!Utility.IsEmpty(ImageCode, false))
                {
                    picImg.Image = Base64ToImage(ImageCode);
                }
            }
            catch (Exception exception)
            {
                picImg.Image = null;
                ImageCode = "";
                WriteOutput("[Error:023] " + exception.Message, Color.Red);
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

        private void btnImage_Click(object sender, EventArgs e)
        {
            var openFile = new OpenFileDialog()
            {
                Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.png *.gif *.bmp) | *.jpg; *.jpeg; *.jpe; *.png; *.gif; *.bmp",
                Title = Text,
                DefaultExt = ".png"
            };
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var imgFormat = ImageFormat.Jpeg;
                    switch (Path.GetExtension(openFile.FileName))
                    {
                        case ".png":
                            imgFormat = ImageFormat.Png;
                            break;
                        case ".jpg":
                        case ".jpeg":
                        case ".jpe":
                            imgFormat = ImageFormat.Jpeg;
                            break;
                        case ".gif":
                            imgFormat = ImageFormat.Gif;
                            break;
                        case ".bmp":
                            imgFormat = ImageFormat.Bmp;
                            break;
                    }

                    ImageCode = ImageToBase64(CompressImage(openFile.FileName, 50, imgFormat), imgFormat);
                    LoadImg();
                }
                catch
                {
                    //
                }
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
                    _mySettings.txtLibName = txtLibName.Text;
                    var toast = listToast.Items.Cast<object>().Aggregate("", (current, t) => current + (t + "|"));
                    if (listToast.Items.Count > 0)
                        toast = toast.Substring(0, toast.Length - 1);
                    _mySettings.txtToast = toast;
                    _mySettings.txtName = txtName.Text;
                    _mySettings.txtSite = txtSite.Text;
                    _mySettings.txtText = txtText.Text;
                    _mySettings.txtEndCredit = txtEndCredit.Text;
                    _mySettings.ImageCode = ImageCode;
                    _mySettings.txtNDK = txtNDK.Text;
                    _mySettings.menuStyle = comboMenu.SelectedIndex;
                    _mySettings.chkRemoveTemp = chkRemoveTemp.Checked;
                    _mySettings.chkTFiveCredit = chkTFiveCredit.Checked;
                    _mySettings.chkLogsComplie = chkLogsComplie.Checked;
                    _mySettings.chkLogsSuccess = chkLogsSuccess.Checked;
                    _mySettings.chkLogsError = chkLogsError.Checked;
                    _mySettings.chkSound = chkSound.Checked;
                    _mySettings.Save();
                    WriteOutput("[Success] Saved Settings", Color.Green);
                }
            }
            catch (Exception exception)
            {
                WriteOutput("[Error:003] " + exception.Message, Color.Red);
            }
        }
        

        #endregion Main Page

        #region Menu Page

        #region Offset Group

        private void BtnFunctionManager()
        {
            switch ((Enums.FunctionType)comboFunction.SelectedIndex)
            {
                case Enums.FunctionType.ToggleHook:
                    EasyEnabled(btnFunction);
                    btnFunction.HighEmphasis = true;
                    EasyEnabled(chkMultiple, false);
                    EasyEnabled(txtHex, false);
                    chkMultiple.Checked = false;
                    break;

                case Enums.FunctionType.ButtonOnOffHook:
                case Enums.FunctionType.ToggleSeekBar:
                case Enums.FunctionType.ButtonOnOffSeekBar:
                case Enums.FunctionType.ToggleInputValue:
                case Enums.FunctionType.ButtonOnOffInputValue:
                    EasyEnabled(btnFunction);
                    btnFunction.HighEmphasis = true;
                    EasyEnabled(chkMultiple);
                    EasyEnabled(txtHex, false);
                    break;

                case Enums.FunctionType.Category:
                    EasyEnabled(btnFunction);
                    btnFunction.HighEmphasis = true;
                    EasyEnabled(txtOffset, false);
                    EasyEnabled(txtHex, false);
                    break;

                case Enums.FunctionType.Button:
                    EasyEnabled(btnFunction);
                    btnFunction.HighEmphasis = true;
                    EasyEnabled(txtHex, false);
                    break;

                default:
                    EasyEnabled(txtOffset);
                    EasyEnabled(txtHex);

                    EasyEnabled(btnFunction, false);
                    EasyEnabled(chkMultiple, false);

                    btnFunction.HighEmphasis = false;
                    chkMultiple.Checked = false;
                    break;
            }
        }

        private void comboFunction_SelectedIndexChanged(object sender, EventArgs e)
        {
            var functionType = (Enums.FunctionType)comboFunction.SelectedIndex;

            BtnFunctionManager();

            if (functionType == Enums.FunctionType.ToggleHook ||
                functionType == Enums.FunctionType.ButtonOnOffHook ||
                functionType == Enums.FunctionType.ToggleSeekBar ||
                functionType == Enums.FunctionType.ButtonOnOffSeekBar ||
                functionType == Enums.FunctionType.ToggleInputValue ||
                functionType == Enums.FunctionType.ButtonOnOffInputValue ||
                functionType == Enums.FunctionType.Button ||
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
            if (Utility.IsEmpty(txtOffset)) return;
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
                case Enums.FunctionType.ToggleHook:
                case Enums.FunctionType.ButtonOnOffHook:
                    if (!Values.Field)
                    {
                        return functionType;
                    }
                    return Utility.IsEmpty(Values.Offset) ? Enums.FunctionType.Empty : functionType;

                case Enums.FunctionType.ToggleSeekBar:
                case Enums.FunctionType.ButtonOnOffSeekBar:
                    if (Values.Field && Utility.IsEmpty(Values.Offset))
                    {
                        return Enums.FunctionType.Empty;
                    }
                    return Utility.IsEmpty(Values.SeekBar) ? Enums.FunctionType.Empty : functionType;

                case Enums.FunctionType.ToggleInputValue:
                case Enums.FunctionType.ButtonOnOffInputValue:
                    if (Values.Field && Utility.IsEmpty(Values.Offset))
                    {
                        return Enums.FunctionType.Empty;
                    }
                    return functionType;

                case Enums.FunctionType.Button:
                    return Utility.IsEmpty(Values.Method) ? Enums.FunctionType.Empty : functionType;

                case Enums.FunctionType.Category:
                    return Utility.IsEmpty(Values.Category) ? Enums.FunctionType.Empty : Enums.FunctionType.Category;

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
                case Enums.FunctionType.ToggleHook:
                case Enums.FunctionType.ButtonOnOffHook:
                case Enums.FunctionType.ToggleSeekBar:
                case Enums.FunctionType.ButtonOnOffSeekBar:
                    btnFunction.HighEmphasis = false;
                    EasyEnabled(btnFunction, false);
                    EasyEnabled(chkMultiple, false);
                    break;

                case Enums.FunctionType.Button:
                case Enums.FunctionType.Category:
                    EasyEnabled(groupOffsets, false);
                    break;
            }

            EasyEnabled(groupFunction);
            EasyEnabled(comboFunction, false);
            if (functionType != Enums.FunctionType.Button)
                txtOffset.Clear();

            WriteOutput($"[Success] Added Offset - Offset ID: {offset.OffsetId}, Offset: {offset.Offset}, Hex: {offset.Hex}", Color.Green);
        }

        private void btnFunction_Click(object sender, EventArgs e)
        {
            Hide();
            switch ((Enums.FunctionType)comboFunction.SelectedIndex)
            {
                case Enums.FunctionType.ToggleHook:
                case Enums.FunctionType.ButtonOnOffHook:
                    var frmToggleHook = new FrmToggleHook();
                    frmToggleHook.ShowDialog();
                    frmToggleHook.Dispose();
                    break;

                case Enums.FunctionType.ToggleSeekBar:
                case Enums.FunctionType.ButtonOnOffSeekBar:
                    var frmSeekBar = new FrmSeekBar();
                    frmSeekBar.ShowDialog();
                    frmSeekBar.Dispose();
                    break;

                case Enums.FunctionType.ToggleInputValue:
                case Enums.FunctionType.ButtonOnOffInputValue:
                    var frmInputValue = new FrmSeekBar(1);
                    frmInputValue.ShowDialog();
                    frmInputValue.Dispose();
                    break;

                case Enums.FunctionType.Button:
                    var frmButton = new FrmButton();
                    frmButton.ShowDialog();
                    frmButton.Dispose();
                    break;

                case Enums.FunctionType.Category:
                    var frmCategory = new FrmCategory();
                    frmCategory.ShowDialog();
                    frmCategory.Dispose();
                    break;
            }
            Show();
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
            var hookInfo = HookValue();

            OffsetPatch.AddFunction(txtNameCheat.Text, OffsetPatch.OffsetList, functionType, functionValue, chkMultiple.Checked, hookInfo);
            ClearValue();
            _offsetCount = 1;

            BtnFunctionManager();
            EasyEnabled(comboFunction);
            EasyEnabled(btnAddOffset);
            EasyEnabled(chkDup);
            EasyEnabled(groupOffsets);
            EasyEnabled(groupFunction, false);
            AddListView();
            txtNameCheat.Clear();
        }

        private static HookInfo HookValue()
        {
            return new HookInfo
            {
                Field = Values.Field,
                Type = Values.Type,
                Offset = Values.Offset,
                Method = Values.Method
            };
        }

        private string FunctionValue()
        {
            switch ((Enums.FunctionType)comboFunction.SelectedIndex)
            {
                case Enums.FunctionType.Category:
                    return Values.Category;

                case Enums.FunctionType.ToggleSeekBar:
                case Enums.FunctionType.ButtonOnOffSeekBar:
                    return Values.SeekBar;

                default:
                    return "";
            }
        }

        private static void ClearValue()
        {
            Values.Category = null;
            Values.SeekBar = null;
            Values.Offset = null;
            Values.Field = false;
            Values.Type = Enums.Type.Empty;
            Values.Method = null;
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

            var field = OffsetPatch.FunctionList[OffsetPatch.FunctionList.Count - 1].HookInfo.Field.ToString();
            var fieldOffset = OffsetPatch.FunctionList[OffsetPatch.FunctionList.Count - 1].HookInfo.Offset;
            var type = OffsetPatch.FunctionList[OffsetPatch.FunctionList.Count - 1].HookInfo.Type.ToString();

            AddListValues(function, offset, hex, cheatName[cheatName.Count - 1], offsetCount, multiple, field, value, fieldOffset, type);

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
                var offset = offsetCount > 1 ? "Multiple Patch Offset" : offsetList[0].Offset;
                var hex = offsetCount > 1 ? "Multiple Patch Hex Code" : offsetList[0].Hex;

                var multiple = OffsetPatch.FunctionList[i].MultipleValue ? "Yes" : "No";

                var field = OffsetPatch.FunctionList[i].HookInfo.Field.ToString();
                var fieldOffset = OffsetPatch.FunctionList[i].HookInfo.Offset;
                var type = OffsetPatch.FunctionList[i].HookInfo.Type.ToString();

                AddListValues(function, offset, hex, cheatName[i], offsetCount, multiple, field, value, fieldOffset, type);
                OffsetPatch.OffsetList.Clear();
            }
        }

        private void AddListValues(string function, string offset, string hex, string cheatName, int offsetCount, string multiple, string field,
              string value = "", string fieldOffset = "", string type = "")
        {
            var items = new ListViewItem(cheatName);
            items.SubItems.Add(offset);
            items.SubItems.Add(hex);
            items.SubItems.Add(function);
            items.SubItems.Add(value);
            items.SubItems.Add(offsetCount.ToString());
            items.SubItems.Add(multiple);
            items.SubItems.Add(field);
            items.SubItems.Add(fieldOffset);
            items.SubItems.Add(type);
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
            if (Utility.IsEmpty(fileName, false)) { return default; }

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
            if (Utility.IsEmpty(txtLibName, false))
            {
                MyMessage.MsgShowWarning(@"Library Name is Empty, Please Check it again!!!");
                WriteOutput("[Warning] Library Name is Empty", Color.Orange);
                return;
            }
            if (Utility.IsEmpty(txtNDK, false))
            {
                MyMessage.MsgShowWarning(@"NDK Path is Empty, Please Check it again!!!");
                WriteOutput("[Warning] NDK Path is Empty", Color.Orange);
                return;
            }
            if (Utility.IsEmpty(ImageCode, false))
            {
                MyMessage.MsgShowWarning(@"Image Code is Empty, Please Check it again!!!");
                WriteOutput("[Warning] Image Code is Empty", Color.Orange);
                return;
            }
            if (Utility.IsEmpty(txtNameGame, false))
            {
                MyMessage.MsgShowWarning(@"Name Game is Empty, Please Check it again!!!");
                WriteOutput("[Warning] Name Game is Empty", Color.Orange);
                return;
            }
            if (Utility.IsEmpty(txtTargetLib, false))
            {
                MyMessage.MsgShowWarning(@"Target Library Name is Empty, Please Check it again!!!");
                WriteOutput("[Warning] Target Library Name is Empty", Color.Orange);
                return;
            }
            if (OffsetPatch.FunctionList.Count == 0)
            {
                MyMessage.MsgShowWarning(@"Function list is Empty, Please Check it again!!!");
                WriteOutput("[Warning] Function list is Empty", Color.Orange);
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
            if (Debug)
            {
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
                    Replace("(yourImage)", ImageCode);
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
                var functionExtra = !Utility.IsEmpty(functionList[i].FunctionValue, false)
                    ? "_" + functionList[i].FunctionValue
                    : "";
                switch (type)
                {
                    case Enums.FunctionType.ToggleHook:
                        result += $@"{Environment.NewLine}            OBFUSCATE(""{num}_Toggle_{cheatName}""),";
                        break;

                    case Enums.FunctionType.ToggleSeekBar:
                        result += $@"{Environment.NewLine}            OBFUSCATE(""{num}_Toggle_{cheatName}""),";
                        realCount++;
                        num = (realCount + 1).ToString();
                        result += $@"{Environment.NewLine}            OBFUSCATE(""{num}_SeekBar_{cheatName}{functionExtra}""),";
                        break;

                    case Enums.FunctionType.ToggleInputValue:
                        result += $@"{Environment.NewLine}            OBFUSCATE(""{num}_InputValue_{cheatName}""),";
                        realCount++;
                        num = (realCount + 1).ToString();
                        result += $@"{Environment.NewLine}            OBFUSCATE(""{num}_Toggle_{cheatName}""),";
                        break;

                    case Enums.FunctionType.ButtonOnOffHook:
                        result += $@"{Environment.NewLine}            OBFUSCATE(""{num}_ButtonOnOff_{cheatName}""),";
                        break;

                    case Enums.FunctionType.ButtonOnOffSeekBar:
                        result += $@"{Environment.NewLine}            OBFUSCATE(""{num}_ButtonOnOff_{cheatName}""),";
                        realCount++;
                        num = (realCount + 1).ToString();
                        result += $@"{Environment.NewLine}            OBFUSCATE(""{num}_SeekBar_{cheatName}{functionExtra}""),";
                        break;

                    case Enums.FunctionType.ButtonOnOffInputValue:
                        result += $@"{Environment.NewLine}            OBFUSCATE(""{num}_InputValue_{cheatName}""),";
                        realCount++;
                        num = (realCount + 1).ToString();
                        result += $@"{Environment.NewLine}            OBFUSCATE(""{num}_ButtonOnOff_{cheatName}""),";
                        break;

                    case Enums.FunctionType.Button:
                        var buttonType = GetButtonExtra(functionList[i].HookInfo.Method);
                        var buttonExtra = GetButtonValue(functionList[i].HookInfo.Method);
                        if (buttonType == 2)
                        {
                            result += $@"{Environment.NewLine}            OBFUSCATE(""{num}_InputValue_{cheatName}""),";
                            realCount++;
                            num = (realCount + 1).ToString();
                        }
                        result += $@"{Environment.NewLine}            OBFUSCATE(""{num}_Button_{cheatName}""),";
                        if (buttonType == 1)
                        {
                            realCount++;
                            num = (realCount + 1).ToString();
                            result += $@"{Environment.NewLine}            OBFUSCATE(""{num}_SeekBar_{cheatName}{buttonExtra}""),";
                        }
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
                var cheatName = list.CheatName.RemoveSuperSpecialCharacters().ReplaceNumCharacters();
                switch (list.FunctionType)
                {
                    case Enums.FunctionType.Toggle:
                    case Enums.FunctionType.ToggleHook:
                    case Enums.FunctionType.ButtonOnOff:
                    case Enums.FunctionType.ButtonOnOffHook:
                        result += $"bool _{cheatName} = false;{Environment.NewLine}";
                        break;

                    case Enums.FunctionType.ToggleSeekBar:
                    case Enums.FunctionType.ToggleInputValue:
                    case Enums.FunctionType.ButtonOnOffSeekBar:
                    case Enums.FunctionType.ButtonOnOffInputValue:
                        result += $"bool _{cheatName} = false;{Environment.NewLine}";
                        result += $"int _{cheatName}Value = 1;{Environment.NewLine}";
                        break;

                    case Enums.FunctionType.Button:
                        var buttonType = GetButtonExtra(list.HookInfo.Method);
                        result += $"void *btn{cheatName};{Environment.NewLine}";
                        if (buttonType == 1 || buttonType == 2)
                        {
                            result += $"int _{cheatName}Value = 1;{Environment.NewLine}";
                        }
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
                var cheatName = list.CheatName.RemoveSuperSpecialCharacters().ReplaceNumCharacters();
                var fieldMultiple = "";
                switch (list.FunctionType)
                {
                    case Enums.FunctionType.ToggleSeekBar:
                    case Enums.FunctionType.ToggleInputValue:
                    case Enums.FunctionType.ButtonOnOffSeekBar:
                    case Enums.FunctionType.ButtonOnOffInputValue:
                        {
                            var multiple = list.MultipleValue ? $"return _{cheatName}Value*old_{cheatName}(instance);" : $"return _{cheatName}Value;";
                            if (list.HookInfo.Field)
                            {
                                var fieldValues = list.HookInfo.Offset.RemoveMiniSpecialCharacters().Split(',');
                                fieldMultiple = fieldValues.Aggregate(fieldMultiple, (current, t) => current + (list.MultipleValue
                                    ? $"*({Type2String(list.HookInfo.Type)} *) ((uint64_t) instance + {t}) = _{cheatName}Value*old_{cheatName}(instance);{Environment.NewLine}        "
                                    : $"*({Type2String(list.HookInfo.Type)} *) ((uint64_t) instance + {t}) = _{cheatName}Value;{Environment.NewLine}        "));

                                result += $@"int (*old_{cheatName})(void *instance);
int Update{cheatName}(void *instance) {{
    if (instance != NULL && _{cheatName} && _{cheatName}Value > 1) {{
        {fieldMultiple}
    }}
    return old_{cheatName}(instance);
}}
";
                            }
                            else
                            {
                                result += $@"int (*old_{cheatName})(void *instance);
int Update{cheatName}(void *instance) {{
    if (instance != NULL && _{cheatName} && _{cheatName}Value > 1) {{
        {multiple}
    }}
    return old_{cheatName}(instance);
}}
";
                            }

                            break;
                        }
                    case Enums.FunctionType.ToggleHook:
                    case Enums.FunctionType.ButtonOnOffHook:
                        {
                            if (list.HookInfo.Field)
                            {
                                fieldMultiple = list.HookInfo.Offset.RemoveMiniSpecialCharacters().Split(',').Aggregate(fieldMultiple, (current, value) => current + $"*(bool *) ((uint64_t) instance + {value}) = _{cheatName};{Environment.NewLine}        ");

                                result += $@"void (*old_{cheatName})(void *instance);
void Update{cheatName}(void *instance) {{
    if (instance != NULL && _{cheatName}) {{
        {fieldMultiple}
    }}
    return old_{cheatName}(instance);
}}
";
                            }
                            else
                            {
                                result += $@"bool (*old_{cheatName})(void *instance);
bool Update{cheatName}(void *instance) {{
    if (instance != NULL && _{cheatName}) {{
        return _{cheatName};
    }}
    return old_{cheatName}(instance);
}}
";
                            }

                            break;
                        }
                    case Enums.FunctionType.Button:
                        {
                            var args = GetButtonArgs(list.HookInfo.Method);
                            result += $@"void (*{cheatName}Method)(void *instance{args});
";
                            result += $@"void (*old_{cheatName})(void *instance);
void Update{cheatName}(void *instance) {{
        btn{cheatName} = instance;
        old_{cheatName}(instance);
}}
";
                            break;
                        }
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
                        {
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
                        }
                    case Enums.FunctionType.ButtonOnOffHook:
                    case Enums.FunctionType.ToggleHook:
                        {
                            result += $@"
                        case {num}:
                            _{cheatName} = boolean;
                            break;{Environment.NewLine}";
                            break;
                        }
                    case Enums.FunctionType.ToggleSeekBar:
                    case Enums.FunctionType.ButtonOnOffSeekBar:
                        {
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
                    case Enums.FunctionType.ButtonOnOffInputValue:
                    case Enums.FunctionType.ToggleInputValue:
                        {
                            result += $@"
                        case {num}:
                            if (value >= 1) {{
                                _{cheatName}Value = value;
                            }}
                            break;{Environment.NewLine}";

                            realCount++;
                            num = (realCount + 1).ToString();
                            result += $@"
                        case {num}:
                            _{cheatName} = boolean;
                            break;{Environment.NewLine}";
                            break;
                        }
                    case Enums.FunctionType.Button:
                        {
                            var buttonType = GetButtonExtra(functionList[i].HookInfo.Method);
                            var args = GetButtonArgsValues(functionList[i].HookInfo.Method, $"_{cheatName}Value");
                            if (buttonType == 2)
                            {
                                result += $@"
                        case {num}:
                            if (value >= 1) {{
                                _{cheatName}Value = value;
                            }}
                            break;{Environment.NewLine}";

                                realCount++;
                                num = (realCount + 1).ToString();
                            }

                            result += $@"
                        case {num}:
                            if (btn{cheatName} != NULL) {{
                                {cheatName}Method(btn{cheatName}{args});
                            }}
                            break;{Environment.NewLine}";

                            if (buttonType == 1)
                            {
                                realCount++;
                                num = (realCount + 1).ToString();
                                result += $@"
                        case {num}:
                            if (value >= 1) {{
                                _{cheatName}Value = value;
                            }}
                            break;{Environment.NewLine}";
                            }

                            break;
                        }
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
            var abiType = comboType.SelectedIndex == (int)Enums.TypeAbi.Arm64 ? "A64HookFunction" : "MSHookFunction";
            foreach (var list in OffsetPatch.FunctionList)
            {
                var cheatName = list.CheatName.RemoveSuperSpecialCharacters().ReplaceNumCharacters();

                switch (list.FunctionType)
                {
                    case Enums.FunctionType.Toggle:
                    case Enums.FunctionType.ButtonOnOff:
                        {
                            result = list.OffsetList.Aggregate(result, (current, info) => current +
                                $@"hexPatches.{cheatName}_{info.OffsetId} = MemoryPatch::createWithHex(""{txtTargetLib.Text}"",
                                        string2Offset(OBFUSCATE_KEY(""{info.Offset}"", 't')),
                                        OBFUSCATE(""{info.Hex}""));{Environment.NewLine}    ");
                            continue;
                        }
                    case Enums.FunctionType.ToggleHook:
                    case Enums.FunctionType.ButtonOnOffHook:
                        {
                            result = list.OffsetList.Aggregate(result, (current, info) => current +
                                $@"{abiType}((void *) getAbsoluteAddress(targetLibName, string2Offset(OBFUSCATE_KEY(""{info.Offset}"", 't'))),
                            (void *) Update{cheatName}, (void **) &old_{cheatName});{Environment.NewLine}    ");
                            continue;
                        }
                    case Enums.FunctionType.ToggleSeekBar:
                    case Enums.FunctionType.ToggleInputValue:
                    case Enums.FunctionType.ButtonOnOffSeekBar:
                    case Enums.FunctionType.ButtonOnOffInputValue:
                        {
                            result = list.OffsetList.Aggregate(result, (current, info) => current +
                                $@"{abiType}((void *) getAbsoluteAddress(targetLibName, string2Offset(OBFUSCATE_KEY(""{info.Offset}"", 't'))),
                            (void *) Update{cheatName}, (void **) &old_{cheatName});{Environment.NewLine}    ");
                            continue;
                        }
                    case Enums.FunctionType.Button:
                        {
                            var args = GetButtonType(list.HookInfo.Method);
                            result += $"{cheatName}Method = (void(*)(void *{args}))getAbsoluteAddress(targetLibName, {list.HookInfo.Offset});{Environment.NewLine}";
                            result = list.OffsetList.Aggregate(result, (current, info) => current +
                                $@"    {abiType}((void *) getAbsoluteAddress(targetLibName, string2Offset(OBFUSCATE_KEY(""{info.Offset}"", 't'))),
                            (void *) Update{cheatName}, (void **) &old_{cheatName});{Environment.NewLine}    ");
                            continue;
                        }
                    case Enums.FunctionType.Patch:
                        {
                            result = list.OffsetList.Aggregate(result, (current, info) => current +
                                $@"hexPatches.{cheatName}_{info.OffsetId} = MemoryPatch::createWithHex(""{txtTargetLib.Text}"",
                                        string2Offset(OBFUSCATE_KEY(""{info.Offset}"", 't')),
                                        OBFUSCATE(""{info.Hex}""));
    hexPatches.{cheatName}_{info.OffsetId}.Modify();{Environment.NewLine}    ");
                            continue;
                        }
                }
            }

            return result;
        }

        #endregion Main.cpp

        private static int GetButtonExtra(List<(string, string)> method)
        {
            for (var i = 0; i < method.Count; i++)
            {
                if (method[i].Item2.IndexOf("seekbar", StringComparison.OrdinalIgnoreCase) >= 0)
                    return 1;
                if (method[i].Item2.IndexOf("inputvalue", StringComparison.OrdinalIgnoreCase) >= 0)
                    return 2;
            }

            return 0;
        }

        private static string GetButtonValue(List<(string, string)> method)
        {
            for (var i = 0; i < method.Count; i++)
            {
                if (method[i].Item2.IndexOf("seekbar", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return method[i].Item2.Replace("seekbar_", "");
                }
            }
            return null;
        }

        private static string GetButtonArgs(List<(string, string)> method)
        {
            var result = "";
            for (var i = 0; i < method.Count; i++)
            {
                result += $", {method[i].Item1} _{method[i].Item1}{i}";
            }
            return result;
        }

        private static string GetButtonType(List<(string, string)> method)
        {
            var result = "";
            for (var i = 0; i < method.Count; i++)
            {
                result += $", {method[i].Item1}";
            }
            return result;
        }

        private static string GetButtonArgsValues(List<(string, string)> method, string value)
        {
            var result = "";
            for (var i = 0; i < method.Count; i++)
            {
                if (method[i].Item2.IndexOf("seekbar", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    result += $", {value}";
                }
                else if (method[i].Item2.IndexOf("inputvalue", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    result += $", {value}";
                }
                else
                {
                    result += $", {method[i].Item2}";
                }
            }
            return result;
        }

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
                        Arguments = $"/c {_mySettings.txtNDK}\\build\\ndk-build",
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
                process.Close();
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
            if (Utility.IsEmpty(e.Data, false)) return;
            WriteOutput("[Compile] " + e.Data);
        }

        private void ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (Utility.IsEmpty(e.Data, false)) return;
            if (e.Data == "fcntl(): Bad file descriptor") return;
            _compile++;
            WriteOutput("[Compile] " + e.Data, Color.Red);
        }

        private void compilerWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (_compile > 0 && !_mySettings.debugMode)
            {
                MyMessage.MsgShowError("Failed to Compile");
                WriteOutput("[Error:020] Failed to Compile", Color.Red);
                SaveLogs();
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

        #endregion Menu Page

        #region Log&About Page

        private void btnOutput_Click(object sender, EventArgs e)
        {
            Process.Start(AppPath + "\\Output");
        }

        private void btnTempDir_Click(object sender, EventArgs e)
        {
            Process.Start(_tempPathMenu);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            rbLog.Clear();
        }

        private void btnSaveLog_Click(object sender, EventArgs e)
        {
            SaveLogs();
        }

        private void SaveLogs()
        {
            var date = DateTime.Now.ToString("yyyy-M-d HH-mm-ss");
            var path = $"{AppPath}\\Logs\\{date}.txt";
            File.WriteAllText(path, rbLog.Text);
            WriteOutput($"[Logs] Log saved successfully. {path}", Color.Gold);
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
            if (!chkLogsComplie.Checked && str.Contains("[Compile]"))
            {
                return;
            }
            if (!chkLogsSuccess.Checked && str.Contains("[Success]"))
            {
                return;
            }
            if (!chkLogsError.Checked && str.Contains("[Error:"))
            {
                return;
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
            _mySettings.txtService = txtService.Text;
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
            _mySettings.txtOnCreate = txtOnCreate.Text;
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
            _mySettings.txtActionMain = txtActionMain.Text;
        }

        #endregion Method 2

        private void CopyText(string str)
        {
            Clipboard.SetText(str);
            if (!_mySettings.chkSound) return;
            System.Media.SystemSounds.Beep.Play();
        }

        #endregion Dev Page

        #region Utility

        private static string Type2String(Enums.Type type)
        {
            switch (type)
            {
                case Enums.Type.Bool:
                    return "bool";

                case Enums.Type.Int:
                    return "int";

                case Enums.Type.Long:
                    return "long";

                case Enums.Type.Float:
                    return "float";

                case Enums.Type.Double:
                    return "double";

                default:
                    return "int";
            }
        }

        // https://stackoverflow.com/a/45673201/8902883
        private static string ImageToBase64(Image image, ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, format);
                var imageBytes = ms.ToArray();
                return Convert.ToBase64String(imageBytes);
            }
        }

        private static Image Base64ToImage(string base64String)
        {
            var imageBytes = Convert.FromBase64String(base64String);
            using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                ms.Write(imageBytes, 0, imageBytes.Length);
                return Image.FromStream(ms, true);
            }
        }

        // https://stackoverflow.com/a/24651073/8902883
        private static Image CompressImage(string fileName, int newQuality, ImageFormat imgFormat)
        {
            using (var image = Image.FromFile(fileName))
            using (Image memImage = new Bitmap(image))
            {
                var type = "";

                if (Equals(imgFormat, ImageFormat.Png))
                {
                    type = "image/png";
                }
                else if (Equals(imgFormat, ImageFormat.Jpeg))
                {
                    type = "image/jpeg";
                }
                else if (Equals(imgFormat, ImageFormat.Bmp))
                {
                    type = "image/bmp";
                }
                else if (Equals(imgFormat, ImageFormat.Gif))
                {
                    type = "image/gif";
                }
                var myImageCodecInfo = GetEncoderInfo(type);
                var myEncoder = Encoder.Quality;
                var myEncoderParameters = new EncoderParameters(1)
                {
                    Param = {[0] = new EncoderParameter(myEncoder, newQuality)}
                };

                var memStream = new MemoryStream();
                memImage.Save(memStream, myImageCodecInfo, myEncoderParameters);
                var newImage = Image.FromStream(memStream);
                var imageAttributes = new ImageAttributes();
                using (var g = Graphics.FromImage(newImage))
                {
                    g.InterpolationMode =
                        System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;  //**
                    g.DrawImage(newImage, new Rectangle(Point.Empty, newImage.Size), 0, 0,
                        newImage.Width, newImage.Height, GraphicsUnit.Pixel, imageAttributes);
                }
                return newImage;
            }
        }

        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            var encoders = ImageCodecInfo.GetImageEncoders();
            return encoders.FirstOrDefault(ici => ici.MimeType == mimeType);
        }

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
                WriteOutput("[Success] Deleted " + path, Color.Green);
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
                foreach (var file in dir.GetFiles())
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
                if (!_mySettings.chkSound) return;
                System.Media.SystemSounds.Beep.Play();
            }
        }

        private static void EnableController(Form form, bool value)
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

        private static void EnableController(Control control, bool value)
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

        private static void EasyEnabled(Control control, bool status = true)
        {
            control.Enabled = status;
        }

        private static bool IsWindows7 => OS_Name().Contains("Windows 7");

        private static bool IsWindows10 => OS_Name().Contains("Windows 10");

        private static bool Is64Bit => Environment.Is64BitOperatingSystem;

        private static string OS_Name()
        {
            return (string)(from x in new ManagementObjectSearcher(
                    "SELECT Caption FROM Win32_OperatingSystem").Get().Cast<ManagementObject>()
                            select x.GetPropertyValue("Caption")).FirstOrDefault();
        }

        #endregion Utility
    }
}